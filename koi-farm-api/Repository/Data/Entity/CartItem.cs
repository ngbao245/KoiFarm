using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Entity
{
    [Table("CartItem")]
    public class CartItem : Entity
    {
        public int Quantity { get; set; }
        public string CartId { get; set; }

        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }

        public string ProductItemId { get; set; }

        [ForeignKey(nameof(ProductItemId))]
        public ProductItem ProductItem { get; set; }
    }
}
