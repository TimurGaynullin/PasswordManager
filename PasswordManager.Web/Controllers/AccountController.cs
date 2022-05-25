using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Contracts;
using PasswordManager.Database;
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
        private ISecretDataService _secretDataService;
        private IUserRepository _userRepository;
        public AccountController(StorageContext context, IValidationService validationService,
            IUserRepository userRepository, ISecretDataService secretDataService)
        {
            db = context;
            this.validationService = validationService;
            _userRepository = userRepository;
            _secretDataService = secretDataService;
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
                    user = await _userRepository.GetIncludingSecretDataAsync(user.Id);
                    var success = await _secretDataService.RecieveSharingSecretDatas(user, model.Password);
                    if (success)
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
                    await Authenticate(model.Login);
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

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
        
        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> Users()
        {
            var users = await db.Users.ToListAsync();
            var response = new List<UserDto>();
            foreach (var user in users)
            {
                var userResponse = new UserDto
                {
                    Id = user.Id,
                    Login = user.Login
                };
                response.Add(userResponse);
            }
            
            return Ok(response);
        }
    }
}