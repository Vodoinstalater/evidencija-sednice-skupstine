using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
    public partial class NovaSednica : System.Web.UI.Page
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
                LoadCurrentSessionInfo();
                SetDefaultDate();
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

        private void LoadCurrentSessionInfo()
        {
            try
            {
                var trenutnoZasedanje = SednicePregled.DajTrenutnoZasedanje();
                
                if (trenutnoZasedanje != null)
                {
                    lblTrenutnoZasedanje.Text = trenutnoZasedanje.Naziv ?? "Nepoznato";
                    lblTipZasedanja.Text = trenutnoZasedanje.TipZasedanja ?? "Nepoznato";
                    lblSaziv.Text = trenutnoZasedanje.NazivSaziva ?? "Nepoznato";
                    
                    // Proveri da li je saziv aktivan
                    if (trenutnoZasedanje.DatumZavrsetka > DateTime.Now)
                    {
                        lblStatus.Text = "Aktivan";
                    }
                    else
                    {
                        lblStatus.Text = "Završen";
                    }
                }
                else
                {
                    lblTrenutnoZasedanje.Text = "Nema aktivnog zasedanja";
                    lblTipZasedanja.Text = "-";
                    lblSaziv.Text = "-";
                    lblStatus.Text = "Nema";
                }
            }
            catch (Exception ex)
            {
                lblTrenutnoZasedanje.Text = "Greška pri učitavanju";
                lblTipZasedanja.Text = "-";
                lblSaziv.Text = "-";
                lblStatus.Text = "Greška";
                
                // Prikaži grešku korisniku
                ShowAlert($"Greška pri učitavanju trenutnog zasedanja: {ex.Message}", "warning");
            }
        }

        private void SetDefaultDate()
        {
            try
            {
                txtDatumSednice.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                // No debug logging for this method
            }
        }

        protected void btnKreirajSednicu_Click(object sender, EventArgs e)
        {
            try
            {
                // Proveri da li postoji aktivno zasedanje
                var trenutnoZasedanje = SednicePregled.DajTrenutnoZasedanje();
                if (trenutnoZasedanje == null)
                {
                    ShowAlert("Nema aktivnog zasedanja. Prvo kreirajte novo zasedanje.", "danger");
                    return;
                }

                // Dohvati pitanja iz forme
                var pitanja = GetQuestionsFromForm();
                if (pitanja.Count == 0)
                {
                    ShowAlert("Morate uneti bar jedno pitanje za sednicu.", "danger");
                    return;
                }

                // Kreiraj novu sednicu sa pravilima
                string nazivSednice = txtNazivSednice.Text.Trim();
                DateTime datumSednice = Convert.ToDateTime(txtDatumSednice.Text);
                string opisSednice = txtOpisSednice.Text.Trim();
                
                // Dohvati razlog za hitnost (pravilo 7 dana)
                string razlogHitnosti = txtRazlogHitnosti.Text.Trim();
                
                // Ako je sednica manje od 7 dana unapred, koristi razlog kao opis
                TimeSpan razlika = datumSednice.Date - DateTime.Today;
                if (razlika.TotalDays < 7 && !string.IsNullOrWhiteSpace(razlogHitnosti))
                {
                    // Kombinuj opis i razlog za hitnost
                    opisSednice = string.IsNullOrWhiteSpace(opisSednice) 
                        ? razlogHitnosti 
                        : opisSednice + "\n\nRazlog za hitno sazivanje: " + razlogHitnosti;
                }
                
                string poruka;

                bool uspesno = SednicePregled.KreirajNovuSednicuSaPravilima(nazivSednice, datumSednice, opisSednice, pitanja, out poruka);

                if (uspesno)
                {
                    ShowAlert(poruka, "success");
                    ClearForm();
                }
                else
                {
                    ShowAlert(poruka, "danger");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Greška pri kreiranju sednice: {ex.Message}", "danger");
            }
        }

        private List<string> GetQuestionsFromForm()
        {
            var pitanja = new List<string>();
            
            try
            {
                // Dohvati pitanja iz hidden field-a (JavaScript popunjava ovo)
                if (!string.IsNullOrWhiteSpace(hdnQuestions.Value))
                {
                    try
                    {
                        // Simple parsing - split by comma and clean up
                        var questionsFromHidden = hdnQuestions.Value.Split(',')
                            .Where(q => !string.IsNullOrWhiteSpace(q.Trim()))
                            .Select(q => q.Trim())
                            .ToList();
                        
                        if (questionsFromHidden != null && questionsFromHidden.Count > 0)
                        {
                            pitanja.AddRange(questionsFromHidden);
                        }
                    }
                    catch (Exception jsonEx)
                    {
                        // No debug logging for this method
                    }
                }

                // Fallback: ako hidden field nije popunjen, koristi prvo pitanje
                if (pitanja.Count == 0 && !string.IsNullOrWhiteSpace(txtPitanje1.Text.Trim()))
                {
                    pitanja.Add(txtPitanje1.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                // No debug logging for this method
            }

            return pitanja;
        }

        protected void btnResetuj_Click(object sender, EventArgs e)
        {
            ClearForm();
            ShowAlert("Forma je resetovana.", "info");
        }

        private void ClearForm()
        {
            txtNazivSednice.Text = string.Empty;
            txtOpisSednice.Text = string.Empty;
            txtRazlogHitnosti.Text = string.Empty;
            txtPitanje1.Text = string.Empty;
            hdnQuestions.Value = string.Empty;
            SetDefaultDate();
            
            // Add JavaScript to reset the questions container
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ResetQuestions", 
                "resetQuestionsContainer();", true);
        }

        private void ShowAlert(string message, string alertType)
        {
            pnlAlert.Visible = true;
            lblAlertMessage.Text = message;
            
            // Ukloni sve CSS klase
            pnlAlert.CssClass = "alert";
            
            // Dodaj odgovarajuću CSS klasu
            switch (alertType.ToLower())
            {
                case "success":
                    pnlAlert.CssClass += " alert-success";
                    break;
                case "danger":
                    pnlAlert.CssClass += " alert-danger";
                    break;
                case "info":
                    pnlAlert.CssClass += " alert-info";
                    break;
                default:
                    pnlAlert.CssClass += " alert-info";
                    break;
            }
        }
    }
}
