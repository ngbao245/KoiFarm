using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Entity
{
    [Table("Order")]
    public class Order : Entity
    {
        public decimal Total { get; set; }
        public string Status { get; set; }

        // Foreign key for the user who placed the order
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        // Foreign key for the staff who processed the order
        public string? StaffId { get; set; }
        [ForeignKey("StaffId")]
        public User? Staff { get; set; }    

        public string? PromotionId { get; set; }
        [ForeignKey(nameof(PromotionId))]
        public Promotion? Promotion { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        public bool? IsDelivered { get; set; }

        public ICollection<OrderItem> Items { get; set; }
    }

}
