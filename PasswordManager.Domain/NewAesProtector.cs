using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Domain
{
    public class NewAesProtector : IAesProtector
    {
        public string ToAes256(string password, string aeskey)
        {
            var iv = "1234567890123456";
            var key = $"{aeskey.Substring(0, 24)}";
            var encryptString = EasyEncryption.AES.Encrypt(password, key, iv);
            return encryptString;
        }

        public string FromAes256(string cipherText, string aeskey)
        {
            var iv = "1234567890123456";
            var key = $"{aeskey.Substring(0, 24)}";
            var result = EasyEncryption.AES.Decrypt(cipherText, key, iv);
            return result;
        }
    }
}