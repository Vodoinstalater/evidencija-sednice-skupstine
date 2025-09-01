using System;
using System.Web.UI;

namespace KorisnickiInterfejs
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set the page title
            Page.Title = "Početna - Evidencija sednica skupštine Srbije";
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Redirect to the login page
            Response.Redirect("~/Login.aspx");
        }
    }
}
