using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Cart
{
    public class CartRequestModel
    {
        public int Quantity { get; set; }
        public string ProductItemId { get; set; }
    }

}
