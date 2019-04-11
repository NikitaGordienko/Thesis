using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Thesis.Models
{
    [NotMapped]
    public class EditProfileModel
    {

        [Required(ErrorMessage = "Не введено имя")]
        [RegularExpression(@"^[А-ЯЁ][а-яё]*$", ErrorMessage = "Неверный формат имени")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не введена фамилия")]
        [RegularExpression(@"^[А-ЯЁ][а-яё]*$", ErrorMessage = "Неверный формат фамилии")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Не введен email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //public string AvatarId { get; set; }

        public string PreferredAddress { get; set; }
    }
}
