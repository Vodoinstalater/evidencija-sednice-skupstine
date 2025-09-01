using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//
using KlaseMapiranja;

namespace PrezentacionaLogika
{
    public class IstorijaGlasanjaKlasa
    {
        private SednicaServisKlasa _servis;
        private string _stringKonekcije;

        // konstruktor
        public IstorijaGlasanjaKlasa()
        {
            _stringKonekcije = "";
            _servis = new SednicaServisKlasa(_stringKonekcije);
        }

        public IstorijaGlasanjaKlasa(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
            _servis = new SednicaServisKlasa(_stringKonekcije);
        }

        // metoda za dobijanje istorije glasanja
        public DataSet DajIstorijuGlasanja()
        {
            try
            {
                ServiceResult<DataSet> rezultat = _servis.DajIstorijuGlasanja();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new DataSet();
            }
            catch (Exception)
            {
                return new DataSet();
            }
        }

        // metoda za dobijanje statistika glasanja
        public Dictionary<string, int> DajStatistikeGlasanja()
        {
            try
            {
                DataSet glasanje = DajIstorijuGlasanja();
                var statistike = new Dictionary<string, int>
                {
                    { "Za", 0 },
                    { "Protiv", 0 },
                    { "Uzdržan", 0 }
                };

                if (glasanje.Tables.Count > 0 && glasanje.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow red in glasanje.Tables[0].Rows)
                    {
                        string glas = red["glas"]?.ToString()?.ToLower();
                        switch (glas)
                        {
                            case "za":
                                statistike["Za"]++;
                                break;
                            case "protiv":
                                statistike["Protiv"]++;
                                break;
                            case "uzdrzan":
                                statistike["Uzdržan"]++;
                                break;
                        }
                    }
                }

                return statistike;
            }
            catch (Exception)
            {
                return new Dictionary<string, int>
                {
                    { "Za", 0 },
                    { "Protiv", 0 },
                    { "Uzdržan", 0 }
                };
            }
        }

        // metoda za formatiranje glasa
        public string FormatiraiGlas(string glas)
        {
            switch (glas?.ToLower())
            {
                case "za": return "✅ Za";
                case "protiv": return "❌ Protiv";
                case "uzdrzan": return "⚪ Uzdržan";
                default: return "❓ Nepoznato";
            }
        }

        // metoda za dobijanje ukupnog broja glasova
        public int DajUkupanBrojGlasova()
        {
            try
            {
                DataSet glasanje = DajIstorijuGlasanja();
                if (glasanje.Tables.Count > 0)
                {
                    return glasanje.Tables[0].Rows.Count;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
