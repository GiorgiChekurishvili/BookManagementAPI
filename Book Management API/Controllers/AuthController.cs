using Book_Management_API.DataAccess;
using Book_Management_API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepisotory _repository;
        public AuthController(IAuthRepisotory repisotory)
        {
            _repository = repisotory;
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDTO userLogin)
        {
            var token = await _repository.Login(userLogin.Email, userLogin.Password);
            if(token == null)
            {
                return BadRequest("User doesnt exist");
            }
            return Ok(token);
        }
        [HttpPost("register")]
        public async Task<ActionResult<int>> Register(UserRegisterDTO userRegister)
        {
            var idOfUser = await _repository.Register(userRegister.Email, userRegister.Password);
            if(idOfUser == -1)
            {
                return BadRequest("User already exists");
            }
            return Ok(idOfUser);
        }
    }
}
