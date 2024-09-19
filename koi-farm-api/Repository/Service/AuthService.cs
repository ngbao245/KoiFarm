using Microsoft.Extensions.Configuration;
using Repository.Data.Entity;
using Repository.Model.Auth;
using Repository.Repository;
using Repository.Helper;
using System;
using Repository.Model.User;
using Repository.Model;

namespace Repository.Service
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;
        private readonly GenerateToken _generateToken;
        private readonly IConfiguration _configuration;

        public AuthService(
            IGenericRepository<User> userRepository,
            IGenericRepository<UserRefreshToken> userRefreshTokenRepository,
            GenerateToken generateToken,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userRefreshTokenRepository = userRefreshTokenRepository;
            _generateToken = generateToken;
            _configuration = configuration;
        }

        public ResponseTokenModel SignIn(SignInModel signInModel)
        {
            var user = _userRepository.GetSingle(u => u.Email == signInModel.Email);
            if (user == null || user.Password != signInModel.Password)
                throw new UnauthorizedAccessException("Invalid credentials.");

            // Generate and return JWT and refresh tokens
            var token = _generateToken.GenerateTokenModel(user);
            return new ResponseTokenModel
            {
                Token = token.Token,
                RefreshToken = token.RefreshToken,
                User = new ResponseUserModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    Address = user.Address,
                    Phone = user.Phone,
                    RoleId = user.RoleId
                }
            };
        }

        public ResponseModel SignUp(SignUpModel signUpModel)
        {
            var existingUser = _userRepository.GetSingle(u => u.Email == signUpModel.Email);
            if (existingUser != null)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "User with this email already exists."
                };
            }

            var user = new User
            {
                Name = signUpModel.Name,
                Email = signUpModel.Email,
                Password = signUpModel.Password,  // No hashing
                Phone = signUpModel.Phone,
                Address = signUpModel.Address,
                RoleId = signUpModel.RoleId
            };

            _userRepository.Create(user);

            return new ResponseModel
            {
                StatusCode = 201,
                Data = user
            };
        }

        public ResponseTokenModel RefreshToken(string refreshToken)
        {
            var storedToken = _userRefreshTokenRepository.GetSingle(t => t.RefreshToken == refreshToken && !t.isUsed);
            if (storedToken == null || storedToken.ExpireTime < DateTime.Now)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            storedToken.isUsed = true;
            _userRefreshTokenRepository.Update(storedToken);

            var user = _userRepository.GetById(storedToken.User_Id);
            return _generateToken.GenerateTokenModel(user);
        }
    }
}
