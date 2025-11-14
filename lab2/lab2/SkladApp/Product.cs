using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladApp
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stillage { get; set; }
        public int Cell { get; set; }
        public int Quantity { get; set; }
    }

}
