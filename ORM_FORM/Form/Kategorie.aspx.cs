using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ORM_FORM.Form
{
    public partial class Kategorie : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DetailCategory_DataBound(object sender, EventArgs e)
        {
            //DetailCategory.Rows[0].Visible = false;
            DetailCategory.Rows[0].Enabled = false;
        }

    }
}