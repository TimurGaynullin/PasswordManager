using System.Security.Cryptography;
using System.Text;
using AutoMapper.Internal;
using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Domain
{
    public class AesProtector : IAesProtector
    {
        public string ToAes256(string password, string aeskey)
        {
            using (SHA256 mySha256 = new SHA256CryptoServiceProvider())
            {
                byte[] aeskeyInBytes = Encoding.UTF8.GetBytes(aeskey);
                byte[] key = mySha256.ComputeHash(aeskeyInBytes);
                if (password == null || password.Length <= 0)
                    throw new ArgumentNullException("plainText");
                if (key == null || key.Length <= 0)
                    throw new ArgumentNullException("Key");
                byte[] encrypted;
                byte[] IV;
                using (Aes aesAlg = new AesCryptoServiceProvider())
                {
                    aesAlg.Key = key;
                    aesAlg.GenerateIV();
                    IV = aesAlg.IV;
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(password);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }
                var cipher = encrypted.Concat(IV).ToArray();
                string retVal = Convert.ToBase64String(cipher);
                return retVal;
            }
        }

        public string FromAes256(string cipherPwd, string aeskey)
        {
            using (SHA256 mySha256 = new SHA256CryptoServiceProvider())
            {
                byte[] aeskeyInBytes = Encoding.UTF8.GetBytes(aeskey);
                byte[] key = mySha256.ComputeHash(aeskeyInBytes);
                byte[] cipherText = Convert.FromBase64String(cipherPwd);
                if (cipherText == null || cipherText.Length <= 0)
                    throw new ArgumentNullException("cipherText");
                if (key == null || key.Length <= 0)
                    throw new ArgumentNullException("Key");
                byte[] bytesIv = new byte[16];
                byte[] mess = new byte[cipherText.Length - 16];
                for (int i = cipherText.Length - 16, j = 0; i < cipherText.Length; i++, j++)
                    bytesIv[j] = cipherText[i];
                for (int i = 0; i < cipherText.Length - 16; i++)
                    mess[i] = cipherText[i];
                string plaintext;
                using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = bytesIv;
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    byte[] data = mess;
                    using (MemoryStream msDecrypt = new MemoryStream(data))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                return plaintext;
            }
        }
    }
}