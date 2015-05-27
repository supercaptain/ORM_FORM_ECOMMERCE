using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
    public class OrderDetail
    {
        public Order Order { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }

        public OrderDetail()
        {
            Order = new Order();
            Product = new Product();
        }

        public override string ToString()
        {
            return String.Format("Objednavka: {0}, Product: {1}, Mnostvi: {2}, Sleva: {3} ", Order.IdOrder,Product.IdProduct, Quantity, Discount);
        }
        


    }


}
