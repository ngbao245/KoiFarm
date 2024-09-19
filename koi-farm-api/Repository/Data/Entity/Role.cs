using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Data.Entity
{
    [Table("Role")]
    public class Role : Entity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        // Navigation property for Users
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
