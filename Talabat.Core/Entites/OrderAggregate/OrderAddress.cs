using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites.OrderAggregate
{
    public class OrderAddress
    {

        public OrderAddress()
        {

        }
        public OrderAddress(string firstName, string secondName, string country, string city, string street)
        {
            FirstName = firstName;
            LastName = secondName;
            Country = country;
            City = city;
            Street = street;
        }

        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

    }
}
