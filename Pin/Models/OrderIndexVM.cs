namespace Pin.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Pin.Entitties;
    using System.Collections.Generic;

    public class OrderIndexVM
    {
        public Company Company { get; set; }
        public List<Order> Orders { get; set; }
        public List<SelectListItem> Companies { get; set; }
        public int Year {get;set;}
        public IEnumerable<SelectListItem> Years { get; set; }

    }
}
