using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class UpravljanjeSazivimaKlasa
    {
        private SazivDBKlasa _sazivDB;
        private KonekcijaKlasa _konekcija;

        public UpravljanjeSazivimaKlasa(KonekcijaKlasa konekcija)
        {
            _konekcija = konekcija;
            _sazivDB = new SazivDBKlasa(konekcija, "saziv");
        }

        public bool KreirajNoviSaziv(SazivKlasa noviSaziv)
        {
            try
            {
                // Proveri da li već postoji aktivan saziv
                bool postojiAktivan = _sazivDB.PostojiAktivanSaziv();
                
                int stariAktivanSazivId = 0;
                if (postojiAktivan)
                {
                    // Dohvati ID starog aktivnog saziva pre nego što ga zatvorimo
                    SazivKlasa stariAktivanSaziv = DajAktivanSaziv();
                    if (stariAktivanSaziv != null)
                    {
                        stariAktivanSazivId = stariAktivanSaziv.Id_saziva;
                        
                        // Zatvori specifično ovaj saziv (postavi kraj na juče da bude sigurno zatvoren)
                        DateTime juce = DateTime.Today.AddDays(-1);
                        string upitZatvori = $"UPDATE saziv SET kraj = '{juce:yyyy-MM-dd}' WHERE id_saziva = {stariAktivanSazivId}";
                        bool zatvoren = _sazivDB.IzvrsiAzuriranje(upitZatvori);
                        
                        if (!zatvoren)
                        {
                            // Greška pri zatvaranju saziva
                        }
                    }
                }

                // Kreiraj novi saziv
                bool rezultat = _sazivDB.DodajNoviSaziv(noviSaziv);
                
                if (rezultat && stariAktivanSazivId > 0)
                {
                    // Dohvati ID novog saziva
                    int noviSazivId = _sazivDB.DajNajnovijiSazivId();
                    
                    // Kopiraj mandate iz starog aktivnog saziva u novi saziv
                    bool mandateKopirani = KopirajMandateIzSaziva(stariAktivanSazivId, noviSazivId);
                }
                
                return rezultat;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ZatvoriAktivanSaziv()
        {
            try
            {
                // Dohvati aktivan saziv
                SazivKlasa aktivanSaziv = DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    return false;
                }
                
                // Zatvori specifično ovaj saziv (postavi kraj na juče da bude sigurno zatvoren)
                DateTime juce = DateTime.Today.AddDays(-1);
                string upit = $"UPDATE saziv SET kraj = '{juce:yyyy-MM-dd}' WHERE id_saziva = {aktivanSaziv.Id_saziva}";
                bool rezultat = _sazivDB.IzvrsiAzuriranje(upit);
                
                return rezultat;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public SazivKlasa DajAktivanSaziv()
        {
            try
            {
                DateTime danas = DateTime.Today;
                // Izmenjen upit da prioritizuje saziv sa najnovijim pocetkom
                // Ovo rešava problem kada stari saziv ima kraj = danas, a novi saziv ima pocetak = danas
                string upit = @"SELECT TOP 1 * FROM saziv 
                               WHERE pocetak <= '" + danas.ToString("yyyy-MM-dd") + 
                             "' AND kraj >= '" + danas.ToString("yyyy-MM-dd") + 
                             "' ORDER BY pocetak DESC";
                
                DataSet rezultat = _sazivDB.DajPodatke(upit);
                
                if (rezultat?.Tables?.Count > 0 && rezultat.Tables[0].Rows.Count > 0)
                {
                    DataRow red = rezultat.Tables[0].Rows[0];
                    SazivKlasa saziv = new SazivKlasa();
                    saziv.Id_saziva = Convert.ToInt32(red["id_saziva"]);
                    saziv.Ime = red["ime"].ToString();
                    saziv.Pocetak = Convert.ToDateTime(red["pocetak"]);
                    saziv.Kraj = Convert.ToDateTime(red["kraj"]);
                    saziv.Opis = red["opis"].ToString();
                    
                    return saziv;
                }
                
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<SazivKlasa> DajSveSazive()
        {
            List<SazivKlasa> listaSaziva = new List<SazivKlasa>();
            try
            {
                DataSet rezultat = _sazivDB.DajSveSazive();
                foreach (DataRow red in rezultat.Tables[0].Rows)
                {
                    SazivKlasa saziv = new SazivKlasa();
                    saziv.Id_saziva = Convert.ToInt32(red["id_saziva"]);
                    saziv.Ime = red["ime"].ToString();
                    saziv.Pocetak = Convert.ToDateTime(red["pocetak"]);
                    saziv.Kraj = Convert.ToDateTime(red["kraj"]);
                    saziv.Opis = red["opis"].ToString();
                    listaSaziva.Add(saziv);
                }
            }
            catch
            {
                // U slučaju greške, vrati praznu listu
            }
            return listaSaziva;
        }

        /// <summary>
        /// Kopira samo Predsednika iz jednog saziva u drugi
        /// </summary>
        /// <param name="izSazivaId">ID saziva iz kog se kopiraju mandate</param>
        /// <param name="uSazivId">ID saziva u koji se kopiraju mandate</param>
        /// <returns>True ako je kopiranje uspešno</returns>
        private bool KopirajMandateIzSaziva(int izSazivaId, int uSazivId)
        {
            try
            {
                // Dohvati sve mandate iz starog saziva sa informacijama o poziciji
                string upit = $@"
                    SELECT m.id_lica, m.id_stranke, l.pozicija 
                    FROM mandat m 
                    INNER JOIN lica l ON m.id_lica = l.id_lica 
                    WHERE m.id_saziva = {izSazivaId}";
                
                DataSet stariMandati = _sazivDB.DajPodatke(upit);
                
                if (stariMandati?.Tables?.Count == 0 || stariMandati.Tables[0].Rows.Count == 0)
                {
                    return true; // Nema mandata za kopiranje, ali to nije greška
                }
                
                int uspesnoKopirano = 0;
                foreach (DataRow row in stariMandati.Tables[0].Rows)
                {
                    int idLica = Convert.ToInt32(row["id_lica"]);
                    int idStranke = Convert.ToInt32(row["id_stranke"]);
                    int pozicija = Convert.ToInt32(row["pozicija"]);
                    
                    // Kopiraj samo Predsednika (pozicija = 2)
                    if (pozicija == 2)
                    {
                        // Kreiraj novi mandat u novom sazivu
                        MandatKlasa noviMandat = new MandatKlasa
                        {
                            Id_lica = idLica,
                            Id_saziva = uSazivId,
                            Id_stranke = idStranke
                        };
                        
                        MandatDBKlasa mandatDB = new MandatDBKlasa(_konekcija, "mandat");
                        bool mandatKopiran = mandatDB.DodajNoviMandat(noviMandat);
                        if (mandatKopiran)
                        {
                            uspesnoKopirano++;
                        }
                    }
                }
                
                return uspesnoKopirano > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
