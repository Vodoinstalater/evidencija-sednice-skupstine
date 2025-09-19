using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class UpravljanjeSednicamaKlasa
    {
        private SednicaDBKlasa _sednicaDB;
        private DnevniRedDBKlasa _dnevniRedDB;
        private PitanjaDBKlasa _pitanjaDB;
        private ZasedanjeDBKlasa _zasedanjeDB;
        private KonekcijaKlasa _konekcija;

        public UpravljanjeSednicamaKlasa(KonekcijaKlasa konekcija)
        {
            _konekcija = konekcija;
            _sednicaDB = new SednicaDBKlasa(konekcija, "sednica");
            _dnevniRedDB = new DnevniRedDBKlasa(konekcija, "dnevni_red");
            _pitanjaDB = new PitanjaDBKlasa(konekcija, "pitanja");
            _zasedanjeDB = new ZasedanjeDBKlasa(konekcija, "zasedanje");
        }

        public bool KreirajNovuSednicu(SednicaKlasa novaSednica, List<string> listaPitanja)
        {
            try
            {
                // Kreiraj novu sednicu
                if (!_sednicaDB.DodajNovuSednicu(novaSednica))
                {
                    return false;
                }

                // Dohvati ID nove sednice
                int idSednice = _sednicaDB.DajNajnovijuSednicuId();
                if (idSednice == 0)
                {
                    return false;
                }

                // Kreiraj dnevni red za sednicu
                DnevniRedKlasa noviDnevniRed = new DnevniRedKlasa();
                noviDnevniRed.Id_sednice = idSednice;
                
                if (!_dnevniRedDB.DodajNoviDnevniRed(noviDnevniRed))
                {
                    return false;
                }

                // Dohvati ID novog dnevnog reda
                int idDnevnogReda = _dnevniRedDB.DajNajnovijiDnevniRedPoSednici(idSednice);
                if (idDnevnogReda == 0)
                {
                    return false;
                }

                // Dodaj pitanja u dnevni red
                foreach (string tekstPitanja in listaPitanja)
                {
                    if (!string.IsNullOrWhiteSpace(tekstPitanja))
                    {
                        PitanjaKlasa novoPitanja = new PitanjaKlasa();
                        novoPitanja.Id_dnevni_red = idDnevnogReda;
                        novoPitanja.Redni_broj = _pitanjaDB.DajSledeciRedniBroj(idDnevnogReda);
                        novoPitanja.Tekst = tekstPitanja.Trim();

                        if (!_pitanjaDB.DodajNovoPitanja(novoPitanja))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kreira novu sednicu sa automatskim generisanjem glasova za sva pitanja
        /// </summary>
        public bool KreirajNovuSednicuSaAutomatiskimGlasovima(SednicaKlasa novaSednica, List<string> listaPitanja, out string poruka)
        {
            poruka = "";
            try
            {
                // Kreiraj novu sednicu
                if (!_sednicaDB.DodajNovuSednicu(novaSednica))
                {
                    poruka = "Greška pri kreiranju sednice.";
                    return false;
                }

                // Dohvati ID nove sednice
                int idSednice = _sednicaDB.DajNajnovijuSednicuId();
                if (idSednice == 0)
                {
                    poruka = "Greška pri dohvatanju ID-ja sednice.";
                    return false;
                }

                // Kreiraj dnevni red za sednicu
                DnevniRedKlasa noviDnevniRed = new DnevniRedKlasa();
                noviDnevniRed.Id_sednice = idSednice;
                
                if (!_dnevniRedDB.DodajNoviDnevniRed(noviDnevniRed))
                {
                    poruka = "Greška pri kreiranju dnevnog reda.";
                    return false;
                }

                // Dohvati ID novog dnevnog reda
                int idDnevnogReda = _dnevniRedDB.DajNajnovijiDnevniRedPoSednici(idSednice);
                if (idDnevnogReda == 0)
                {
                    poruka = "Greška pri dohvatanju ID-ja dnevnog reda.";
                    return false;
                }

                // Dodaj pitanja u dnevni red i generiši glasove
                int brojPitanja = 0;
                foreach (string tekstPitanja in listaPitanja)
                {
                    if (!string.IsNullOrWhiteSpace(tekstPitanja))
                    {
                        PitanjaKlasa novoPitanja = new PitanjaKlasa();
                        novoPitanja.Id_dnevni_red = idDnevnogReda;
                        novoPitanja.Redni_broj = _pitanjaDB.DajSledeciRedniBroj(idDnevnogReda);
                        novoPitanja.Tekst = tekstPitanja.Trim();

                        if (!_pitanjaDB.DodajNovoPitanja(novoPitanja))
                        {
                            poruka = "Greška pri dodavanju pitanja.";
                            return false;
                        }

                        // Dohvati ID novog pitanja
                        int idPitanja = _pitanjaDB.DajNajnovijePitanjaId();
                        if (idPitanja > 0)
                        {
                            // Generiši automatske glasove za ovo pitanje
                            if (!GenerisiRandomGlasove(idPitanja))
                            {
                                poruka = "Greška pri generisanju glasova za pitanje.";
                                return false;
                            }
                            brojPitanja++;
                        }
                    }
                }

                poruka = $"Sednica je uspešno kreirana sa {brojPitanja} pitanja i automatskim glasovima.";
                return true;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju sednice: {ex.Message}";
                return false;
            }
        }

        public bool KreirajNovuSednicuSaDnevnimRedom(SednicaKlasa novaSednica, DnevniRedKlasa dnevniRed, List<PitanjaKlasa> listaPitanja)
        {
            try
            {
                // Kreiraj novu sednicu
                if (!_sednicaDB.DodajNovuSednicu(novaSednica))
                {
                    return false;
                }

                // Dohvati ID nove sednice
                int idSednice = _sednicaDB.DajNajnovijuSednicuId();
                if (idSednice == 0)
                {
                    return false;
                }

                // Kreiraj dnevni red za sednicu
                dnevniRed.Id_sednice = idSednice;
                
                if (!_dnevniRedDB.DodajNoviDnevniRed(dnevniRed))
                {
                    return false;
                }

                // Dohvati ID novog dnevnog reda
                int idDnevnogReda = _dnevniRedDB.DajNajnovijiDnevniRedPoSednici(idSednice);
                if (idDnevnogReda == 0)
                {
                    return false;
                }

                // Dodaj pitanja u dnevni red
                foreach (PitanjaKlasa pitanja in listaPitanja)
                {
                    if (!string.IsNullOrWhiteSpace(pitanja.Tekst))
                    {
                        pitanja.Id_dnevni_red = idDnevnogReda;
                        pitanja.Redni_broj = _pitanjaDB.DajSledeciRedniBroj(idDnevnogReda);

                        if (!_pitanjaDB.DodajNovoPitanja(pitanja))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DodajPitanjaUSednicu(int idSednice, List<string> listaPitanja)
        {
            try
            {
                // Dohvati dnevni red za sednicu
                DataSet dnevniRed = _dnevniRedDB.DajDnevniRedPoSednici(idSednice);
                if (dnevniRed.Tables[0].Rows.Count == 0)
                {
                    return false;
                }

                int idDnevnogReda = Convert.ToInt32(dnevniRed.Tables[0].Rows[0]["id_dnevni_red"]);

                // Dodaj nova pitanja
                foreach (string tekstPitanja in listaPitanja)
                {
                    if (!string.IsNullOrWhiteSpace(tekstPitanja))
                    {
                        PitanjaKlasa novoPitanja = new PitanjaKlasa();
                        novoPitanja.Id_dnevni_red = idDnevnogReda;
                        novoPitanja.Redni_broj = _pitanjaDB.DajSledeciRedniBroj(idDnevnogReda);
                        novoPitanja.Tekst = tekstPitanja.Trim();

                        if (!_pitanjaDB.DodajNovoPitanja(novoPitanja))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ObrisiPitanjeIzSednice(int idSednice, int redniBrojPitanja)
        {
            try
            {
                // Dohvati dnevni red za sednicu
                DataSet dnevniRed = _dnevniRedDB.DajDnevniRedPoSednici(idSednice);
                if (dnevniRed.Tables[0].Rows.Count == 0)
                {
                    return false;
                }

                int idDnevnogReda = Convert.ToInt32(dnevniRed.Tables[0].Rows[0]["id_dnevni_red"]);

                // Pronađi pitanja sa tim rednim brojem
                string upit = "SELECT id_pitanja FROM pitanja WHERE id_dnevni_red = " + idDnevnogReda + 
                             " AND redni_broj = " + redniBrojPitanja;
                DataSet rezultat = _pitanjaDB.DajPodatke(upit);
                
                if (rezultat.Tables[0].Rows.Count > 0)
                {
                    int idPitanja = Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_pitanja"]);
                    return _pitanjaDB.ObrisiPitanja(idPitanja);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool IzmeniPitanjeUSednici(int idSednice, int redniBrojPitanja, string noviTekst)
        {
            try
            {
                // Dohvati dnevni red za sednicu
                DataSet dnevniRed = _dnevniRedDB.DajDnevniRedPoSednici(idSednice);
                if (dnevniRed.Tables[0].Rows.Count == 0)
                {
                    return false;
                }

                int idDnevnogReda = Convert.ToInt32(dnevniRed.Tables[0].Rows[0]["id_dnevni_red"]);

                // Pronađi pitanja sa tim rednim brojem
                string upit = "SELECT id_pitanja FROM pitanja WHERE id_dnevni_red = " + idDnevnogReda + 
                             " AND redni_broj = " + redniBrojPitanja;
                DataSet rezultat = _pitanjaDB.DajPodatke(upit);
                
                if (rezultat.Tables[0].Rows.Count > 0)
                {
                    int idPitanja = Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_pitanja"]);
                    
                    // Izmeni pitanja
                    string upitIzmeni = "UPDATE pitanja SET tekst = '" + noviTekst + 
                                       "' WHERE id_pitanja = " + idPitanja;
                    return _pitanjaDB.IzvrsiAzuriranje(upitIzmeni);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public DataSet DajSednicuSaPitanjima(int idSednice)
        {
            try
            {
                string upit = @"SELECT s.id_sednice, s.naziv, s.datum, s.opis, s.zasedanje_id,
                                      dr.id_dnevni_red, p.id_pitanja, p.redni_broj, p.tekst,
                                      z.naziv_zasedanja, sa.ime as saziv_ime
                               FROM sednica s
                               INNER JOIN dnevni_red dr ON s.id_sednice = dr.id_sednice
                               LEFT JOIN pitanja p ON dr.id_dnevni_red = p.id_dnevni_red
                               LEFT JOIN zasedanje z ON s.zasedanje_id = z.id_zasedanja
                               LEFT JOIN saziv sa ON z.id_saziv = sa.id_saziva
                               WHERE s.id_sednice = " + idSednice + 
                               " ORDER BY p.redni_broj";
                
                return _sednicaDB.DajPodatke(upit);
            }
            catch
            {
                return new DataSet();
            }
        }

        public List<string> DajPitanjaSednice(int idSednice)
        {
            List<string> listaPitanja = new List<string>();
            try
            {
                DataSet rezultat = DajSednicuSaPitanjima(idSednice);
                foreach (DataRow red in rezultat.Tables[0].Rows)
                {
                    if (red["tekst"] != DBNull.Value)
                    {
                        listaPitanja.Add(red["tekst"].ToString());
                    }
                }
            }
            catch
            {
                // U slučaju greške, vrati praznu listu
            }
            return listaPitanja;
        }

        /// <summary>
        /// Kreira novu sednicu sa validacijom poslovnih pravila
        /// </summary>
        public bool KreirajNovuSednicuSaValidacijom(string nazivSednice, DateTime datumSednice, string opisSednice, 
            List<string> pitanja, out string poruka)
        {
            poruka = "";
            try
            {
                // Dohvati aktivan saziv i najnovije zasedanje
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(_konekcija);
                UpravljanjeZasedanjimaKlasa upravljanjeZasedanjima = new UpravljanjeZasedanjimaKlasa(_konekcija);

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

                // Kreiraj sednicu
                SednicaKlasa novaSednica = new SednicaKlasa
                {
                    Naziv = nazivSednice,
                    Datum = datumSednice,
                    Opis = opisSednice,
                    Zasedanje_id = najnovijeZasedanje.Id_zasedanja
                };

                // Koristi metodu za kreiranje sednice sa automatskim glasovima
                return KreirajNovuSednicuSaAutomatiskimGlasovima(novaSednica, pitanja, out poruka);
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju sednice: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Generiše random glasove za pitanje - SAMO ZA TESTIRANJE
        /// </summary>
        public bool GenerisiRandomGlasove(int pitanjeId)
        {
            try
            {
                // Dohvati aktivan saziv
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(_konekcija);
                SazivKlasa aktivanSaziv = upravljanjeSazivima.DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    return false; // Nema aktivnog saziva
                }

                // Dohvati sva lica sa mandatima u aktivnom sazivu
                UpravljanjeMandatimaKlasa upravljanjeMandatima = new UpravljanjeMandatimaKlasa(_konekcija);
                List<MandatKlasa> mandati = upravljanjeMandatima.DajMandatePoSazivuLista(aktivanSaziv.Id_saziva);
                
                if (mandati.Count == 0)
                {
                    return false; // Nema mandata za glasanje
                }

                // Generiši random glasove
                Random random = new Random();
                string[] moguciGlasovi = { "ZA", "PROTIV", "SUZDRŽAN" };
                
                foreach (MandatKlasa mandat in mandati)
                {
                    int idLica = mandat.Id_lica;
                    string randomGlas = moguciGlasovi[random.Next(moguciGlasovi.Length)];
                    
                    // Kreiraj glasanje
                    GlasanjeKlasa novoGlasanje = new GlasanjeKlasa
                    {
                        IdPitanja = pitanjeId,
                        IdLica = idLica,
                        Glas = randomGlas
                    };
                    
                    // Dodaj glasanje u bazu
                    GlasanjeDBKlasa glasanjeDB = new GlasanjeDBKlasa(_konekcija, "glasanje");
                    glasanjeDB.DodajNovoGlasanje(novoGlasanje);
                }
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
