using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace ORM_FORM
{
    public class Database
    {        
            private SqlConnection connection;
            SqlTransaction mSqlTransaction = null;

            //private String CONNECTION_STRING = "Server=dbsys.cs.vsb.cz\\STUDENT;Database=kon0021;User=kon0021;Password=Y827ZAJSio;";
          


            public Database()
            {
                connection = new SqlConnection();
            }

            public bool Connect()
            {
                bool ret = true;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    ret = Connect(WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                   // ret = Connect(CONNECTION_STRING);
                }
                return ret;
            }

            public bool Connect(String connstring)
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.ConnectionString = connstring;
                    connection.Open();
                }
                return true;
            }

            public void Close()
            {
                connection.Close();
            }

            public SqlCommand CreateCommand(string strCommand)
            {
                SqlCommand command = new SqlCommand(strCommand, connection);

                if (mSqlTransaction != null)
                {
                    command.Transaction = mSqlTransaction;
                }
                return command;
            }
                   


            //insert update delete 
            public int ExecuteNonQuery(SqlCommand command)
            {
                int rowNumber = 0;
                try
                {
                    command.Prepare();
                    rowNumber = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    Close();
                }
                return rowNumber;
            }
        
            //SELECT EXEXUTE READER
            public SqlDataReader Select(SqlCommand command)
            {
                command.Prepare();
                SqlDataReader sqlReader = command.ExecuteReader();
                return sqlReader;
            }

        }
    
}
