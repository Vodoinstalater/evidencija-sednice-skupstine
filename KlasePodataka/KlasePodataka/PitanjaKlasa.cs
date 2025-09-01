using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class PitanjaKlasa
    {
        // atributi
        private int _id_pitanja;
        private int _id_dnevni_red;
        private int _redni_broj;
        private string _tekst;
        private DnevniRedKlasa _dnevniRedObjekat;

        // property
        public int Id_pitanja
        {
            get { return _id_pitanja; }
            set { _id_pitanja = value; }
        }

        public int Id_dnevni_red
        {
            get { return _id_dnevni_red; }
            set { _id_dnevni_red = value; }
        }

        public int Redni_broj
        {
            get { return _redni_broj; }
            set { _redni_broj = value; }
        }

        public string Tekst
        {
            get { return _tekst; }
            set { _tekst = value; }
        }

        public DnevniRedKlasa DnevniRed
        {
            get { return _dnevniRedObjekat; }
            set { _dnevniRedObjekat = value; }
        }
    }
}
