using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrezentacionaLogika;
using KlaseMapiranja;
using System.Collections.Generic; // Added for List<ZasedanjeDTO>
using System.Linq; // Added for Count()

namespace KorisnickiInterfejs
{
    public partial class IstorijaZasedanja : System.Web.UI.Page
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
            
            // Ne postavljamo default datume jer ne koristimo datum filtere
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
                // Učitaj sazive za dropdown
                LoadSazivi();
                // Tip filter je uklonjen - ne učitavamo više tipove
                
                // Učitaj početne podatke
                LoadZasedanja();
                
                // Učitaj statistike
                LoadStatistics();
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
                // Fallback na hardkodirane vrednosti ako servis ne radi
                ddlSaziv.Items.Clear();
                ddlSaziv.Items.Add(new ListItem("-- Svi sazivi --", ""));
                ddlSaziv.Items.Add(new ListItem("Saziv 1", "1"));
                ddlSaziv.Items.Add(new ListItem("Saziv 2", "2"));
            }
        }
        
        // LoadZasedanja metoda je uklonjena jer ne koristimo zasedanje filter

        private void LoadZasedanja()
        {
            try
            {
                EnsureSednicePregledInitialized();
                var zasedanja = _sednicePregled.DajSvaZasedanja();
                BindZasedanjaToGrid(zasedanja);
                UpdateResultsCount(zasedanja.Count);
            }
            catch (Exception ex)
            {
                ShowError("Greška pri učitavanju zasedanja. Molimo pokušajte ponovo.");
            }
        }

        private void LoadStatistics()
        {
            try
            {
                EnsureSednicePregledInitialized();
                var stats = _sednicePregled.DajStatistikeZasedanja();
                
                lblUkupnoZasedanja.Text = stats.UkupnoZasedanja.ToString();
                lblAktivnaZasedanja.Text = stats.AktivnaZasedanja.ToString();
                lblZavrsenaZasedanja.Text = stats.ZavrsenaZasedanja.ToString();
                lblUkupnoSednica.Text = stats.UkupnoSednica.ToString();
            }
            catch (Exception ex)
            {
                // Postavi default vrednosti
                lblUkupnoZasedanja.Text = "0";
                lblAktivnaZasedanja.Text = "0";
                lblZavrsenaZasedanja.Text = "0";
                lblUkupnoSednica.Text = "0";
            }
        }

        protected void btnPretrazi_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                
                // Samo Saziv filter - uklanjamo Tip filter
                int? sazivId = null;
                if (!string.IsNullOrEmpty(ddlSaziv.SelectedValue) && ddlSaziv.SelectedValue != "")
                {
                    if (int.TryParse(ddlSaziv.SelectedValue, out int tempId))
                    {
                        sazivId = tempId;
                    }
                }
                
                // Koristi direktno filter metodu
                var zasedanja = _sednicePregled.DajZasedanjaZaFilter(sazivId, null);
                BindZasedanjaToGrid(zasedanja);
                UpdateResultsCount(zasedanja.Count);
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
                // Resetuj filtere - samo Saziv (Tip filter je uklonjen)
                ddlSaziv.SelectedIndex = 0;
                
                // Učitaj sve podatke
                LoadZasedanja();
            }
            catch (Exception ex)
            {
                ShowError("Greška pri resetovanju filtera. Molimo pokušajte ponovo.");
            }
        }

        protected void gvZasedanja_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                gvZasedanja.PageIndex = e.NewPageIndex;
                LoadZasedanja(); // Ponovo učitaj podatke za novu stranicu
            }
            catch (Exception ex)
            {
                ShowError("Greška pri promeni stranice. Molimo pokušajte ponovo.");
            }
        }

        private void BindZasedanjaToGrid(System.Collections.Generic.List<ZasedanjeDTO> zasedanja)
        {
            if (zasedanja != null && zasedanja.Count > 0)
            {
                // Konvertuj u DataTable za GridView - kolone moraju da se poklapaju sa DataField u GridView
                var dt = new DataTable();
                dt.Columns.Add("Naziv", typeof(string));
                dt.Columns.Add("Tip", typeof(string));
                dt.Columns.Add("NazivSaziva", typeof(string));

                foreach (var zasedanje in zasedanja)
                {
                    dt.Rows.Add(
                        zasedanje.Naziv ?? "",
                        zasedanje.TipZasedanja ?? "",
                        zasedanje.NazivSaziva ?? "N/A"
                    );
                }

                gvZasedanja.DataSource = dt;
                gvZasedanja.DataBind();
            }
            else
            {
                gvZasedanja.DataSource = null;
                gvZasedanja.DataBind();
            }
        }

        // Metode za status su uklonjene jer ne koristimo status kolonu

        private void UpdateResultsCount(int count)
        {
            lblBrojRezultata.Text = $"Pronađeno: {count} zasedanja";
        }

        private void ShowError(string message)
        {
            // Možete implementirati prikaz greške (npr. Label ili JavaScript alert)
        }
    }
}
