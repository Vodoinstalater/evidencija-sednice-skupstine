using System;
using System.Web.UI;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
    public partial class NoviSaziv : System.Web.UI.Page
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
                LoadCurrentSazivInfo();
                SetDefaultDates();
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri inicijalizaciji stranice: {ex.Message}", "danger");
            }
        }

        private void LoadCurrentSazivInfo()
        {
            try
            {
                var aktivanSaziv = SednicePregled.DajAktivanSaziv();
                
                if (aktivanSaziv != null)
                {
                    lblTrenutniSaziv.Text = aktivanSaziv.Ime ?? "Nepoznato";
                    
                    if (aktivanSaziv.DatumPocetka.HasValue && aktivanSaziv.DatumZavrsetka.HasValue)
                    {
                        lblPeriodSaziva.Text = $"{aktivanSaziv.DatumPocetka.Value:dd.MM.yyyy} - {aktivanSaziv.DatumZavrsetka.Value:dd.MM.yyyy}";
                    }
                    else
                    {
                        lblPeriodSaziva.Text = aktivanSaziv.PeriodFormatiran ?? "-";
                    }
                    
                    // Proveri da li je saziv aktivan
                    if (aktivanSaziv.Aktivan)
                    {
                        lblStatusSaziva.Text = "Aktivan";
                    }
                    else
                    {
                        lblStatusSaziva.Text = "Završen";
                    }
                }
                else
                {
                    lblTrenutniSaziv.Text = "Nema aktivnog saziva";
                    lblPeriodSaziva.Text = "-";
                    lblStatusSaziva.Text = "Nema";
                }
            }
            catch (Exception ex)
            {
                lblTrenutniSaziv.Text = "Greška pri učitavanju";
                lblPeriodSaziva.Text = "-";
                lblStatusSaziva.Text = "Greška";
                
                // Prikaži grešku korisniku
                ShowAlert($"Greška pri učitavanju trenutnog saziva: {ex.Message}", "warning");
            }
        }

        private void SetDefaultDates()
        {
            try
            {
                txtDatumPocetka.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtDatumZavrsetka.Text = DateTime.Today.AddYears(4).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                // Log greške pri postavljanju datuma
            }
        }

        protected void btnKonstituisiSaziv_Click(object sender, EventArgs e)
        {
            try
            {
                
                // Validacija
                if (string.IsNullOrWhiteSpace(txtNazivSaziva.Text.Trim()))
                {
                    ShowAlert("Naziv saziva je obavezan!", "danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDatumPocetka.Text))
                {
                    ShowAlert("Datum početka je obavezan!", "danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDatumZavrsetka.Text))
                {
                    ShowAlert("Datum završetka je obavezan!", "danger");
                    return;
                }

                // Kreiraj novi saziv
                string nazivSaziva = txtNazivSaziva.Text.Trim();
                DateTime datumPocetka = Convert.ToDateTime(txtDatumPocetka.Text);
                DateTime datumZavrsetka = Convert.ToDateTime(txtDatumZavrsetka.Text);
                string opisSaziva = txtOpisSaziva.Text.Trim();
                
                string poruka;

                bool uspesno = SednicePregled.KreirajNoviSaziv(nazivSaziva, datumPocetka, datumZavrsetka, opisSaziva, out poruka);

                if (uspesno)
                {
                    ShowAlert(poruka, "success");
                    ClearForm();
                    LoadCurrentSazivInfo(); // Osveži informacije o trenutnom sazivu
                }
                else
                {
                    ShowAlert(poruka, "danger");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri konstituisanju saziva: {ex.Message}", "danger");
            }
        }

        protected void btnResetuj_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtNazivSaziva.Text = string.Empty;
            txtOpisSaziva.Text = string.Empty;
            SetDefaultDates();
        }

        private void ShowAlert(string message, string alertType)
        {
            pnlAlert.Visible = true;
            pnlAlert.CssClass = $"alert alert-{alertType}";
            lblAlertMessage.Text = message;
        }
    }
}
