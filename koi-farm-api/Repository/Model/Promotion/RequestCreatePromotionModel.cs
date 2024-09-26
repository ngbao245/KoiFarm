using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Promotion
{
    public class RequestCreatePromotionModel
    {
        public string Code { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; }
    }
}
