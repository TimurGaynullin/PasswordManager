using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PasswordManager.Contracts;
using PasswordManager.Database;

namespace PasswordManager.Web.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SecretDataController : ControllerBase
    {
        
        
        public SecretDataController()
        {
        }
        
        [HttpPost("{id}")]
        [Description("Получить секретные данные по id")]
        public async Task<ApiResponse> Get(int id, [FromBody] string masterPassword)
        {
            var fields = new Dictionary<string, string>();
                fields.Add("Фамилия", "Бобин");
                fields.Add("Имя", "Боб");
                fields.Add("Отчество", "Бобович");
                fields.Add("Серия", "9200");
                fields.Add("Номер", "123456");
                
                return ApiResponse.CreateSuccess(new SecretDataDto
                {
                    Id = 1,
                    Name = "Паспорт Боба",
                    Type = "Паспорт",
                    Fields = fields
                });
            
        }
        
        [HttpPost]
        [Description("Получить все данные")]
        public async Task<ApiResponse> Get([FromBody] string masterPassword)
        {
            
                var fields1 = new Dictionary<string, string>();
                fields1.Add("Фамилия", "Бобин");
                fields1.Add("Имя", "Боб");
                fields1.Add("Отчество", "Бобович");
                fields1.Add("Серия", "9200");
                fields1.Add("Номер", "123456");
                var fields2 = new Dictionary<string, string>();
                fields2.Add("Логин", "bob");
                fields2.Add("Пароль", "katarakta");
                
                return ApiResponse.CreateSuccess(new List<SecretDataDto>
                {
                    new SecretDataDto 
                    { 
                        Id = 1,
                        Name = "Паспорт Боба",
                        Type = "Паспорт",
                        Fields = fields1
                    },
                    new SecretDataDto
                    {
                        Id = 2,
                        Name = "Пароль от vk.com",
                        Type = "Пароль",
                        Fields = fields2
                    }
                });
            
        }
        
        [HttpPost("create")]
        [Description("Create")]
        public async Task<ApiResponse> Create(CreateSecretDataDto createSecretDataDto)
        {
            
                return ApiResponse.CreateSuccess(new SecretDataDto());
        }
        
        [HttpPut]
        [Description("Update")]
        public async Task<ApiResponse> Update(CreateSecretDataDto createSecretDataDto)
        {
            return ApiResponse.CreateSuccess(new SecretDataDto());
        }
        
        [HttpDelete("{id}")]
        [Description("Delete")]
        public async Task<ApiResponse> Delete(int id)
        {
            return ApiResponse.CreateSuccess("Удалено");
        }
        
        [HttpPost("sharing")]
        [Description("Sharing")]
        public async Task<ApiResponse> Share(ShareSecretDataRequestDto shareSecretDataRequestDto)
        {
            return ApiResponse.CreateSuccess();
        }
    }
}