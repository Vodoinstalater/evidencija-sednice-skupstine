using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrezentacionaLogika;
using KlaseMapiranja;
using System.Linq; // Added for .Where() and .ToList()
using System.Collections.Generic; // Added for List

namespace KorisnickiInterfejs
{
    public partial class IstorijaSaziva : System.Web.UI.Page
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
            
            // Datum filteri su uklonjeni
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
                // Učitaj početne podatke
                LoadSazivi();
                
                // Učitaj statistike
                LoadStatistics();
                
                // Učitaj trenutni status
                LoadCurrentStatus();
            }
            catch (Exception)
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
                BindSaziviToGrid(sazivi);
                UpdateResultsCount(sazivi.Count);
            }
            catch (Exception)
            {
                ShowError("Greška pri učitavanju saziva. Molimo pokušajte ponovo.");
            }
        }

        private void LoadStatistics()
        {
            try
            {
                EnsureSednicePregledInitialized();
                var stats = _sednicePregled.DajStatistikeSaziva();
                
                lblUkupnoSaziva.Text = stats.UkupnoSaziva.ToString();
                lblAktivnihSaziva.Text = stats.AktivnihSaziva.ToString();
                lblZavrsenihSaziva.Text = stats.ZavrsenihSaziva.ToString();
                lblUkupnoMandata.Text = stats.UkupnoMandata.ToString();
            }
            catch (Exception)
            {
                // Postavi default vrednosti
                lblUkupnoSaziva.Text = "0";
                lblAktivnihSaziva.Text = "0";
                lblZavrsenihSaziva.Text = "0";
                lblUkupnoMandata.Text = "0";
            }
        }

        private void LoadCurrentStatus()
        {
            try
            {
                EnsureSednicePregledInitialized();
                var aktivanSaziv = _sednicePregled.DajAktivanSaziv();
                if (aktivanSaziv != null)
                {
                    lblAktivanSaziv.Text = aktivanSaziv.Ime ?? "Nepoznato";
                    lblBrojZasedanja.Text = aktivanSaziv.BrojZasedanja.ToString();
                    lblBrojSednica.Text = "25";
                }
                else
                {
                    lblAktivanSaziv.Text = "Nema aktivnog saziva";
                    lblBrojZasedanja.Text = "0";
                    lblBrojSednica.Text = "0";
                }
            }
            catch (Exception)
            {
                lblAktivanSaziv.Text = "Greška pri učitavanju";
                lblBrojZasedanja.Text = "0";
                lblBrojSednica.Text = "0";
            }
        }

        protected void btnPretrazi_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                
                string naziv = txtNaziv.Text.Trim();

                if (string.IsNullOrEmpty(naziv))
                {
                    // Ako nema kriterijuma, učitaj sve
                    LoadSazivi();
                    return;
                }

                // Koristi jednostavan filter sa boljim null checking
                var sviSazivi = _sednicePregled.DajSveSazive();
                
                if (sviSazivi == null || sviSazivi.Count == 0)
                {
                    BindSaziviToGrid(new List<SazivDTO>());
                    UpdateResultsCount(0);
                    return;
                }

                var filtriraniSazivi = sviSazivi
                    .Where(s => s != null && !string.IsNullOrEmpty(s.Ime) && 
                               s.Ime.IndexOf(naziv, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                BindSaziviToGrid(filtriraniSazivi);
                UpdateResultsCount(filtriraniSazivi.Count);
            }
            catch (Exception)
            {
                ShowError("Greška pri pretraživanju. Molimo pokušajte ponovo.");
            }
        }

        protected void btnResetuj_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                // Resetuj filtere - samo naziv
                txtNaziv.Text = string.Empty;
                
                // Učitaj sve podatke
                LoadSazivi();
            }
            catch (Exception)
            {
                ShowError("Greška pri resetovanju filtera. Molimo pokušajte ponovo.");
            }
        }

        protected void gvSazivi_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                gvSazivi.PageIndex = e.NewPageIndex;
                LoadSazivi(); // Ponovo učitaj podatke za novu stranicu
            }
            catch (Exception)
            {
                ShowError("Greška pri promeni stranice. Molimo pokušajte ponovo.");
            }
        }

        private void BindSaziviToGrid(System.Collections.Generic.List<SazivDTO> sazivi)
        {
            if (sazivi != null && sazivi.Count > 0)
            {
                // Konvertuj u DataTable za GridView - samo kolone koje postoje u ASPX
                var dt = new DataTable();
                dt.Columns.Add("Naziv", typeof(string));
                dt.Columns.Add("DatumPocetka", typeof(DateTime));
                dt.Columns.Add("DatumZavrsetka", typeof(DateTime));
                dt.Columns.Add("Opis", typeof(string));

                foreach (var saziv in sazivi)
                {
                    dt.Rows.Add(
                        saziv.Ime ?? "",
                        saziv.DatumPocetka ?? DateTime.Now.AddYears(-1),
                        saziv.DatumZavrsetka ?? DateTime.Now,
                        saziv.Opis ?? "N/A"
                    );
                }

                gvSazivi.DataSource = dt;
                gvSazivi.DataBind();
            }
            else
            {
                gvSazivi.DataSource = null;
                gvSazivi.DataBind();
            }
        }

        // GetStatusText metoda je uklonjena jer ne koristimo Status kolonu

        // GetStatusClass metoda je uklonjena jer ne koristimo Status kolonu

        private void UpdateResultsCount(int count)
        {
            lblBrojRezultata.Text = $"Pronađeno: {count} saziva";
        }

        private void ShowError(string message)
        {
            // Možete implementirati prikaz greške (npr. Label ili JavaScript alert)
        }
    }
}
