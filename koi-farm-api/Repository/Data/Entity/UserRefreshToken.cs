using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Entity
{

    [Table("UserRefreshToken")]
    public class UserRefreshToken : Entity
    {
        public string User_Id { get; set; }
        [ForeignKey(nameof(User_Id))]
        public User UserEntity { get; set; }
        public string RefreshToken { get; set; }
        public string JwtId { get; set; }
        public bool isUsed { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
