using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class ZasedanjeKlasa
    {
        // atributi
        private int _id_zasedanja;
        private int _tip;
        private string _naziv_zasedanja;
        private int _id_saziv;
        private SazivKlasa _sazivObjekat;

        // property
        public int Id_zasedanja
        {
            get { return _id_zasedanja; }
            set { _id_zasedanja = value; }
        }

        public int Tip
        {
            get { return _tip; }
            set { _tip = value; }
        }

        public string Naziv_zasedanja
        {
            get { return _naziv_zasedanja; }
            set { _naziv_zasedanja = value; }
        }

        public int Id_saziv
        {
            get { return _id_saziv; }
            set { _id_saziv = value; }
        }

        public SazivKlasa Saziv
        {
            get { return _sazivObjekat; }
            set { _sazivObjekat = value; }
        }
    }
}
