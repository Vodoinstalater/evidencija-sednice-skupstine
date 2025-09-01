using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
//
using DBUtils;

namespace KorisnickiInterfejs
{
    public class Global : System.Web.HttpApplication
    {
        // ATRIBUTI - GLOBALNE PROMENLJIVE ZA CELU APLIKACIJU
        public static KonekcijaKlasa otvorenaKonekcija;
        public static bool uspehKonekcije;

        // NASE PROCEDURE
        public static bool OtvoriKonekcijuDoBazePodataka()
        // ovde se procedura OtvoriKonekciju zove isto kao i metoda klase konekcija
        // to je dozvoljeno
        {
            try
            {
                // Koristi default konekciju iz DBUtils (sednica3 na DESKTOP-VANE1TI\SQLEXPRESS)
                
                // Prvo testiraj različite connection string formate
                if (KonekcijaKlasa.TestirajKonekciju("DESKTOP-VANE1TI\\SQLEXPRESS", "sednica3"))
                {
                    // Test konekcije uspešan
                }
                else
                {
                    // Test konekcije neuspešan
                }
                
                // Prvo pokušaj sa default server imenom
                otvorenaKonekcija = new KonekcijaKlasa();
                
                uspehKonekcije = otvorenaKonekcija.OtvoriKonekciju();
                
                if (uspehKonekcije)
                {
                    return true;
                }
                
                // Ako ne uspe, pokušaj sa alternativnim server imenima
                
                string[] alternativeServers = {
                    "DESKTOP-VANE1TI",
                    "localhost\\SQLEXPRESS",
                    ".\\SQLEXPRESS",
                    "(local)\\SQLEXPRESS"
                };
                
                foreach (string server in alternativeServers)
                {
                    otvorenaKonekcija = new KonekcijaKlasa(server, "sednica3");
                    uspehKonekcije = otvorenaKonekcija.OtvoriKonekciju();
                    
                    if (uspehKonekcije)
                    {
                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                // Log grešku i vrati false
                uspehKonekcije = false;
                return false;
            }
        }


        public void ZatvoriKonekciju()
        {
            otvorenaKonekcija.ZatvoriKonekciju();
        }

        // DOGADJAJI - GLOBALNI ZA CELU APLIKACIJU
        void Application_Start(object sender, EventArgs e)
        {
            uspehKonekcije = OtvoriKonekcijuDoBazePodataka();

        }

        void Application_End(object sender, EventArgs e)
        {
            ZatvoriKonekciju();

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
