using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Batch
{
    public class BatchItemResponseModel
    {
        public string BatchItemId { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }

        public int Age { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
    }
}
