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
using System.IO;
using Newtonsoft.Json;
using System.Globalization;


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

        public void Modification(object sender, RoutedEventArgs e)
        {

            if (Table.SelectedIndex == 0)
            {
                if (lvStockPiece.SelectedItem != null)
                {
                    DataRowView dataTable = (DataRowView)lvStockPiece.SelectedItem;
                    PrixPiece window = new PrixPiece(connexion, dataTable["numeroPiece"].ToString());
                    window.Show();
                }

                else
                {
                    MessageBox.Show("Veuillez sélectionner une pièce dont vous voulez modifier le prix");

                }
            }
            if (Table.SelectedIndex == 1)
            {
                if (lvStockVelo.SelectedItem != null)
                {
                    DataRowView dataTable = (DataRowView)lvStockVelo.SelectedItem;
                    PrixVelo window = new PrixVelo(connexion, dataTable["numeroModele"].ToString());
                    window.Show();
                }

                else
                {
                    MessageBox.Show("Veuillez sélectionner un modèle dont vous voulez modifier le prix");
                }
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
        public void ExportXMLJSON(object sender, RoutedEventArgs e)
        {
            connexion.Open();
            string request = $"SELECT numeroPiece, description, stock FROM piece WHERE stock<=3";
            MySqlCommand cmd = new MySqlCommand(request, connexion);
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            adp.Fill(ds);
            connexion.Close();

            XmlDocument stockXml = new XmlDocument();
            XmlDeclaration xmldecl = stockXml.CreateXmlDeclaration("1.0", "UTF-8", "no");
            XmlElement racine = stockXml.CreateElement("pieces_a_commander");
            stockXml.AppendChild(racine);
            stockXml.InsertBefore(xmldecl, racine);

            foreach (DataTable table in ds.Tables) //il n'y aura qu'une seule table
            {
                foreach (DataRow row in table.Rows)
                {
                    XmlElement piece = stockXml.CreateElement("piece");

                    XmlElement numeroPiece = stockXml.CreateElement("numeroPiece");
                    numeroPiece.InnerText = Convert.ToString(row.ItemArray[0]);
                    piece.AppendChild(numeroPiece);

                    XmlElement description = stockXml.CreateElement("description");
                    description.InnerText = (string)row.ItemArray[1];
                    piece.AppendChild(description);

                    XmlElement stock = stockXml.CreateElement("stock");
                    stock.InnerText = Convert.ToString(row.ItemArray[2]);
                    piece.AppendChild(stock);
                    
                    //fournisseurs

                    connexion.Open();
                    string requestFournisseurs = $"SELECT nom_F, prix_unit_P, numeroCatalogue FROM fournisseur NATURAL JOIN fournit WHERE numeroPiece={row.ItemArray[0]} ORDER BY prix_unit_P";
                    MySqlCommand cmdFournisseurs = new MySqlCommand(requestFournisseurs, connexion);
                    MySqlDataAdapter adpFournisseurs = new MySqlDataAdapter(cmdFournisseurs);
                    DataSet dsFournisseurs = new DataSet();
                    adpFournisseurs.Fill(dsFournisseurs);
                    connexion.Close();
                    

                    foreach (DataRow rowFournisseur in dsFournisseurs.Tables[0].Rows)
                    {
                        XmlElement fournisseur = stockXml.CreateElement("fournisseur");

                        XmlElement nom = stockXml.CreateElement("nom");
                        nom.InnerText = (string)rowFournisseur.ItemArray[0];
                        fournisseur.AppendChild(nom);

                        XmlElement prixUnitaire = stockXml.CreateElement("prixUnitaire");
                        prixUnitaire.InnerText = Convert.ToString(rowFournisseur.ItemArray[1]);
                        fournisseur.AppendChild(prixUnitaire);

                        XmlElement numeroCatalogue = stockXml.CreateElement("numeroCatalogue");
                        numeroCatalogue.InnerText = Convert.ToString(rowFournisseur.ItemArray[2]);
                        fournisseur.AppendChild(numeroCatalogue);

                        piece.AppendChild(fournisseur);
                    }
                    racine.AppendChild(piece);
                }

       
            }
            stockXml.Save("stocks_faibles.xml");
            MessageBox.Show("le fichier stockPiece.xml a été créé !");
            

            //JSON



            string monFichier = "clientsARelancer.json";

            connexion.Open();
            string requestClients = "SELECT nom_C, telephone_C, DATEDIFF(DATE_ADD(datePaiement, interval duree year), NOW())" +
           " FROM individu NATURAL JOIN programme NATURAL join fidelio " +
           "WHERE DATEDIFF(DATE_ADD(datePaiement, interval duree year),NOW())<60";
            MySqlCommand cmdClients = new MySqlCommand(requestClients, connexion);
            MySqlDataAdapter adpClients = new MySqlDataAdapter(cmdClients);
            DataSet dsClients = new DataSet();
            adpClients.Fill(dsClients);
            connexion.Close();

            //instanciation des "writer"
            StreamWriter writer = new StreamWriter(monFichier);
            JsonTextWriter jwriter = new JsonTextWriter(writer);

            //debut du fichier Json
            jwriter.WriteStartObject();

            //debut du tableau Json
            jwriter.WritePropertyName("clientsARelancer");
            jwriter.WriteStartArray();

            foreach (DataRow row in dsClients.Tables[0].Rows)
            {
                jwriter.WriteStartObject();
                jwriter.WritePropertyName("Nom");
                jwriter.WriteValue((string)row.ItemArray[0]);
                jwriter.WritePropertyName("Telephone");
                jwriter.WriteValue((string)row.ItemArray[1]);
                jwriter.WritePropertyName("JoursRestants");
                jwriter.WriteValue(Convert.ToInt32(row.ItemArray[2]));
                jwriter.WriteEndObject();
            }

            jwriter.WriteEndArray();
            jwriter.WriteEndObject();

            //fermeture de "writer"
            jwriter.Close();
            writer.Close();

            MessageBox.Show("le fichier clients_à_relancer.json a été créé !");
        }
    }

    [ValueConversion(typeof(object), typeof(int))]
    public class IsLessThan2IncludedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value <= 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    [ValueConversion(typeof(object), typeof(int))]
    public class IsBetween2and4IncludedConverter : IValueConverter

    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value <= 4 && (int)value >2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
