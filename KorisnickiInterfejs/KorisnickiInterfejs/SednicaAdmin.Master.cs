using System;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;

namespace KorisnickiInterfejs
{
    public partial class SednicaAdmin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Proveri da li je korisnik ulogovan
                if (!IsUserLoggedIn())
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                // Učitaj korisničke podatke
                LoadUserInfo();
                
                // Postavi role-based navigation
                SetupRoleBasedNavigation();
            }
        }

        private void LoadUserInfo()
        {
            try
            {
                var user = GetLoggedInUser();
                if (user != null)
                {
                    userName.InnerText = user.KorisnickoIme;
                    userRole.InnerText = GetRoleDisplayName(user.Pozicija);
                }
            }
            catch (Exception ex)
            {
                userName.InnerText = "Greška";
                userRole.InnerText = "Nepoznato";
            }
        }

        private void SetupRoleBasedNavigation()
        {
            try
            {
                var user = GetLoggedInUser();
                if (user == null) return;

                // Sakrij sve role-specific panele
                predsednikFunctions.Visible = false;
                potpredsednikFunctions.Visible = false;
                poslanikFunctions.Visible = false;

                // Prikaži odgovarajuće funkcije na osnovu role
                switch (user.Pozicija)
                {
                    case 2: // Predsednik
                        predsednikFunctions.Visible = true;
                        break;
                    case 3: // Potpredsednik
                        potpredsednikFunctions.Visible = true;
                        break;
                    case 1: // Poslanik
                    case 4: // Poslanik
                    case 5: // Poslanik
                    case 6: // Poslanik
                        poslanikFunctions.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log greške pri postavljanju role-based navigacije
            }
        }

        private string GetRoleDisplayName(int pozicija)
        {
            switch (pozicija)
            {
                case 2: return "Predsednik";
                case 3: return "Potpredsednik";
                case 1:
                case 4:
                case 5:
                case 6: return "Poslanik";
                default: return "Nepoznato";
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Očisti sesiju
            if (Session["UserData"] != null)
            {
                Session.Remove("UserData");
            }
            
            // Preusmeri na login
            Response.Redirect("~/Login.aspx");
        }

        // Static helper methods for session management
        public static bool IsUserLoggedIn()
        {
            return HttpContext.Current.Session["UserData"] != null;
        }

        public static dynamic GetLoggedInUser()
        {
            return HttpContext.Current.Session["UserData"];
        }

        public static bool HasPermission(int requiredPosition)
        {
            var user = GetLoggedInUser();
            if (user == null) return false;
            
            return user.Pozicija == requiredPosition;
        }
    }
}
