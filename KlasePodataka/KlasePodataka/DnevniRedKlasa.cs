using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class DnevniRedKlasa
    {
        // atributi
        private int _id_dnevni_red;
        private int _id_sednice;
        private SednicaKlasa _sednicaObjekat;

        // property
        public int Id_dnevni_red
        {
            get { return _id_dnevni_red; }
            set { _id_dnevni_red = value; }
        }

        public int Id_sednice
        {
            get { return _id_sednice; }
            set { _id_sednice = value; }
        }

        public SednicaKlasa Sednica
        {
            get { return _sednicaObjekat; }
            set { _sednicaObjekat = value; }
        }
    }
}
