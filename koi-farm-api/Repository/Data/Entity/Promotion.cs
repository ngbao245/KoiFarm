using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Entity
{
    [Table("Promotion")]
    public class Promotion : Entity
    {
        public string Code { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
