using PasswordManager.Database.Models.Entities;

namespace PasswordManager.Domain.Abstractions
{
    public interface IValidationService
    {
        bool ChangingPassword(User? user, string pass);
        bool LogIn(User? user, string pass);
        bool Registering(User? user, string login, string password);
    }
}