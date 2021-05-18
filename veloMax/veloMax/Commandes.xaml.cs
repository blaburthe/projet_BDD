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
            SqlBD.LoadData(connexion, "SELECT * FROM commande NATURAL JOIN adresse", "LoadDataBinding", lvCommandes);
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

                //récupérer le total pièces
                string requestTotalPieces = $"SELECT SUM(qte*prix_P) FROM piece NATURAL JOIN compose_piece WHERE numeroCommande ={idCommande}";
                totalPieces.Content = $"{SqlBD.SingleValueRequest(connexion, requestTotalPieces)} €";

                //récupérer la composition en vélos
                string requestVelo = $"SELECT * FROM compose_modele NATURAL JOIN commande NATURAL JOIN modele WHERE numeroCommande = {idCommande}";//noms des attributs etc à corriger
                SqlBD.LoadData(connexion, requestVelo, "LoadDataBindingModeles", lvVelos);

                //récupérer le total velos
                string requestTotalVelos = $"SELECT SUM(qte*prix_M) FROM modele NATURAL JOIN compose_modele WHERE numeroCommande ={idCommande}";
                totalVelos.Content = $"{SqlBD.SingleValueRequest(connexion, requestTotalVelos)} €";

                //récupérer le total général
                string requestTotal = "SELECT sum(tot) total FROM " +
                    $"(SELECT SUM(qte*prix_M) tot FROM modele NATURAL JOIN compose_modele WHERE numeroCommande ={idCommande} UNION ALL " +
                    $"SELECT SUM(qte*prix_P) tot FROM piece NATURAL JOIN compose_piece WHERE numeroCommande ={idCommande})t";
                total.Content = $"{SqlBD.SingleValueRequest(connexion, requestTotal)} €";
            }
        }

        private void LivrerCommande(object sender, RoutedEventArgs e)
        {
            DataRowView dataTable = (DataRowView)lvCommandes.SelectedItem;
            string idCommande = dataTable["numeroCommande"].ToString();
            string modifDateL = $"UPDATE commande SET date_L='{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}' WHERE numeroCommande='{idCommande}'";
            SqlBD.NoAnswerRequest(connexion, modifDateL);
            //réactualiser
            SqlBD.LoadData(connexion, "SELECT * FROM commande NATURAL JOIN adresse", "LoadDataBinding", lvCommandes);

        }
        private void SupprimerCommande(object sender, RoutedEventArgs e)
        {
            DataRowView dataTable = (DataRowView)lvCommandes.SelectedItem;
            string idCommande = dataTable["numeroCommande"].ToString();

            SqlBD.NoAnswerRequest(connexion, "SET SQL_SAFE_UPDATES = 0");
            SqlBD.NoAnswerRequest(connexion, "SET FOREIGN_KEY_CHECKS = 0");

            string delPiece = $"DELETE FROM compose_piece WHERE numeroCommande='{idCommande}'";
            SqlBD.NoAnswerRequest(connexion, delPiece);
            string delModele = $"DELETE FROM compose_modele WHERE numeroCommande='{idCommande}'";
            SqlBD.NoAnswerRequest(connexion, delModele);
            string delCommandePart = $"DELETE FROM commande_particulier_effectuée WHERE numeroCommande='{idCommande}'";
            SqlBD.NoAnswerRequest(connexion, delCommandePart);
            string delCommandePro = $"DELETE FROM commande_pro_effectuée WHERE numeroCommande='{idCommande}';";
            SqlBD.NoAnswerRequest(connexion, delCommandePro);
            string delCommande = $"DELETE FROM commande WHERE numeroCommande='{idCommande}'";
            SqlBD.NoAnswerRequest(connexion, delCommande);
            
            SqlBD.NoAnswerRequest(connexion, "SET SQL_SAFE_UPDATES = 1");
            SqlBD.NoAnswerRequest(connexion, "SET FOREIGN_KEY_CHECKS = 1");

            //réactualiser
            SqlBD.LoadData(connexion, "SELECT * FROM commande NATURAL JOIN adresse", "LoadDataBinding", lvCommandes);
        }

        private void OuvrirNouvelleCommande(object sender, RoutedEventArgs e)
        {
            NouvelleCommande window = new NouvelleCommande(connexion);
            window.Show();
        }

        private void Rafraichir(object sender, RoutedEventArgs e)
        {
            SqlBD.LoadData(connexion, "SELECT * FROM commande NATURAL JOIN adresse", "LoadDataBinding", lvCommandes);
        }
    }
}
