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
    /// Logique d'interaction pour PrixPiece.xaml
    /// </summary>
    public partial class PrixPiece : Window
    {
        MySqlConnection connexion;
        NouvelleCommande windowCommande;
        string numeroPiece;

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public PrixPiece(MySqlConnection connexion, string num)
        {
            InitializeComponent();
            this.connexion = connexion;

            this.numeroPiece = num;
        }

        private void SauvegarderModif(object sender, RoutedEventArgs e)
        {
            if (prix.Text != "")
            {
                string modifPrix = $"UPDATE piece" +
                    $" SET prix_P='{prix.Text}'" +
                    $" WHERE numeroPiece='{numeroPiece}';";
                SqlBD.NoAnswerRequest(connexion, modifPrix);

                SqlBD.NoAnswerRequest(connexion, "SET SQL_SAFE_UPDATES = 0");
                SqlBD.NoAnswerRequest(connexion, "SET FOREIGN_KEY_CHECKS = 0");

                MessageBox.Show("Le prix a bien été modifié !");
            }

            else
            {
                MessageBox.Show("Veuillez remplir l'intégralité des champs");
            }
        }
    }
}
