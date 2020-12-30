using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSC_AY2020_Task6_new.Models
{
    public class PurchaseModel
    {
        [Required]
        public string CardToken { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string PriceId { get; set; }
    }
}
