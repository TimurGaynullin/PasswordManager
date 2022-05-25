using System.Threading.Tasks;
using PasswordManager.Database.Models.Entities;

namespace PasswordManager.Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<User> GetIncludingSecretDataAsync(int id);
    }
}