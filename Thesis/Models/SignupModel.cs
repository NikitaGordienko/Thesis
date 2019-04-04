using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Thesis.Models
{
    [NotMapped]
    public class SignupModel
    {
        [Required(ErrorMessage = "Не введен логин")]
        [Display(Name ="Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не введено имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не введена фамилия")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Не введен пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не введен пароль")]
        [Display(Name = "Пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Не введен email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
