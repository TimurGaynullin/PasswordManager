using System;
using System.Linq;
using PasswordManager.Database;
using PasswordManager.Database.Models.Entities;
using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Domain
{
    public class ValidationService : IValidationService
    {
        IHasher hasher;
        StorageContext db;
        public ValidationService(IHasher hasher, StorageContext db)
        {
            this.hasher = hasher;
            this.db = db;
        }
        
        public bool ChangingPassword(User? user, string oldPassword, string newPassword)
        {
            if (user != null)
            {
                if (user.MasterPasswordHash != hasher.CryptPassword(oldPassword))
                {
                    return false;
                }

                var oldPasswordSumHash = hasher.CryptPassword(user.MasterPasswordHash + oldPassword); //старый ключ шифрования
                var newPasswordHash = hasher.CryptPassword(newPassword);
                user.MasterPasswordHash = newPasswordHash;
                var newPasswordSumHash = hasher.CryptPassword(newPasswordHash + newPassword); //новый ключ шифрования

                foreach (var password in user.Passwords)
                {
                    //расшифровать старым и зашифровать новым ключом
                }
                
                db.SaveChanges();
                return true;
            }
            
            return false;
        }

        public bool LogIn(User? user, string pass)
        {
            if (user != null)
            {
                if (user.MasterPasswordHash == hasher.CryptPassword(pass))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Registering(User? user, string login, string password)
        {
            if (user == null)
            {
                db.Users.Add(new User
                {
                    Login = login,
                    MasterPasswordHash = hasher.CryptPassword(password)
                });
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}