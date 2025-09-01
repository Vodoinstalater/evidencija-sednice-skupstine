using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class SazivKlasa
    {
        // atributi
        private int _id_saziva;
        private string _ime;
        private DateTime _pocetak; 
        private DateTime _kraj;
        private string _opis;

        // property
        public int Id_saziva
        {
            get { return _id_saziva; }
            set { _id_saziva = value; }
        }

        public string Ime
        {
            get { return _ime; }
            set { _ime = value; }
        }

        public DateTime Pocetak
        {
            get { return _pocetak; }
            set { _pocetak = value; }
        }

        public DateTime Kraj
        {
            get { return _kraj; }
            set { _kraj = value; }
        }

        public string Opis
        {
            get { return _opis; }
            set { _opis = value; }
        }
    }
}
