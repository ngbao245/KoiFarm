using Repository.Model.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Service.Interface
{
    public interface ICartService
    {
        CartResponseModel AddToCart(CartRequestModel requestModel);
        void RemoveFromCart(string cartId);
        void UpdateCartItem(string cartId, string productItemId, int quantity);
    }

}
