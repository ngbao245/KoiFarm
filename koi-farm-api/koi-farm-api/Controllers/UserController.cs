using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
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

        [HttpGet("all-users")]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            var users = _unitOfWork.UserRepository.GetAll();

            var responseUsers = _mapper.Map<List<ResponseUserModel>>(users);

            return Ok(responseUsers);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var user = _unitOfWork.UserRepository.GetById(id);
            user.Role = _unitOfWork.RoleRepository.Get().FirstOrDefault(r => r.Id.Equals(user.RoleId));

            if (user == null)
            {
                return NotFound();
            }

            var responseUser = _mapper.Map<ResponseUserModel>(user);



            return Ok(user);
        }


    }
}