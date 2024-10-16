using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Consignment
{
    public class ConsignmentResponseModel
    {
        public string ConsignmentId { get; set; }
        public string UserId { get; set; }
        public List<ConsignmentItemResponseModel> Items { get; set; }
    }
}
