using System;
using System.Web.UI;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
    public partial class Dashboard : System.Web.UI.Page
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

                // Učitaj korisničke podatke i statistike
                LoadDashboardData();
            }
        }

        private void LoadDashboardData()
        {
            try
            {
                var user = SednicaAdmin.GetLoggedInUser();
                if (user != null)
                {
                    // Postavi naslov stranice
                    Page.Title = $"Dashboard - {user.KorisnickoIme}";
                    
                    // Učitaj dashboard statistike
                    var dashboardKlasa = new SednicaDashboardKlasa();
                    
                    // Postavi welcome poruku
                    string tipKorisnika = user.TipKorisnika ?? "Korisnik";
                    string imePrezime = user.ImePrezime ?? user.KorisnickoIme;
                    lblWelcomeMessage.Text = dashboardKlasa.GenerisiWelcomeMessage(imePrezime, tipKorisnika);
                    
                    // Učitaj pojedinačne statistike
                    var brojSednica = dashboardKlasa.DajBrojSednica();
                    var brojZasedanja = dashboardKlasa.DajBrojZasedanja();
                    var brojMandata = dashboardKlasa.DajBrojMandata();
                    var aktivanSaziv = dashboardKlasa.DajAktivanSaziv();
                    var poslednjaAktivnost = dashboardKlasa.DajPoslednuAktivnost();
                    
                    // Popuni statistike na UI
                    lblBrojSednica.Text = brojSednica.ToString();
                    lblBrojZasedanja.Text = brojZasedanja.ToString();
                    lblBrojMandata.Text = brojMandata.ToString();
                    lblAktivanSaziv.Text = aktivanSaziv;
                    lblPoslednjaAktivnost.Text = poslednjaAktivnost;
                }
                else
                {
                    lblWelcomeMessage.Text = "Dobrodošli u sistem evidencije sednica!";
                }
            }
            catch (Exception ex)
            {
                lblWelcomeMessage.Text = "Greška pri učitavanju korisničkih podataka.";
                // Postavi default statistike
                lblBrojSednica.Text = "0";
                lblBrojZasedanja.Text = "0";
                lblBrojMandata.Text = "0";
                lblAktivanSaziv.Text = "N/A";
                lblPoslednjaAktivnost.Text = "Greška pri učitavanju podataka.";
            }
        }
    }
}
