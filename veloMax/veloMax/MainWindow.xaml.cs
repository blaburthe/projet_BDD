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

        
        MySqlConnection conn = new
        MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        public MainWindow()
        {
            InitializeComponent(); //coucou
            //useless comment
        }

        private void OpenCommandes(object sender, RoutedEventArgs e)
        {
            Commandes windowCommandes = new Commandes(conn);
            windowCommandes.Show();
        }
        
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void OuvrirClients(object sender, RoutedEventArgs e)
        {
            Clients window = new Clients(conn);
            window.Show();
        }

        private void loadData(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select numeroPiece, description, stock from piece", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                lvStock.DataContext = ds;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }

        private void OuvrirFournisseurs(object sender, RoutedEventArgs e)
        {
            Fournisseurs window = new Fournisseurs(conn);
            window.Show();
        }
    }
}
