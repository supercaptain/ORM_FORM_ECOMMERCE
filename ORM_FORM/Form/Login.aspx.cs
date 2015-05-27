using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ORM_FORM.Form
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void LoginUser_Click(object sender, EventArgs e)
        {
            string email = Email.Text;
            string pass = Password.Text;

            int result = CustomerTable.LoginUser(email, pass);
            label1.Text = "uzivatel je s ID: " + result;
            Session["idCustomer"] = result;
            Response.Redirect("~/Contact.aspx");

        }
    }
}