using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//
using KlaseMapiranja;

namespace PrezentacionaLogika
{
    public class SednicePregledKlasa
    {
        private SednicaServisKlasa _servis;
        private string _stringKonekcije;

        // konstruktor
        public SednicePregledKlasa()
        {
            // Koristimo default connection string iz DBUtils
            _stringKonekcije = "";
            _servis = new SednicaServisKlasa(_stringKonekcije);
        }

        public SednicePregledKlasa(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
            _servis = new SednicaServisKlasa(_stringKonekcije);
        }

        // ========================================================================
        // METODE ZA SEDNICE
        // ========================================================================

        // metoda za dobijanje svih sednica
        public List<SednicaDTO> DajSveSednice()
        {
            try
            {
                var rezultat = _servis.DajSveSednice();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<SednicaDTO>();
            }
            catch (Exception)
            {
                return new List<SednicaDTO>();
            }
        }

        // metoda za dobijanje sednica po filteru
        public List<SednicaDTO> DajSednicePoFilteru(int? sazivId, int? zasedanjeId)
        {
            try
            {
                var rezultat = _servis.DajSveSednice(sazivId, zasedanjeId);
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<SednicaDTO>();
            }
            catch (Exception)
            {
                return new List<SednicaDTO>();
            }
        }

        // metoda za pretraživanje sednica
        public List<SednicaDTO> PretraziSednice(dynamic searchCriteria)
        {
            try
            {
                // Za sada koristimo postojeće metode - možemo dodati pretraživanje kasnije
                int? sazivId = null;
                if (!string.IsNullOrEmpty(searchCriteria.SazivId))
                {
                    int.TryParse(searchCriteria.SazivId, out int parsedSazivId);
                    sazivId = parsedSazivId;
                }
                
                return DajSednicePoFilteru(sazivId, null);
            }
            catch (Exception)
            {
                return new List<SednicaDTO>();
            }
        }



