using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Consignment
{
    public class ConsignmentItemResponseModel
    {
        public string ItemsId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Origin { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Size { get; set; }
        public string Species { get; set; }
        public string Status { get; set; }
    }

}
