using MertBayraktar.Social.Network.Api.DTOs;
using MertBayraktar.Social.Network.Api.Entities;
using MertBayraktar.Social.Network.Api.Generators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MertBayraktar.Social.Network.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public AccController(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
                return BadRequest("Kullanıcı zaten var");

            User newUser = new User
            {
                UserName = model.Username,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
                return BadRequest("Kullnanıcı oluşturulken hata oluştu");
            else
                return Ok(model);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(login.Username);

            if (user == null) return BadRequest("Kullanıcı adı yanlış");

            var result = await _userManager.CheckPasswordAsync(user, login.Password.Trim());

            if (result)
            {
                TokenGenerator tokenGenerator = new TokenGenerator(_configuration);
                var token = tokenGenerator.GetToken(user);
                return Ok(token);
                
            }
            
            return Unauthorized();

        }

    }
}
