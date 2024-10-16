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
            return User.FindFirst("UserID")?.Value;
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
            var consignment = _unitOfWork.ConsignmentRepository.GetSingle(c => c.UserId == userId, c => c.Items);
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
                status = "Pending" // Set initial status as "Pending"
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
                        Status = item.status
                    }).ToList()
                }
            });
        }

        // Update Consignment Status Endpoint
        [HttpPut("update-status/{consignmentId}")]
        public IActionResult UpdateConsignmentStatus(string consignmentId, [FromBody] UpdateConsignmentStatusRequestModel model)
        {
            var consignment = _unitOfWork.ConsignmentRepository.GetSingle(c => c.Id == consignmentId, c => c.Items);
            if (consignment == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Consignment not found."
                });
            }

            if (consignment.Items.All(i => i.status != "Pending"))
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
                if (item.status == "Pending")
                {
                    item.status = "Approved";
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
                        Status = item.status
                    }).ToList()
                }
            });
        }
    }
}
