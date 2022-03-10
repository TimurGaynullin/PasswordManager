using System.Collections.Generic;
using System.Threading.Tasks;
using PasswordManager.Contracts;
using PasswordManager.Database.Models.Entities;

namespace PasswordManager.Domain.Abstractions
{
    public interface IPasswordService
    {
        Task<PasswordDto> GetPassword(int id, User user, string masterPassword);

        Task<List<PasswordDto>> GetPasswords(User user, string masterPassword);

        Task<PasswordDto> CreatePassword(PasswordDto passwordDto, User user, string masterPassword);

        Task<PasswordDto> UpdatePassword(PasswordDto passwordDto, User user, string masterPassword);

        Task<bool> DeletePassword(int id, User user);

        Task<bool> SharePassword(int passwordId, User userSender, User userReciver, string masterPassword);

        Task<bool> RecieveSharingPasswords(User user, string masterPassword);
    }
}