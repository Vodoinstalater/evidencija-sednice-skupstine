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
            }
            catch (Exception)
            {
                ShowError("Greška pri učitavanju saziva. Molimo pokušajte ponovo.");
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



        private void ShowError(string message)
        {
            // Možete implementirati prikaz greške (npr. Label ili JavaScript alert)
        }
    }
}
