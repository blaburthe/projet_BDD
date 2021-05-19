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

using MySql.Data.MySqlClient;

namespace veloMax
{
    /// <summary>
    /// Logique d'interaction pour InfoFournisseur.xaml
    /// </summary>
    public partial class InfoFournisseur : Window
    {

        string numeroSiret;
        MySqlConnection connexion;

        public InfoFournisseur(MySqlConnection connexion, string nom, string contact, string siret, string note, string numeroRue, string rue,
             string codePostal, string ville)
        {
            InitializeComponent();

            this.numeroSiret = siret;
            this.connexion = connexion;

            this.nom.Text = nom;
            this.contact.Text = contact;
            this.siret.Text = siret;
           
            this.noteCombobox.SelectedIndex = Convert.ToInt32(note) - 1; //décalage de 1 entre la note et l'index dans le combobox
            this.numeroRue.Text = numeroRue;
            this.rue.Text = rue;
            this.codePostal.Text = codePostal;
            this.ville.Text = ville;
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SauvegarderModif(object sender, RoutedEventArgs e)
        {
            if (nom.Text != "" && contact.Text != "" && siret.Text != "" && rue.Text != "" && numeroRue.Text != "" &&
                ville.Text != "" && codePostal.Text != "" && noteCombobox.SelectedItem != null)
            {
                //trouver le fournisseur grace à this.numeroSiret
                
                //Modification adresse
                string idAdresse = SqlBD.SingleValueRequest(connexion, $"SELECT idAdresse FROM adresse NATURAL JOIN fournisseur WHERE siret ='{this.numeroSiret}'");
                string modifAdresse = $"UPDATE adresse " +
                    $"SET rue='{rue.Text}', " +
                    $"numeroRue = '{numeroRue.Text}', " +
                    $"ville = '{ville.Text}', " +
                    $"codeP = '{codePostal.Text}' " +
                    $"WHERE idAdresse='{idAdresse}'";
                SqlBD.NoAnswerRequest(connexion, modifAdresse);

                SqlBD.NoAnswerRequest(connexion, "SET SQL_SAFE_UPDATES = 0");
                SqlBD.NoAnswerRequest(connexion, "SET FOREIGN_KEY_CHECKS = 0");

                //Modification fournisseur
                string modifFournisseur = $"UPDATE fournisseur " +
                    $"SET siret ='{siret.Text}', " +
                    $"nom_F = '{nom.Text}', " +
                    $"contact_F = '{contact.Text}', " +
                    $"libelle = '{noteCombobox.SelectedIndex +1}' " +
                    $"WHERE siret='{this.numeroSiret}'";//ancien siret
                SqlBD.NoAnswerRequest(connexion, modifFournisseur);

                //Modification catalogue pièces

                string modifCataloguePiece = $"UPDATE fournit " +
                    $"SET siret='{siret.Text}' " +
                    $"WHERE siret='{this.numeroSiret}'"; //ancien siret
                SqlBD.NoAnswerRequest(connexion, modifCataloguePiece);

                //Modification catalogue vélos

                string modifCatalgoueVelos = $"UPDATE procure " +
                    $"SET siret='{siret.Text}' " +
                    $"WHERE siret='{this.numeroSiret}'"; //ancien siret
                SqlBD.NoAnswerRequest(connexion, modifCatalgoueVelos);

                MessageBox.Show($"Le fournisseur {nom.Text} a bien été modifié !");
                this.Close();

                SqlBD.NoAnswerRequest(connexion, "SET SQL_SAFE_UPDATES = 1");
                SqlBD.NoAnswerRequest(connexion, "SET FOREIGN_KEY_CHECKS = 1");
            }
            else
            {
                MessageBox.Show("Veuillez remplir l'intégralité des champs");
            }
        }
    }
}
