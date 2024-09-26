using System;
using System.Collections.Generic;
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
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public string? PromotionId { get; set; }
        [ForeignKey(nameof(PromotionId))]
        public Promotion Promotion { get; set; }

        public ICollection<OrderItem> Items { get; set; }
        // Mới làm entity tới chỗ này thôi

    }
}
