using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Contracts;
using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataTypeController : ControllerBase
    {
        private IDataTypeService _dataTypeService;
        
        public DataTypeController(IDataTypeService dataTypeService)
        {
            _dataTypeService = dataTypeService;
        }
        
        [HttpGet]
        [Description("Получить все типы данных")]
        public async Task<ApiResponse> Get()
        {
            var dataTypes = await _dataTypeService.DataTypes();
            return ApiResponse.CreateSuccess(dataTypes);
        }
    }
}