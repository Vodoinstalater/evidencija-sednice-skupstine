using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//
using DBUtils;
using KlasePodataka;
using PoslovnaLogika;

namespace KlaseMapiranja
{
    /// <summary>
    /// Servisna klasa koja kombinuje poslovnu logiku sa mapiranjem podataka
    /// Služi kao most između Layer 2 (Poslovna Logika) i Layer 4 (UI)
    /// </summary>
    public class SednicaServisKlasa
    {
        // atributi
        private string _stringKonekcije;
        private SednicaPoslovnaLogikaKlasa _poslovnaLogika;
        private SednicaMaperKlasa _maper;

        // konstruktor
        public SednicaServisKlasa(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
            _poslovnaLogika = new SednicaPoslovnaLogikaKlasa(_stringKonekcije);
            _maper = new SednicaMaperKlasa(_stringKonekcije);
        }

        // ========================================================================
        // SERVISI ZA VIEW OPERACIJE (za Layer 4 UI)
        // ========================================================================

        /// <summary>
        /// Servis za dohvatanje svih sednica sa formatiranjem za UI
        /// </summary>
        public ServiceResult<List<SednicaDTO>> DajSveSednice(int? sazivId = null, int? zasedanjeId = null)
        {
            try
            {
                DataSet dsSednice = _poslovnaLogika.DajSveSednice(sazivId, zasedanjeId);
                List<SednicaDTO> sednice = _maper.KonvertujSedniceIzDataSet(dsSednice);
                
                return _maper.KreirajUspeh(sednice, $"Dohvaćeno je {sednice.Count} sednica.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<SednicaDTO>>("Greška pri dohvatanju sednica.", ex);
            }
        }

        /// <summary>
        /// Servis za dohvatanje svih saziva sa formatiranjem za UI
        /// </summary>
        public ServiceResult<List<SazivDTO>> DajSveSazive()
        {
            try
            {
                List<SazivKlasa> sazivi = _poslovnaLogika.DajSveSazive();
                List<SazivDTO> saziviDTO = sazivi.Select(s => new SazivDTO
                {
                    Id = s.Id_saziva,
                    Ime = s.Ime ?? "",
                    DatumPocetka = s.Pocetak,
                    DatumZavrsetka = s.Kraj,
                    PeriodFormatiran = $"{s.Pocetak:dd.MM.yyyy} - {s.Kraj:dd.MM.yyyy}",
                    Opis = s.Opis ?? "",
                    Aktivan = s.Kraj > DateTime.Now
                }).ToList();
                
                return _maper.KreirajUspeh(saziviDTO, $"Dohvaćeno je {saziviDTO.Count} saziva.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<SazivDTO>>("Greška pri dohvatanju saziva.", ex);
            }
        }

        /// <summary>
        /// Servis za dohvatanje aktivnog saziva
        /// </summary>
        public ServiceResult<SazivDTO> DajAktivanSaziv()
        {
            try
            {
                SazivKlasa aktivanSaziv = _poslovnaLogika.DajAktivanSaziv();
                
                if (aktivanSaziv != null)
                {
                    SazivDTO sazivDTO = new SazivDTO
                    {
                        Id = aktivanSaziv.Id_saziva,
                        Ime = aktivanSaziv.Ime ?? "",
                        DatumPocetka = aktivanSaziv.Pocetak,
                        DatumZavrsetka = aktivanSaziv.Kraj,
                        PeriodFormatiran = $"{aktivanSaziv.Pocetak:dd.MM.yyyy} - {aktivanSaziv.Kraj:dd.MM.yyyy}",
                        Opis = aktivanSaziv.Opis ?? "",
                        Aktivan = true
                    };
                    
                    return _maper.KreirajUspeh(sazivDTO, "Aktivan saziv je uspešno dohvaćen.");
                }
                
                return _maper.KreirajGresku<SazivDTO>("Nema aktivnog saziva.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<SazivDTO>("Greška pri dohvatanju aktivnog saziva.", ex);
            }
        }

        /// <summary>
        /// Servis za dohvatanje svih zasedanja sa formatiranjem za UI
        /// </summary>
        public ServiceResult<List<ZasedanjeDTO>> DajSvaZasedanja(int? sazivId = null, string tipZasedanja = null)
        {
            try
            {
                DataSet dsZasedanja = _poslovnaLogika.DajSvaZasedanja(sazivId, tipZasedanja);
                List<ZasedanjeDTO> zasedanja = new List<ZasedanjeDTO>();

                // Konverzija DataSet u DTO
                if (dsZasedanja?.Tables?.Count > 0 && dsZasedanja.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsZasedanja.Tables[0].Rows)
                    {
                        zasedanja.Add(new ZasedanjeDTO
                        {
                            Id = Convert.ToInt32(row["id_zasedanja"]),
                            Naziv = row["naziv_zasedanja"].ToString(),
                            TipZasedanja = row["tip"].ToString(),
                            DatumPocetka = null, // Datum nije u bazi
                            DatumZavrsetka = null, // Datum nije u bazi
                            NazivSaziva = row["saziv_ime"]?.ToString() ?? "N/A",
                            Opis = "", // Opis nije u bazi
                            BrojSednica = 0
                        });
                    }
                }
                
                return _maper.KreirajUspeh(zasedanja, $"Dohvaćeno je {zasedanja.Count} zasedanja.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<ZasedanjeDTO>>("Greška pri dohvatanju zasedanja.", ex);
            }
        }

        /// <summary>
        /// Servis za dohvatanje svih mandata sa formatiranjem za UI
        /// </summary>
        public ServiceResult<List<MandatDTO>> DajSveMandate()
        {
            try
            {
                DataSet dsMandate = _poslovnaLogika.DajSveMandateZaPregled();
                List<MandatDTO> mandate = _maper.KonvertujMandateIzDataSet(dsMandate);
                
                return _maper.KreirajUspeh(mandate, $"Dohvaćeno je {mandate.Count} mandata.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<MandatDTO>>("Greška pri dohvatanju mandata.", ex);
            }
        }

        /// <summary>
        /// Servis za dohvatanje mandata sa filterima
        /// </summary>
        public ServiceResult<List<MandatDTO>> DajMandatePoFilterima(int? id_saziva = null, int? id_stranke = null, int? id_pozicije = null, string ime_prezime = null)
        {
            try
            {
                DataSet dsMandate = _poslovnaLogika.DajMandatePoFilterima(id_saziva, id_stranke, id_pozicije, ime_prezime);
                List<MandatDTO> mandate = _maper.KonvertujMandateIzDataSet(dsMandate);
                
                return _maper.KreirajUspeh(mandate, $"Dohvaćeno je {mandate.Count} mandata sa filterima.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<MandatDTO>>("Greška pri dohvatanju mandata sa filterima.", ex);
            }
        }

        /// <summary>
        /// Servis za dohvatanje saziva sa filterima
        /// </summary>
        public ServiceResult<List<SazivDTO>> DajSazivePoFilterima(string naziv = null, DateTime? pocetak = null, DateTime? kraj = null)
        {
            try
            {
                DataSet dsSazivi = _poslovnaLogika.DajSazivePoFilterima(naziv, pocetak, kraj);
                List<SazivDTO> sazivi = new List<SazivDTO>();

                if (dsSazivi?.Tables?.Count > 0 && dsSazivi.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsSazivi.Tables[0].Rows)
                    {
                        try
                        {
                            DateTime? datumPocetka = null;
                            DateTime? datumZavrsetka = null;
                            
                            // Safe date conversion
                            if (row["pocetak"] != DBNull.Value)
                                datumPocetka = Convert.ToDateTime(row["pocetak"]);
                            if (row["kraj"] != DBNull.Value)
                                datumZavrsetka = Convert.ToDateTime(row["kraj"]);
                            
                            int sazivId = Convert.ToInt32(row["id_saziva"]);
                            sazivi.Add(new SazivDTO
                            {
                                Id = sazivId,
                                Ime = row["ime"]?.ToString() ?? "",
                                DatumPocetka = datumPocetka,
                                DatumZavrsetka = datumZavrsetka,
                                PeriodFormatiran = datumPocetka.HasValue && datumZavrsetka.HasValue 
                                    ? $"{datumPocetka.Value:dd.MM.yyyy} - {datumZavrsetka.Value:dd.MM.yyyy}"
                                    : "Nepoznato",
                                Opis = row["opis"]?.ToString() ?? "",
                                Aktivan = datumZavrsetka.HasValue ? datumZavrsetka.Value > DateTime.Now : false
                            });
                        }
                        catch (Exception)
                        {
                            // Skip this row if there's an error, continue with others
                            continue;
                        }
                    }
                }
                
                return _maper.KreirajUspeh(sazivi, $"Dohvaćeno je {sazivi.Count} saziva sa filterima.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<SazivDTO>>("Greška pri dohvatanju saziva sa filterima.", ex);
            }
        }

        /// <summary>
        /// Servis za dohvatanje glasanja sa filterima
        /// </summary>
        public ServiceResult<List<GlasanjeDTO>> DajGlasanjaPoFilterima(int? id_pitanja = null, int? id_lica = null, int? id_sednice = null, int? id_saziva = null, string glas = null)
        {
            try
            {
                DataSet dsGlasanja = _poslovnaLogika.DajIstorijuGlasanjaPoFilterima(id_pitanja, id_lica, id_sednice, id_saziva, glas);
                List<GlasanjeDTO> glasanja = _maper.KonvertujGlasanjaIzDataSet(dsGlasanja);
                
                return _maper.KreirajUspeh(glasanja, $"Dohvaćeno je {glasanja.Count} glasanja sa filterima.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<GlasanjeDTO>>("Greška pri dohvatanju glasanja sa filterima.", ex);
            }
        }



        /// <summary>
        /// Servis za dohvatanje istorije glasanja sa formatiranjem za UI
        /// </summary>
        public ServiceResult<DataSet> DajIstorijuGlasanja()
        {
            try
            {
                DataSet dsGlasanja = _poslovnaLogika.DajIstorijuGlasanja();
                
                return _maper.KreirajUspeh(dsGlasanja, "Istorija glasanja je uspešno dohvaćena.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<DataSet>("Greška pri dohvatanju istorije glasanja.", ex);
            }
        }

        /// <summary>
        /// Dohvata sva pitanja za View Glasanja
        /// </summary>
        public ServiceResult<List<string>> DajSvaPitanja()
        {
            try
            {
                var pitanjaDataSet = _poslovnaLogika.DajSvaPitanja();
                var pitanja = new List<string>();
                
                if (pitanjaDataSet != null && pitanjaDataSet.Tables.Count > 0 && pitanjaDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in pitanjaDataSet.Tables[0].Rows)
                    {
                        string tekst = row["tekst"]?.ToString() ?? "";
                        if (!string.IsNullOrEmpty(tekst))
                        {
                            pitanja.Add(tekst);
                        }
                    }
                }
                
                return new ServiceResult<List<string>>
                {
                    Uspesno = true,
                    Podaci = pitanja,
                    Poruka = $"Uspešno dohvaćeno {pitanja.Count} pitanja"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<List<string>>
                {
                    Uspesno = false,
                    Podaci = new List<string>(),
                    Poruka = $"Greška pri dohvatanju pitanja: {ex.Message}"
                };
            }
        }





        /// <summary>
        /// Dohvata trenutno aktivno zasedanje za aktivan saziv
        /// </summary>
        public ServiceResult<ZasedanjeDTO> DajTrenutnoZasedanje()
        {
            try
            {
                DataSet dsZasedanje = _poslovnaLogika.DajTrenutnoZasedanje();
                
                if (dsZasedanje?.Tables?.Count > 0 && dsZasedanje.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsZasedanje.Tables[0].Rows[0];
                    var zasedanje = new ZasedanjeDTO
                    {
                        Id = Convert.ToInt32(row["id_zasedanja"]),
                        Naziv = row["naziv_zasedanja"].ToString(),
                        TipZasedanja = row["tip"].ToString(),
                        DatumPocetka = Convert.ToDateTime(row["saziv_pocetak"]),
                        DatumZavrsetka = Convert.ToDateTime(row["saziv_kraj"]),
                        NazivSaziva = row["saziv_ime"].ToString(),
                        Opis = "",
                        BrojSednica = 0
                    };
                    
                    return _maper.KreirajUspeh(zasedanje, "Trenutno aktivno zasedanje je uspešno dohvaćeno.");
                }
                
                return _maper.KreirajGresku<ZasedanjeDTO>("Nema aktivnog zasedanja.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<ZasedanjeDTO>("Greška pri dohvatanju trenutnog zasedanja.", ex);
            }
        }

        /// <summary>
        /// Kreira novo zasedanje za aktivan saziv
        /// </summary>
        public ServiceResult<bool> KreirajNovoZasedanje(string nazivZasedanja, int tipZasedanja)
        {
            try
            {
                string poruka;
                
                // 1. VALIDACIJA - Layer 2 (PoslovnaLogika)
                if (!_poslovnaLogika.DaLiSeMozeKreiratiZasedanje(nazivZasedanja, DateTime.Today.AddDays(1), "Automatski kreiran", tipZasedanja, out poruka))
                {
                    return _maper.KreirajGresku<bool>(poruka);
                }
                
                // 2. DATA MANIPULATION - Layer 1 (KlasePodataka)
                KonekcijaKlasa konekcija = new KonekcijaKlasa();
                konekcija.OtvoriKonekciju();
                UpravljanjeZasedanjimaKlasa upravljanjeZasedanjima = new UpravljanjeZasedanjimaKlasa(konekcija);
                
                bool uspesno = upravljanjeZasedanjima.KreirajNovoZasedanjeSaValidacijom(nazivZasedanja, DateTime.Today.AddDays(1), "Automatski kreiran", tipZasedanja, out poruka);
                
                if (uspesno)
                    return _maper.KreirajUspeh(true, poruka);
                else
                    return _maper.KreirajGresku<bool>(poruka);
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<bool>("Neočekivana greška pri kreiranju zasedanja.", ex);
            }
        }

        /// <summary>
        /// Kreira novu sednicu sa pitanjima za određeno zasedanje
        /// </summary>
        public ServiceResult<bool> KreirajNovuSednicu(int idZasedanja, string nazivSednice, DateTime datumSednice, string opisSednice, List<string> pitanja)
        {
            try
            {
                // Prvo validacija u Layer 2
                string poruka;
                bool validacijaUspešna = _poslovnaLogika.KreirajNovuSednicu(idZasedanja, nazivSednice, datumSednice, opisSednice, pitanja, out poruka);
                
                if (!validacijaUspešna)
                {
                    return _maper.KreirajGresku<bool>(poruka);
                }

                // Ako je validacija prošla, pozovi Layer 1 za kreiranje
                KonekcijaKlasa konekcija = new KonekcijaKlasa(); // Koristi default konekciju;
                UpravljanjeSednicamaKlasa upravljanjeSednicama = new UpravljanjeSednicamaKlasa(konekcija);
                
                bool kreiranjeUspešno = upravljanjeSednicama.KreirajNovuSednicuSaValidacijom(nazivSednice, datumSednice, opisSednice, pitanja, out string layer1Poruka);
                
                if (kreiranjeUspešno)
                {
                    return _maper.KreirajUspeh(true, "Sednica je uspešno kreirana.");
                }
                else
                {
                    return _maper.KreirajGresku<bool>(layer1Poruka);
                }
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<bool>("Greška pri kreiranju sednice.", ex);
            }
        }

        /// <summary>
        /// Kreira novu sednicu sa pitanjima za određeno zasedanje (sa 7-dnevnim pravilom)
        /// </summary>
        public ServiceResult<bool> KreirajNovuSednicuSaPravilima(string nazivSednice, DateTime datumSednice, string opisSednice, List<string> pitanja)
        {
            try
            {
                string poruka;
                
                // 1. VALIDACIJA - Layer 2 (PoslovnaLogika)
                if (!_poslovnaLogika.DaLiSeMozeKreiratiSednica(nazivSednice, datumSednice, opisSednice, pitanja, out poruka))
                {
                    return _maper.KreirajGresku<bool>(poruka);
                }
                
                // 2. DATA MANIPULATION - Layer 1 (KlasePodataka)
                KonekcijaKlasa konekcija = new KonekcijaKlasa();
                konekcija.OtvoriKonekciju();
                UpravljanjeSednicamaKlasa upravljanjeSednicama = new UpravljanjeSednicamaKlasa(konekcija);
                
                bool uspesno = upravljanjeSednicama.KreirajNovuSednicuSaValidacijom(nazivSednice, datumSednice, opisSednice, pitanja, out poruka);
                
                if (uspesno)
                {
                    return _maper.KreirajUspeh(true, poruka);
                }
                else
                {
                    return _maper.KreirajGresku<bool>(poruka);
                }
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<bool>("Greška pri kreiranju sednice.", ex);
            }
        }

        // ========================================================================
        // SERVISI ZA CREATE OPERACIJE (sa validacijom i formatiranjem)
        // ========================================================================

        /// <summary>
        /// Servis za kreiranje novog saziva
        /// </summary>
        public ServiceResult<bool> KreirajNoviSaziv(string naziv, DateTime pocetak, DateTime zavrsetak, string opis)
        {
            try
            {
                string poruka;
                
                // 1. VALIDACIJA - Layer 2 (PoslovnaLogika)
                if (!_poslovnaLogika.DaLiSeMozeKreiratiSaziv(naziv, pocetak, zavrsetak, opis, out poruka))
                {
                    return _maper.KreirajGresku<bool>(poruka);
                }
                
                // 2. DATA MANIPULATION - Layer 1 (KlasePodataka)
                KonekcijaKlasa konekcija = new KonekcijaKlasa();
                konekcija.OtvoriKonekciju();
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(konekcija);
                
                bool uspesno = upravljanjeSazivima.KreirajNoviSazivSaValidacijom(naziv, pocetak, zavrsetak, opis, out poruka);
                
                if (uspesno)
                    return _maper.KreirajUspeh(true, poruka);
                else
                    return _maper.KreirajGresku<bool>(poruka);
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<bool>("Neočekivana greška pri kreiranju saziva.", ex);
            }
        }

        /// <summary>
        /// Servis za kreiranje nove sednice - implementira 7-dana poslovno pravilo
        /// </summary>
        public ServiceResult<bool> KreirajNovuSednicu(string naziv, DateTime datum, string opis, 
            List<string> pitanja, string objasnjenje)
        {
            try
            {
                // Prvo validacija u Layer 2
                string poruka;
                bool validacijaUspešna = _poslovnaLogika.KreirajNovuSednicu(naziv, datum, opis, pitanja, objasnjenje, out poruka);
                
                if (!validacijaUspešna)
                {
                    return _maper.KreirajGresku<bool>(poruka);
                }

                // Ako je validacija prošla, pozovi Layer 1 za kreiranje
                KonekcijaKlasa konekcija = new KonekcijaKlasa(); // Koristi default konekciju;
                UpravljanjeSednicamaKlasa upravljanjeSednicama = new UpravljanjeSednicamaKlasa(konekcija);
                
                bool kreiranjeUspešno = upravljanjeSednicama.KreirajNovuSednicuSaValidacijom(naziv, datum, opis, pitanja, out string layer1Poruka);
                
                if (kreiranjeUspešno)
                {
                    return _maper.KreirajUspeh(true, "Sednica je uspešno kreirana.");
                }
                else
                {
                    return _maper.KreirajGresku<bool>(layer1Poruka);
                }
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<bool>("Neočekivana greška pri kreiranju sednice.", ex);
            }
        }

        /// <summary>
        /// Servis za kreiranje novog lica i povezivanje sa mandatom
        /// </summary>
        public ServiceResult<bool> KreirajNovoLicaIMandat(string ime, string prezime, string korisnickoIme, 
            string lozinka, int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, string bio)
        {
            try
            {
                string poruka;
                
                // 1. VALIDACIJA - Layer 2 (PoslovnaLogika)
                if (!_poslovnaLogika.DaLiSeMozeKreiratiLiceIMandat(ime, prezime, korisnickoIme, lozinka, 
                    pozicijaId, strankaId, pol, datumRodjenja, bio, out poruka))
                {
                    return _maper.KreirajGresku<bool>(poruka);
                }
                
                // 2. DATA MANIPULATION - Layer 1 (KlasePodataka)
                KonekcijaKlasa konekcija = new KonekcijaKlasa(); // Koristi default konekciju
                UpravljanjeMandatimaKlasa upravljanjeMandatima = new UpravljanjeMandatimaKlasa(konekcija);
                
                bool uspesno = upravljanjeMandatima.KreirajNovoLiceIMandat(ime, prezime, korisnickoIme, lozinka, 
                    pozicijaId, strankaId, pol, datumRodjenja, bio, out poruka);
                
                if (uspesno)
                    return _maper.KreirajUspeh(true, poruka);
                else
                    return _maper.KreirajGresku<bool>(poruka);
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<bool>("Neočekivana greška pri kreiranju lica.", ex);
            }
        }

        // ========================================================================
        // SERVISI ZA AUTENTIFIKACIJU I AUTORIZACIJU

        // ========================================================================

        /// <summary>
        /// Servis za prijavu korisnika - poziva Layer 2 business logic
        /// </summary>
        public ServiceResult<KorisnikDTO> PrijaviKorisnika(string korisnickoIme, string lozinka)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(korisnickoIme) || string.IsNullOrWhiteSpace(lozinka))
                {
                    return _maper.KreirajGresku<KorisnikDTO>("Korisničko ime i lozinka su obavezni.", null);
                }

                // Poziv Layer 2 business logic za autentifikaciju
                bool uspesnaPrijava = _poslovnaLogika.ProveriKorisnika(korisnickoIme, lozinka, out string poruka);
                
                if (uspesnaPrijava)
                {
                    // Dohvati podatke o korisniku
                    DataSet dsKorisnik = _poslovnaLogika.DajPodatkeKorisnika(korisnickoIme, out string porukaKorisnik);
                    
                    if (dsKorisnik != null && dsKorisnik.Tables.Count > 0 && dsKorisnik.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = dsKorisnik.Tables[0].Rows[0];
                        
                        // Kreiraj KorisnikDTO sa stvarnim podacima
                        KorisnikDTO korisnik = new KorisnikDTO
                        {
                            Id = Convert.ToInt32(row["id_lica"]),
                            KorisnickoIme = korisnickoIme,
                            ImePrezime = $"{row["ime"]} {row["prezime"]}",
                            TipKorisnika = _maper.FormatiraTipKorisnika(Convert.ToInt32(row["pozicija"])),
                            VremePoslednjePrijave = DateTime.Now,
                            Dozvole = DajDozvolePoProfilu(_maper.FormatiraTipKorisnika(Convert.ToInt32(row["pozicija"])))
                        };
                        
                        return _maper.KreirajUspeh(korisnik, "Uspešna prijava.");
                    }
                    else
                    {
                        return _maper.KreirajGresku<KorisnikDTO>("Greška pri dohvatanju podataka korisnika.", null);
                    }
                }
                else
                {
                    return _maper.KreirajGresku<KorisnikDTO>(poruka, null);
                }
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<KorisnikDTO>("Greška pri prijavi korisnika.", ex);
            }
        }

        /// <summary>
        /// Servis za proveru dozvola korisnika - poziva Layer 2 business logic
        /// </summary>
        public ServiceResult<bool> ProveriDozvolu(string tipKorisnika, string akcija)
        {
            try
            {
        
                // Za sada koristimo jednostavnu logiku na osnovu tipa korisnika
                
                if (tipKorisnika == "Predsednik")
                {
                    return _maper.KreirajUspeh(true, "Predsednik ima sve dozvole.");
                }
                else if (tipKorisnika == "Potpredsednik")
                {
                    if (akcija == "SazoviNovuSednicu" || akcija == "OtvoriNovoZasedanje")
                    {
                        return _maper.KreirajUspeh(true, "Potpredsednik ima dozvolu za ovu akciju.");
                    }
                }
                else if (tipKorisnika == "Poslanik")
                {
                    if (akcija == "PogledajSednice" || akcija == "PogledajZasedanja" || 
                        akcija == "PogledajSazive" || akcija == "PogledajMandate" || 
                        akcija == "IstorijaGlasanja")
                    {
                        return _maper.KreirajUspeh(true, "Poslanik ima dozvolu za ovu akciju.");
                    }
                }
                
                return _maper.KreirajGresku<bool>("Korisnik nema dozvolu za ovu akciju.", null);
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<bool>("Greška pri proveri dozvola.", ex);
            }
        }

        // ========================================================================
        // SERVISI ZA GLASANJA
        // ========================================================================

        /// <summary>
        /// Servis za dohvatanje svih glasanja sa formatiranjem za UI
        /// </summary>
        public ServiceResult<List<GlasanjeDTO>> DajSvaGlasanja()
        {
            try
            {
                DataSet dsGlasanja = _poslovnaLogika.DajSvaGlasanja();
                List<GlasanjeDTO> glasanja = _maper.KonvertujGlasanjaIzDataSet(dsGlasanja);
                
                return _maper.KreirajUspeh(glasanja, $"Dohvaćeno je {glasanja.Count} glasanja.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<GlasanjeDTO>>("Greška pri dohvatanju glasanja.", ex);
            }
        }

        /// <summary>
        /// Generiše random glasove za novo pitanje
        /// </summary>
        public ServiceResult<bool> GenerisiRandomGlasove(int pitanjeId)
        {
            try
            {
                string poruka;
                bool rezultat = _poslovnaLogika.GenerisiRandomGlasove(pitanjeId, out poruka);
                
                if (rezultat)
                {
                    return new ServiceResult<bool>
                    {
                        Uspesno = true,
                        Podaci = true,
                        Poruka = poruka
                    };
                }
                else
                {
                    return new ServiceResult<bool>
                    {
                        Uspesno = false,
                        Podaci = false,
                        Poruka = poruka
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>
                {
                    Uspesno = false,
                    Podaci = false,
                    Poruka = $"Greška pri generisanju random glasova: {ex.Message}"
                };
            }
        }

        // ========================================================================
        // SERVISI ZA TIPOVE ZASEDANJA


        // ========================================================================
        // SERVISI ZA LICA
        // ========================================================================

        /// <summary>
        /// Servis za dohvatanje svih lica
        /// </summary>
        public ServiceResult<List<LiceDTO>> DajSvaLica()
        {
            try
            {
                List<LicaKlasa> lica = _poslovnaLogika.DajSvaLica();
                List<LiceDTO> licaDTO = lica.Select(l => new LiceDTO
                {
                    Id = l.Id_lica,
                    Ime = l.Ime,
                    Prezime = l.Prezime,
                    KorisnickoIme = l.Korisnicko_ime,
                    Pozicija = l.Pozicija.ToString(),
                    Stranka = l.Stranka.ToString(),
                    Pol = l.Pol,
                    DatumRodjenja = l.Datumr,
                    Biografija = l.Bio
                }).ToList();
                
                return _maper.KreirajUspeh(licaDTO, $"Dohvaćeno je {licaDTO.Count} lica.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<LiceDTO>>("Greška pri dohvatanju lica.", ex);
            }
        }

        /// <summary>
        /// Servis za kreiranje novog lica (bez mandata)
        /// </summary>
        public ServiceResult<bool> KreirajNovoLice(string ime, string prezime, string korisnickoIme, string lozinka, 
                                                  int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, 
                                                  string biografija)
        {
            try
            {
                // Prvo validacija u Layer 2
                string poruka;
                bool validacijaUspešna = _poslovnaLogika.KreirajNovoLice(ime, prezime, korisnickoIme, lozinka, pozicijaId, strankaId, pol, datumRodjenja, biografija, out poruka);
                
                if (!validacijaUspešna)
                {
                    return _maper.KreirajGresku<bool>(poruka);
                }

                // Ako je validacija prošla, pozovi Layer 1 za kreiranje
                KonekcijaKlasa konekcija = new KonekcijaKlasa(); // Koristi default konekciju
                UpravljanjeMandatimaKlasa upravljanjeMandatima = new UpravljanjeMandatimaKlasa(konekcija);
                
                bool kreiranjeUspešno = upravljanjeMandatima.KreirajNovoLice(ime, prezime, korisnickoIme, lozinka, pozicijaId, strankaId, pol, datumRodjenja, biografija, out string layer1Poruka);
                
                if (kreiranjeUspešno)
                {
                    return _maper.KreirajUspeh(true, "Lice je uspešno kreirano.");
                }
                else
                {
                    return _maper.KreirajGresku<bool>(layer1Poruka);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>
                {
                    Uspesno = false,
                    Podaci = false,
                    Poruka = $"Greška pri kreiranju lica: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Servis za kreiranje novog lica i mandata zajedno
        /// </summary>
        public ServiceResult<bool> KreirajNovoLiceIMandat(string ime, string prezime, string korisnickoIme, string lozinka, 
                                                         int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, 
                                                         string biografija)
        {
            try
            {
                string poruka;
                bool rezultat = _poslovnaLogika.KreirajNovoLiceIMandat(ime, prezime, korisnickoIme, lozinka, pozicijaId, strankaId, pol, datumRodjenja, biografija, out poruka);
                
                if (rezultat)
                {
                    return new ServiceResult<bool>
                    {
                        Uspesno = true,
                        Podaci = true,
                        Poruka = poruka
                    };
                }
                else
                {
                    return new ServiceResult<bool>
                    {
                        Uspesno = false,
                        Podaci = false,
                        Poruka = poruka
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>
                {
                    Uspesno = false,
                    Podaci = false,
                    Poruka = $"Greška pri kreiranju lica i mandata: {ex.Message}"
                };
            }
        }

        // ========================================================================
        // METODE ZA STRANKE
        // ========================================================================

        /// <summary>
        /// Dohvata sve stranke iz baze podataka
        /// </summary>
        /// <returns>ServiceResult sa listom StrankaDTO objekata</returns>
        public ServiceResult<List<StrankaDTO>> DajSveStranke()
        {
            try
            {
                List<StrankaKlasa> stranke = _poslovnaLogika.DajSveStranke();
                List<StrankaDTO> strankeDTO = stranke.Select(s => new StrankaDTO
                {
                    Id = s.Id_stranke,
                    Naziv = s.Naziv_stranke
                }).ToList();
                return _maper.KreirajUspeh(strankeDTO, $"Dohvaćeno je {strankeDTO.Count} stranaka.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<StrankaDTO>>($"Greška pri dohvatanju stranaka: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata sve mandate za aktivan saziv
        /// </summary>
        /// <returns>ServiceResult sa listom MandatDTO objekata</returns>
        public ServiceResult<List<MandatDTO>> DajMandateZaAktivanSaziv()
        {
            try
            {
                List<MandatKlasa> mandati = _poslovnaLogika.DajMandateZaAktivanSaziv();
                List<MandatDTO> mandatiDTO = mandati.Select(m => new MandatDTO
                {
                    Id = m.Id_mandata,
                    IdLica = m.Id_lica,
                    IdSaziva = m.Id_saziva,
                    IdStranke = m.Id_stranke,
                    ImeLica = m.ImeLica,
                    PrezimeLica = m.PrezimeLica,
                    NazivStranke = m.NazivStranke,
                    NazivPozicije = m.NazivPozicije,
                    NazivSaziva = ""
                }).ToList();
                return _maper.KreirajUspeh(mandatiDTO, $"Dohvaćeno je {mandatiDTO.Count} mandata za aktivan saziv.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<MandatDTO>>($"Greška pri dohvatanju mandata za aktivan saziv: {ex.Message}");
            }
        }

        /// <summary>
        /// Dohvata statistike mandata za aktivan saziv
        /// </summary>
        /// <returns>ServiceResult sa statistikama mandata</returns>
        public ServiceResult<object> DajStatistikeMandataZaAktivanSaziv()
        {
            try
            {
                List<MandatKlasa> mandati = _poslovnaLogika.DajMandateZaAktivanSaziv();
                
                if (mandati == null || mandati.Count == 0)
                {
                    return _maper.KreirajUspeh((object)new
                    {
                        UkupnoMandata = 0,
                        AktivnihMandata = 0,
                        Poslanika = 0,
                        Stranaka = 0
                    }, "Nema mandata za aktivan saziv.");
                }

                int ukupno = mandati.Count;
                int aktivnih = mandati.Count; // Svi mandate su aktivni u trenutnom sazivu
                int poslanika = mandati.Select(m => m.Id_lica).Distinct().Count();
                int stranaka = mandati.Select(m => m.NazivStranke).Distinct().Count();

                return _maper.KreirajUspeh((object)new
                {
                    UkupnoMandata = ukupno,
                    AktivnihMandata = aktivnih,
                    Poslanika = poslanika,
                    Stranaka = stranaka
                }, $"Dohvaćene su statistike za {ukupno} mandata.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<object>($"Greška pri dohvatanju statistika mandata: {ex.Message}");
            }
        }

        // ========================================================================
        // METODE ZA POZICIJE
        // ========================================================================

        /// <summary>
        /// Dohvata sve pozicije iz baze podataka
        /// </summary>
        /// <returns>ServiceResult sa listom PozicijaDTO objekata</returns>
        public ServiceResult<List<PozicijaDTO>> DajSvePozicije()
        {
            try
            {
                List<PozicijaKlasa> pozicije = _poslovnaLogika.DajSvePozicije();
                List<PozicijaDTO> pozicijeDTO = pozicije.Select(p => new PozicijaDTO
                {
                    Id = p.Id_pozicije,
                    Naziv = p.Naziv_pozicije
                }).ToList();
                return _maper.KreirajUspeh(pozicijeDTO, $"Dohvaćeno je {pozicijeDTO.Count} pozicija.");
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<List<PozicijaDTO>>($"Greška pri dohvatanju pozicija: {ex.Message}");
            }
        }

        /// <summary>
        /// Validira da li je sistem spreman za rad
        /// </summary>
        public ServiceResult<bool> DaLiJeSistemSpremanZaRad()
        {
            try
            {
                bool spreman = _poslovnaLogika.DaLiJeSistemSpremanZaRad(out string poruka);
                return new ServiceResult<bool>
                {
                    Uspesno = true,
                    Podaci = spreman,
                    Poruka = poruka
                };
            }
            catch (Exception ex)
            {
                return _maper.KreirajGresku<bool>("Greška pri proveri sistema.", ex);
            }
        }

        // ========================================================================
        // SERVISI ZA MANDATE
        // ========================================================================

        /// <summary>
        /// Servis za kreiranje novog mandata
        /// </summary>
        public ServiceResult<bool> KreirajNoviMandat(int liceId, string status)
        {
            try
            {
                string poruka;
                bool rezultat = _poslovnaLogika.KreirajNoviMandat(liceId, status, out poruka);
                
                if (rezultat)
                {
                    return new ServiceResult<bool>
                    {
                        Uspesno = true,
                        Podaci = true,
                        Poruka = poruka
                    };
                }
                else
                {
                    return new ServiceResult<bool>
                    {
                        Uspesno = false,
                        Podaci = false,
                        Poruka = poruka
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>
                {
                    Uspesno = false,
                    Podaci = false,
                    Poruka = $"Greška pri kreiranju mandata: {ex.Message}"
                };
            }
        }

        // ========================================================================
        // PRIVATNE POMOĆNE METODE
        // ========================================================================

        /// <summary>
        /// Vraća listu dozvola na osnovu profila korisnika
        /// </summary>
        private List<string> DajDozvolePoProfilu(string tipKorisnika)
        {
            List<string> dozvole = new List<string>();
            
            // Sve profile mogu da pregledaju podatke
            dozvole.Add("Pregledaj");
            
            // Samo Predsednik i Potpredsednik mogu kreirati entitete
            if (tipKorisnika == "Predsednik" || tipKorisnika == "Potpredsednik")
            {
                dozvole.Add("KreirajSaziv");
                dozvole.Add("KreirajZasedanje");
                dozvole.Add("KreirajSednicu");
                dozvole.Add("UrediMandate");
            }
            
            // Svi mogu glasati (kada bude implementirano)
            dozvole.Add("Glasaj");
            
            return dozvole;
        }
    }
}
