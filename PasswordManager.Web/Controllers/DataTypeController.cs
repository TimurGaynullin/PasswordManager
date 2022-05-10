using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PasswordManager.Contracts;


namespace PasswordManager.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataTypeController : ControllerBase
    {

        public DataTypeController()
        {
        }
        
        [HttpGet]
        [Description("Получить все типы данных")]
        public async Task<ApiResponse> Get()
        {
            var response = new List<DataTypeDto>();
            response.Add(new DataTypeDto
            {
                Id = 1, 
                Name = "Паспорт",
                TypeFields = new List<TypeFieldDto>
                    {
                        new()
                        {
                            Name = "Фамилия"
                        },
                        new()
                        {
                            Name = "Имя"
                        },
                        new()
                        {
                            Name = "Отчество"
                        },
                        new()
                        {
                            Name = "Серия"
                        },
                        new()
                        {
                            Name = "Номер"
                        },
                    }
                });
                response.Add(new DataTypeDto
                {
                    Id = 2, 
                    Name = "Пароль",
                    TypeFields = new List<TypeFieldDto>
                    {
                        new()
                        {
                            Name = "Логин"
                        },
                        new()
                        {
                            Name = "Пароль"
                        }
                    }
                });
                return ApiResponse.CreateSuccess(response);
            
        }
    }
}