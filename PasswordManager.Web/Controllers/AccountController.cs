using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Database;
using PasswordManager.Database.Models.Entities;
using PasswordManager.Domain.Abstractions;
using PasswordManager.Web.Controllers.ViewModels;

namespace PasswordManager.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private StorageContext db;
        private IValidationService validationService;
        public AccountController(StorageContext context, IValidationService validationService)
        {
            db = context;
            this.validationService = validationService;
        }
        
        [Authorize]
        [HttpPost("password/change")]
        public async Task<IActionResult> ChangePassword([FromBody]RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users
                    .Include(x => x.Passwords)
                    .FirstOrDefaultAsync(u => u.Login == model.Login);
                if (validationService.ChangingPassword(user, model.Password))
                {
                    await Authenticate(model.Login);
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (validationService.LogIn(user, model.Password))
                {
                    await Authenticate(model.Login);
                    return Ok();
                }
            }
            return StatusCode(401);
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (validationService.Registering(user, model.Login, model.Password))
                {
                    await Authenticate(model.Login); // аутентификация
                    return Ok();
                }
            }
            return NotFound();
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}