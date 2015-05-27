using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
   
    public class Customer
    {
        public int IdCustomer { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public DateTime LastVisit { get; set; }
        public List<Order> Orders { get; set; } //Zakazník muze mit seznam objednavek    
        public List<WatchDog> WatchDogs { get; set; } //Zakazník muze mit seznam hlidacich psu

        public Login Login { get; set; }

        public Customer()
            : this(null, null, null, null, null, null)
        {
          
        }

        public Customer(string fname, string lname, string email, string password, string phone = null, string company = null)
        {
            Orders = new List<Order>();
            WatchDogs = new List<WatchDog>();
            Login = new Login();

            FName = fname;
            LName = lname;
            Company = company;
            Phone = phone;
            Login.Email = email;
            Login.Password = password;

        }

        public Customer(int idcustomer, string fname, string lname, string company, string phone, string type, DateTime lastvisit)
        {
            IdCustomer = idcustomer;
            FName = fname;
            LName = lname;
            Company = company;
            Phone = phone;
            Type = type;
            LastVisit = lastvisit;
            Orders = new List<Order>();
            WatchDogs = new List<WatchDog>();
            Login = new Login();
        }

        public void RemoveToOrders(Order order)
        {
            Orders.Add(order);
        }

        public void RemoveFromOrders(Order order)
        {
            Orders.Remove(order);
        }


        public void RemoveToWatchDog(WatchDog watchDog)
        {
            WatchDogs.Add(watchDog);
        }

        public void RemoveFromWatchDog(WatchDog watchDog)
        {
            WatchDogs.Remove(watchDog);
        }


        public override string ToString()
        {
            return String.Format("ID: {0}, FN: {1}, LN: {2}, Phone: {3}, Company: {4}", IdCustomer, FName, LName, Phone, Company);
        }

        public  string ToStringDetail()
        {
            return String.Format("ID: {0}, FN: {1}, LN: {2}, TEL: {3}, Company: {4}, Date: {5}, Email: {6}", IdCustomer, FName, LName, Phone, Company, LastVisit, Login.Email);
        }


        public void PrintInformation()
        {
            Console.WriteLine("Uzivatel: " + ToString());
            Console.WriteLine("Objednavky: ");
            foreach (Order order in Orders)
            {
                //order.ToString();
                Console.WriteLine(order.ToString());
            }
            
            Console.WriteLine("Hlidaci psi: ");
            foreach (WatchDog wg in WatchDogs)
            {
                //wg.ToString();
                Console.WriteLine(wg.ToString());
            }
            

        }

    }
}
