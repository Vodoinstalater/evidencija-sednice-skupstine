using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrezentacionaLogika;
using KlaseMapiranja;

namespace KorisnickiInterfejs
{
    public partial class PogledajMandate : System.Web.UI.Page
    {
        private SednicePregledKlasa _sednicePregled;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializePage();
                LoadInitialData();
            }
        }

        private void InitializePage()
        {
            _sednicePregled = new SednicePregledKlasa();
        }

        /// <summary>
        /// Osigurava da je _sednicePregled inicijalizovan pre korišćenja
        /// </summary>
        private void EnsureSednicePregledInitialized()
        {
            if (_sednicePregled == null)
            {
                _sednicePregled = new SednicePregledKlasa();
            }
        }

        private void LoadInitialData()
        {
            try
            {
                EnsureSednicePregledInitialized();
                // Učitaj dropdown-ove
                LoadSazivi();
                LoadPozicije();
                LoadStranke();
                
                // Učitaj statistike
                LoadStatistics();
                
                // Učitaj početne podatke
                LoadMandate();
            }
            catch (Exception ex)
            {
                ShowError("Greška pri učitavanju podataka. Molimo pokušajte ponovo.");
            }
        }

        private void LoadSazivi()
        {
            try
            {
                EnsureSednicePregledInitialized();
                var sazivi = _sednicePregled.DajSveSazive();
                ddlSaziv.Items.Clear();
                ddlSaziv.Items.Add(new ListItem("-- Svi sazivi --", ""));
                
                foreach (var saziv in sazivi)
                {
                    ddlSaziv.Items.Add(new ListItem(saziv.Ime, saziv.Id.ToString()));
                }
                
            }
            catch (Exception ex)
            {
                // Log greške pri učitavanju saziva
            }
        }

        private void LoadPozicije()
        {
            try
            {
                EnsureSednicePregledInitialized();
                var pozicije = _sednicePregled.DajSvePozicije();
                ddlPozicija.Items.Clear();
                ddlPozicija.Items.Add(new ListItem("-- Sve pozicije --", ""));
                
                foreach (var pozicija in pozicije)
                {
                    ddlPozicija.Items.Add(new ListItem(pozicija.Naziv, pozicija.Naziv));
                }
                
            }
            catch (Exception ex)
            {
                // Fallback na hardkodirane vrednosti ako servis ne radi
                ddlPozicija.Items.Clear();
                ddlPozicija.Items.Add(new ListItem("-- Sve pozicije --", ""));
                ddlPozicija.Items.Add(new ListItem("Poslanik", "Poslanik"));
                ddlPozicija.Items.Add(new ListItem("Predsednik", "Predsednik"));
                ddlPozicija.Items.Add(new ListItem("Potpredsednik", "Potpredsednik"));
            }
        }

        private void LoadStranke()
        {
            try
            {
                EnsureSednicePregledInitialized();
                var stranke = _sednicePregled.DajSveStranke();
                ddlStranka.Items.Clear();
                ddlStranka.Items.Add(new ListItem("-- Sve stranke --", ""));
                
                foreach (var stranka in stranke)
                {
                    ddlStranka.Items.Add(new ListItem(stranka.Naziv, stranka.Naziv));
                }
                
            }
            catch (Exception ex)
            {
                // Fallback na hardkodirane vrednosti ako servis ne radi
                ddlStranka.Items.Clear();
                ddlStranka.Items.Add(new ListItem("-- Sve stranke --", ""));
                ddlStranka.Items.Add(new ListItem("SNS", "SNS"));
                ddlStranka.Items.Add(new ListItem("SPS", "SPS"));
                ddlStranka.Items.Add(new ListItem("DS", "DS"));
                ddlStranka.Items.Add(new ListItem("Dveri", "Dveri"));
                ddlStranka.Items.Add(new ListItem("Nezavisni", "Nezavisni"));
            }
        }



        private void LoadStatistics()
        {
            try
            {
                EnsureSednicePregledInitialized();
                var mandate = _sednicePregled.DajSveMandate();
                
                if (mandate != null && mandate.Count > 0)
                {
                    int ukupno = mandate.Count;
                    int aktivnih = mandate.Count; // Svi mandate su aktivni u trenutnom sazivu
                    int poslanika = mandate.Select(m => m.IdLica).Distinct().Count();
                    int stranaka = mandate.Select(m => m.NazivStranke).Distinct().Count();

                    lblUkupnoMandata.Text = ukupno.ToString();
                    lblAktivnihMandata.Text = aktivnih.ToString();
                    lblPoslanika.Text = poslanika.ToString();
                    lblStranaka.Text = stranaka.ToString();
                }
                else
                {
                    lblUkupnoMandata.Text = "0";
                    lblAktivnihMandata.Text = "0";
                    lblPoslanika.Text = "0";
                    lblStranaka.Text = "0";
                }
            }
            catch (Exception ex)
            {
                // Postavi default vrednosti
                lblUkupnoMandata.Text = "0";
                lblAktivnihMandata.Text = "0";
                lblPoslanika.Text = "0";
                lblStranaka.Text = "0";
            }
        }

        private void LoadMandate()
        {
            try
            {
                EnsureSednicePregledInitialized();
                
                // Učitaj realne podatke iz baze podataka
                var mandate = _sednicePregled.DajSveMandate();
                
                if (mandate != null && mandate.Count > 0)
                {
                    var dt = ConvertMandateToDataTable(mandate);
                    BindMandateToGrid(dt);
                    UpdateResultsCount(dt.Rows.Count);
                }
                else
                {
                    BindMandateToGrid(new DataTable());
                    UpdateResultsCount(0);
                }
            }
            catch (Exception ex)
            {
                ShowError("Greška pri učitavanju mandata. Molimo pokušajte ponovo.");
            }
        }

        private DataTable ConvertMandateToDataTable(List<MandatDTO> mandate)
        {
            var dt = new DataTable();
            dt.Columns.Add("ImePrezime", typeof(string));
            dt.Columns.Add("NazivStranke", typeof(string));
            dt.Columns.Add("NazivPozicije", typeof(string));
            dt.Columns.Add("NazivSaziva", typeof(string));
            dt.Columns.Add("DatumMandata", typeof(DateTime));

            if (mandate != null && mandate.Count > 0)
            {
                foreach (var mandat in mandate)
                {
                    dt.Rows.Add(
                        mandat.ImeLica ?? "",
                        mandat.NazivStranke ?? "",
                        mandat.NazivPozicije ?? "",
                        mandat.NazivSaziva ?? "",
                        DateTime.Now // Placeholder - možemo dodati stvarni datum kasnije
                    );
                }
            }

            return dt;
        }

        protected void btnPretrazi_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                
                // Prikupljaj filtere
                int? sazivId = null;
                if (!string.IsNullOrEmpty(ddlSaziv.SelectedValue) && ddlSaziv.SelectedValue != "")
                {
                    int.TryParse(ddlSaziv.SelectedValue, out int tempId);
                    sazivId = tempId;
                }
                
                string pozicijaNaziv = ddlPozicija.SelectedValue;
                if (pozicijaNaziv == "-- Sve pozicije --")
                {
                    pozicijaNaziv = null;
                }
                
                string strankaNaziv = ddlStranka.SelectedValue;
                if (strankaNaziv == "-- Sve stranke --")
                {
                    strankaNaziv = null;
                }
                
                string imePrezime = txtImePrezime.Text.Trim();
                
                // Koristi novu filter metodu - za sada ne prosleđujemo poziciju i stranku ID jer koristimo nazive
                var mandate = _sednicePregled.PretraziMandate(sazivId, null, null, imePrezime);
                
                if (mandate != null && mandate.Count > 0)
                {
                    // Primeni pozicija filter ako je postavljen
                    if (!string.IsNullOrEmpty(pozicijaNaziv))
                    {
                        var preFilterCount = mandate.Count;
                        mandate = mandate.Where(m => 
                            !string.IsNullOrEmpty(m.NazivPozicije) && 
                            m.NazivPozicije.Equals(pozicijaNaziv, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }
                    
                    // Primeni stranka filter ako je postavljen
                    if (!string.IsNullOrEmpty(strankaNaziv))
                    {
                        var preFilterCount = mandate.Count;
                        mandate = mandate.Where(m => 
                            !string.IsNullOrEmpty(m.NazivStranke) && 
                            m.NazivStranke.Equals(strankaNaziv, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }
                    
                    var dt = ConvertMandateToDataTable(mandate);
                    BindMandateToGrid(dt);
                    UpdateResultsCount(dt.Rows.Count);
                }
                else
                {
                    BindMandateToGrid(new DataTable());
                    UpdateResultsCount(0);
                }
            }
            catch (Exception ex)
            {
                ShowError("Greška pri pretraživanju. Molimo pokušajte ponovo.");
            }
        }

        protected void btnResetuj_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                // Resetuj sve filtere
                txtImePrezime.Text = "";
                ddlSaziv.SelectedIndex = 0;
                ddlPozicija.SelectedIndex = 0;
                ddlStranka.SelectedIndex = 0;
                
                // Učitaj sve podatke
                LoadMandate();
            }
            catch (Exception ex)
            {
                ShowError("Greška pri resetovanju filtera. Molimo pokušajte ponovo.");
            }
        }

        protected void gvMandate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                gvMandate.PageIndex = e.NewPageIndex;
                LoadMandate(); // Ponovo učitaj podatke za novu stranicu
            }
            catch (Exception ex)
            {
                ShowError("Greška pri promeni stranice. Molimo pokušajte ponovo.");
            }
        }

        private void BindMandateToGrid(DataTable mandate)
        {
            if (mandate != null && mandate.Rows.Count > 0)
            {
                gvMandate.DataSource = mandate;
                gvMandate.DataBind();
            }
            else
            {
                gvMandate.DataSource = null;
                gvMandate.DataBind();
            }
        }

        private void UpdateResultsCount(int count)
        {
            lblBrojRezultata.Text = $"Pronađeno: {count} mandata";
        }

        private void ShowError(string message)
        {
            // Možete implementirati prikaz greške (npr. Label ili JavaScript alert)
        }
    }
}
