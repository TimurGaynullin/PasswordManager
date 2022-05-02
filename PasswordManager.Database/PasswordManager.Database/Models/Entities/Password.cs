using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Database.Models.Entities
{
    public class Password
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Login { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        public string CryptPasswordValue { get; set; }
        public bool IsUsingUniversalPassword { get; set; }
    }
}