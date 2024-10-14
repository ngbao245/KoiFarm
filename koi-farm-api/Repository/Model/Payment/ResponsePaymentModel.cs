using Repository.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Payment
{
    public class ResponsePaymentModel
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Method { get; set; }
        public DateTimeOffset CreatedTime { get; set; }

        public string OrderId { get; set; }
        public OrderResponseModel Order { get; set; }
    }
}
