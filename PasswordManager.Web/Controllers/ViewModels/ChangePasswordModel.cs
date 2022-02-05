using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Web.Controllers.ViewModels
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Не указан Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан старый пароль")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        
        [Required(ErrorMessage = "Не указан новый пароль")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}