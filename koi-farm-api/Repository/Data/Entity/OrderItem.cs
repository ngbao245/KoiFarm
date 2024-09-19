using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Entity
{
    [Table("OrderItem")]
    public class OrderItem : Entity
    {
        public string OrderID { get; set; }

        [ForeignKey(nameof(OrderID))]
        public Order order { get; set; }

        public string ProductItemId { get; set; }

        [ForeignKey(nameof(ProductItemId))]
        public ProductItem ProductItem { get; set; }
    }
}
