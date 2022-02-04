using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Database.Models.Entities
{
    public class User
    {
        public User()
        {
            Passwords = new List<Password>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string MasterPasswordHash { get; set; }
        
        public List<Password> Passwords { get; set; }
    }
}