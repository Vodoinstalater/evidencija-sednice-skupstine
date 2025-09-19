using System;
using System.Collections.Generic;
using System.Linq;

namespace PoslovnaLogika
{
    /// <summary>
    /// Klasa za validaciju poslovnih pravila - čista poslovna logika bez pristupa podacima
    /// </summary>
    public class ValidacijaPoslovnihPravilaKlasa
    {
        private readonly BusinessRulesConfigKlasa _config;

        public ValidacijaPoslovnihPravilaKlasa()
        {
            _config = BusinessRulesConfigKlasa.Instance;
        }

        // ========================================================================
        // VALIDACIJA ZA SEDNICE
        // ========================================================================

        /// <summary>
        /// Proverava da li je sednica blagovremeno zakazana
        /// </summary>
        public bool DaLiJeSednicaBlagovremenoZakazana(DateTime datum, string objasnjenje, out string poruka)
        {
            poruka = "";
            TimeSpan razlika = datum.Date - DateTime.Today;
            
            if (razlika.TotalDays < _config.MinDanaZaSednicu)
            {
                if (string.IsNullOrWhiteSpace(objasnjenje))
                {
                    poruka = $"Sednica ne može biti zakazana manje od {_config.MinDanaZaSednicu} dana unapred bez objašnjenja razloga.";
                    return false;
                }
            }
            
            return true;
        }

