using Repository.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Auth
{
    public class ResponseTokenModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public ResponseUserModel User { get; set; }
    }
}
