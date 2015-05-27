using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
    public class TypeDeliveryTable
    {
        public static String SQL_SELECT = "SELECT * FROM TypeDelivery ";
        public static String SQL_SELECT_ID = "SELECT * FROM TypeDelivery WHERE idDelivery=@idDelivery";
        public static String SQL_INSERT = "INSERT INTO TypeDelivery(name,cost) VALUES (@name,@cost)";
        public static String SQL_UPDATE = "UPDATE TypeDelivery SET name=@name, cost=@cost WHERE idDelivery=@idDelivery";
        public static String SQL_DELETE_ID = "DELETE FROM  TypeDelivery WHERE idDelivery=@idDelivery";

        public static int Insert(TypeDelivery delivery)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_INSERT);

            PrepareCommand(command, delivery);
            //int ret = db.ExecuteNonQuery(command);
            int ret = 0;
            
            try
            {
               ret = db.ExecuteNonQuery(command);
               Console.WriteLine("Vlozen update");
            }
            catch
            {
                Console.WriteLine("Chyba pri vlozeni update");
            }
            finally
            {
                db.Close();
            }
             
            db.Close();
            return 0;
        }

        public static int Update(TypeDelivery delivery)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_UPDATE);

            PrepareCommand(command, delivery);
            //int ret = db.ExecuteNonQuery(command);
            int ret = 0;
            
            try
            {
               ret = db.ExecuteNonQuery(command);
               Console.WriteLine("Vlozen update");
            }
            catch
            {
                Console.WriteLine("Chyba pri update ");
            }
            finally
            {
                db.Close();
            }
            
            db.Close();
            return 0;
        }

        public static int Delete(int id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);
            command.Parameters.Add(new SqlParameter("@idDelivery", SqlDbType.Int));
            command.Parameters["@idDelivery"].Value = id;
            int ret = 0;
            try
            {
                ret = db.ExecuteNonQuery(command);
                Console.WriteLine("Smazano");
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


        public static TypeDelivery Select(int id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);
            command.Parameters.Add(new SqlParameter("@idDelivery", SqlDbType.Int));
            command.Parameters["@idDelivery"].Value = id;

            SqlDataReader reader = db.Select(command);
            List<TypeDelivery> deliveries = Read(reader);
            TypeDelivery delivery = null;
            if (deliveries.Count == 1) { delivery = deliveries[0]; } 
            reader.Close();
            db.Close();

            return delivery;
        }

        public static List<TypeDelivery> SelectAll()
        {

            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            List<TypeDelivery> deliveries = Read(reader);
            reader.Close();
            db.Close();
            return deliveries;
        }

        private static List<TypeDelivery> Read(SqlDataReader reader)
        {
            List<TypeDelivery> deliveries = new List<TypeDelivery>();

            while (reader.Read())
            {
                TypeDelivery delivery = new TypeDelivery();
                delivery.IdDelivery = reader.GetInt32(0);
                delivery.Name = reader.GetString(1);
                delivery.Cost = (float)reader.GetSqlDouble(2);
                deliveries.Add(delivery);
            }
            return deliveries;
        }
                
        private static void PrepareCommand(SqlCommand command, TypeDelivery delivery)
        {
            command.Parameters.Add(new SqlParameter("@idDelivery", SqlDbType.Int));
            command.Parameters["@idDelivery"].Value = delivery.IdDelivery;

            command.Parameters.Add(new SqlParameter("@cost", SqlDbType.Float));
            command.Parameters["@cost"].Value = delivery.Cost;

            command.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 100));
            command.Parameters["@name"].Value = delivery.Name;
        }
    }
}
