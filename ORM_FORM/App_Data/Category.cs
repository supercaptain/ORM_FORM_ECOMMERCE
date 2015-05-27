using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
    public class Category
    {

        private int idCategory;
        private string name;
        private string description;
        public List<Product> Products { get; set; } //Predstavuje seznam produktu, ktere patri dane kategorii



        public Category() : this(null, null)
        {
            
        }
        public Category(string name, string description)            
        {
            Products = new List<Product>();
            Name = name;
            Description = description;
        }
       

        public int IdCategory
        {
            get { return idCategory; }
            set { idCategory = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        public void RemoveToProduct(Product product)
        {
            Products.Add(product);
        }

        public void RemoveFromProduct(Product product)
        {
            Products.Remove(product);
        }

        public override string ToString()
        {
            return String.Format("ID Categorie: {0}, Jmeno: {1}, Popis: {2}", IdCategory, Name, Description);
        }

        public string ToStringDetail()
        {
            return String.Format("ID Categorie: {0}, Jmeno: {1}, Popis: {2}", IdCategory, Name, Description);
        }

        public  string ToStringList()
        {
            return String.Format("ID Categorie: {0}, Jmeno: {1} ", IdCategory, Name);
        }

    }
}
