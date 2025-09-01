using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class UpravljanjeMandatimaKlasa
    {
        private LicaDBKlasa _licaDB;
        private MandatDBKlasa _mandatDB;
        private SazivDBKlasa _sazivDB;
        private KonekcijaKlasa _konekcija;

        public UpravljanjeMandatimaKlasa(KonekcijaKlasa konekcija)
        {
            _konekcija = konekcija;
            _licaDB = new LicaDBKlasa(konekcija, "lica");
            _mandatDB = new MandatDBKlasa(konekcija, "mandat");
            _sazivDB = new SazivDBKlasa(konekcija, "saziv");
        }

        public bool DodajNovoLica(LicaKlasa novaLica)
        {
            try
            {
                // Proveri da li već postoji korisničko ime
                if (_licaDB.PostojiLicaSaKorisnickimImenom(novaLica.Korisnicko_ime))
                {
                    return false; // Korisničko ime već postoji
                }

                // Dodaj novo lice
                return _licaDB.DodajNovaLica(novaLica);
            }
            catch
            {
                return false;
            }
        }

        public bool DodajMandat(int id_lica, int id_saziva, int id_stranke)
        {
            try
            {
                // Proveri da li već postoji mandat za ovo lice u ovom sazivu
                if (_mandatDB.PostojiMandat(id_lica, id_saziva))
                {
                    return false; // Mandat već postoji
                }

                // Proveri da li već postoji mandat za ovo lice u ovom sazivu sa istom strankom
                if (_mandatDB.PostojiMandatPoStranci(id_lica, id_saziva, id_stranke))
                {
                    return false; // Mandat sa istom strankom već postoji
                }

                // Kreiraj novi mandat
                MandatKlasa noviMandat = new MandatKlasa();
                noviMandat.Id_lica = id_lica;
                noviMandat.Id_saziva = id_saziva;
                noviMandat.Id_stranke = id_stranke;

                return _mandatDB.DodajNoviMandat(noviMandat);
            }
            catch
            {
                return false;
            }
        }

        public bool UkloniMandat(int id_lica, int id_saziva)
        {
            try
            {
                // Pronađi mandat za ovo lice u ovom sazivu
                string upit = "SELECT id_mandata FROM mandat WHERE id_lica = " + id_lica + " AND id_saziva = " + id_saziva;
                DataSet rezultat = _mandatDB.DajPodatke(upit);
                
                if (rezultat.Tables[0].Rows.Count > 0)
                {
                    int id_mandata = Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_mandata"]);
                    return _mandatDB.ObrisiMandat(id_mandata);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool IzmeniMandat(int id_lica, int id_saziva, int novaStranka)
        {
            try
            {
                // Pronađi mandat za ovo lice u ovom sazivu
                string upit = "SELECT id_mandata FROM mandat WHERE id_lica = " + id_lica + " AND id_saziva = " + id_saziva;
                DataSet rezultat = _mandatDB.DajPodatke(upit);
                
                if (rezultat.Tables[0].Rows.Count > 0)
                {
                    int id_mandata = Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_mandata"]);
                    
                    // Proveri da li već postoji mandat sa novom strankom
                    if (_mandatDB.PostojiMandatPoStranci(id_lica, id_saziva, novaStranka))
                    {
                        return false; // Mandat sa novom strankom već postoji
                    }

                    // Izmeni mandat
                    string upitIzmeni = "UPDATE mandat SET id_stranke = " + novaStranka + 
                                       " WHERE id_mandata = " + id_mandata;
                    return _mandatDB.IzvrsiAzuriranje(upitIzmeni);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<LicaKlasa> DajSvaLica()
        {
            try
            {
                return _licaDB.DajSvaLica();
            }
            catch
            {
                // U slučaju greške, vrati praznu listu
                return new List<LicaKlasa>();
            }
        }

        public List<MandatKlasa> DajMandatePoSazivu(int id_saziva)
        {
            List<MandatKlasa> listaMandata = new List<MandatKlasa>();
            try
            {
                DataSet rezultat = _mandatDB.DajMandatePoSazivu(id_saziva);
                foreach (DataRow red in rezultat.Tables[0].Rows)
                {
                    MandatKlasa mandat = new MandatKlasa();
                    mandat.Id_mandata = Convert.ToInt32(red["id_mandata"]);
                    mandat.Id_lica = Convert.ToInt32(red["id_lica"]);
                    mandat.Id_saziva = Convert.ToInt32(red["id_saziva"]);
                    mandat.Id_stranke = Convert.ToInt32(red["id_stranke"]);
                    listaMandata.Add(mandat);
                }
            }
            catch
            {
                // U slučaju greške, vrati praznu listu
            }
            return listaMandata;
        }

        public DataSet DajMandateSaDetaljima(int id_saziva)
        {
            try
            {
                string upit = @"SELECT m.id_mandata, m.id_lica, m.id_saziva, m.id_stranke,
                                      l.ime + ' ' + l.prezime as puno_ime, l.pozicija, l.pol,
                                      s.naziv_stranke, sa.ime as saziv_ime
                               FROM mandat m
                               INNER JOIN lica l ON m.id_lica = l.id_lica
                               INNER JOIN stranka s ON m.id_stranke = s.id_stranke
                               INNER JOIN saziv sa ON m.id_saziva = sa.id_saziva
                               WHERE m.id_saziva = " + id_saziva + 
                               " ORDER BY s.naziv_stranke, l.prezime, l.ime";
                
                return _mandatDB.DajPodatke(upit);
            }
            catch
            {
                return new DataSet();
            }
        }
    }
}
