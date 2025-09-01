using System;

namespace KlasePodataka
{
    /// <summary>
    /// Klasa za pozicije
    /// </summary>
    public class PozicijaKlasa
    {
        // atributi
        private int _id_pozicije;
        private string _naziv_pozicije;

        // konstruktor
        public PozicijaKlasa()
        {
            _id_pozicije = 0;
            _naziv_pozicije = "";
        }

        public PozicijaKlasa(int id_pozicije, string naziv_pozicije)
        {
            _id_pozicije = id_pozicije;
            _naziv_pozicije = naziv_pozicije;
        }

        // property
        public int Id_pozicije
        {
            get { return _id_pozicije; }
            set { _id_pozicije = value; }
        }

        public string Naziv_pozicije
        {
            get { return _naziv_pozicije; }
            set { _naziv_pozicije = value; }
        }
    }
}
