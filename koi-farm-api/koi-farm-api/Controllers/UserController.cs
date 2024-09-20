using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Model;
using Repository.Model.User;
using Repository.Repository;

namespace koi_farm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        [HttpGet("users-by-{role}")]
        //[Authorize]
        public IActionResult GetUsersByRole(string role)
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

            var responseUsers = _mapper.Map<List<ResponseUserModel>>(users);

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responseUsers
            });
        }

        [HttpGet("{id}")]
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

        [HttpPost("create-user-staff")]
        public IActionResult CreateUserStaff([FromBody] ResponseCreateUserModel responseCreateUser)
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


        [HttpDelete("{id}")]
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

    }
}