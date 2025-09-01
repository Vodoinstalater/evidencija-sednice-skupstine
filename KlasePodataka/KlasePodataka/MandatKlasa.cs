using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class MandatKlasa
    {
        // atributi - samo oni koji postoje u bazi podataka
        private int _id_mandata;
        private int _id_lica;
        private int _id_saziva;
        private int _id_stranke;
        private LicaKlasa _licaObjekat;
        private SazivKlasa _sazivObjekat;

        // Dodatne informacije za prikaz u grid-u (ne postoje u bazi, ali se koriste za UI)
        private string _imeLica;
        private string _prezimeLica;
        private string _nazivStranke;
        private string _nazivPozicije;

        // property - samo oni koji postoje u bazi podataka
        public int Id_mandata
        {
            get { return _id_mandata; }
            set { _id_mandata = value; }
        }

        public int Id_lica
        {
            get { return _id_lica; }
            set { _id_lica = value; }
        }

        public int Id_saziva
        {
            get { return _id_saziva; }
            set { _id_saziva = value; }
        }

        public int Id_stranke
        {
            get { return _id_stranke; }
            set { _id_stranke = value; }
        }

        public LicaKlasa Lica
        {
            get { return _licaObjekat; }
            set { _licaObjekat = value; }
        }

        public SazivKlasa Saziv
        {
            get { return _sazivObjekat; }
            set { _sazivObjekat = value; }
        }

        // Dodatne informacije za prikaz (ne postoje u bazi, ali se koriste za UI)
        public string ImeLica
        {
            get { return _imeLica; }
            set { _imeLica = value; }
        }

        public string PrezimeLica
        {
            get { return _prezimeLica; }
            set { _prezimeLica = value; }
        }

        public string NazivStranke
        {
            get { return _nazivStranke; }
            set { _nazivStranke = value; }
        }

        public string NazivPozicije
        {
            get { return _nazivPozicije; }
            set { _nazivPozicije = value; }
        }
    }
}
