using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSC_AY2020_Task3.Models
{
    public class CaptchaRequest
    {

        
        public string secret { get; set; } = "6LdsMwYaAAAAAK0BYrBD0mSxu9cYl9nF1JsRrT1Z"; //you're supposed to have a version yourself
        public string response { get; set; }
    }
}