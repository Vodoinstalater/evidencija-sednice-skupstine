using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
//
using System.Data;
using PoslovnaLogika;
using KlasePodataka;

namespace KadrovskiPodaci
{
    /// <summary>
    /// SOAP Web servis za Evidenciju sednica skupstine Srbije
    /// Omogućava spoljnim sistemima pristup podacima o sednicama, sazivima, i glasanjima
    /// </summary>
    [WebService(Namespace = "http://skupstina.gov.rs/sednice/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SednicaWebServis : System.Web.Services.WebService
    {
        private SednicaPoslovnaLogikaKlasa _poslovnaLogika;
        private string _stringKonekcije;

        public SednicaWebServis()
        {
            // Koristi default konekciju za dnevnik3 bazu
            _stringKonekcije = "";
            _poslovnaLogika = new SednicaPoslovnaLogikaKlasa(_stringKonekcije);
        }

        /// <summary>
        /// Vraća sve sednice u XML formatu za spoljne sisteme
        /// </summary>
        [WebMethod]
        public DataSet DajSveSednice()
        {
            try
            {
                return _poslovnaLogika.DajSveSednice();
            }
            catch (Exception ex)
            {
                // Log greška i vrati prazan DataSet
                DataSet prazan = new DataSet();
                prazan.Tables.Add("Greska");
                prazan.Tables["Greska"].Columns.Add("Poruka");
                prazan.Tables["Greska"].Rows.Add($"Greška pri dohvatanju sednica: {ex.Message}");
                return prazan;
            }
        }

        /// <summary>
        /// Vraća sednice filtrirane po sazivu i zasedanju
        /// </summary>
        [WebMethod]
        public DataSet DajSednicePoFilteru(int sazivId, int zasedanjeId)
        {
            try
            {
                return _poslovnaLogika.DajSveSednice(sazivId, zasedanjeId);
            }
            catch (Exception ex)
            {
                DataSet prazan = new DataSet();
                prazan.Tables.Add("Greska");
                prazan.Tables["Greska"].Columns.Add("Poruka");
                prazan.Tables["Greska"].Rows.Add($"Greška pri dohvatanju sednica: {ex.Message}");
                return prazan;
            }
        }

        /// <summary>
        /// Vraća sve sazive za spoljne sisteme
        /// </summary>
        [WebMethod]
        public string DajSveSaziveXML()
        {
            try
            {
                List<SazivKlasa> sazivi = _poslovnaLogika.DajSveSazive();
                
                // Konvertuj u XML string
                System.Text.StringBuilder xml = new System.Text.StringBuilder();
                xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xml.Append("<Sazivi>");
                
                foreach (SazivKlasa saziv in sazivi)
                {
                    xml.Append("<Saziv>");
                    xml.AppendFormat("<Id>{0}</Id>", saziv.Id_saziva);
                    xml.AppendFormat("<Ime>{0}</Ime>", saziv.Ime);
                    xml.AppendFormat("<Pocetak>{0:yyyy-MM-dd}</Pocetak>", saziv.Pocetak);
                    xml.AppendFormat("<Kraj>{0:yyyy-MM-dd}</Kraj>", saziv.Kraj);
                    xml.AppendFormat("<Opis>{0}</Opis>", saziv.Opis);
                    xml.Append("</Saziv>");
                }
                
                xml.Append("</Sazivi>");
                return xml.ToString();
            }
            catch (Exception ex)
            {
                return $"<Greska><Poruka>Greška pri dohvatanju saziva: {ex.Message}</Poruka></Greska>";
            }
        }

        /// <summary>
        /// Vraća sve mandate sa detaljima
        /// </summary>
        [WebMethod]
        public DataSet DajSveMandate()
        {
            try
            {
                return _poslovnaLogika.DajSveMandate();
            }
            catch (Exception ex)
            {
                DataSet prazan = new DataSet();
                prazan.Tables.Add("Greska");
                prazan.Tables["Greska"].Columns.Add("Poruka");
                prazan.Tables["Greska"].Rows.Add($"Greška pri dohvatanju mandata: {ex.Message}");
                return prazan;
            }
        }

        /// <summary>
        /// Vraća istoriju glasanja za javni pristup
        /// </summary>
        [WebMethod]
        public DataSet DajIstorijuGlasanja()
        {
            try
            {
                return _poslovnaLogika.DajIstorijuGlasanja();
            }
            catch (Exception ex)
            {
                DataSet prazan = new DataSet();
                prazan.Tables.Add("Greska");
                prazan.Tables["Greska"].Columns.Add("Poruka");
                prazan.Tables["Greska"].Rows.Add($"Greška pri dohvatanju istorije glasanja: {ex.Message}");
                return prazan;
            }
        }

        /// <summary>
        /// Vraća aktivan saziv u XML formatu
        /// </summary>
        [WebMethod]
        public string DajAktivanSazivXML()
        {
            try
            {
                SazivKlasa aktivanSaziv = _poslovnaLogika.DajAktivanSaziv();
                if (aktivanSaziv == null)
                {
                    return "<AktivanSaziv><Poruka>Nema aktivnog saziva</Poruka></AktivanSaziv>";
                }

                System.Text.StringBuilder xml = new System.Text.StringBuilder();
                xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xml.Append("<AktivanSaziv>");
                xml.AppendFormat("<Id>{0}</Id>", aktivanSaziv.Id_saziva);
                xml.AppendFormat("<Ime>{0}</Ime>", aktivanSaziv.Ime);
                xml.AppendFormat("<Pocetak>{0:yyyy-MM-dd}</Pocetak>", aktivanSaziv.Pocetak);
                xml.AppendFormat("<Kraj>{0:yyyy-MM-dd}</Kraj>", aktivanSaziv.Kraj);
                xml.Append("</AktivanSaziv>");
                
                return xml.ToString();
            }
            catch (Exception ex)
            {
                return $"<Greska><Poruka>Greška pri dohvatanju aktivnog saziva: {ex.Message}</Poruka></Greska>";
            }
        }

        /// <summary>
        /// Provera zdravlja servisa
        /// </summary>
        [WebMethod]
        public string ProveriServis()
        {
            return $"Sednica Web Servis je aktivan. Vreme: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }
    }
}