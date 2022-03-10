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

        public StorageContext(DbContextOptions<StorageContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataType>().HasData(
                new DataType { Id = 1, Name = "Паспорт"},
                new DataType { Id = 2, Name = "Свидетельство о рождении"},
                new DataType { Id = 3, Name = "Водительское удостоверение"},
                new DataType { Id = 4, Name = "Банковская карта"},
                new DataType { Id = 5, Name = "Полис обязательного медицинского страхования"},
                new DataType { Id = 6, Name = "Свидетельство о постановке на учет физического лица в налоговом органе"}
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
                new TypeField { Id = 11, Name = "Код подразделения", IsSecret = true, DataTypeId = 1},
                
                new TypeField { Id = 12, Name = "Фамилия", IsSecret = false, DataTypeId = 2},
                new TypeField { Id = 13, Name = "Имя", IsSecret = false, DataTypeId = 2},
                new TypeField { Id = 14, Name = "Отчество", IsSecret = true, DataTypeId = 2},
                new TypeField { Id = 15, Name = "Дата рождения", IsSecret = true, DataTypeId = 2},
                new TypeField { Id = 16, Name = "Место рождения", IsSecret = true, DataTypeId = 2},
                new TypeField { Id = 17, Name = "Серия", IsSecret = true, DataTypeId = 2},
                new TypeField { Id = 18, Name = "Номер", IsSecret = true, DataTypeId = 2},
                new TypeField { Id = 19, Name = "Дата составления акта о рождении", IsSecret = true, DataTypeId = 2},
                new TypeField { Id = 20, Name = "Номер акта о рождении", IsSecret = true, DataTypeId = 2},
                new TypeField { Id = 21, Name = "Отец", IsSecret = true, DataTypeId = 2},
                new TypeField { Id = 22, Name = "Мать", IsSecret = true, DataTypeId = 2},
                new TypeField { Id = 23, Name = "Место государственной регистрации", IsSecret = true, DataTypeId = 2},
                new TypeField { Id = 24, Name = "Дата выдачи", IsSecret = true, DataTypeId = 2},
                
                new TypeField { Id = 25, Name = "Фамилия", IsSecret = false, DataTypeId = 3},
                new TypeField { Id = 26, Name = "Имя", IsSecret = false, DataTypeId = 3},
                new TypeField { Id = 27, Name = "Отчество", IsSecret = true, DataTypeId = 3},
                new TypeField { Id = 28, Name = "Дата рождения", IsSecret = true, DataTypeId = 3},
                new TypeField { Id = 29, Name = "Место рождения", IsSecret = true, DataTypeId = 3},
                new TypeField { Id = 30, Name = "Серия", IsSecret = true, DataTypeId = 3},
                new TypeField { Id = 31, Name = "Номер", IsSecret = true, DataTypeId = 3},
                new TypeField { Id = 32, Name = "Дата выдачи", IsSecret = true, DataTypeId = 3},
                new TypeField { Id = 33, Name = "Дата окончания срока действия", IsSecret = true, DataTypeId = 3},
                new TypeField { Id = 34, Name = "Выдано", IsSecret = true, DataTypeId = 3},
                new TypeField { Id = 35, Name = "Регион", IsSecret = true, DataTypeId = 3},
                new TypeField { Id = 36, Name = "Категории", IsSecret = true, DataTypeId = 3},
                
                new TypeField { Id = 37, Name = "Банк", IsSecret = false, DataTypeId = 4},
                new TypeField { Id = 38, Name = "Фамилия и имя", IsSecret = false, DataTypeId = 4},
                new TypeField { Id = 39, Name = "Номер карты", IsSecret = true, DataTypeId = 4},
                new TypeField { Id = 40, Name = "Дата окончания", IsSecret = true, DataTypeId = 4},
                new TypeField { Id = 41, Name = "CVV", IsSecret = true, DataTypeId = 4},
                
                new TypeField { Id = 42, Name = "Фамилия", IsSecret = false, DataTypeId = 5},
                new TypeField { Id = 43, Name = "Имя", IsSecret = false, DataTypeId = 5},
                new TypeField { Id = 44, Name = "Отчество", IsSecret = true, DataTypeId = 5},
                new TypeField { Id = 45, Name = "Дата рождения", IsSecret = true, DataTypeId = 5},
                new TypeField { Id = 46, Name = "Пол", IsSecret = true, DataTypeId = 5},
                new TypeField { Id = 47, Name = "Номер", IsSecret = false, DataTypeId = 5},
                
                new TypeField { Id = 48, Name = "Фамилия", IsSecret = false, DataTypeId = 6},
                new TypeField { Id = 49, Name = "Имя", IsSecret = false, DataTypeId = 6},
                new TypeField { Id = 50, Name = "Отчество", IsSecret = true, DataTypeId = 6},
                new TypeField { Id = 51, Name = "Дата рождения", IsSecret = true, DataTypeId = 6},
                new TypeField { Id = 52, Name = "Пол", IsSecret = true, DataTypeId = 6},
                new TypeField { Id = 53, Name = "Место рождения", IsSecret = false, DataTypeId = 6},
                new TypeField { Id = 54, Name = "ИНН", IsSecret = false, DataTypeId = 6},
                new TypeField { Id = 55, Name = "Дата присвоения ИНН", IsSecret = false, DataTypeId = 6}
            );
        }
    }
}