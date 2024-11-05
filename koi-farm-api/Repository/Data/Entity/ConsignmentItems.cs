using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Data.Entity
{
    [Table("ConsignmentItem")]
    public class ConsignmentItems : Entity
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Origin { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Size { get; set; }
        public string Species { get; set; }

        public bool Checkedout { get; set; } = false; // Indicates if this item has been checked out

        public string Status { get; set; } = "Pending"; // Status: Pending -> Approved -> Checkedout
        public string? ImageUrl { get; set; }
        public string ConsignmentId { get; set; }
        [ForeignKey(nameof(ConsignmentId))]
        public Consignment Consignment { get; set; }

        // Optional link to an OrderItem (when the consignment is checked out)
        public string? OrderItemId { get; set; }
        [ForeignKey(nameof(OrderItemId))]
        public OrderItem? OrderItem { get; set; }

        // Additional fields to align with ProductItem requirements
        public string Personality { get; set; } = "Unknown"; // Default value if not provided
        public string FoodAmount { get; set; } = "Standard"; // Default value if not provided
        public string WaterTemp { get; set; } = "Normal"; // Default value if not provided
        public string MineralContent { get; set; } = "Normal"; // Default value if not provided
        public string PH { get; set; } = "Neutral"; // Default value if not provided
        public string Type { get; set; } = "Default"; // Default value if not provided
    }
}
