using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class MandatDBKlasa : TabelaKlasa
    {
        public MandatDBKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele) : base(novaKonekcija, noviNazivTabele)
        {
            // nesto drugo u vezi specificno ove klase
        }

        public DataSet DajSveMandate()
        {
            string upit = @"
                SELECT 
                    m.id_mandata,
                    l.id_lica,
                    l.ime + ' ' + l.prezime AS ime_prezime,
                    l.korisnicko_ime,
                    l.pol,
                    l.datumr,
                    l.bio,
                    s.id_saziva,
                    s.ime AS saziv_ime,
                    s.pocetak AS saziv_pocetak,
                    s.kraj AS saziv_kraj,
                    st.id_stranke,
                    st.naziv_stranke,
                    p.id_pozicije,
                    p.naziv_pozicije
                FROM mandat m
                LEFT JOIN lica l ON m.id_lica = l.id_lica
                LEFT JOIN saziv s ON m.id_saziva = s.id_saziva
                LEFT JOIN stranka st ON m.id_stranke = st.id_stranke
                LEFT JOIN pozicija p ON l.pozicija = p.id_pozicije
                ORDER BY s.id_saziva, l.prezime, l.ime";
            
            return this.DajPodatke(upit);
        }

        public DataSet DajMandatePoSazivu(int id_saziva)
        {
            string upit = @"
                SELECT 
                    m.id_mandata,
                    l.id_lica,
                    l.ime + ' ' + l.prezime AS ime_prezime,
                    l.korisnicko_ime,
                    l.pol,
                    l.datumr,
                    l.bio,
                    s.id_saziva,
                    s.ime AS saziv_ime,
                    s.pocetak AS saziv_pocetak,
                    s.kraj AS saziv_kraj,
                    st.id_stranke,
                    st.naziv_stranke,
                    p.id_pozicije,
                    p.naziv_pozicije
                FROM mandat m
                LEFT JOIN lica l ON m.id_lica = l.id_lica
                LEFT JOIN saziv s ON m.id_saziva = s.id_saziva
                LEFT JOIN stranka st ON m.id_stranke = st.id_stranke
                LEFT JOIN pozicija p ON l.pozicija = p.id_pozicije
                WHERE m.id_saziva = " + id_saziva + @"
                ORDER BY s.id_saziva, l.prezime, l.ime";
            
            return this.DajPodatke(upit);
        }

        /// <summary>
        /// Dohvata sve mandate za određeni saziv kao listu objekata
        /// </summary>
        /// <param name="sazivId">ID saziva</param>
        /// <returns>Lista mandata za saziv</returns>
        public List<MandatKlasa> DajMandatePoSazivuLista(int sazivId)
        {
            List<MandatKlasa> mandati = new List<MandatKlasa>();
            string upit = $"SELECT m.*, l.ime, l.prezime, s.naziv_stranke, p.naziv_pozicije " +
                         $"FROM mandat m " +
                         $"INNER JOIN lica l ON m.id_lica = l.id_lica " +
                         $"INNER JOIN stranka s ON l.stranka = s.id_stranke " +
                         $"INNER JOIN pozicija p ON l.pozicija = p.id_pozicije " +
                         $"WHERE m.id_saziva = {sazivId} " +
                         $"ORDER BY l.prezime, l.ime";
            
            DataSet ds = this.DajPodatke(upit);
            if (ds?.Tables?.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    mandati.Add(new MandatKlasa
                    {
                        Id_mandata = Convert.ToInt32(row["id_mandata"]),
                        Id_lica = Convert.ToInt32(row["id_lica"]),
                        Id_saziva = Convert.ToInt32(row["id_saziva"]),
                        Id_stranke = Convert.ToInt32(row["id_stranke"]),
                        // Dodajemo dodatne informacije za prikaz
                        ImeLica = row["ime"]?.ToString() ?? "",
                        PrezimeLica = row["prezime"]?.ToString() ?? "",
                        NazivStranke = row["naziv_stranke"]?.ToString() ?? "",
                        NazivPozicije = row["naziv_pozicije"]?.ToString() ?? ""
                    });
                }
            }
            return mandati;
        }

        public DataSet DajMandatePoFilterima(int? id_saziva = null, int? id_stranke = null, int? id_pozicije = null, string ime_prezime = null)
        {
            string upit = @"
                SELECT 
                    m.id_mandata,
                    l.id_lica,
                    l.ime + ' ' + l.prezime AS ime_prezime,
                    l.korisnicko_ime,
                    l.pol,
                    l.datumr,
                    l.bio,
                    s.id_saziva,
                    s.ime AS saziv_ime,
                    s.pocetak AS saziv_pocetak,
                    s.kraj AS saziv_kraj,
                    st.id_stranke,
                    st.naziv_stranke,
                    p.id_pozicije,
                    p.naziv_pozicije
                FROM mandat m
                LEFT JOIN lica l ON m.id_lica = l.id_lica
                LEFT JOIN saziv s ON m.id_saziva = s.id_saziva
                LEFT JOIN stranka st ON m.id_stranke = st.id_stranke
                LEFT JOIN pozicija p ON l.pozicija = p.id_pozicije
                WHERE 1=1";
            
            if (id_saziva.HasValue)
            {
                upit += $" AND m.id_saziva = {id_saziva.Value}";
            }
            
            if (id_stranke.HasValue)
            {
                upit += $" AND m.id_stranke = {id_stranke.Value}";
            }
            
            if (id_pozicije.HasValue)
            {
                upit += $" AND l.pozicija = {id_pozicije.Value}";
            }
            
            if (!string.IsNullOrEmpty(ime_prezime))
            {
                upit += $" AND (l.ime + ' ' + l.prezime LIKE '%{ime_prezime}%')";
            }
            
            upit += " ORDER BY s.id_saziva, l.prezime, l.ime";
            
            return this.DajPodatke(upit);
        }

        public DataSet DajMandatPoId(int id_mandata)
        {
            string upit = "SELECT * FROM mandat WHERE id_mandata = " + id_mandata;
            return this.DajPodatke(upit);
        }

        public DataSet DajMandatePoLici(int id_lica)
        {
            string upit = "SELECT * FROM mandat WHERE id_lica = " + id_lica + " ORDER BY id_saziva";
            return this.DajPodatke(upit);
        }

        public bool DodajNoviMandat(MandatKlasa noviMandat)
        {
            // Generiši novi ID
            int noviId = DajNajnovijiMandatId() + 1;
            
            // Prema bazi podataka, mandat tabela ima samo: id_mandata, id_lica, id_saziva, id_stranke
            string upit = "INSERT INTO mandat (id_mandata, id_lica, id_saziva, id_stranke) VALUES (" + 
                         noviId + ", " + 
                         noviMandat.Id_lica + ", " + 
                         noviMandat.Id_saziva + ", " + 
                         noviMandat.Id_stranke + ")";
            return this.IzvrsiAzuriranje(upit);
        }

        public List<MandatKlasa> DajMandatePoLiciISazivu(int liceId, int sazivId)
        {
            List<MandatKlasa> mandati = new List<MandatKlasa>();
            string upit = "SELECT * FROM mandat WHERE id_lica = " + liceId + " AND id_saziva = " + sazivId;
            DataSet ds = this.DajPodatke(upit);
            
            if (ds?.Tables?.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    mandati.Add(new MandatKlasa
                    {
                        Id_mandata = Convert.ToInt32(row["id_mandata"]),
                        Id_lica = Convert.ToInt32(row["id_lica"]),
                        Id_saziva = Convert.ToInt32(row["id_saziva"]),
                        Id_stranke = Convert.ToInt32(row["id_stranke"])
                    });
                }
            }
            
            return mandati;
        }

        public bool IzmeniMandat(MandatKlasa mandatObjekat)
        {
            string upit = "UPDATE mandat SET id_lica = " + mandatObjekat.Id_lica + 
                         ", id_saziva = " + mandatObjekat.Id_saziva + 
                         ", id_stranke = " + mandatObjekat.Id_stranke + 
                         " WHERE id_mandata = " + mandatObjekat.Id_mandata;
            return this.IzvrsiAzuriranje(upit);
        }

        public bool ObrisiMandat(int id_mandata)
        {
            string upit = "DELETE FROM mandat WHERE id_mandata = " + id_mandata;
            return this.IzvrsiAzuriranje(upit);
        }

        public int DajNajnovijiMandatId()
        {
            string upit = "SELECT TOP 1 id_mandata FROM mandat ORDER BY id_mandata DESC";
            DataSet rezultat = this.DajPodatke(upit);
            if (rezultat.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_mandata"]);
            }
            return 0;
        }

        public bool PostojiMandat(int id_lica, int id_saziva)
        {
            string upit = "SELECT COUNT(*) as broj FROM mandat WHERE id_lica = " + id_lica + " AND id_saziva = " + id_saziva;
            DataSet rezultat = this.DajPodatke(upit);
            return Convert.ToInt32(rezultat.Tables[0].Rows[0]["broj"]) > 0;
        }

        public bool PostojiMandatPoStranci(int id_lica, int id_saziva, int id_stranke)
        {
            string upit = "SELECT COUNT(*) as broj FROM mandat WHERE id_lica = " + id_lica + 
                         " AND id_saziva = " + id_saziva + " AND id_stranke = " + id_stranke;
            DataSet rezultat = this.DajPodatke(upit);
            return Convert.ToInt32(rezultat.Tables[0].Rows[0]["broj"]) > 0;
        }





        // ========================================================================
        // METODE ZA STRANKE - NOVE METODE KOJE VRACAJU LISTE
        // ========================================================================

        /// <summary>
        /// Dohvata sve stranke kao listu StrankaKlasa objekata
        /// </summary>
        /// <returns>Lista svih stranaka</returns>
        public List<StrankaKlasa> DajSveStranke()
        {
            List<StrankaKlasa> stranke = new List<StrankaKlasa>();
            DataSet ds = this.DajSveStrankeDataSet();
            if (ds?.Tables?.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    stranke.Add(new StrankaKlasa
                    {
                        Id_stranke = Convert.ToInt32(row["id_stranke"]),
                        Naziv_stranke = row["naziv_stranke"].ToString()
                    });
                }
            }
            return stranke;
        }

        // ========================================================================
        // METODE ZA POZICIJE - NOVE METODE KOJE VRACAJU LISTE
        // ========================================================================

        /// <summary>
        /// Dohvata sve pozicije kao listu PozicijaKlasa objekata
        /// </summary>
        /// <returns>Lista svih pozicija</returns>
        public List<PozicijaKlasa> DajSvePozicije()
        {
            List<PozicijaKlasa> pozicije = new List<PozicijaKlasa>();
            DataSet ds = this.DajSvePozicijeDataSet();
            if (ds?.Tables?.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    pozicije.Add(new PozicijaKlasa
                    {
                        Id_pozicije = Convert.ToInt32(row["id_pozicije"]),
                        Naziv_pozicije = row["naziv_pozicije"].ToString()
                    });
                }
            }
            return pozicije;
        }

        // ========================================================================
        // PREIMENOVANE POSTOJEĆE METODE DA IZBEGNEMO KONFLIKTE
        // ========================================================================

        /// <summary>
        /// Dohvata sve stranke kao DataSet (preimenovana metoda)
        /// </summary>
        /// <returns>DataSet sa svim strankama</returns>
        public DataSet DajSveStrankeDataSet()
        {
            return this.DajPodatke("SELECT * FROM stranka ORDER BY naziv_stranke");
        }

        /// <summary>
        /// Dohvata sve pozicije kao DataSet (preimenovana metoda)
        /// </summary>
        /// <returns>DataSet sa svim pozicijama</returns>
        public DataSet DajSvePozicijeDataSet()
        {
            return this.DajPodatke("SELECT * FROM pozicija ORDER BY naziv_pozicije");
        }
    }
}
