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

        // Координаты
        public string Address { get; set; }

        public string PhotoId { get; set; }
        public FileModel Photo { get; set; }

        public string DistrictId { get; set; }
        public District District { get; set; }

        public string TypeId { get; set; }
        public ObjectType Type { get; set; }

        public string TerrainId { get; set; }
        public Terrain Terrain { get; set; }

        public bool Light { get; set; } 

        [ForeignKey("Id")]
        public List<Event> Events { get; set; }

        [ForeignKey("Id")]
        public List<UserObject> UserObjects { get; set; }
    }
}
