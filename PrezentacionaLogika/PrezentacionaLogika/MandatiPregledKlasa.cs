using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//
using KlaseMapiranja;

namespace PrezentacionaLogika
{
    public class MandatiPregledKlasa
    {
        private SednicaServisKlasa _servis;
        private string _stringKonekcije;

        // konstruktor
        public MandatiPregledKlasa()
        {
            _stringKonekcije = "";
            _servis = new SednicaServisKlasa(_stringKonekcije);
        }

        public MandatiPregledKlasa(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
            _servis = new SednicaServisKlasa(_stringKonekcije);
        }

        // metoda za dobijanje svih mandata
        public List<MandatDTO> DajSveMandate()
        {
            try
            {
                ServiceResult<List<MandatDTO>> rezultat = _servis.DajSveMandate();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<MandatDTO>();
            }
            catch (Exception)
            {
                return new List<MandatDTO>();
            }
        }

        // metoda za dobijanje mandata po sazivima
        public List<MandatDTO> DajMandatePoSazivu(string nazivSaziva)
        {
            try
            {
        
                List<MandatDTO> sviMandati = DajSveMandate();
                return sviMandati; // Za sada vraćamo sve mandate
            }
            catch (Exception)
            {
                return new List<MandatDTO>();
            }
        }

        // metoda za dobijanje pozicija za statistike
        public Dictionary<string, int> DajStatistikePozicija()
        {
            try
            {
                List<MandatDTO> mandati = DajSveMandate();
                var statistike = new Dictionary<string, int>();
                
                var grupePozicija = mandati.GroupBy(m => m.NazivPozicije);
                
                foreach (var grupa in grupePozicija)
                {
                    statistike[grupa.Key] = grupa.Count();
                }

                return statistike;
            }
            catch (Exception)
            {
                return new Dictionary<string, int>();
            }
        }

        // metoda za dobijanje mandata po strankama
        public Dictionary<string, int> DajStatistikeStranaka()
        {
            try
            {
                List<MandatDTO> mandati = DajSveMandate();
                var statistike = new Dictionary<string, int>();
                
                var grupeStranaka = mandati.GroupBy(m => m.NazivStranke ?? "Nezavisni");
                
                foreach (var grupa in grupeStranaka)
                {
                    statistike[grupa.Key] = grupa.Count();
                }

                return statistike;
            }
            catch (Exception)
            {
                return new Dictionary<string, int>();
            }
        }

        // pomocna metoda za formatiranje tip korisnika
        public string FormatiraTipKorisnika(string tipKorisnika)
        {
            switch (tipKorisnika?.ToLower())
            {
                case "poslanik": return "👨‍💼 Poslanik";
                case "potpredsednik": return "🎯 Potpredsednik";
                case "predsednik": return "👑 Predsednik";
                default: return "❓ Nepoznato";
            }
        }
    }
}
