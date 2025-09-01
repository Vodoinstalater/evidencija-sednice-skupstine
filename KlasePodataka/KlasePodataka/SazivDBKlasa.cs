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
            return this.DajPodatke("SELECT * FROM saziv ORDER BY pocetak DESC");
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
            string upit = "SELECT * FROM saziv WHERE 1=1";
            
            if (!string.IsNullOrEmpty(naziv))
            {
                upit += $" AND ime LIKE '%{naziv}%'";
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

        public bool PostojiAktivanSaziv()
        {
            DateTime danas = DateTime.Today;
            // Koristi istu logiku kao DajAktivanSaziv - prioritizuje saziv sa najnovijim pocetkom
            string upit = @"SELECT COUNT(*) as broj FROM saziv 
                           WHERE pocetak <= '" + danas.ToString("yyyy-MM-dd") + 
                         "' AND kraj >= '" + danas.ToString("yyyy-MM-dd") + "'";
            DataSet rezultat = this.DajPodatke(upit);
            return Convert.ToInt32(rezultat.Tables[0].Rows[0]["broj"]) > 0;
        }
    }
}
