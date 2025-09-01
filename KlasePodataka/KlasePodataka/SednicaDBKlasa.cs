using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
    public class SednicaDBKlasa : TabelaKlasa
    {
        public SednicaDBKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele) : base(novaKonekcija, noviNazivTabele)
        {
            // nesto drugo u vezi specificno ove klase
        }

        public DataSet DajSveSednice()
        {
            return this.DajPodatke("SELECT * FROM sednica ORDER BY datum DESC");
        }

        public DataSet DajSednicuPoId(int id_sednice)
        {
            string upit = "SELECT * FROM sednica WHERE id_sednice = " + id_sednice;
            return this.DajPodatke(upit);
        }

        public DataSet DajSednicePoZasedanju(int id_zasedanja)
        {
            string upit = "SELECT * FROM sednica WHERE zasedanje_id = " + id_zasedanja + " ORDER BY datum DESC";
            return this.DajPodatke(upit);
        }

        public DataSet DajSednicePoSazivuIZasedanju(int? id_saziva = null, int? id_zasedanja = null)
        {
            try
            {
                // Koristi direktan SQL upit umesto stored procedure jer sp_sveSednice ne postoji
                string sqlUpit = @"
                    SELECT 
                        s.id_sednice,
                        s.naziv AS sednica_naziv,
                        s.datum AS sednica_datum,
                        s.opis AS sednica_opis,
                        z.id_zasedanja,
                        z.naziv_zasedanja,
                        sa.id_saziva,
                        sa.ime AS saziv_Ime,
                        sa.pocetak,
                        sa.kraj
                    FROM dbo.sednica s
                    LEFT JOIN dbo.zasedanje z ON s.zasedanje_id = z.id_zasedanja
                    LEFT JOIN dbo.saziv sa ON z.id_saziv = sa.id_saziva
                    WHERE 1=1";
                
                if (id_saziva.HasValue)
                {
                    sqlUpit += $" AND sa.id_saziva = {id_saziva.Value}";
                }
                
                if (id_zasedanja.HasValue)
                {
                    sqlUpit += $" AND z.id_zasedanja = {id_zasedanja.Value}";
                }
                
                sqlUpit += " ORDER BY sa.id_saziva, z.id_zasedanja, s.id_sednice";
                
                DataSet rezultat = this.DajPodatke(sqlUpit);
                return rezultat;
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri dohvatanju sednica: {ex.Message}", ex);
            }
        }

        public bool DodajNovuSednicu(SednicaKlasa novaSednicaObjekat)
        {
            try
            {
                // Generate new ID
                int noviId = DajNajnovijuSednicuId() + 1;
                
                string upit = "INSERT INTO sednica (id_sednice, naziv, datum, opis, zasedanje_id) VALUES (" + 
                             noviId + ", '" + 
                             novaSednicaObjekat.Naziv + "', '" + 
                             novaSednicaObjekat.Datum.ToString("yyyy-MM-dd") + "', '" + 
                             novaSednicaObjekat.Opis + "', " + 
                             novaSednicaObjekat.Zasedanje_id + ")";
                
                bool rezultat = this.IzvrsiAzuriranje(upit);
                return rezultat;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool IzmeniSednicu(SednicaKlasa sednicaObjekat)
        {
            string upit = "UPDATE sednica SET naziv = '" + sednicaObjekat.Naziv + 
                         "', datum = '" + sednicaObjekat.Datum.ToString("yyyy-MM-dd") + 
                         "', opis = '" + sednicaObjekat.Opis + 
                         "', zasedanje_id = " + sednicaObjekat.Zasedanje_id + 
                         " WHERE id_sednice = " + sednicaObjekat.Id_sednice;
            return this.IzvrsiAzuriranje(upit);
        }

        public bool ObrisiSednicu(int id_sednice)
        {
            string upit = "DELETE FROM sednica WHERE id_sednice = " + id_sednice;
            return this.IzvrsiAzuriranje(upit);
        }

        public int DajNajnovijuSednicuId()
        {
            string upit = "SELECT TOP 1 id_sednice FROM sednica ORDER BY id_sednice DESC";
            DataSet rezultat = this.DajPodatke(upit);
            if (rezultat.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_sednice"]);
            }
            return 0;
        }

        public int DajNajnovijuSednicuPoZasedanju(int id_zasedanja)
        {
            string upit = "SELECT TOP 1 id_sednice FROM sednica WHERE zasedanje_id = " + id_zasedanja + 
                         " ORDER BY id_sednice DESC";
            DataSet rezultat = this.DajPodatke(upit);
            if (rezultat.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_sednice"]);
            }
            return 0;
        }
    }
}
