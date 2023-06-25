using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SampleRESTAPI.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }


        public string? FirstName { get; set; }

 
        public string? LastName { get; set; }


        public string? Email { get; set; }


        public string? Phone { get; set; }
    }
}
