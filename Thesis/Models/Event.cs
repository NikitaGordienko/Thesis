using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thesis.Models
{
    public class Event
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string ObjectId { get; set; }
        public Object Object { get; set; }
        // Названия ключей-свойств
        // + Должны быть ссылки на классы
    }
}
