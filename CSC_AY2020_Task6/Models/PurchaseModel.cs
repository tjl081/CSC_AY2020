using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CSC_AY2020_Task6.Models
{
    public class PurchaseModel
    {
        //[Required]
        //public string CardNo { get; set; }
        //[Required]
        //public int ExpiryMonth { get; set; }
        //[Required]
        //public int ExpiryYear { get; set; }
        //[Required]
        //public string CVC { get; set; }
        [Required]
        public string CardToken { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string PriceId { get; set; }
    }
}