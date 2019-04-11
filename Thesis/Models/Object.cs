using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis.Models
{
    public class Object
    {
        public string Id { get; set; }  
        
        public string Address { get; set; }

        public string PhotoId { get; set; }
        public FileModel Photo { get; set; }

        public string District { get; set; }

        public string Type { get; set; }

        public string Terrain { get; set; }

        public bool Light { get; set; } 

        [ForeignKey("Id")]
        public List<Event> Events { get; set; }

        [ForeignKey("Id")]
        public List<UserObject> UserObjects { get; set; }
    }
}
