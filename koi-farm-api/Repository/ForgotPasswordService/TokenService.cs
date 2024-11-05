using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ForgotPasswordService
{
    public class TokenService : ITokenService
    {
        private readonly Dictionary<string, (string token, DateTime expiry)> _tokens = new();

        public string GenerateToken(string email)
        {
            var token = Guid.NewGuid().ToString();
            _tokens[email] = (token, DateTime.UtcNow.AddMinutes(10)); // 10-minute expiry
            return token;
        }

        public bool ValidateToken(string email, string token)
        {
            if (_tokens.TryGetValue(email, out var value) && value.token == token)
            {
                if (DateTime.UtcNow <= value.expiry)
                {
                    return true;
                }
                _tokens.Remove(email); // Expired token cleanup
            }
            return false;
        }

        public void RemoveToken(string email) => _tokens.Remove(email);
    }
}
