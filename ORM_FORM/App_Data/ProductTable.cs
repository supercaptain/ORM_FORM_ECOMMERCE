using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
    public class ProductTable
    {

        //public static String SQL_SELECT = "SELECT * FROM Product ";
        public static String SQL_SELECT = " Select  pr.idProduct, pr.IdCategory, pr.Name, pr.currentcost, cat.name"
                                          + " FROM Product pr, Category cat "
                                          + " Where pr.Idcategory = cat.idcategory ";

        //public static String SQL_SELECT_ID = "SELECT * FROM Product WHERE idProduct=@idProduct";

        public static String SQL_SELECT_ID = " Select  pr.idProduct, pr.IdCategory, pr.Name, pr.currentcost, cat.name, pr.description "
                                             + " FROM Product pr, Category cat "
                                             + " Where pr.Idcategory = cat.idcategory AND pr.idProduct=@idProduct";

        public static String SQL_INSERT_T = "INSERT INTO Product(idcategory,name,description,currentcost, create_at) VALUES (@idcategory, @name, @description, @currentcost, @currtime)";
        public static String SQL_INSERT = "INSERT INTO Product(idcategory, name,description,currentcost) VALUES (@idcategory, @name, @description, @currentcost)";
        public static String SQL_INSERT_D = "INSERT INTO Product(idcategory, name,currentcost) VALUES (@idcategory, @name,  @currentcost)";

        public static String SQL_UPDATE = "UPDATE Product SET name=@name, description=@description, currentcost=@currentcost WHERE idProduct=@idProduct";
        public static String SQL_DELETE_ID = "DELETE FROM Product WHERE idProduct=@idProduct";


        public static String SQL_SELECT_LIST_ORDER = "SELECT * FROM OrderDetail WHERE idProduct=@idProduct";
        public static String SQL_SELECT_LIST_COSTHISTORY = "SELECT * FROM ProductCostHistory WHERE idProduct=@idProduct";
        public static String SQL_SELECT_LIST_WATCHDOG = "SELECT * FROM WatchDog WHERE idProduct=@idProduct";

        public static int Insert(Product product)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_INSERT);

            PrepareCommand(command, product);
            int ret = db.ExecuteNonQuery(command);
            //int ret = 0;
            /*
            try
            {
               ret = db.ExecuteNonQuery(command);
               Console.WriteLine("Vlozen produkt");
            }
            catch
            {
                Console.WriteLine("Chyba pri vlozeni produktu");
            }
            finally
            {
                db.Close();
            }
             * */
            db.Close();
            return 0;
        }

        public static int Update(Product product)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_UPDATE);

            PrepareCommand(command, product);
            int ret = db.ExecuteNonQuery(command);
            //int ret = 0;
            /*
            try
            {
               ret = db.ExecuteNonQuery(command);
               Console.WriteLine("Updatovan produkt");
            }
            catch
            {
                Console.WriteLine("Chyba pri updatu produktu");
            }
            finally
            {
                db.Close();
            }
             * */
            db.Close();
            return 0;
        }

        public static int Delete(int id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);
            command.Parameters.Add(new SqlParameter("@idProduct", SqlDbType.Int));
            command.Parameters["@idProduct"].Value = id;
            int ret = 0;
            try
            {
                ret = db.ExecuteNonQuery(command);
            }
            catch
            {
                Console.WriteLine("Nelze smazat. Cizi klice");
            }
            finally
            {
                db.Close();
            }
            
            return ret;
        }


        public static Product Select(int id)
        {

            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);
            command.Parameters.Add(new SqlParameter("@idProduct", SqlDbType.Int));
            command.Parameters["@idProduct"].Value = id;

            SqlDataReader reader = db.Select(command);
            reader.Read();

            Product product = new Product();
            product.IdProduct = reader.GetInt32(0);
            product.Category.IdCategory = reader.GetInt32(1);
            product.Name = reader.GetString(2);
            product.CurrenCost = (float)reader.GetSqlDouble(3);
            product.Category.Name = reader.GetString(4);
            product.Description = reader.GetString(5);      
            
            reader.Close();
            db.Close();
            return product;
        }

        public static List<Product> SelectAll()
        {

            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);
            List<Product> products = Read(reader);
            reader.Close();
            db.Close();
            return products;
        }


        private static List<OrderDetail> SelectListOrderDetail(int id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_LIST_ORDER);
            command.Parameters.Add(new SqlParameter("@idProduct", SqlDbType.Int));
            command.Parameters["@idProduct"].Value = id;

            SqlDataReader reader = db.Select(command);

            List<OrderDetail> ordDetails = new List<OrderDetail>();

            while (reader.Read())
            {
                OrderDetail order = new OrderDetail();
                order.Order.IdOrder = reader.GetInt32(0);
                order.Product.IdProduct = reader.GetInt32(1);
                order.Quantity = reader.GetInt32(2);
                if (!reader.IsDBNull(3))
                {
                    order.Discount = reader.GetInt32(3);
                }
                ordDetails.Add(order);
            }
            db.Close();
            return ordDetails;
        }



        private static List<ProductCostHistory> SelectListCostHistory(int id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_LIST_ORDER);
            command.Parameters.Add(new SqlParameter("@idProduct", SqlDbType.Int));
            command.Parameters["@idProduct"].Value = id;

            SqlDataReader reader = db.Select(command);

            List<ProductCostHistory> costhistory = new List<ProductCostHistory>();

            while (reader.Read())
            {
                ProductCostHistory history = new ProductCostHistory();
                history.IdHistory = reader.GetInt32(0);
                history.Product.IdProduct = reader.GetInt32(1);
                history.StartDate = reader.GetDateTime(2);
                history.EndDate = reader.GetDateTime(3);
                costhistory.Add(history);
            }
            db.Close();
            return costhistory;
        }


        private static List<WatchDog> SelectListWatchDog(int id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_LIST_WATCHDOG);
            command.Parameters.Add(new SqlParameter("@idProduct", SqlDbType.Int));
            command.Parameters["@idProduct"].Value = id;

            SqlDataReader reader = db.Select(command);

            List<WatchDog> watchdogs = new List<WatchDog>();
            while (reader.Read())
            {
                WatchDog watchdog = new WatchDog();
                watchdog.Customer.IdCustomer = reader.GetInt32(0);               
                watchdog.Product.IdProduct = reader.GetInt32(1);
                watchdog.CreateDate = reader.GetDateTime(2);
                watchdog.Cost = reader.GetDouble(3);
                watchdog.Notify = reader.GetBoolean(4);

                watchdogs.Add(watchdog);
            }
            db.Close();
            return watchdogs;
        }


        private static List<Product> Read(SqlDataReader reader)
        {
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                Product product = new Product();
                product.IdProduct = reader.GetInt32(0);
                product.Category.IdCategory = reader.GetInt32(1);
                product.Name = reader.GetString(2);
                product.CurrenCost = (float)reader.GetSqlDouble(3);             
                product.Category.Name = reader.GetString(4);               
                products.Add(product);
            }
            return products;
        }

        private static void PrepareCommand(SqlCommand command, Product product)
        {
            command.Parameters.Add(new SqlParameter("@idCategory", SqlDbType.Int));
            command.Parameters["@idCategory"].Value = product.Category.IdCategory;

            command.Parameters.Add(new SqlParameter("@idProduct", SqlDbType.Int));
            command.Parameters["@idProduct"].Value = product.IdProduct;

            command.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 100));
            command.Parameters["@name"].Value = product.Name;
            
            command.Parameters.Add(new SqlParameter("@description", SqlDbType.Text, -1));
            command.Parameters["@description"].Value = GetValueOrNull(product.Description);            

            command.Parameters.Add(new SqlParameter("@currentcost", SqlDbType.Float));
            command.Parameters["@currentcost"].Value = product.CurrenCost;

            /*
            command.Parameters.Add(new SqlParameter("@description", SqlDbType.DateTime));
            command.Parameters["@description"].Value = product.Create;          
            */
        }
        
        private static object GetValueOrNull<T>(T obj)
        {
            if (obj == null) return DBNull.Value;
            else return obj;
        }

    }
}
