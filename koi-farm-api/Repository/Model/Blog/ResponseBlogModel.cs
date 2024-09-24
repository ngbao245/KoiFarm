using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Blog
{
    public class ResponseBlogModel
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
