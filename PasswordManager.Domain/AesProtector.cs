using PasswordManager.Domain.Abstractions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Domain
{
    public class AesProtector : IAesProtector
    {
        private static StringBuilder sb;

        private static byte[] salt = new byte[13]
        {
          (byte)73,
          (byte)118,
          (byte)97,
          (byte)110,
          (byte)32,
          (byte)77,
          (byte)101,
          (byte)100,
          (byte)118,
          (byte)101,
          (byte)100,
          (byte)101,
          (byte)118
        };
        private static string Key { get; set; }

        private static string Encrypt(string raw)
        {
          byte[] bytes = Encoding.Unicode.GetBytes(raw);
          using Aes aes = Aes.Create();
          Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(Key, salt);
          aes.Key = rfc2898DeriveBytes.GetBytes(32);
          aes.IV = rfc2898DeriveBytes.GetBytes(16);
          using MemoryStream memoryStream = new MemoryStream();
          using (CryptoStream cryptoStream = 
            new CryptoStream((Stream) memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
          {
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.Close();
          }
          sb = new StringBuilder(Convert.ToBase64String(memoryStream.ToArray()));
          return sb.Replace("+", "-").Replace("/", "_").Replace("=", ".").Remove(sb.Length - 2, 2).ToString();
        }

        private static string Decrypt(string encrypted)
        {
          sb = new StringBuilder(encrypted);
          encrypted = sb.Replace("-", "+").Replace("_", "/").Replace(".", "=").Replace(" ", "+").Insert(encrypted.Length, "==").ToString();
          byte[] buffer = Convert.FromBase64String(encrypted);
          using Aes aes = Aes.Create();
          Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(Key, salt);
          aes.Key = rfc2898DeriveBytes.GetBytes(32);
          aes.IV = rfc2898DeriveBytes.GetBytes(16);
          using MemoryStream memoryStream = new MemoryStream();
          using (CryptoStream cryptoStream = 
            new CryptoStream((Stream) memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
          {
            cryptoStream.Write(buffer, 0, buffer.Length);
            cryptoStream.Close();
          }
          return Encoding.Unicode.GetString(memoryStream.ToArray());
        }
        
        public string ToAes256(string password, string aeskey)
        {
            Key = aeskey;
            return Encrypt(password);
        }

        public string FromAes256(string cipherText, string aeskey)
        {
            Key = aeskey;
            return Decrypt(cipherText);
        }
    }
}