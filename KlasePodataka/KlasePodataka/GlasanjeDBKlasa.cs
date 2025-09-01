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
    public class GlasanjeDBKlasa : TabelaKlasa
    {
        public GlasanjeDBKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele) : base(novaKonekcija, noviNazivTabele)
        {
            // nesto drugo u vezi specificno ove klase
        }

        public DataSet DajSvaGlasanja()
        {
            return this.DajPodatke("SELECT * FROM glasanje ORDER BY id_pitanja, id_lica");
        }

        public DataSet DajGlasanjePoId(int id_glasanja)
        {
            string upit = "SELECT * FROM glasanje WHERE id_glasanja = " + id_glasanja;
            return this.DajPodatke(upit);
        }

        public DataSet DajGlasanjePoPitanju(int id_pitanja)
        {
            string upit = "SELECT * FROM glasanje WHERE id_pitanja = " + id_pitanja + " ORDER BY id_lica";
            return this.DajPodatke(upit);
        }

        public DataSet DajGlasanjePoLici(int id_lica)
        {
            string upit = "SELECT * FROM glasanje WHERE id_lica = " + id_lica + " ORDER BY id_pitanja";
            return this.DajPodatke(upit);
        }

        public DataSet DajSveGlasoveSaDetaljima()
        {
            // PRIVREMENO: Koristi jednostavan SELECT umesto stored procedure
            try
            {
                string simpleQuery = @"
                    SELECT 
                        g.id_glasanja,
                        g.glas,
                        l.ime + ' ' + l.prezime AS ime_prezime,
                        p.id_pitanja,
                        p.redni_broj,
                        p.tekst AS pitanje,
                        s.id_sednice,
                        s.naziv AS sednica_Naziv,
                        s.datum AS sednica_datum,
                        z.naziv_zasedanja,
                        sa.ime AS saziv_ime
                    FROM glasanje g
                    INNER JOIN lica l ON g.id_lica = l.id_lica
                    INNER JOIN pitanja p ON g.id_pitanja = p.id_pitanja
                    INNER JOIN dnevni_red dr ON p.id_dnevni_red = dr.id_dnevni_red
                    INNER JOIN sednica s ON dr.id_sednice = s.id_sednice
                    INNER JOIN zasedanje z ON s.zasedanje_id = z.id_zasedanja
                    INNER JOIN saziv sa ON z.id_saziv = sa.id_saziva
                    ORDER BY s.datum DESC, p.redni_broj, l.prezime, l.ime";
                
                return this.DajPodatke(simpleQuery);
            }
            catch
            {
                return new DataSet();
            }
        }

        public DataSet DajGlasovePoFilterima(int? id_pitanja = null, int? id_lica = null, int? id_sednice = null, int? id_saziva = null, string glas = null)
        {
            try
            {
                string upit = @"
                    SELECT 
                        g.id_glasanja,
                        g.glas,
                        l.ime + ' ' + l.prezime AS ime_prezime,
                        p.id_pitanja,
                        p.redni_broj,
                        p.tekst AS pitanje,
                        s.id_sednice,
                        s.naziv AS sednica_Naziv,
                        s.datum AS sednica_datum,
                        z.naziv_zasedanja,
                        sa.ime AS saziv_ime
                    FROM glasanje g
                    INNER JOIN lica l ON g.id_lica = l.id_lica
                    INNER JOIN pitanja p ON g.id_pitanja = p.id_pitanja
                    INNER JOIN dnevni_red dr ON p.id_dnevni_red = dr.id_dnevni_red
                    INNER JOIN sednica s ON dr.id_sednice = s.id_sednice
                    INNER JOIN zasedanje z ON s.zasedanje_id = z.id_zasedanja
                    INNER JOIN saziv sa ON z.id_saziv = sa.id_saziva
                    WHERE 1=1";
                
                if (id_pitanja.HasValue)
                {
                    upit += $" AND g.id_pitanja = {id_pitanja.Value}";
                }
                
                if (id_lica.HasValue)
                {
                    upit += $" AND g.id_lica = {id_lica.Value}";
                }
                
                if (id_sednice.HasValue)
                {
                    upit += $" AND s.id_sednice = {id_sednice.Value}";
                }
                
                if (id_saziva.HasValue)
                {
                    upit += $" AND sa.id_saziva = {id_saziva.Value}";
                }
                
                if (!string.IsNullOrEmpty(glas))
                {
                    upit += $" AND g.glas = '{glas}'";
                }
                
                upit += " ORDER BY s.datum DESC, p.redni_broj, l.prezime, l.ime";
                
                return this.DajPodatke(upit);
            }
            catch
            {
                return new DataSet();
            }
        }

        public bool DodajNovoGlasanje(GlasanjeKlasa novoGlasanjeObjekat)
        {
            try
            {
                // Generate new ID
                int noviId = DajNajnovijeGlasanjeId() + 1;
                
                string upit = "INSERT INTO glasanje (id_glasanja, id_pitanja, id_lica, glas) VALUES (" + 
                             noviId + ", " + 
                             novoGlasanjeObjekat.IdPitanja + ", " + 
                             novoGlasanjeObjekat.IdLica + ", '" + 
                             novoGlasanjeObjekat.Glas + "')";
                
                bool rezultat = this.IzvrsiAzuriranje(upit);
                return rezultat;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int DajNajnovijeGlasanjeId()
        {
            string upit = "SELECT TOP 1 id_glasanja FROM glasanje ORDER BY id_glasanja DESC";
            DataSet rezultat = this.DajPodatke(upit);
            if (rezultat.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_glasanja"]);
            }
            return 0;
        }

        public bool IzmeniGlasanje(int id_glasanja, string noviGlas)
        {
            string upit = "UPDATE glasanje SET glas = '" + noviGlas + 
                         "' WHERE id_glasanja = " + id_glasanja;
            return this.IzvrsiAzuriranje(upit);
        }

        public bool ObrisiGlasanje(int id_glasanja)
        {
            string upit = "DELETE FROM glasanje WHERE id_glasanja = " + id_glasanja;
            return this.IzvrsiAzuriranje(upit);
        }
    }
}
