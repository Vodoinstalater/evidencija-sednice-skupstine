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

        public DataSet DajSednicuPoId(int id_sednice)
        {
            string upit = "SELECT * FROM sednica WHERE id_sednice = " + id_sednice;
            return this.DajPodatke(upit);
        }


        public DataSet DajSednicePoSazivuIZasedanju(int? id_saziva = null, int? id_zasedanja = null)
        {
            try
            {
                // Koristi stored procedure sp_sveSednice sa parametrima
                string spCall = "EXEC sp_sveSednice";
                
                if (id_saziva.HasValue && id_zasedanja.HasValue)
                {
                    spCall += $" @id_saziva = {id_saziva.Value}, @id_zasedanja = {id_zasedanja.Value}";
                }
                else if (id_saziva.HasValue)
                {
                    spCall += $" @id_saziva = {id_saziva.Value}";
                }
                else if (id_zasedanja.HasValue)
                {
                    spCall += $" @id_zasedanja = {id_zasedanja.Value}";
                }
                
                DataSet rezultat = this.DajPodatke(spCall);
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
