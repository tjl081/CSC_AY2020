using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSC_AY2020_Task6_new.Models
{
    public class RegistrationModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }

        // These fields are optional and are not
        // required for the creation of the token

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressCity { get; set; }
        public string AddressPostcode { get; set; }
        public string AddressCountry { get; set; }

    }
}
