using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SampleRESTAPI.Models
{
    public class Customer
    {
        // PRIMARY KEY
        public int CustomerID { get; set; }


        public string? FirstName { get; set; }

 
        public string? LastName { get; set; }


        public string? Email { get; set; }


        public string? Phone { get; set; }
    }
}
