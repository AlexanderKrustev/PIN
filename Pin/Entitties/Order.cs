

namespace Pin.Entitties
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0000}")]
        public int Number { get; set; }

        [Required]
        public Company Company { get; set; }

        [MaxLength(150)]
        public string Receiver { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        public decimal  Amount{ get; set; }

        [MaxLength(250)]
        public string Reason { get; set; }
    }
}
