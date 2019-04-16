using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Thesis.Models
{
    [NotMapped]
    public class CreateEventModel
    {
        public string ObjectId { get; set; }

        [Required(ErrorMessage = "Укажите дату")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Укажите время начала")]
        public DateTime TimeFrom { get; set; }

        [Required(ErrorMessage = "Укажите время окончания")]
        public DateTime TimeTo { get; set; }

        [Required(ErrorMessage = "Добавьте описание")]
        public string Description { get; set; }
    }
}
