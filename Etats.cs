using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace EtatCommandes
{
    class Etats
    {

        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;


        private MySqlConnection OpenConnection()
        {
            try
            {
                server = "localhost";
                database = "northwind";
                uid = "root";
                password = " ";
                string connectionString;
                connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                connection = new MySqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Connexion DB");

             
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur DB");
            }

            return connection;

        }
        public void Select()
        {
            string query = "SELECT * FROM tableinfo";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id"] + "");
                    list[1].Add(dataReader["name"] + "");
                    list[2].Add(dataReader["age"] + "");
                }

                //close Data Reader
                dataReader.Close();

                
            }
            static void Main(string[] args)
            {
            MySqlDataReader dataReader;
            Etats etats = new Etats();

            try
            {
                etats.connection = etats.OpenConnection();
                Console.Write("Entrez un code client SVP 5 Car. : ");
                String customerID = Console.ReadLine();

                MySqlCommand dataCommand = new MySqlCommand();

                dataCommand.Connection = etats.connection;
                dataCommand.CommandText = "Select OrderID,OrderDate,ShippedDate,Shipname,ShipAddress,ShipCity,ShipCountry " + "From Orders Where CustomerID = @CustomerIdParam";

                MySqlParameter param = new MySqlParameter("@CustomerIdParam", MySqlDbType.VarChar, 5);
                param.Value = customerID;
                dataCommand.Parameters.Add(param);
                Console.WriteLine("Prêt à exécuter la requête : {0}\n\n", dataCommand.CommandText);
                dataReader = dataCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    int orderID = dataReader.GetInt32(0);
                    DateTime dateCommande = dataReader.GetDateTime(1);
                    if (dataReader.IsDBNull(2))
                        Console.WriteLine("Commande {0} non encore livree", orderID);
                    else
                    {
                        DateTime dateLivraison = dataReader.GetDateTime(2);
                        String nomLivraison = dataReader.GetString(3);
                        String adrLivraison = dataReader.GetString(4);
                        String villeLivraison = dataReader.GetString(5);
                        String paysLivraison = dataReader.GetString(6);

                        Console.WriteLine("Commande: {0}\nPlacee le: {1}\nLivree le: {2}\n  l’Adresse: {3}\n{4}\n{5}\n{6}\n\n", orderID, dateCommande, dateLivraison, nomLivraison, adrLivraison, villeLivraison, paysLivraison);

                    }

                }
                

            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur accès à la Base Northwind :  {0}  ", e.Message);

            }
            finally
            {
                etats.connection.Close();

            }
            Console.ReadLine();
        }
    }
}
