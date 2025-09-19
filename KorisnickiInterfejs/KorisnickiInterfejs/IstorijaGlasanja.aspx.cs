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
    /// <summary>
    /// Stranica za pregled istorije glasanja - SAMO VIEW SISTEM, NEMA GLASANJA
    /// Prikazuje postojeće podatke o glasanjima iz baze podataka
    /// </summary>
    public partial class IstorijaGlasanja : System.Web.UI.Page
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
            
            // Datum filteri su uklonjeni - ne postavljamo više default datume
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
                LoadSednice();
                LoadPitanja();
                
                // Učitaj statistike
                LoadStatistics();
                
                // Učitaj početne podatke
                LoadGlasanja();
            }
            catch (Exception ex)
            {
                ShowError("Greška pri učitavanju podataka. Molimo pokušajte ponovo.");
            }
        }

        private void LoadSednice()
        {
            try
            {
                EnsureSednicePregledInitialized();
                var sednice = _sednicePregled.DajSveSednice();
                ddlSednica.Items.Clear();
                ddlSednica.Items.Add(new ListItem("-- Sve sednice --", ""));
                
                foreach (var sednica in sednice)
                {
                    ddlSednica.Items.Add(new ListItem(sednica.Naziv ?? "Bez naziva", sednica.Id.ToString()));
                }
            }
            catch (Exception ex)
            {
                // Fallback na hardkodirane vrednosti ako servis ne radi
                ddlSednica.Items.Clear();
                ddlSednica.Items.Add(new ListItem("-- Sve sednice --", ""));
                ddlSednica.Items.Add(new ListItem("Sednica 1", "1"));
                ddlSednica.Items.Add(new ListItem("Sednica 2", "2"));
                ddlSednica.Items.Add(new ListItem("Sednica 3", "3"));
            }
        }

        private void LoadPitanja()
        {
            try
            {
                EnsureSednicePregledInitialized();
                var pitanja = _sednicePregled.DajSvaPitanja();
                ddlPitanje.Items.Clear();
                ddlPitanje.Items.Add(new ListItem("-- Sva pitanja --", ""));
                
                foreach (var pitanje in pitanja)
                {
                    // Koristimo tekst pitanja kao display text i ID kao value
                    // Za sada koristimo tekst kao value, ali možemo implementirati ID kasnije
                    ddlPitanje.Items.Add(new ListItem(pitanje, pitanje));
                }
            }
            catch (Exception ex)
            {
                // Fallback na hardkodirane vrednosti ako servis ne radi
                ddlPitanje.Items.Clear();
                ddlPitanje.Items.Add(new ListItem("-- Sva pitanja --", ""));
                ddlPitanje.Items.Add(new ListItem("Pitanje 1", "1"));
                ddlPitanje.Items.Add(new ListItem("Pitanje 2", "2"));
                ddlPitanje.Items.Add(new ListItem("Pitanje 3", "3"));
            }
        }

        private void LoadStatistics()
        {
            try
            {
                EnsureSednicePregledInitialized();
                // Učitaj realne statistike iz baze podataka
                var glasanja = _sednicePregled.DajSvaGlasanja();
                
                if (glasanja != null && glasanja.Count > 0)
                {
                    int ukupno = glasanja.Count;
                    int za = glasanja.Count(g => g.Glas?.ToLower() == "za");
                    int protiv = glasanja.Count(g => g.Glas?.ToLower() == "protiv");
                    int uzdrzani = glasanja.Count(g => g.Glas?.ToLower() == "uzdrzan");

                    lblUkupnoGlasanja.Text = ukupno.ToString();
                    lblGlasanjaZa.Text = za.ToString();
                    lblGlasanjaProtiv.Text = protiv.ToString();
                    lblUzdrzanih.Text = uzdrzani.ToString();
                }
                else
                {
                    lblUkupnoGlasanja.Text = "0";
                    lblGlasanjaZa.Text = "0";
                    lblGlasanjaProtiv.Text = "0";
                    lblUzdrzanih.Text = "0";
                }
            }
            catch (Exception ex)
            {
                // Postavi default vrednosti
                lblUkupnoGlasanja.Text = "0";
                lblGlasanjaZa.Text = "0";
                lblGlasanjaProtiv.Text = "0";
                lblUzdrzanih.Text = "0";
            }
        }

        private void LoadGlasanja()
        {
            try
            {
                EnsureSednicePregledInitialized();
                // Učitaj realne podatke iz baze podataka
                var glasanja = _sednicePregled.DajSvaGlasanja();
                
                if (glasanja != null && glasanja.Count > 0)
                {
                    var dt = ConvertGlasanjaToDataTable(glasanja);
                    BindGlasanjaToGrid(dt);
                    UpdateResultsCount(dt.Rows.Count);
                }
                else
                {
                    BindGlasanjaToGrid(new DataTable());
                    UpdateResultsCount(0);
                }
            }
            catch (Exception ex)
            {
                ShowError("Greška pri učitavanju glasanja. Molimo pokušajte ponovo.");
            }
        }

        private DataTable ConvertGlasanjaToDataTable(List<GlasanjeDTO> glasanja)
        {
            var dt = new DataTable();
            dt.Columns.Add("NazivSednice", typeof(string));
            dt.Columns.Add("Pitanje", typeof(string));
            dt.Columns.Add("DatumGlasanja", typeof(DateTime));
            dt.Columns.Add("BrojGlasovaZa", typeof(int));
            dt.Columns.Add("BrojGlasovaProtiv", typeof(int));
            dt.Columns.Add("BrojUzdrzanih", typeof(int));
            dt.Columns.Add("Rezultat", typeof(string));
            dt.Columns.Add("ProcenatUcesca", typeof(double));

            // Grupiši glasanja po pitanju i sednici
            var grupisanaGlasanja = glasanja
                .GroupBy(g => new { g.IdPitanja, g.NazivSednice })
                .Select(g => new
                {
                    NazivSednice = g.Key.NazivSednice,
                    Pitanje = g.FirstOrDefault()?.TekstPitanja ?? "",
                    DatumGlasanja = g.FirstOrDefault()?.DatumGlasanja ?? DateTime.Now,
                    BrojGlasovaZa = g.Count(gl => gl.Glas?.ToLower() == "za"),
                    BrojGlasovaProtiv = g.Count(gl => gl.Glas?.ToLower() == "protiv"),
                    BrojUzdrzanih = g.Count(gl => gl.Glas?.ToLower() == "uzdrzan")
                });

            foreach (var grupa in grupisanaGlasanja)
            {
                int ukupno = grupa.BrojGlasovaZa + grupa.BrojGlasovaProtiv + grupa.BrojUzdrzanih;
                string rezultat = grupa.BrojGlasovaZa > grupa.BrojGlasovaProtiv ? "Usvojeno" : 
                                 grupa.BrojGlasovaProtiv > grupa.BrojGlasovaZa ? "Odbijeno" : "Neodlučeno";
                double procenatUcesca = ukupno > 0 ? (double)ukupno / 250 * 100 : 0; // 250 je ukupan broj poslanika

                dt.Rows.Add(
                    grupa.NazivSednice,
                    grupa.Pitanje,
                    grupa.DatumGlasanja,
                    grupa.BrojGlasovaZa,
                    grupa.BrojGlasovaProtiv,
                    grupa.BrojUzdrzanih,
                    rezultat,
                    Math.Round(procenatUcesca, 1)
                );
            }

            return dt;
        }

        protected void btnPretrazi_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                
                // Samo filteri za ime, pitanje i rezultat - uklanjamo datum filtere
                int? sednicaId = null;
                if (!string.IsNullOrEmpty(ddlSednica.SelectedValue) && ddlSednica.SelectedValue != "")
                {
                    int.TryParse(ddlSednica.SelectedValue, out int tempId);
                    sednicaId = tempId;
                }
                
                string pitanjeTekst = ddlPitanje.SelectedValue;
                if (pitanjeTekst == "-- Sva pitanja --")
                {
                    pitanjeTekst = null;
                }
                
                string rezultat = ddlRezultat.SelectedValue;
                if (rezultat == "-- Svi rezultati --")
                {
                    rezultat = null;
                }
                
                // Koristi novu filter metodu - za sada ne prosleđujemo pitanje ID jer koristimo tekst
                var glasanja = _sednicePregled.PretraziGlasanja(null, null, sednicaId, null, null);
                
                if (glasanja != null && glasanja.Count > 0)
                {
                    // Primeni pitanje filter ako je postavljen
                    if (!string.IsNullOrEmpty(pitanjeTekst))
                    {
                        var preFilterCount = glasanja.Count;
                        glasanja = glasanja.Where(g => 
                            !string.IsNullOrEmpty(g.TekstPitanja) && 
                            g.TekstPitanja.IndexOf(pitanjeTekst, StringComparison.OrdinalIgnoreCase) >= 0
                        ).ToList();
                    }
                    
                    // Primeni rezultat filter ako je postavljen
                    if (!string.IsNullOrEmpty(rezultat))
                    {
                        var preFilterCount = glasanja.Count;
                        glasanja = glasanja.Where(g => 
                            GetVoteResult(g.Glas) == rezultat
                        ).ToList();
                    }
                    
                    var dt = ConvertGlasanjaToDataTable(glasanja);
                    BindGlasanjaToGrid(dt);
                    UpdateResultsCount(dt.Rows.Count);
                }
                else
                {
                    BindGlasanjaToGrid(new DataTable());
                    UpdateResultsCount(0);
                }
            }
            catch (Exception ex)
            {
                ShowError("Greška pri pretraživanju. Molimo pokušajte ponovo.");
            }
        }

        /// <summary>
        /// Pomoćna metoda za određivanje rezultata glasanja na osnovu glasa
        /// </summary>
        private string GetVoteResult(string glas)
        {
            if (string.IsNullOrEmpty(glas))
                return "Neodlučeno";
                
            switch (glas.ToLower())
            {
                case "za":
                    return "Usvojeno";
                case "protiv":
                    return "Odbijeno";
                case "uzdrzan":
                    return "Neodlučeno";
                default:
                    return "Neodlučeno";
            }
        }

        /// <summary>
        /// Pomoćna metoda za određivanje CSS klase za stilizovanje rezultata glasanja
        /// </summary>
        public string GetVoteClass(object rezultat)
        {
            if (rezultat == null)
                return "vote-abstain";
                
            string rezultatStr = rezultat.ToString().ToLower();
            
            switch (rezultatStr)
            {
                case "usvojeno":
                    return "vote-for";
                case "odbijeno":
                    return "vote-against";
                case "neodlučeno":
                default:
                    return "vote-abstain";
            }
        }

        protected void btnResetuj_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                
                // Resetuj samo dropdown filtere - ne resetujemo datum filtere
                ddlSednica.SelectedIndex = 0;
                ddlPitanje.SelectedIndex = 0;
                ddlRezultat.SelectedIndex = 0;
                
                // Učitaj sve podatke
                LoadGlasanja();
            }
            catch (Exception ex)
            {
                ShowError("Greška pri resetovanju filtera. Molimo pokušajte ponovo.");
            }
        }

        protected void gvGlasanja_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                gvGlasanja.PageIndex = e.NewPageIndex;
                LoadGlasanja(); // Ponovo učitaj podatke za novu stranicu
            }
            catch (Exception ex)
            {
                ShowError("Greška pri promeni stranice. Molimo pokušajte ponovo.");
            }
        }

        private void BindGlasanjaToGrid(DataTable glasanja)
        {
            if (glasanja != null && glasanja.Rows.Count > 0)
            {
                gvGlasanja.DataSource = glasanja;
                gvGlasanja.DataBind();
                
                // Sačuvaj podatke u ViewState za export funkcionalnost
                ViewState["CurrentGlasanjaData"] = glasanja;
            }
            else
            {
                gvGlasanja.DataSource = null;
                gvGlasanja.DataBind();
                
                // Očisti ViewState
                ViewState["CurrentGlasanjaData"] = null;
            }
        }

        private void UpdateResultsCount(int count)
        {
            lblBrojRezultata.Text = $"Pronađeno: {count} glasanja";
        }

        private void ShowError(string message)
        {
            // Možete implementirati prikaz greške (npr. Label ili JavaScript alert)
        }

        /// <summary>
        /// Exportuje trenutno prikazane glasanja u CSV format
        /// </summary>
        protected void btnExportuj_Click(object sender, EventArgs e)
        {
            try
            {
                // Uzmi podatke direktno iz GridView-a
                var glasanja = GetCurrentGridViewData();
                
                if (glasanja != null && glasanja.Rows.Count > 0)
                {
                    // Koristi univerzalnu export funkcionalnost
                    ExportUtility.ExportToCSV(glasanja, "istorija_glasanja", Response);
                }
                else
                {
                    ShowError("Nema podataka za export. Molimo prvo pretražite glasanja.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Greška pri exportovanju. Molimo pokušajte ponovo.");
            }
        }

        /// <summary>
        /// Generiše stampu trenutno prikazanih glasanja
        /// </summary>
        protected void btnStampa_Click(object sender, EventArgs e)
        {
            try
            {
                // Uzmi podatke direktno iz GridView-a
                var glasanja = GetCurrentGridViewData();
                
                if (glasanja != null && glasanja.Rows.Count > 0)
                {
                    // Pripremi aktivne filtere za prikaz
                    var activeFilters = GetActiveFilters();
                    
                    // Koristi univerzalnu stampa funkcionalnost
                    ExportUtility.ExportToHTML(glasanja, "stampa_glasanja", "Istorija Glasanja", 
                        "Skupština - Pregled istorije glasanja", activeFilters, Response);
                }
                else
                {
                    ShowError("Nema podataka za stampu. Molimo prvo pretražite glasanja.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Greška pri generisanju stampe. Molimo pokušajte ponovo.");
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
                if (ViewState["CurrentGlasanjaData"] != null)
                {
                    return ViewState["CurrentGlasanjaData"] as DataTable;
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
            
            if (ddlSednica.SelectedIndex > 0)
                activeFilters.Add($"Sednica: {ddlSednica.SelectedItem.Text}");
            if (ddlPitanje.SelectedIndex > 0)
                activeFilters.Add($"Pitanje: {ddlPitanje.SelectedItem.Text}");
            if (ddlRezultat.SelectedIndex > 0)
                activeFilters.Add($"Rezultat: {ddlRezultat.SelectedItem.Text}");
            
            return activeFilters;
        }
    }
}
