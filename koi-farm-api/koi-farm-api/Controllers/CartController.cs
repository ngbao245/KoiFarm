using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Model.Cart;
using Repository.Model;
using Repository.Service.Interface;

namespace koi_farm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Add to Cart Endpoint
        [HttpPost("add-to-cart")]
        public IActionResult AddToCart([FromBody] CartRequestModel requestModel)
        {
            var response = _cartService.AddToCart(requestModel);

            if (response == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    MessageError = "Failed to add item to cart."
                });
            }

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Data = response
            });
        }

        // Remove from Cart Endpoint
        [HttpDelete("remove-from-cart/{cartId}")]
        public IActionResult RemoveFromCart(string cartId)
        {
            _cartService.RemoveFromCart(cartId);

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                MessageError = null,
                Data = "Cart removed successfully."
            });
        }

        // Update Cart Item Endpoint
        [HttpPut("update-cart-item/{cartId}/{productItemId}")]
        public IActionResult UpdateCartItem(string cartId, string productItemId, [FromBody] int quantity)
        {
            _cartService.UpdateCartItem(cartId, productItemId, quantity);

            return Ok(new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                MessageError = null,
                Data = "Cart item updated successfully."
            });
        }
    }

}
