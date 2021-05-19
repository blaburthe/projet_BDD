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
    /// Logique d'interaction pour PrixVelo.xaml
    /// </summary>
    public partial class PrixVelo : Window
    {
        MySqlConnection connexion;
        NouvelleCommande windowCommande;
        string numeroModele;

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public PrixVelo(MySqlConnection connexion, string num)
        {
            InitializeComponent();
            this.connexion = connexion;

            this.numeroModele = num;
        }

        private void SauvegarderModif(object sender, RoutedEventArgs e)
        {
            if (prix.Text != "")
            {
                string modifPrix = $"UPDATE modele " +
                    $" SET prix_M='{prix.Text}' " +
                    $" WHERE numeroModele='{numeroModele}'";
                SqlBD.NoAnswerRequest(connexion, modifPrix);
                

                MessageBox.Show("Le prix a bien été modifié !");
            }

            else
            {
                MessageBox.Show("Veuillez remplir l'intégralité des champs");
            }
        }
    }
}
