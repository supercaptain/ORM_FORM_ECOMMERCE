using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
    public class CustomerTable
    {
        public static String SQL_SELECT = "SELECT idCustomer, fname, lname, phone, company FROM Customer";
        public static String SQL_SELECT_ID = "SELECT idCustomer, fname, lname, phone, company  FROM Customer WHERE idCustomer=@idCustomer";
        public static String SQL_SELECT_LOGIN_ID = "SELECT c.idCustomer, c.fname, c.lname, c.company, c.phone, c.lastvisit, lg.email FROM Customer c " 
                                                  + " LEFT JOIN [Login] lg  ON  c.idCustomer = lg.idCustomer "
                                                  + " WHERe   c.idCustomer=@idCustomer ";

        public static String SQL_DELETE_ID = "DELETE FROM Customer WHERE idCustomer=@idCustomer";
        public static String SQL_UPDATE = "UPDATE Customer SET fname=@fname, lname=@lname, phone = @phone,  company = @company  WHERE idCustomer=@idCustomer";
               
        public static String SQL_SELECT_LIST_ORDER = " SELECT ord.idOrder, td.name, ord.orderDate, ord.cost, ord.status, ord.discount "
                                        + " FROM [Order] ord "
                                        + " JOIN Customer cu ON cu.idCustomer = ord.idCustomer "
                                        + " JOIN TypeDelivery td ON td.idDelivery = ord.idDelivery "
                                        + " WHERE cu.Idcustomer = @idCustomer";      

        public static String SQL_SELECT_LIST_WATCHDOG = "SELECT w.idCustomer, w.idProduct, p.name,  w.createDate, w.cost, w.notify "
                                                       + "FROM WatchDog w "
                                                       + "JOIN Product p ON p.idproduct = w.idproduct "
                                                       + "WHERE w.idCustomer = @idCustomer ";


       /*
        * Náhrada klasického insertu vlozeni zakaznika probíha pomocí formulare.
        * Vlozeni zakaznika probiha pres transakci
        */
        public static void CreateUser(Customer customer){

            Database db = new Database();
            db.Connect();

            SqlCommand cmd = db.CreateCommand("CreateCustomer");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@p_email", customer.Login.Email));
            cmd.Parameters.Add(new SqlParameter("@p_password", customer.Login.Password));
            cmd.Parameters.Add(new SqlParameter("@p_fname", customer.FName));
            cmd.Parameters.Add(new SqlParameter("@p_lname", customer.LName));
            cmd.Parameters.Add(new SqlParameter("@p_company", customer.Company));
            cmd.Parameters.Add(new SqlParameter("@p_phone", customer.Phone));

            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("Registrovano");
            }
            catch
            {
                Console.WriteLine("Error");
            }          
           
        }

        /**
         * Prihlaseni zakaznika
         * Pokud bude vyhledan email a hesla, bude vracena 1 (nalezeno)
         * Zaroven dojde k aktualizaci data prihlaseni.
         */

        public static int LoginUser(string email, string password)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand cmd = db.CreateCommand("UserLoginProcedure");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // set up the parameters
            cmd.Parameters.Add(new SqlParameter("@p_email", email));
            cmd.Parameters.Add(new SqlParameter("@p_password", password));
            cmd.Parameters.Add("@p_status", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@p_idcustomer", SqlDbType.Int).Direction = ParameterDirection.Output;

            try
            {
                //SqlDataReader reader = cmd.ExecuteReader();
                db.ExecuteNonQuery(cmd);
                int status = Convert.ToInt32(cmd.Parameters["@p_status"].Value);
                int customer = Convert.ToInt32(cmd.Parameters["@p_idcustomer"].Value);

                //Pokud bude status 1, vrati hodnotu v customer, protoze pokud je status 1
                //Customer obsahuje idCustomera
                if (status == 1)
                {
                    return customer;
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                Console.WriteLine("Error");
            }
            finally
            {
                //reader.Close();
                db.Close();
            }
            return 0;
        }


        public static int Update(Customer customer)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, customer);
            int ret = db.ExecuteNonQuery(command);
            db.Close();
            return ret;
        }


        public static int Delete(int id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);
            command.Parameters.Add(new SqlParameter("@idCustomer", SqlDbType.Int));
            command.Parameters["@idCustomer"].Value = id;
            int ret = 0;
            try
            {
                ret = db.ExecuteNonQuery(command);
            }
            catch
            {
                Console.WriteLine("Nelze smazat.Obsahuje odkaz na cizi klice");
            }           
            db.Close();
            return ret;
        }

    

        /*
         * Najde Customera podle ID vrati ho jako objekt
         */
        public static Customer Select(int id, Database dbpr = null)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);
            command.Parameters.Add(new SqlParameter("@idCustomer", SqlDbType.Int));
            command.Parameters["@idCustomer"].Value = id;

            SqlDataReader reader = db.Select(command);
            List<Customer> customers = Read(reader);
            Customer customer = null;

            if (customers.Count == 1)
            {
                customer = customers[0]; //prida se prvni item
            }
            //Vsechny objednavky Zakaznika
            //customer.Orders = SelectListOrder(customer.IdCustomer);

            //Vsechny hlidaci psi Zakaznika
            //customer.WatchDogs = SelectListWatchDog(customer.IdCustomer);

            reader.Close();
            db.Close();

            return customer;
        }


        /*Bude slouzit jako detail pro formulare*/
        public static Customer SelectIncludeLogin(int id, Database dbpr = null)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_LOGIN_ID);
            command.Parameters.Add(new SqlParameter("@idCustomer", SqlDbType.Int));
            command.Parameters["@idCustomer"].Value = id;

            SqlDataReader reader = db.Select(command);            
            reader.Read();
                Customer customer = new Customer();

                customer.IdCustomer = reader.GetInt32(0);
                customer.FName = reader.GetString(1);
                customer.LName = reader.GetString(2);
                if (!reader.IsDBNull(3))
                {
                    customer.Company = reader.GetString(3);
                }   
                if (!reader.IsDBNull(4))
                {
                    customer.Phone = reader.GetString(4);
                }
                if (!reader.IsDBNull(5))
                {
                    customer.LastVisit = reader.GetDateTime(5);
                }
                customer.Login.Email = reader.GetString(6);
            reader.Close();
            db.Close();
            return customer;
        }


        /*
         * Vrati seznam vsech uzivatelu
         * 
         */

        public static List<Customer> SelectAll()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            List<Customer> customers = Read(reader);
            reader.Close();
            db.Close();
            return customers;
        }


        /*
         * Vrati seznam Objednavek, ktere provedl zakaznik
         * Vrací cely seznam objednavek 
         */ 
        public static List<Order> SelectListOrder(int id, Database dbpr = null)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_LIST_ORDER);
            command.Parameters.Add(new SqlParameter("@idCustomer", SqlDbType.Int));
            command.Parameters["@idCustomer"].Value = id;

            SqlDataReader reader = db.Select(command);
            List<Order> orders = new List<Order>();
            while (reader.Read())
            {
                Order order = new Order();
                order.IdOrder = reader.GetInt32(0);
                order.TypeDelivery.Name = reader.GetString(1);
                order.OrderDate = reader.GetDateTime(2);
                order.Cost = (float)reader.GetSqlDouble(3);
                //zmenit DB                 
                if (!reader.IsDBNull(4))
                {
                    order.Status = reader.GetString(4);
                }
                if (!reader.IsDBNull(5))
                {
                    order.Discount = reader.GetInt32(5);
                }
                orders.Add(order);
            }
            reader.Close();
            db.Close();
            return orders;
        }


        /*
         * Vrati seznam Hlidacích psů, ktere zakaznik vytvoril
         * Vrací cely seznam Hlidacích psů
         * Select vybira i jmena produktu.
         */
        public static List<WatchDog> SelectListWatchDog(int id, Database dbpr = null)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_LIST_WATCHDOG);
            command.Parameters.Add(new SqlParameter("@idCustomer", SqlDbType.Int));
            command.Parameters["@idCustomer"].Value = id;

            SqlDataReader reader = db.Select(command);

            List<WatchDog> watchdogs = new List<WatchDog>();
            while (reader.Read())
            {                
                WatchDog watchdog = new WatchDog();                
                watchdog.Customer.IdCustomer = id;
                watchdog.Product.IdProduct = reader.GetInt32(1);
                watchdog.Product.Name = reader.GetString(2);
                watchdog.CreateDate = reader.GetDateTime(3);
                watchdog.Cost = reader.GetDouble(4);
                watchdog.Notify = reader.GetBoolean(5);

                watchdogs.Add(watchdog);
            }
            db.Close();
            return watchdogs;
        }



        private static List<Customer> Read(SqlDataReader reader)
        {
            List<Customer> customers = new List<Customer>();

            while (reader.Read())
            {
                Customer customer = new Customer();
                customer.IdCustomer = reader.GetInt32(0);
                customer.FName = reader.GetString(1);
                customer.LName = reader.GetString(2);                
                if (!reader.IsDBNull(3))
                {
                    customer.Phone = reader.GetString(3);
                }
                
                if (!reader.IsDBNull(4))
                {
                    customer.Company = reader.GetString(4);
                }                
                customers.Add(customer);
            }
            return customers;
        }            
                
        private static void PrepareCommand(SqlCommand command, Customer customer)
        {
            command.Parameters.Add(new SqlParameter("@idCustomer", SqlDbType.Int));
            command.Parameters["@idCustomer"].Value = customer.IdCustomer;

            command.Parameters.Add(new SqlParameter("@fname", SqlDbType.VarChar, 100));
            command.Parameters["@fname"].Value = customer.FName;

            command.Parameters.Add(new SqlParameter("@lname", SqlDbType.VarChar,100));
            command.Parameters["@lname"].Value = customer.LName;
           
            command.Parameters.Add(new SqlParameter("@phone", SqlDbType.VarChar, 50));
            command.Parameters["@phone"].Value = GetValueOrNull(customer.Phone);
              
            command.Parameters.Add(new SqlParameter("@company", SqlDbType.VarChar, 100));
            command.Parameters["@company"].Value = GetValueOrNull(customer.Company);                       
        }

        private static object GetValueOrNull<T>(T obj)
        {
            if (obj == null) return DBNull.Value;
            else return obj;
        }


    }
}
