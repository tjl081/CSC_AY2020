using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [Range(0,100)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Price must not be negative and should be 2 decimal place or less!")]
        public decimal Price { get; set; }
    }
}