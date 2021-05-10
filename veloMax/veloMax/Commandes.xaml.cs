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
    /// Logique d'interaction pour Commandes.xaml
    /// </summary>
    public partial class Commandes : Window
    {

        MySqlConnection connexion;

        public Commandes(MySqlConnection connexion)
        {
            this.connexion = connexion;
            InitializeComponent();
            LoadData("select * from commande natural join adresse", "LoadDataBinding", lvCommandes);
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        public void LoadData(string request, string bindingSourceName, ListView target)
        {
            try
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand(request, connexion);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                adp.Fill(ds, bindingSourceName);
                target.DataContext = ds;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            connexion.Close();
        }

        private void LvCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView dataTable = (DataRowView)lvCommandes.SelectedItem;
            adresseLabel.Content = $"{dataTable["numeroRue"]} {dataTable["rue"]}, {dataTable["codeP"]} {dataTable["ville"]}"; //à voir si ça marche

            string idCommande = dataTable["numeroCommande"].ToString();

            //récupérer la composition en pièce
            string requestPiece = $"SELECT * FROM compose_piece NATURAL JOIN commande NATURAL JOIN piece WHERE numeroCommande = {idCommande}";//noms des attributs etc à corriger
            SqlBD.LoadData(connexion, requestPiece, "LoadDataBindingPieces", lvPieces);

            //récupérer le total
            string requestTotalPieces = $"SELECT SUM(qte*prix_unit_P) FROM piece NATURAL JOIN compose_piece WHERE numeroCommande ={idCommande}";
            totalPieces.Content = $"{SqlBD.SingleValueRequest(connexion, requestTotalPieces)} €";

            //récupérer la composition en vélos
            string requestVelo = $"SELECT * FROM compose_modele NATURAL JOIN commande NATURAL JOIN modele WHERE numeroCommande = {idCommande}";//noms des attributs etc à corriger
            SqlBD.LoadData(connexion, requestVelo, "LoadDataBindingModeles", lvVelos);

            //récupérer le total
            string requestTotalVelos = $"SELECT SUM(qte*prix_unit_M) FROM modele NATURAL JOIN compose_modele WHERE numeroCommande ={idCommande}";
            totalVelos.Content = $"{SqlBD.SingleValueRequest(connexion, requestTotalVelos)} €";


        }
    }
}
