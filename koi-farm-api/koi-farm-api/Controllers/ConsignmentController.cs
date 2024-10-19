using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Model;
using Repository.Model.Consignment;
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

        // Create Consignment Endpoint
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
                Status = "Pending" // Set initial status as "Pending"
            };

            // Add the item to the consignment
            consignment.Items.Add(consignmentItem);
            _unitOfWork.ConsignmentRepository.Update(consignment);
            _unitOfWork.SaveChange();

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = new ConsignmentResponseModel
                {
                    ConsignmentId = consignment.Id,
                    UserId = consignment.UserId,
                    Items = consignment.Items.Select(item => new ConsignmentItemResponseModel
                    {
                        Name = item.Name,
                        Category = item.Category,
                        Origin = item.Origin,
                        Sex = item.Sex,
                        Age = item.Age,
                        Size = item.Size,
                        Species = item.Species,
                        Status = item.Status,
                    }).ToList()
                }
            });
        }

        // Update Consignment Status Endpoint
        [HttpPut("update-status/{consignmentId}")]
        public IActionResult UpdateConsignmentStatus(string consignmentId, [FromBody] UpdateConsignmentStatusRequestModel model)
        {
            var consignment = _unitOfWork.ConsignmentRepository.GetSingle(c => c.Id == consignmentId && !c.IsDeleted, c => c.Items);
            if (consignment == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment not found."
                });
            }

            if (consignment.Items.All(i => i.Status != "Pending"))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "All items are already approved or have a different status."
                });
            }

            // Change status to "Approved"
            foreach (var item in consignment.Items)
            {
                if (item.Status == "Pending")
                {
                    item.Status = "Approved";
                }
            }

            _unitOfWork.ConsignmentRepository.Update(consignment);
            _unitOfWork.SaveChange();

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = "Consignment status updated successfully.",
                Data = new ConsignmentResponseModel
                {
                    ConsignmentId = consignment.Id,
                    UserId = consignment.UserId,
                    Items = consignment.Items.Select(item => new ConsignmentItemResponseModel
                    {
                        Name = item.Name,
                        Category = item.Category,
                        Origin = item.Origin,
                        Sex = item.Sex,
                        Age = item.Age,
                        Size = item.Size,
                        Species = item.Species,
                        Status = item.Status
                    }).ToList()
                }
            });
        }

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

            // Retrieve the consignment and its approved items
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
            var approvedItems = consignment.Items.Where(i => i.Status == "Approved" && !i.Checkedout).ToList();
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
                Address = "Address goes here", // You can fetch from the user profile or input
                Items = new List<OrderItem>()
            };

            // Create OrderItems for each approved ConsignmentItem
            foreach (var consignmentItem in approvedItems)
            {
                var orderItem = new OrderItem
                {
                    ConsignmentItemId = consignmentItem.Id, // Link to ConsignmentItemId
                    Quantity = 1 // Each consignment item is unique, quantity is always 1
                };

                order.Items.Add(orderItem);

                // Mark consignment item as checked out and link to orderItem
                consignmentItem.Checkedout = true;
                consignmentItem.OrderItem = orderItem;
            }

            // Save the order and update the consignment
            _unitOfWork.OrderRepository.Create(order);
            _unitOfWork.ConsignmentRepository.Update(consignment);
            _unitOfWork.SaveChange();

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = new
                {
                    OrderId = order.Id,
                    TotalPrice = totalPrice,
                    Items = approvedItems.Select(item => new
                    {
                        item.Id,
                        item.Name,
                        PricePerItem = 25000 * totalDays // Display price per item based on the number of days
                    }).ToList()
                }
            });
        }

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

            // Retrieve the consignment item
            var consignmentItem = _unitOfWork.ConsignmentItemRepository.GetById(itemId);
            if (consignmentItem == null || consignmentItem.Consignment == null || consignmentItem.Consignment.UserId != userId)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment item not found or does not belong to the current user."
                });
            }

            // Ensure the item is approved and not already checked out
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
            if (totalDays < 1) totalDays = 1; // Minimum charge for at least 1 day
            var totalPrice = 25000 * totalDays;

            // Create the order for this single consignment item
            var order = new Order
            {
                UserId = userId,
                Status = "Pending",
                Total = totalPrice,
                Address = "Address goes here", // You can fetch from the user profile or input
                Items = new List<OrderItem>()
            };

            var orderItem = new OrderItem
            {
                ConsignmentItemId = consignmentItem.Id, // Link to the consignment item
                Quantity = 1 // Since each consignment item represents a single product
            };

            order.Items.Add(orderItem);

            // Mark consignment item as checked out
            consignmentItem.Checkedout = true;
            consignmentItem.OrderItem = orderItem;

            // Save changes
            _unitOfWork.OrderRepository.Create(order);
            _unitOfWork.ConsignmentItemRepository.Update(consignmentItem);
            _unitOfWork.SaveChange();

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = new
                {
                    OrderId = order.Id,
                    TotalPrice = totalPrice,
                    Item = new
                    {
                        consignmentItem.Id,
                        consignmentItem.Name,
                        PricePerItem = totalPrice
                    }
                }
            });
        }


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
    }
}
