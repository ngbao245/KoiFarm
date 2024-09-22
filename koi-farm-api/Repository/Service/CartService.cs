using Microsoft.AspNetCore.Http;
using Repository.Repository;
using Repository.Service.Interface;
using Repository.Data.Entity;
using Repository.Model.Cart;
using System.Linq;

namespace Repository.Service
{
    public class CartService : ICartService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public CartResponseModel AddToCart(CartRequestModel requestModel)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst("UserID")?.Value;

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

            if (requestModel.Quantity > productItem.Quantity)
            {
            }

            var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductItemId == requestModel.ProductItemId);
            if (cartItem != null)
            {
                cartItem.Quantity += requestModel.Quantity;
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

            return new CartResponseModel
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
        }

        public void RemoveFromCart(string cartId)
        {
            var cart = _unitOfWork.CartRepository.GetById(cartId);
            if (cart != null)
            {
                _unitOfWork.CartRepository.Delete(cart);
                _unitOfWork.SaveChange();
            }
        }

        public void UpdateCartItem(string cartId, string productItemId, int quantity)
        {
            var cart = _unitOfWork.CartRepository.GetSingle(c => c.Id == cartId, c => c.Items);
            if (cart == null)
            {
                return;
            }

            var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductItemId == productItemId);
            if (cartItem == null)
            {
                return;
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
        }
    }
}
