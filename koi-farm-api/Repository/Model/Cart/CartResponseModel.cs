using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Cart
{
    public class CartResponseModel
    {
        public string CartId { get; set; }
        public List<CartItemModel> Items { get; set; }
        public decimal Total { get; set; }
    }

}
