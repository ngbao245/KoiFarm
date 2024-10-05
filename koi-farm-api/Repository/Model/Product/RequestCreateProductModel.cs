using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Product
{
    public class RequestCreateProductModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }
}
