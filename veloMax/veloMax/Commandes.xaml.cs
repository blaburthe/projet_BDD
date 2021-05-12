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
            SqlBD.LoadData(connexion, "select * from commande natural join adresse", "LoadDataBinding", lvCommandes);
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LvCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvCommandes.SelectedItem != null)
            {
                DataRowView dataTable = (DataRowView)lvCommandes.SelectedItem;
                adresseLabel.Content = $"{dataTable["numeroRue"]} {dataTable["rue"]}, {dataTable["codeP"]} {dataTable["ville"]}";

                string idCommande = dataTable["numeroCommande"].ToString();

                //récupérer la composition en pièce
                string requestPiece = $"SELECT * FROM compose_piece NATURAL JOIN commande NATURAL JOIN piece WHERE numeroCommande = {idCommande}";//noms des attributs etc à corriger
                SqlBD.LoadData(connexion, requestPiece, "LoadDataBindingPieces", lvPieces);

                //récupérer le total
                string requestTotalPieces = $"SELECT SUM(qte*prix_P) FROM piece NATURAL JOIN compose_piece WHERE numeroCommande ={idCommande}";
                totalPieces.Content = $"{SqlBD.SingleValueRequest(connexion, requestTotalPieces)} €";

                //récupérer la composition en vélos
                string requestVelo = $"SELECT * FROM compose_modele NATURAL JOIN commande NATURAL JOIN modele WHERE numeroCommande = {idCommande}";//noms des attributs etc à corriger
                SqlBD.LoadData(connexion, requestVelo, "LoadDataBindingModeles", lvVelos);

                //récupérer le total
                string requestTotalVelos = $"SELECT SUM(qte*prix_M) FROM modele NATURAL JOIN compose_modele WHERE numeroCommande ={idCommande}";
                totalVelos.Content = $"{SqlBD.SingleValueRequest(connexion, requestTotalVelos)} €";
            }
        }

        private void LivrerCommande(object sender, RoutedEventArgs e)
        {
            DataRowView dataTable = (DataRowView)lvCommandes.SelectedItem;
            string idCommande = dataTable["numeroCommande"].ToString();
            //alter date_L where {idcommande}

            //réactualiser
            SqlBD.LoadData(connexion, "select * from commande natural join adresse", "LoadDataBinding", lvCommandes);

        }
        private void SupprimerCommande(object sender, RoutedEventArgs e)
        {
            DataRowView dataTable = (DataRowView)lvCommandes.SelectedItem;
            string idCommande = dataTable["numeroCommande"].ToString();
            //del where {idcommande}

            //réactualiser
            SqlBD.LoadData(connexion, "select * from commande natural join adresse", "LoadDataBinding", lvCommandes);

        }

        private void OuvrirNouvelleCommande(object sender, RoutedEventArgs e)
        {
            NouvelleCommande window = new NouvelleCommande(connexion);
            window.Show();
        }
    }
}
