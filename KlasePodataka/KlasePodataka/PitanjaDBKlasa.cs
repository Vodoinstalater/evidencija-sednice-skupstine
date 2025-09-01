using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class PitanjaDBKlasa : TabelaKlasa
    {
        public PitanjaDBKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele) : base(novaKonekcija, noviNazivTabele)
        {
            // nesto drugo u vezi specificno ove klase
        }

        public DataSet DajSvaPitanja()
        {
            return this.DajPodatke("SELECT * FROM pitanja ORDER BY id_dnevni_red, redni_broj");
        }

        public DataSet DajPitanjaPoId(int id_pitanja)
        {
            string upit = "SELECT * FROM pitanja WHERE id_pitanja = " + id_pitanja;
            return this.DajPodatke(upit);
        }

        public DataSet DajPitanjaPoDnevnomRedu(int id_dnevni_red)
        {
            string upit = "SELECT * FROM pitanja WHERE id_dnevni_red = " + id_dnevni_red + " ORDER BY redni_broj";
            return this.DajPodatke(upit);
        }

        public DataSet DajPitanjaPoRednomBroju(int redni_broj)
        {
            string upit = "SELECT * FROM pitanja WHERE redni_broj = " + redni_broj + " ORDER BY id_dnevni_red";
            return this.DajPodatke(upit);
        }

        public bool DodajNovoPitanja(PitanjaKlasa novoPitanjaObjekat)
        {
            try
            {
                // Generate new ID
                int noviId = DajNajnovijePitanjaId() + 1;
                
                string upit = "INSERT INTO pitanja (id_pitanja, id_dnevni_red, redni_broj, tekst) VALUES (" + 
                             noviId + ", " + 
                             novoPitanjaObjekat.Id_dnevni_red + ", " + 
                             novoPitanjaObjekat.Redni_broj + ", '" + 
                             novoPitanjaObjekat.Tekst + "')";
                
                bool rezultat = this.IzvrsiAzuriranje(upit);
                return rezultat;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool IzmeniPitanja(PitanjaKlasa pitanjaObjekat)
        {
            string upit = "UPDATE pitanja SET id_dnevni_red = " + pitanjaObjekat.Id_dnevni_red + 
                         ", redni_broj = " + pitanjaObjekat.Redni_broj + 
                         ", tekst = '" + pitanjaObjekat.Tekst + 
                         "' WHERE id_pitanja = " + pitanjaObjekat.Id_pitanja;
            return this.IzvrsiAzuriranje(upit);
        }

        public bool ObrisiPitanja(int id_pitanja)
        {
            string upit = "DELETE FROM pitanja WHERE id_pitanja = " + id_pitanja;
            return this.IzvrsiAzuriranje(upit);
        }

        public int DajNajnovijePitanjaId()
        {
            string upit = "SELECT TOP 1 id_pitanja FROM pitanja ORDER BY id_pitanja DESC";
            DataSet rezultat = this.DajPodatke(upit);
            if (rezultat.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_pitanja"]);
            }
            return 0;
        }

        public int DajNajnovijePitanjaPoDnevnomRedu(int id_dnevni_red)
        {
            string upit = "SELECT TOP 1 id_pitanja FROM pitanja WHERE id_dnevni_red = " + id_dnevni_red + 
                         " ORDER BY id_pitanja DESC";
            DataSet rezultat = this.DajPodatke(upit);
            if (rezultat.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_pitanja"]);
            }
            return 0;
        }

        public int DajSledeciRedniBroj(int id_dnevni_red)
        {
            string upit = "SELECT ISNULL(MAX(redni_broj), 0) + 1 as sledeci_broj FROM pitanja WHERE id_dnevni_red = " + id_dnevni_red;
            DataSet rezultat = this.DajPodatke(upit);
            return Convert.ToInt32(rezultat.Tables[0].Rows[0]["sledeci_broj"]);
        }
    }
}
