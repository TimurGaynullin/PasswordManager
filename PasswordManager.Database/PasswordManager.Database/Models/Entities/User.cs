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
            SecretDatas = new List<SecretData>();
        }
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Уникальный логин
        /// </summary>
        [Required]
        public string Login { get; set; }
        [Required]
        public string MasterPasswordHash { get; set; }
        
        public List<Password> Passwords { get; set; }
        
        public List<SecretData> SecretDatas { get; set; }
    }
}