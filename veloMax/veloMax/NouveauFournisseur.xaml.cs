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
    /// Logique d'interaction pour NouveauFournisseur.xaml
    /// </summary>
    public partial class NouveauFournisseur : Window
    {

        MySqlConnection connexion;

        public NouveauFournisseur(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AjouterFournisseur(object sender, RoutedEventArgs e)
        {
            if (nom.Text == "" || contact.Text == "" || siret.Text == "" || rue.Text == "" || numeroRue.Text == "" || ville.Text == "" || codePostal.Text == "")
            {
                //Please fill all the required fields
                MessageBox.Show("Tous les champs doivent être remplis");
            }
            else
            {
                // On ajoute l'adresse du client à la BD

                //On créer l'idAdresse :
                int idAdresse = Convert.ToInt32(SqlBD.SingleValueRequest(connexion, "SELECT MAX(idAdresse) from adresse")) + 1;
                string ajoutAdresse = "INSERT INTO `veloMax`.`adresse` " +
                                        "(`idAdresse`,`numeroRue`, `rue`, `ville`, `codeP`) " +
                                        $"VALUES ('{idAdresse}', '{numeroRue.Text}', '{rue.Text}', '{ville.Text}', '{codePostal.Text}')";
                SqlBD.NoAnswerRequest(connexion, ajoutAdresse);

                //On ajoute le fournisseur
                string ajoutFournisseur = "INSERT INTO `veloMax`.`fournisseur` " +
                                        "(`siret`, `nom_F`, `contact_F`, `libelle`, `idAdresse`) " +
                                        $"VALUES('{siret.Text}', '{nom.Text}', '{contact.Text}', '{noteCombobox.SelectedIndex +1}', '{idAdresse}')";
                SqlBD.NoAnswerRequest(connexion, ajoutFournisseur);


                MessageBox.Show($"Le fournisseur {nom.Text} a été ajouté avec succès !");
                nom.Text = "";
                contact.Text = "";
                siret.Text = "";
                rue.Text = "";
                numeroRue.Text = "";
                ville.Text = "";
                codePostal.Text = "";
            }
        }
        
    }
}
