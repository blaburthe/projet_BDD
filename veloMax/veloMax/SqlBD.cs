using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Data;



namespace veloMax
{
    class SqlBD
    {
        MySqlConnection connexion;


        public SqlBD() { }


        /// <summary>
        /// Connexion à la base de donnée
        /// </summary>
        public void Connexion()
        {
            #region Ouverture de connexion

            MySqlConnection maConnexion = null;
            try
            {
                string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=veloMax;" +
                                         "UID=root;PASSWORD=AurelieLEDUC2021";

                maConnexion = new MySqlConnection(connexionString);
                maConnexion.Open();
                connexion = maConnexion;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
            }
            #endregion
        }

        public List<List<string>> Requete(string requete)
        {
            MySqlCommand commande = connexion.CreateCommand();
            commande.CommandText = requete;
            MySqlDataReader reader = commande.ExecuteReader();

            List<List<string>> data = new List<List<string>> { };

            int rows = reader.FieldCount;

            while (reader.Read())
            {
                List<string> line = new List<string> { };
                foreach (string val in reader)
                {
                    line.Add(val);
                }
                data.Add(line);
            }
            return data;
        }

        /// <summary>
        /// Renvoie le résultat d'une requête attendant une seule valeur en retour
        /// </summary>
        /// <param name="requete"></param>
        /// <returns></returns>
        public static string SingleValueRequest(MySqlConnection connexion, string requete)
        {

            string data = "";

            try
            {
                connexion.Open();
                MySqlCommand commande = connexion.CreateCommand();
                commande.CommandText = requete;
                MySqlDataReader reader = commande.ExecuteReader();

                reader.Read();
                data = Convert.ToString(reader[0]);
                connexion.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return data;
        }

        /// <summary>
        /// Permet de charger les donnée d'une table sql à l'intérieur d'un objet ListView
        /// </summary>
        /// <param name="request">requête pour obtenir la table à charger</param>
        /// <param name="bindingSourceName">nom du chemin vers ces données défini pour le binding (itemsSource = {Binding path=bindingSourceName})</param>
        /// <param name="target">objet ListView dans lequel charger ces données</param>
        public static void LoadData(MySqlConnection connexion, string request, string bindingSourceName, ListView target)
        {
            try
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand(request, connexion);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                adp.Fill(ds, bindingSourceName);
                target.DataContext = ds;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            connexion.Close();
        }

        /// <summary>
        /// Envoie une commande sql n'attendant aucun retour (ex : insert info)
        /// </summary>
        /// <param name="connexion"></param>
        /// <param name="request"></param>
        public static void NoAnswerRequest(MySqlConnection connexion, string request)
        {
            try
            {
                connexion.Open();
                MySqlCommand command = connexion.CreateCommand();
                command.CommandText = request;
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            connexion.Close();

        }
    }
}
