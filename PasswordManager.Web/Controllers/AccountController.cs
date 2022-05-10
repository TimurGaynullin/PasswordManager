using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PasswordManager.Contracts;
using PasswordManager.Database;

using PasswordManager.Web.Controllers.ViewModels;

namespace PasswordManager.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        public AccountController()
        {
        }
        /*
        [Authorize]
        [HttpPost("password/change")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model) //TODO
        {
            if (ModelState.IsValid)
            {
                var currentUserName = User.Identity?.Name;
                if (currentUserName == null) return BadRequest();
                var user = await db.Users
                    .Include(x => x.Passwords)//Когда появятся другие секретные данные, подтянуть все и перешифровать
                    .FirstOrDefaultAsync(u => u.Login == currentUserName);
                if (validationService.ChangingPassword(user, model.OldPassword, model.NewPassword))
                {
                    await Authenticate(currentUserName);
                    return Ok();
                }
            }
            return NotFound();
        }*/

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return Ok();
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            return Ok();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            return Ok();
        }
        
        //[Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> Users()
        {
            var testResponse = new List<UserDto>();
            testResponse.Add(new UserDto
            {
                Id = 1,
                Login = "Alice"
                
            });
            testResponse.Add(new UserDto
            {
                Id = 2,
                Login = "Bob"
            });
            return Ok(testResponse);
        }
    }
}