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
                    UserId = userId,
                    Items = new List<ConsignmentItems>()
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
                ImageUrl = model.ImageUrl,
                Status = "Pending",
                ConsignmentId = consignment.Id
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
                        Origin = item.Origin,
                        Sex = item.Sex,
                        Age = item.Age,
                        Size = item.Size,
                        Species = item.Species,
                        ImageUrl = item.ImageUrl,
                        Status = item.Status
                    }).ToList()
                }
            });
        }


        // Update Consignment Item Status
        [HttpPut("update-item-status/{itemId}")]
        public IActionResult UpdateConsignmentItemStatus(string itemId, [FromBody] UpdateConsignmentStatusRequestModel model)
        {
            // Get the user ID from the claims
            var userId = GetUserIdFromClaims();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized. User ID not found in claims."
                });
            }

            // Get the current user from the repository, including the user's role
            var currentUser = _unitOfWork.UserRepository.GetSingle(u => u.Id == userId, u => u.Role);
            if (currentUser == null)
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized. User not found."
                });
            }

            // Retrieve the consignment item without filtering by userId
            var consignmentItem = _unitOfWork.ConsignmentItemRepository.GetSingle(
                ci => ci.Id == itemId,
                ci => ci.Consignment);

            if (consignmentItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment item not found."
                });
            }

            // Check if the current user has access (owner or Staff/Manager role)
            bool isOwner = consignmentItem.Consignment.UserId == userId;
            bool isStaffOrManager = currentUser.Role.Name == "Staff" || currentUser.Role.Name == "Manager";

            if (!isOwner && !isStaffOrManager)
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 403,
                    MessageError = "You do not have permission to update the status of this consignment item."
                });
            }

            // Update the status
            consignmentItem.Status = model.Status;
            _unitOfWork.ConsignmentItemRepository.Update(consignmentItem);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = "Consignment item status updated successfully.",
                Data = consignmentItem.Status
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
                Items = consignment.Items.Select(item => new
                {
                    ItemId = item.Id,
                    Name = item.Name,
                    Category = item.Category,
                    Status = item.Status,
                    ImageUrl = item.ImageUrl,

                    Checkedout = item.Checkedout,
                    createDate = item.CreatedTime
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
                ImageUrl = consignmentItem.ImageUrl,
                ConsignmentId = consignmentItem.ConsignmentId,
                createDate = consignmentItem.CreatedTime
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
                Items = consignment.Items.Select(item => new
                {
                    ItemId = item.Id,
                    
                    Name = item.Name,
                    Category = item.Category,
                    Status = item.Status,
                    ImageUrl = item.ImageUrl,
                    Checkedout = item.Checkedout,
                    createDate = item.CreatedTime
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
                ci => ci.Id == consignmentItemId && ci.Consignment.UserId == userId && ci.Status == "Approved" && !ci.Checkedout
            );

            if (consignmentItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment item not found or not available for checkout."
                });
            }

            var consignment = _unitOfWork.ConsignmentRepository.GetSingle(
                ci => ci.Id == consignmentItem.ConsignmentId
            );

            if (consignment == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment not found or not available for checkout."
                });
            }

            try
            {
                // Prefix the name to identify consignment-generated products
                var newProduct = new Product
                {
                    Name = $"[Consignment]-{consignmentItem.Name}",
                    Quantity = 1,
                };

                var newProductItem = new ProductItem
                {
                    Name = $"[Consignment]-{newProduct.Name}",
                    Price = 25000,
                    Category = consignmentItem.Category ?? "Unknown",
                    Origin = consignmentItem.Origin ?? "Unknown",
                    Size = consignmentItem.Size ?? "Unknown",
                    Species = consignmentItem.Species ?? "Unknown",
                    Sex = consignmentItem.Sex ?? "Unknown",
                    Age = consignmentItem.Age,
                    Personality = consignmentItem.Personality ?? "Unknown",
                    FoodAmount = consignmentItem.FoodAmount ?? "Standard",
                    WaterTemp = consignmentItem.WaterTemp ?? "Normal",
                    MineralContent = consignmentItem.MineralContent ?? "Normal",
                    PH = consignmentItem.PH ?? "Neutral",
                    ImageUrl = consignmentItem.ImageUrl ?? "",
                    Quantity = 1,
                    Type = consignmentItem.Type ?? "Default",
                    ProductId = newProduct.Id,
                    
                };

                newProduct.ProductItems = new List<ProductItem> { newProductItem };

                var newOrder = new Order
                {
                    UserId = userId,
                    Total = 25000,
                    CreatedTime = DateTimeOffset.Now,
                    Status = "Pending",
                    ConsignmentId = consignment.Id,
                };

                var newOrderItem = new OrderItem
                {
                    OrderID = newOrder.Id,
                    ProductItemId = newProductItem.Id,
                    ConsignmentItemId = consignmentItem.Id,
                    Quantity = 1,
                };

                newOrder.Items = new List<OrderItem> { newOrderItem };

                _unitOfWork.ProductRepository.Create(newProduct);
                _unitOfWork.OrderRepository.Create(newOrder);

                consignmentItem.Checkedout = true;
                consignmentItem.OrderItemId = newOrderItem.Id;
                _unitOfWork.ConsignmentItemRepository.Update(consignmentItem);
                _unitOfWork.SaveChange();

                return Ok(new ResponseModel
                {
                    StatusCode = 201,
                    MessageError = "Consignment item checked out and converted to product successfully.",
                    Data = new { ProductId = newProduct.Id, ProductItemId = newProductItem.Id, OrderId = newOrder.Id, OrderItemId = newOrderItem.Id }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    MessageError = "An error occurred while processing the checkout.",
                    Data = null
                });
            }
        }




        [HttpDelete("remove-item/{itemId}")]
        public IActionResult RemoveConsignmentItem(string itemId)
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
                ci => ci.Id == itemId && ci.Consignment.UserId == userId && !ci.Checkedout,
                ci => ci.Consignment
            );

            if (consignmentItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment item not found or does not belong to the current user."
                });
            }

            if (consignmentItem.Checkedout)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Cannot remove a checked out consignment item."
                });
            }

            var consignment = consignmentItem.Consignment;
            consignment.Items.Remove(consignmentItem);
            _unitOfWork.ConsignmentItemRepository.Delete(consignmentItem);
            _unitOfWork.ConsignmentRepository.Update(consignment);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = "Consignment item removed successfully."
            });
        }


        private string GetUserAddress(string userId)
        {
            var user = _unitOfWork.UserRepository.GetById(userId);
            return user?.Address ?? string.Empty;
        }

    }
}
