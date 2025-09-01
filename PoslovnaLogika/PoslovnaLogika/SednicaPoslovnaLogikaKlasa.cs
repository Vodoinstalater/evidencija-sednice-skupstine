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
    /// 
    /// Poslovna logika klasa za aplikaciju "Evidencija sednica skupstine Srbije"
    ///
    public class SednicaPoslovnaLogikaKlasa
    {
        // atributi
        private string _stringKonekcije;

        // konstruktor
        public SednicaPoslovnaLogikaKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // ========================================================================
        // POMOĆNE METODE ZA KONEKCIJU
        // ========================================================================

        /// 
        /// Kreira konekciju na bazu podataka - koristi default ako je string prazan
        ///
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
                    throw new InvalidOperationException("Ne mogu da otvorim konekciju na bazu podataka.");
                }
                
                return konekcija;
            }
            else
            {
                KonekcijaKlasa konekcija = new KonekcijaKlasa(_stringKonekcije);
                
                // OTVORI konekciju pre nego što je vratiš
                bool uspeh = konekcija.OtvoriKonekciju();
                
                if (!uspeh)
                {
                    throw new InvalidOperationException("Ne mogu da otvorim custom konekciju na bazu podataka.");
                }
                
                return konekcija;
            }
        }

        // ========================================================================
        // POSLOVNA PRAVILA ZA VIEW OPERACIJE (LAYER 4)
        // ========================================================================

        /// 
        /// Dohvata sve sednice sa mogućim filterima za View Sednice
        /// 
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
                throw new Exception($"Greška pri dohvatanju sednica: {ex.Message}");
            }
        }

        /// 
        /// Dohvata sva zasedanja sa mogućim filterom po sazivu za View Zasedanja
        /// 
        public DataSet DajSvaZasedanja(int? sazivId = null, string tipZasedanja = null)
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                ZasedanjeDBKlasa zasedanjeDB = new ZasedanjeDBKlasa(konekcija, "zasedanje");
                
                // Koristi novu metodu sa filterima
                return zasedanjeDB.DajZasedanjaPoSazivuITipu(sazivId, tipZasedanja);
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri dohvatanju zasedanja: {ex.Message}");
            }
        }

        ///
        /// Dohvata sve sazive za View Saziv
        /// 
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
                throw new Exception($"Greška pri dohvatanju saziva: {ex.Message}");
            }
        }

        /// 
        /// Dohvata sazive sa filterima za View Saziv
        /// 
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
                throw new Exception($"Greška pri dohvatanju saziva sa filterima: {ex.Message}");
            }
        }

        /// 
        /// Dohvata sve mandate sa detaljima za View Mandati
        /// 
        public DataSet DajSveMandate()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                MandatDBKlasa mandatDB = new MandatDBKlasa(konekcija, "mandat");
                return mandatDB.DajSveMandate(); // Koristi v_sviMandati view
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri dohvatanju mandata: {ex.Message}");
            }
        }

        /// 
        /// Dohvata mandate sa filterima za View Mandati
        /// 
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
                throw new Exception($"Greška pri dohvatanju mandata sa filterima: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata istoriju glasanja za View Istorija Glasanja
        /// </summary>
        public DataSet DajIstorijuGlasanja()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                GlasanjeDBKlasa glasanjeDB = new GlasanjeDBKlasa(konekcija, "glasanje");
                return glasanjeDB.DajSveGlasoveSaDetaljima(); // Koristi sp_sviGlasovi
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri dohvatanju istorije glasanja: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata istoriju glasanja sa filterima za View Istorija Glasanja
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
                throw new Exception($"Greška pri dohvatanju istorije glasanja sa filterima: {ex.Message}");
            }
        }

        // ========================================================================
        // POSLOVNA PRAVILA ZA KREIRANJE NOVIH ENTITETA
        // ========================================================================

        /// <summary>
        /// Kreira novi saziv - poslovno pravilo: postavlja se kao aktivan
        /// </summary>
        public bool KreirajNoviSaziv(string naziv, DateTime pocetak, DateTime zavrsetak, string opis, out string poruka)
        {
            poruka = "";
            
            // Validacija - osnovne provere
            if (string.IsNullOrWhiteSpace(naziv))
            {
                poruka = "Naziv saziva ne sme biti prazan.";
                return false;
            }

            if (zavrsetak <= pocetak)
            {
                poruka = "Datum završetka mora biti nakon datuma početka.";
                return false;
            }

            if (pocetak.Date < DateTime.Today)
            {
                poruka = "Datum početka ne sme biti u prošlosti.";
                return false;
            }

            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(konekcija);
                
                SazivKlasa noviSaziv = new SazivKlasa
                {
                    Ime = naziv,
                    Pocetak = pocetak,
                    Kraj = zavrsetak,
                    Opis = opis
                };

                bool uspesno = upravljanjeSazivima.KreirajNoviSaziv(noviSaziv);
                
                if (uspesno)
                {
                    poruka = "Novi saziv je uspešno kreiran i postavljen kao aktivan. Predsednik iz prethodnog saziva je automatski kopiran.";
                    return true;
                }
                else
                {
                    poruka = "Greška pri kreiranju novog saziva.";
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
        /// Kreira novo zasedanje za aktivan saziv
        /// </summary>
        public bool KreirajNovoZasedanje(string naziv, DateTime datum, string opis, int tipId, out string poruka)
        {
            poruka = "";
            
            // Validacija
            if (string.IsNullOrWhiteSpace(naziv))
            {
                poruka = "Naziv zasedanja ne sme biti prazan.";
                return false;
            }

            if (datum.Date < DateTime.Today)
            {
                poruka = "Datum zasedanja ne sme biti u prošlosti.";
                return false;
            }

            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeZasedanjimaKlasa upravljanjeZasedanjima = new UpravljanjeZasedanjimaKlasa(konekcija);
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(konekcija);

                bool uspesno = upravljanjeZasedanjima.KreirajNovoZasedanjeZaAktivanSaziv(tipId, naziv);
                if (uspesno)
                {
                    poruka = "Novo zasedanje je uspešno kreirano.";
                    return true;
                }
                else
                {
                    poruka = "Greška pri kreiranju novog zasedanja.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju zasedanja: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Kreira novu sednicu - POSLOVNO PRAVILO: ako je manje od 7 dana unapred, mora postojati objašnjenje
        /// </summary>
        public bool KreirajNovuSednicu(string naziv, DateTime datum, string opis, List<string> listaPitanja, 
            string objasnjenje, out string poruka)
        {
            poruka = "";
            
            // Validacija
            if (string.IsNullOrWhiteSpace(naziv))
            {
                poruka = "Naziv sednice ne sme biti prazan.";
                return false;
            }

            if (datum.Date < DateTime.Today)
            {
                poruka = "Datum sednice ne sme biti u prošlosti.";
                return false;
            }

            // GLAVNO POSLOVNO PRAVILO: Ako je sednica manje od 7 dana unapred, mora postojati objašnjenje
            TimeSpan razlika = datum.Date - DateTime.Today;
            if (razlika.TotalDays < 7)
            {
                if (string.IsNullOrWhiteSpace(objasnjenje))
                {
                    poruka = "Sednica ne može biti zakazana manje od 7 dana unapred bez objašnjenja razloga.";
                    return false;
                }
            }

            if (listaPitanja == null || listaPitanja.Count == 0)
            {
                poruka = "Sednica mora imati bar jedno pitanje za dnevni red.";
                return false;
            }

            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeSednicamaKlasa upravljanjeSednicama = new UpravljanjeSednicamaKlasa(konekcija);
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(konekcija);
                UpravljanjeZasedanjimaKlasa upravljanjeZasedanjima = new UpravljanjeZasedanjimaKlasa(konekcija);

                // Proveri da li postoji aktivan saziv
                SazivKlasa aktivanSaziv = upravljanjeSazivima.DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    poruka = "Ne postoji aktivan saziv. Prvo kreirajte novi saziv.";
                    return false;
                }

                // Uzmi najnovije zasedanje
                ZasedanjeKlasa najnovijeZasedanje = upravljanjeZasedanjima.DajNajnovijeZasedanje();
                if (najnovijeZasedanje == null)
                {
                    poruka = "Ne postoji nijedno zasedanje. Prvo kreirajte novo zasedanje.";
                    return false;
                }

                KlasePodataka.SednicaKlasa novaSednica = new KlasePodataka.SednicaKlasa
                {
                    Naziv = naziv,
                    Datum = datum,
                    Opis = opis,
                    Zasedanje_id = najnovijeZasedanje.Id_zasedanja
                };

                bool uspesno = upravljanjeSednicama.KreirajNovuSednicu(novaSednica, listaPitanja);
                if (uspesno)
                {
                    poruka = "Nova sednica je uspešno kreirana sa dnevnim redom i pitanjima.";
                    return true;
                }
                else
                {
                    poruka = "Greška pri kreiranju nove sednice.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju sednice: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Kreira novo lice i povezuje ga sa sazivom kroz mandat
        /// </summary>
        public bool KreirajNovoLicaIMandat(string ime, string prezime, string korisnickoIme, string lozinka, 
            int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, string bio, out string poruka)
        {
            poruka = "";
            
            // Validacija
            if (string.IsNullOrWhiteSpace(ime) || string.IsNullOrWhiteSpace(prezime) || 
                string.IsNullOrWhiteSpace(korisnickoIme) || string.IsNullOrWhiteSpace(lozinka))
            {
                poruka = "Ime, prezime, korisničko ime i lozinka moraju biti popunjeni.";
                return false;
            }

            if (lozinka.Length < 6)
            {
                poruka = "Lozinka mora imati bar 6 karaktera.";
                return false;
            }

            if (pozicijaId < 1 || pozicijaId > 6)
            {
                poruka = "Nevažeća pozicija. Pozicija mora biti između 1 i 6.";
                return false;
            }

            if (datumRodjenja.Date >= DateTime.Today.AddYears(-18))
            {
                poruka = "Lice mora biti starije od 18 godina.";
                return false;
            }

            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeMandatimaKlasa upravljanjeMandatima = new UpravljanjeMandatimaKlasa(konekcija);
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(konekcija);

                // Proveri da li postoji aktivan saziv
                SazivKlasa aktivanSaziv = upravljanjeSazivima.DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    poruka = "Ne postoji aktivan saziv. Prvo kreirajte novi saziv.";
                    return false;
                }

                LicaKlasa novoLica = new LicaKlasa
                {
                    Ime = ime,
                    Prezime = prezime,
                    Korisnicko_ime = korisnickoIme,
                    Lozinka = lozinka, // U stvarnoj aplikaciji bi se hesirala
                    Pozicija = pozicijaId,
                    Stranka = strankaId,
                    Pol = pol,
                    Datumr = datumRodjenja,
                    Bio = bio
                };

                bool uspesno = upravljanjeMandatima.DodajNovoLica(novoLica);
                if (uspesno)
                {
                    poruka = "Novo lice je uspešno dodato i povezano sa mandatom u aktivnom sazivu.";
                    return true;
                }
                else
                {
                    poruka = "Greška pri dodavanju novog lica.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri dodavanju lica: {ex.Message}";
                return false;
            }
        }

        // ========================================================================
        // POMOĆNE METODE ZA LAYER 4
        // ========================================================================

        /// <summary>
        /// Dohvata aktivan saziv
        /// </summary>
        public SazivKlasa DajAktivanSaziv()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(konekcija);
                return upravljanjeSazivima.DajAktivanSaziv();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Dohvata najnovije zasedanje (umesto aktivnog zasedanja)
        /// </summary>
        public ZasedanjeKlasa DajAktivanZasedanje()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeZasedanjimaKlasa upravljanjeZasedanjima = new UpravljanjeZasedanjimaKlasa(konekcija);
                return upravljanjeZasedanjima.DajNajnovijeZasedanje();
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// Generiše random glasove za novo pitanje na osnovu trenutnih mandata
        /// </summary>
        public bool GenerisiRandomGlasove(int pitanjeId, out string poruka)
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                
                // Dohvati aktivan saziv
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(konekcija);
                SazivKlasa aktivanSaziv = upravljanjeSazivima.DajAktivanSaziv();
                
                if (aktivanSaziv == null)
                {
                    poruka = "Nema aktivnog saziva za generisanje glasova.";
                    return false;
                }
                
                // Dohvati sve mandate za aktivan saziv
                MandatDBKlasa mandatDB = new MandatDBKlasa(konekcija, "mandat");
                DataSet dsMandati = mandatDB.DajMandatePoSazivu(aktivanSaziv.Id_saziva);
                
                if (dsMandati?.Tables?.Count == 0 || dsMandati.Tables[0].Rows.Count == 0)
                {
                    poruka = "Nema mandata za aktivan saziv.";
                    return false;
                }
                
                // Dohvati sve lica iz mandata
                List<int> listaLicaIds = new List<int>();
                foreach (DataRow row in dsMandati.Tables[0].Rows)
                {
                    int idLica = Convert.ToInt32(row["id_lica"]);
                    listaLicaIds.Add(idLica);
                }
                
                // Generiši random glasove
                Random random = new Random();
                GlasanjeDBKlasa glasanjeDB = new GlasanjeDBKlasa(konekcija, "glasanje");
                int uspesnoGenerisano = 0;
                
                foreach (int idLica in listaLicaIds)
                {
                    // Random glas: "Za", "Protiv", "Uzdržan"
                    string[] moguciGlasovi = { "Za", "Protiv", "Uzdržan" };
                    string randomGlas = moguciGlasovi[random.Next(moguciGlasovi.Length)];
                    
                    // Kreiraj glasanje
                    GlasanjeKlasa novoGlasanje = new GlasanjeKlasa
                    {
                        IdPitanja = pitanjeId,
                        IdLica = idLica,
                        Glas = randomGlas
                    };
                    
                    bool glasDodat = glasanjeDB.DodajNovoGlasanje(novoGlasanje);
                    if (glasDodat)
                    {
                        uspesnoGenerisano++;
                    }
                }
                
                poruka = $"Uspešno generisano {uspesnoGenerisano} random glasova za pitanje ID: {pitanjeId}";
                
                return uspesnoGenerisano > 0;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri generisanju random glasova: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Validira da li je datum u validnom opsegu za business pravila
        /// </summary>
        private bool ValidirajDatum(DateTime datum, out string poruka)
        {
            poruka = "";
            
            if (datum.Date < DateTime.Today)
            {
                poruka = "Datum ne sme biti u prošlosti.";
                return false;
            }

            if (datum.Year > DateTime.Today.Year + 10)
            {
                poruka = "Datum ne sme biti više od 10 godina u budućnosti.";
                return false;
            }

            return true;
        }

        // ========================================================================
        // AUTENTIFIKACIJA I AUTORIZACIJA METODE
        // ========================================================================

        /// <summary>
        /// Proverava da li su korisničko ime i lozinka validni
        /// </summary>
        public bool ProveriKorisnika(string korisnickoIme, string lozinka, out string poruka)
        {
            poruka = "";
            
            try
            {
                if (string.IsNullOrWhiteSpace(korisnickoIme) || string.IsNullOrWhiteSpace(lozinka))
                {
                    poruka = "Korisničko ime i lozinka su obavezni.";
                    return false;
                }

                // Koristi default konekciju ako je string prazan
                KonekcijaKlasa konekcija = KreirajKonekciju();
                
                LicaDBKlasa licaDB = new LicaDBKlasa(konekcija, "lica");
                
                // Proveri da li korisnik postoji i da li je lozinka ispravna
                DataSet dsKorisnik = licaDB.DajLicaPoKorisnickomImenu(korisnickoIme);
                
                if (dsKorisnik?.Tables?.Count > 0 && dsKorisnik.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsKorisnik.Tables[0].Rows[0];
                    string storedLozinka = row["lozinka"].ToString();
                    
                    // Dodatni debug za encoding
                    string cleanStoredLozinka = storedLozinka?.Trim();
                    string cleanLozinka = lozinka?.Trim();
                    
                    if (cleanStoredLozinka == cleanLozinka)
                    {
                        poruka = "Uspešna prijava.";
                        return true;
                    }
                    else
                    {
                        poruka = $"Neispravna lozinka. Očekivano: '{cleanStoredLozinka}', Uneto: '{cleanLozinka}'";
                        return false;
                    }
                }
                else
                {
                    poruka = "Korisnik sa tim korisničkim imenom ne postoji.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri proveri korisnika: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Dohvata podatke o korisniku na osnovu korisničkog imena
        /// </summary>
        public DataSet DajPodatkeKorisnika(string korisnickoIme, out string poruka)
        {
            poruka = "";
            
            try
            {
                if (string.IsNullOrWhiteSpace(korisnickoIme))
                {
                    poruka = "Korisničko ime je obavezno.";
                    return null;
                }

                // Koristi default konekciju ako je string prazan
                KonekcijaKlasa konekcija = KreirajKonekciju();
                
                LicaDBKlasa licaDB = new LicaDBKlasa(konekcija, "lica");
                
                return licaDB.DajLicaPoKorisnickomImenu(korisnickoIme);
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri dohvatanju podataka korisnika: {ex.Message}";
                return null;
            }
        }

        /// <summary>
        /// Proverava da li korisnik ima dozvolu za određenu akciju
        /// </summary>
        public bool DaLiKorisnikImaDozvolu(string korisnickoIme, string akcija, out string poruka)
        {
            poruka = "";
            
            try
            {
                if (string.IsNullOrWhiteSpace(korisnickoIme) || string.IsNullOrWhiteSpace(akcija))
                {
                    poruka = "Korisničko ime i akcija su obavezni.";
                    return false;
                }

                // Dohvati podatke o korisniku
                DataSet dsKorisnik = DajPodatkeKorisnika(korisnickoIme, out string porukaKorisnik);
                if (dsKorisnik == null)
                {
                    poruka = porukaKorisnik;
                    return false;
                }

                DataRow row = dsKorisnik.Tables[0].Rows[0];
                int pozicijaId = Convert.ToInt32(row["pozicija"]);
                
                // Proveri dozvole na osnovu pozicije
                switch (pozicijaId)
                {
                    case 2: // Predsednik - ima sve dozvole
                        poruka = "Predsednik ima sve dozvole.";
                        return true;
                        
                    case 3: // Potpredsednik - može da saziva sednice i otvara zasedanja
                        if (akcija == "SazoviNovuSednicu" || akcija == "OtvoriNovoZasedanje")
                        {
                            poruka = "Potpredsednik ima dozvolu za ovu akciju.";
                            return true;
                        }
                        break;
                        
                    case 1:
                    case 4:
                    case 5:
                    case 6: // Poslanici - mogu samo da pregledaju
                        if (akcija == "PogledajSednice" || akcija == "PogledajZasedanja" || 
                            akcija == "PogledajSazive" || akcija == "PogledajMandate" || 
                            akcija == "IstorijaGlasanja")
                        {
                            poruka = "Poslanik ima dozvolu za ovu akciju.";
                            return true;
                        }
                        break;
                }
                
                poruka = "Korisnik nema dozvolu za ovu akciju.";
                return false;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri proveri dozvole: {ex.Message}";
                return false;
            }
        }

        // ========================================================================
        // POSLOVNA PRAVILA ZA GLASANJA (LAYER 4)
        // ========================================================================

        /// <summary>
        /// Dohvata sva glasanja sa detaljima za View Istorija Glasanja
        /// </summary>
        public DataSet DajSvaGlasanja()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                GlasanjeDBKlasa glasanjeDB = new GlasanjeDBKlasa(konekcija, "glasanje");
                
                // Koristi stored procedure sp_sviGlasovi
                return glasanjeDB.DajSveGlasoveSaDetaljima();
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri dohvatanju glasanja: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata sva pitanja za View Glasanja
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
                throw new Exception($"Greška pri dohvatanju pitanja: {ex.Message}");
            }
        }





        /// <summary>
        /// Dohvata trenutno aktivno zasedanje za aktivan saziv
        /// </summary>
        public DataSet DajTrenutnoZasedanje()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                
                // Prvo dohvati aktivan saziv
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(konekcija);
                SazivKlasa aktivanSaziv = upravljanjeSazivima.DajAktivanSaziv();
                
                if (aktivanSaziv == null)
                {
                    return new DataSet(); // Vraća prazan DataSet ako nema aktivnog saziva
                }
                
                // Dohvati najnovije zasedanje za aktivan saziv
                ZasedanjeDBKlasa zasedanjeDB = new ZasedanjeDBKlasa(konekcija, "zasedanje");
                DataSet rezultat = zasedanjeDB.DajTrenutnoZasedanjePoSazivu(aktivanSaziv.Id_saziva);
                
                return rezultat;
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri dohvatanju trenutnog zasedanja: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Dohvata trenutno aktivno zasedanje za određeni saziv
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
                throw new Exception($"Greška pri dohvatanju trenutnog zasedanja za saziv: {ex.Message}");
            }
        }

        /// <summary>
        /// Kreira novo zasedanje za aktivan saziv
        /// </summary>
        public bool KreirajNovoZasedanje(string nazivZasedanja, int tipZasedanja, out string poruka)
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                
                // Prvo dohvati aktivan saziv
                var aktivanSaziv = DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    poruka = "Nema aktivnog saziva. Ne možete kreirati novo zasedanje.";
                    return false;
                }

                // Kreiraj novo zasedanje
                ZasedanjeDBKlasa zasedanjeDB = new ZasedanjeDBKlasa(konekcija, "zasedanje");
                ZasedanjeKlasa novoZasedanje = new ZasedanjeKlasa
                {
                    Naziv_zasedanja = nazivZasedanja,
                    Tip = tipZasedanja,
                    Id_saziv = aktivanSaziv.Id_saziva
                };

                bool uspesno = zasedanjeDB.DodajNovoZasedanje(novoZasedanje);
                if (uspesno)
                {
                    poruka = $"Uspešno kreirano novo zasedanje '{nazivZasedanja}' za saziv '{aktivanSaziv.Ime}'";
                }
                else
                {
                    poruka = "Greška pri kreiranju zasedanja u bazi podataka";
                }

                return uspesno;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju zasedanja: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Kreira novu sednicu sa pitanjima za određeno zasedanje
        /// </summary>
        public bool KreirajNovuSednicu(int idZasedanja, string nazivSednice, DateTime datumSednice, string opisSednice, List<string> pitanja, out string poruka)
        {
            try
            {
                // Validacija
                if (string.IsNullOrWhiteSpace(nazivSednice))
                {
                    poruka = "Naziv sednice je obavezan.";
                    return false;
                }

                if (datumSednice.Date < DateTime.Today)
                {
                    poruka = "Datum sednice ne sme biti u prošlosti.";
                    return false;
                }

                // GLAVNO POSLOVNO PRAVILO: Ako je sednica manje od 7 dana unapred, mora postojati objašnjenje
                TimeSpan razlika = datumSednice.Date - DateTime.Today;
                if (razlika.TotalDays < 7)
                {
                    if (string.IsNullOrWhiteSpace(opisSednice))
                    {
                        poruka = "Sednica ne može biti zakazana manje od 7 dana unapred bez objašnjenja razloga.";
                        return false;
                    }
                }

                if (pitanja == null || pitanja.Count == 0)
                {
                    poruka = "Sednica mora imati bar jedno pitanje za dnevni red.";
                    return false;
                }

                KonekcijaKlasa konekcija = KreirajKonekciju();
                
                // 1. Kreiraj novu sednicu
                SednicaDBKlasa sednicaDB = new SednicaDBKlasa(konekcija, "sednica");
                SednicaKlasa novaSednica = new SednicaKlasa
                {
                    Naziv = nazivSednice,
                    Datum = datumSednice,
                    Opis = opisSednice,
                    Zasedanje_id = idZasedanja
                };

                bool sednicaKreirana = sednicaDB.DodajNovuSednicu(novaSednica);
                if (!sednicaKreirana)
                {
                    poruka = "Greška pri kreiranju sednice u bazi podataka";
                    return false;
                }

                // 2. Dohvati ID kreirane sednice
                int idSednice = sednicaDB.DajNajnovijuSednicuPoZasedanju(idZasedanja);
                if (idSednice == 0)
                {
                    poruka = "Greška pri dohvatanju ID-ja kreirane sednice";
                    return false;
                }

                // 3. Kreiraj dnevni red
                DnevniRedDBKlasa dnevniRedDB = new DnevniRedDBKlasa(konekcija, "dnevni_red");
                DnevniRedKlasa noviDnevniRed = new DnevniRedKlasa
                {
                    Id_sednice = idSednice
                };

                bool dnevniRedKreiran = dnevniRedDB.DodajNoviDnevniRed(noviDnevniRed);
                if (!dnevniRedKreiran)
                {
                    poruka = "Greška pri kreiranju dnevnog reda";
                    return false;
                }

                // 4. Dohvati ID kreiranog dnevnog reda
                int idDnevniRed = dnevniRedDB.DajNajnovijiDnevniRedPoSednici(idSednice);
                if (idDnevniRed == 0)
                {
                    poruka = "Greška pri dohvatanju ID-ja dnevnog reda";
                    return false;
                }

                // 5. Dodaj pitanja
                PitanjaDBKlasa pitanjaDB = new PitanjaDBKlasa(konekcija, "pitanja");
                for (int i = 0; i < pitanja.Count; i++)
                {
                    PitanjaKlasa novoPitanje = new PitanjaKlasa
                    {
                        Id_dnevni_red = idDnevniRed,
                        Redni_broj = i + 1,
                        Tekst = pitanja[i]
                    };

                    bool pitanjeDodato = pitanjaDB.DodajNovoPitanja(novoPitanje);
                    if (!pitanjeDodato)
                    {
                        poruka = $"Greška pri dodavanju pitanja {i + 1}";
                        return false;
                    }
                }

                poruka = $"Uspešno kreirana nova sednica '{nazivSednice}' sa {pitanja.Count} pitanja";
                return true;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju sednice: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Kreira novu sednicu sa pitanjima za najnovije zasedanje (sa 7-dnevnim pravilom)
        /// </summary>
        public bool KreirajNovuSednicuSaPravilima(string nazivSednice, DateTime datumSednice, string opisSednice, List<string> pitanja, out string poruka)
        {
            try
            {
                // Validacija
                if (string.IsNullOrWhiteSpace(nazivSednice))
                {
                    poruka = "Naziv sednice je obavezan.";
                    return false;
                }

                if (datumSednice.Date < DateTime.Today)
                {
                    poruka = "Datum sednice ne sme biti u prošlosti.";
                    return false;
                }

                // GLAVNO POSLOVNO PRAVILO: Ako je sednica manje od 7 dana unapred, mora postojati objašnjenje
                TimeSpan razlika = datumSednice.Date - DateTime.Today;
                if (razlika.TotalDays < 7)
                {
                    if (string.IsNullOrWhiteSpace(opisSednice))
                    {
                        poruka = "Sednica ne može biti zakazana manje od 7 dana unapred bez objašnjenja razloga.";
                        return false;
                    }
                }

                if (pitanja == null || pitanja.Count == 0)
                {
                    poruka = "Sednica mora imati bar jedno pitanje za dnevni red.";
                    return false;
                }

                KonekcijaKlasa konekcija = KreirajKonekciju();
                
                // Dohvati aktivan saziv i najnovije zasedanje
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(konekcija);
                UpravljanjeZasedanjimaKlasa upravljanjeZasedanjima = new UpravljanjeZasedanjimaKlasa(konekcija);

                // Proveri da li postoji aktivan saziv
                SazivKlasa aktivanSaziv = upravljanjeSazivima.DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    poruka = "Ne postoji aktivan saziv. Prvo kreirajte novi saziv.";
                    return false;
                }

                // Uzmi najnovije zasedanje za aktivan saziv
                ZasedanjeKlasa najnovijeZasedanje = upravljanjeZasedanjima.DajNajnovijeZasedanjePoSazivu(aktivanSaziv.Id_saziva);
                if (najnovijeZasedanje == null)
                {
                    poruka = "Ne postoji nijedno zasedanje za aktivan saziv. Prvo kreirajte novo zasedanje.";
                    return false;
                }

                // 1. Kreiraj novu sednicu
                SednicaDBKlasa sednicaDB = new SednicaDBKlasa(konekcija, "sednica");
                SednicaKlasa novaSednica = new SednicaKlasa
                {
                    Naziv = nazivSednice,
                    Datum = datumSednice,
                    Opis = opisSednice,
                    Zasedanje_id = najnovijeZasedanje.Id_zasedanja  // Automatski koristi najnovije zasedanje
                };

                bool sednicaKreirana = sednicaDB.DodajNovuSednicu(novaSednica);
                if (!sednicaKreirana)
                {
                    poruka = "Greška pri kreiranju sednice u bazi podataka";
                    return false;
                }

                // 2. Dohvati ID kreirane sednice
                int idSednice = sednicaDB.DajNajnovijuSednicuPoZasedanju(najnovijeZasedanje.Id_zasedanja);
                if (idSednice == 0)
                {
                    poruka = "Greška pri dohvatanju ID-ja kreirane sednice";
                    return false;
                }

                // 3. Kreiraj dnevni red
                DnevniRedDBKlasa dnevniRedDB = new DnevniRedDBKlasa(konekcija, "dnevni_red");
                DnevniRedKlasa noviDnevniRed = new DnevniRedKlasa
                {
                    Id_sednice = idSednice
                };

                bool dnevniRedKreiran = dnevniRedDB.DodajNoviDnevniRed(noviDnevniRed);
                if (!dnevniRedKreiran)
                {
                    poruka = "Greška pri kreiranju dnevnog reda";
                    return false;
                }

                // 4. Dohvati ID kreiranog dnevnog reda
                int idDnevniRed = dnevniRedDB.DajNajnovijiDnevniRedPoSednici(idSednice);
                if (idDnevniRed == 0)
                {
                    poruka = "Greška pri dohvatanju ID-ja dnevnog reda";
                    return false;
                }

                // 5. Dodaj pitanja
                PitanjaDBKlasa pitanjaDB = new PitanjaDBKlasa(konekcija, "pitanja");
                for (int i = 0; i < pitanja.Count; i++)
                {
                    PitanjaKlasa novoPitanje = new PitanjaKlasa
                    {
                        Id_dnevni_red = idDnevniRed,
                        Redni_broj = i + 1,
                        Tekst = pitanja[i]
                    };

                    bool pitanjeDodato = pitanjaDB.DodajNovoPitanja(novoPitanje);
                    if (!pitanjeDodato)
                    {
                        poruka = $"Greška pri dodavanju pitanja {i + 1}";
                        return false;
                    }

                    // 6. Generiši random glasove za ovo pitanje
                    int idPitanja = pitanjaDB.DajNajnovijePitanjaId();
                    if (idPitanja > 0)
                    {
                        string porukaGlasanja;
                        bool glasoviGenerisani = GenerisiRandomGlasove(idPitanja, out porukaGlasanja);
                        if (glasoviGenerisani)
                        {
                        }
                        else
                        {
                        }
                    }
                }

                poruka = $"Uspešno kreirana nova sednica '{nazivSednice}' sa {pitanja.Count} pitanja za zasedanje '{najnovijeZasedanje.Naziv_zasedanja}'";
                return true;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju sednice: {ex.Message}";
                return false;
            }
        }



        // ========================================================================
        // POSLOVNA PRAVILA ZA LICA (LAYER 4)
        // ========================================================================

        /// <summary>
        /// Dohvata sva lica iz baze podataka
        /// </summary>
        public List<LicaKlasa> DajSvaLica()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                LicaDBKlasa liceDB = new LicaDBKlasa(konekcija, "lica");
                
                return liceDB.DajSvaLica();
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri dohvatanju lica: {ex.Message}");
            }
        }

        /// <summary>
        /// Kreira novo lice sa validacijom poslovnih pravila
        /// </summary>
        public bool KreirajNovoLice(string ime, string prezime, string korisnickoIme, string lozinka, 
                                   int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, 
                                   string biografija, out string poruka)
        {
            poruka = "";
            
            try
            {
                // Validacija poslovnih pravila
                if (string.IsNullOrWhiteSpace(ime))
                {
                    poruka = "Ime je obavezno.";
                    return false;
                }
                
                if (string.IsNullOrWhiteSpace(prezime))
                {
                    poruka = "Prezime je obavezno.";
                    return false;
                }
                
                if (string.IsNullOrWhiteSpace(korisnickoIme))
                {
                    poruka = "Korisničko ime je obavezno.";
                    return false;
                }
                
                if (string.IsNullOrWhiteSpace(lozinka) || lozinka.Length < 6)
                {
                    poruka = "Lozinka mora imati bar 6 karaktera.";
                    return false;
                }
                
                if (datumRodjenja >= DateTime.Today)
                {
                    poruka = "Datum rođenja ne sme biti u budućnosti.";
                    return false;
                }
                
                // Kreiraj konekciju i DB klasu
                KonekcijaKlasa konekcija = KreirajKonekciju();
                LicaDBKlasa liceDB = new LicaDBKlasa(konekcija, "lica");
                
                // Kreiraj novo lice
                LicaKlasa novoLice = new LicaKlasa
                {
                    Ime = ime,
                    Prezime = prezime,
                    Korisnicko_ime = korisnickoIme,
                    Lozinka = lozinka,
                    Pozicija = pozicijaId,
                    Stranka = strankaId,
                    Pol = pol,
                    Datumr = datumRodjenja,
                    Bio = biografija ?? ""
                };
                
                bool uspesno = liceDB.DodajNovoLice(novoLice);
                if (uspesno)
                {
                    poruka = $"Uspešno kreirano novo lice: {ime} {prezime}";
                    return true;
                }
                else
                {
                    poruka = "Greška pri kreiranju lica u bazi podataka.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju lica: {ex.Message}";
                return false;
            }
        }

        // ========================================================================
        // POSLOVNA PRAVILA ZA MANDATE (LAYER 4)
        // ========================================================================

        /// <summary>
        /// Dohvata sve mandate sa detaljima za View Pogledaj Mandate
        /// </summary>
        public DataSet DajSveMandateZaPregled()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                MandatDBKlasa mandatDB = new MandatDBKlasa(konekcija, "mandat");
                
                // Koristi view v_sviMandati
                return mandatDB.DajSveMandate();
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri dohvatanju mandata: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata sve mandate za aktivan saziv
        /// </summary>
        /// <returns>Lista mandata za aktivan saziv</returns>
        public List<MandatKlasa> DajMandateZaAktivanSaziv()
        {
            try
            {
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeSazivimaKlasa sazivManager = new UpravljanjeSazivimaKlasa(konekcija);
                SazivKlasa aktivanSaziv = sazivManager.DajAktivanSaziv();
                
                if (aktivanSaziv == null)
                {
                    return new List<MandatKlasa>();
                }

                MandatDBKlasa mandatDB = new MandatDBKlasa(konekcija, "mandat");
                return mandatDB.DajMandatePoSazivuLista(aktivanSaziv.Id_saziva);
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri dohvatanju mandata za aktivan saziv: {ex.Message}");
            }
        }

        /// <summary>
        /// Kreira novi mandat za lice u aktivan saziv
        /// </summary>
        public bool KreirajNoviMandat(int liceId, string status, out string poruka)
        {
            poruka = "";
            
            try
            {
                // Validacija poslovnih pravila
                if (liceId <= 0)
                {
                    poruka = "Nevažeći ID lica.";
                    return false;
                }
                
                if (string.IsNullOrWhiteSpace(status))
                {
                    poruka = "Status mandata je obavezan.";
                    return false;
                }
                
                // Dohvati aktivan saziv
                KonekcijaKlasa konekcija = KreirajKonekciju();
                UpravljanjeSazivimaKlasa sazivManager = new UpravljanjeSazivimaKlasa(konekcija);
                
                SazivKlasa aktivanSaziv = sazivManager.DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    poruka = "Nema aktivnog saziva. Prvo kreirajte novi saziv.";
                    return false;
                }
                
                // Proveri da li lice već ima mandat u aktivan saziv
                MandatDBKlasa mandatDB = new MandatDBKlasa(konekcija, "mandat");
                List<MandatKlasa> postojeciMandati = mandatDB.DajMandatePoLiciISazivu(liceId, aktivanSaziv.Id_saziva);
                
                if (postojeciMandati.Any())
                {
                    poruka = "Lice već ima mandat u aktivan saziv.";
                    return false;
                }
                
                // Dohvati informacije o licu da bismo dobili id_stranke
                LicaDBKlasa licaDB = new LicaDBKlasa(konekcija, "lica");
                
                // Dohvati informacije o licu da bismo dobili id_stranke
                DataSet liceDataSet = licaDB.DajSvaLicaDataSet();
                DataRow liceRow = liceDataSet.Tables[0].Select($"id_lica = {liceId}").FirstOrDefault();
                
                if (liceRow == null)
                {
                    poruka = "Lice nije pronađeno u bazi podataka.";
                    return false;
                }
                
                int idStranke = Convert.ToInt32(liceRow["stranka"]);
                
                // Kreiraj novi mandat - samo sa poljima koja postoje u bazi podataka
                MandatKlasa noviMandat = new MandatKlasa
                {
                    Id_lica = liceId,
                    Id_saziva = aktivanSaziv.Id_saziva,
                    Id_stranke = idStranke
                };
                
                bool uspesno = mandatDB.DodajNoviMandat(noviMandat);
                if (uspesno)
                {
                    poruka = $"Uspešno kreiran mandat za lice ID: {liceId} u saziv: {aktivanSaziv.Ime}";
                    return true;
                }
                else
                {
                    poruka = "Greška pri kreiranju mandata u bazi podataka.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju mandata: {ex.Message}";
                return false;
            }
        }



        // ========================================================================
        // METODE ZA STRANKE
        // ========================================================================

        /// <summary>
        /// Dohvata sve stranke iz baze podataka
        /// </summary>
        /// <returns>Lista svih stranaka</returns>
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
                throw new Exception($"Greška pri dohvatanju stranaka: {ex.Message}");
            }
        }

        // ========================================================================
        // METODE ZA POZICIJE
        // ========================================================================

        /// <summary>
        /// Dohvata sve pozicije iz baze podataka
        /// </summary>
        /// <returns>Lista svih pozicija</returns>
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
                throw new Exception($"Greška pri dohvatanju pozicija: {ex.Message}");
            }
        }
    }
}
