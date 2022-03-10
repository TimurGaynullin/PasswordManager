using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Database.Models.Entities
{
    public class SecretData
    {
        [Key]
        public int Id { get; set; }
        public int DataTypeId { get; set; }
        public DataType DataType { get; set; }
        public List<Field> Fields { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}