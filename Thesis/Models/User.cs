using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Thesis.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public override string Email { get; set; } // переопределяет IdentityUser.Email
        public string PreferredAddress { get; set; }

        //[ForeignKey("Id")]
        public List<Event> Events { get; set; }

        //[ForeignKey("Id")]
        public List<UserObject> UserObjects { get; set; }


    }
}
