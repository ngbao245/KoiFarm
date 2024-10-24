using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Data.Entity
{
    [Table("Consignment")]
    public class Consignment : Entity
    {
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public ICollection<ConsignmentItems> Items { get; set; } = new List<ConsignmentItems>();

        // You don't need an OrderId here; consignments don't need to map directly to orders
        // Instead, OrderItems will now link to ConsignmentItems
    }
}
