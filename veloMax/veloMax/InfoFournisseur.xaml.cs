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

        private void SauvegarderModif(object sender, RoutedEventArgs e)
        {
            if (nom.Text != "" && contact.Text != "" && siret.Text != "" && rue.Text != "" && numeroRue.Text != "" &&
                ville.Text != "" && codePostal.Text != "" && noteCombobox.SelectedItem != null)
            {
                //trouver le fournisseur grace à this.numeroSiret
                //send alter
                MessageBox.Show($"Le fournisseur {nom.Text} a bien été modifié !");
                this.Close();
            }
            else
            {
                MessageBox.Show("Veuillez remplir l'intégralité des champs");
            }
        }
    }
}
