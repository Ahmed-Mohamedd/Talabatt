using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites.Identity
{
    public  class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        [Required]
        public string UserId { get; set; } // foreign key
        public User User { get; set; }
    }
}
