using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Data;
using MySql.Data.MySqlClient;

namespace veloMax
{
    /// <summary>
    /// Logique d'interaction pour Fournisseurs.xaml
    /// </summary>
    public partial class Fournisseurs : Window
    {
        MySqlConnection connexion;

        public Fournisseurs(MySqlConnection connexion)
        {
            this.connexion = connexion;
            InitializeComponent();
            SqlBD.LoadData(connexion, "SELECT * FROM fournisseur NATURAL JOIN adresse", "LoadDataBinding", lvFournisseurs);
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void OuvrirInfoFournisseur(object sender, RoutedEventArgs e)
        {
            if (lvFournisseurs.SelectedItem != null)
            {
                DataRowView dataTable = (DataRowView)lvFournisseurs.SelectedItem;
                
                InfoFournisseur window = new InfoFournisseur(connexion, dataTable["nom_F"].ToString(), dataTable["contact_F"].ToString(), dataTable["siret"].ToString(), dataTable["libelle"].ToString(),
                        dataTable["numeroRue"].ToString(), dataTable["rue"].ToString(), dataTable["codeP"].ToString(), dataTable["ville"].ToString());

                window.Show();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un fournisseur à modifier");
            }
        }

        private void OuvrirNouveauFournisseur(object sender, RoutedEventArgs e)
        {
            NouveauFournisseur window = new NouveauFournisseur(connexion);
            window.Show();
        }

        private void SupprimerFournisseur(object sender, RoutedEventArgs e)
        {
            if(lvFournisseurs.SelectedItem != null)
            {
                DataRowView dataTable = (DataRowView)lvFournisseurs.SelectedItem;

                SqlBD.NoAnswerRequest(connexion, "SET SQL_SAFE_UPDATES = 0");
                SqlBD.NoAnswerRequest(connexion, "SET FOREIGN_KEY_CHECKS = 0");

                string siret = dataTable["siret"].ToString();
                string idAdresse = SqlBD.SingleValueRequest(connexion, $"SELECT idAdresse FROM fournisseur WHERE siret ='{siret}'");
                string delAdresse = $"DELETE FROM adresse WHERE idAdresse = '{idAdresse}'";
                SqlBD.NoAnswerRequest(connexion, delAdresse);
                string delFournit = $"DELETE FROM fournit WHERE siret='{siret}'";
                SqlBD.NoAnswerRequest(connexion, delFournit);
                string delProcure = $"DELETE FROM procure WHERE siret='{siret}'";
                SqlBD.NoAnswerRequest(connexion, delFournit);
                string delFournisseur = $"DELETE FROM fournisseur WHERE siret='{siret}'";
                SqlBD.NoAnswerRequest(connexion, delFournisseur);

                SqlBD.NoAnswerRequest(connexion, "SET SQL_SAFE_UPDATES = 1");
                SqlBD.NoAnswerRequest(connexion, "SET FOREIGN_KEY_CHECKS = 1");

                MessageBox.Show($"Le fournisseur {dataTable["nom_F"]} a bien été supprimé");
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un fournisseur à supprimer");
            }
        }

        private void Actualiser(object sender, RoutedEventArgs e)
        {
            SqlBD.LoadData(connexion, "SELECT * FROM fournisseur NATURAL JOIN adresse", "LoadDataBinding", lvFournisseurs);
        }
    }
}
