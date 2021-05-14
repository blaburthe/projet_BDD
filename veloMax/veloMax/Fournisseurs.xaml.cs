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
    /// Logique d'interaction pour Fournisseurs.xaml
    /// </summary>
    public partial class Fournisseurs : Window
    {
        MySqlConnection connexion;

        public Fournisseurs(MySqlConnection connexion)
        {
            this.connexion = connexion;
            InitializeComponent();
            LoadData("SELECT * FROM fournisseur NATURAL JOIN adresse");
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadData(string request)
        {
            try
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand(request, connexion);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                lvFournisseurs.DataContext = ds;
                
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            connexion.Close();
        }

        private void OuvrirInfoFournisseur(object sender, RoutedEventArgs e)
        {
            if (lvFournisseurs.SelectedItem != null)
            {
                DataRowView dataTable = (DataRowView)lvFournisseurs.SelectedItem;
                
                InfoFournisseur window = new InfoFournisseur(connexion, dataTable["nom_F"].ToString(), dataTable["contact_F"].ToString(), dataTable["siret"].ToString(), dataTable["libelle"].ToString(),
                        dataTable["numeroRue"].ToString(), dataTable["rue"].ToString(), dataTable["codeP"].ToString(), dataTable["ville"].ToString());

                window.Show();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un fournisseur à modifier");
            }
        }

        private void OuvrirNouveauFournisseur(object sender, RoutedEventArgs e)
        {
            NouveauFournisseur window = new NouveauFournisseur(connexion);
            window.Show();
        }

    }
}
