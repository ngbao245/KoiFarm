using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ForgotPasswordService
{
    public interface ITokenService
    {
        string GenerateToken(string email);
        bool ValidateToken(string email, string token);
        void RemoveToken(string email);
    }
}
