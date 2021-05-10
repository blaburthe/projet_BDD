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

        private void AjouterFournisseur(object sender, RoutedEventArgs e)
        {
            if (nom.Text == "" || contact.Text == "" || siret.Text == "" || rue.Text == "" || numeroRue.Text == "" || ville.Text == "" || codePostal.Text == "")
            {
                //Please fill all the required fields
                MessageBox.Show("Tous les champs doivent être remplis");
            }
            else
            {
                //insert info sql.....
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
