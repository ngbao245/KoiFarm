using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Model;
using Repository.Model.Product;
using Repository.Repository;

namespace koi_farm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("get-all-products")]

        public IActionResult GetAllProducts()
        {
            var products = _unitOfWork.ProductRepository.GetAll();

            if (!products.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No products found."
                });
            }

            var responseProducts = _mapper.Map<List<ResponseProductModel>>(products);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseProducts
            });
        }

        [HttpPost("create-product")]

        public IActionResult CreateProduct(RequestCreateProductModel productModel)
        {
            if (productModel == null || string.IsNullOrEmpty(productModel.Name) || string.IsNullOrEmpty(productModel.Quantity))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid product data. Name and Quantity are required."
                });
            }

            if (!int.TryParse(productModel.Quantity, out _))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid product data. Quantity must be a valid number."
                });
            }

            var existingProduct = _unitOfWork.ProductRepository.GetSingle(p => p.Name.ToLower() == productModel.Name.ToLower());
            if (existingProduct != null)
            {
                return Conflict(new ResponseModel
                {
                    StatusCode = 409,
                    MessageError = "Name already exists."
                });
            }

            var product = _mapper.Map<Product>(productModel);

            _unitOfWork.ProductRepository.Create(product);

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = product
            });
        }

        [HttpPut("update-product/{id}")]

        public IActionResult UpdateProduct(string id, [FromBody] RequestCreateProductModel productModel)
        {
            if (string.IsNullOrEmpty(id) || productModel == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid product data. ID and product data are required."
                });
            }

            if (!int.TryParse(productModel.Quantity, out _))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid product data. Quantity must be a valid number."
                });
            }

            var product = _unitOfWork.ProductRepository.GetById(id);
            if (product == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Product not found."
                });
            }

            var existingProduct = _unitOfWork.ProductRepository.GetSingle(p => p.Name.ToLower() == productModel.Name.ToLower() && p.Id != id);
            if (existingProduct != null)
            {
                return Conflict(new ResponseModel
                {
                    StatusCode = 409,
                    MessageError = "Product name already exists."
                });
            }

            _mapper.Map(productModel, product);

            _unitOfWork.ProductRepository.Update(product);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = product
            });
        }

        [HttpDelete("delete-product/{id}")]
        public IActionResult DeleteProduct(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "ProductId cannot be null or empty."
                });
            }

            var product = _unitOfWork.ProductRepository.GetById(id);

            if (product == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = $"Product with Id: {id} not found."
                });
            }

            _unitOfWork.ProductRepository.Delete(product);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = $"Product with ID {id} successfully deleted."
            });
        }
    }
}
