using System.Security.Cryptography;
using System.Text;
using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Domain
{
    public class Hasher : IHasher
    {
        public string CryptPassword(string password)
        {
            var data = Encoding.ASCII.GetBytes(password);
            var shaM = new SHA512Managed();
            var result = shaM.ComputeHash(data);
            var sBuilder = new StringBuilder();
            foreach (var @byte in result)
            {
                sBuilder.Append(@byte.ToString("x2"));
            }
            password = sBuilder.ToString();
            return password;
        }
    }
}