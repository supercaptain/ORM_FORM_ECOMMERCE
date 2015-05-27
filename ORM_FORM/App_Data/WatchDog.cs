using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
     public class WatchDog
    {
        public Customer Customer { get; set; }
        public Product Product { get;set; }
        public double Cost {get;set;}
        public bool Notify { get; set; }
        public DateTime CreateDate {get; set;}

        public WatchDog()
        {
            Product = new Product();
            Customer = new Customer();
        }

        public override string ToString()
        {
            return String.Format("Customer: {0}, Product: {1}, NameProduct: {2}, Cost: {3}, Create: {4}, Notify: {5}", 
                Customer.IdCustomer, Product.IdProduct, Product.Name, Cost, CreateDate, Notify);
        }
        


    }
}
