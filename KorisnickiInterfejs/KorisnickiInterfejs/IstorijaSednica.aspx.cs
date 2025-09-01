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
    public partial class IstorijaSednica : System.Web.UI.Page
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
            
            // Postavi default datume (poslednjih 6 meseci)
            txtDatumOd.Text = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            txtDatumDo.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
                
                // Učitaj početne podatke
                LoadSednice();
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

        private void LoadSednice()
        {
            try
            {
                EnsureSednicePregledInitialized();
                var sednice = _sednicePregled.DajSveSednice();
                
                if (sednice != null && sednice.Count > 0)
                {
                    BindSedniceToGrid(sednice);
                    UpdateResultsCount(sednice.Count);
                }
                else
                {
                    BindSedniceToGrid(new List<SednicaDTO>());
                    UpdateResultsCount(0);
                }
            }
            catch (Exception ex)
            {
                ShowError("Greška pri učitavanju sednica. Molimo pokušajte ponovo.");
            }
        }

        protected void btnPretrazi_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                
                // Uzmi vrednosti filtera
                int? sazivId = string.IsNullOrEmpty(ddlSaziv.SelectedValue) ? (int?)null : int.Parse(ddlSaziv.SelectedValue);
                string nazivFilter = txtNaziv.Text.Trim();
                DateTime? datumOd = null;
                DateTime? datumDo = null;
                
                // Parsiraj datume ako su uneti
                if (!string.IsNullOrEmpty(txtDatumOd.Text))
                {
                    if (DateTime.TryParse(txtDatumOd.Text, out DateTime parsedDatumOd))
                        datumOd = parsedDatumOd;
                }
                
                if (!string.IsNullOrEmpty(txtDatumDo.Text))
                {
                    if (DateTime.TryParse(txtDatumDo.Text, out DateTime parsedDatumDo))
                        datumDo = parsedDatumDo;
                }
                
                // Prvo dohvati sve sednice sa stored procedure (saziv filter)
                var sednice = _sednicePregled.DajSednicePoFilteru(sazivId, null);
                
                // Zatim primeni dodatne filtere (naziv i datum) na klijentskoj strani
                var filtriraneSednice = ApplyClientSideFilters(sednice, nazivFilter, datumOd, datumDo);
                
                BindSedniceToGrid(filtriraneSednice);
                UpdateResultsCount(filtriraneSednice.Count);
            }
            catch (Exception ex)
            {
                ShowError("Greška pri pretraživanju. Molimo pokušajte ponovo.");
            }
        }
        
        private List<SednicaDTO> ApplyClientSideFilters(List<SednicaDTO> sednice, string nazivFilter, DateTime? datumOd, DateTime? datumDo)
        {
            var filtrirane = sednice.AsEnumerable();
            
            // Filter po nazivu
            if (!string.IsNullOrEmpty(nazivFilter))
            {
                filtrirane = filtrirane.Where(s => 
                    !string.IsNullOrEmpty(s.Naziv) && 
                    s.Naziv.IndexOf(nazivFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            
            // Filter po datumu od
            if (datumOd.HasValue)
            {
                filtrirane = filtrirane.Where(s => s.Datum.HasValue && s.Datum.Value.Date >= datumOd.Value.Date);
            }
            
            // Filter po datumu do
            if (datumDo.HasValue)
            {
                filtrirane = filtrirane.Where(s => s.Datum.HasValue && s.Datum.Value.Date <= datumDo.Value.Date);
            }
            
            return filtrirane.ToList();
        }

        protected void btnResetuj_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                // Resetuj sve filtere
                txtNaziv.Text = string.Empty;
                ddlSaziv.SelectedIndex = 0;
                txtDatumOd.Text = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
                txtDatumDo.Text = DateTime.Now.ToString("yyyy-MM-dd");
                
                // Učitaj sve podatke
                LoadSednice();
            }
            catch (Exception ex)
            {
                ShowError("Greška pri resetovanju filtera. Molimo pokušajte ponovo.");
            }
        }

        protected void gvSednice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                EnsureSednicePregledInitialized();
                gvSednice.PageIndex = e.NewPageIndex;
                LoadSednice(); // Ponovo učitaj podatke za novu stranicu
            }
            catch (Exception ex)
            {
                ShowError("Greška pri promeni stranice. Molimo pokušajte ponovo.");
            }
        }

        private void BindSedniceToGrid(System.Collections.Generic.List<SednicaDTO> sednice)
        {
            if (sednice != null && sednice.Count > 0)
            {
                // Konvertuj u DataTable za GridView - samo kolone koje postoje u stored procedure
                var dt = new DataTable();
                dt.Columns.Add("Naziv", typeof(string));
                dt.Columns.Add("Datum", typeof(DateTime));
                dt.Columns.Add("Opis", typeof(string));
                dt.Columns.Add("NazivSaziva", typeof(string));
                dt.Columns.Add("NazivZasedanja", typeof(string));

                foreach (var sednica in sednice)
                {
                    dt.Rows.Add(
                        sednica.Naziv ?? "",
                        sednica.Datum ?? DateTime.Now,
                        sednica.Opis ?? "N/A",
                        sednica.NazivSaziva ?? "N/A",
                        sednica.NazivZasedanja ?? "N/A"
                    );
                }

                gvSednice.DataSource = dt;
                gvSednice.DataBind();
            }
            else
            {
                gvSednice.DataSource = null;
                gvSednice.DataBind();
            }
        }

        // Metode za status su uklonjene jer ne koristimo status kolonu

        private void UpdateResultsCount(int count)
        {
            lblBrojRezultata.Text = $"Pronađeno: {count} sednica";
        }

        private void ShowError(string message)
        {
            // Možete implementirati prikaz greške (npr. Label ili JavaScript alert)
        }
    }
}
