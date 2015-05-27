using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{

     public class Order
    {
        public int IdOrder { get; set; }
        public Customer Customer { get; set; } //Kvuli  int idCustomer
        public TypeDelivery TypeDelivery { get; set; } //Kvuli  int idDelivery
        public float Cost { get; set; } 
        public string Status { get; set; }
        public DateTime ShipDate { get; set; } //muze byt null '||
        public DateTime OrderDate { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public int Discount { get; set; }
        public string PostalCode { get; set; }
        public  List<OrderDetail> OrderDetail { get; set; }
            
        
       


        public Order()
        {
            OrderDetail = new List<OrderDetail>();
            Customer = new Customer();
            TypeDelivery = new TypeDelivery();
        }

         
        public override string ToString()
        {
            return String.Format("Objednavka: {0},  Dopravce {1}, Datum: {2}, Cena: {3},  Sleva: {4} ", IdOrder, TypeDelivery.Name , OrderDate, Cost, Discount);
        }

        public string ToStringDetail()
        {
            return String.Format("ORD: {0},CUS: {1}, FName: {2}, LNAME: {3}, ID DEL: {4},  NAME DEV: {5}, \nDatum: {6}, Cena: {7}, Status {8}, Sleva:{9}, \nAdresa {10}, Mesto {11} , PostalCode {12}",
                                  IdOrder, Customer.IdCustomer, Customer.FName, Customer.LName, TypeDelivery.IdDelivery, TypeDelivery.Name, OrderDate, Cost , Status, Discount, Adress, City, PostalCode);
        }

        // ord.idOrder, ord.idCustomer, cu.Fname, cu.Lname, ord.idDelivery, td.name, ord.orderDate, ord.cost, ord.status "
        public  string ToStringList()
        {
            return String.Format("ORD: {0},CUS: {1}, FName: {2}, LNAME: {3}, ID DEL: {4},  NAME DEV: {5}, Datum: {6}, Cena: {7}, Status {8}, Sleva:{9}",
                                  IdOrder, Customer.IdCustomer, Customer.FName, Customer.LName, TypeDelivery.IdDelivery, TypeDelivery.Name, OrderDate, Cost , Status, Discount);
        }


        public void PrintDetails()
        {
            foreach (var item in OrderDetail)
            {
                Console.WriteLine(item.ToString()); 
            }
        }
        
    }
}
 