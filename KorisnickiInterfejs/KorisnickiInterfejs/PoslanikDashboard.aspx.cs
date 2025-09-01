using System;
using System.Web.UI;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
    public partial class PoslanikDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Proveri da li je korisnik ulogovan
                if (!SednicaAdmin.IsUserLoggedIn())
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                // Učitaj korisničke podatke
                LoadUserInfo();
            }
        }

        private void LoadUserInfo()
        {
            try
            {
                var user = SednicaAdmin.GetLoggedInUser();
                if (user != null)
                {
                    // Postavi naslov stranice
                    Page.Title = $"Poslanik Dashboard - {user.KorisnickoIme}";
                    
                    // Možete dodati dodatnu logiku za prikaz specifičnih podataka za poslanika
                    // Poslanik Dashboard učitavan za korisnika
                }
            }
            catch (Exception ex)
            {
                // Log greške pri učitavanju korisničkih podataka
            }
        }
    }
}
