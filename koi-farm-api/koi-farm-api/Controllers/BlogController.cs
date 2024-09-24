using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Model;
using Repository.Model.Blog;
using Repository.Repository;
using System.Security.Claims;

namespace koi_farm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BlogController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private string GetUserIdFromClaims()
        {
            return User.FindFirst("UserID")?.Value;
        }

        [HttpGet("get-all-blogs")]
        public IActionResult GetAllBlogs()
        {
            var blogs = _unitOfWork.BlogRepository.GetAll();

            if (!blogs.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No blogs found."
                });
            }

            var responseBlogs = _mapper.Map<List<ResponseBlogModel>>(blogs);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseBlogs
            });
        }

        [HttpGet("get-blog/{id}")]
        public IActionResult GetBlog(string id)
        {
            var blog = _unitOfWork.BlogRepository.GetById(id);
            if (blog == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Blog not found."
                });
            }

            var responseBlog = _mapper.Map<ResponseBlogModel>(blog);
            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseBlog
            });
        }

        [HttpPost("create-blog")]
        public IActionResult CreateBlog([FromBody] RequestCreateBlogModel blogModel)
        {
            if (blogModel == null || string.IsNullOrEmpty(blogModel.Title) || string.IsNullOrEmpty(blogModel.Description))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid blog data. Title and Description are required."
                });
            }

            // Get the UserId from claims
            var userId = GetUserIdFromClaims();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized: UserId not found."
                });
            }

            var blog = _mapper.Map<Blog>(blogModel);
            blog.UserId = userId;

            _unitOfWork.BlogRepository.Create(blog);

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = blog
            });
        }

        [HttpPut("update-blog/{id}")]
        public IActionResult UpdateBlog(string id, [FromBody] RequestCreateBlogModel blogModel)
        {
            if (string.IsNullOrEmpty(id) || blogModel == null || string.IsNullOrEmpty(blogModel.Title))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid blog data. ID and Title are required."
                });
            }

            var blog = _unitOfWork.BlogRepository.GetById(id);
            if (blog == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Blog not found."
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

            // Only allow the user who created the blog to update it
            if (blog.UserId != userId)
            {
                return Forbid();
            }

            _mapper.Map(blogModel, blog);

            _unitOfWork.BlogRepository.Update(blog);
            _unitOfWork.SaveChange();

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = blog
            });
        }

        [HttpDelete("delete-blog/{id}")]
        public IActionResult DeleteBlog(string id)
        {
            var blog = _unitOfWork.BlogRepository.GetById(id);
            if (blog == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Blog not found."
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

            // Only allow the user who created the blog to delete it
            if (blog.UserId != userId)
            {
                return Forbid();
            }

            _unitOfWork.BlogRepository.Delete(blog);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = "Blog deleted successfully."
            });
        }
    }
}
