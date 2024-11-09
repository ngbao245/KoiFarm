using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Order
{
    public class CreateOrderRequestModel
    {
        public string CartId { get; set; }

        public string? PromotionCode { get; set; }
    }
}
