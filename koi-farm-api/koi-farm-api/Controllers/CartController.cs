using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Model.Cart;
using Repository.Model;
using Repository.Repository;
using Repository.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace koi_farm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CartController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Add to Cart Endpoint
        [HttpPost("add-to-cart")]
        public IActionResult AddToCart([FromBody] CartRequestModel requestModel)
        {
            var userId = User.FindFirst("UserID")?.Value;

            var cart = _unitOfWork.CartRepository.GetSingle(c => c.UserId == userId, c => c.Items);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem>(),
                    Total = 0
                };
                _unitOfWork.CartRepository.Create(cart);
            }

            var productItem = _unitOfWork.ProductItemRepository.GetById(requestModel.ProductItemId);

            if (productItem == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "ProductItem not found."
                });
            }

            if (requestModel.Quantity > productItem.Quantity)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    MessageError = "Requested quantity exceeds available stock."
                });
            }

            var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductItemId == requestModel.ProductItemId);
            if (cartItem != null)
            {
                cartItem.Quantity += requestModel.Quantity;
                if (cartItem.Quantity > productItem.Quantity)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        MessageError = "Exceeds Quantity of Product Item"
                    });
                }
                _unitOfWork.CartItemRepository.Update(cartItem);
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductItemId = requestModel.ProductItemId,
                    Quantity = requestModel.Quantity,
                    CartId = cart.Id
                };
                cart.Items.Add(cartItem);
                _unitOfWork.CartItemRepository.Create(cartItem);
            }

            cart.Total = cart.Items.Sum(item => item.Quantity * _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price);
            _unitOfWork.SaveChange();

            var response = new CartResponseModel
            {
                CartId = cart.Id,
                Total = cart.Total,
                Items = cart.Items.Select(item => new CartItemModel
                {
                    ProductItemId = item.ProductItemId,
                    Quantity = item.Quantity,
                    ProductName = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Name,
                    Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price
                }).ToList()
            };

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Data = response
            });
        }

        [HttpPost("add-batch-to-cart")]
        public IActionResult AddBatchToCart([FromBody] BatchCartRequestModel requestModel)
        {
            var userId = User.FindFirst("UserID")?.Value;

            var cart = _unitOfWork.CartRepository.GetSingle(c => c.UserId == userId, c => c.Items);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem>(),
                    Total = 0
                };
                _unitOfWork.CartRepository.Create(cart);
            }

            var batch = _unitOfWork.BatchRepository.GetById(requestModel.BatchId);
            if (batch == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Batch not found."
                });
            }

            var productItems = _unitOfWork.ProductItemRepository
                .Get(pi => pi.BatchId == requestModel.BatchId)
                .ToList();

            if (!productItems.Any())
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "No product items found in this batch."
                });
            }

            foreach (var productItem in productItems)
            {
                if (productItem.Quantity <= 0)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        MessageError = $"Product item '{productItem.Name}' is out of stock."
                    });
                }

                var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductItemId == productItem.Id);
                if (cartItem != null)
                {
                    cartItem.Quantity++;
                    if (cartItem.Quantity > productItem.Quantity)
                    {
                        return BadRequest(new ResponseModel
                        {
                            StatusCode = 400,
                            MessageError = $"Requested quantity exceeds available stock for '{productItem.Name}'."
                        });
                    }
                    _unitOfWork.CartItemRepository.Update(cartItem);
                }
                else
                {
                    cartItem = new CartItem
                    {
                        ProductItemId = productItem.Id,
                        Quantity = 1,
                        CartId = cart.Id
                    };
                    cart.Items.Add(cartItem);
                    _unitOfWork.CartItemRepository.Create(cartItem);
                }
            }

            cart.Total = cart.Items.Sum(item => item.Quantity * _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price);
            _unitOfWork.SaveChange();

            var response = new CartResponseModel
            {
                CartId = cart.Id,
                Total = cart.Total,
                Items = cart.Items.Select(item => new CartItemModel
                {
                    ProductItemId = item.ProductItemId,
                    Quantity = item.Quantity,
                    ProductName = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Name,
                    Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price
                }).ToList()
            };

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = response
            });
        }


        // Remove from Cart Endpoint
        [HttpDelete("remove-from-cart/{cartId}")]
        [Authorize]
        public IActionResult RemoveFromCart(string cartId)
        {
            var userId = User.FindFirst("UserID")?.Value;
            var cart = _unitOfWork.CartRepository.GetSingle(c => c.Id == cartId && c.UserId == userId);

            if (cart == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    MessageError = "Cart not found."
                });
            }

            _unitOfWork.CartRepository.Delete(cart);

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Data = "Cart removed successfully."
            });
        }

        [HttpDelete("remove-batch-from-cart/{batchId}")]
        [Authorize]
        public IActionResult RemoveBatchFromCart(string batchId)
        {
            var userId = User.FindFirst("UserID")?.Value;

            var cart = _unitOfWork.CartRepository.GetSingle(c => c.UserId == userId, c => c.Items);
            if (cart == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    MessageError = "Cart not found."
                });
            }

            var productItems = _unitOfWork.ProductItemRepository.Get(pi => pi.BatchId == batchId).ToList();
            if (!productItems.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    MessageError = "No product items found in this batch."
                });
            }

            var itemsToRemove = cart.Items.Where(ci => productItems.Any(pi => pi.Id == ci.ProductItemId)).ToList();
            if (!itemsToRemove.Any())
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    MessageError = "No items in the cart are associated with the specified batch."
                });
            }

            foreach (var item in itemsToRemove)
            {
                _unitOfWork.CartItemRepository.Delete(item);
            }

            // Check if the cart is now empty
            cart.Items = cart.Items.Except(itemsToRemove).ToList();
            if (!cart.Items.Any())
            {
                _unitOfWork.CartRepository.Delete(cart);
            }
            else
            {
                // Update cart total if the cart is not empty
                cart.Total = cart.Items.Sum(item => item.Quantity * _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price);
                _unitOfWork.CartRepository.Update(cart);
            }

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Data = "Batch removed from cart successfully."
            });
        }


        // Update Cart Item Endpoint
        [HttpPut("update-cart-item/{cartId}/{productItemId}")]
        public IActionResult UpdateCartItem(string cartId, string productItemId, [FromBody] int quantity)
        {
            var userId = User.FindFirst("UserID")?.Value;
            var cart = _unitOfWork.CartRepository.GetSingle(c => c.Id == cartId && c.UserId == userId, c => c.Items);

            if (cart == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    MessageError = "Cart not found."
                });
            }

            var productItem = _unitOfWork.ProductItemRepository.GetById(productItemId);
            if (productItem == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    MessageError = "ProductItem not found."
                });
            }

            var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductItemId == productItemId);
            if (cartItem == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    MessageError = "Cart item not found."
                });
            }

            if (quantity > productItem.Quantity)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    MessageError = "Requested quantity exceeds available stock."
                });
            }

            if (quantity == 0)
            {
                _unitOfWork.CartItemRepository.Delete(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
                _unitOfWork.CartItemRepository.Update(cartItem);
            }

            cart.Total = cart.Items.Sum(item => item.Quantity * _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price);
            _unitOfWork.CartRepository.Update(cart);

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Data = "Cart item updated successfully."
            });
        }

        // Get Cart by UserID
        [HttpGet("get-cart")]
        [Authorize]
        public IActionResult GetCartByUserId()
        {
            var userId = User.FindFirst("UserID")?.Value;

            if (userId == null)
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    MessageError = "User not authorized."
                });
            }

            var cart = _unitOfWork.CartRepository.GetSingle(c => c.UserId == userId, c => c.Items);
            if (cart == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    MessageError = "Cart not found."
                });
            }

            var response = new CartResponseModel
            {
                CartId = cart.Id,
                Total = cart.Total,
                Items = cart.Items.Select(item => new CartItemModel
                {
                    ProductItemId = item.ProductItemId,
                    Quantity = item.Quantity,
                    ProductName = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Name,
                    Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price,
                    BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                }).ToList()
            };

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Data = response
            });
        }

        // Get all carts
        [HttpGet("get-all-carts")]
        public IActionResult GetAllCarts()
        {
            var carts = _unitOfWork.CartRepository.Get(includeProperties: c => c.Items).ToList();

            if (!carts.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    MessageError = "No carts found."
                });
            }

            var response = carts.Select(cart => new CartResponseModel
            {
                CartId = cart.Id,
                Total = cart.Total,
                Items = cart.Items.Select(item => new CartItemModel
                {
                    ProductItemId = item.ProductItemId,
                    Quantity = item.Quantity,
                    ProductName = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Name,
                    Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price,
                    BatchId = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).BatchId
                }).ToList()
            }).ToList();

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Data = response
            });
        }
    }
}
