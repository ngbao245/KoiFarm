using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Data.Entity
{
    [Table("OrderItem")]
    public class OrderItem : Entity
    {
        public string OrderID { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(OrderID))]
        public Order Order { get; set; }

        public string? ProductItemId { get; set; }
        [ForeignKey(nameof(ProductItemId))]
        public ProductItem? ProductItem { get; set; }

        // Link to consignment item if the order item is generated from one
        public string? ConsignmentItemId { get; set; }
        [ForeignKey(nameof(ConsignmentItemId))]
        public ConsignmentItems? ConsignmentItem { get; set; }
    }
}
