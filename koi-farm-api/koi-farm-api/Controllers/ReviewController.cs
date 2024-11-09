using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Model.Blog;
using Repository.Model;
using Repository.Repository;
using Repository.Model.Review;
using Microsoft.AspNetCore.Authorization;
using Repository.Data.Entity;

namespace koi_farm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private string GetUserIdFromClaims()
        {
            return User.FindFirst("UserID")?.Value;
        }

        [HttpGet("get-all-reviews")]
        public IActionResult GetAllReviews()
        {
            var reviews = _unitOfWork.ReviewRepository.GetAll();

            if (!reviews.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No reviews found."
                });
            }

            var responseReviews = _mapper.Map<List<ResponseReviewModel>>(reviews);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseReviews
            });
        }

        [HttpGet("get-review/{id}")]
        public IActionResult GetReview(string id)
        {
            var review = _unitOfWork.ReviewRepository.GetById(id);

            if (review == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Review not found."
                });
            }

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = review
            });
        }

        [HttpGet("get-reviews-by-product-item/{productItemId}")]
        public IActionResult GetReviewsByProductItem(string productItemId)
        {
            var productItem = _unitOfWork.ProductItemRepository.GetById(productItemId);
            if (productItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "ProductItem not found."
                });
            }

            var reviews = _unitOfWork.ReviewRepository.GetAll().Where(r => r.ProductItemId == productItemId);

            if (!reviews.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No reviews found for this product item."
                });
            }

            var responseReviews = _mapper.Map<List<ResponseReviewModel>>(reviews);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseReviews
            });
        }

        [Authorize]
        [HttpPost("create-review")]
        public IActionResult CreateReview(RequestCreateReviewModel reviewModel)
        {
            if (reviewModel == null || reviewModel.Rating < 0 || reviewModel.Rating > 5 || 
                string.IsNullOrEmpty(reviewModel.Description) || string.IsNullOrEmpty(reviewModel.ProductItemId))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid review data. Rating, Description and ProductItemId are required."
                });
            }

            var userId = GetUserIdFromClaims();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized: UserId not found."
                });
            }

            var productItem = _unitOfWork.ProductItemRepository.GetById(reviewModel.ProductItemId);
            if (productItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "ProductItem not found."
                });
            }

            var review = _mapper.Map<Review>(reviewModel);
            review.UserId = userId;

            _unitOfWork.ReviewRepository.Create(review);

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = review
            });
        }

        [HttpPut("update-review/{reviewId}")]
        [Authorize]
        public IActionResult UpdateReview(string reviewId, RequestCreateReviewModel reviewModel)
        {
            if (reviewModel == null || reviewModel.Rating < 0 || reviewModel.Rating > 5 ||
                string.IsNullOrEmpty(reviewModel.Description) || string.IsNullOrEmpty(reviewModel.ProductItemId))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid review data. Rating, Description, and ProductItemId are required."
                });
            }

            var userId = GetUserIdFromClaims();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized: UserId not found."
                });
            }

            var existingReview = _unitOfWork.ReviewRepository.GetById(reviewId);

            if (existingReview == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Review not found."
                });
            }

            // Only allow the user who created the review to update it
            if (existingReview.UserId != userId)
            {
                return Forbid();
            }

            var productItem = _unitOfWork.ProductItemRepository.GetById(reviewModel.ProductItemId);
            if (productItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "ProductItem not found."
                });
            }

            _mapper.Map(reviewModel, existingReview);

            _unitOfWork.ReviewRepository.Update(existingReview);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = existingReview
            });
        }

        [Authorize]
        [HttpDelete("delete-review/{id}")]
        public IActionResult DeleteReview(string id)
        {
            var review = _unitOfWork.ReviewRepository.GetById(id);

            if (review == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Review not found."
                });
            }

            var userId = GetUserIdFromClaims();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized: UserId not found."
                });
            }

            // Only allow the user who created the review to delete it
            if (review.UserId != userId)
            {
                return Forbid();
            }

            _unitOfWork.ReviewRepository.Delete(review);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = "Review deleted successfully."
            });
        }
    }
}
