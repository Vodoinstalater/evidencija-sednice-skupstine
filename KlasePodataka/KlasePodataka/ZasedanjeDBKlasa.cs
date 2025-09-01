using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class ZasedanjeDBKlasa : TabelaKlasa
    {
        public ZasedanjeDBKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele) : base(novaKonekcija, noviNazivTabele)
        {
            // nesto drugo u vezi specificno ove klase
        }

        public DataSet DajSvaZasedanja()
        {
            string upit = @"
                SELECT 
                    z.id_zasedanja,
                    z.naziv_zasedanja,
                    ISNULL(t.tip_zasedanja, CAST(z.tip AS VARCHAR(50))) AS tip,
                    z.id_saziv,
                    s.ime AS saziv_ime
                FROM zasedanje z
                LEFT JOIN saziv s ON z.id_saziv = s.id_saziva
                LEFT JOIN tipovi t ON z.tip = t.id
                ORDER BY z.id_saziv, z.id_zasedanja";
            
            return this.DajPodatke(upit);
        }

        public DataSet DajZasedanjaPoSazivu(int id_saziva)
        {
            string upit = @"
                SELECT 
                    z.id_zasedanja,
                    z.naziv_zasedanja,
                    ISNULL(t.tip_zasedanja, CAST(z.tip AS VARCHAR(50))) AS tip,
                    z.id_saziv,
                    s.ime AS saziv_ime
                FROM zasedanje z
                LEFT JOIN saziv s ON z.id_saziv = s.id_saziva
                LEFT JOIN tipovi t ON z.tip = t.id
                WHERE z.id_saziv = " + id_saziva + @"
                ORDER BY z.id_zasedanja";
            
            return this.DajPodatke(upit);
        }

        public DataSet DajZasedanjePoId(int id_zasedanja)
        {
            string upit = "SELECT * FROM zasedanje WHERE id_zasedanja = " + id_zasedanja;
            return this.DajPodatke(upit);
        }

        public DataSet DajZasedanjaPoSazivuITipu(int? id_saziva = null, string tip_zasedanja = null)
        {
            string upit = @"
                SELECT 
                    z.id_zasedanja,
                    z.naziv_zasedanja,
                    ISNULL(t.tip_zasedanja, CAST(z.tip AS VARCHAR(50))) AS tip,
                    z.id_saziv,
                    s.ime AS saziv_ime
                FROM zasedanje z
                LEFT JOIN saziv s ON z.id_saziv = s.id_saziva
                LEFT JOIN tipovi t ON z.tip = t.id
                WHERE 1=1";
            
            if (id_saziva.HasValue)
            {
                upit += $" AND z.id_saziv = {id_saziva.Value}";
            }
            
            if (!string.IsNullOrEmpty(tip_zasedanja))
            {
                upit += $" AND t.tip_zasedanja = '{tip_zasedanja}'";
            }
            
            upit += " ORDER BY z.id_saziv, z.id_zasedanja";
            
            return this.DajPodatke(upit);
        }

        public bool DodajNovoZasedanje(ZasedanjeKlasa novoZasedanjeObjekat)
        {
            try
            {
                // Generate new ID
                int noviId = DajNajnovijeZasedanjeId() + 1;
                
                string upit = "INSERT INTO zasedanje (id_zasedanja, tip, naziv_zasedanja, id_saziv) VALUES (" + 
                             noviId + ", " + 
                             novoZasedanjeObjekat.Tip + ", '" + 
                             novoZasedanjeObjekat.Naziv_zasedanja + "', " + 
                             novoZasedanjeObjekat.Id_saziv + ")";
                
                bool rezultat = this.IzvrsiAzuriranje(upit);
                
                return rezultat;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IzmeniZasedanje(ZasedanjeKlasa zasedanjeObjekat)
        {
            string upit = "UPDATE zasedanje SET tip = " + zasedanjeObjekat.Tip + 
                         ", naziv_zasedanja = '" + zasedanjeObjekat.Naziv_zasedanja + 
                         "', id_saziv = " + zasedanjeObjekat.Id_saziv + 
                         " WHERE id_zasedanja = " + zasedanjeObjekat.Id_zasedanja;
            return this.IzvrsiAzuriranje(upit);
        }

        public bool ObrisiZasedanje(int id_zasedanja)
        {
            string upit = "DELETE FROM zasedanje WHERE id_zasedanja = " + id_zasedanja;
            return this.IzvrsiAzuriranje(upit);
        }

        public int DajNajnovijeZasedanjeId()
        {
            string upit = "SELECT TOP 1 id_zasedanja FROM zasedanje ORDER BY id_zasedanja DESC";
            DataSet rezultat = this.DajPodatke(upit);
            if (rezultat.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_zasedanja"]);
            }
            return 0;
        }

        public int DajNajnovijeZasedanjePoSazivu(int id_saziva)
        {
            string upit = "SELECT TOP 1 id_zasedanja FROM zasedanje WHERE id_saziv = " + id_saziva + 
                         " ORDER BY id_zasedanja DESC";
            DataSet rezultat = this.DajPodatke(upit);
            if (rezultat.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_zasedanja"]);
            }
            return 0;
        }



        /// <summary>
        /// Dohvata trenutno aktivno zasedanje za aktivan saziv
        /// </summary>
        public DataSet DajTrenutnoZasedanje()
        {
            try
            {
                string upit = @"
                    SELECT TOP 1
                        z.id_zasedanja,
                        z.naziv_zasedanja,
                        ISNULL(t.tip_zasedanja, CAST(z.tip AS VARCHAR(50))) AS tip,
                        z.id_saziv,
                        s.ime AS saziv_ime,
                        s.pocetak AS saziv_pocetak,
                        s.kraj AS saziv_kraj
                    FROM zasedanje z
                    LEFT JOIN saziv s ON z.id_saziv = s.id_saziva
                    LEFT JOIN tipovi t ON z.tip = t.id
                    WHERE s.kraj > GETDATE() -- Samo aktivni sazivi
                    ORDER BY z.id_zasedanja DESC";
                
                DataSet rezultat = this.DajPodatke(upit);
                
                return rezultat;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Dohvata trenutno aktivno zasedanje za određeni saziv
        /// </summary>
        public DataSet DajTrenutnoZasedanjePoSazivu(int id_saziva)
        {
            string upit = @"
                SELECT TOP 1
                    z.id_zasedanja,
                    z.naziv_zasedanja,
                    ISNULL(t.tip_zasedanja, CAST(z.tip AS VARCHAR(50))) AS tip,
                    z.id_saziv,
                    s.ime AS saziv_ime,
                    s.pocetak AS saziv_pocetak,
                    s.kraj AS saziv_kraj
                FROM zasedanje z
                LEFT JOIN saziv s ON z.id_saziv = s.id_saziva
                LEFT JOIN tipovi t ON z.tip = t.id
                WHERE z.id_saziv = " + id_saziva + @"
                ORDER BY z.id_zasedanja DESC";
            
            return this.DajPodatke(upit);
        }
    }
}
