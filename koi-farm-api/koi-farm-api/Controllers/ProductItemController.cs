using AutoMapper;
using Repository.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Model;
using Repository.Model.ProductItem;
using Repository.Model.Review;
using Repository.Repository;
using Microsoft.AspNetCore.Authorization;

namespace koi_farm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductItemController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("get-all-product-items")]
        public IActionResult GetAllProductItems(int pageIndex = 1, int pageSize = 10, string? searchQuery = null)
        {
            var productItems = _unitOfWork.ProductItemRepository
                .Get(c => !c.IsDeleted && !c.Name.StartsWith("[Consignment]-") && c.BatchId == null)
                .OrderByDescending(c => c.CreatedTime)
                .ToList();

            if (!productItems.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No product items found."
                });
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                productItems = productItems
                    .Where(item => item.Name != null &&
                        item.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var totalItems = productItems.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedProductItems = productItems
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var responseProductItems = _mapper.Map<List<ResponseProductItemModel>>(pagedProductItems);

            var responseSearchModel = new ResponseSearchModel<ResponseProductItemModel>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Entities = responseProductItems
            };

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseSearchModel
            });
        }

        [HttpGet("get-all-batch-product-items")]
        public IActionResult GetAllBatchProductItems()
        {
            var productItems = _unitOfWork.ProductItemRepository
                .Get(c => !c.IsDeleted && !c.Name.StartsWith("[Consignment]-") && c.BatchId != null)
                .ToList();

            if (!productItems.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No product items found."
                });
            }

            var responseProductItems = _mapper.Map<List<ResponseBatchProductItemModel>>(productItems);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseProductItems
            });
        }

        [HttpGet("get-product-item/{id}")]
        public IActionResult GetProductItemById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "ProductItem ID is required."
                });
            }

            var productItem = _unitOfWork.ProductItemRepository.Get(c => c.Id == id && c.BatchId == null).FirstOrDefault();

            if (productItem == null || productItem.Name.StartsWith("[Consignment]-"))
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "ProductItem not found."
                });
            }

            var responseProductItem = _mapper.Map<ResponseProductItemModel>(productItem);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseProductItem
            });
        }

        [HttpGet("get-batch-product-item/{id}")]
        public IActionResult GetBatchProductItemById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "ProductItem ID is required."
                });
            }

            var productItem = _unitOfWork.ProductItemRepository.Get(c => c.Id == id && c.BatchId != null).FirstOrDefault();

            if (productItem == null || productItem.Name.StartsWith("[Consignment]-"))
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "ProductItem not found."
                });
            }

            var responseProductItem = _mapper.Map<ResponseBatchProductItemModel>(productItem);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseProductItem
            });
        }


        [HttpGet("get-product-item-by-product/{productId}")]
        public IActionResult GetProductItemByProduct(string productId)
        {
            var product = _unitOfWork.ProductRepository.GetById(productId);
            if (product == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Product not found."
                });
            }

            var productItems = _unitOfWork.ProductItemRepository
                .Get(r => r.ProductId == productId && !r.Name.StartsWith("[Consignment]-") && r.BatchId == null)
                .ToList();

            if (!productItems.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No product items found for this product."
                });
            }

            var responseProductItems = _mapper.Map<List<ResponseProductItemModel>>(productItems);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseProductItems
            });
        }

        [HttpGet("get-product-item-by-batch/{batchId}")]
        public IActionResult GetProductItemByBatch(string batchId)
        {
            var product = _unitOfWork.BatchRepository.GetById(batchId);
            if (product == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Product not found."
                });
            }

            var productItems = _unitOfWork.ProductItemRepository
                .Get(r => r.BatchId == batchId && !r.Name.StartsWith("[Consignment]-"))
                .ToList();

            if (!productItems.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No product items found for this product."
                });
            }

            var responseProductItems = _mapper.Map<List<ResponseBatchProductItemModel>>(productItems);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseProductItems
            });
        }


        private bool ValidateField(RequestCreateProductItemModel productItemModel)
        {
            if (string.IsNullOrEmpty(productItemModel.Name) ||
                string.IsNullOrEmpty(productItemModel.Category) ||
                string.IsNullOrEmpty(productItemModel.Origin) ||
                string.IsNullOrEmpty(productItemModel.Sex) ||
                string.IsNullOrEmpty(productItemModel.Size) ||
                string.IsNullOrEmpty(productItemModel.Species) ||
                string.IsNullOrEmpty(productItemModel.Personality) ||
                string.IsNullOrEmpty(productItemModel.FoodAmount) ||
                string.IsNullOrEmpty(productItemModel.WaterTemp) ||
                string.IsNullOrEmpty(productItemModel.MineralContent) ||
                string.IsNullOrEmpty(productItemModel.PH) ||
                string.IsNullOrEmpty(productItemModel.ImageUrl) ||
                string.IsNullOrEmpty(productItemModel.Type) ||
                productItemModel.Price < 10000 ||
                productItemModel.Age <= 0 ||
                productItemModel.Quantity <= 0 ||
                string.IsNullOrEmpty(productItemModel.ProductId))
            {
                return false;
            }
            return true;
        }

        [Authorize(Roles = "Manager,Staff")]
        [HttpPost("create-product-item")]
        public IActionResult CreateProductItem(RequestCreateProductItemModel productItemModel)
        {
            if (productItemModel == null || ValidateField(productItemModel) == false)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid ProductItem data. Every field is required."
                });
            }

            var productExists = _unitOfWork.ProductRepository.GetById(productItemModel.ProductId);
            if (productExists == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid ProductId. Product does not exist."
                });
            }

            productExists.Quantity += productItemModel.Quantity;

            var productItem = _mapper.Map<ProductItem>(productItemModel);
            _unitOfWork.ProductItemRepository.Create(productItem);
            _unitOfWork.ProductRepository.Update(productExists);

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = productItem
            });
        }

        [Authorize(Roles = "Manager,Staff")]
        [HttpPut("update-product-item/{id}")]
        public IActionResult UpdateProductItem(string id, RequestCreateProductItemModel productItemModel)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "ProductItem ID is required."
                });
            }

            if (productItemModel == null || ValidateField(productItemModel) == false)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid ProductItem data. Every field is required."
                });
            }

            var existingProductItem = _unitOfWork.ProductItemRepository.GetById(id);
            if (existingProductItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "ProductItem not found."
                });
            }

            var productExists = _unitOfWork.ProductRepository.GetById(existingProductItem.ProductId);
            if (productExists == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid ProductId. Product does not exist."
                });
            }

            if (existingProductItem.Quantity != productItemModel.Quantity)
            {
                var quantity = productItemModel.Quantity - existingProductItem.Quantity;
                productExists.Quantity = productExists.Quantity + quantity;
            }

            _mapper.Map(productItemModel, existingProductItem);

            _unitOfWork.ProductItemRepository.Update(existingProductItem);
            _unitOfWork.ProductRepository.Update(productExists);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = existingProductItem
            });
        }

        [Authorize(Roles = "Manager,Staff")]
        [HttpDelete("delete-product-item/{id}")]
        public IActionResult DeleteProductItem(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "ProductItem ID is required."
                });
            }

            var productItem = _unitOfWork.ProductItemRepository.GetById(id);
            if (productItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "ProductItem not found."
                });
            }

            var productExists = _unitOfWork.ProductRepository.GetById(productItem.ProductId);
            if (productExists == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid ProductId. Product does not exist."
                });
            }

            productExists.Quantity -= productItem.Quantity;

            _unitOfWork.ProductItemRepository.Delete(productItem);
            _unitOfWork.ProductRepository.Update(productExists);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = "ProductItem successfully deleted."
            });
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("update-product-item-type/{id}")]
        public IActionResult UpdateProductItemType(string id, [FromBody] RequestUpdateProductItemType model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "ProductItem ID is required."
                });
            }

            if (string.IsNullOrEmpty(model.Type))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "New type is required."
                });
            }

            var productItem = _unitOfWork.ProductItemRepository.GetById(id);
            if (productItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "ProductItem not found."
                });
            }

            var validTypes = new[] { "Pending Approval", "Approved", "Rejected" };
            if (!validTypes.Contains(model.Type))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid order status."
                });
            }

            productItem.Type = model.Type;
            _unitOfWork.ProductItemRepository.Update(productItem);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = "ProductItem type updated successfully."
            });
        }

    }
}
