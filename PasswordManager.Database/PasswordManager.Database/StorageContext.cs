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
        public DbSet<SecretData> SecretDatas { get; set; }

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
                new DataType { Id = 6, Name = "Свидетельство о постановке на учет физического лица в налоговом органе"},
                new DataType { Id = 7, Name = "Пароль"}
            );
            
            modelBuilder.Entity<TypeField>().HasData(
                new TypeField { Id = 1, Name = "Фамилия", DataTypeId = 1},
                new TypeField { Id = 2, Name = "Имя", DataTypeId = 1},
                new TypeField { Id = 3, Name = "Отчество", DataTypeId = 1},
                new TypeField { Id = 4, Name = "Пол", DataTypeId = 1},
                new TypeField { Id = 5, Name = "Дата рождения", DataTypeId = 1},
                new TypeField { Id = 6, Name = "Место рождения", DataTypeId = 1},
                new TypeField { Id = 7, Name = "Серия", DataTypeId = 1},
                new TypeField { Id = 8, Name = "Номер", DataTypeId = 1},
                new TypeField { Id = 9, Name = "Кем выдан", DataTypeId = 1},
                new TypeField { Id = 10, Name = "Дата выдачи", DataTypeId = 1},
                new TypeField { Id = 11, Name = "Код подразделения", DataTypeId = 1},
                
                new TypeField { Id = 12, Name = "Фамилия", DataTypeId = 2},
                new TypeField { Id = 13, Name = "Имя", DataTypeId = 2},
                new TypeField { Id = 14, Name = "Отчество", DataTypeId = 2},
                new TypeField { Id = 15, Name = "Дата рождения", DataTypeId = 2},
                new TypeField { Id = 16, Name = "Место рождения", DataTypeId = 2},
                new TypeField { Id = 17, Name = "Серия", DataTypeId = 2},
                new TypeField { Id = 18, Name = "Номер", DataTypeId = 2},
                new TypeField { Id = 19, Name = "Дата составления акта о рождении", DataTypeId = 2},
                new TypeField { Id = 20, Name = "Номер акта о рождении", DataTypeId = 2},
                new TypeField { Id = 21, Name = "Отец", DataTypeId = 2},
                new TypeField { Id = 22, Name = "Мать", DataTypeId = 2},
                new TypeField { Id = 23, Name = "Место государственной регистрации", DataTypeId = 2},
                new TypeField { Id = 24, Name = "Дата выдачи", DataTypeId = 2},
                
                new TypeField { Id = 25, Name = "Фамилия", DataTypeId = 3},
                new TypeField { Id = 26, Name = "Имя", DataTypeId = 3},
                new TypeField { Id = 27, Name = "Отчество", DataTypeId = 3},
                new TypeField { Id = 28, Name = "Дата рождения", DataTypeId = 3},
                new TypeField { Id = 29, Name = "Место рождения", DataTypeId = 3},
                new TypeField { Id = 30, Name = "Серия", DataTypeId = 3},
                new TypeField { Id = 31, Name = "Номер", DataTypeId = 3},
                new TypeField { Id = 32, Name = "Дата выдачи", DataTypeId = 3},
                new TypeField { Id = 33, Name = "Дата окончания срока действия", DataTypeId = 3},
                new TypeField { Id = 34, Name = "Выдано", DataTypeId = 3},
                new TypeField { Id = 35, Name = "Регион", DataTypeId = 3},
                new TypeField { Id = 36, Name = "Категории", DataTypeId = 3},
                
                new TypeField { Id = 37, Name = "Банк", DataTypeId = 4},
                new TypeField { Id = 38, Name = "Фамилия и имя", DataTypeId = 4},
                new TypeField { Id = 39, Name = "Номер карты", DataTypeId = 4},
                new TypeField { Id = 40, Name = "Дата окончания", DataTypeId = 4},
                new TypeField { Id = 41, Name = "CVV", DataTypeId = 4},
                
                new TypeField { Id = 42, Name = "Фамилия", DataTypeId = 5},
                new TypeField { Id = 43, Name = "Имя", DataTypeId = 5},
                new TypeField { Id = 44, Name = "Отчество", DataTypeId = 5},
                new TypeField { Id = 45, Name = "Дата рождения", DataTypeId = 5},
                new TypeField { Id = 46, Name = "Пол", DataTypeId = 5},
                new TypeField { Id = 47, Name = "Номер", DataTypeId = 5},
                
                new TypeField { Id = 48, Name = "Фамилия", DataTypeId = 6},
                new TypeField { Id = 49, Name = "Имя", DataTypeId = 6},
                new TypeField { Id = 50, Name = "Отчество", DataTypeId = 6},
                new TypeField { Id = 51, Name = "Дата рождения", DataTypeId = 6},
                new TypeField { Id = 52, Name = "Пол", DataTypeId = 6},
                new TypeField { Id = 53, Name = "Место рождения", DataTypeId = 6},
                new TypeField { Id = 54, Name = "ИНН", DataTypeId = 6},
                new TypeField { Id = 55, Name = "Дата присвоения ИНН", DataTypeId = 6},
                
                new TypeField { Id = 56, Name = "Логин", DataTypeId = 7},
                new TypeField { Id = 57, Name = "Пароль", DataTypeId = 7}
            );
        }
    }
}