using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace RESTauranter.Models

{
    public class Help
    {
        public int ID { get; set; }

        public int Review_ID { get; set; }
        
        public string Value { get; set; }

        public DateTime Created_At { get; set; }

        public DateTime Updated_At { get; set; }
    }
}