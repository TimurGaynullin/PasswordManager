using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Domain
{
    public class Hasher
    {
        public string CryptPassword(string password)
        {
            var data = Encoding.ASCII.GetBytes(password);
            var shaM = new SHA512Managed();
            var result = shaM.ComputeHash(data);
            var sBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sBuilder.Append(result[i].ToString("x2"));
            }
            password = sBuilder.ToString();
            return password;
        }
    }
}