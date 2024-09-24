using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Review
{
    public class ResponseReviewModel
    {
        public string Id { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }

        public string ProductItemId { get; set; }
        public string UserId { get; set; }
    }
}
