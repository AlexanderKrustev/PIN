namespace Pin.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Pin.Entitties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderCreateVM
    {
       
        public int Number { get; set; }

        public string Company { get; set; }

        public string Receiver { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
        
        public decimal Amount { get; set; }
 
        public string Reason { get; set; }

        public List<SelectListItem> Companies { get; set; }
    }
}
