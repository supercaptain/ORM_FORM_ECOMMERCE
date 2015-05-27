using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ORM_FORM.Form
{

    public class ProductItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Pocet { get; set; }
        public ProductItem() { }
        public ProductItem(int id, string name) { }
    }

    public partial class Kosik : System.Web.UI.Page
    {
        private List<string> coll;
        private List<int> list_num;
        List<ORM_FORM.Form.ProductItem> itemlist;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idCustomer"] != null){     

            /*
                if(Session["products"] != null){
                    coll = (List<string>)Session["products"];
                    BindItemsInCart(coll);
                }
           */
                lbl.Text = "Uzivatel s ID: " + (int)Session["idCustomer"];
                if (Session["productsList"] != null)
                {
                    itemlist = (List<ORM_FORM.Form.ProductItem>)Session["productsList"];
                    BindItemsInCart2(itemlist);
                }
                
             }
            else
            {
                Response.Redirect("~/Form/Login.aspx");
            }
        }


        private void BindItemsInCart(List<string> ListOfSelectedProducts)
        {
            List<ProductItem> productList = new List<ProductItem>();

            foreach (var item in ListOfSelectedProducts)
            {
                /*productList.Add(
                    new ProductItem { Name = item}
                    );*/
                productList.Add(
                    new ProductItem { ID = int.Parse(item) }
                    );
            }
            this.productCartRepeater.DataSource = productList;

            this.productCartRepeater.DataBind();
        }

        private void BindItemsInCart2(List<ORM_FORM.Form.ProductItem> ListOfSelectedProducts)
        {
            this.productCartRepeater.DataSource = ListOfSelectedProducts;
            this.productCartRepeater.DataBind();
        }


        protected void btnCollection_Click(object sender, EventArgs e)
        {
            /*
            List<string> coll = (List<string>)Session["products"];
            string bb = "";
            foreach (var item in coll)
            {
                bb += item + " , ";
            }

            lbl.Text = bb;

            //printValueListNum();
            //getTextBoxValue();
            gen();

            List<ORM_FORM.ProductInCart> producs = createProductCartList(itemlist, getTextBoxValue());
            string x = DeliveryList.SelectedValue;
            label.Text = x;
            */

            ///////OBJEDNAVKA////////
            //Vytvoreni objektu ORDER
            Order order = new Order();
            order.Customer.IdCustomer = (int)Session["idCustomer"];
            order.TypeDelivery.IdDelivery = int.Parse(DeliveryList.SelectedValue);
            order.Adress = Adresa.Text;
            order.City = Mesto.Text;
            order.PostalCode = PSC.Text;

            List<ORM_FORM.ProductInCart> producs = createProductCartList(itemlist, getTextBoxValue());

            OrderTable.CreateOrder(producs, order);



            Session["productsList"] = null;
            Response.Redirect("~/Form/Konec.aspx");

        }

        public List<string> getTextBoxValue()
        {
            List<string> num_list = new List<string>();

            foreach (RepeaterItem item in productCartRepeater.Items)
            {
                TextBox txt = (TextBox)item.FindControl("TxtBox");
                if (txt != null)
                {
                    //label.Text = txt.Text;
                    num_list.Add(txt.Text);

                }
            }
            return num_list;
        }

        public void gen()
        {
            List<string> num_lis = getTextBoxValue();
            string s = null;
            foreach (var item in num_lis)
            {
                s += item + " - ";
            }
            label.Text=s;
        }



        /*Vytvori list objednavek pro TSQL proceduru*/
        public List<ORM_FORM.ProductInCart> createProductCartList(List<ORM_FORM.Form.ProductItem> items, List<string> listNum)
        {
            List<ORM_FORM.ProductInCart> productCart = new List<ORM_FORM.ProductInCart>();
            int i = 0;
            foreach (ORM_FORM.Form.ProductItem item in items)
            {
                productCart.Add(new ORM_FORM.ProductInCart(item.ID, int.Parse(listNum[i])));                
                i++;
            }
            return productCart;
        }
                     



    }

}