using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Repository.Data.Entity
{
    [Table("ProductItem")]
    public class ProductItem : Entity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Origin { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }

        public string Size { get; set; }

        public string Species { get; set; }

        public string Personality { get; set; }

        public string FoodAmount { get; set; } 

        public string WaterTemp { get; set; }

        public string MineralContent { get; set; } 

        public string PH { get; set; } 

        public string ImageUrl { get; set; } 

        public int Quantity { get; set; }

        public string Type { get; set; }

        public string ProductId { get; set; }

        // Foreign key for Product entity
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public ICollection<ProductCertificate> productCertificates { get; set; }
        public ICollection<CartItem> cartItems { get; set; }
        public ICollection<OrderItem> orderItems { get; set; }

        public ICollection<Review> Reviews { get; set; }

        [AllowNull]
        public string? BatchId { get; set; }

        // Foreign key for Product entity
        [ForeignKey(nameof(BatchId))]
        public Batch? Batch { get; set; }


    }
}
