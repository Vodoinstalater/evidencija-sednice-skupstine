using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
    public partial class UpravljajMandatima : System.Web.UI.Page
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
                LoadDropdownData();
                LoadGridData();
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
                    lblAktivanSaziv.Text = aktivanSaziv.Ime ?? "Nepoznato";
                    
                    if (aktivanSaziv.DatumPocetka.HasValue && aktivanSaziv.DatumZavrsetka.HasValue)
                    {
                        lblPeriodSaziva.Text = $"{aktivanSaziv.DatumPocetka.Value:dd.MM.yyyy} - {aktivanSaziv.DatumZavrsetka.Value:dd.MM.yyyy}";
                    }
                    else
                    {
                        lblPeriodSaziva.Text = aktivanSaziv.PeriodFormatiran ?? "-";
                    }
                    
                    // Učitaj broj mandata za aktivan saziv
                    var mandati = SednicePregled.DajSveMandate();
                    int brojAktivnihMandata = mandati.Count;
                    lblBrojMandata.Text = brojAktivnihMandata.ToString();
                }
                else
                {
                    lblAktivanSaziv.Text = "Nema aktivnog saziva";
                    lblPeriodSaziva.Text = "-";
                    lblBrojMandata.Text = "0";
                }
            }
            catch (Exception ex)
            {
                lblAktivanSaziv.Text = "Greška pri učitavanju";
                lblPeriodSaziva.Text = "-";
                lblBrojMandata.Text = "0";
                
                ShowAlert($"Greška pri učitavanju trenutnog saziva: {ex.Message}", "warning");
            }
        }

        private void LoadDropdownData()
        {
            try
            {
                // Učitaj stranke u dropdown
                LoadStrankeDropdown();
                
                // Učitaj pozicije u dropdown
                LoadPozicijeDropdown();
                
                // Učitaj lica u dropdown za mandate
                LoadLicaDropdown();
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri učitavanju podataka: {ex.Message}", "warning");
            }
        }

        private void LoadStrankeDropdown()
        {
            try
            {
                ddlStranka.Items.Clear();
                ddlStranka.Items.Add(new ListItem("-- Izaberite stranku --", ""));

                var stranke = SednicePregled.DajSveStranke();
                foreach (var stranka in stranke)
                {
                    ddlStranka.Items.Add(new ListItem(stranka.Naziv, stranka.Id.ToString()));
                }
                
                // Stranke učitane uspešno
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri učitavanju stranaka: {ex.Message}", "danger");
            }
        }

        private void LoadPozicijeDropdown()
        {
            try
            {
                ddlPozicija.Items.Clear();
                ddlPozicija.Items.Add(new ListItem("-- Izaberite poziciju --", ""));

                var pozicije = SednicePregled.DajSvePozicije();
                foreach (var pozicija in pozicije)
                {
                    ddlPozicija.Items.Add(new ListItem(pozicija.Naziv, pozicija.Id.ToString()));
                }
                
                // Pozicije učitane uspešno
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri učitavanju pozicija: {ex.Message}", "danger");
            }
        }

        private void LoadLicaDropdown()
        {
            try
            {
    
                // Za sada dodajemo neke test lica
                ddlLica.Items.Clear();
                ddlLica.Items.Add(new ListItem("-- Izaberite lice --", ""));
                
                // Učitaj sva lica iz baze
                var svaLica = SednicePregled.DajSvaLica();
                foreach (var lice in svaLica)
                {
                    string displayText = $"{lice.Ime} {lice.Prezime} ({lice.KorisnickoIme})";
                    ddlLica.Items.Add(new ListItem(displayText, lice.Id.ToString()));
                }
                
                // Lica učitana uspešno
            }
            catch (Exception ex)
            {
                // Log greške pri učitavanju lica
            }
        }

        private void LoadGridData()
        {
            try
            {
                // Učitaj sva lica u grid
                LoadSvaLicaGrid();
                
                // Učitaj mandate za aktivan saziv
                LoadMandatiGrid();
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri učitavanju podataka: {ex.Message}", "warning");
            }
        }

        private void LoadSvaLicaGrid()
        {
            try
            {
                var svaLica = SednicePregled.DajSvaLica();
                gvSvaLica.DataSource = svaLica;
                gvSvaLica.DataBind();
                
                // Grid sa lica učitan uspešno
            }
            catch (Exception ex)
            {
                // Log greške pri učitavanju grid-a sa lica
            }
        }

        private void LoadMandatiGrid()
        {
            try
            {
                var mandati = SednicePregled.DajMandateZaAktivanSaziv();
                gvMandati.DataSource = mandati;
                gvMandati.DataBind();
                
                // Grid sa mandatima učitan uspešno
            }
            catch (Exception ex)
            {
                // Log greške pri učitavanju grid-a sa mandatima
            }
        }

        protected void btnDodajLice_Click(object sender, EventArgs e)
        {
            try
            {
                
                // Validacija
                if (string.IsNullOrWhiteSpace(txtIme.Text.Trim()))
                {
                    ShowAlert("Ime je obavezno!", "danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrezime.Text.Trim()))
                {
                    ShowAlert("Prezime je obavezno!", "danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtKorisnickoIme.Text.Trim()))
                {
                    ShowAlert("Korisničko ime je obavezno!", "danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLozinka.Text))
                {
                    ShowAlert("Lozinka je obavezna!", "danger");
                    return;
                }

                if (txtLozinka.Text.Length < 6)
                {
                    ShowAlert("Lozinka mora imati bar 6 karaktera!", "danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ddlPozicija.SelectedValue))
                {
                    ShowAlert("Pozicija je obavezna!", "danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ddlStranka.SelectedValue))
                {
                    ShowAlert("Stranka je obavezna!", "danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDatumRodjenja.Text))
                {
                    ShowAlert("Datum rođenja je obavezan!", "danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ddlPol.SelectedValue))
                {
                    ShowAlert("Pol je obavezan!", "danger");
                    return;
                }

                // Kreiraj novo lice
                string ime = txtIme.Text.Trim();
                string prezime = txtPrezime.Text.Trim();
                string korisnickoIme = txtKorisnickoIme.Text.Trim();
                string lozinka = txtLozinka.Text;
                int pozicijaId = Convert.ToInt32(ddlPozicija.SelectedValue);
                int strankaId = Convert.ToInt32(ddlStranka.SelectedValue);
                DateTime datumRodjenja = Convert.ToDateTime(txtDatumRodjenja.Text);
                char pol = ddlPol.SelectedValue[0];
                string biografija = txtBiografija.Text.Trim();
                
                string poruka;

                bool uspesno = SednicePregled.KreirajNovoLice(ime, prezime, korisnickoIme, lozinka, pozicijaId, strankaId, pol, datumRodjenja, biografija, out poruka);

                if (uspesno)
                {
                    ShowAlert(poruka, "success");
                    ClearLiceForm();
                    LoadDropdownData();
                    LoadGridData();
                }
                else
                {
                    ShowAlert(poruka, "danger");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri dodavanju lica: {ex.Message}", "danger");
            }
        }

        protected void btnDodajMandat_Click(object sender, EventArgs e)
        {
            try
            {
                
                // Validacija
                if (string.IsNullOrWhiteSpace(ddlLica.SelectedValue))
                {
                    ShowAlert("Lice je obavezno!", "danger");
                    return;
                }

                // Kreiraj novi mandat
                int liceId = Convert.ToInt32(ddlLica.SelectedValue);
                string status = "Aktivan"; // Default status jer ne postoji u bazi
                
                string poruka;

                bool uspesno = SednicePregled.KreirajNoviMandat(liceId, status, out poruka);

                if (uspesno)
                {
                    ShowAlert(poruka, "success");
                    ClearMandatForm();
                    LoadCurrentSazivInfo();
                    LoadGridData();
                }
                else
                {
                    ShowAlert(poruka, "danger");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri dodavanju mandata: {ex.Message}", "danger");
            }
        }

        protected void btnResetujLice_Click(object sender, EventArgs e)
        {
            ClearLiceForm();
        }

        protected void btnResetujMandat_Click(object sender, EventArgs e)
        {
            ClearMandatForm();
        }

        private void ClearLiceForm()
        {
            txtIme.Text = string.Empty;
            txtPrezime.Text = string.Empty;
            txtKorisnickoIme.Text = string.Empty;
            txtLozinka.Text = string.Empty;
            txtBiografija.Text = string.Empty;
            ddlPozicija.SelectedIndex = 0;
            ddlStranka.SelectedIndex = 0;
            ddlPol.SelectedIndex = 0;
            txtDatumRodjenja.Text = string.Empty;
        }

        private void ClearMandatForm()
        {
            ddlLica.SelectedIndex = 0;
        }



        private void ShowAlert(string message, string alertType)
        {
            pnlAlert.Visible = true;
            pnlAlert.CssClass = $"alert alert-{alertType}";
            lblAlertMessage.Text = message;
        }
    }
}
