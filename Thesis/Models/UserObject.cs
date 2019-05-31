using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thesis.Models
{
    public class UserObject
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string ObjectId { get; set; }
        public Object Object { get; set; }

    }
}
