using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Database.Models.Entities
{
    public class Field
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Unicode(false)]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Value { get; set; }

        public int SecretDataId { get; set; }
        public SecretData SecretData { get; set; }
    }
}