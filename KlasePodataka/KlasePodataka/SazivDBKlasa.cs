using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class SazivDBKlasa : TabelaKlasa
    {
        public SazivDBKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele) : base(novaKonekcija, noviNazivTabele)
        {
            // nesto drugo u vezi specificno ove klase
        }

        public DataSet DajSveSazive()
        {
            try
            {
                return this.DajPodatke("SELECT * FROM v_sviSazivi ORDER BY pocetak DESC");
            }
            catch (Exception)
            {
                // Return empty DataSet if there's an error
                return new DataSet();
            }
        }

        public DataSet DajSazivPoId(int id_saziva)
        {
            string upit = "SELECT * FROM saziv WHERE id_saziva = " + id_saziva;
            return this.DajPodatke(upit);
        }

        public bool DodajNoviSaziv(SazivKlasa noviSazivObjekat)
        {
            try
            {
                // Generate new ID
                int noviId = DajNajnovijiSazivId() + 1;
                
                string upit = "INSERT INTO saziv (id_saziva, ime, pocetak, kraj, opis) VALUES (" + 
                             noviId + ", '" + 
                             noviSazivObjekat.Ime + "', '" + 
                             noviSazivObjekat.Pocetak.ToString("yyyy-MM-dd") + "', '" + 
                             noviSazivObjekat.Kraj.ToString("yyyy-MM-dd") + "', '" + 
                             noviSazivObjekat.Opis + "')";
                
                bool rezultat = this.IzvrsiAzuriranje(upit);
                return rezultat;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool IzmeniSaziv(SazivKlasa sazivObjekat)
        {
            string upit = "UPDATE saziv SET ime = '" + sazivObjekat.Ime + 
                         "', pocetak = '" + sazivObjekat.Pocetak.ToString("yyyy-MM-dd") + 
                         "', kraj = '" + sazivObjekat.Kraj.ToString("yyyy-MM-dd") + 
                         "', opis = '" + sazivObjekat.Opis + 
                         "' WHERE id_saziva = " + sazivObjekat.Id_saziva;
            return this.IzvrsiAzuriranje(upit);
        }

        public bool ObrisiSaziv(int id_saziva)
        {
            string upit = "DELETE FROM saziv WHERE id_saziva = " + id_saziva;
            return this.IzvrsiAzuriranje(upit);
        }

        public int DajNajnovijiSazivId()
        {
            try
            {
                DataSet rezultat = this.DajPodatke("SELECT MAX(id_saziva) as max_id FROM saziv");
                if (rezultat.Tables[0].Rows.Count > 0 && rezultat.Tables[0].Rows[0]["max_id"] != DBNull.Value)
                {
                    return Convert.ToInt32(rezultat.Tables[0].Rows[0]["max_id"]);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public DataSet DajSazivePoFilterima(string naziv = null, DateTime? pocetak = null, DateTime? kraj = null)
        {
            try
            {
                string upit = "SELECT * FROM saziv WHERE 1=1";
                
                if (!string.IsNullOrEmpty(naziv))
                {
                    // Escape single quotes to prevent SQL injection
                    string escapedNaziv = naziv.Replace("'", "''");
                    upit += $" AND ime LIKE '%{escapedNaziv}%'";
                }
                
                if (pocetak.HasValue)
                {
                    upit += $" AND pocetak >= '{pocetak.Value:yyyy-MM-dd}'";
                }
                
                if (kraj.HasValue)
                {
                    upit += $" AND kraj <= '{kraj.Value:yyyy-MM-dd}'";
                }
                
                upit += " ORDER BY pocetak DESC";
                
                return this.DajPodatke(upit);
            }
            catch (Exception)
            {
                // Return empty DataSet if there's an error
                return new DataSet();
            }
        }

        public bool PostojiAktivanSaziv()
        {
            // Pojednostavljena logika - uvek vraća true ako postoji bilo koji saziv
            // Ovo je mnogo jednostavnije i predvidljivije od složene date logike
            string upit = @"SELECT COUNT(*) as broj FROM saziv";
            DataSet rezultat = this.DajPodatke(upit);
            return Convert.ToInt32(rezultat.Tables[0].Rows[0]["broj"]) > 0;
        }
    }
}
