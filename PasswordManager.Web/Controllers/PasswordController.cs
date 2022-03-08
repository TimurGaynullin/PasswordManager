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
        private IPasswordService _passwordService;
        private IUserRepository _userRepository;
        private IValidationService _validationService;
        
        public PasswordController(StorageContext context, IPasswordService passwordService,
            IUserRepository userRepository, IValidationService validationService)
        {
            db = context;
            _passwordService = passwordService;
            _userRepository = userRepository;
            _validationService = validationService;
        }
        
        [HttpPost("{id}")]
        [Description("Получить пароль по id")]
        public async Task<ApiResponse> Get(int id, [FromBody] string masterPassword)
        {
            var currentUserName = User.Identity?.Name;
            var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
            if (userId == null)
                throw new Exception("Пользователь не найден");
            var user = await _userRepository.GetIncludingPasswordsAsync(userId.Value);
            if (!_validationService.LogIn(user, masterPassword))
                throw new Exception("Неправильный мастер-пароль");
            var passwordDto = await _passwordService.GetPassword(id, user, masterPassword);
            if (passwordDto != null)
            {
                return ApiResponse.CreateSuccess(passwordDto);
            }

            return ApiResponse.CreateFailure("Пароль не найден");
        }
        
        [HttpPost]
        [Description("Получить все пароли")]
        public async Task<ApiResponse> Get([FromBody] string masterPassword)
        {
            var currentUserName = User.Identity?.Name;
            var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
            if (userId == null)
                throw new Exception("Пользователь не найден");
            var user = await _userRepository.GetIncludingPasswordsAsync(userId.Value);
            if (!_validationService.LogIn(user, masterPassword))
                throw new Exception("Неправильный мастер-пароль");
            var passwordsDto = await _passwordService.GetPasswords(user, masterPassword);
            return ApiResponse.CreateSuccess(passwordsDto);
        }
        
        [HttpPost("create")]
        [Description("Create")]
        public async Task<ApiResponse> Create(CreatePasswordDto createPasswordDto)
        {
            try
            {
                var currentUserName = User.Identity?.Name;
                var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
                if (userId == null)
                    throw new Exception("Пользователь не найден");

                var user = await _userRepository.GetIncludingPasswordsAsync(userId.Value);
                if (!_validationService.LogIn(user, createPasswordDto.MasterPassword))
                    throw new Exception("Неправильный мастер-пароль");

                var passwordDto = new PasswordDto
                {
                    Id = createPasswordDto.Id,
                    Login = createPasswordDto.Login,
                    Name = createPasswordDto.Name,
                    Value = createPasswordDto.Value
                };
                passwordDto = await _passwordService.CreatePassword(passwordDto, user, createPasswordDto.MasterPassword);
                return ApiResponse.CreateSuccess(passwordDto);
            }
            catch(Exception e)
            {
                return ApiResponse.CreateFailure(e.InnerException == null ? e.Message : e.InnerException.Message);
            }
        }
        
        [HttpPut]
        [Description("Update")]
        public async Task<ApiResponse> Update(CreatePasswordDto createPasswordDto)
        {
            var currentUserName = User.Identity?.Name;
            var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
            if (userId == null)
                throw new Exception("Пользователь не найден");

            var user = await _userRepository.GetIncludingPasswordsAsync(userId.Value);
            if (!_validationService.LogIn(user, createPasswordDto.MasterPassword))
                throw new Exception("Неправильный мастер-пароль");
            var passwordDto = new PasswordDto
            {
                Id = createPasswordDto.Id,
                Login = createPasswordDto.Login,
                Name = createPasswordDto.Name,
                Value = createPasswordDto.Value
            };
            
            passwordDto = await _passwordService.UpdatePassword(passwordDto, user, createPasswordDto.MasterPassword);
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