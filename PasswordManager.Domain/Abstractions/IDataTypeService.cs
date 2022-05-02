using System.Collections.Generic;
using System.Threading.Tasks;
using PasswordManager.Contracts;

namespace PasswordManager.Domain.Abstractions
{
    public interface IDataTypeService
    {
        Task<List<DataTypeDto>> DataTypes();
    }
}