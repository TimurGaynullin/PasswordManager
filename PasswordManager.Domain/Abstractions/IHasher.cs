namespace PasswordManager.Domain.Abstractions
{
    public interface IHasher
    {
        string CryptPassword(string password);
    }
}