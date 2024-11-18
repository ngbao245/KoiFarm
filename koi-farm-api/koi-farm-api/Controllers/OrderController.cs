using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data.Entity;
using Repository.EmailService;
using Repository.Model;
using Repository.Model.Email;
using Repository.Model.Order;
using Repository.Repository;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace koi_farm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public OrderController(UnitOfWork unitOfWork, IServiceProvider serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _emailService = serviceProvider.GetRequiredService<IEmailService>();
        }

        private string GetUserIdFromClaims()
        {
            return User.FindFirst("UserID")?.Value;
        }

        [HttpPost("create")]
        public IActionResult CreateOrder([FromBody] CreateOrderRequestModel model)
        {
            var cart = _unitOfWork.CartRepository.GetSingle(c => c.Id == model.CartId, c => c.Items);
            if (cart == null || !cart.Items.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "There is no Cart found or the cart is empty."
                });
            }

            //Check if there is a valid promotion
            var promotion = _unitOfWork.PromotionRepository.GetAll().FirstOrDefault(p => p.Code == model.PromotionCode);
            bool isPromotionValid = promotion != null &&
                                    ((promotion.Type == "Percentage" && promotion.Amount > 0 && promotion.Amount <= 100) ||
                                     (promotion.Type == "Direct" && promotion.Amount > 0));

            var order = new Order
            {
                UserId = GetUserIdFromClaims(),
                Total = 0,
                Status = "Pending",
                Items = new List<OrderItem>(),

                PromotionId = isPromotionValid ? promotion.Id : null
            };

            foreach (var cartItem in cart.Items)
            {
                var productItem = _unitOfWork.ProductItemRepository.GetById(cartItem.ProductItemId);
                if (productItem == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        MessageError = $"Product item with ID {cartItem.ProductItemId} not found."
                    });
                }

                if (cartItem.Quantity > productItem.Quantity)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        MessageError = $"Requested quantity for {productItem.Name} exceeds available stock."
                    });
                }

                var orderItem = new OrderItem
                {
                    ProductItemId = cartItem.ProductItemId,
                    Quantity = cartItem.Quantity
                };

                order.Items.Add(orderItem);
                order.Total += orderItem.Quantity * productItem.Price;

                // Reduce ProductItem quantity
                productItem.Quantity -= orderItem.Quantity;
                _unitOfWork.ProductItemRepository.Update(productItem);

                // Reduce Product quantity
                var product = _unitOfWork.ProductRepository.GetById(productItem.ProductId);
                if (product != null)
                {
                    product.Quantity -= orderItem.Quantity;
                    _unitOfWork.ProductRepository.Update(product);
                }
                else
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        MessageError = $"Product not found for ProductItem with ID {productItem.Id}."
                    });
                }
            }

            if (isPromotionValid)
            {
                if (promotion.Type == "Percentage")
                {
                    order.Total -= order.Total * ((decimal)promotion.Amount / 100);
                }
                else if (promotion.Type == "Direct")
                {
                    order.Total -= promotion.Amount;
                }
            }

            string address = _unitOfWork.UserRepository.GetById(GetUserIdFromClaims()).Address;
            order.Address = address;

            _unitOfWork.OrderRepository.Create(order);

            foreach (var cartItem in cart.Items.ToList())
            {
                _unitOfWork.CartItemRepository.Delete(cartItem);
            }

            _unitOfWork.CartRepository.Delete(cart);

            try
            {
                _unitOfWork.SaveChange();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    MessageError = "An error occurred while processing your order. Please try again later."
                });
            }

            //Send mail with certificate
            SendOrderConfirmationEmailWithCertificates(order);

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = new OrderResponseModel
                {
                    OrderId = order.Id,
                    Total = order.Total,
                    Status = order.Status,
                    UserId = order.UserId,
                    StaffId = order.StaffId,
                    Address = order.Address,
                    CreatedTime = order.CreatedTime,
                    Items = order.Items.Select(item => new OrderItemResponseModel
                    {
                        ProductItemId = item.ProductItemId,
                        Quantity = item.Quantity,
                        Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price,
                        BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                    }).ToList()
                }
            });
        }

        private void SendOrderConfirmationEmailWithCertificates(Order order)
        {
            var user = _unitOfWork.UserRepository.GetById(order.UserId);
            if (user == null) return;

            var certificateImages = new List<string>();

            foreach (var orderItem in order.Items)
            {
                var productCertificates = _unitOfWork.ProductCertificateRepository
                 .Get()
                 .Include(pc => pc.certificate)
                 .Where(pc => pc.ProductItemId == orderItem.ProductItemId)
                 .Select(pc => pc.certificate.ImageUrl)
                 .ToList();

                certificateImages.AddRange(productCertificates);
            }

            var emailContent = new StringBuilder();
            emailContent.AppendLine($"<p>Kính chào {user.Name},</p>");
            emailContent.AppendLine($"<p>Đơn hàng ID: <strong>{order.Id}</strong> của quý khách đã được tạo thành công.<p>");

            if (certificateImages.Any())
            {
                emailContent.AppendLine("<p>Hình ảnh giấy chứng nhận cho các sản phẩm quý khách đã mua:</p>");
                foreach (var imageUrl in certificateImages)
                {
                    emailContent.AppendLine($"<p><img src=\"{imageUrl}\" style=\"max-width:200px;\"></p>");
                }
            }
            else
            {
                emailContent.AppendLine("<p>Hiện tại chưa có giấy chứng nhận cho sản phẩm quý khách đã mua, mong quý khách thông cảm:</p>");
            }

            emailContent.AppendLine("<br>");
            emailContent.AppendLine("<p>Trân trọng,</p>");
            emailContent.AppendLine("<p>KoiShop</p>");


            var emailModel = new SendMailModel
            {
                ReceiveAddress = user.Email,
                Title = "Xác Nhận Các Chứng Chỉ Cho Cá Koi Đã Mua",
                Content = emailContent.ToString()
            };

            _emailService.SendMail(emailModel);
        }


        // Get Order by ID Endpoint
        [HttpGet("{orderId}")]
        public IActionResult GetOrderById(string orderId)
        {
            var order = _unitOfWork.OrderRepository.GetSingle(o => o.Id == orderId, o => o.Items);
            if (order == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Order not found."
                });
            }

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = new OrderResponseModel
                {
                    OrderId = order.Id,
                    Total = order.Total,
                    Status = order.Status,
                    UserId = order.UserId,
                    StaffId = order.StaffId,
                    Address = order.Address,
                    ConsignmentId = order.ConsignmentId,
                    CreatedTime = order.CreatedTime,
                    IsDelivered = order.IsDelivered,
                    Items = order.Items.Select(item => new OrderItemResponseModel
                    {
                        ProductItemId = item.ProductItemId,
                        Quantity = item.Quantity,
                        Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId)?.Price ?? 25000,
                        BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                    }).ToList()
                }
            });
        }

        // Get All Orders for a User Endpoint
        [HttpGet("user")]
        public IActionResult GetAllOrdersForUser()
        {
            var userId = GetUserIdFromClaims();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized. User ID not found in claims."
                });
            }

            var orders = _unitOfWork.OrderRepository.Get(o => o.UserId == userId, o => o.Items).ToList();
            if (!orders.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No orders found for the user."
                });
            }

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = orders.Select(order => new OrderResponseModel
                {
                    OrderId = order.Id,
                    Total = order.Total,
                    Status = order.Status,
                    UserId = order.UserId,
                    StaffId = order.StaffId,
                    Address = order.Address,
                    ConsignmentId = order.ConsignmentId,
                    CreatedTime = order.CreatedTime,
                    IsDelivered = order.IsDelivered,
                    Items = order.Items.Select(item => new OrderItemResponseModel
                    {
                        ProductItemId = item.ProductItemId,
                        Quantity = item.Quantity,
                        Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price,
                        BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                    }).ToList()
                }).ToList()
            });
        }

        // Update Order Status Endpoint
        [HttpPut("update-order-status/{orderId}")]
        public IActionResult UpdateOrderStatus(string orderId, [FromBody] RequestUpdateStatusModel model)
        {
            var userId = GetUserIdFromClaims();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized. User ID not found in claims."
                });
            }
            var order = _unitOfWork.OrderRepository.GetSingle(o => o.Id == orderId, o => o.Items);
            if (order == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Order not found."
                });
            }

            var validStatuses = new[] { "Pending", "Delivering", "Completed", "Cancelled" };
            if (!validStatuses.Contains(model.Status))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid order status."
                });
            }

            order.Status = model.Status;
            _unitOfWork.OrderRepository.Update(order);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = new OrderResponseModel
                {
                    OrderId = order.Id,
                    Total = order.Total,
                    Status = order.Status,
                    UserId = order.UserId,
                    StaffId = order.StaffId,
                    Address = order.Address,
                    CreatedTime = order.CreatedTime,
                    IsDelivered = order.IsDelivered,
                    Items = order.Items.Select(item => new OrderItemResponseModel
                    {
                        ProductItemId = item.ProductItemId,
                        Quantity = item.Quantity,
                        Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price,
                        BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                    }).ToList()
                }
            });
        }

        // Get All Orders Endpoint
        [HttpGet("get-all-orders")]
        public IActionResult GetAllOrders()
        {
            try
            {
                var orders = _unitOfWork.OrderRepository.Get(includeProperties: o => o.Items).ToList();
                if (!orders.Any())
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        MessageError = "No orders found."
                    });
                }

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Data = orders.Select(order => new OrderResponseModel
                    {
                        OrderId = order.Id,
                        Total = order.Total,
                        Status = order.Status,
                        UserId = order.UserId,
                        StaffId = order.StaffId,
                        Address = order.Address,
                        ConsignmentId = order.ConsignmentId,
                        CreatedTime = order.CreatedTime,
                        IsDelivered = order.IsDelivered,
                        Items = order.Items.Select(item => new OrderItemResponseModel
                        {
                            ProductItemId = item.ProductItemId,
                            Quantity = item.Quantity,
                            Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId)?.Price ?? 0,
                            BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                        }).ToList()
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetAllOrders: {ex.Message}");
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    MessageError = "An error occurred while retrieving orders."
                });
            }
        }


        // Get Orders by Status Endpoint
        [HttpGet("get-orders-by-status/{status}")]
        public IActionResult GetOrdersByStatus(string status)
        {
            var validStatuses = new[] { "Pending", "Delivering", "Completed", "Cancelled" };
            if (!validStatuses.Contains(status))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid order status."
                });
            }

            var orders = _unitOfWork.OrderRepository.Get(o => o.Status == status, o => o.Items).ToList();
            if (!orders.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = $"No orders found with status '{status}'."
                });
            }

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = orders.Select(order => new OrderResponseModel
                {
                    OrderId = order.Id,
                    Total = order.Total,
                    Status = order.Status,
                    UserId = order.UserId,
                    StaffId = order.StaffId,
                    Address = order.Address,
                    ConsignmentId = order.ConsignmentId,
                    CreatedTime = order.CreatedTime,
                    IsDelivered = order.IsDelivered,
                    Items = order.Items.Select(item => new OrderItemResponseModel
                    {
                        ProductItemId = item.ProductItemId,
                        Quantity = item.Quantity,
                        Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price,
                        BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                    }).ToList()
                }).ToList()
            });
        }

        // Get Orders by Status for Current User Endpoint
        [HttpGet("user/get-orders-by-status/{status}")]
        public IActionResult GetOrdersByStatusOfUser(string status)
        {
            var validStatuses = new[] { "Pending", "Delivering", "Completed", "Cancelled" };

            if (!validStatuses.Contains(status))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid order status."
                });
            }

            var userId = GetUserIdFromClaims();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized. User ID not found in claims."
                });
            }

            var orders = _unitOfWork.OrderRepository.Get(o => o.Status == status && o.UserId == userId, o => o.Items).ToList();

            if (!orders.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = $"No orders found with status '{status}' for the current user."
                });
            }

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = orders.Select(order => new OrderResponseModel
                {
                    OrderId = order.Id,
                    Total = order.Total,
                    Status = order.Status,
                    UserId = order.UserId,
                    StaffId = order.StaffId,
                    Address = order.Address,
                    ConsignmentId = order.ConsignmentId,
                    CreatedTime = order.CreatedTime,
                    IsDelivered = order.IsDelivered,
                    Items = order.Items.Select(item => new OrderItemResponseModel
                    {
                        ProductItemId = item.ProductItemId,
                        Quantity = item.Quantity,
                        Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price,
                        BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                    }).ToList()
                }).ToList()
            });
        }

        // Cancel Order Endpoint
        [HttpPut("cancel-order/{orderId}")]
        public IActionResult CancelOrder(string orderId)
        {
            var order = _unitOfWork.OrderRepository.GetSingle(o => o.Id == orderId, o => o.Items);
            if (order == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Order not found."
                });
            }

            if (order.Status != "Pending" && order.Status != "Failed")
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Order can't be canceled."
                });
            }

            foreach (var orderItem in order.Items)
            {
                var productItem = _unitOfWork.ProductItemRepository.GetById(orderItem.ProductItemId);
                if (productItem != null)
                {
                    productItem.Quantity += orderItem.Quantity;
                    _unitOfWork.ProductItemRepository.Update(productItem);

                    var product = _unitOfWork.ProductRepository.GetById(productItem.ProductId);
                    if (product != null)
                    {
                        product.Quantity += orderItem.Quantity;
                        _unitOfWork.ProductRepository.Update(product);
                    }
                }
            }

            order.Status = "Cancelled";
            _unitOfWork.OrderRepository.Update(order);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = "Order successfully canceled. Quantities have been updated accordingly."
            });
        }

        // Assign Staff to Order Endpoint
        [HttpPut("order/assign-staff/{orderId}")]
        public IActionResult AssignStaffToOrder(string orderId, [FromBody] RequestAssginStaffModel model)
        {
            var order = _unitOfWork.OrderRepository.GetSingle(o => o.Id == orderId, o => o.Items);
            if (order == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Order not found."
                });
            }

            if (order.Status != "Pending")
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Order must be in Pending status."
                });
            }

            var staff = _unitOfWork.UserRepository.GetById(model.StaffId);
            if (staff == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = $"Staff with ID {model.StaffId} not found."
                });
            }

            if (staff.RoleId != "2")
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = $"{model.StaffId} is not a staff member."
                });
            }

            order.StaffId = model.StaffId;
            _unitOfWork.OrderRepository.Update(order);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = new OrderResponseModel
                {
                    OrderId = order.Id,
                    Total = order.Total,
                    Status = order.Status,
                    UserId = order.UserId,
                    StaffId = order.StaffId,
                    Address = order.Address,
                    CreatedTime = order.CreatedTime,
                    IsDelivered = order.IsDelivered,
                    Items = order.Items.Select(item => new OrderItemResponseModel
                    {
                        ProductItemId = item.ProductItemId,
                        Quantity = item.Quantity,
                        Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price,
                        BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                    }).ToList()
                }
            });
        }

        // Get Orders Assigned to Staff Endpoint
        [HttpGet("staff/get-assigned-orders")]
        public IActionResult GetOrdersAssignedToStaff()
        {
            var staffId = GetUserIdFromClaims();

            if (string.IsNullOrEmpty(staffId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized. Staff ID not found in claims."
                });
            }

            var orders = _unitOfWork.OrderRepository.Get(o => o.StaffId == staffId, o => o.Items).ToList();

            if (!orders.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No orders found for the assigned staff."
                });
            }

            var responseOrders = orders.Select(order => new OrderResponseModel
            {
                OrderId = order.Id,
                Total = order.Total,
                Status = order.Status,
                UserId = order.UserId,
                StaffId = order.StaffId,
                Address = order.Address,
                ConsignmentId = order.ConsignmentId,
                CreatedTime = order.CreatedTime,
                IsDelivered = order.IsDelivered,
                Items = order.Items.Select(item => new OrderItemResponseModel
                {
                    ProductItemId = item.ProductItemId,
                    Quantity = item.Quantity,
                    Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price,
                    BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                }).ToList()
            }).ToList();

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseOrders
            });
        }

        // Update IsDelivered Flag for Order
        [HttpPut("is-delivered/{orderId}")]
        public IActionResult UpdateIsDelivered(string orderId, [FromBody] RequestIsDeliveredModel model)
        {
            var order = _unitOfWork.OrderRepository.GetSingle(o => o.Id == orderId, o => o.Items);
            if (order == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Order not found."
                });
            }

            if (order.Status != "Completed")
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Order is not in Completed status."
                });
            }

            if (order.IsDelivered == true)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Order is already marked as delivered."
                });
            }

            if (model.IsDelivered == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "The isDelivered field is required."
                });
            }

            order.IsDelivered = model.IsDelivered;
            _unitOfWork.OrderRepository.Update(order);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = null,
                Data = new OrderResponseModel
                {
                    OrderId = order.Id,
                    Total = order.Total,
                    Status = order.Status,
                    UserId = order.UserId,
                    StaffId = order.StaffId,
                    Address = order.Address,
                    IsDelivered = order.IsDelivered,
                    Items = order.Items.Select(item => new OrderItemResponseModel
                    {
                        ProductItemId = item.ProductItemId,
                        Quantity = item.Quantity,
                        Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price,
                        BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                    }).ToList()
                }
            });
        }
    }
}
