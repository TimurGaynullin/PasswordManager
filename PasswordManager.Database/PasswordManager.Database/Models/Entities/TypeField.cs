using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Database.Models.Entities
{
    public class TypeField
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsSecret { get; set; }
        public int DataTypeId { get; set; }
        public DataType DataType { get; set; }
    }
}