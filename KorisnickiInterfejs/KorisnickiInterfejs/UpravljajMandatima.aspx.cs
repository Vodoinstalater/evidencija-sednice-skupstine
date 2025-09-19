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
                CheckDatabaseInitialization();
            }
        }

        private void InitializePage()
        {
            try
            {
                EnsureSednicePregledInitialized();
                LoadCurrentSazivInfo();
                LoadDropdownData();
                LoadGridData();
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

        private void CheckDatabaseInitialization()
        {
            try
            {
                // Test database connection first
                bool connectionOk = TestDatabaseConnection();
                if (!connectionOk)
                {
                    ShowAlert("❌ GREŠKA KONEKCIJE SA BAZOM PODATAKA!<br/>" +
                             "Proverite da li je SQL Server pokrenut i da li je baza 'sednica3' dostupna.", "danger");
                    return;
                }

                // Use proper layer architecture - delegate to Layer 3B
                bool sistemSpreman = SednicePregled.DaLiJeSistemSpremanZaRad(out string poruka);
                
                if (sistemSpreman)
                {
                    ShowAlert($"✅ {poruka}", "success");
                }
                else
                {
                    ShowAlert($"⚠️ {poruka}", "warning");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri proveri inicijalizacije: {ex.Message}", "danger");
            }
        }

        private bool TestDatabaseConnection()
        {
            try
            {
                DBUtils.KonekcijaKlasa konekcija = new DBUtils.KonekcijaKlasa();
                return konekcija.OtvoriKonekciju();
            }
            catch (Exception ex)
            {
                return false;
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
                if (stranke != null && stranke.Count > 0)
                {
                    foreach (var stranka in stranke)
                    {
                        ddlStranka.Items.Add(new ListItem(stranka.Naziv, stranka.Id.ToString()));
                    }
                }
                else
                {
                    ShowAlert("Nema stranaka u sistemu. Dodajte stranke pre dodavanja lica.", "warning");
                }
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
                if (pozicije != null && pozicije.Count > 0)
                {
                    foreach (var pozicija in pozicije)
                    {
                        ddlPozicija.Items.Add(new ListItem(pozicija.Naziv, pozicija.Id.ToString()));
                    }
                }
                else
                {
                    ShowAlert("Nema pozicija u sistemu. Proverite konfiguraciju.", "warning");
                }
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
                ddlLica.Items.Clear();
                ddlLica.Items.Add(new ListItem("-- Izaberite lice --", ""));
                
                // Učitaj sva lica iz baze
                var svaLica = SednicePregled.DajSvaLica();
                if (svaLica != null && svaLica.Count > 0)
                {
                    foreach (var lice in svaLica)
                    {
                        string displayText = $"{lice.Ime} {lice.Prezime} ({lice.KorisnickoIme})";
                        ddlLica.Items.Add(new ListItem(displayText, lice.Id.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri učitavanju lica u dropdown: {ex.Message}", "danger");
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
                
                if (svaLica == null || svaLica.Count == 0)
                {
                    ShowAlert("Nema lica u sistemu.", "info");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri učitavanju lica: {ex.Message}", "danger");
            }
        }

        private void LoadMandatiGrid()
        {
            try
            {
                var mandati = SednicePregled.DajMandateZaAktivanSaziv();
                gvMandati.DataSource = mandati;
                gvMandati.DataBind();
                
                if (mandati == null || mandati.Count == 0)
                {
                    ShowAlert("Nema mandata za aktivan saziv.", "info");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri učitavanju mandata: {ex.Message}", "danger");
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

                // Proveri da li postoji aktivan saziv
                var aktivanSaziv = SednicePregled.DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    ShowAlert("Ne postoji aktivan saziv! Prvo kreirajte novi saziv pre dodavanja lica.", "danger");
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
                    ShowAlert($"Greška: {poruka}", "danger");
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
