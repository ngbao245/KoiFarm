using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Entity
{
    [Table("Consignment")]
    public class Consignment : Entity
    {
        public string UserId {  get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public ICollection<ConsignmentItems> Items { get; set; }
    }
}
