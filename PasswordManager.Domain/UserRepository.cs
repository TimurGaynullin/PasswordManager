using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Database;
using PasswordManager.Database.Models.Entities;
using PasswordManager.Domain.Abstractions;

namespace PasswordManager.Domain
{
    public class UserRepository : IUserRepository
    {
        private StorageContext db;
        public UserRepository(StorageContext context)
        {
            db = context;
        }
        
        
        
        public async Task<User> GetIncludingPasswordsAsync(int id)
        {
            var dbEntity = await db.Users
                .Include(user => user.Passwords)
                .FirstOrDefaultAsync(user => user.Id == id);
            if (dbEntity == null)
                throw new Exception("Entity with this id does not exist");
            return dbEntity;
        }
    }
}