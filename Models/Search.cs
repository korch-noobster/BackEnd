using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Search
    {
        [Required]
        public  List<Article> Articles { get; set; }
        [Required]
        public List<Discount> Discounts { get; set; }
        [Required]
        public List<Service> Services { get; set; }
    }
}
