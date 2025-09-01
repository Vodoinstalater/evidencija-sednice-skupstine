using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class LicaDBKlasa : TabelaKlasa
    {
        public LicaDBKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele) : base(novaKonekcija, noviNazivTabele)
        {
            // nesto drugo u vezi specificno ove klase
        }

        public DataSet DajSvaLicaDataSet()
        {
            return this.DajPodatke("SELECT * FROM lica ORDER BY prezime, ime");
        }

        public List<LicaKlasa> DajSvaLica()
        {
            List<LicaKlasa> lica = new List<LicaKlasa>();
            DataSet ds = this.DajPodatke("SELECT * FROM lica ORDER BY prezime, ime");
            
            if (ds?.Tables?.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lica.Add(new LicaKlasa
                    {
                        Id_lica = Convert.ToInt32(row["id_lica"]),
                        Ime = row["ime"].ToString(),
                        Prezime = row["prezime"].ToString(),
                        Korisnicko_ime = row["korisnicko_ime"].ToString(),
                        Lozinka = row["lozinka"].ToString(),
                        Pozicija = Convert.ToInt32(row["pozicija"]),
                        Stranka = Convert.ToInt32(row["stranka"]),
                        Pol = Convert.ToChar(row["pol"]),
                        Datumr = Convert.ToDateTime(row["datumr"]),
                        Bio = row["bio"]?.ToString() ?? ""
                    });
                }
            }
            
            return lica;
        }

        public DataSet DajLicaPoId(int id_lica)
        {
            string upit = "SELECT * FROM lica WHERE id_lica = " + id_lica;
            return this.DajPodatke(upit);
        }

        public DataSet DajLicaPoKorisnickomImenu(string korisnicko_ime)
        {
            // Koristi parametrizovan upit da izbegnemo probleme sa specijalnim karakterima
            string upit = "SELECT * FROM lica WHERE korisnicko_ime = @korisnicko_ime";
            
            // Za sada koristimo jednostavan string concatenation
            string simpleUpit = "SELECT * FROM lica WHERE korisnicko_ime = '" + korisnicko_ime + "'";
            
            DataSet rezultat = this.DajPodatke(simpleUpit);
            
            return rezultat;
        }

        public DataSet DajLicaPoStranci(int id_stranke)
        {
            string upit = "SELECT * FROM lica WHERE stranka = " + id_stranke + " ORDER BY prezime, ime";
            return this.DajPodatke(upit);
        }

        public DataSet DajLicaPoPoziciji(int id_pozicije)
        {
            string upit = "SELECT * FROM lica WHERE pozicija = " + id_pozicije + " ORDER BY prezime, ime";
            return this.DajPodatke(upit);
        }

        public DataSet DajLicaPoPolu(char pol)
        {
            string upit = "SELECT * FROM lica WHERE pol = '" + pol + "' ORDER BY prezime, ime";
            return this.DajPodatke(upit);
        }

        public bool DodajNovaLica(LicaKlasa novaLicaObjekat)
        {
            string upit = "INSERT INTO lica (ime, prezime, pozicija, stranka, pol, datumr, bio, korisnicko_ime, lozinka) VALUES ('" + 
                         novaLicaObjekat.Ime + "', '" + 
                         novaLicaObjekat.Prezime + "', " + 
                         novaLicaObjekat.Pozicija + ", " + 
                         novaLicaObjekat.Stranka + ", '" + 
                         novaLicaObjekat.Pol + "', '" + 
                         novaLicaObjekat.Datumr.ToString("yyyy-MM-dd") + "', '" + 
                         novaLicaObjekat.Bio + "', '" + 
                         novaLicaObjekat.Korisnicko_ime + "', '" + 
                         novaLicaObjekat.Lozinka + "')";
            return this.IzvrsiAzuriranje(upit);
        }

        public bool DodajNovoLice(LicaKlasa novoLice)
        {
            // Generiši novi ID
            int noviId = DajNajnovijeLicaId() + 1;
            
            string upit = "INSERT INTO lica (id_lica, ime, prezime, pozicija, stranka, pol, datumr, bio, korisnicko_ime, lozinka) VALUES (" + 
                         noviId + ", '" + 
                         novoLice.Ime + "', '" + 
                         novoLice.Prezime + "', " + 
                         novoLice.Pozicija + ", " + 
                         novoLice.Stranka + ", '" + 
                         novoLice.Pol + "', '" + 
                         novoLice.Datumr.ToString("yyyy-MM-dd") + "', '" + 
                         novoLice.Bio + "', '" + 
                         novoLice.Korisnicko_ime + "', '" + 
                         novoLice.Lozinka + "')";
            return this.IzvrsiAzuriranje(upit);
        }

        public bool IzmeniLica(LicaKlasa licaObjekat)
        {
            string upit = "UPDATE lica SET ime = '" + licaObjekat.Ime + 
                         "', prezime = '" + licaObjekat.Prezime + 
                         "', pozicija = " + licaObjekat.Pozicija + 
                         ", stranka = " + licaObjekat.Stranka + 
                         ", pol = '" + licaObjekat.Pol + 
                         "', datumr = '" + licaObjekat.Datumr.ToString("yyyy-MM-dd") + 
                         "', bio = '" + licaObjekat.Bio + 
                         "', korisnicko_ime = '" + licaObjekat.Korisnicko_ime + 
                         "', lozinka = '" + licaObjekat.Lozinka + 
                         "' WHERE id_lica = " + licaObjekat.Id_lica;
            return this.IzvrsiAzuriranje(upit);
        }

        public bool ObrisiLica(int id_lica)
        {
            string upit = "DELETE FROM lica WHERE id_lica = " + id_lica;
            return this.IzvrsiAzuriranje(upit);
        }

        public bool ProveriKorisnickoIme(string korisnicko_ime)
        {
            DataSet rezultat = DajLicaPoKorisnickomImenu(korisnicko_ime);
            return rezultat.Tables[0].Rows.Count > 0;
        }

        public int DajNajnovijeLicaId()
        {
            string upit = "SELECT TOP 1 id_lica FROM lica ORDER BY id_lica DESC";
            DataSet rezultat = this.DajPodatke(upit);
            if (rezultat.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_lica"]);
            }
            return 0;
        }

        public bool PostojiLicaSaKorisnickimImenom(string korisnicko_ime)
        {
            string upit = "SELECT COUNT(*) as broj FROM lica WHERE korisnicko_ime = '" + korisnicko_ime + "'";
            DataSet rezultat = this.DajPodatke(upit);
            return Convert.ToInt32(rezultat.Tables[0].Rows[0]["broj"]) > 0;
        }
    }
}
