using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Contracts;
using PasswordManager.Database;
using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private StorageContext db;
        //private IValidationService validationService;
        private IPasswordService _passwordService;
        private IUserRepository _userRepository;
        
        public PasswordController(StorageContext context, IValidationService validationService, IPasswordService passwordService, IUserRepository userRepository)
        {
            db = context;
            //this.validationService = validationService;
            _passwordService = passwordService;
            _userRepository = userRepository;
        }
        
        [HttpGet("{id}")]
        [Description("Get")]
        public async Task<ApiResponse> Get(int id)
        {
            var currentUserName = User.Identity?.Name;
            var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
            if (userId == null)
                throw new Exception("Пользователь не найден");

            var user = await _userRepository.GetIncludingPasswordsAsync(userId.Value);
            var passwordDto = await _passwordService.GetPassword(id, user);
            if (passwordDto != null)
            {
                return ApiResponse.CreateSuccess(passwordDto);
            }

            return ApiResponse.CreateFailure("Пароль не найден");
        }
        
        [HttpGet]
        [Description("Get")]
        public async Task<ApiResponse> Get()
        {
            var currentUserName = User.Identity?.Name;
            var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
            if (userId == null)
                throw new Exception("Пользователь не найден");

            var user = await _userRepository.GetIncludingPasswordsAsync(userId.Value);
            var passwordsDto = await _passwordService.GetPasswords(user);
            return ApiResponse.CreateSuccess(passwordsDto);
        }
        
        [HttpPost]
        [Description("Create")]
        public async Task<ApiResponse> Create(PasswordDto passwordDto)
        {
            try
            {
                var currentUserName = User.Identity?.Name;
                var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
                if (userId == null)
                    throw new Exception("Пользователь не найден");

                var user = await _userRepository.GetIncludingPasswordsAsync(userId.Value);
                passwordDto = await _passwordService.CreatePassword(passwordDto, user);
                return ApiResponse.CreateSuccess(passwordDto);
            }
            catch(Exception e)
            {
                return ApiResponse.CreateFailure(e.InnerException == null ? e.Message : e.InnerException.Message);
            }
        }
        
        [HttpPut]
        [Description("Update")]
        public async Task<ApiResponse> Update(PasswordDto passwordDto)
        {
            var currentUserName = User.Identity?.Name;
            var currentUser = await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName);
            passwordDto = await _passwordService.UpdatePassword(passwordDto, currentUser);
            if (passwordDto != null)
            {
                return ApiResponse.CreateSuccess(passwordDto);
            }
            return ApiResponse.CreateFailure();
        }
        
        [HttpDelete("{id}")]
        [Description("Delete")]
        public async Task<ApiResponse> Delete(int id)
        {
            var currentUserName = User.Identity?.Name;
            var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
            if (userId == null)
                throw new Exception("Пользователь не найден");

            var user = await _userRepository.GetIncludingPasswordsAsync(userId.Value);
            try
            {
                var isDeleted = await _passwordService.DeletePassword(id, user);
                if (isDeleted)
                {
                    return ApiResponse.CreateSuccess("Удалено");
                }
                return ApiResponse.CreateFailure();
            }
            catch(Exception e)
            {
                return ApiResponse.CreateFailure(e.Message);
            }
			
        }
        
    }
}