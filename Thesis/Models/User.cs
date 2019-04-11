using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Thesis.Models
{
    public class User : IdentityUser
    { 

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PreferredAddress { get; set; }

        public string AvatarId { get; set; }
        public FileModel Avatar { get; set; }

        [ForeignKey("Id")]
        public List<Event> Events { get; set; }

        [ForeignKey("Id")]
        public List<UserObject> UserObjects { get; set; }

    }
}
