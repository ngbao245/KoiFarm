using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Order
{
    public class OrderItemResponseModel
    {
        public string ProductItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? BatchId { get; set; }

    }
}
