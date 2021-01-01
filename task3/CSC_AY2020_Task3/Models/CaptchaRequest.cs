using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSC_AY2020_Task3.Models
{
    public class CaptchaRequest
    {

        
        public string secret { get; set; } = ""; //you're supposed to have a version yourself
        public string response { get; set; }
    }
}