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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
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
                //nho check quantity cua productItem

                cartItem.Quantity += requestModel.Quantity;
                if(cartItem.Quantity > productItem.Quantity) {
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

            cart.Total = cart.Items.Sum(item => item.Quantity * productItem.Price);
            _unitOfWork.SaveChange();

            var response = new CartResponseModel
            {
                CartId = cart.Id,
                Total = cart.Total,
                Items = cart.Items.Select(item => new CartItemModel
                {
                    ProductItemId = item.ProductItemId,
                    Quantity = item.Quantity,
                    ProductName = productItem.Name,
                    Price = productItem.Price
                }).ToList()
            };

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Data = response
            });
        }

        // Remove from Cart Endpoint
        [HttpDelete("remove-from-cart/{cartId}")]
        [Authorize]
        public IActionResult RemoveFromCart(string cartId)
        {
            var cart = _unitOfWork.CartRepository.GetById(cartId);
            if (cart == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    MessageError = "Cart not found."
                });
            }

            _unitOfWork.CartRepository.Delete(cart);
            _unitOfWork.SaveChange();

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Data = "Cart removed successfully."
            });
        }

        // Update Cart Item Endpoint
        [HttpPut("update-cart-item/{cartId}/{productItemId}")]
        public IActionResult UpdateCartItem(string cartId, string productItemId, [FromBody] int quantity)
        {
            var cart = _unitOfWork.CartRepository.GetSingle(c => c.Id == cartId, c => c.Items);
            if (cart == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    MessageError = "Cart not found."
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
            _unitOfWork.SaveChange();

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Data = "Cart item updated successfully."
            });
        }
    }
}
