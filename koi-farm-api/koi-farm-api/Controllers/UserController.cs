using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Repository;

namespace koi_farm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserController(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("all-users")]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }
    }
}