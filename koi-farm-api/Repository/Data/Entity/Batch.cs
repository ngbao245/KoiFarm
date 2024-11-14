using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Entity
{
    [Table("Batch")]
    public class Batch : Entity
    {
        public string name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public virtual ICollection<ProductItem> batchItems { get; set; }

        //[AllowNull]
        //public string? ProductId { get; set; }

        //[ForeignKey(nameof(ProductId))]
        //public Product? Product { get; set; }
    }
}
