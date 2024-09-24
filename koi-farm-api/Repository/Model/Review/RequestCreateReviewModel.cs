using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Review
{
    public class RequestCreateReviewModel
    {
        public int Rating { get; set; }
        public string Description { get; set; }

        public string ProductItemId { get; set; }
    }
}
