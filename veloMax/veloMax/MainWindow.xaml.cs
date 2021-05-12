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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace veloMax
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        MySqlConnection connexion;

        public MainWindow()
        {
            this.connexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            InitializeComponent();
        }

        private void OpenCommandes(object sender, RoutedEventArgs e)
        {
            Commandes windowCommandes = new Commandes(connexion);
            windowCommandes.Show();
        }
        
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void OuvrirClients(object sender, RoutedEventArgs e)
        {
            Clients window = new Clients(connexion);
            window.Show();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            string request = "Select numeroPiece, description, stock from piece";
            SqlBD.LoadData(connexion, request, "LoadDataBinding", lvStockPiece);
        }

        private void OuvrirFournisseurs(object sender, RoutedEventArgs e)
        {
            Fournisseurs window = new Fournisseurs(connexion);
            window.Show();
        }
    }
}
