using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Model;
using Repository.Model.Product;
using Repository.Model.ProductItem;
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
            var products = _unitOfWork.ProductRepository
                .Get(c => !c.IsDeleted && !c.Name.StartsWith("[Consignment]-"))
                .ToList();

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

        [HttpGet("get-product/{id}")]
        public IActionResult GetProductById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Product ID is required."
                });
            }

            var product = _unitOfWork.ProductRepository.GetById(id);

            if (product == null || product.Name.StartsWith("[Consignment]-"))
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Product not found."
                });
            }

            var responseProduct = _mapper.Map<ResponseProductModel>(product);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseProduct
            });
        }


        [Authorize(Roles = "Manager,Staff")]
        [HttpPost("create-product")]
        public IActionResult CreateProduct(RequestCreateProductModel productModel)
        {
            if (productModel == null || string.IsNullOrEmpty(productModel.Name))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid product data. Name and Quantity are required."
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

        [Authorize(Roles = "Manager,Staff")]
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

        [Authorize(Roles = "Manager,Staff")]
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
