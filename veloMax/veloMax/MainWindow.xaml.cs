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
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlClient;

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

            RafraichirStock();
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
        public void RafraichirStock()
        {
            string request1 = "SELECT numeroPiece, description, stock FROM piece";
            SqlBD.LoadData(connexion, request1, "LoadDataBindingPiece", lvStockPiece);

            string request2 = "SELECT numeroModele, nom_M, grandeur_M, ligne_produit, stock FROM modele";
            SqlBD.LoadData(connexion, request2, "LoadDataBindingModele", lvStockVelo);
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            RafraichirStock();
        }

        public void Information(object sender, RoutedEventArgs e)
        {
            if (lvStockVelo.SelectedItem != null)
            {
                DataRowView dataTable = (DataRowView)lvStockVelo.SelectedItem;
                Info window = new Info(connexion, dataTable["numeroModele"].ToString());
                window.Show();
            }

            else
            {
                MessageBox.Show("Veuillez sélectionner un modèle dont vous voulez connaître l'assemblage");
            }
            
        }

        private void OuvrirFournisseurs(object sender, RoutedEventArgs e)
        {
            Fournisseurs window = new Fournisseurs(connexion);
            window.Show();
        }
        private void OuvrirStatistiques(object sender, RoutedEventArgs e)
        {
            Statistiques window = new Statistiques(connexion);
            window.Show();
        }
        public void ChargementDonneesXML(object sender, RoutedEventArgs e)
        {
            XmlDocument docXml = new XmlDocument();

            XmlElement racine = docXml.CreateElement("veloMax");
            docXml.AppendChild(racine);


            XmlDeclaration xmldecl = docXml.CreateXmlDeclaration("1.0", "UTF-8", "no");
            docXml.InsertBefore(xmldecl, racine);

            XmlElement numeroPiece = docXml.CreateElement("numeroPiece");
            racine.AppendChild(numeroPiece);

            XmlElement stock = docXml.CreateElement("stock");
            racine.AppendChild(stock);

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT stock FROM piece WHERE stock<=2;";
            command.CommandType = CommandType.Text;
            command.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds, "Member");
                
            
            StreamWriter xmlDoc = new StreamWriter("stockPiece.xml", false);
            ds.WriteXml(xmlDoc);
            xmlDoc.Close();
            
           
            MessageBox.Show("le fichier stockPiece.xml a été créé !");
            
        } 
    }
}
