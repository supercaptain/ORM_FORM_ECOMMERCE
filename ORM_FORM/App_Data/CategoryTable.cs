using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ORM_ECOMMERCE;
using System.Data.SqlClient;
using System.Data;

namespace ORM_FORM
{
    public class CategoryTable
    {
        public static String SQL_SELECT = "SELECT * FROM Category";
        public static String SQL_SELECT_ID = "SELECT * FROM Category WHERE idCategory=@idCategory";
        public static String SQL_INSERT = "INSERT INTO Category VALUES (@name, @description)";
        public static  String SQL_DELETE_ID = "DELETE FROM Category WHERE idCategory=@idCategory";
        public static String SQL_UPDATE = "UPDATE Category SET name=@name, description=@description WHERE idCategory=@idCategory";

        public static String SQL_PRODUCTLIST = "SELECT idProduct, name, currentcost FROM Product WHERE idCategory = @idCategory";

        public CategoryTable() { }

        public static int Insert(Category category)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_INSERT);
            
            PrepareCommand(command, category);
            int ret = db.ExecuteNonQuery(command);
            
            db.Close();
            return 0;
        }

        public static int Update(Category Category)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, Category);
            int ret = db.ExecuteNonQuery(command);

            db.Close();
            return ret;
        }

         public static int Delete(int id)
         {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);
            command.Parameters.Add(new SqlParameter("@idCategory", SqlDbType.Int));
            command.Parameters["@idCategory"].Value = id;
            int ret = 0;
            try
            {
                ret = db.ExecuteNonQuery(command);   
            }
            catch
            {
                Console.WriteLine("Chyba pri mazani kategorie. Pravdepodobne existuji reference v tabulkach");
            }
            finally{
                db.Close();
            }                        
         
            return ret;
        }

         public static void Delete(Category category)
         {
             if (category.Products == null || category.Products.Count == 0)
             {
                 Delete(category.IdCategory);
             }
             else{
                 Console.WriteLine("Nelze smazat kvuli cizim klicum.");
             }
         }



         public static Category Select(int id)
         {
             Database db = new Database();
             db.Connect();

             SqlCommand command = db.CreateCommand(SQL_SELECT_ID);
             command.Parameters.Add(new SqlParameter("@idCategory", SqlDbType.Int));
             command.Parameters["@idCategory"].Value = id;

             SqlDataReader reader = db.Select(command);
             List<Category> categorie = Read(reader);
             Category category = null;

             if (categorie.Count == 1)
             {
                 category = categorie[0]; //prida se prvni item
             }

             reader.Close();
             db.Close();


             return category;
         }

         public static List<Category> SelectAll()
         {
             
             Database db = new Database();
             db.Connect();

             SqlCommand command = db.CreateCommand(SQL_SELECT);
             SqlDataReader reader = db.Select(command);

             List<Category> Categorie = Read(reader);
             reader.Close();
             db.Close();
             return Categorie;
         }


         public static List<Product> SelectProductCategory(int id)
         {
             Database db = new Database();
             db.Connect();

             SqlCommand command = db.CreateCommand(SQL_PRODUCTLIST);
             command.Parameters.Add(new SqlParameter("@idCategory", SqlDbType.Int));
             command.Parameters["@idCategory"].Value = id;

             SqlDataReader reader = db.Select(command);

             List<Product> products = new List<Product>();
             while (reader.Read())
             {
                 Product product = new Product();
                 product.IdProduct = reader.GetInt32(0);
                 product.Name = reader.GetString(1);
                 product.CurrenCost = (float)reader.GetSqlDouble(2);
                 products.Add(product); 
             }

             return products;
         }


         private static List<Category> Read(SqlDataReader reader)
         {
             List<Category> categories = new List<Category>();

             while (reader.Read())
             {
                 Category category = new Category();
                 category.IdCategory = reader.GetInt32(0);
                 category.Name = reader.GetString(1);               

                 if (!reader.IsDBNull(2))
                 {
                     category.Description = reader.GetString(2);
                 }
                 //category.Products = SelectProductCategory(category.IdCategory);
                 categories.Add(category);

             }
             return categories;
         }

        private static void PrepareCommand(SqlCommand command, Category Category)
        {            
            command.Parameters.Add(new SqlParameter("@idCategory", SqlDbType.Int));
            command.Parameters["@idCategory"].Value = Category.IdCategory;            
            
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, Category.Name.Length));
            command.Parameters["@name"].Value = Category.Name;

            command.Parameters.Add(new SqlParameter("@description", SqlDbType.Text, Category.Description.Length));
            command.Parameters["@description"].Value = Category.Description;
        }
    }
}
