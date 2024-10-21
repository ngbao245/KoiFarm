using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Model;
using Repository.Model.Consignment;
using Repository.Model.Order;
using Repository.Repository;
using System.Linq;

namespace koi_farm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConsignmentController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public ConsignmentController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst("UserID");
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in claims.");
            }
            return userIdClaim.Value;
        }

        // Create Consignment Item Endpoint
        [HttpPost("create")]
        public IActionResult CreateConsignmentItem([FromBody] CreateConsignmentItemRequestModel model)
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

            // Retrieve or create the consignment for the user
            var consignment = _unitOfWork.ConsignmentRepository.GetSingle(c => c.UserId == userId && !c.IsDeleted, c => c.Items);
            if (consignment == null)
            {
                consignment = new Consignment
                {
                    UserId = userId
                };
                _unitOfWork.ConsignmentRepository.Create(consignment);
            }

            // Create the new consignment item
            var consignmentItem = new ConsignmentItems
            {
                Name = model.Name,
                Category = model.Category,
                Origin = model.Origin,
                Sex = model.Sex,
                Age = model.Age,
                Size = model.Size,
                Species = model.Species,
                Status = "Pending",
                ConsignmentId = consignment.Id // Link to the user's consignment
            };

            consignment.Items.Add(consignmentItem);
            _unitOfWork.ConsignmentRepository.Update(consignment);

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = new ConsignmentResponseModel
                {
                    ConsignmentId = consignment.Id,
                    UserId = consignment.UserId,
                    Items = consignment.Items.Select(item => new ConsignmentItemResponseModel
                    {
                        ItemsId = item.Id,
                        Name = item.Name,
                        Category = item.Category,
                        Status = item.Status
                    }).ToList()
                }
            });
        }


        // Update Consignment Item Status
        [HttpPut("update-item-status/{itemId}")]
        public IActionResult UpdateConsignmentItemStatus(string itemId, [FromBody] UpdateConsignmentStatusRequestModel model)
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

            // Retrieve the consignment item and ensure it belongs to the current user's consignment
            var consignmentItem = _unitOfWork.ConsignmentItemRepository.GetSingle(ci => ci.Id == itemId && ci.Consignment.UserId == userId, ci => ci.Consignment);
            if (consignmentItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment item not found or does not belong to the current user."
                });
            }

            // Update the status
            consignmentItem.Status = model.Status;
            _unitOfWork.ConsignmentItemRepository.Update(consignmentItem);
            _unitOfWork.SaveChange();

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = "Consignment item status updated successfully."
            });
        }


        // Checkout Consignment
        [HttpPost("checkout-consignment/{consignmentId}")]
        public IActionResult CheckoutConsignment(string consignmentId)
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

            // Retrieve the consignment and ensure it belongs to the current user
            var consignment = _unitOfWork.ConsignmentRepository.GetSingle(c => c.Id == consignmentId && c.UserId == userId, c => c.Items);
            if (consignment == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment not found."
                });
            }

            // Only checkout approved items
            var approvedItems = consignment.Items.Where(i => i.Status == "Approved").ToList();
            if (!approvedItems.Any())
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "No approved consignment items available for checkout."
                });
            }

            // Calculate the total price based on the number of days
            var totalDays = (DateTimeOffset.Now - consignment.CreatedTime).Days;
            if (totalDays < 1) totalDays = 1; // Minimum charge for at least 1 day
            var totalPrice = approvedItems.Count * 25000 * totalDays;

            // Create the order
            var order = new Order
            {
                UserId = userId,
                Status = "Pending",
                Total = totalPrice,
                Items = new List<OrderItem>()
            };

            // Create OrderItems for each approved ConsignmentItem
            foreach (var consignmentItem in approvedItems)
            {
                var orderItem = new OrderItem
                {
                    ConsignmentItemId = consignmentItem.Id,
                    Quantity = 1
                };
                order.Items.Add(orderItem);
                consignmentItem.Checkedout = true;
            }

            // Save the order and update the consignment
            _unitOfWork.OrderRepository.Create(order);
            _unitOfWork.SaveChange();

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = new
                {
                    OrderId = order.Id,
                    TotalPrice = totalPrice
                }
            });
        }


        // Checkout a Single Consignment Item
        [HttpPost("checkout-item/{itemId}")]
        public IActionResult CheckoutItem(string itemId)
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

            // Retrieve the consignment item and ensure it belongs to the current user's consignment
            var consignmentItem = _unitOfWork.ConsignmentItemRepository.GetSingle(ci => ci.Id == itemId && ci.Consignment.UserId == userId, ci => ci.Consignment);
            if (consignmentItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment item not found or does not belong to the current user."
                });
            }

            if (consignmentItem.Status != "Approved" || consignmentItem.Checkedout)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Item is not available for checkout."
                });
            }

            // Calculate the number of days
            var totalDays = (DateTimeOffset.Now - consignmentItem.CreatedTime).Days;
            if (totalDays < 1) totalDays = 1;
            var totalPrice = 25000 * totalDays;

            // Create the order for this single consignment item
            var order = new Order
            {
                UserId = userId,
                Status = "Pending",
                Total = totalPrice,
                Items = new List<OrderItem>
        {
            new OrderItem
            {
                ConsignmentItemId = consignmentItem.Id,
                Quantity = 1
            }
        }
            };

            consignmentItem.Checkedout = true;

            // Save the order and update the consignment item
            _unitOfWork.OrderRepository.Create(order);
            _unitOfWork.ConsignmentItemRepository.Update(consignmentItem);
            _unitOfWork.SaveChange();

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = new
                {
                    OrderId = order.Id,
                    TotalPrice = totalPrice
                }
            });
        }


        // Get all consignments
        [HttpGet("all-consignments")]
        public IActionResult GetAllConsignments()
        {
            var consignments = _unitOfWork.ConsignmentRepository.Get(c => !c.IsDeleted, includeProperties: c => c.Items).ToList();
            if (!consignments.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No consignments found."
                });
            }

            var response = consignments.Select(consignment => new
            {
                ConsignmentId = consignment.Id,
                UserId = consignment.UserId,
                Status = consignment.Status,
                Items = consignment.Items.Select(item => new
                {
                    ItemId = item.Id,
                    Name = item.Name,
                    Category = item.Category,
                    Status = item.Status,
                    Checkedout = item.Checkedout
                }).ToList()
            }).ToList();

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = response
            });
        }

        // Get consignment item by ID
        [HttpGet("item/{itemId}")]
        public IActionResult GetConsignmentItemById(string itemId)
        {
            var consignmentItem = _unitOfWork.ConsignmentItemRepository.GetById(itemId);
            if (consignmentItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment item not found."
                });
            }

            var response = new
            {
                ItemId = consignmentItem.Id,
                Name = consignmentItem.Name,
                Category = consignmentItem.Category,
                Status = consignmentItem.Status,
                Checkedout = consignmentItem.Checkedout,
                ConsignmentId = consignmentItem.ConsignmentId
            };

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = response
            });
        }

        // Get consignments for the current user
        [HttpGet("user-consignments")]
        public IActionResult GetConsignmentsForCurrentUser()
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

            var consignments = _unitOfWork.ConsignmentRepository.Get(c => c.UserId == userId && !c.IsDeleted, includeProperties: c => c.Items).ToList();
            if (!consignments.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No consignments found for the current user."
                });
            }

            var response = consignments.Select(consignment => new
            {
                ConsignmentId = consignment.Id,
                UserId = consignment.UserId,
                Status = consignment.Status,
                Items = consignment.Items.Select(item => new
                {
                    ItemId = item.Id,
                    Name = item.Name,
                    Category = item.Category,
                    Status = item.Status,
                    Checkedout = item.Checkedout
                }).ToList()
            }).ToList();

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = response
            });
        }

        [HttpPost("checkout/{consignmentItemId}")]
        public IActionResult CheckoutConsignmentItem(string consignmentItemId)
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

            var consignmentItem = _unitOfWork.ConsignmentItemRepository.GetSingle(
                ci => ci.Id == consignmentItemId && ci.Consignment.UserId == userId && !ci.Checkedout
            );

            if (consignmentItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment item not found or not available for checkout."
                });
            }

            if (consignmentItem.Status != "Approved")
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Consignment item is not approved for checkout."
                });
            }

            // Calculate the total price based on the number of days
            var totalDays = (DateTimeOffset.Now - consignmentItem.CreatedTime).Days;
            if (totalDays < 1) totalDays = 1; // Minimum charge for at least 1 day
            var totalPrice = 25000 * totalDays;

            // Create a new order
            var order = new Order
            {
                UserId = userId,
                Total = totalPrice,
                Status = "Pending",
                Address = GetUserAddress(userId),
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ConsignmentItemId = consignmentItem.Id,
                        Quantity = 1
                    }
                }
            };

            // Mark the consignment item as checked out
            consignmentItem.Checkedout = true;

            // Save changes
            _unitOfWork.OrderRepository.Create(order);
            _unitOfWork.ConsignmentItemRepository.Update(consignmentItem);
            _unitOfWork.SaveChange();

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = new OrderResponseModel
                {
                    OrderId = order.Id,
                    Total = order.Total,
                    Status = order.Status,
                    UserId = order.UserId,
                    Address = order.Address,
                    CreatedTime = order.CreatedTime,
                    Items = order.Items.Select(item => new OrderItemResponseModel
                    {
                        ProductItemId = item.ConsignmentItemId,
                        Quantity = item.Quantity,
                        Price = totalPrice
                    }).ToList()
                }
            });
        }

        private string GetUserAddress(string userId)
        {
            var user = _unitOfWork.UserRepository.GetById(userId);
            return user?.Address ?? string.Empty;
        }

    }
}
