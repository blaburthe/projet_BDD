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
    /// Logique d'interaction pour NouveauClient.xaml
    /// </summary>
    public partial class NouveauClient : Window
    {
        MySqlConnection connexion;

        public NouveauClient(MySqlConnection connexion)
        {
            this.connexion = connexion;
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectionChangedTypeClient(object sender, RoutedEventArgs e)
        {
            if(particulierRadio.IsChecked==true)
            {
                //On passe en configuration particulier
                label1.Content = "Nom";
                label2.Content = "Prénom";
                fidelioCombobox.IsEnabled = true;
                remise.IsEnabled = false;
            }
            else
            {
                //On passe en configuration boutique
                label1.Content = "Boutique";
                label2.Content = "Nom contact";
                fidelioCombobox.IsEnabled = false;
                remise.IsEnabled = true;
            }
        }

        private void ajouterClient(object sender, RoutedEventArgs e)
        {
            if(nom.Text =="" || prenom.Text == "" || mail.Text == "" || tel.Text == "" || rue.Text == "" || numeroRue.Text == "" || ville.Text == "" || codePostal.Text == "")
            {
                //Please fill all the required fields
                MessageBox.Show("Tous les champs doivent être remplis");
            }
            else
            {
                if(tel.Text.Length!=10)
                {
                    //Telephone number has to be 10 in length
                    MessageBox.Show("Ce numéro de téléphone doit comporter 10 chiffres");
                }
                else
                {
                    // On ajoute l'adresse du client à la BD

                    //On créer l'idAdresse :
                    int idAdresse = Convert.ToInt32(SqlBD.SingleValueRequest(connexion, "SELECT MAX(idAdresse) from adresse")) +1;
                    string requestAdresse = "INSERT INTO `veloMax`.`adresse` " +
                                            "(`idAdresse`,`numeroRue`, `rue`, `ville`, `codeP`) " +
                                            $"VALUES ('{idAdresse}', '{numeroRue.Text}', '{rue.Text}', '{ville.Text}', '{codePostal.Text}')";
                    SqlBD.NoAnswerRequest(connexion, requestAdresse);


                    if (particulierRadio.IsChecked == true)
                    {
                        //Commande pour les particuliers
                        int numeroFidelio = fidelioCombobox.SelectedIndex;
                        int numeroProgramme = Convert.ToInt32(SqlBD.SingleValueRequest(connexion, "SELECT MAX(numeroProgramme) FROM programme")) + 1; 


                        //On ajoute le programme du client à la BD
                        string requestFidelio = "INSERT INTO `veloMax`.`programme` " +
                                                 "(`numeroProgramme`, `numeroFidelio`, `datePaiement`)" +
                                                 $"VALUES('{numeroProgramme}', '{numeroFidelio}','{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}')";
                        SqlBD.NoAnswerRequest(connexion, requestFidelio);


                        //On ajoute le client à la BD
                        string requestClient = "INSERT INTO `veloMax`.`individu` "+ 
                                                "(`telephone_C`,`nom_C`, `prenom_C`, `courriel_C`, `numeroProgramme`, `idAdresse`) " +
                                                $"VALUES('{tel.Text}', '{nom.Text}', '{prenom.Text}', '{mail.Text}', '{numeroProgramme}', '{idAdresse}')";
                        SqlBD.NoAnswerRequest(connexion, requestClient);

                    }
                    else
                    {
                        //Commande pour les boutique

                        string requestClient = "INSERT INTO `veloMax`.`boutique` " + 
                                               "(`telephone_B`,`nom_B`, `courriel_B`, `contact_B`, `remise`, `idAdresse`) " +
                                               $"VALUES('{tel.Text}', '{nom.Text}', '{mail.Text}', '{prenom.Text}', '{remise.Text}', '{idAdresse}')";
                        SqlBD.NoAnswerRequest(connexion, requestClient);
                    }
                    MessageBox.Show("Client ajouté avec succès");

                    nom.Text = "";
                    prenom.Text = "";
                    mail.Text = "";
                    tel.Text = "";
                    rue.Text = "";
                    numeroRue.Text = "";
                    ville.Text = "";
                    codePostal.Text = "";
                    remise.Text = "";
                }
            }
            
            
        }
        
    }
}
