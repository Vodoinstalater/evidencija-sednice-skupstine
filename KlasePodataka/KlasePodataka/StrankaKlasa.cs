using System;

namespace KlasePodataka
{
    /// <summary>
    /// Klasa za stranke
    /// </summary>
    public class StrankaKlasa
    {
        // atributi
        private int _id_stranke;
        private string _naziv_stranke;

        // konstruktor
        public StrankaKlasa()
        {
            _id_stranke = 0;
            _naziv_stranke = "";
        }

        public StrankaKlasa(int id_stranke, string naziv_stranke)
        {
            _id_stranke = id_stranke;
            _naziv_stranke = naziv_stranke;
        }

        // property
        public int Id_stranke
        {
            get { return _id_stranke; }
            set { _id_stranke = value; }
        }

        public string Naziv_stranke
        {
            get { return _naziv_stranke; }
            set { _naziv_stranke = value; }
        }
    }
}
