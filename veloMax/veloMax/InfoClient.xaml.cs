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
    /// Logique d'interaction pour InfoClient.xaml
    /// </summary>
    public partial class InfoClient : Window
    {

        MySqlConnection connexion;
        string typeClient;
        string telephone; //will be used to find the client we are modifying
        int numeroFidelio;
        /// <summary>
        /// Constructeur pour modifier un client particulier
        /// </summary>
        public InfoClient(MySqlConnection connexion, string nom, string prenom, string tel, string mail, string numeroRue,
            string rue, string codePostal, string ville, string datePaiement ="", string duree="", int numeroFidelio=0)
        {
            InitializeComponent();
            this.connexion = connexion;

            this.remise.IsEnabled = false;

            this.nom.Text = nom;
            this.prenom.Text = prenom;
            this.tel.Text = tel;
            this.mail.Text = mail;
            this.numeroRue.Text = numeroRue;
            this.rue.Text = rue;
            this.codePostal.Text = codePostal;
            this.ville.Text = ville;
            this.datePaiement.Text = datePaiement;
            this.duree.Text = duree;

            typeClient = "particulier";
            this.telephone = tel;
            this.numeroFidelio = numeroFidelio;
            this.fidelioCombobox.SelectedIndex = numeroFidelio;
            
        }
        /// <summary>
        /// Constructeur pour modifier un client pro (boutique)
        /// </summary>
        public InfoClient(MySqlConnection connexion, string boutique, string contact, string tel, string mail, string numeroRue,
            string rue, string codePostal, string ville, string remise)
        {
            InitializeComponent();
            this.connexion = connexion;


            label1.Content = "Boutique";
            label2.Content = "Contact";
            this.fidelioCombobox.IsEnabled = false;
            this.datePaiement.IsEnabled = false;
            this.duree.IsEnabled = false;

            this.nom.Text = boutique;
            this.prenom.Text = contact;
            this.tel.Text = tel;
            this.mail.Text = mail;
            this.numeroRue.Text = numeroRue;
            this.rue.Text = rue;
            this.codePostal.Text = codePostal;
            this.ville.Text = ville;
            this.remise.Text = remise;


            typeClient = "boutique";
            telephone = tel;

        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SauvegarderModif(object sender, RoutedEventArgs e)
        {
            if(typeClient == "particulier")
            {
                //checking every field is completed
                if (nom.Text != "" && prenom.Text != "" && tel.Text != "" && mail.Text != ""
                && rue.Text != "" && numeroRue.Text != "" && ville.Text != "" && codePostal.Text != "")           
                {
                    //Modification adresse
                    string idAdresse = SqlBD.SingleValueRequest(connexion, $"SELECT idAdresse FROM adresse NATURAL JOIN individu WHERE telephone_C ={telephone}");
                    string modifAdresse = $"UPDATE adresse " +
                        $"SET rue='{rue.Text}', " +
                        $"numeroRue = '{numeroRue.Text}', " +
                        $"ville = '{ville.Text}', " +
                        $"codeP = '{codePostal.Text}' " +
                        $"WHERE idAdresse='{idAdresse}'";
                    SqlBD.NoAnswerRequest(connexion, modifAdresse);

                    SqlBD.NoAnswerRequest(connexion, "SET SQL_SAFE_UPDATES = 0");
                    SqlBD.NoAnswerRequest(connexion, "SET FOREIGN_KEY_CHECKS = 0");
                    
                    //Modification client
                    string modifClient = $"UPDATE individu " +
                        $"SET courriel_C='{mail.Text}', " +
                        $"nom_C = '{nom.Text}', " +
                        $"prenom_C = '{prenom.Text}', " +
                        $"telephone_C = '{tel.Text}' " +
                        $"WHERE telephone_C='{telephone}'";//ancien num
                    SqlBD.NoAnswerRequest(connexion, modifClient);

                    //Modification commandes

                    string modifCommande = $"UPDATE commande_particulier_effectuée " +
                        $"SET telephone_C='{tel.Text}' " +
                        $"WHERE telephone_C='{telephone}'"; //ancien num
                    SqlBD.NoAnswerRequest(connexion, modifCommande);

                    //Modification programme
                    if (fidelioCombobox.SelectedIndex == 0 && numeroFidelio != 0)
                    {
                        //supprimer l'ancien programme au cas où il y en avait un
                        string numeroProgramme = SqlBD.SingleValueRequest(connexion, $"SELECT numeroProgramme FROM individu WHERE telephone_C ={tel.Text}");
                        string reqProgr = $"DELETE FROM programme WHERE numeroProgramme = {numeroProgramme}";
                        SqlBD.NoAnswerRequest(connexion, reqProgr);

                    }
                    else
                    {   
                        if(fidelioCombobox.SelectedIndex != 0 || numeroFidelio != 0)
                        {
                            //faire les modif si besoin
                            string numeroProgramme = SqlBD.SingleValueRequest(connexion, $"SELECT numeroProgramme FROM individu WHERE telephone_C ={tel.Text}");
                            string modifProgramme = $"UPDATE programme " +
                                $"SET numeroFidelio='{fidelioCombobox.SelectedIndex}', " +
                                $"datePaiement = '{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}' " +
                                $"WHERE numeroProgramme='{numeroProgramme}'";   //ancien num
                            SqlBD.NoAnswerRequest(connexion, modifProgramme);

                        }
                        //sinon pas de changemnt
                    }

                    SqlBD.NoAnswerRequest(connexion, "SET SQL_SAFE_UPDATES = 1");
                    SqlBD.NoAnswerRequest(connexion, "SET FOREIGN_KEY_CHECKS = 1");



                    MessageBox.Show($"Le profil client {nom.Text} {prenom.Text} a bien été modifié !");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Veuillez remplir l'intégralité des champs");
                }

            }
            else
            {
                //checking every field is completed
                if (nom.Text != "" && prenom.Text != "" && tel.Text != "" && mail.Text != ""
                && rue.Text != "" && numeroRue.Text != "" && ville.Text != "" && codePostal.Text != ""
                && remise.Text != "")
                {

                    //Modification adresse
                    string idAdresse = SqlBD.SingleValueRequest(connexion, $"SELECT idAdresse FROM adresse NATURAL JOIN boutique WHERE telephone_B ={telephone}");
                    string modifAdresse = $"UPDATE adresse " +
                        $"SET rue='{rue.Text}', " +
                        $"numeroRue = '{numeroRue.Text}', " +
                        $"ville = '{ville.Text}', " +
                        $"codeP = '{codePostal.Text}' " +
                        $"WHERE idAdresse='{idAdresse}'";
                    SqlBD.NoAnswerRequest(connexion, modifAdresse);

                    SqlBD.NoAnswerRequest(connexion, "SET SQL_SAFE_UPDATES = 0");
                    SqlBD.NoAnswerRequest(connexion, "SET FOREIGN_KEY_CHECKS = 0");


                    //Modification client
                    string modifClient = $"UPDATE boutique " +
                        $"SET courriel_B='{mail.Text}', " +
                        $"nom_B = '{nom.Text}', " +
                        $"contact_B = '{prenom.Text}', " +
                        $"telephone_B = '{tel.Text}', " +
                        $"remise = '{remise.Text}' " +
                        $"WHERE telephone_B='{telephone}'";//ancien num
                    SqlBD.NoAnswerRequest(connexion, modifClient);

                    //Modification commandes

                    string modifCommande = $"UPDATE commande_pro_effectuée " +
                        $"SET telephone_B='{tel.Text}' " +
                        $"WHERE telephone_B='{telephone}'"; //ancien num
                    SqlBD.NoAnswerRequest(connexion, modifCommande);

                    SqlBD.NoAnswerRequest(connexion, "SET SQL_SAFE_UPDATES = 1");
                    SqlBD.NoAnswerRequest(connexion, "SET FOREIGN_KEY_CHECKS = 1");



                    MessageBox.Show($"La boutique {nom.Text} a bien été modifiée !");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Veuillez remplir l'intégralité des champs");
                }
            }

        }
    }
}
