using Repository.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Consignment
{
    public class CheckoutConsignmentResponseModel
    {
        public string OrderId { get; set; }
        public decimal Total { get; set; }
        public List<OrderItemResponseModel> Items { get; set; }
    }
}
