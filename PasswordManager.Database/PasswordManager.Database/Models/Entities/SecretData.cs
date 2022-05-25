using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Database.Models.Entities
{
    public class SecretData
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Название (например, "паспорт Тимура", "мой пароль от kai.ru")
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Тип данных (например, пароль, паспорт, водительское удостоверение)
        /// </summary>
        public DataType DataType { get; set; }
        public int DataTypeId { get; set; }
        /// <summary>
        /// Поля, которые есть в документе (фамилия, имя, серия, номер)
        /// </summary>
        public List<Field> Fields { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsUsingUniversalPassword { get; set; }
    }
}