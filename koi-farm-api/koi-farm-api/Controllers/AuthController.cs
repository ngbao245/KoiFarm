using Azure;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Helper;
using Repository.Model;
using Repository.Model.Auth;
using Repository.Model.User;
using Repository.Repository;
using System;

namespace Repository.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly GenerateToken _generateToken;

        public AuthController(
            UnitOfWork unitOfWork,
            GenerateToken generateToken,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _generateToken = generateToken;
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInModel signInModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == signInModel.Email);
                if (user == null || user.Password != signInModel.Password)
                    return Unauthorized("Invalid credentials.");

                var token = _generateToken.GenerateTokenModel(user);
                return Ok(new ResponseModel
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = new ResponseTokenModel
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
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] SignUpModel signUpModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingUser = _unitOfWork.UserRepository.GetSingle(u => u.Email == signUpModel.Email);
                if (existingUser != null)
                    return Conflict("User with this email already exists.");

                var user = new User
                {
                    Name = signUpModel.Name,
                    Email = signUpModel.Email,
                    Password = signUpModel.Password,
                    Phone = signUpModel.Phone,
                    Address = signUpModel.Address,
                    RoleId = "0"
                };

                _unitOfWork.UserRepository.Create(user);

                return Created("User created", new ResponseModel
                {
                    StatusCode = 201,
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh token is required");

            try
            {
                var storedToken = _unitOfWork.UserRefreshTokenRepository.GetSingle(t => t.RefreshToken == refreshToken && !t.isUsed);
                if (storedToken == null || storedToken.ExpireTime < DateTime.Now)
                    return Unauthorized("Invalid or expired refresh token.");

                storedToken.isUsed = true;
                _unitOfWork.UserRefreshTokenRepository.Update(storedToken);

                var user = _unitOfWork.UserRepository.GetById(storedToken.User_Id);
                var newToken = _generateToken.GenerateTokenModel(user);

                return Ok(newToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
