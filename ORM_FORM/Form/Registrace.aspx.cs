using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ORM_FORM.Form
{
    public partial class Registrace : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            
            string email = Email.Text;
            string pass = Password.Text;
            string pass2 = ConfirmPassword.Text;
            string phone = PhoneTxt.Text;
            string company = CompanyTxt.Text;
            string fname = (string)StringOrNull(FnameTxt.Text);                //FnameTxt.Text;
            string lname = (string)StringOrNull(LnameTxt.Text);                //LnameTxt.Text;

            //label1.Text = email + " " + pass + " " + phone + " " + company + " " + lname + " " + fname;
            Customer customer = new Customer(lname, fname, email, pass, phone, company);         
            
            try
            {
                CustomerTable.CreateUser(customer);
                label1.Text = "Registrovano ";
                Response.Redirect("~/Form/Login.aspx");
            }
            catch
            {
                label1.Text = "Chyba ";
            }
            
        }

        private object StringOrNull<T>(T s)
        {
            if (s == null)
            {
                return DBNull.Value;
            }
            else return s;            
            
        }
    }
}