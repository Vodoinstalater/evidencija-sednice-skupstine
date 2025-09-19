using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace PoslovnaLogika
{
    /// <summary>
    /// Klasa za upravljanje poslovnim pravilima iz XML konfiguracionog fajla
    /// </summary>
    public class BusinessRulesConfigKlasa
    {
        private static BusinessRulesConfigKlasa _instance;
        private static readonly object _lock = new object();
        
        // Poslovna pravila - podrazumevane vrednosti
        public int MinDanaZaSednicu { get; private set; } = 7;
        public int MaxDanaUBuducnosti { get; private set; } = 3650; // 10 godina
        public int MaxGodinaUBuducnosti { get; private set; } = 10; // 10 godina
        public int MinStarost { get; private set; } = 18;
        public int MinDuzinaLozinke { get; private set; } = 6;
        public int MaxDuzinaLozinke { get; private set; } = 50;
        public int MinDanaIzmedjuSaziva { get; private set; } = 1;
        public int MaxDuzinaSaziva { get; private set; } = 1825; // 5 godina
        public int MinDanaUnapred { get; private set; } = 1;
        public int MaxMandataPoLici { get; private set; } = 1;
        public int MinGodinaRodjenja { get; private set; } = 1900;
        public int MinPozicijaId { get; private set; } = 1;
        public int MaxPozicijaId { get; private set; } = 6;
        public List<string> MoguciGlasovi { get; private set; } = new List<string> { "Za", "Protiv", "Uzdržan" };
        
        // Pozicije
        public int PredsednikPozicija { get; private set; } = 2;
        public int PotpredsednikPozicija { get; private set; } = 3;
        public List<int> PoslaniciPozicije { get; private set; } = new List<int> { 1, 4, 5, 6 };
        
        // Akcije
        public string SazoviNovuSednicuAkcija { get; private set; } = "SazoviNovuSednicu";
        public string OtvoriNovoZasedanjeAkcija { get; private set; } = "OtvoriNovoZasedanje";
        public string PogledajSedniceAkcija { get; private set; } = "PogledajSednice";
        public string PogledajZasedanjaAkcija { get; private set; } = "PogledajZasedanja";
        public string PogledajSaziveAkcija { get; private set; } = "PogledajSazive";
        public string PogledajMandateAkcija { get; private set; } = "PogledajMandate";
        public string IstorijaGlasanjaAkcija { get; private set; } = "IstorijaGlasanja";

        private BusinessRulesConfigKlasa()
        {
            UcitajKonfiguraciju();
        }

        /// <summary>
        /// Singleton pattern - vraća instancu konfiguracije
        /// </summary>
        public static BusinessRulesConfigKlasa Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BusinessRulesConfigKlasa();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Ponovo učitava konfiguraciju iz XML fajla
        /// </summary>
        public void ReloadConfig()
        {
            UcitajKonfiguraciju();
        }

        /// <summary>
        /// Učitava konfiguraciju iz XML fajla
        /// </summary>
        private void UcitajKonfiguraciju()
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BusinessRulesConfig.xml");
                
                if (!File.Exists(configPath))
                {
                    // Ako fajl ne postoji, koristi podrazumevane vrednosti
                    return;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(configPath);

                // Učitaj pravila za sednice
                XmlNode sednicaNode = doc.SelectSingleNode("//Sednica");
                if (sednicaNode != null)
                {
                    XmlNode minDanaNode = sednicaNode.SelectSingleNode("MinDanaZaSednicu");
                    if (minDanaNode != null && int.TryParse(minDanaNode.InnerText, out int minDana))
                    {
                        MinDanaZaSednicu = minDana;
                    }

                    XmlNode maxDanaNode = sednicaNode.SelectSingleNode("MaxDanaUBuducnosti");
                    if (maxDanaNode != null && int.TryParse(maxDanaNode.InnerText, out int maxDana))
                    {
                        MaxDanaUBuducnosti = maxDana;
                    }
                }

                // Učitaj pravila za lica
                XmlNode licaNode = doc.SelectSingleNode("//Lica");
                if (licaNode != null)
                {
                    XmlNode minStarostNode = licaNode.SelectSingleNode("MinStarost");
                    if (minStarostNode != null && int.TryParse(minStarostNode.InnerText, out int minStarost))
                    {
                        MinStarost = minStarost;
                    }

                    XmlNode minLozinkaNode = licaNode.SelectSingleNode("MinDuzinaLozinke");
                    if (minLozinkaNode != null && int.TryParse(minLozinkaNode.InnerText, out int minLozinka))
                    {
                        MinDuzinaLozinke = minLozinka;
                    }

                    XmlNode maxLozinkaNode = licaNode.SelectSingleNode("MaxDuzinaLozinke");
                    if (maxLozinkaNode != null && int.TryParse(maxLozinkaNode.InnerText, out int maxLozinka))
                    {
                        MaxDuzinaLozinke = maxLozinka;
                    }
                }

                // Učitaj pravila za sazive
                XmlNode sazivNode = doc.SelectSingleNode("//Saziv");
                if (sazivNode != null)
                {
                    XmlNode minDanaSazivNode = sazivNode.SelectSingleNode("MinDanaIzmedjuSaziva");
                    if (minDanaSazivNode != null && int.TryParse(minDanaSazivNode.InnerText, out int minDanaSaziv))
                    {
                        MinDanaIzmedjuSaziva = minDanaSaziv;
                    }

                    XmlNode maxDuzinaSazivNode = sazivNode.SelectSingleNode("MaxDuzinaSaziva");
                    if (maxDuzinaSazivNode != null && int.TryParse(maxDuzinaSazivNode.InnerText, out int maxDuzinaSaziv))
                    {
                        MaxDuzinaSaziva = maxDuzinaSaziv;
                    }
                }

                // Učitaj pravila za zasedanja
                XmlNode zasedanjeNode = doc.SelectSingleNode("//Zasedanje");
                if (zasedanjeNode != null)
                {
                    XmlNode minDanaZasedanjeNode = zasedanjeNode.SelectSingleNode("MinDanaUnapred");
                    if (minDanaZasedanjeNode != null && int.TryParse(minDanaZasedanjeNode.InnerText, out int minDanaZasedanje))
                    {
                        MinDanaUnapred = minDanaZasedanje;
                    }
                }

                // Učitaj pravila za mandate
                XmlNode mandatNode = doc.SelectSingleNode("//Mandat");
                if (mandatNode != null)
                {
                    XmlNode maxMandataNode = mandatNode.SelectSingleNode("MaxMandataPoLici");
                    if (maxMandataNode != null && int.TryParse(maxMandataNode.InnerText, out int maxMandata))
                    {
                        MaxMandataPoLici = maxMandata;
                    }
                }

                // Učitaj moguće glasove
                XmlNode glasanjeNode = doc.SelectSingleNode("//Glasanje");
                if (glasanjeNode != null)
                {
                    XmlNodeList glasoviNodes = glasanjeNode.SelectNodes("MoguciGlasovi/Glas");
                    if (glasoviNodes != null)
                    {
                        MoguciGlasovi.Clear();
                        foreach (XmlNode glasNode in glasoviNodes)
                        {
                            if (!string.IsNullOrWhiteSpace(glasNode.InnerText))
                            {
                                MoguciGlasovi.Add(glasNode.InnerText.Trim());
                            }
                        }
                    }
                }

                // Učitaj pozicije
                XmlNode pozicijeNode = doc.SelectSingleNode("//Pozicije");
                if (pozicijeNode != null)
                {
                    XmlNode predsednikNode = pozicijeNode.SelectSingleNode("Predsednik");
                    if (predsednikNode != null && int.TryParse(predsednikNode.InnerText, out int predsednik))
                    {
                        PredsednikPozicija = predsednik;
                    }

                    XmlNode potpredsednikNode = pozicijeNode.SelectSingleNode("Potpredsednik");
                    if (potpredsednikNode != null && int.TryParse(potpredsednikNode.InnerText, out int potpredsednik))
                    {
                        PotpredsednikPozicija = potpredsednik;
                    }

                    XmlNode poslaniciNode = pozicijeNode.SelectSingleNode("Poslanici");
                    if (poslaniciNode != null)
                    {
                        string[] poslaniciStr = poslaniciNode.InnerText.Split(',');
                        PoslaniciPozicije.Clear();
                        foreach (string poslanikStr in poslaniciStr)
                        {
                            if (int.TryParse(poslanikStr.Trim(), out int poslanik))
                            {
                                PoslaniciPozicije.Add(poslanik);
                            }
                        }
                    }
                }

                // Učitaj akcije
                XmlNode akcijeNode = doc.SelectSingleNode("//Akcije");
                if (akcijeNode != null)
                {
                    XmlNode sazoviNode = akcijeNode.SelectSingleNode("SazoviNovuSednicu");
                    if (sazoviNode != null && !string.IsNullOrWhiteSpace(sazoviNode.InnerText))
                    {
                        SazoviNovuSednicuAkcija = sazoviNode.InnerText.Trim();
                    }

                    XmlNode otvoriNode = akcijeNode.SelectSingleNode("OtvoriNovoZasedanje");
                    if (otvoriNode != null && !string.IsNullOrWhiteSpace(otvoriNode.InnerText))
                    {
                        OtvoriNovoZasedanjeAkcija = otvoriNode.InnerText.Trim();
                    }

                    XmlNode pogledajSedniceNode = akcijeNode.SelectSingleNode("PogledajSednice");
                    if (pogledajSedniceNode != null && !string.IsNullOrWhiteSpace(pogledajSedniceNode.InnerText))
                    {
                        PogledajSedniceAkcija = pogledajSedniceNode.InnerText.Trim();
                    }

                    XmlNode pogledajZasedanjaNode = akcijeNode.SelectSingleNode("PogledajZasedanja");
                    if (pogledajZasedanjaNode != null && !string.IsNullOrWhiteSpace(pogledajZasedanjaNode.InnerText))
                    {
                        PogledajZasedanjaAkcija = pogledajZasedanjaNode.InnerText.Trim();
                    }

                    XmlNode pogledajSaziveNode = akcijeNode.SelectSingleNode("PogledajSazive");
                    if (pogledajSaziveNode != null && !string.IsNullOrWhiteSpace(pogledajSaziveNode.InnerText))
                    {
                        PogledajSaziveAkcija = pogledajSaziveNode.InnerText.Trim();
                    }

                    XmlNode pogledajMandateNode = akcijeNode.SelectSingleNode("PogledajMandate");
                    if (pogledajMandateNode != null && !string.IsNullOrWhiteSpace(pogledajMandateNode.InnerText))
                    {
                        PogledajMandateAkcija = pogledajMandateNode.InnerText.Trim();
                    }

                    XmlNode istorijaNode = akcijeNode.SelectSingleNode("IstorijaGlasanja");
                    if (istorijaNode != null && !string.IsNullOrWhiteSpace(istorijaNode.InnerText))
                    {
                        IstorijaGlasanjaAkcija = istorijaNode.InnerText.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                // U slučaju greške, koristi podrazumevane vrednosti
            }
        }

        /// <summary>
        /// Ponovo učitava konfiguraciju iz fajla
        /// </summary>
        public void OsveziKonfiguraciju()
        {
            UcitajKonfiguraciju();
        }

        /// <summary>
        /// Vraća string reprezentaciju konfiguracije za debug
        /// </summary>
        public override string ToString()
        {
            return $"MinDanaZaSednicu: {MinDanaZaSednicu}, MinStarost: {MinStarost}, MinDuzinaLozinke: {MinDuzinaLozinke}";
        }
    }
}
