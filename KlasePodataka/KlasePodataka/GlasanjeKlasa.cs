using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    /// <summary>
    /// Klasa koja predstavlja entitet glasanja u bazi podataka
    /// </summary>
    public class GlasanjeKlasa
    {
        // atributi
        private int _idGlasanja;
        private int _idPitanja;
        private int _idLica;
        private string _glas;

        // konstruktor
        public GlasanjeKlasa()
        {
            _idGlasanja = 0;
            _idPitanja = 0;
            _idLica = 0;
            _glas = "";
        }

        // konstruktor sa parametrima
        public GlasanjeKlasa(int idGlasanja, int idPitanja, int idLica, string glas)
        {
            _idGlasanja = idGlasanja;
            _idPitanja = idPitanja;
            _idLica = idLica;
            _glas = glas;
        }

        // properties
        public int IdGlasanja
        {
            get { return _idGlasanja; }
            set { _idGlasanja = value; }
        }

        public int IdPitanja
        {
            get { return _idPitanja; }
            set { _idPitanja = value; }
        }

        public int IdLica
        {
            get { return _idLica; }
            set { _idLica = value; }
        }

        public string Glas
        {
            get { return _glas; }
            set { _glas = value; }
        }

        // navigation properties
        public virtual PitanjaKlasa Pitanja { get; set; }
        public virtual LicaKlasa Lica { get; set; }
    }
}
