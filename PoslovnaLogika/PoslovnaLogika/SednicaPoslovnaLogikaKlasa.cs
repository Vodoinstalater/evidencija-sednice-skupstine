using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//
using KlasePodataka;
using DBUtils;

namespace PoslovnaLogika
{
    /// <summary>
    /// Poslovna logika klasa za aplikaciju "Evidencija sednica skupstine Srbije"
    /// SADRŽI SAMO VALIDACIJU I ODLUČIVANJE - NEMA MANIPULACIJU PODACIMA
    /// </summary>
    public class SednicaPoslovnaLogikaKlasa
    {
        // atributi
        private string _stringKonekcije;
        private readonly ValidacijaPoslovnihPravilaKlasa _validacija;
        private readonly BusinessRulesConfigKlasa _config;

        // konstruktor
        public SednicaPoslovnaLogikaKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
            _validacija = new ValidacijaPoslovnihPravilaKlasa();
            _config = BusinessRulesConfigKlasa.Instance;
        }

        // ========================================================================
        // POMOĆNE METODE ZA KONEKCIJU
        // ========================================================================

        /// <summary>
        /// Kreira konekciju na bazu podataka - koristi default ako je string prazan
        /// </summary>
        private KonekcijaKlasa KreirajKonekciju()
        {
            // Ako nema globalne konekcije, kreiraj novu
            if (string.IsNullOrWhiteSpace(_stringKonekcije))
            {
                KonekcijaKlasa konekcija = new KonekcijaKlasa(); // Koristi default: sednica3 na DESKTOP-VANE1TI\SQLEXPRESS
                
                // OTVORI konekciju pre nego što je vratiš
                bool uspeh = konekcija.OtvoriKonekciju();
                
                if (!uspeh)
                {
                    throw new InvalidOperationException("Ne mogu da otvorim konekciju na bazu podataka 'sednica3' na serveru 'DESKTOP-VANE1TI\\SQLEXPRESS'. Proverite da li je SQL Server pokrenut i da li baza podataka postoji.");
                }
                
                return konekcija;
            }
            else
            {
                // Koristi prosleđenu konekciju
                KonekcijaKlasa konekcija = new KonekcijaKlasa(_stringKonekcije);
                bool uspeh = konekcija.OtvoriKonekciju();
                
                if (!uspeh)
                {
                    throw new InvalidOperationException($"Ne mogu da otvorim konekciju na bazu podataka sa prosleđenim stringom: {_stringKonekcije}");
                }
                
                return konekcija;
            }
        }

        // ========================================================================
        // METODE ZA DOHVATANJE PODATAKA ZA ODLUČIVANJE (NE MANIPULACIJA)
        // ========================================================================

        /// <summary>
        /// Dohvata sve sednice - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public DataSet DajSveSednice(int? sazivId = null, int? zasedanjeId = null)
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                SednicaDBKlasa sednicaDB = new SednicaDBKlasa(konekcija, "sednica");
                
