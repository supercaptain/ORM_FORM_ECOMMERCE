using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
    public class ProductCostHistory
    {
        public int IdHistory { get; set; }
        public Product Product { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float Cost { get; set; }

        public ProductCostHistory()
        {
            Product = new Product();
        }

        public override string ToString()
        {
            return String.Format("Datum zacatku: {0} , Datum konce{1}, Cena: {2}", StartDate, EndDate, Cost);
        }
    }
}
