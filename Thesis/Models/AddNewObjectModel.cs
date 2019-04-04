using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Thesis.Models
{
    [NotMapped]
    public class AddNewObjectModel
    {
        [Required(ErrorMessage = "Не указан адрес")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        // Убрать фото?
        [Required(ErrorMessage = "Не загружена фотография")] // сделать необязательным с заглушкой по умолчанию?
        [Display(Name = "Фото")]
        public string Photo { get; set; }

        [Required(ErrorMessage = "Не выбран тип площадки")]
        [Display(Name = "Тип площадки")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Не указан тип покрытия")]
        [Display(Name = "Покрытие")]
        public string Terrain { get; set; }

        [Display(Name = "Наличие освещения")]
        public bool Light { get; set; }

    }
}
