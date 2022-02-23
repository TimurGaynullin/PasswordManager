namespace PasswordManager.Domain.Abstractions
{
    public interface IAesProtector
    {
        string ToAes256(string password, string aeskey);

        string FromAes256(string cipherText, string aeskey);
    }
}