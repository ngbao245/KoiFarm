using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.EmailService;
using Repository.ForgotPasswordService;
using Repository.Model;
using Repository.Model.Email;
using Repository.Model.ForgotPassword;
using Repository.Model.ProductItem;
using Repository.Model.User;
using Repository.Repository;
using System.Security.Cryptography;
using System.Text;

namespace koi_farm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;


        public UserController(UnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _emailService = serviceProvider.GetRequiredService<IEmailService>();
            _configuration = configuration;
        }

        [HttpGet("get-all-users")]
        //[Authorize]
        public IActionResult GetAllUsers()
        {
            var users = _unitOfWork.UserRepository.GetAll();

            if (!users.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No users found."
                });
            }

            var responseUsers = _mapper.Map<List<ResponseUserModel>>(users);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseUsers
            });
        }

        [HttpGet("get-users-by-role/{role}")]
        //[Authorize]
        public IActionResult GetUsersByRole(string role, int pageIndex = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(role))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Role cannot be null or empty."
                });
            }

            var users = _unitOfWork.UserRepository.GetAll().Where(u => u.RoleId.Equals(role));

            if (!users.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = $"No users found with role: {role}."
                });
            }

            var totalItems = users.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedUsers = users
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var responseUsers = _mapper.Map<List<ResponseUserModel>>(pagedUsers);

            var responseSearchModel = new ResponseSearchModel<ResponseUserModel>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Entities = responseUsers
            };

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseSearchModel
            });
        }

        [HttpGet("get-user/{id}")]
        public IActionResult GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "UserId cannot be null or empty."
                });
            }

            var user = _unitOfWork.UserRepository.GetById(id);

            if (user == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = $"User with ID: {id} not found."
                });
            }

            //user.Role = _unitOfWork.RoleRepository.GetAll().FirstOrDefault(r => r.Id.Equals(user.RoleId));

            var responseUser = _mapper.Map<ResponseUserModel>(user);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseUser
            });
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("create-user-staff")]
        public IActionResult CreateUserStaff([FromBody] RequestCreateUserModel responseCreateUser)
        {
            if (responseCreateUser == null || string.IsNullOrEmpty(responseCreateUser.Email) || string.IsNullOrEmpty(responseCreateUser.Password))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid user data. Email and password are required."
                });
            }

            var existingUser = _unitOfWork.UserRepository.GetSingle(u => u.Email == responseCreateUser.Email);
            if (existingUser != null)
            {
                return Conflict(new ResponseModel
                {
                    StatusCode = 409,
                    MessageError = "Email already exists."
                });
            }

            var user = _mapper.Map<User>(responseCreateUser);

            user.RoleId = "2";

            _unitOfWork.UserRepository.Create(user);

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = user
            });
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("update-user/{id}")]
        public IActionResult UpdateUser(string id, [FromBody] RequestCreateUserModel updateUserModel)
        {
            if (string.IsNullOrEmpty(id) || updateUserModel == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "UserId and user data cannot be null or empty."
                });
            }

            var user = _unitOfWork.UserRepository.GetById(id);

            if (user == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = $"User with Id: {id} not found."
                });
            }

            _mapper.Map(updateUserModel, user);

            _unitOfWork.UserRepository.Update(user);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = user
            });
        }

        [HttpPut("update-my-user")]
        [Authorize]
        public IActionResult UpdateMyUser([FromBody] RequestCreateUserModel updateUserModel)
        {
            if (updateUserModel == null)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "User data cannot be null or empty."
                });
            }

            var userId = User.FindFirst("UserID")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized: UserId not found."
                });
            }

            var user = _unitOfWork.UserRepository.GetById(userId);

            if (user == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = $"User with Id: {userId} not found."
                });
            }

            _mapper.Map(updateUserModel, user);

            _unitOfWork.UserRepository.Update(user);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = user
            });
        }


        [Authorize(Roles = "Manager")]
        [HttpDelete("delete-user/{id}")]
        public IActionResult DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "UserId cannot be null or empty."
                });
            }

            var user = _unitOfWork.UserRepository.GetById(id);

            if (user == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = $"User with Id: {id} not found."
                });
            }

            _unitOfWork.UserRepository.Delete(user);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = $"User with ID {id} successfully deleted."
            });
        }

        [HttpGet("get-my-user")]
        [Authorize]
        public IActionResult GetMyUser()
        {
            var userId = User.FindFirst("UserID")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized: UserId not found in claims."
                });
            }

            var user = _unitOfWork.UserRepository.GetById(userId);

            if (user == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = $"User with Id: {userId} not found."
                });
            }

            var responseUser = _mapper.Map<ResponseUserModel>(user);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseUser
            });
        }

        [HttpPost("request-password-reset")]
        public IActionResult RequestPasswordReset([FromBody] RequestPasswordResetModel model)
        {
            var user = _unitOfWork.UserRepository.GetAll().FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "User not found!"
                });
            }

            var token = _tokenService.GenerateToken(model.Email);
            var frontendUrl = _configuration["FrontEndPort:PaymentUrl"];

            // Send reset email
            var resetUrl = $"{frontendUrl}/reset-password?token={token}&email={model.Email}";
            _emailService.SendMail(new SendMailModel
            {
                ReceiveAddress = model.Email,
                Title = "Password Reset Request",
                Content = $"Click the link to reset your password: {resetUrl}"
            });

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = "Successfully request password reset"
            });
        }


        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!_tokenService.ValidateToken(model.Email, model.Token)) return BadRequest("Invalid or expired token");

            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest(new ResponseModel { StatusCode = 400, MessageError = "Email is required!" });
            }

            var user = _unitOfWork.UserRepository.GetAll().FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "User not found!"
                });
            }

            user.Password = model.NewPassword;
            _unitOfWork.UserRepository.Update(user);
            _tokenService.RemoveToken(model.Email);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                MessageError = "New password updated successfully"
            });
        }

    }
}