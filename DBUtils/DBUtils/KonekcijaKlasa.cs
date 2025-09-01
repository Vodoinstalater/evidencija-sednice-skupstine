using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data.SqlClient; 

namespace DBUtils
{
    public class KonekcijaKlasa
    {
         /* CRC: 
          * Responsibility - ODGOVORNOST: Konekcija na celinu baze podataka, SQL server tipa  
          Collaboration - zavisi od standardne klase SQlConnection iz biblioteke System.Data.SqlClient*/

        #region Atributi
        private SqlConnection _konekcija;
        //
        private string _putanjaBaze;
        private string _nazivBaze;
        private string _nazivDBMSinstance;
        private string _stringKonekcije;
        #endregion

        #region Konstruktor
        public KonekcijaKlasa(string nazivDBMSInstance, string putanjaBaze, string nazivBaze)
        {
            _putanjaBaze = putanjaBaze;
            _nazivBaze = nazivBaze;
            _nazivDBMSinstance = nazivDBMSInstance;
            _stringKonekcije = "";
        }

        // default konstruktor za Sednica projekat - koristi bazu 'sednica3' i DBMS instancu 'DESKTOP-VANE1TI\SQLEXPRESS'
        public KonekcijaKlasa()
        {
            _putanjaBaze = "";
            _nazivBaze = "sednica3";
            _nazivDBMSinstance = "DESKTOP-VANE1TI\\SQLEXPRESS";
            _stringKonekcije = "";
        }

        // alternativni konstruktor za testiranje različitih server imena
        public KonekcijaKlasa(string serverName, string databaseName)
        {
            _putanjaBaze = "";
            _nazivBaze = databaseName;
            _nazivDBMSinstance = serverName;
            _stringKonekcije = "";
        }

        // metoda za testiranje različitih connection string formata
        public static bool TestirajKonekciju(string serverName, string databaseName)
        {
            try
            {
                string[] connectionFormats = {
                    $"Data Source={serverName};Initial Catalog={databaseName};Integrated Security=True",
                    $"Server={serverName};Database={databaseName};Trusted_Connection=true",
                    $"Data Source={serverName};Database={databaseName};Integrated Security=True;TrustServerCertificate=True",
                    $"Server={serverName};Database={databaseName};Trusted_Connection=true;TrustServerCertificate=True"
                };

                foreach (string connectionString in connectionFormats)
                {
                    try
                    {
                        using (SqlConnection testConn = new SqlConnection(connectionString))
                        {
                            testConn.Open();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
            
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
    
                return false;
            }
        }

        // druga varijanta - preklapajuca (overload) metoda - sa razlicitim parametrima
        public KonekcijaKlasa(string noviStringKonekcije)
        {
            _putanjaBaze = "";
            _nazivBaze = "";
            _nazivDBMSinstance = "";
            _stringKonekcije = noviStringKonekcije;
        }
        #endregion

        #region Privatne metode
        // Metoda je uklonjena jer koristimo GetConnectionStrings() umesto nje
        #endregion

        #region Javne metode
        public bool OtvoriKonekciju()
        {
            try
            {
                if (_konekcija != null)
                {
                    _konekcija.Close();
                    _konekcija.Dispose();
                }
                
                // Koristi samo jednu, najbolju konekciju
                string connectionString = $"Data Source={_nazivDBMSinstance};Initial Catalog={_nazivBaze};Integrated Security=True;Connection Timeout=10;MultipleActiveResultSets=True;TrustServerCertificate=True;Application Name=SednicaApp";
                
                _konekcija = new SqlConnection(connectionString);
                _konekcija.Open();
                
                return true;
            }
            catch (Exception ex)
            {
    
                return false;
            }
        }

        public SqlConnection DajKonekciju()
        {
            // Ako konekcija nije otvorena, otvori je
            if (_konekcija == null || _konekcija.State != System.Data.ConnectionState.Open)
            {
                OtvoriKonekciju();
            }
            
            return _konekcija;
        }

        public void ZatvoriKonekciju()
        {
            _putanjaBaze = "";
            _konekcija.Close();
            _konekcija.Dispose();
        }

        #endregion

    }
}

