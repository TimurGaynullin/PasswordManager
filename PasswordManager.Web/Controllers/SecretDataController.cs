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
    public class SecretDataController : ControllerBase
    {
        private StorageContext db;
        private IPasswordService _passwordService;
        private ISecretDataService _secretDataService;
        private IUserRepository _userRepository;
        private IValidationService _validationService;
        
        public SecretDataController(StorageContext context, IPasswordService passwordService,
            IUserRepository userRepository, IValidationService validationService, ISecretDataService secretDataService)
        {
            db = context;
            _passwordService = passwordService;
            _userRepository = userRepository;
            _validationService = validationService;
            _secretDataService = secretDataService;
        }
        
        [HttpPost("{id}")]
        [Description("Получить секретные данные по id")]
        public async Task<ApiResponse> Get(int id, [FromBody] string masterPassword)
        {
            var currentUserName = User.Identity?.Name;
            var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
            if (userId == null)
                throw new Exception("Пользователь не найден");
            var user = await _userRepository.GetIncludingSecretDataAsync(userId.Value);
            if (!_validationService.LogIn(user, masterPassword))
                throw new Exception("Неправильный мастер-пароль");
            
            var secretDataDto = await _secretDataService.GetSecretData(id, user, masterPassword);
            if (secretDataDto != null)
            {
                return ApiResponse.CreateSuccess(secretDataDto);
            }

            return ApiResponse.CreateFailure("Данные не найдены");
        }
        
        [HttpPost]
        [Description("Получить все данные")]
        public async Task<ApiResponse> Get([FromBody] string masterPassword)
        {
            var currentUserName = User.Identity?.Name;
            var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
            if (userId == null)
                throw new Exception("Пользователь не найден");
            var user = await _userRepository.GetIncludingSecretDataAsync(userId.Value);
            if (!_validationService.LogIn(user, masterPassword))
                throw new Exception("Неправильный мастер-пароль");

            var secretDataDto = await _secretDataService.GetSecretDatas(user, masterPassword);
            return ApiResponse.CreateSuccess(secretDataDto);
        }
        
        [HttpPost("create")]
        [Description("Create")]
        public async Task<ApiResponse> Create(CreateSecretDataDto createSecretDataDto)
        {
            try
            {
                var currentUserName = User.Identity?.Name;
                var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
                if (userId == null)
                    throw new Exception("Пользователь не найден");

                var user = await _userRepository.GetIncludingSecretDataAsync(userId.Value);
                if (!_validationService.LogIn(user, createSecretDataDto.MasterPassword))
                    throw new Exception("Неправильный мастер-пароль");
                
                var secretDataDto = await _secretDataService.CreateSecretData(createSecretDataDto, user, createSecretDataDto.MasterPassword);
                return ApiResponse.CreateSuccess(secretDataDto);
            }
            catch(Exception e)
            {
                return ApiResponse.CreateFailure(e.InnerException == null ? e.Message : e.InnerException.Message);
            }
        }
        
        [HttpPut]
        [Description("Update")]
        public async Task<ApiResponse> Update(CreateSecretDataDto createSecretDataDto)
        {
            var currentUserName = User.Identity?.Name;
            var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
            if (userId == null)
                throw new Exception("Пользователь не найден");

            var user = await _userRepository.GetIncludingSecretDataAsync(userId.Value);
            if (!_validationService.LogIn(user, createSecretDataDto.MasterPassword))
                throw new Exception("Неправильный мастер-пароль");
            
            var secretDataDto = await _secretDataService.UpdateSecretData(createSecretDataDto, user, createSecretDataDto.MasterPassword);
            if (secretDataDto != null)
            {
                return ApiResponse.CreateSuccess(secretDataDto);
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

            var user = await _userRepository.GetIncludingSecretDataAsync(userId.Value);
            try
            {
                var isDeleted = await _secretDataService.DeleteSecretData(id, user);
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
        
        [HttpPost("sharing")]
        [Description("Sharing")]
        public async Task<ApiResponse> Share(SharePasswordRequestDto sharePasswordRequestDto) //надо делать
        {
            try
            {
                var currentUserName = User.Identity?.Name;
                var userId = (await db.Users.FirstOrDefaultAsync(x => x.Login == currentUserName))?.Id;
                if (userId == null)
                    throw new Exception("Пользователь не найден");
                
                var reciverUserId = (await db.Users.FirstOrDefaultAsync(x => x.Id == sharePasswordRequestDto.UserId))?.Id;
                if (reciverUserId == null)
                    throw new Exception("Пользователь-получатель не найден");
                
                var user = await _userRepository.GetIncludingPasswordsAsync(userId.Value);
                var reciverUser = await _userRepository.GetIncludingPasswordsAsync(reciverUserId.Value);
                if (!_validationService.LogIn(user, sharePasswordRequestDto.MasterPassword))
                    throw new Exception("Неправильный мастер-пароль");

                var success = await _passwordService.SharePassword(sharePasswordRequestDto.PasswordId, user,
                    reciverUser, sharePasswordRequestDto.MasterPassword);
                
                return ApiResponse.CreateSuccess();
            }
            catch(Exception e)
            {
                return ApiResponse.CreateFailure(e.InnerException == null ? e.Message : e.InnerException.Message);
            }
        }
    }
}