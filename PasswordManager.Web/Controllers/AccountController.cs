﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private IPasswordService _passwordService;
        private IUserRepository _userRepository;
        public AccountController(StorageContext context, IValidationService validationService,
            IPasswordService passwordService, IUserRepository userRepository)
        {
            db = context;
            this.validationService = validationService;
            _passwordService = passwordService;
            _userRepository = userRepository;
        }
        
        [Authorize]
        [HttpPost("password/change")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
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
                    user = await _userRepository.GetIncludingPasswordsAsync(user.Id);
                    var success = await _passwordService.RecieveSharingPasswords(user, model.Password);
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
        
        
    }
}