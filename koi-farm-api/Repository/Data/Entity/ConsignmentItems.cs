using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Entity
{
    [Table("ConsignmentItem")]
    public class ConsignmentItems
    {
        public string? Name { get; set; }

        public string? Category { get; set; }

        public string? Origin { get; set; }

        public string? Sex { get; set; }

        public int? Age { get; set; }

        public string? Size { get; set; }

        public string? Species { get; set; }
        public bool Checkedout { get; set; }
        public string status { get; set; }

    }
}