                // Uvek koristi stored procedure sp_sveSednice
                return sednicaDB.DajSednicePoSazivuIZasedanju(sazivId, zasedanjeId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju sednica: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata sva zasedanja - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public DataSet DajSvaZasedanja(int? sazivId = null, string tipZasedanja = null)
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                ZasedanjeDBKlasa zasedanjeDB = new ZasedanjeDBKlasa(konekcija, "zasedanje");
                return zasedanjeDB.DajZasedanjaPoSazivuITipu(sazivId, tipZasedanja);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju zasedanja: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata sve sazive - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public List<SazivKlasa> DajSveSazive()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(konekcija);
                return upravljanjeSazivima.DajSveSazive();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju saziva: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata sazive po filterima - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public DataSet DajSazivePoFilterima(string naziv = null, DateTime? pocetak = null, DateTime? kraj = null)
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                SazivDBKlasa sazivDB = new SazivDBKlasa(konekcija, "saziv");
                return sazivDB.DajSazivePoFilterima(naziv, pocetak, kraj);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju saziva po filterima: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata mandate po filterima - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public DataSet DajMandatePoFilterima(int? id_saziva = null, int? id_stranke = null, int? id_pozicije = null, string ime_prezime = null)
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                MandatDBKlasa mandatDB = new MandatDBKlasa(konekcija, "mandat");
                return mandatDB.DajMandatePoFilterima(id_saziva, id_stranke, id_pozicije, ime_prezime);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju mandata po filterima: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata istoriju glasanja po filterima - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public DataSet DajIstorijuGlasanjaPoFilterima(int? id_pitanja = null, int? id_lica = null, int? id_sednice = null, int? id_saziva = null, string glas = null)
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                GlasanjeDBKlasa glasanjeDB = new GlasanjeDBKlasa(konekcija, "glasanje");
                return glasanjeDB.DajGlasovePoFilterima(id_pitanja, id_lica, id_sednice, id_saziva, glas);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju istorije glasanja: {ex.Message}");
            }
        }

        // ========================================================================
        // VALIDACIJA POSLOVNIH PRAVILA - SAMO ODLUČIVANJE
        // ========================================================================

        /// <summary>
        /// Validira da li se može kreirati novi saziv - SAMO VALIDACIJA
        /// </summary>
        public bool DaLiSeMozeKreiratiSaziv(string naziv, DateTime pocetak, DateTime zavrsetak, string opis, out string poruka)
        {
            poruka = "";
            
            // Validacija naziva
            if (!_validacija.DaLiJeNazivValidan(naziv, "saziv", out string nazivPoruka))
            {
                poruka = nazivPoruka;
                return false;
            }

            // Validacija datuma
            if (!_validacija.DaLiSuDatumiSazivaValidni(pocetak, zavrsetak, out string datumPoruka))
            {
                poruka = datumPoruka;
                return false;
            }

            // Validacija obaveznih polja
            var polja = new Dictionary<string, object>
            {
                {"naziv", naziv},
                {"opis", opis}
            };
            if (!_validacija.DaLiSuObaveznaPoljaPopunjena(polja, out string poljaPoruka))
            {
                poruka = poljaPoruka;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validira da li se može kreirati novo zasedanje - SAMO VALIDACIJA
        /// </summary>
        public bool DaLiSeMozeKreiratiZasedanje(string naziv, DateTime datum, string opis, int tipId, out string poruka)
        {
            poruka = "";
            
            // Validacija naziva
            if (!_validacija.DaLiJeNazivValidan(naziv, "zasedanje", out string nazivPoruka))
            {
                poruka = nazivPoruka;
                return false;
            }

            // Validacija datuma
            if (!_validacija.DaLiJeDatumZasedanjaValidan(datum, out string datumPoruka))
            {
                poruka = datumPoruka;
                return false;
            }

            // Validacija pozicije
            if (!_validacija.DaLiJePozicijaValidna(tipId, out string pozicijaPoruka))
            {
                poruka = pozicijaPoruka;
                return false;
            }

            // Validacija obaveznih polja
            var polja = new Dictionary<string, object>
            {
                {"naziv", naziv},
                {"opis", opis}
            };
            if (!_validacija.DaLiSuObaveznaPoljaPopunjena(polja, out string poljaPoruka))
            {
                poruka = poljaPoruka;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validira da li se može kreirati nova sednica - SAMO VALIDACIJA
        /// </summary>
        public bool DaLiSeMozeKreiratiSednica(string nazivSednice, DateTime datumSednice, string opisSednice, List<string> pitanja, out string poruka)
        {
            poruka = "";
            
            // Validacija naziva
            if (!_validacija.DaLiJeNazivValidan(nazivSednice, "sednice", out string nazivPoruka))
            {
                poruka = nazivPoruka;
                return false;
            }

            // Validacija datuma
            if (!_validacija.DaLiJeDatumValidan(datumSednice, out string datumPoruka))
            {
                poruka = datumPoruka;
                return false;
            }

            // GLAVNO POSLOVNO PRAVILO: Ako je sednica manje od konfigurisanih dana unapred, mora postojati objašnjenje
            if (!_validacija.DaLiJeSednicaBlagovremenoZakazana(datumSednice, opisSednice, out string sednicaPoruka))
            {
                poruka = sednicaPoruka;
                return false;
            }

            // Validacija pitanja
            if (!_validacija.DaLiImaPitanjaZaDnevniRed(pitanja, out string pitanjaPoruka))
            {
                poruka = pitanjaPoruka;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validira da li se može kreirati novo lice i mandat - SAMO VALIDACIJA
        /// </summary>
        public bool DaLiSeMozeKreiratiLiceIMandat(string ime, string prezime, string korisnickoIme, string lozinka, 
            int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, string bio, out string poruka)
        {
            poruka = "";
            
            // Validacija imena
            if (!_validacija.DaLiJeNazivValidan(ime, "ime", out string imePoruka))
            {
                poruka = imePoruka;
                return false;
            }

            if (!_validacija.DaLiJeNazivValidan(prezime, "prezime", out string prezimePoruka))
            {
                poruka = prezimePoruka;
                return false;
            }

            // Validacija lozinke
            if (!_validacija.DaLiJeLozinkaValidna(lozinka, out string lozinkaPoruka))
            {
                poruka = lozinkaPoruka;
                return false;
            }

            // Validacija starosti
            if (!_validacija.DaLiJeLicePunoljetno(datumRodjenja, out string starostPoruka))
            {
                poruka = starostPoruka;
                return false;
            }

            // Validacija pozicije
            if (!_validacija.DaLiJePozicijaValidna(pozicijaId, out string pozicijaPoruka))
            {
                poruka = pozicijaPoruka;
                return false;
            }

            // Validacija obaveznih polja
            var polja = new Dictionary<string, object>
            {
                {"ime", ime},
                {"prezime", prezime},
                {"korisnicko_ime", korisnickoIme},
                {"lozinka", lozinka},
                {"bio", bio}
            };
            if (!_validacija.DaLiSuObaveznaPoljaPopunjena(polja, out string poljaPoruka))
            {
                poruka = poljaPoruka;
                return false;
            }

            return true;
        }

        // ========================================================================
        /// <summary>
        /// Dohvata aktivan saziv - SAMO ZA ODLUČIVANJE
        /// Pojednostavljena logika - uvek vraća najnoviji saziv kao "aktivan"
        /// </summary>
        public SazivKlasa DajAktivanSaziv()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(konekcija);
                
                // Pojednostavljena logika - uvek koristi najnoviji saziv kao "aktivan"
                // Ovo je mnogo jednostavnije i predvidljivije od složene date logike
                List<SazivKlasa> sviSazivi = upravljanjeSazivima.DajSveSazive();
                if (sviSazivi != null && sviSazivi.Count > 0)
                {
                    return sviSazivi.OrderByDescending(s => s.Id_saziva).FirstOrDefault();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju aktivnog saziva: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata trenutno zasedanje po sazivu - SAMO ZA ODLUČIVANJE
        /// </summary>
        public DataSet DajTrenutnoZasedanjePoSazivu(int id_saziva)
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                ZasedanjeDBKlasa zasedanjeDB = new ZasedanjeDBKlasa(konekcija, "zasedanje");
                return zasedanjeDB.DajTrenutnoZasedanjePoSazivu(id_saziva);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju trenutnog zasedanja: {ex.Message}");
            }
        }

        /// <summary>
        /// Broji zasedanja za određeni saziv - SAMO ZA ODLUČIVANJE
        /// </summary>
        public int BrojZasedanjaZaSaziv(int sazivId)
        {
            try
            {
                DataSet zasedanja = DajSvaZasedanja(sazivId, null);
                return zasedanja?.Tables?.Count > 0 ? zasedanja.Tables[0].Rows.Count : 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri brojanju zasedanja za saziv {sazivId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Broji sednice za određeni saziv - SAMO ZA ODLUČIVANJE
        /// </summary>
        public int BrojSednicaZaSaziv(int sazivId)
        {
            try
            {
                DataSet sednice = DajSveSednice(sazivId, null);
                return sednice?.Tables?.Count > 0 ? sednice.Tables[0].Rows.Count : 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri brojanju sednica za saziv {sazivId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Broji mandate za aktivan saziv - SAMO ZA ODLUČIVANJE
        /// </summary>
        public int BrojMandataZaAktivanSaziv()
        {
            try
            {
                List<MandatKlasa> mandate = DajMandateZaAktivanSaziv();
                return mandate?.Count ?? 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri brojanju mandata za aktivan saziv: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata sve mandate za pregled - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public DataSet DajSveMandateZaPregled()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                MandatDBKlasa mandatDB = new MandatDBKlasa(konekcija, "mandat");
                return mandatDB.DajSveMandate();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju mandata: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata istoriju glasanja - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public DataSet DajIstorijuGlasanja()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                GlasanjeDBKlasa glasanjeDB = new GlasanjeDBKlasa(konekcija, "glasanje");
                return glasanjeDB.DajSveGlasoveSaDetaljima();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju istorije glasanja: {ex.Message}");
            }
        }

        /// <summary>
        /// Proverava korisnika za prijavu - SAMO VALIDACIJA
        /// </summary>
        public bool ProveriKorisnika(string korisnickoIme, string lozinka, out string poruka)
        {
            poruka = "";
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                LicaDBKlasa licaDB = new LicaDBKlasa(konekcija, "lica");
                
                DataSet korisnik = licaDB.DajLicaPoKorisnickomImenu(korisnickoIme);
                
                if (korisnik.Tables[0].Rows.Count == 0)
                {
                    poruka = "Korisnik sa tim korisničkim imenom ne postoji.";
                    return false;
                }
                
                string pravaLozinka = korisnik.Tables[0].Rows[0]["lozinka"].ToString();
                if (pravaLozinka != lozinka)
                {
                    poruka = "Pogrešna lozinka.";
                    return false;
                }
                
                poruka = "Uspešna prijava.";
                return true;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri proveri korisnika: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Dohvata podatke korisnika - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public DataSet DajPodatkeKorisnika(string korisnickoIme, out string poruka)
        {
            poruka = "";
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                LicaDBKlasa licaDB = new LicaDBKlasa(konekcija, "lica");
                
                DataSet korisnik = licaDB.DajLicaPoKorisnickomImenu(korisnickoIme);
                
                if (korisnik.Tables[0].Rows.Count == 0)
                {
                    poruka = "Korisnik ne postoji.";
                    return null;
                }
                
                poruka = "Podaci korisnika uspešno dohvaćeni.";
                return korisnik;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri dohvatanju podataka korisnika: {ex.Message}";
                return null;
            }
        }

        /// <summary>
        /// Generiše random glasove za pitanje - SAMO ZA TESTIRANJE
        /// </summary>
        public bool GenerisiRandomGlasove(int pitanjeId, out string poruka)
        {
            poruka = "";
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeSednicamaKlasa upravljanjeSednicama = new UpravljanjeSednicamaKlasa(konekcija);
                
                bool rezultat = upravljanjeSednicama.GenerisiRandomGlasove(pitanjeId);
                
                if (rezultat)
                {
                    poruka = "Random glasovi su uspešno generisani.";
                }
                else
                {
                    poruka = "Greška pri generisanju random glasova.";
                }
                
                return rezultat;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri generisanju random glasova: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Dohvata sva pitanja - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public DataSet DajSvaPitanja()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                PitanjaDBKlasa pitanjaDB = new PitanjaDBKlasa(konekcija, "pitanja");
                return pitanjaDB.DajSvaPitanja();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju pitanja: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata trenutno zasedanje - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public DataSet DajTrenutnoZasedanje()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                ZasedanjeDBKlasa zasedanjeDB = new ZasedanjeDBKlasa(konekcija, "zasedanje");
                return zasedanjeDB.DajTrenutnoZasedanje();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju trenutnog zasedanja: {ex.Message}");
            }
        }

        /// <summary>
        /// Kreira novu sednicu - DELEGIRA NA LAYER 1
        /// </summary>
        public bool KreirajNovuSednicu(int idZasedanja, string nazivSednice, DateTime datumSednice, string opisSednice, List<string> pitanja, out string poruka)
        {
            poruka = "";
            try
            {
                // SAMO VALIDACIJA - NEMA MANIPULACIJE PODATAKA
                if (!DaLiSeMozeKreiratiSednica(nazivSednice, datumSednice, opisSednice, pitanja, out string validacijaPoruka))
                {
                    poruka = validacijaPoruka;
                    return false;
                }

                // Ako je validacija prošla, vrati true - Layer 1 će obaviti kreiranje
                poruka = "Validacija uspešna. Sednica može biti kreirana.";
                return true;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri validaciji sednice: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Dohvata sva glasanja - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public DataSet DajSvaGlasanja()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                GlasanjeDBKlasa glasanjeDB = new GlasanjeDBKlasa(konekcija, "glasanje");
                return glasanjeDB.DajSveGlasoveSaDetaljima();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju glasanja: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata sva lica - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public List<LicaKlasa> DajSvaLica()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                LicaDBKlasa licaDB = new LicaDBKlasa(konekcija, "lica");
                return licaDB.DajSvaLica();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju lica: {ex.Message}");
            }
        }

        /// <summary>
        /// Kreira samo novo lice (bez mandata) - DELEGIRA NA LAYER 1
        /// </summary>
        public bool KreirajNovoLice(string ime, string prezime, string korisnickoIme, string lozinka, int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, string bio, out string poruka)
        {
            poruka = "";
            try
            {
                // SAMO VALIDACIJA - NEMA MANIPULACIJE PODATAKA
                if (!DaLiSeMozeKreiratiLiceIMandat(ime, prezime, korisnickoIme, lozinka, pozicijaId, strankaId, pol, datumRodjenja, bio, out string validacijaPoruka))
                {
                    poruka = validacijaPoruka;
                    return false;
                }

                // Ako je validacija prošla, vrati true - Layer 1 će obaviti kreiranje
                poruka = "Validacija uspešna. Lice može biti kreirano.";
                return true;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri validaciji lica: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Kreira novo lice i mandat zajedno - DELEGIRA NA LAYER 1
        /// </summary>
        public bool KreirajNovoLiceIMandat(string ime, string prezime, string korisnickoIme, string lozinka, int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, string bio, out string poruka)
        {
            poruka = "";
            try
            {
                // SAMO VALIDACIJA - NEMA MANIPULACIJE PODATAKA
                if (!DaLiSeMozeKreiratiLiceIMandat(ime, prezime, korisnickoIme, lozinka, pozicijaId, strankaId, pol, datumRodjenja, bio, out string validacijaPoruka))
                {
                    poruka = validacijaPoruka;
                    return false;
                }

                // Ako je validacija prošla, vrati true - Layer 1 će obaviti kreiranje
                poruka = "Validacija uspešna. Lice i mandat mogu biti kreirani.";
                return true;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri validaciji lica i mandata: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Dohvata sve stranke - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public List<StrankaKlasa> DajSveStranke()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                MandatDBKlasa mandatDB = new MandatDBKlasa(konekcija, "mandat");
                return mandatDB.DajSveStranke();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju stranaka: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata mandate za aktivan saziv - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public List<MandatKlasa> DajMandateZaAktivanSaziv()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeMandatimaKlasa upravljanjeMandatima = new UpravljanjeMandatimaKlasa(konekcija);
                
                SazivKlasa aktivanSaziv = DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    return new List<MandatKlasa>();
                }
                
                return upravljanjeMandatima.DajMandatePoSazivuLista(aktivanSaziv.Id_saziva);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju mandata za aktivan saziv: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata sve pozicije - SAMO ZA PRIKAZ, NEMA MANIPULACIJU
        /// </summary>
        public List<PozicijaKlasa> DajSvePozicije()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                MandatDBKlasa mandatDB = new MandatDBKlasa(konekcija, "mandat");
                return mandatDB.DajSvePozicije();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Greška pri dohvatanju pozicija: {ex.Message}");
            }
        }

        /// <summary>
        /// Validira da li je sistem spreman za rad - POSLOVNA LOGIKA
        /// </summary>
        public bool DaLiJeSistemSpremanZaRad(out string poruka)
        {
            poruka = "";
            try
            {
                // Proveri da li postoji aktivan saziv
                SazivKlasa aktivanSaziv = DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    poruka = "Ne postoji aktivan saziv. Prvo kreirajte novi saziv.";
                    return false;
                }

                // Proveri da li postoje stranke
                List<StrankaKlasa> stranke = DajSveStranke();
                if (stranke == null || stranke.Count == 0)
                {
                    poruka = "Nema stranaka u sistemu. Dodajte stranke pre dodavanja lica.";
                    return false;
                }

                // Proveri da li postoje pozicije
                List<PozicijaKlasa> pozicije = DajSvePozicije();
                if (pozicije == null || pozicije.Count == 0)
                {
                    poruka = "Nema pozicija u sistemu. Proverite konfiguraciju pozicija.";
                    return false;
                }

                poruka = "Sistem je spreman za rad.";
                return true;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri proveri sistema: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Kreira novu sednicu - DELEGIRA NA LAYER 1 (overload)
        /// </summary>
        public bool KreirajNovuSednicu(string naziv, DateTime datum, string opis, List<string> pitanja, string objasnjenje, out string poruka)
        {
            poruka = "";
            try
            {
                // SAMO VALIDACIJA - NEMA MANIPULACIJE PODATAKA
                if (!DaLiSeMozeKreiratiSednica(naziv, datum, opis, pitanja, out string validacijaPoruka))
                {
                    poruka = validacijaPoruka;
                    return false;
                }

                // Ako je validacija prošla, vrati true - Layer 1 će obaviti kreiranje
                poruka = "Validacija uspešna. Sednica može biti kreirana.";
                return true;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri validaciji sednice: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Kreira novi mandat - DELEGIRA NA LAYER 1
        /// </summary>
        public bool KreirajNoviMandat(int liceId, string status, out string poruka)
        {
            poruka = "";
            try
            {
                // Za sada jednostavna implementacija - status se ignoriše
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeMandatimaKlasa upravljanjeMandatima = new UpravljanjeMandatimaKlasa(konekcija);
                
                // Dohvati aktivan saziv
                SazivKlasa aktivanSaziv = DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    poruka = "Nema aktivnog saziva.";
                    return false;
                }
                
                // Za sada koristi default stranku (ID = 1)
                bool rezultat = upravljanjeMandatima.DodajMandat(liceId, aktivanSaziv.Id_saziva, 1);
                
                if (rezultat)
                {
                    poruka = "Mandat je uspešno kreiran.";
                }
                else
                {
                    poruka = "Greška pri kreiranju mandata.";
                }
                
                return rezultat;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju mandata: {ex.Message}";
                return false;
            }
        }
    }
}