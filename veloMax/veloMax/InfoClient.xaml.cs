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
        /// <summary>
        /// Constructeur pour modifier un client particulier
        /// </summary>
        public InfoClient(MySqlConnection connexion, string nom, string prenom, string tel, string mail, string numeroRue,
            string rue, string codePostal, string ville, string fidelio = "", string datePaiement ="", string duree="")
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
            this.fidelio.Text = fidelio;
            this.datePaiement.Text = datePaiement;
            this.duree.Text = duree;

            typeClient = "particulier";
            telephone = tel;
            
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
            this.fidelio.IsEnabled = false;
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


        private void SauvegarderModif(object sender, RoutedEventArgs e)
        {
            if(typeClient == "particulier")
            {
                //checking every field is completed and that fidelio related fields are either all completed or all empty
                if (nom.Text != "" && prenom.Text != "" && tel.Text != "" && mail.Text != ""
                && rue.Text != "" && numeroRue.Text != "" && ville.Text != "" && codePostal.Text != ""
                && !((fidelio.Text != "" && (datePaiement.Text == "" || duree.Text == "")) || (fidelio.Text == "" && (datePaiement.Text != "" || duree.Text != ""))))           
                {
                    //send alter
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
                    //send alter
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
