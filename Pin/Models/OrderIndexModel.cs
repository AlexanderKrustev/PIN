
namespace Pin.Models
{
    using System;
    
using System.ComponentModel.DataAnnotations;

    public class OrderIndexModel
    {
       
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:0000}")]    
        public int Number { get; set; }

        public string CompanyName { get; set; }

        public string Receiver { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string Reason { get; set; }
    }
}
