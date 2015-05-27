using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
     public class Product
    {
        public int IdProduct { get; set; }
        public Category Category { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public float CurrenCost { get; set; }
        public DateTime Create { get; set; }    
        public List<OrderDetail> OrderDetail { get; set; } // Product ma nekolik zaznamu v OrderDetail  N:M
        public List<ProductCostHistory> ProductCostHistory { get; set; } //  Nekolik zaznamu v ProductCostHistory      1:N
        public List<WatchDog> WatchDog { get; set; } // Product ma nekolik zaznamu v WatchDog     N:M
        public Product() : this(0, null, 0, null)
        {
          
        }

        public Product(int idCategory, string name, float cost, string desc = null )
        {
            OrderDetail = new List<OrderDetail>();
            ProductCostHistory = new List<ProductCostHistory>();
            WatchDog = new List<WatchDog>();
            Category = new Category();

            Category.IdCategory = idCategory;
            Name = name;
            CurrenCost = cost;
            Description = desc;

        } 
         
        public override string ToString()
        {
            return String.Format("Id: {0}, Nazev: {1}, Popis: {2}, Cena: {3} ", IdProduct, Name, Description, CurrenCost);
        }

        public void PrintDetails()
        {
            foreach (var item in ProductCostHistory)
            {
                Console.WriteLine(item.ToString());         
            }
        }


        public string ToStringList()
        {
            return String.Format("ID PRODUCT: {0}, CATEGORY: {1}, Nazev: {2},  Cena: {3} ", 
                IdProduct, Category.Name, Name, CurrenCost);
        }

        public string ToStringDetail()
        {
            return 
                String.Format("ID PRODUCT: {0}, CATEGORY: {1}, Nazev: {2},  Cena: {3} , Popis: {4}  ",
                IdProduct, Category.Name, Name, CurrenCost, Description);
        }
    }
    
}