        /// <summary>
        /// Proverava da li je datum u validnom opsegu
        /// </summary>
        public bool DaLiJeDatumValidan(DateTime datum, out string poruka)
        {
            poruka = "";
            
            if (datum.Date < DateTime.Today)
            {
                poruka = "Datum ne sme biti u prošlosti.";
                return false;
            }

            if (datum.Year > DateTime.Today.Year + _config.MaxGodinaUBuducnosti)
            {
                poruka = $"Datum ne sme biti više od {_config.MaxGodinaUBuducnosti} godina u budućnosti.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Proverava da li je datum iz prošlosti
        /// </summary>
        public bool DaLiJeDatumIzProslosti(DateTime datum, out string poruka)
        {
            poruka = "";
            
            if (datum.Date < DateTime.Today)
            {
                poruka = "Datum ne sme biti u prošlosti.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Proverava da li ima barem jedno pitanje za dnevni red
        /// </summary>
        public bool DaLiImaPitanjaZaDnevniRed(List<string> pitanja, out string poruka)
        {
            poruka = "";
            
            if (pitanja == null || pitanja.Count == 0)
            {
                poruka = "Sednica mora imati bar jedno pitanje za dnevni red.";
                return false;
            }

            // Proveri da li ima bar jedno ne-prazno pitanje
            if (!pitanja.Any(p => !string.IsNullOrWhiteSpace(p)))
            {
                poruka = "Sednica mora imati bar jedno ne-prazno pitanje za dnevni red.";
                return false;
            }

            return true;
        }

        // ========================================================================
        // VALIDACIJA ZA LICA
        // ========================================================================

        /// <summary>
        /// Proverava da li je lice punoletno
        /// </summary>
        public bool DaLiJeLicePunoljetno(DateTime datumRodjenja, out string poruka)
        {
            poruka = "";
            
            if (datumRodjenja.Date >= DateTime.Today.AddYears(-_config.MinStarost))
            {
                poruka = $"Lice mora biti starije od {_config.MinStarost} godina.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Proverava da li je lice punoletno (alternativni naziv)
        /// </summary>
        public bool DaLiJeLicePunoletno(DateTime datumRodjenja, out string poruka)
        {
            return DaLiJeLicePunoljetno(datumRodjenja, out poruka);
        }

        /// <summary>
        /// Proverava da li je datum rođenja validan
        /// </summary>
        public bool DaLiJeDatumRodjenjaValidan(DateTime datumRodjenja, out string poruka)
        {
            poruka = "";
            
            if (datumRodjenja.Date >= DateTime.Today)
            {
                poruka = "Datum rođenja ne sme biti u budućnosti.";
                return false;
            }

            if (datumRodjenja.Year < _config.MinGodinaRodjenja)
            {
                poruka = $"Datum rođenja ne sme biti pre {_config.MinGodinaRodjenja}. godine.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Proverava da li je lozinka validna
        /// </summary>
        public bool DaLiJeLozinkaValidna(string lozinka, out string poruka)
        {
            poruka = "";
            
            if (string.IsNullOrWhiteSpace(lozinka))
            {
                poruka = "Lozinka je obavezna.";
                return false;
            }

            if (lozinka.Length < _config.MinDuzinaLozinke)
            {
                poruka = $"Lozinka mora imati bar {_config.MinDuzinaLozinke} karaktera.";
                return false;
            }

            if (lozinka.Length > _config.MaxDuzinaLozinke)
            {
                poruka = $"Lozinka ne sme imati više od {_config.MaxDuzinaLozinke} karaktera.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Proverava da li je pozicija validna
        /// </summary>
        public bool DaLiJePozicijaValidna(int pozicijaId, out string poruka)
        {
            poruka = "";
            
            if (pozicijaId < _config.MinPozicijaId || pozicijaId > _config.MaxPozicijaId)
            {
                poruka = $"Nevažeća pozicija. Pozicija mora biti između {_config.MinPozicijaId} i {_config.MaxPozicijaId}.";
                return false;
            }

            return true;
        }

        // ========================================================================
        // VALIDACIJA ZA SAZIVE
        // ========================================================================

        /// <summary>
        /// Proverava da li su datumi saziva validni
        /// </summary>
        public bool DaLiSuDatumiSazivaValidni(DateTime pocetak, DateTime kraj, out string poruka)
        {
            poruka = "";
            
            if (kraj <= pocetak)
            {
                poruka = "Datum završetka mora biti nakon datuma početka.";
                return false;
            }

            if (pocetak.Date < DateTime.Today)
            {
                poruka = "Datum početka ne sme biti u prošlosti.";
                return false;
            }

            TimeSpan duzinaSaziva = kraj - pocetak;
            if (duzinaSaziva.TotalDays > _config.MaxDuzinaSaziva)
            {
                poruka = $"Saziv ne sme trajati duže od {_config.MaxDuzinaSaziva} dana.";
                return false;
            }

            return true;
        }

        // ========================================================================
        // VALIDACIJA ZA ZASEDANJA
        // ========================================================================

        /// <summary>
        /// Proverava da li je datum zasedanja validan
        /// </summary>
        public bool DaLiJeDatumZasedanjaValidan(DateTime datum, out string poruka)
        {
            poruka = "";
            
            if (datum.Date < DateTime.Today.AddDays(_config.MinDanaUnapred))
            {
                poruka = $"Datum zasedanja mora biti bar {_config.MinDanaUnapred} dan unapred.";
                return false;
            }

            return true;
        }

        // ========================================================================
        // VALIDACIJA ZA MANDATE
        // ========================================================================

        /// <summary>
        /// Proverava da li je mandat validan
        /// </summary>
        public bool DaLiJeMandatValidan(int liceId, int sazivId, out string poruka)
        {
            poruka = "";
            
            if (liceId <= 0)
            {
                poruka = "Nevažeći ID lica.";
                return false;
            }

            if (sazivId <= 0)
            {
                poruka = "Nevažeći ID saziva.";
                return false;
            }

            return true;
        }

        // ========================================================================
        // VALIDACIJA ZA GLASANJE
        // ========================================================================

        /// <summary>
        /// Proverava da li je glas validan
        /// </summary>
        public bool DaLiJeGlasValidan(string glas, out string poruka)
        {
            poruka = "";
            
            if (string.IsNullOrWhiteSpace(glas))
            {
                poruka = "Glas je obavezan.";
                return false;
            }

            if (!_config.MoguciGlasovi.Contains(glas.Trim()))
            {
                poruka = $"Nevažeći glas. Mogući glasovi su: {string.Join(", ", _config.MoguciGlasovi)}";
                return false;
            }

            return true;
        }

        // ========================================================================
        // OPŠTE VALIDACIJE
        // ========================================================================

        /// <summary>
        /// Proverava da li je naziv validan
        /// </summary>
        public bool DaLiJeNazivValidan(string naziv, string tipEntiteta, out string poruka)
        {
            poruka = "";
            
            if (string.IsNullOrWhiteSpace(naziv))
            {
                poruka = $"Naziv {tipEntiteta} ne sme biti prazan.";
                return false;
            }

            if (naziv.Trim().Length < 2)
            {
                poruka = $"Naziv {tipEntiteta} mora imati bar 2 karaktera.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Proverava da li su obavezna polja popunjena
        /// </summary>
        public bool DaLiSuObaveznaPoljaPopunjena(Dictionary<string, object> polja, out string poruka)
        {
            poruka = "";
            
            foreach (var polje in polja)
            {
                if (polje.Value == null || 
                    (polje.Value is string str && string.IsNullOrWhiteSpace(str)) ||
                    (polje.Value is int intVal && intVal <= 0))
                {
                    poruka = $"Polje '{polje.Key}' je obavezno.";
                    return false;
                }
            }

            return true;
        }

        // ========================================================================
        // VALIDACIJA ZA AUTORIZACIJU
        // ========================================================================

        /// <summary>
        /// Proverava da li je akcija validna
        /// </summary>
        public bool DaLiJeAkcijaValidna(string akcija, out string poruka)
        {
            poruka = "";
            
            if (string.IsNullOrWhiteSpace(akcija))
            {
                poruka = "Akcija je obavezna.";
                return false;
            }

            // Lista validnih akcija iz konfiguracije
            var validneAkcije = new List<string>
            {
                _config.SazoviNovuSednicuAkcija,
                _config.OtvoriNovoZasedanjeAkcija,
                _config.PogledajSedniceAkcija,
                _config.PogledajZasedanjaAkcija,
                _config.PogledajSaziveAkcija,
                _config.PogledajMandateAkcija,
                _config.IstorijaGlasanjaAkcija
            };

            if (!validneAkcije.Contains(akcija.Trim()))
            {
                poruka = $"Nevažeća akcija: {akcija}. Validne akcije su: {string.Join(", ", validneAkcije)}";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Proverava da li korisnik ima dozvolu za određenu akciju na osnovu pozicije
        /// </summary>
        public bool DaLiKorisnikImaDozvolu(int pozicijaId, string akcija, out string poruka)
        {
            poruka = "";
            
            if (!DaLiJeAkcijaValidna(akcija, out string akcijaPoruka))
            {
                poruka = akcijaPoruka;
                return false;
            }

            // Predsednik ima sve dozvole
            if (pozicijaId == _config.PredsednikPozicija)
            {
                poruka = "Predsednik ima sve dozvole.";
                return true;
            }

            // Potpredsednik može da saziva sednice i otvara zasedanja
            if (pozicijaId == _config.PotpredsednikPozicija)
            {
                if (akcija == _config.SazoviNovuSednicuAkcija || akcija == _config.OtvoriNovoZasedanjeAkcija)
                {
                    poruka = "Potpredsednik ima dozvolu za ovu akciju.";
                    return true;
                }
            }

            // Poslanici mogu samo da pregledaju
            if (_config.PoslaniciPozicije.Contains(pozicijaId))
            {
                if (akcija == _config.PogledajSedniceAkcija || 
                    akcija == _config.PogledajZasedanjaAkcija || 
                    akcija == _config.PogledajSaziveAkcija || 
                    akcija == _config.PogledajMandateAkcija || 
                    akcija == _config.IstorijaGlasanjaAkcija)
                {
                    poruka = "Poslanik ima dozvolu za ovu akciju.";
                    return true;
                }
            }

            poruka = "Korisnik nema dozvolu za ovu akciju.";
            return false;
        }

        /// <summary>
        /// Proverava da li je pozicija validna za autorizaciju
        /// </summary>
        public bool DaLiJePozicijaValidnaZaAutorizaciju(int pozicijaId, out string poruka)
        {
            poruka = "";
            
            if (pozicijaId == _config.PredsednikPozicija || 
                pozicijaId == _config.PotpredsednikPozicija || 
                _config.PoslaniciPozicije.Contains(pozicijaId))
            {
                return true;
            }

            poruka = $"Nevažeća pozicija za autorizaciju: {pozicijaId}";
            return false;
        }
    }
}
