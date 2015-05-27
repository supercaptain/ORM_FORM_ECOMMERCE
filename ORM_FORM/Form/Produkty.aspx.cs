using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ORM_FORM.Form
{
    public partial class Produkty : System.Web.UI.Page
    {
        List<string> lister;
        List<ORM_FORM.Form.ProductItem> itemlist;
        protected void Page_Load(object sender, EventArgs e)
        {
            lister = (List<string>)Session["products"] ?? new List<string>();
            itemlist = (List<ORM_FORM.Form.ProductItem>)Session["productsList"] ?? new List<ORM_FORM.Form.ProductItem>();
        }

        protected void GridViewProduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridViewProduct.Rows[index];
                //DataRowView rowView =  row.DataItem;
                GridView g = (GridView)sender;
                string idproduct = g.Rows[index].Cells[0].Text;
                lister.Add(idproduct);
                Session["products"] = lister;


                //////////////////////////////////////////
                ///////////////////////////////////////////
                int ID = 0;
                string CustomerID = g.Rows[index].Cells[0].Text;
                string Nameproduct = g.Rows[index].Cells[1].Text;
                ID = Int32.Parse(CustomerID);
                //lbl.Text = ID + " " + Nameproduct;

                ORM_FORM.Form.ProductItem product = new ORM_FORM.Form.ProductItem();
                product.ID = ID;
                product.Name = Nameproduct;
                lbl.Text = product.ID + " " + product.Name;
                itemlist.Add(product);
                Session["productsList"] = itemlist;
                ////////////////////////////////////////////
                ///////////////////////////////////////////      
             
            }
        }

        protected void btnCollection_Click(object sender, EventArgs e)
        {         
            //predelat
            List<ORM_FORM.Form.ProductItem> coll = (List<ORM_FORM.Form.ProductItem>)Session["productsList"];
            string bb = "";
            foreach (var item in coll)
            {
                bb += item.ID + "-" +  item.Name +  " ----- ";
            }

            lbl.Text = bb;
            
        }
    }
}