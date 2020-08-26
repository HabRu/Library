using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Library.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required]
        [Display(Name ="Логин")]
        public string Email { get; set; }
        [Required]
        [Display(Name ="Имя пользователя")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Год рождения")]
        public int Year { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Пароль")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage="Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

    }
}
