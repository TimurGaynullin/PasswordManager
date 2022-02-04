using Microsoft.EntityFrameworkCore;
using PasswordManager.Database.Models.Entities;

namespace PasswordManager.Database
{
    public class StorageContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Password> Passwords { get; set; }
        /*
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
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Tom", Age = 37 },
                new User { Id = 2, Name = "Bob", Age = 41 },
                new User { Id = 3, Name = "Sam", Age = 24 }
            );
            */
        }
    }
}