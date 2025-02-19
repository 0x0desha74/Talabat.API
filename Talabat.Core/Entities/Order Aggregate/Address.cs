using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
   public class Address
    {
        public Address() //For EFCore used in the migration process
        {

        }
        public Address(string firstName, string lastName, string street, string city, string counrty)
        {
            FirstName = firstName;
            this.lastName = lastName;
            Street = street;
            City = city;
            Counrty = counrty;
        }

        public string FirstName { get; set; }
        public string lastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Counrty { get; set; }
    }
}
