using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Data.Entity
{
    [Table("User")]
    public class User : Entity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Blog> Blogs { get; set; }


    }
}
