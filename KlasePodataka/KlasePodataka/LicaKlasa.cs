using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class LicaKlasa
    {
        // atributi
        private int _id_lica;
        private string _ime;
        private string _prezime;
        private int _pozicija;
        private int _stranka;
        private char _pol;
        private DateTime _datumr;
        private string _bio;
        private string _korisnicko_ime;
        private string _lozinka;

        // property
        public int Id_lica
        {
            get { return _id_lica; }
            set { _id_lica = value; }
        }

        public string Ime
        {
            get { return _ime; }
            set { _ime = value; }
        }

        public string Prezime
        {
            get { return _prezime; }
            set { _prezime = value; }
        }

        public int Pozicija
        {
            get { return _pozicija; }
            set { _pozicija = value; }
        }

        public int Stranka
        {
            get { return _stranka; }
            set { _stranka = value; }
        }

        public char Pol
        {
            get { return _pol; }
            set { _pol = value; }
        }

        public DateTime Datumr
        {
            get { return _datumr; }
            set { _datumr = value; }
        }

        public string Bio
        {
            get { return _bio; }
            set { _bio = value; }
        }

        public string Korisnicko_ime
        {
            get { return _korisnicko_ime; }
            set { _korisnicko_ime = value; }
        }

        public string Lozinka
        {
            get { return _lozinka; }
            set { _lozinka = value; }
        }
    }
}
