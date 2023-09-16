using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_NET.Models
{
    public class Products
    {
        public int Id{ get; init; }

        public string Name { get; set; }

        public string Description{ get; set; }

        public double Price { get; set; }

        public DateTime DateUp { get; set; }

        public string SKU{ get; set; }
    }
}
