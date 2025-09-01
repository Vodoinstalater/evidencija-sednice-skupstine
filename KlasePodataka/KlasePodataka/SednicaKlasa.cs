using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class SednicaKlasa
    {
        // atributi
        private int _id_sednice;
        private string _naziv;
        private DateTime _datum;
        private string _opis;
        private int _zasedanje_id;
        private ZasedanjeKlasa _zasedanjeObjekat;

        // property
        public int Id_sednice
        {
            get { return _id_sednice; }
            set { _id_sednice = value; }
        }

        public string Naziv
        {
            get { return _naziv; }
            set { _naziv = value; }
        }

        public DateTime Datum
        {
            get { return _datum; }
            set { _datum = value; }
        }

        public string Opis
        {
            get { return _opis; }
            set { _opis = value; }
        }

        public int Zasedanje_id
        {
            get { return _zasedanje_id; }
            set { _zasedanje_id = value; }
        }

        public ZasedanjeKlasa Zasedanje
        {
            get { return _zasedanjeObjekat; }
            set { _zasedanjeObjekat = value; }
        }
    }
}
