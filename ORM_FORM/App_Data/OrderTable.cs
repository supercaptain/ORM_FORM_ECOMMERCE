using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{

    public class ProductInCart{
        public int IdProduct{get;set;}
        public int NumProduct{get;set;}

        public ProductInCart(int p, int n)
        { 
            IdProduct = p;
            NumProduct = n; 
        }
    }

    public class OrderTable
    {
        public static String ODESLANA = "odeslana";
        public static String PRIJATA = "prijata";
        public static String STORNOVANA = "stornovana";
        public static String ZAPLACENA = "zaplacena";
        public static String ZPRACOVANA = "zpracovana";

        //Select vybere  detail objednavky beyz adresa, mesta, postal code
        public static String SQL_SELECT = " SELECT ord.idOrder, ord.idCustomer, cu.Fname, cu.Lname, ord.idDelivery, td.name, ord.orderDate, ord.cost, ord.status, ord.discount "
                                        + " FROM [Order] ord "
                                        + " JOIN Customer cu ON cu.idCustomer = ord.idCustomer "
                                        + " JOIN TypeDelivery td ON td.idDelivery = ord.idDelivery ";



        /* SELECT ord.idOrder, ord.idCustomer, cu.Fname, cu.Lname, ord.idDelivery, td.name, ord.orderDate, ord.cost, ord.status, ord.discount
        FROM [Order] ord , Customer cu ,  TypeDelivery td
        WHERE ord.idCustomer = cu.idCustomer AND  ord.idDelivery = td.idDelivery
       */
       
        //Select vybere kompletni detail objednavky - adresa, mesta, postal code
        public static String SQL_SELECT_ID_ORDER = " SELECT ord.idOrder, ord.idCustomer, cu.Fname, cu.Lname, ord.idDelivery, td.name, ord.orderDate, ord.cost, ord.status, ord.discount, ord.adress, ord.city, ord.postalcode "
                                            + " FROM [Order] ord "
                                            + " JOIN Customer cu ON cu.idCustomer = ord.idCustomer "
                                            + " JOIN TypeDelivery td ON td.idDelivery = ord.idDelivery "
                                            + " WHERE ord.idOrder=@idOrder";



        //Select zjisti veskere udaje z OrderDetal
        public static String SQL_SELECT_LIST_ORDER = "SELECT * FROM OrderDetail WHERE idOrder=@idOrder";
        public static String SQL_UPDATE = "UPDATE [Order] SET status=@status WHERE idOrder=@idOrder";
        public static String SQL_DELETE_ID = "DELETE FROM [Order] WHERE idOrder=@idOrder";

   

        public OrderTable() { }
               
          /*
           * Vytvořeni objednavky nahrada klasickoho INSERTU 
           * Objednavka se vytvari pres formular
          */
          public static void CreateOrder(List<ProductInCart> products, Order order)
          {
              DataTable table = FillDataTable(products);
              Database db = new Database();
              db.Connect();

              SqlCommand cmd = db.CreateCommand("createOrder");
              cmd.CommandType = System.Data.CommandType.StoredProcedure;

              cmd.Parameters.Add(new SqlParameter("@p_adress", order.Adress));
              cmd.Parameters.Add(new SqlParameter("@p_city", order.City));
              cmd.Parameters.Add(new SqlParameter("@p_postalcode", order.PostalCode));
              cmd.Parameters.Add(new SqlParameter("@p_idDelivery", order.TypeDelivery.IdDelivery));
              cmd.Parameters.Add(new SqlParameter("@p_idCustomer", order.Customer.IdCustomer));

              //Table
              SqlParameter param = new SqlParameter();
              param.ParameterName = "@tableProduct";
              param.SqlDbType = SqlDbType.Structured;
              param.Value = table;
              param.Direction = ParameterDirection.Input;

              cmd.Parameters.Add(param);

              try
              {
                  SqlDataReader reader = cmd.ExecuteReader();
                  Console.WriteLine("Objednavka uspesne poslana.");
              }
              catch
              {
                  Console.WriteLine("Chyba nakup nekde jinde.");
              }
              finally
              {
                  db.Close();
              }

          }



          public static int Update(Order order)
          {
              Database db = new Database();
              db.Connect();

              SqlCommand command = db.CreateCommand(SQL_UPDATE);
              PrepareCommand(command, order);
              int ret = -1;
              try
              {
                  ret = db.ExecuteNonQuery(command);
                  Console.WriteLine("Objedna zmenena.");
              }
              catch
              {
                  Console.WriteLine("Vyskytla se chyba."); 
              }
              finally
              {
                  db.Close();
              } 
              return ret;
          }

          public static int Delete(int id)
          {
              Database db = new Database();
              db.Connect();

              SqlCommand command = db.CreateCommand(SQL_DELETE_ID);
              command.Parameters.Add(new SqlParameter("@idOrder", SqlDbType.Int));
              command.Parameters["@idOrder"].Value = id;
              int ret = 0;
              try
              {
                  ret = db.ExecuteNonQuery(command);
              }
              catch
              {
                  Console.WriteLine("Nelze smazat. Cizi klice");
              }
              db.Close();
              return ret;
          }            


          /*
          * Vybere objednavku podle ID objevky,
          * vcetne informaci o polozkach v orderList            
          */ 
          public static Order Select(int id)
          {
              Database db = new Database();
              db.Connect();

              SqlCommand command = db.CreateCommand(SQL_SELECT_ID_ORDER);
              command.Parameters.Add(new SqlParameter("@idOrder", SqlDbType.Int));
              command.Parameters["@idOrder"].Value = id;

              SqlDataReader reader = db.Select(command);
              //List<Order> orders = Read(reader);

              Order order = new Order();
              reader.Read();

              order.IdOrder = reader.GetInt32(0);
              order.Customer.IdCustomer = reader.GetInt32(1);
              order.Customer.FName = reader.GetString(2);
              order.Customer.LName = reader.GetString(3);
              order.TypeDelivery.IdDelivery = reader.GetInt32(4);
              order.TypeDelivery.Name = reader.GetString(5);
              order.OrderDate = reader.GetDateTime(6);
              //order.Cost = (float)reader.GetInt32(7);
              order.Cost =(float) reader.GetSqlDouble(7);
              //zmenit DB                 
              if (!reader.IsDBNull(8))
              {
                  order.Status = reader.GetString(8);
              }
              if (!reader.IsDBNull(9))
              {
                  order.Discount = reader.GetInt32(9);
              }
              order.Adress = reader.GetString(10);
              order.City = reader.GetString(11);
              order.PostalCode = reader.GetString(12);
            
              reader.Close();
              db.Close();
              return order;
          }
              





           /*
            * Vybere vsechny objednavky
            */ 
          public static List<Order> SelectAll()
          {
              Database db = new Database();
              db.Connect();

              SqlCommand command = db.CreateCommand(SQL_SELECT);
              SqlDataReader reader = db.Select(command);
              //List<Order> orders = Read(reader);
              List<Order> orders = new List<Order>();              
              while (reader.Read())
              {
                  Order order = new Order();
                  
                  order.IdOrder = reader.GetInt32(0);
                  order.Customer.IdCustomer = reader.GetInt32(1);
                  order.Customer.FName = reader.GetString(2);
                  order.Customer.LName = reader.GetString(3);
                  order.TypeDelivery.IdDelivery = reader.GetInt32(4);  
                  order.TypeDelivery.Name = reader.GetString(5);
                  order.OrderDate = reader.GetDateTime(6);
                  order.Cost = (float)reader.GetSqlDouble(7); 
                             
                  if (!reader.IsDBNull(8))
                  {
                      order.Status = reader.GetString(8);
                  }                
                  if (!reader.IsDBNull(9))
                  {
                      order.Discount = reader.GetInt32(9);
                  } 
                  orders.Add(order);
              }
              reader.Close();
              db.Close();
              return orders;
          }

        /*
          private static List<Order> Read(SqlDataReader reader)
          {
              List<Order> orders = new List<Order>();

              while (reader.Read())
              {                  
                  Order order = new Order();
                  /*
                  order.IdOrder = reader.GetInt32(0);
                  order.Customer.IdCustomer = reader.GetInt32(1);
                  order.TypeDelivery.IdDelivery = reader.GetInt32(2);  
                  if (!reader.IsDBNull(3))
                  {
                      order.ShipDate = reader.GetDateTime(3);
                  }
                  order.OrderDate = reader.GetDateTime(4);
                  order.Cost = (float)reader.GetInt32(5);   //zmenit DB
                 
                  if (!reader.IsDBNull(6))
                  {
                      order.Status = reader.GetString(6);
                  }                
                  if (!reader.IsDBNull(7))
                  {
                      order.Discount = reader.GetInt32(7);
                  }
                  order.Adress = reader.GetString(8);
                  order.City = reader.GetString(9);
                  order.PostalCode = reader.GetString(10);
                  orders.Add(order);
                   


                  order.IdOrder = reader.GetInt32(0);


                  orders.Add(order);
              }
              return orders;
          }
        */

          private static List<OrderDetail> SelectListOrderDetail(int id)
          {
              Database db = new Database();
              db.Connect();

              SqlCommand command = db.CreateCommand(SQL_SELECT_LIST_ORDER);
              command.Parameters.Add(new SqlParameter("@idOrder", SqlDbType.Int));
              command.Parameters["@idOrder"].Value = id;

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
        
          public static void CalculateOrderDiscount(Order order)
          {
              Database db = new Database();
              db.Connect();

              SqlCommand cmd = db.CreateCommand("CalculateDiscount");
              cmd.CommandType = System.Data.CommandType.StoredProcedure;

              cmd.Parameters.Add(new SqlParameter("@p_idOrder", order.IdOrder));
              cmd.Parameters.Add(new SqlParameter("@p_idCustomer", order.Customer.IdCustomer));  

              try
              {
                  SqlDataReader reader = cmd.ExecuteReader();
                  Console.WriteLine("Vypocet slevy proveden");
              }
              catch
              {
                  Console.WriteLine("Nastala chyba.");
              }
              finally
              {
                  db.Close();              
              }

          }
            
          //Dodelat az bude hotovy trigger na ulozeni ceny
          public static float CheckOrderCost(Order order)
          {
              float cost = 0;
              Database db = new Database();
              db.Connect();

              SqlCommand cmd = db.CreateCommand("checkCountOrder");
              cmd.CommandType = System.Data.CommandType.StoredProcedure;

              cmd.Parameters.Add(new SqlParameter("@p_idOrder", order.IdOrder));
              cmd.Parameters.Add("@p_calcCost", SqlDbType.Float).Direction = ParameterDirection.Output;
              
              try
              {
                  //SqlDataReader reader = cmd.ExecuteReader();
                  db.ExecuteNonQuery(cmd);
                  Console.WriteLine("Vypocet slevy proveden");
                  cost =  (float) Convert.ToDouble(cmd.Parameters["@p_calcCost"].Value);  
              }
              catch
              {
                  Console.WriteLine("Nastala chyba.");
              }
              finally
              {
                  db.Close();
              }
              return cost;
          }
        
        /*
         * V ramci objednaky je mozne zmenit pouze status objednavky
         */
        private static void PrepareCommand(SqlCommand command, Order order)
          {
              command.Parameters.Add(new SqlParameter("@idOrder", SqlDbType.Int));
              command.Parameters["@idOrder"].Value = order.IdOrder;

              command.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar, 20));
              command.Parameters["@status"].Value = GetValueOrNull(order.Status);            
          }






          /*
           * POMOCNE METODY NA NAPLANENI TABULEK SIMULACE KOSIKU  
           * 
           */

        //Vytvori tabulku datoveho typu odpovidajici tabulce pro objednavku
        public static DataTable CreateDataTable()
        {
            DataTable table = new DataTable("createOrderTable");
            table.Columns.Add("idProduct", typeof(Int32));
            table.Columns.Add("numProduct", typeof(Int32));
            return table;
        }

        //Naplneni daty z listu produktu
        public static DataTable FillDataTable(List<ProductInCart> products)
        {
            DataTable table = CreateDataTable();
            DataRow row = null;          

            foreach (ProductInCart p in products)
	        {
		        row = table.NewRow();
                row["idProduct"] = p.IdProduct;
                row["numProduct"] = p.NumProduct;
                table.Rows.Add(row);
	        }
            return table;
        }

        //vypis na konzoli
        private static void PrintDataTable(List<ProductInCart> products)
        {
            DataTable table = FillDataTable(products);
            DataRowCollection rows = table.Rows;
            foreach (DataRow r in rows)
            {
                Console.WriteLine(r["idProduct"].ToString() + "  " + r["numProduct"].ToString());
                
            }       
            
        }

        private static object GetValueOrNull<T>(T obj)
        {
            if (obj == null) return DBNull.Value;
            else return obj;
        }
    }
}
