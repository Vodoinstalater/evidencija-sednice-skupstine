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
    }
}
