using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class UpravljanjeZasedanjimaKlasa
    {
        private ZasedanjeDBKlasa _zasedanjeDB;
        private SazivDBKlasa _sazivDB;
        private KonekcijaKlasa _konekcija;

        public UpravljanjeZasedanjimaKlasa(KonekcijaKlasa konekcija)
        {
            _konekcija = konekcija;
            _zasedanjeDB = new ZasedanjeDBKlasa(konekcija, "zasedanje");
            _sazivDB = new SazivDBKlasa(konekcija, "saziv");
        }

        public bool KreirajNovoZasedanje(ZasedanjeKlasa novoZasedanje)
        {
            try
            {
                // Proveri da li saziv postoji
                DataSet saziv = _sazivDB.DajSazivPoId(novoZasedanje.Id_saziv);
                if (saziv.Tables[0].Rows.Count == 0)
                {
                    return false; // Saziv ne postoji
                }

                // Kreiraj novo zasedanje
                return _zasedanjeDB.DodajNovoZasedanje(novoZasedanje);
            }
            catch
            {
                return false;
            }
        }

        public bool KreirajNovoZasedanjeSaTipom(int tip, string nazivZasedanja, int idSaziva)
        {
            try
            {
                // Proveri da li saziv postoji
                DataSet saziv = _sazivDB.DajSazivPoId(idSaziva);
                if (saziv.Tables[0].Rows.Count == 0)
                {
                    return false; // Saziv ne postoji
                }

                // Kreiraj novo zasedanje
                ZasedanjeKlasa novoZasedanje = new ZasedanjeKlasa();
                novoZasedanje.Tip = tip;
                novoZasedanje.Naziv_zasedanja = nazivZasedanja;
                novoZasedanje.Id_saziv = idSaziva;

                return _zasedanjeDB.DodajNovoZasedanje(novoZasedanje);
            }
            catch
            {
                return false;
            }
        }

        public bool KreirajNovoZasedanjeZaAktivanSaziv(int tip, string nazivZasedanja)
        {
            try
            {
                // Dohvati aktivan saziv
                DateTime danas = DateTime.Today;
                string upit = "SELECT id_saziva FROM saziv WHERE pocetak <= '" + danas.ToString("yyyy-MM-dd") + 
                             "' AND kraj >= '" + danas.ToString("yyyy-MM-dd") + "'";
                DataSet rezultat = _sazivDB.DajPodatke(upit);
                
                if (rezultat.Tables[0].Rows.Count == 0)
                {
                    return false; // Nema aktivan saziv
                }

                int idAktivnogSaziva = Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_saziva"]);

                // Kreiraj novo zasedanje za aktivan saziv
                return KreirajNovoZasedanjeSaTipom(tip, nazivZasedanja, idAktivnogSaziva);
            }
            catch
            {
                return false;
            }
        }

        public bool ZatvoriZasedanje(int idZasedanja)
        {
            try
            {
                // Ova metoda može biti proširena u budućnosti za zatvaranje zasedanja
                // Za sada samo vraća true jer zasedanja se ne zatvaraju automatski
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ZasedanjeKlasa> DajSvaZasedanja()
        {
            List<ZasedanjeKlasa> listaZasedanja = new List<ZasedanjeKlasa>();
            try
            {
                DataSet rezultat = _zasedanjeDB.DajSvaZasedanja();
                foreach (DataRow red in rezultat.Tables[0].Rows)
                {
                    ZasedanjeKlasa zasedanje = new ZasedanjeKlasa();
                    zasedanje.Id_zasedanja = Convert.ToInt32(red["id_zasedanja"]);
                    // Безбедно конвертовање tip поља - могуће је да је string або int
                    if (int.TryParse(red["tip"].ToString(), out int tipValue))
                    {
                        zasedanje.Tip = tipValue;
                    }
                    else
                    {
                        zasedanje.Tip = 1; // Default у случају greške
                    }
                    zasedanje.Naziv_zasedanja = red["naziv_zasedanja"].ToString();
                    zasedanje.Id_saziv = Convert.ToInt32(red["id_saziv"]);
                    listaZasedanja.Add(zasedanje);
                }
            }
            catch
            {
                // U slučaju greške, vrati praznu listu
            }
            return listaZasedanja;
        }

        public List<ZasedanjeKlasa> DajZasedanjaPoSazivu(int idSaziva)
        {
            List<ZasedanjeKlasa> listaZasedanja = new List<ZasedanjeKlasa>();
            try
            {
                DataSet rezultat = _zasedanjeDB.DajZasedanjaPoSazivu(idSaziva);
                foreach (DataRow red in rezultat.Tables[0].Rows)
                {
                    ZasedanjeKlasa zasedanje = new ZasedanjeKlasa();
                    zasedanje.Id_zasedanja = Convert.ToInt32(red["id_zasedanja"]);
                    // Безбедно конвертовање tip поља - могуће је да је string або int
                    if (int.TryParse(red["tip"].ToString(), out int tipValue))
                    {
                        zasedanje.Tip = tipValue;
                    }
                    else
                    {
                        zasedanje.Tip = 1; // Default у случају greške
                    }
                    zasedanje.Naziv_zasedanja = red["naziv_zasedanja"].ToString();
                    zasedanje.Id_saziv = Convert.ToInt32(red["id_saziv"]);
                    listaZasedanja.Add(zasedanje);
                }
            }
            catch
            {
                // U slučaju greške, vrati praznu listu
            }
            return listaZasedanja;
        }

        public DataSet DajZasedanjaSaDetaljima(int idSaziva = 0)
        {
            try
            {
                string upit;
                if (idSaziva > 0)
                {
                    upit = @"SELECT z.id_zasedanja, z.tip, z.naziv_zasedanja, z.id_saziv,
                                   sa.ime as saziv_ime, sa.pocetak, sa.kraj,
                                   t.tip_zasedanja
                            FROM zasedanje z
                            INNER JOIN saziv sa ON z.id_saziv = sa.id_saziva
                            LEFT JOIN tipovi t ON z.tip = t.id
                            WHERE z.id_saziv = " + idSaziva + 
                            " ORDER BY z.id_zasedanja";
                }
                else
                {
                    upit = @"SELECT z.id_zasedanja, z.tip, z.naziv_zasedanja, z.id_saziv,
                                   sa.ime as saziv_ime, sa.pocetak, sa.kraj,
                                   t.tip_zasedanja
                            FROM zasedanje z
                            INNER JOIN saziv sa ON z.id_saziv = sa.id_saziva
                            LEFT JOIN tipovi t ON z.tip = t.id
                            ORDER BY sa.pocetak DESC, z.id_zasedanja";
                }
                
                return _zasedanjeDB.DajPodatke(upit);
            }
            catch
            {
                return new DataSet();
            }
        }

        public ZasedanjeKlasa DajNajnovijeZasedanje()
        {
            try
            {
                int najnovijiId = _zasedanjeDB.DajNajnovijeZasedanjeId();
                if (najnovijiId > 0)
                {
                    DataSet rezultat = _zasedanjeDB.DajZasedanjePoId(najnovijiId);
                    if (rezultat.Tables[0].Rows.Count > 0)
                    {
                        DataRow red = rezultat.Tables[0].Rows[0];
                        ZasedanjeKlasa zasedanje = new ZasedanjeKlasa();
                        zasedanje.Id_zasedanja = Convert.ToInt32(red["id_zasedanja"]);
                        zasedanje.Tip = Convert.ToInt32(red["tip"]);
                        zasedanje.Naziv_zasedanja = red["naziv_zasedanja"].ToString();
                        zasedanje.Id_saziv = Convert.ToInt32(red["id_saziv"]);
                        return zasedanje;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public ZasedanjeKlasa DajNajnovijeZasedanjePoSazivu(int idSaziva)
        {
            try
            {
                int najnovijiId = _zasedanjeDB.DajNajnovijeZasedanjePoSazivu(idSaziva);
                if (najnovijiId > 0)
                {
                    DataSet rezultat = _zasedanjeDB.DajZasedanjePoId(najnovijiId);
                    if (rezultat.Tables[0].Rows.Count > 0)
                    {
                        DataRow red = rezultat.Tables[0].Rows[0];
                        ZasedanjeKlasa zasedanje = new ZasedanjeKlasa();
                        zasedanje.Id_zasedanja = Convert.ToInt32(red["id_zasedanja"]);
                        zasedanje.Tip = Convert.ToInt32(red["tip"]);
                        zasedanje.Naziv_zasedanja = red["naziv_zasedanja"].ToString();
                        zasedanje.Id_saziv = Convert.ToInt32(red["id_saziv"]);
                        return zasedanje;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Kreira novo zasedanje sa validacijom
        /// </summary>
        public bool KreirajNovoZasedanjeSaValidacijom(string naziv, DateTime datum, string opis, int tipId, out string poruka)
        {
            poruka = "";
            try
            {
                // Dohvati aktivan saziv
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(_konekcija);
                SazivKlasa aktivanSaziv = upravljanjeSazivima.DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    poruka = "Ne postoji aktivan saziv. Prvo kreirajte novi saziv.";
                    return false;
                }

                // Kreiraj zasedanje objekat
                ZasedanjeKlasa novoZasedanje = new ZasedanjeKlasa
                {
                    Naziv_zasedanja = naziv,
                    Tip = tipId,
                    Id_saziv = aktivanSaziv.Id_saziva
                };

                // Koristi postojeću metodu
                bool uspesno = KreirajNovoZasedanje(novoZasedanje);
                if (uspesno)
                {
                    poruka = "Zasedanje je uspešno kreirano.";
                }
                else
                {
                    poruka = "Greška pri kreiranju zasedanja.";
                }
                return uspesno;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju zasedanja: {ex.Message}";
                return false;
            }
        }
    }
}
