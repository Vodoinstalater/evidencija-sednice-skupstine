using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class DnevniRedDBKlasa : TabelaKlasa
    {
        public DnevniRedDBKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele) : base(novaKonekcija, noviNazivTabele)
        {
            // nesto drugo u vezi specificno ove klase
        }



        public DataSet DajDnevniRedPoId(int id_dnevni_red)
        {
            string upit = "SELECT * FROM dnevni_red WHERE id_dnevni_red = " + id_dnevni_red;
            return this.DajPodatke(upit);
        }

        public DataSet DajDnevniRedPoSednici(int id_sednice)
        {
            string upit = "SELECT * FROM dnevni_red WHERE id_sednice = " + id_sednice;
            return this.DajPodatke(upit);
        }

        public bool DodajNoviDnevniRed(DnevniRedKlasa noviDnevniRedObjekat)
        {
            try
            {
                // Generate new ID
                int noviId = DajNajnovijiDnevniRedId() + 1;
                
                string upit = "INSERT INTO dnevni_red (id_dnevni_red, id_sednice) VALUES (" + 
                             noviId + ", " + 
                             noviDnevniRedObjekat.Id_sednice + ")";
                
                bool rezultat = this.IzvrsiAzuriranje(upit);
                return rezultat;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool IzmeniDnevniRed(DnevniRedKlasa dnevniRedObjekat)
        {
            string upit = "UPDATE dnevni_red SET id_sednice = " + dnevniRedObjekat.Id_sednice + 
                         " WHERE id_dnevni_red = " + dnevniRedObjekat.Id_dnevni_red;
            return this.IzvrsiAzuriranje(upit);
        }

        public bool ObrisiDnevniRed(int id_dnevni_red)
        {
            string upit = "DELETE FROM dnevni_red WHERE id_dnevni_red = " + id_dnevni_red;
            return this.IzvrsiAzuriranje(upit);
        }

        public int DajNajnovijiDnevniRedId()
        {
            string upit = "SELECT TOP 1 id_dnevni_red FROM dnevni_red ORDER BY id_dnevni_red DESC";
            DataSet rezultat = this.DajPodatke(upit);
            if (rezultat.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_dnevni_red"]);
            }
            return 0;
        }

        public int DajNajnovijiDnevniRedPoSednici(int id_sednice)
        {
            string upit = "SELECT TOP 1 id_dnevni_red FROM dnevni_red WHERE id_sednice = " + id_sednice + 
                         " ORDER BY id_dnevni_red DESC";
            DataSet rezultat = this.DajPodatke(upit);
            if (rezultat.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_dnevni_red"]);
            }
            return 0;
        }
    }
}
