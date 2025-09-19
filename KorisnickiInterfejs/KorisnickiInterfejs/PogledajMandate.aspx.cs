using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
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
                try
                {
                    _sednicePregled = new SednicePregledKlasa();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Greška pri inicijalizaciji SednicePregledKlasa: {ex.Message}");
                    // Ako ne može da se inicijalizuje, kreiraj novi objekat
                    _sednicePregled = new SednicePregledKlasa();
                }
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
                
                // Učitaj početne podatke
                LoadMandate();
                
                // Učitaj statistike na kraju
                LoadStatistics();
            }
            catch (Exception ex)
            {
                // Log grešku ali ne prikazuj je korisniku
                System.Diagnostics.Debug.WriteLine($"Greška pri učitavanju početnih podataka: {ex.Message}");
                
                // Postavi default vrednosti
                lblUkupnoMandata.Text = "0";
                lblAktivnihMandata.Text = "0";
                lblPoslanika.Text = "0";
                lblStranaka.Text = "0";
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
                
                if (_sednicePregled != null)
                {
                    // Uzmi sve mandate za statistike
                    var sviMandati = _sednicePregled.DajSveMandate();
                    var aktivniMandati = _sednicePregled.DajMandateZaAktivanSaziv();
                    
                    if (sviMandati != null && sviMandati.Count > 0)
                    {
                        int ukupno = sviMandati.Count;
                        int aktivnih = aktivniMandati?.Count ?? 0;
                        int poslanika = sviMandati.Select(m => m.IdLica).Distinct().Count();
                        int stranaka = sviMandati.Select(m => m.NazivStranke).Distinct().Count();

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
                
                if (_sednicePregled != null)
                {
                    // Učitaj sve mandate
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
                else
                {
                    BindMandateToGrid(new DataTable());
                    UpdateResultsCount(0);
                }
            }
            catch (Exception ex)
            {
                // Log grešku ali ne prikazuj je korisniku
                System.Diagnostics.Debug.WriteLine($"Greška pri učitavanju mandata: {ex.Message}");
                
                // Prikaži prazan rezultat
                BindMandateToGrid(new DataTable());
                UpdateResultsCount(0);
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
                        $"{mandat.ImeLica ?? ""} {mandat.PrezimeLica ?? ""}".Trim(),
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
                // Osiguraj da je _sednicePregled inicijalizovan
                EnsureSednicePregledInitialized();
                
                // Uzmi sve mandate i filtriraj ih u UI layer-u
                var sviMandati = _sednicePregled.DajSveMandate();
                
                if (sviMandati != null && sviMandati.Count > 0)
                {
                    var filtriraniMandati = sviMandati;
                    
                    // Primeni saziv filter
                    if (!string.IsNullOrEmpty(ddlSaziv.SelectedValue) && ddlSaziv.SelectedValue != "")
                    {
                        if (int.TryParse(ddlSaziv.SelectedValue, out int sazivId))
                        {
                            filtriraniMandati = filtriraniMandati.Where(m => m.IdSaziva == sazivId).ToList();
                        }
                    }
                    
                    // Primeni pozicija filter
                    string pozicijaNaziv = ddlPozicija.SelectedValue;
                    if (!string.IsNullOrEmpty(pozicijaNaziv) && pozicijaNaziv != "-- Sve pozicije --")
                    {
                        filtriraniMandati = filtriraniMandati.Where(m => 
                            !string.IsNullOrEmpty(m.NazivPozicije) && 
                            m.NazivPozicije.Equals(pozicijaNaziv, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }
                    
                    // Primeni stranka filter
                    string strankaNaziv = ddlStranka.SelectedValue;
                    if (!string.IsNullOrEmpty(strankaNaziv) && strankaNaziv != "-- Sve stranke --")
                    {
                        filtriraniMandati = filtriraniMandati.Where(m => 
                            !string.IsNullOrEmpty(m.NazivStranke) && 
                            m.NazivStranke.Equals(strankaNaziv, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }
                    
                    // Primeni ime/prezime filter - siguran pristup
                    string imePrezime = txtImePrezime.Text.Trim();
                    if (!string.IsNullOrEmpty(imePrezime))
                    {
                        try
                        {
                            var tempList = new List<MandatDTO>();
                            foreach (var m in filtriraniMandati)
                            {
                                if (m != null)
                                {
                                    // Bezbedno kreiranje kombinovanog imena
                                    string ime = m.ImeLica ?? "";
                                    string prezime = m.PrezimeLica ?? "";
                                    string imePrezimeKombinacija = $"{ime} {prezime}".Trim();
                                    
                                    // Pretraži u kombinovanom polju
                                    if (!string.IsNullOrEmpty(imePrezimeKombinacija) && 
                                        imePrezimeKombinacija.IndexOf(imePrezime, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        tempList.Add(m);
                                    }
                                }
                            }
                            filtriraniMandati = tempList;
                        }
                        catch (Exception nameEx)
                        {
                            // Ako ime/prezime filter pravi problem, preskoči ga
                            System.Diagnostics.Debug.WriteLine($"Greška pri ime/prezime filteru: {nameEx.Message}");
                        }
                    }
                    
                    var dt = ConvertMandateToDataTable(filtriraniMandati);
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
                // Log grešku ali ne prikazuj je korisniku da ne bi došlo do logout-a
                System.Diagnostics.Debug.WriteLine($"Greška pri pretraživanju mandata: {ex.Message}");
                
                // Prikaži prazan rezultat umesto greške
                BindMandateToGrid(new DataTable());
                UpdateResultsCount(0);
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
                
                // Sačuvaj podatke u ViewState za export funkcionalnost
                ViewState["CurrentMandateData"] = mandate;
            }
            else
            {
                gvMandate.DataSource = null;
                gvMandate.DataBind();
                
                // Očisti ViewState
                ViewState["CurrentMandateData"] = null;
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

        /// <summary>
        /// Exportuje trenutno prikazane mandate u CSV format
        /// </summary>
        protected void btnExportuj_Click(object sender, EventArgs e)
        {
            try
            {
                // Uzmi podatke direktno iz ViewState
                var mandate = GetCurrentGridViewData();
                
                if (mandate != null && mandate.Rows.Count > 0)
                {
                    // Koristi univerzalnu export funkcionalnost
                    ExportUtility.ExportToCSV(mandate, "mandati", Response);
                }
                else
                {
                    ShowError("Nema podataka za export. Molimo prvo pretražite mandate.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Greška pri exportovanju. Molimo pokušajte ponovo.");
            }
        }

        /// <summary>
        /// Dohvata trenutno prikazane podatke iz ViewState
        /// </summary>
        private DataTable GetCurrentGridViewData()
        {
            try
            {
                // Uzmi podatke iz ViewState
                if (ViewState["CurrentMandateData"] != null)
                {
                    return ViewState["CurrentMandateData"] as DataTable;
                }
                
                return new DataTable();
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// Dohvata listu aktivnih filtera za prikaz u stampi
        /// </summary>
        private List<string> GetActiveFilters()
        {
            var activeFilters = new List<string>();
            
            if (!string.IsNullOrEmpty(txtImePrezime.Text.Trim()))
                activeFilters.Add($"Ime/Prezime: {txtImePrezime.Text.Trim()}");
            if (ddlSaziv.SelectedIndex > 0)
                activeFilters.Add($"Saziv: {ddlSaziv.SelectedItem.Text}");
            if (ddlPozicija.SelectedIndex > 0)
                activeFilters.Add($"Pozicija: {ddlPozicija.SelectedItem.Text}");
            if (ddlStranka.SelectedIndex > 0)
                activeFilters.Add($"Stranka: {ddlStranka.SelectedItem.Text}");
            
            return activeFilters;
        }

        /// <summary>
        /// Generiše stampu trenutno prikazanih mandata
        /// </summary>
        protected void btnStampa_Click(object sender, EventArgs e)
        {
            try
            {
                // Uzmi podatke direktno iz ViewState
                var mandate = GetCurrentGridViewData();
                
                if (mandate != null && mandate.Rows.Count > 0)
                {
                    // Pripremi aktivne filtere za prikaz
                    var activeFilters = GetActiveFilters();
                    
                    // Koristi univerzalnu stampa funkcionalnost
                    ExportUtility.ExportToHTML(mandate, "stampa_mandati", "Stampa Mandata", 
                        "Skupština - Pregled mandata", activeFilters, Response);
                }
                else
                {
                    ShowError("Nema podataka za stampu. Molimo prvo pretražite mandate.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Greška pri generisanju stampe. Molimo pokušajte ponovo.");
            }
        }

    }
}
