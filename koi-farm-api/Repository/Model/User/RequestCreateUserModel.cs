using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.User
{
    public class RequestCreateUserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
