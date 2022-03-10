using Microsoft.EntityFrameworkCore;
using PasswordManager.Database.Models.Entities;

namespace PasswordManager.Database
{
    public class StorageContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<DataType> DataTypes { get; set; }
        public DbSet<TypeField> TypeFields { get; set; }
        
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
            modelBuilder.Entity<DataType>().HasData(
                new DataType { Id = 1, Name = "Паспорт"}
            );
            
            modelBuilder.Entity<TypeField>().HasData(
                new TypeField { Id = 1, Name = "Фамилия", IsSecret = false, DataTypeId = 1},
                new TypeField { Id = 2, Name = "Имя", IsSecret = false, DataTypeId = 1},
                new TypeField { Id = 3, Name = "Отчество", IsSecret = true, DataTypeId = 1},
                new TypeField { Id = 4, Name = "Пол", IsSecret = true, DataTypeId = 1},
                new TypeField { Id = 5, Name = "Дата рождения", IsSecret = true, DataTypeId = 1},
                new TypeField { Id = 6, Name = "Место рождения", IsSecret = true, DataTypeId = 1},
                new TypeField { Id = 7, Name = "Серия", IsSecret = true, DataTypeId = 1},
                new TypeField { Id = 8, Name = "Номер", IsSecret = true, DataTypeId = 1},
                new TypeField { Id = 9, Name = "Кем выдан", IsSecret = true, DataTypeId = 1},
                new TypeField { Id = 10, Name = "Дата выдачи", IsSecret = true, DataTypeId = 1},
                new TypeField { Id = 11, Name = "Код подразделения", IsSecret = true, DataTypeId = 1}
            );
            
        }
    }
}