        // metoda za dobijanje svih pitanja
        public List<string> DajSvaPitanja()
        {
            try
            {
                var rezultat = _servis.DajSvaPitanja();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<string>();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }





        // metoda za dobijanje trenutnog aktivnog zasedanja
        public ZasedanjeDTO DajTrenutnoZasedanje()
        {
            try
            {
                var rezultat = _servis.DajTrenutnoZasedanje();
                
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // metoda za kreiranje novog zasedanja
        public bool KreirajNovoZasedanje(string nazivZasedanja, int tipZasedanja, out string poruka)
        {
            try
            {
                var rezultat = _servis.KreirajNovoZasedanje(nazivZasedanja, tipZasedanja);
                if (rezultat.Uspesno)
                {
                    poruka = rezultat.Poruka;
                    return true;
                }
                else
                {
                    poruka = rezultat.Poruka;
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju zasedanja: {ex.Message}";
                return false;
            }
        }

        // metoda za kreiranje nove sednice sa pitanjima
        public bool KreirajNovuSednicu(int idZasedanja, string nazivSednice, DateTime datumSednice, string opisSednice, List<string> pitanja, out string poruka)
        {
            try
            {
                // Use the service method with the correct signature that matches the business logic
                var rezultat = _servis.KreirajNovuSednicu(idZasedanja, nazivSednice, datumSednice, opisSednice, pitanja);
                if (rezultat.Uspesno)
                {
                    poruka = rezultat.Poruka;
                    return true;
                }
                else
                {
                    poruka = rezultat.Poruka;
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju sednice: {ex.Message}";
                return false;
            }
        }

        // metoda za kreiranje nove sednice sa pitanjima (sa 7-dnevnim pravilom)
        public bool KreirajNovuSednicuSaPravilima(string nazivSednice, DateTime datumSednice, string opisSednice, List<string> pitanja, out string poruka)
        {
            try
            {
                var rezultat = _servis.KreirajNovuSednicuSaPravilima(nazivSednice, datumSednice, opisSednice, pitanja);
                if (rezultat.Uspesno)
                {
                    poruka = rezultat.Poruka;
                    return true;
                }
                else
                {
                    poruka = rezultat.Poruka;
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju sednice: {ex.Message}";
                return false;
            }
        }

        // metoda za kreiranje novog saziva
        public bool KreirajNoviSaziv(string naziv, DateTime pocetak, DateTime zavrsetak, string opis, out string poruka)
        {
            try
            {
                var rezultat = _servis.KreirajNoviSaziv(naziv, pocetak, zavrsetak, opis);
                if (rezultat.Uspesno)
                {
                    poruka = rezultat.Poruka;
                    return true;
                }
                else
                {
                    poruka = rezultat.Poruka;
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju saziva: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Generiše random glasove za novo pitanje
        /// </summary>
        public bool GenerisiRandomGlasove(int pitanjeId, out string poruka)
        {
            try
            {
                ServiceResult<bool> rezultat = _servis.GenerisiRandomGlasove(pitanjeId);
                
                if (rezultat.Uspesno)
                {
                    poruka = rezultat.Poruka;
                    return true;
                }
                else
                {
                    poruka = rezultat.Poruka;
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri generisanju random glasova: {ex.Message}";
                return false;
            }
        }

        // ========================================================================
        // METODE ZA SAZIVE
        // ========================================================================

        // metoda za dobijanje saziva za filter
        public List<SazivDTO> DajSaziveZaFilter()
        {
            try
            {
                var rezultat = _servis.DajSveSazive();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<SazivDTO>();
            }
            catch (Exception)
            {
                return new List<SazivDTO>();
            }
        }

        // metoda za dobijanje svih saziva
        public List<SazivDTO> DajSveSazive()
        {
            return DajSaziveZaFilter();
        }

        // metoda za pretraživanje saziva
        public List<SazivDTO> PretraziSazive(dynamic searchCriteria)
        {
            try
            {
                var sviSazivi = DajSveSazive();
                
                // Ako nema kriterijuma za pretragu, vrati sve
                if (string.IsNullOrWhiteSpace(searchCriteria.Naziv))
                {
                    return sviSazivi;
                }
                
                // Filtriraj po nazivu (case-insensitive)
                var filtriraniSazivi = sviSazivi
                    .Where(s => s.Ime?.IndexOf(searchCriteria.Naziv, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                
                return filtriraniSazivi;
            }
            catch (Exception)
            {
                return new List<SazivDTO>();
            }
        }

        // metoda za pretraživanje saziva sa naprednim filterima
        public List<SazivDTO> PretraziSaziveNapredno(string naziv = null, DateTime? pocetak = null, DateTime? kraj = null)
        {
            try
            {
                var rezultat = _servis.DajSazivePoFilterima(naziv, pocetak, kraj);
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<SazivDTO>();
            }
            catch (Exception)
            {
                return new List<SazivDTO>();
            }
        }

        // metoda za pretraživanje mandata sa filterima
        public List<MandatDTO> PretraziMandate(int? id_saziva = null, int? id_stranke = null, int? id_pozicije = null, string ime_prezime = null)
        {
            try
            {
                var rezultat = _servis.DajMandatePoFilterima(id_saziva, id_stranke, id_pozicije, ime_prezime);
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<MandatDTO>();
            }
            catch (Exception)
            {
                return new List<MandatDTO>();
            }
        }

        // metoda za pretraživanje glasanja sa filterima
        public List<GlasanjeDTO> PretraziGlasanja(int? id_pitanja = null, int? id_lica = null, int? id_sednice = null, int? id_saziva = null, string glas = null)
        {
            try
            {
                var rezultat = _servis.DajGlasanjaPoFilterima(id_pitanja, id_lica, id_sednice, id_saziva, glas);
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<GlasanjeDTO>();
            }
            catch (Exception)
            {
                return new List<GlasanjeDTO>();
            }
        }

        // metoda za dobijanje aktivnog saziva
        public SazivDTO DajAktivanSaziv()
        {
            try
            {
                // Koristi servis layer koji poziva pravu business logiku
                var rezultat = _servis.DajAktivanSaziv();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ========================================================================
        // METODE ZA ZASEDANJA
        // ========================================================================

        // metoda za dobijanje zasedanja za filter
        public List<ZasedanjeDTO> DajZasedanjaZaFilter(int? sazivId = null, string tipZasedanja = null)
        {
            try
            {
                var rezultat = _servis.DajSvaZasedanja(sazivId, tipZasedanja);
                
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<ZasedanjeDTO>();
            }
            catch (Exception)
            {
                return new List<ZasedanjeDTO>();
            }
        }

        // metoda za dobijanje svih zasedanja
        public List<ZasedanjeDTO> DajSvaZasedanja()
        {
            return DajZasedanjaZaFilter();
        }

        // metoda za pretraživanje zasedanja
        public List<ZasedanjeDTO> PretraziZasedanja(dynamic searchCriteria)
        {
            try
            {
                // Koristi novu servisnu metodu sa filterima
                int? sazivId = null;
                if (!string.IsNullOrEmpty(searchCriteria.SazivId))
                {
                    int.TryParse(searchCriteria.SazivId, out int parsedSazivId);
                    sazivId = parsedSazivId;
                }
                
                string tipZasedanja = null;
                if (!string.IsNullOrEmpty(searchCriteria.Tip) && searchCriteria.Tip != "-- Svi tipovi --")
                {
                    tipZasedanja = searchCriteria.Tip;
                }
                
                var zasedanja = DajZasedanjaZaFilter(sazivId, tipZasedanja);
                return zasedanja;
            }
            catch (Exception)
            {
                return new List<ZasedanjeDTO>();
            }
        }

        // ========================================================================
        // METODE ZA STATISTIKE
        // ========================================================================

        // statistike za zasedanja
        public dynamic DajStatistikeZasedanja()
        {
            try
            {
                var zasedanja = DajSvaZasedanja();
                
                return new
                {
                    UkupnoZasedanja = zasedanja.Count,
                    AktivnaZasedanja = zasedanja.Count(z => z.TipZasedanja?.ToLower() == "redovno"),
                    ZavrsenaZasedanja = zasedanja.Count(z => z.TipZasedanja?.ToLower() == "vanredno"),
                    UkupnoSednica = zasedanja.Sum(z => z.BrojSednica)
                };
            }
            catch (Exception)
            {
                return new
                {
                    UkupnoZasedanja = 0,
                    AktivnaZasedanja = 0,
                    ZavrsenaZasedanja = 0,
                    UkupnoSednica = 0
                };
            }
        }

        // statistike za sazive
        public dynamic DajStatistikeSaziva()
        {
            try
            {
                var sazivi = DajSveSazive();
                
                if (sazivi == null || sazivi.Count == 0)
                {
                    return new
                    {
                        UkupnoSaziva = 0,
                        AktivnihSaziva = 0,
                        ZavrsenihSaziva = 0,
                        UkupnoMandata = 0
                    };
                }
                
                return new
                {
                    UkupnoSaziva = sazivi.Count,
                    AktivnihSaziva = sazivi.Count(s => s != null && s.Aktivan),
                    ZavrsenihSaziva = sazivi.Count(s => s != null && !s.Aktivan),
                    UkupnoMandata = 0
                };
            }
            catch (Exception)
            {
                return new
                {
                    UkupnoSaziva = 0,
                    AktivnihSaziva = 0,
                    ZavrsenihSaziva = 0,
                    UkupnoMandata = 0
                };
            }
        }

        // ========================================================================
        // METODE ZA GLASANJA
        // ========================================================================

        // metoda za dobijanje svih glasanja
        public List<GlasanjeDTO> DajSvaGlasanja()
        {
            try
            {
                var rezultat = _servis.DajSvaGlasanja();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<GlasanjeDTO>();
            }
            catch (Exception)
            {
                return new List<GlasanjeDTO>();
            }
        }

        // ========================================================================
        // METODE ZA MANDATE
        // ========================================================================

        // metoda za dobijanje svih mandata
        public List<MandatDTO> DajSveMandate()
        {
            try
            {
                var rezultat = _servis.DajSveMandate();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<MandatDTO>();
            }
            catch (Exception)
            {
                return new List<MandatDTO>();
            }
        }

        // metoda za kreiranje novog mandata
        public bool KreirajNoviMandat(int liceId, string status, out string poruka)
        {
            try
            {
                var rezultat = _servis.KreirajNoviMandat(liceId, status);
                if (rezultat.Uspesno)
                {
                    poruka = rezultat.Poruka ?? "Mandat uspešno kreiran.";
                    return true;
                }
                else
                {
                    poruka = rezultat.Poruka ?? "Greška pri kreiranju mandata.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju mandata: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Dohvata sve mandate za aktivan saziv za prikaz u UI
        /// </summary>
        /// <returns>Lista mandata za aktivan saziv kao DTO</returns>
        public List<MandatDTO> DajMandateZaAktivanSaziv()
        {
            try
            {
                var rezultat = _servis.DajMandateZaAktivanSaziv();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<MandatDTO>();
            }
            catch (Exception)
            {
                return new List<MandatDTO>();
            }
        }

        /// <summary>
        /// Dohvata statistike mandata za aktivan saziv za prikaz u UI
        /// </summary>
        /// <returns>Statistike mandata za aktivan saziv</returns>
        public dynamic DajStatistikeMandataZaAktivanSaziv()
        {
            try
            {
                var rezultat = _servis.DajStatistikeMandataZaAktivanSaziv();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new
                {
                    UkupnoMandata = 0,
                    AktivnihMandata = 0,
                    Poslanika = 0,
                    Stranaka = 0
                };
            }
            catch (Exception)
            {
                return new
                {
                    UkupnoMandata = 0,
                    AktivnihMandata = 0,
                    Poslanika = 0,
                    Stranaka = 0
                };
            }
        }

        // ========================================================================
        // METODE ZA LICA
        // ========================================================================

        // metoda za dobijanje svih lica
        public List<LiceDTO> DajSvaLica()
        {
            try
            {
                var rezultat = _servis.DajSvaLica();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<LiceDTO>();
            }
            catch (Exception)
            {
                return new List<LiceDTO>();
            }
        }

        // metoda za kreiranje novog lica (bez mandata)
        public bool KreirajNovoLice(string ime, string prezime, string korisnickoIme, string lozinka, 
                                   int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, 
                                   string biografija, out string poruka)
        {
            try
            {
                var rezultat = _servis.KreirajNovoLice(ime, prezime, korisnickoIme, lozinka, pozicijaId, strankaId, pol, datumRodjenja, biografija);
                if (rezultat.Uspesno)
                {
                    poruka = rezultat.Poruka ?? "Lice uspešno kreirano.";
                    return true;
                }
                else
                {
                    poruka = rezultat.Poruka ?? "Greška pri kreiranju lica.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju lica: {ex.Message}";
                return false;
            }
        }

        // metoda za kreiranje novog lica i mandata zajedno
        public bool KreirajNovoLiceIMandat(string ime, string prezime, string korisnickoIme, string lozinka, 
                                          int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, 
                                          string biografija, out string poruka)
        {
            try
            {
                var rezultat = _servis.KreirajNovoLiceIMandat(ime, prezime, korisnickoIme, lozinka, pozicijaId, strankaId, pol, datumRodjenja, biografija);
                if (rezultat.Uspesno)
                {
                    poruka = rezultat.Poruka ?? "Lice i mandat uspešno kreirani.";
                    return true;
                }
                else
                {
                    poruka = rezultat.Poruka ?? "Greška pri kreiranju lica i mandata.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju lica i mandata: {ex.Message}";
                return false;
            }
        }

        // ========================================================================
        // METODE ZA VALIDACIJU SISTEMA
        // ========================================================================

        /// <summary>
        /// Validira da li je sistem spreman za rad
        /// </summary>
        public bool DaLiJeSistemSpremanZaRad(out string poruka)
        {
            try
            {
                var rezultat = _servis.DaLiJeSistemSpremanZaRad();
                poruka = rezultat.Poruka ?? "Sistem je spreman za rad.";
                return rezultat.Uspesno && rezultat.Podaci;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri proveri sistema: {ex.Message}";
                return false;
            }
        }

        // ========================================================================
        // METODE ZA STRANKE
        // ========================================================================

        /// <summary>
        /// Dohvata sve stranke za prikaz u UI
        /// </summary>
        /// <returns>Lista svih stranaka kao DTO</returns>
        public List<StrankaDTO> DajSveStranke()
        {
            try
            {
                var rezultat = _servis.DajSveStranke();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<StrankaDTO>();
            }
            catch (Exception)
            {
                return new List<StrankaDTO>();
            }
        }

        // ========================================================================
        // METODE ZA POZICIJE
        // ========================================================================

        /// <summary>
        /// Dohvata sve pozicije za prikaz u UI
        /// </summary>
        /// <returns>Lista svih pozicija kao DTO</returns>
        public List<PozicijaDTO> DajSvePozicije()
        {
            try
            {
                var rezultat = _servis.DajSvePozicije();
                if (rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return rezultat.Podaci;
                }
                return new List<PozicijaDTO>();
            }
            catch (Exception)
            {
                return new List<PozicijaDTO>();
            }
        }

        // ========================================================================
        // POMOĆNE METODE
        // ========================================================================
    }
}
