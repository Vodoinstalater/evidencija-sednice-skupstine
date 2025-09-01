using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//
using KlaseMapiranja;

namespace PrezentacionaLogika
{
    public class SednicaDashboardKlasa
    {
        private SednicaServisKlasa _servis;
        private string _stringKonekcije;

        // konstruktor
        public SednicaDashboardKlasa()
        {
            _stringKonekcije = ""; // koristi default konekciju
            _servis = new SednicaServisKlasa(_stringKonekcije);
        }

        public SednicaDashboardKlasa(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
            _servis = new SednicaServisKlasa(_stringKonekcije);
        }

        // metode za Dashboard statistike
        public int DajBrojSednica()
        {
            try
            {
                ServiceResult<List<SednicaDTO>> rezultat = _servis.DajSveSednice();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci.Count;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int DajBrojZasedanja()
        {
            try
            {
                ServiceResult<List<ZasedanjeDTO>> rezultat = _servis.DajSvaZasedanja();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci.Count;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int DajBrojMandata()
        {
            try
            {
                ServiceResult<List<MandatDTO>> rezultat = _servis.DajSveMandate();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci.Count;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string DajAktivanSaziv()
        {
            try
            {
                ServiceResult<List<SazivDTO>> rezultat = _servis.DajSveSazive();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    var aktivanSaziv = rezultat.Podaci.FirstOrDefault(s => s.Aktivan);
                    return aktivanSaziv?.Ime ?? "Nema aktivnog";
                }
                return "N/A";
            }
            catch (Exception)
            {
                return "N/A";
            }
        }

        // metoda za dobijanje poruke dobrodošlice
        public string GenerisiWelcomeMessage(string imePrezime, string tipKorisnika)
        {
            DateTime trenutno = DateTime.Now;
            string pozdravi = "";

            // pozdrav na osnovu vremena dana
            if (trenutno.Hour < 12)
                pozdravi = "Dobro jutro";
            else if (trenutno.Hour < 18)
                pozdravi = "Dobar dan";
            else
                pozdravi = "Dobro veče";

            return $"{pozdravi}, {imePrezime}! Vi ste ulogovani kao {tipKorisnika}.";
        }

        // metoda za dobijanje poslednje aktivnosti
        public string DajPoslednuAktivnost()
        {
            return $"Poslednja prijava: {DateTime.Now:dd.MM.yyyy HH:mm}";
        }
    }
}
