using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//
using KlasePodataka;

namespace KlaseMapiranja
{
    // ========================================================================
    // DTO KLASE (Data Transfer Objects) za Layer 3
    // ========================================================================

    /// <summary>
    /// DTO za prikaz sednice u UI
    /// </summary>
    public class SednicaDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public DateTime? Datum { get; set; }
        public string DatumFormatiran { get; set; }
        public string Opis { get; set; }
        public string NazivZasedanja { get; set; }
        public string NazivSaziva { get; set; }
        public string StatusSednice { get; set; }
        public int? BrojPitanja { get; set; }
    }

    /// <summary>
    /// DTO za prikaz saziva u UI
    /// </summary>
    public class SazivDTO
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public DateTime? DatumPocetka { get; set; }
        public DateTime? DatumZavrsetka { get; set; }
        public string PeriodFormatiran { get; set; }
        public string Opis { get; set; }
        public bool Aktivan { get; set; }
        public int BrojZasedanja { get; set; }
        public int BrojSednica { get; set; }
        public int BrojMandata { get; set; }
    }

    /// <summary>
    /// DTO za prikaz zasedanja u UI
    /// </summary>
    public class ZasedanjeDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string TipZasedanja { get; set; }
        public string NazivSaziva { get; set; }
        public DateTime? DatumPocetka { get; set; }
        public DateTime? DatumZavrsetka { get; set; }
        public string Status { get; set; }
        public string Opis { get; set; }
        public int BrojSednica { get; set; }
    }

    /// <summary>
    /// DTO za mandate
    /// </summary>
    public class MandatDTO
    {
        public int Id { get; set; }
        public int IdLica { get; set; }
        public int IdSaziva { get; set; }
        public int IdStranke { get; set; }
        
        // Dodatne informacije za prikaz (ne postoje u bazi, ali se koriste za UI)
        public string ImeLica { get; set; }
        public string PrezimeLica { get; set; }
        public string NazivStranke { get; set; }
        public string NazivPozicije { get; set; }
        public string NazivSaziva { get; set; }
    }

    /// <summary>
    /// DTO za prikaz glasanja u UI
    /// </summary>
    public class GlasanjeDTO
    {
        public int IdGlasanja { get; set; }
        public int IdPitanja { get; set; }
        public string TekstPitanja { get; set; }
        public string NazivSednice { get; set; }
        public DateTime DatumGlasanja { get; set; }
        public string Glas { get; set; }
        public string ImePrezime { get; set; }
    }

    /// <summary>
    /// DTO za lica (people)
    /// </summary>
    public class LiceDTO
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string KorisnickoIme { get; set; }
        public string Pozicija { get; set; }
        public string Stranka { get; set; }
        public char Pol { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Biografija { get; set; }
    }

    /// <summary>
    /// DTO za stranke
    /// </summary>
    public class StrankaDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
    }

    /// <summary>
    /// DTO za pozicije
    /// </summary>
    public class PozicijaDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
    }

    /// <summary>
    /// DTO za rezultate autentifikacije
    /// </summary>
    public class KorisnikDTO
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string ImePrezime { get; set; }
        public string TipKorisnika { get; set; }
        public List<string> Dozvole { get; set; }
        public DateTime VremePoslednjePrijave { get; set; }
    }

    /// <summary>
    /// DTO za servise rezultate (standardni odgovor)
    /// </summary>
    public class ServiceResult<T>
    {
        public bool Uspesno { get; set; }
        public string Poruka { get; set; }
        public T Podaci { get; set; }
        public List<string> Greske { get; set; }

        public ServiceResult()
        {
            Greske = new List<string>();
        }
    }

    // ========================================================================
    // MAPER KLASE (konverzije između slojeva)
    // ========================================================================

    /// <summary>
    /// Glavna maper klasa za konverzije između entity objekata i DTO objekata
    /// </summary>
    public class SednicaMaperKlasa
    {
        // atributi
        private string _stringKonekcije;

        // konstruktor
        public SednicaMaperKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        /// <summary>
        /// Konvertuje SazivKlasa u SazivDTO za UI
        /// </summary>
        public SazivDTO KonvertujSazivUDTO(SazivKlasa saziv)
        {
            if (saziv == null) return null;

            return new SazivDTO
            {
                Id = saziv.Id_saziva,
                Ime = saziv.Ime ?? "",
                DatumPocetka = saziv.Pocetak,
                DatumZavrsetka = saziv.Kraj,
                PeriodFormatiran = $"{saziv.Pocetak:dd.MM.yyyy} - {saziv.Kraj:dd.MM.yyyy}",
                Opis = saziv.Opis ?? "",
                Aktivan = saziv.Kraj > DateTime.Now
            };
        }

        /// <summary>
        /// Konvertuje listu saziva u DTO listu
        /// </summary>
        public List<SazivDTO> KonvertujSazive(List<SazivKlasa> sazivi)
        {
            if (sazivi == null) return new List<SazivDTO>();

            return sazivi.Select(s => KonvertujSazivUDTO(s)).ToList();
        }


        /// <summary>
        /// Konvertuje DataSet sa sednicama u DTO listu
        /// </summary>
        public List<SednicaDTO> KonvertujSedniceIzDataSet(DataSet dsSednice)
        {
            List<SednicaDTO> sednice = new List<SednicaDTO>();

            if (dsSednice?.Tables?.Count > 0)
            {
                var tabela = dsSednice.Tables[0];
                
                if (tabela.Rows.Count > 0)
                {
                    foreach (DataRow row in tabela.Rows)
                    {
                        try
                        {
                            DateTime datumSednice = Convert.ToDateTime(row["sednica_datum"]);
                            sednice.Add(new SednicaDTO
                            {
                                Id = Convert.ToInt32(row["id_sednice"]),
                                Naziv = row["sednica_naziv"].ToString(),
                                Datum = datumSednice,
                                DatumFormatiran = datumSednice.ToString("dd.MM.yyyy"),
                                Opis = row["sednica_opis"]?.ToString() ?? "N/A",
                                NazivZasedanja = row["naziv_zasedanja"].ToString(),
                                NazivSaziva = row["saziv_Ime"].ToString(),
                                StatusSednice = DajStatusSednice(datumSednice),
                                BrojPitanja = 0 // Broj pitanja nije u stored procedure, postavljamo default vrednost
                            });
                        }
                        catch (Exception)
                        {
                            // Nastavi sa sledećim redom
                        }
                    }
                }
            }

            return sednice;
        }

        /// <summary>
        /// Konvertuje DataSet sa mandatima u DTO listu
        /// </summary>
        public List<MandatDTO> KonvertujMandateIzDataSet(DataSet dsMandate)
        {
            List<MandatDTO> mandate = new List<MandatDTO>();

            if (dsMandate?.Tables?.Count > 0 && dsMandate.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsMandate.Tables[0].Rows)
                {
                    mandate.Add(new MandatDTO
                    {
                        Id = Convert.ToInt32(row["id_mandata"]),
                        IdLica = Convert.ToInt32(row["id_lica"]),
                        IdSaziva = Convert.ToInt32(row["id_saziva"]),
                        IdStranke = Convert.ToInt32(row["id_stranke"]),
                        ImeLica = row["ime"]?.ToString() ?? "",
                        PrezimeLica = row["prezime"]?.ToString() ?? "",
                        NazivStranke = row["naziv_stranke"]?.ToString() ?? "",
                        NazivPozicije = row["naziv_pozicije"]?.ToString() ?? "Poslanik",
                        NazivSaziva = row["saziv_ime"]?.ToString() ?? ""
                    });
                }
            }

            return mandate;
        }

        /// <summary>
        /// Konvertuje DataSet sa glasanjima u DTO listu
        /// </summary>
        public List<GlasanjeDTO> KonvertujGlasanjaIzDataSet(DataSet dsGlasanja)
        {
            List<GlasanjeDTO> glasanja = new List<GlasanjeDTO>();

            if (dsGlasanja?.Tables?.Count > 0 && dsGlasanja.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsGlasanja.Tables[0].Rows)
                {
                    glasanja.Add(new GlasanjeDTO
                    {
                        IdGlasanja = Convert.ToInt32(row["id_glasanja"]),
                        IdPitanja = Convert.ToInt32(row["id_pitanja"]),
                        TekstPitanja = row["pitanje"].ToString(),
                        NazivSednice = row["sednica_Naziv"].ToString(),
                        DatumGlasanja = Convert.ToDateTime(row["sednica_Datum"]),
                        Glas = row["glas"].ToString(),
                        ImePrezime = row["ime_prezime"].ToString()
                    });
                }
            }

            return glasanja;
        }

        /// <summary>
        /// Formatira tip korisnika na osnovu pozicije
        /// </summary>
        public string FormatiraTipKorisnika(int pozicijaId)
        {
            switch (pozicijaId)
            {
                case 1:
                case 4:
                case 5:
                case 6:
                    return "Poslanik";
                case 2:
                    return "Predsednik";
                case 3:
                    return "Potpredsednik";
                default:
                    return "Nepoznato";
            }
        }

        /// <summary>
        /// Određuje status sednice na osnovu datuma
        /// </summary>
        private string DajStatusSednice(DateTime datumSednice)
        {
            DateTime danas = DateTime.Today;
            
            if (datumSednice.Date < danas)
                return "Završena";
            else if (datumSednice.Date == danas)
                return "Aktuelna";
            else
                return "Planirana";
        }

        /// <summary>
        /// Kreira ServiceResult sa greškama
        /// </summary>
        public ServiceResult<T> KreirajGresku<T>(string poruka, Exception ex = null)
        {
            var result = new ServiceResult<T>
            {
                Uspesno = false,
                Poruka = poruka
            };

            if (ex != null)
            {
                result.Greske.Add(ex.Message);
                if (ex.InnerException != null)
                    result.Greske.Add(ex.InnerException.Message);
            }

            return result;
        }

        /// <summary>
        /// Kreira uspešan ServiceResult
        /// </summary>
        public ServiceResult<T> KreirajUspeh<T>(T podaci, string poruka = "Operacija je uspešno izvršena")
        {
            return new ServiceResult<T>
            {
                Uspesno = true,
                Poruka = poruka,
                Podaci = podaci
            };
        }
    }
}
