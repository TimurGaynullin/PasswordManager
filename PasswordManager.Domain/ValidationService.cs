using System;
using System.Linq;
using PasswordManager.Database;
using PasswordManager.Database.Models.Entities;
using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Domain
{
    public class ValidationService : IValidationService
    {
        Hasher hasher;
        StorageContext db;
        public ValidationService(Hasher hasher, StorageContext db)
        {
            this.hasher = hasher;
            this.db = db;
        }
        
        public bool ChangingPassword(User? user, string pass)
        {
            /*
            if (user != null)
            {
                pass = hasher.CryptPassword(pass);
                var passwords = user.Passwords.OrderByDescending(n => n.CreatingTime.Date).Take(3).ToList();
                if (passwords.Any(x => x.Value == pass))
                    return false;
                user.Passwords.Add(new Password { Value = pass, CreatingTime = DateTime.Now });
                db.SaveChanges();
                return true;
            }
            */
            return false;
        }

        public bool LogIn(User? user, string pass)
        {
            if (user != null)
            {
                var password = user.MasterPasswordHash;
                pass = hasher.CryptPassword(pass);
                if (password == pass)
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
                    MasterPasswordHash = password //захэшировать
                });
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}