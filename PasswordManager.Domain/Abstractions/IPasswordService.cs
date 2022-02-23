using System.Collections.Generic;
using System.Threading.Tasks;
using PasswordManager.Contracts;
using PasswordManager.Database.Models.Entities;

namespace PasswordManager.Domain.Abstractions
{
    public interface IPasswordService
    {
        Task<PasswordDto> GetPassword(int id, User user);

        Task<List<PasswordDto>> GetPasswords(User user);

        Task<PasswordDto> CreatePassword(PasswordDto passwordDto, User user);

        Task<PasswordDto> UpdatePassword(PasswordDto passwordDto, User user);

        Task<bool> DeletePassword(int id, User user);
    }
}