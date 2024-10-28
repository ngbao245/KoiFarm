using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Data.Entity;
using Repository.Model.Auth;
using Repository.Repository;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Repository.Helper
{
    public class GenerateToken
    {
        private readonly IConfiguration _configuration;
        private readonly UnitOfWork _unitOfWork;

        public GenerateToken(IConfiguration configuration, UnitOfWork unitOfWork)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public ResponseTokenModel GenerateTokenModel(User userEntity)
        {
            if (userEntity == null)
                throw new ArgumentNullException(nameof(userEntity), "User entity cannot be null.");

            if (string.IsNullOrEmpty(userEntity.Id))
                throw new ArgumentException("User ID cannot be null or empty");

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? throw new ArgumentNullException("JWT:Secret is not configured"));

            // Manually generate a JwtId as a GUID
            var jwtId = Guid.NewGuid().ToString();

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserID", userEntity.Id),
                    new Claim(ClaimTypes.Role, userEntity.Role.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, jwtId)
                }),
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JWT:TokenExpirationInDays"] ?? "1")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature),
            };

            // Create the JWT token
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);

            // Generate a refresh token
            var refreshToken = GenerateRefreshToken();

            // Create the UserRefreshToken object with the manually generated JwtId
            var tokenEntity = new UserRefreshToken
            {
                User_Id = userEntity.Id,
                RefreshToken = refreshToken,
                JwtId = jwtId,
                isUsed = false,
                CreateTime = DateTime.Now,
                ExpireTime = DateTime.Now.AddMonths(Convert.ToInt32(_configuration["JWT:RefreshTokenExpirationInMonths"] ?? "6")),
            };

            try
            {
                _unitOfWork.UserRefreshTokenRepository.Create(tokenEntity);
                _unitOfWork.SaveChange(); // Save changes using UnitOfWork
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to store the refresh token", ex);
            }

            return new ResponseTokenModel
            {
                Token = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
    }
}
