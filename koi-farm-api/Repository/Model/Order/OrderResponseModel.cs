using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Order
{
    public class OrderResponseModel
    {
        public string OrderId { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public string StaffId { get; set; }
        public List<OrderItemResponseModel> Items { get; set; }
    }
}
