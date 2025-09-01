using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
    public partial class NovoZasedanje : System.Web.UI.Page
    {
        private SednicePregledKlasa _sednicePregled;

        // Property to ensure _sednicePregled is always initialized
        private SednicePregledKlasa SednicePregled
        {
            get
            {
                if (_sednicePregled == null)
                {
                    _sednicePregled = new SednicePregledKlasa();
                }
                return _sednicePregled;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializePage();
            }
        }

        private void InitializePage()
        {
            try
            {
                LoadCurrentSessionInfo();
                LoadTipoviZasedanja();
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri inicijalizaciji stranice: {ex.Message}", "danger");
            }
        }

        private void EnsureSednicePregledInitialized()
        {
            try
            {
                // The property will automatically initialize the object if it's null
                var temp = SednicePregled;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Greška pri inicijalizaciji SednicePregledKlasa", ex);
            }
        }

        private void LoadCurrentSessionInfo()
        {
            try
            {
                var trenutnoZasedanje = SednicePregled.DajTrenutnoZasedanje();
                
                if (trenutnoZasedanje != null)
                {
                    lblTrenutnoZasedanje.Text = trenutnoZasedanje.Naziv ?? "Nepoznato";
                    lblTipZasedanja.Text = trenutnoZasedanje.TipZasedanja ?? "Nepoznato";
                    lblSaziv.Text = trenutnoZasedanje.NazivSaziva ?? "Nepoznato";
                    
                    // Proveri da li je saziv aktivan
                    if (trenutnoZasedanje.DatumZavrsetka > DateTime.Now)
                    {
                        lblStatus.Text = "Aktivan";
                    }
                    else
                    {
                        lblStatus.Text = "Završen";
                    }
                }
                else
                {
                    lblTrenutnoZasedanje.Text = "Nema aktivnog zasedanja";
                    lblTipZasedanja.Text = "-";
                    lblSaziv.Text = "-";
                    lblStatus.Text = "Nema";
                }
            }
            catch (Exception ex)
            {
                lblTrenutnoZasedanje.Text = "Greška pri učitavanju";
                lblTipZasedanja.Text = "-";
                lblSaziv.Text = "-";
                lblStatus.Text = "Greška";
                
                // Prikaži grešku korisniku
                ShowAlert($"Greška pri učitavanju trenutnog zasedanja: {ex.Message}", "warning");
            }
        }

        private void LoadTipoviZasedanja()
        {
            try
            {
                // Tipovi su već definisani u ASPX fajlu
                // Ovo je mesto gde bi se mogli dinamički učitati iz baze ako je potrebno
            }
            catch (Exception ex)
            {
                // Log greške pri učitavanju tipova zasedanja
            }
        }

        protected void btnKreirajZasedanje_Click(object sender, EventArgs e)
        {
            try
            {
                // Validacija
                if (string.IsNullOrWhiteSpace(txtNazivZasedanja.Text.Trim()))
                {
                    ShowAlert("Naziv zasedanja je obavezan!", "danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ddlTipZasedanja.SelectedValue))
                {
                    ShowAlert("Tip zasedanja je obavezan!", "danger");
                    return;
                }

                // Kreiraj novo zasedanje
                string nazivZasedanja = txtNazivZasedanja.Text.Trim();
                int tipZasedanja = Convert.ToInt32(ddlTipZasedanja.SelectedValue);
                string poruka;

                bool uspesno = SednicePregled.KreirajNovoZasedanje(nazivZasedanja, tipZasedanja, out poruka);

                if (uspesno)
                {
                    ShowAlert(poruka, "success");
                    ClearForm();
                    LoadCurrentSessionInfo(); // Osveži informacije o trenutnom zasedanju
                }
                else
                {
                    ShowAlert(poruka, "danger");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri kreiranju zasedanja: {ex.Message}", "danger");
            }
        }

        protected void btnResetuj_Click(object sender, EventArgs e)
        {
            ClearForm();
            ShowAlert("Forma je resetovana.", "info");
        }

        private void ClearForm()
        {
            txtNazivZasedanja.Text = string.Empty;
            ddlTipZasedanja.SelectedIndex = 0;
        }

        private void ShowAlert(string message, string alertType)
        {
            pnlAlert.Visible = true;
            lblAlertMessage.Text = message;
            
            // Ukloni sve CSS klase
            pnlAlert.CssClass = "alert";
            
            // Dodaj odgovarajuću CSS klasu
            switch (alertType.ToLower())
            {
                case "success":
                    pnlAlert.CssClass += " alert-success";
                    break;
                case "danger":
                    pnlAlert.CssClass += " alert-danger";
                    break;
                case "info":
                    pnlAlert.CssClass += " alert-info";
                    break;
                default:
                    pnlAlert.CssClass += " alert-info";
                    break;
            }
        }
    }
}
