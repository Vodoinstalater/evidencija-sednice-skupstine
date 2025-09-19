using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using KlaseMapiranja;
using System.Data;
using PoslovnaLogika;

namespace PrezentacionaLogika
{
    public class SednicaLoginKlasa
    {
        private SednicaServisKlasa _servis;
        private string _korisnickoIme;
        private string _sifra;
        private KorisnikDTO _trenutniKorisnik;

        public string KorisnickoIme
        {
            get { return _korisnickoIme; }
            set { _korisnickoIme = value; }
        }

        public string Sifra
        {
            get { return _sifra; }
            set { _sifra = value; }
        }

        public KorisnikDTO TrenutniKorisnik
        {
            get { return _trenutniKorisnik; }
        }

        // konstruktor koji koristi default konekciju
        public SednicaLoginKlasa()
        {
            _servis = new SednicaServisKlasa(""); // koristi default konekciju iz DBUtils
        }

        // konstruktor sa custom konekcijom (za kompatibilnost)
        public SednicaLoginKlasa(string stringKonekcije)
        {
            _servis = new SednicaServisKlasa(stringKonekcije);
        }

        // javne metode
        public bool VazeciKorisnik()
        {
            try
            {
                // poziv Layer 3 servisa za autentifikaciju
                ServiceResult<KorisnikDTO> rezultat = _servis.PrijaviKorisnika(_korisnickoIme, _sifra);
                
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    _trenutniKorisnik = rezultat.Podaci;
                    return true;
                }
                else
                {
                    _trenutniKorisnik = null;
                    return false;
                }
            }
            catch (Exception)
            {
                _trenutniKorisnik = null;
                return false;
            }
        }

        public string DajImePrezimeKorisnika()
        {
            if (_trenutniKorisnik != null)
            {
                return _trenutniKorisnik.ImePrezime;
            }
            return "";
        }

        public string DajTipKorisnika()
        {
            if (_trenutniKorisnik != null)
            {
                return _trenutniKorisnik.TipKorisnika;
            }
            return "";
        }

        public int DajPozicijuKorisnika()
        {
            if (_trenutniKorisnik != null)
            {
                // Dohvati poziciju iz baze podataka
                try
                {
                    SednicaPoslovnaLogikaKlasa poslovnaLogika = new SednicaPoslovnaLogikaKlasa("");
                    DataSet dsKorisnik = poslovnaLogika.DajPodatkeKorisnika(_korisnickoIme, out string poruka);
                    
                    if (dsKorisnik != null && dsKorisnik.Tables.Count > 0 && dsKorisnik.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = dsKorisnik.Tables[0].Rows[0];
                        if (row["pozicija"] != null && row["pozicija"] != DBNull.Value)
                        {
                            return Convert.ToInt32(row["pozicija"]);
                        }
                    }
                }
                catch (Exception)
                {
                    // Ako ne može da dohvati iz baze, vrati 0
                }
            }
            return 0;
        }

        // metoda za proveru dozvole
        public bool ImaDozvolu(string akcija)
        {
            if (_trenutniKorisnik != null)
            {
                ServiceResult<bool> rezultat = _servis.ProveriDozvolu(_trenutniKorisnik.TipKorisnika, akcija);
                return rezultat.Uspesno && rezultat.Podaci;
            }
            return false;
        }

        // pomocne metode za role - koriste TipKorisnika umesto pozicije
        public bool JePoslanik()
        {
            return DajTipKorisnika().ToLower() == "poslanik";
        }

        public bool JePotpredsednik()
        {
            return DajTipKorisnika().ToLower() == "potpredsednik";
        }

        public bool JePredsednik()
        {
            return DajTipKorisnika().ToLower() == "predsednik";
        }
    }
}
