using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class UpravljanjeMandatimaKlasa
    {
        private LicaDBKlasa _licaDB;
        private MandatDBKlasa _mandatDB;
        private SazivDBKlasa _sazivDB;
        private KonekcijaKlasa _konekcija;

        public UpravljanjeMandatimaKlasa(KonekcijaKlasa konekcija)
        {
            _konekcija = konekcija;
            _licaDB = new LicaDBKlasa(konekcija, "lica");
            _mandatDB = new MandatDBKlasa(konekcija, "mandat");
            _sazivDB = new SazivDBKlasa(konekcija, "saziv");
        }

        public bool DodajNovoLica(LicaKlasa novaLica)
        {
            try
            {
                // Proveri da li već postoji korisničko ime
                if (_licaDB.PostojiLicaSaKorisnickimImenom(novaLica.Korisnicko_ime))
                {
                    return false; // Korisničko ime već postoji
                }

                // Dodaj novo lice
                return _licaDB.DodajNovoLice(novaLica);
            }
            catch (Exception ex)
            {
                // Log the actual error for debugging
                System.Diagnostics.Debug.WriteLine($"Error in DodajNovoLica: {ex.Message}");
                return false;
            }
        }

        public bool DodajMandat(int id_lica, int id_saziva, int id_stranke)
        {
            try
            {
                // Proveri da li već postoji mandat za ovo lice u ovom sazivu
                if (_mandatDB.PostojiMandat(id_lica, id_saziva))
                {
                    return false; // Mandat već postoji
                }

                // Proveri da li već postoji mandat za ovo lice u ovom sazivu sa istom strankom
                if (_mandatDB.PostojiMandatPoStranci(id_lica, id_saziva, id_stranke))
                {
                    return false; // Mandat sa istom strankom već postoji
                }

                // Kreiraj novi mandat
                MandatKlasa noviMandat = new MandatKlasa();
                noviMandat.Id_lica = id_lica;
                noviMandat.Id_saziva = id_saziva;
                noviMandat.Id_stranke = id_stranke;

                bool result = _mandatDB.DodajNoviMandat(noviMandat);
                if (!result)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to add mandat: liceId={id_lica}, sazivId={id_saziva}, strankaId={id_stranke}");
                }
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in DodajMandat: {ex.Message}");
                return false;
            }
        }

        public bool UkloniMandat(int id_lica, int id_saziva)
        {
            try
            {
                // Pronađi mandat za ovo lice u ovom sazivu
                string upit = "SELECT id_mandata FROM mandat WHERE id_lica = " + id_lica + " AND id_saziva = " + id_saziva;
                DataSet rezultat = _mandatDB.DajPodatke(upit);
                
                if (rezultat.Tables[0].Rows.Count > 0)
                {
                    int id_mandata = Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_mandata"]);
                    return _mandatDB.ObrisiMandat(id_mandata);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool IzmeniMandat(int id_lica, int id_saziva, int novaStranka)
        {
            try
            {
                // Pronađi mandat za ovo lice u ovom sazivu
                string upit = "SELECT id_mandata FROM mandat WHERE id_lica = " + id_lica + " AND id_saziva = " + id_saziva;
                DataSet rezultat = _mandatDB.DajPodatke(upit);
                
                if (rezultat.Tables[0].Rows.Count > 0)
                {
                    int id_mandata = Convert.ToInt32(rezultat.Tables[0].Rows[0]["id_mandata"]);
                    
                    // Proveri da li već postoji mandat sa novom strankom
                    if (_mandatDB.PostojiMandatPoStranci(id_lica, id_saziva, novaStranka))
                    {
                        return false; // Mandat sa novom strankom već postoji
                    }

                    // Izmeni mandat
                    string upitIzmeni = "UPDATE mandat SET id_stranke = " + novaStranka + 
                                       " WHERE id_mandata = " + id_mandata;
                    return _mandatDB.IzvrsiAzuriranje(upitIzmeni);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<LicaKlasa> DajSvaLica()
        {
            try
            {
                return _licaDB.DajSvaLica();
            }
            catch
            {
                // U slučaju greške, vrati praznu listu
                return new List<LicaKlasa>();
            }
        }

        public List<MandatKlasa> DajMandatePoSazivu(int id_saziva)
        {
            List<MandatKlasa> listaMandata = new List<MandatKlasa>();
            try
            {
                DataSet rezultat = _mandatDB.DajMandatePoSazivu(id_saziva);
                foreach (DataRow red in rezultat.Tables[0].Rows)
                {
                    MandatKlasa mandat = new MandatKlasa();
                    mandat.Id_mandata = Convert.ToInt32(red["id_mandata"]);
                    mandat.Id_lica = Convert.ToInt32(red["id_lica"]);
                    mandat.Id_saziva = Convert.ToInt32(red["id_saziva"]);
                    mandat.Id_stranke = Convert.ToInt32(red["id_stranke"]);
                    listaMandata.Add(mandat);
                }
            }
            catch
            {
                // U slučaju greške, vrati praznu listu
            }
            return listaMandata;
        }

        public List<MandatKlasa> DajMandatePoSazivuLista(int id_saziva)
        {
            try
            {
                return _mandatDB.DajMandatePoSazivuLista(id_saziva);
            }
            catch
            {
                return new List<MandatKlasa>();
            }
        }

        public DataSet DajMandateSaDetaljima(int id_saziva)
        {
            try
            {
                string upit = @"SELECT m.id_mandata, m.id_lica, m.id_saziva, m.id_stranke,
                                      l.ime + ' ' + l.prezime as puno_ime, l.pozicija, l.pol,
                                      s.naziv_stranke, sa.ime as saziv_ime
                               FROM mandat m
                               INNER JOIN lica l ON m.id_lica = l.id_lica
                               INNER JOIN stranka s ON m.id_stranke = s.id_stranke
                               INNER JOIN saziv sa ON m.id_saziva = sa.id_saziva
                               WHERE m.id_saziva = " + id_saziva + 
                               " ORDER BY s.naziv_stranke, l.prezime, l.ime";
                
                return _mandatDB.DajPodatke(upit);
            }
            catch
            {
                return new DataSet();
            }
        }

        /// <summary>
        /// Kreira samo novo lice (bez mandata)
        /// </summary>
        public bool KreirajNovoLice(string ime, string prezime, string korisnickoIme, string lozinka, 
            int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, string bio, out string poruka)
        {
            poruka = "";
            try
            {
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
                    Bio = bio
                };

                // Dodaj lice
                if (!DodajNovoLica(novoLice))
                {
                    // Proveri da li je problem sa korisničkim imenom
                    if (_licaDB.PostojiLicaSaKorisnickimImenom(korisnickoIme))
                    {
                        poruka = "Korisničko ime već postoji. Molimo izaberite drugo korisničko ime.";
                    }
                    else
                    {
                        poruka = "Greška pri kreiranju novog lica. Proverite da li su svi podaci validni.";
                    }
                    return false;
                }

                poruka = "Lice je uspešno kreirano. Sada možete dodati mandat ručno.";
                return true;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju lica: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Kreira novo lice i mandat zajedno (za kompatibilnost)
        /// </summary>
        public bool KreirajNovoLiceIMandat(string ime, string prezime, string korisnickoIme, string lozinka, 
            int pozicijaId, int strankaId, char pol, DateTime datumRodjenja, string bio, out string poruka)
        {
            poruka = "";
            try
            {
                // Prvo kreiraj lice
                if (!KreirajNovoLice(ime, prezime, korisnickoIme, lozinka, pozicijaId, strankaId, pol, datumRodjenja, bio, out string licePoruka))
                {
                    poruka = licePoruka;
                    return false;
                }

                // Dohvati ID novog lica
                int liceId = _licaDB.DajNajnovijeLicaId();
                if (liceId == 0)
                {
                    poruka = "Greška pri dohvatanju ID-ja novog lica.";
                    return false;
                }

                // Dohvati aktivan saziv
                UpravljanjeSazivimaKlasa upravljanjeSazivima = new UpravljanjeSazivimaKlasa(_konekcija);
                SazivKlasa aktivanSaziv = upravljanjeSazivima.DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    poruka = "Ne postoji aktivan saziv. Prvo kreirajte novi saziv.";
                    return false;
                }

                // Kreiraj mandat
                if (!DodajMandat(liceId, aktivanSaziv.Id_saziva, strankaId))
                {
                    // Proveri da li mandat već postoji
                    if (_mandatDB.PostojiMandat(liceId, aktivanSaziv.Id_saziva))
                    {
                        poruka = "Mandat za ovo lice već postoji u ovom sazivu.";
                    }
                    else if (_mandatDB.PostojiMandatPoStranci(liceId, aktivanSaziv.Id_saziva, strankaId))
                    {
                        poruka = "Mandat za ovo lice sa istom strankom već postoji u ovom sazivu.";
                    }
                    else
                    {
                        poruka = "Greška pri kreiranju mandata. Proverite da li su svi podaci validni.";
                    }
                    return false;
                }

                poruka = "Lice i mandat su uspešno kreirani.";
                return true;
            }
            catch (Exception ex)
            {
                poruka = $"Greška pri kreiranju lica i mandata: {ex.Message}";
                return false;
            }
        }
    }
}
