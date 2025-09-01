using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using PrezentacionaLogika;
using System.Configuration;

namespace KorisnickiInterfejs
{
    public partial class Login1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // sakrij status panel na početku
                StatusPanel.Visible = false;
            }
        }
                      
        protected void PrijavaButton_Click(object sender, EventArgs e)
        {
            try
            {
                // validacija unosa
                if (string.IsNullOrWhiteSpace(KorisnickoImeTextBox.Text) || 
                    string.IsNullOrWhiteSpace(SifraTextBox.Text))
                {
                    PrikaziGresku("Molimo unesite korisničko ime i šifru!");
                    return;
                }

                // provera korisnika pomoću Sednica login sistema
                SednicaLoginKlasa sednicaLogin = new SednicaLoginKlasa();
                sednicaLogin.KorisnickoIme = KorisnickoImeTextBox.Text.Trim();
                sednicaLogin.Sifra = SifraTextBox.Text.Trim();
                
                bool pronadjenKorisnik = sednicaLogin.VazeciKorisnik();

                if (pronadjenKorisnik)
                {
                    // uspešna prijava - čuvaj korisničke podatke u novom formatu sesije
                    string imePrezime = sednicaLogin.DajImePrezimeKorisnika();
                    string tipKorisnika = sednicaLogin.DajTipKorisnika();
                    int pozicijaId = sednicaLogin.DajPozicijuKorisnika();

                    // kreiraj user objekat za novu sesiju
                    var userData = new {
                        KorisnickoIme = sednicaLogin.KorisnickoIme,
                        ImePrezime = imePrezime,
                        TipKorisnika = tipKorisnika,
                        Pozicija = pozicijaId
                    };

                    // čuvanje u novom session formatu
                    Session["UserData"] = userData;
                    
                    // čuvanje za backward compatibility
                    Session["KorisnikImePrezime"] = imePrezime;
                    Session["TipKorisnika"] = tipKorisnika;
                    Session["PozicijaId"] = pozicijaId;
                    Session["KorisnickoIme"] = sednicaLogin.KorisnickoIme;
                    Session["SednicaLoginObjekat"] = sednicaLogin;

                    // Uspesna prijava - sacuvaj korisnika u sesiju
                    Session["SednicaLoginObjekat"] = sednicaLogin;

                    // redirekcija na Dashboard sa novim master page-om
                    Response.Redirect("Dashboard.aspx");
                }
                else
                {
                    PrikaziGresku("Neispravno korisničko ime ili šifra!");
                }
            }
            catch (Exception ex)
            {
                // logiraj grešku i prikaži generičku poruku
                PrikaziGresku("Greška prilikom prijave. Molimo pokušajte ponovo.");
            }
        }

        protected void OdustaniButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        private void PrikaziGresku(string poruka)
        {
            lblStatus.Text = poruka;
            StatusPanel.Visible = true;
        }
    }
}