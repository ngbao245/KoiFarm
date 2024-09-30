using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Payment
{
    public class PaymentRequestModel
    {
        public string OrderDescription { get; set; }
        public string OrderType { get; set; }
        public string Name { get; set; }
        public string OrderId { get; set; }
    }
}
