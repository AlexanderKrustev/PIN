namespace Pin.Entitties
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public string EIK { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}