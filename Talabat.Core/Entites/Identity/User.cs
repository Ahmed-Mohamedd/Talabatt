using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites.Identity
{
    public  class User:IdentityUser
    {
        public string DiplayName { get; set; }
        public Address Address { get; set; }
    }
}
