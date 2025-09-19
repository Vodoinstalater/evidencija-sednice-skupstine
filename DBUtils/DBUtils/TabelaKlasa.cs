using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data.SqlClient;
using System.Data;


namespace DBUtils
{
    public class TabelaKlasa
    {
        /* CRC: 
          * Responsibility - ODGOVORNOST: Konekcija na celinu baze podataka, SQL server tipa  
          Collaboration - zavisi od standardne klase SQlDataAdapter iz biblioteke System.Data.SqlClient
                          kao i klase Dataset iz standardne biblioteke System.Data*/

        #region Atributi

        private string _nazivTabele;
        private KonekcijaKlasa _konekcijaObjekat;
        private SqlDataAdapter _adapterObjekat;
        private System.Data.DataSet _dataSetObjekat;
        
        #endregion

        #region Konstruktor

        public TabelaKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele)
        {
            _konekcijaObjekat = novaKonekcija;
            _nazivTabele = noviNazivTabele;
        }
        
        #endregion

        #region Privatne metode

        private void KreirajAdapter(string selectUpit, string insertUpit, string deleteUpit, string updateUpit)
        {
            try
            {
                SqlCommand pomSelectKomanda, pomInsertKomanda, pomDeleteKomanda, pomUpdateKomanda;

                // Proveri konekciju pre kreiranja komandi
                SqlConnection konekcija = _konekcijaObjekat.DajKonekciju();

                pomSelectKomanda = new SqlCommand();
                pomSelectKomanda.CommandText = selectUpit;
                pomSelectKomanda.Connection = konekcija;

                pomInsertKomanda = new SqlCommand();
                pomInsertKomanda.CommandText = insertUpit;
                pomInsertKomanda.Connection = konekcija;

                pomDeleteKomanda = new SqlCommand();
                pomDeleteKomanda.CommandText = deleteUpit;
                pomDeleteKomanda.Connection = konekcija;

                pomUpdateKomanda = new SqlCommand();
                pomUpdateKomanda.CommandText = updateUpit;
                pomUpdateKomanda.Connection = konekcija;

                _adapterObjekat = new SqlDataAdapter();
                _adapterObjekat.SelectCommand = pomSelectKomanda;
                _adapterObjekat.InsertCommand = pomInsertKomanda;
                _adapterObjekat.UpdateCommand = pomUpdateKomanda;
                _adapterObjekat.DeleteCommand = pomDeleteKomanda;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void KreirajDataset()
        {
            try
            {
                _dataSetObjekat = new System.Data.DataSet();
                
                _adapterObjekat.Fill(_dataSetObjekat, _nazivTabele);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ZatvoriAdapterDataset()
        {
            _adapterObjekat.Dispose();
            _dataSetObjekat.Dispose();
        }
        
        #endregion

        #region Javne metode

        public SqlConnection DajKonekciju()
        {
            return _konekcijaObjekat.DajKonekciju();
        }

        public DataSet DajPodatke(string selectUpit)
            // izdvaja podatke u odnosu na dat selectupit
        {
            try
            {
                // Proveri da li je konekcija validna
                if (_konekcijaObjekat == null)
                {
                    throw new InvalidOperationException("Konekcija objekat nije inicijalizovan.");
                }

                SqlConnection konekcija = _konekcijaObjekat.DajKonekciju();
                if (konekcija == null)
                {
                    throw new InvalidOperationException("Konekcija nije dostupna.");
                }

                // Proveri stanje konekcije
                if (konekcija.State != System.Data.ConnectionState.Open)
                {
                    throw new InvalidOperationException($"Konekcija nije otvorena. Stanje: {konekcija.State}");
                }

                // POJEDNOSTAVLJENO: Koristi direktan SqlDataAdapter umesto kompleksnog adapter pattern-a
                DataSet rezultat = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(selectUpit, konekcija);
                adapter.Fill(rezultat, _nazivTabele);
                
                return rezultat;
            }
            catch (Exception ex)
            {
                throw; // Re-throw the exception for proper error handling
            }
        }

        public int DajBrojSlogova()
        {
            int BrojSlogova = _dataSetObjekat.Tables[0].Rows.Count; 
            return BrojSlogova;
        }

        public bool IzvrsiAzuriranje(string Upit)
            // izvrzava azuriranje unos/brisanje/izmena u odnosu na dati i upit
        {

            //
            bool uspeh = false;
           SqlConnection pomKonekcija;
           SqlCommand pomKomanda;
           SqlTransaction pomTransakcija = null; 
            try
            {
                pomKonekcija = _konekcijaObjekat.DajKonekciju();
                // aktivan kod  

                // povezivanje
                pomKomanda = new SqlCommand();
                pomKomanda.Connection = pomKonekcija;
                pomKomanda = pomKonekcija.CreateCommand();
                // pokretanje
                // NE TREBA OPEN JER DOBIJAMO OTVORENU KONEKCIJU KROZ KONSTRUKTOR
                // mKonekcija.Open();
                pomTransakcija = pomKonekcija.BeginTransaction();
                pomKomanda.Transaction = pomTransakcija;
                pomKomanda.CommandText = Upit;
                pomKomanda.ExecuteNonQuery();
                pomTransakcija.Commit();
                uspeh = true;
            }
            catch
            {
                pomTransakcija.Rollback();
                uspeh = false;
            }
            return uspeh;
        }

        // druga varijanta - preklapajuca (overload) metoda kada dobijemo vise upita da se izvrsi u transakciji
        public bool IzvrsiAzuriranje(List<string> listaUpita)
        // izvrzava azuriranje unos/brisanje/izmena 
            // moze se dodeliti kao parametar lista od vise upita
            // sada transakcija ima smisla, jer izvrsava vise upita u paketu
        {

            //
            bool uspeh = false;
            SqlConnection pomKonekcija;
            SqlCommand pomKomanda;
            SqlTransaction pomTransakcija = null;
            try
            {
                pomKonekcija = _konekcijaObjekat.DajKonekciju();
                // aktivan kod  

                // povezivanje
                pomKomanda = new SqlCommand();
                pomKomanda.Connection = pomKonekcija;
                pomKomanda = pomKonekcija.CreateCommand();
                // pokretanje
                // NE TREBA OPEN JER DOBIJAMO OTVORENU KONEKCIJU KROZ KONSTRUKTOR
                // mKonekcija.Open();
                string pomUpit="";
                pomTransakcija = pomKonekcija.BeginTransaction();
                pomKomanda.Transaction = pomTransakcija;
                for (int i = 0; i < listaUpita.Count(); i++)
                {
                    pomUpit = listaUpita[i];
                    pomKomanda.CommandText = pomUpit;
                    pomKomanda.ExecuteNonQuery();
                }
                pomTransakcija.Commit();
                uspeh = true;
            }
            catch
            {
                pomTransakcija.Rollback();
                uspeh = false;
            }
            return uspeh;
        }


        #endregion

    }
}
