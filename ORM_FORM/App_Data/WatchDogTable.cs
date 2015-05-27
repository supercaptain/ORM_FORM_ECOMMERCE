using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
    public class WatchDogTable
    {
        public static String SQL_SELECT = "SELECT * FROM WatchDog";
        public static String SQL_SELECT_ID = "SELECT * FROM WatchDog  WHERE idProduct=@idProduct AND idCustomer=@idCustomer";
        public static String SQL_DELETE_ID = "DELETE FROM  WatchDog WHERE idProduct=@idProduct AND idCustomer=@idCustomer";

        
        //Vytvori novy polozku v DB na zaklade ulozene procedury
        public static void CreateWatchDog(WatchDog watchdog)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand cmd = db.CreateCommand("createWatchDog");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@p_idCustomer", watchdog.Customer.IdCustomer));
            cmd.Parameters.Add(new SqlParameter("@p_idProduct", watchdog.Product.IdProduct));
            cmd.Parameters.Add(new SqlParameter("@p_cost", watchdog.Cost));
            int ret = -1;
            try
            {
               // SqlDataReader reader = cmd.ExecuteReader();
                ret = db.ExecuteNonQuery(cmd);
                Console.WriteLine("Vytvoren hlidaci pes");
            }
            catch
            {
                Console.WriteLine("Chyba pri vytvareni");
            }
            finally
            {
                db.Close();
            }

        }



        //Vymaze konkretniho hlidaciho psa
        // Je nutne znat idCustomer, idProduct
        public static int Delete(int idcustomer, int idproduct)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);
            command.Parameters.Add(new SqlParameter("@idProduct", SqlDbType.Int));
            command.Parameters["@idProduct"].Value = idproduct;
            command.Parameters.Add(new SqlParameter("@idCustomer", SqlDbType.Int));
            command.Parameters["@idCustomer"].Value = idcustomer;
            int ret = 0;          
            try
            {
                ret = db.ExecuteNonQuery(command);
                //Console.WriteLine("Select proveden");
            }
            catch
            {
                Console.WriteLine("Chyba nenalezeneo");
            }
            finally
            {
                db.Close();
            }         
            
            return ret;
        }
        


        //Select na zaklade idcustomera, idproduktu
        //Primarne se nepouziva
        public static WatchDog Select(int idcustomer, int idproduct)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);
            command.Parameters.Add(new SqlParameter("@idProduct", SqlDbType.Int));
            command.Parameters["@idProduct"].Value = idproduct;
            command.Parameters.Add(new SqlParameter("@idCustomer", SqlDbType.Int));
            command.Parameters["@idCustomer"].Value = idcustomer;
            SqlDataReader reader = null;
            List<WatchDog> WatchDogs = null;
            WatchDog watchDog = null;
            try
            {
                reader = db.Select(command);
                WatchDogs = Read(reader);
                //Console.WriteLine("Select proveden");
            }
            catch 
            {
                Console.WriteLine("Chyba nenalezeneo");
            }

            if (WatchDogs.Count == 1)
            {
                watchDog = WatchDogs[0]; //prida se prvni item
            }
            reader.Close();
            db.Close();
            return watchDog;
        }

        public static List<WatchDog> SelectAll()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            List<WatchDog> watchdogs = Read(reader);
            reader.Close();
            db.Close();
            return watchdogs;
        }

        
        private static List<WatchDog> Read(SqlDataReader reader)
        {
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
            return watchdogs;
        }








    }
}
