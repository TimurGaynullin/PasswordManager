using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Database
{
    public class StorageContext : DbContext
    {
        /*
        public DbSet<User> Users { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<GroupPassword> GroupPasswords { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<FolderPassword> FolderPasswords { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        */

        public StorageContext(DbContextOptions<StorageContext> options)
            : base(options)
        {
            
        }
    }
}