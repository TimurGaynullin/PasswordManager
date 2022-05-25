using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Database.Models.Entities
{
    public class DataType
    {
        public DataType()
        {
            TypeFields = new List<TypeField>();
        }
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Название типа данных (паспорт, ИНН и т.д.)
        /// </summary>
        [Required]
        public string Name { get; set; }
        
        public List<TypeField> TypeFields { get; set; }
    }
}