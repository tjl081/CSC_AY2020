using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSC_AY2020_Task3.Models
{
    public class CaptchaResponse
    {
        public bool success { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }
        public List<string> error_codes { get; set; }
        public double score { get; set; }
        public string action { get; set; }
    }
}