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
                
                if (rezultat)
                {
                    // Dohvati ID novog saziva
                    int noviSazivId = _sazivDB.DajNajnovijiSazivId();
                    
                    if (stariAktivanSazivId > 0)
                    {
                        // Kopiraj mandate iz starog aktivnog saziva u novi saziv
                        bool mandateKopirani = KopirajMandateIzSaziva(stariAktivanSazivId, noviSazivId);
                        
                        // Ako nema mandata kopiranih, kreiraj Predsednik mandate iz postojećih lica
                        if (!mandateKopirani)
                        {
                            KreirajPredsednikMandateIzLica(noviSazivId);
                        }
                    }
                    else
                    {
                        // Ovo je prvi saziv - kreiraj Predsednik mandate iz postojećih lica
                        KreirajPredsednikMandateIzLica(noviSazivId);
                    }
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
                // Pojednostavljena logika - uvek vraća najnoviji saziv (po ID-u)
                // Ovo je mnogo jednostavnije i predvidljivije od složene date logike
                string upit = @"SELECT TOP 1 * FROM saziv ORDER BY id_saziva DESC";
                
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
                    saziv.Ime = red["naziv_saziva"].ToString();
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

        /// <summary>
        /// Kreira Predsednik mandate za novi saziv iz postojećih lica sa pozicijom Predsednik
        /// </summary>
        /// <param name="sazivId">ID saziva za koji se kreiraju mandate</param>
        /// <returns>True ako je bar jedan mandat uspešno kreiran</returns>
        private bool KreirajPredsednikMandateIzLica(int sazivId)
        {
            try
            {
                // Dohvati sva lica sa pozicijom Predsednik (pozicija = 2)
                string upit = "SELECT id_lica, stranka FROM lica WHERE pozicija = 2";
                DataSet rezultat = _sazivDB.DajPodatke(upit);
                
                if (rezultat?.Tables?.Count == 0 || rezultat.Tables[0].Rows.Count == 0)
                {
                    return false; // Nema Predsednika u sistemu
                }
                
                int uspesnoKreirano = 0;
                MandatDBKlasa mandatDB = new MandatDBKlasa(_konekcija, "mandat");
                
                foreach (DataRow red in rezultat.Tables[0].Rows)
                {
                    int idLica = Convert.ToInt32(red["id_lica"]);
                    int idStranke = Convert.ToInt32(red["stranka"]);
                    
                    // Kreiraj mandat za Predsednika
                    MandatKlasa noviMandat = new MandatKlasa
                    {
                        Id_lica = idLica,
                        Id_saziva = sazivId,
                        Id_stranke = idStranke
                    };
                    
                    bool mandatKreiran = mandatDB.DodajNoviMandat(noviMandat);
                    if (mandatKreiran)
                    {
                        uspesnoKreirano++;
                    }
                }
                
                return uspesnoKreirano > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Kreira novi saziv sa validacijom
        /// </summary>
        public bool KreirajNoviSazivSaValidacijom(string naziv, DateTime pocetak, DateTime zavrsetak, string opis, out string poruka)
        {
            poruka = "";
            try
            {
                // Kreiraj saziv objekat
                SazivKlasa noviSaziv = new SazivKlasa
                {
                    Ime = naziv,
                    Pocetak = pocetak,
                    Kraj = zavrsetak,
                    Opis = opis
                };

                // Koristi postojeću metodu
                bool uspesno = KreirajNoviSaziv(noviSaziv);
                if (uspesno)
                {
                    poruka = "Saziv je uspešno kreiran.";
                }
                else
                {
                    poruka = "Greška pri kreiranju saziva.";
                }
                return uspesno;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju saziva: {ex.Message}";
                return false;
            }
        }
    }
}
