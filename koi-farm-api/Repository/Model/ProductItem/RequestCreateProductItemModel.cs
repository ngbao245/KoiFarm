using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.ProductItem
{
    public class RequestCreateProductItemModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Origin { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }

        public string Size { get; set; }

        public string Species { get; set; }

        public string Personality { get; set; }

        public string FoodAmount { get; set; }

        public string WaterTemp { get; set; }

        public string MineralContent { get; set; }

        public string PH { get; set; }

        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        public string Type { get; set; }

        public string ProductId { get; set; }
    }
}
