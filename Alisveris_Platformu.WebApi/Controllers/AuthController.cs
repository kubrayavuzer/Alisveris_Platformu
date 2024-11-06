using Alisveris_Platformu.Business.Operations.User;
using Alisveris_Platformu.Business.Operations.User.Dtos;
using Alisveris_Platformu.WebApi.Jwt;
using Alisveris_Platformu.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineAlışverişPlatformu.WebApi.Jwt;
using System.IdentityModel.Tokens.Jwt;

namespace Alisveris_Platformu.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addUserDto = new AddUserDto
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await _userService.AddUser(addUserDto);

            if (result.IsSucceed)
                return Ok();

            else
                return BadRequest(result.Message);


        }

        [HttpPost("login")]
        public IActionResult Login(Models.LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }//ilerde action filter olarak kodlanacak

            var result = _userService.LoginUser(new LoginUserDto
            {
                Email = request.Email,
                Password = request.Password
            });

            if (!result.IsSucceed)
                return BadRequest(result.Message);

            var user = result.Data;

            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)

            });

            return Ok(new LoginResponse
            {
                Message = "Giriş tamamlandı.",
                Token = token

            });
        }

        [HttpGet("me")]
        [Authorize]

        public IActionResult GetMyUser()
        {

            return Ok();
        }

    }

}
