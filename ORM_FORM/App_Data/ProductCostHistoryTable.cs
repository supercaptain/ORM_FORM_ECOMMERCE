using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
   public class ProductCostHistoryTable
    {

       public static String SQL_SELECT_ID = "SELECT * FROM ProductCostHistory WHERE idProduct=@idProduct";



       public static List<ProductCostHistory> Select(int id)
       {

           Database db = new Database();
           db.Connect();

           SqlCommand command = db.CreateCommand(SQL_SELECT_ID);
           command.Parameters.Add(new SqlParameter("@idProduct", SqlDbType.Int));
           command.Parameters["@idProduct"].Value = id;

           SqlDataReader reader = db.Select(command);
           List<ProductCostHistory> products = Read(reader);
           
           reader.Close();
           db.Close();

           return products;
       }

       private static List<ProductCostHistory> Read(SqlDataReader reader)
        {
            List<ProductCostHistory> costhistory = new List<ProductCostHistory>();

            while (reader.Read())
            {
                ProductCostHistory history = new ProductCostHistory();
                history.IdHistory = reader.GetInt32(0);
                history.Product.IdProduct = reader.GetInt32(1);
                history.StartDate = reader.GetDateTime(2);
                history.EndDate = reader.GetDateTime(3);
                history.Cost = (float)reader.GetSqlDouble(4);
                costhistory.Add(history);                
            }
            return costhistory;
        }

    }
}
