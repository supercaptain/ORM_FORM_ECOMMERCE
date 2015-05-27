using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ORM_FORM
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idCustomer"] != null)
            {
                int result = (int)Session["idCustomer"];
                label1.Text = "uzivatel je s ID: " + result;
            }


        }
    }
}