﻿using PasswordManager.Domain.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Domain
{
    public class AesProtector : IAesProtector
    {
        //TODO: исправить баги
        public string ToAes256(string password, string aeskey)
        {
            using (SHA256 mySha256 = new SHA256CryptoServiceProvider())
            {
                byte[] aeskeyInBytes = Encoding.UTF8.GetBytes(aeskey);
                byte[] key = mySha256.ComputeHash(aeskeyInBytes);
                if (password == null || password.Length <= 0)
                    throw new ArgumentNullException("password");
                if (key == null || key.Length <= 0)
                    throw new ArgumentNullException("aeskey");

                byte[] encrypted;
                byte[] IV;
                using (Aes aesAlg = new AesCryptoServiceProvider())
                {
                    aesAlg.Key = key;
                    // Рандомный Initialization Vector
                    aesAlg.GenerateIV();
                    IV = aesAlg.IV;
                    //aesAlg.IV = IV;
                    // Create an encryptor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(password);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }
                // Return the encrypted bytes from the memory stream.
                //Возвращаем поток байт + крепим соль
                var cipher = encrypted.Concat(IV).ToArray();
                string retVal = Convert.ToBase64String(cipher);

                return retVal;
            }
        }

        //TODO: исправить баги
        public string FromAes256(string cipherText, string aeskey)
        {
            using (SHA256 mySha256 = new SHA256CryptoServiceProvider())
            {
                byte[] aeskeyInBytes = Encoding.UTF8.GetBytes(aeskey);
                byte[] key = mySha256.ComputeHash(aeskeyInBytes);

                //mySha256.Dispose();

                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                // Check arguments.
                if (cipherText == null || cipherText.Length <= 0)
                    throw new ArgumentNullException("cipherText");
                if (key == null || key.Length <= 0)
                    throw new ArgumentNullException("aeskey");

                // Размер IV равен всегда 16
                byte[] bytesIv = new byte[16];
                byte[] mess = new byte[cipherText.Length - 16];
                //Списываем соль
                for (int i = cipherText.Length - 16, j = 0; i < cipherText.Length; i++, j++)
                    bytesIv[j] = cipherTextBytes[i];
                //Списываем оставшуюся часть сообщения
                for (int i = 0; i < cipherText.Length - 16; i++)
                    mess[i] = cipherTextBytes[i];

                // Declare the string used to hold
                // the decrypted text.
                string plaintext;

                // Create an AesCryptoServiceProvider object
                // with the specified key and IV.
                using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = bytesIv;

                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    byte[] data = mess;
                    // Create the streams used for decryption
                    using (MemoryStream msDecrypt = new MemoryStream(data))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
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