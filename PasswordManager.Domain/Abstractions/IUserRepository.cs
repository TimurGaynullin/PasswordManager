using System.Threading.Tasks;
using PasswordManager.Database.Models.Entities;

namespace PasswordManager.Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<User> GetIncludingPasswordsAsync(int id);
        
        Task<User> GetIncludingSecretDataAsync(int id);
    }
}