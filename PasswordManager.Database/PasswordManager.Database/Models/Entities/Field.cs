using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Database.Models.Entities
{
    public class Field
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Value { get; set; }

        public int SecretDataId { get; set; }
        public SecretData SecretData { get; set; }
    }
}