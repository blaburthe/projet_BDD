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
using System.Collections.ObjectModel;



namespace veloMax
{
    /// <summary>
    /// Logique d'interaction pour NouvelleCommande.xaml
    /// </summary>
    public partial class NouvelleCommande : Window
    {

        MySqlConnection connexion;
        ObservableCollection<ItemCommande> itemsCommande = new ObservableCollection<ItemCommande> { };
        string typeCommande;
        string telClient;
        int remise;
         
        
        public NouvelleCommande(MySqlConnection connexion)
        {
            this.connexion = connexion;
            this.telClient = "";
            this.remise = 0;
            this.typeCommande = ""; //pro ou particulier

            InitializeComponent();

            lvItems.ItemsSource = itemsCommande;
            
            //Chargement des données du catalogue
            //string requestPiece = "Select * from piece";
            string requestPiece = @"SELECT * FROM piece P1, fournit F1
                                        WHERE P1.numeroPiece = F1.numeroPiece
                                        AND F1.delai =
                                        (SELECT MIN(T.delai) FROM(SELECT * FROM fournit F WHERE F.numeroPiece = F1.numeroPiece) AS T)
                                        GROUP BY P1.numeroPiece; "; //group by pour n'avoir qu'un seul élément par numéro de pièce
            SqlBD.LoadData(connexion, requestPiece, "LoadDataPieces", lvCataloguePieces);

            //string requestVelos = "select * from modele";
            string requestVelos = @"SELECT * FROM modele M1, procure P1
                                        WHERE M1.numeroModele = P1.numeroModele
                                        AND P1.delai =
                                        (SELECT MIN(T.delai) FROM(SELECT * FROM procure P WHERE P.numeroModele = P1.numeroModele) AS T)
                                        GROUP BY P1.numeroModele; "; //group by pour n'avoir qu'un seul élément par numéro de pièce
            SqlBD.LoadData(connexion , requestVelos, "LoadDataVelos", lvCatalogueVelos);
        }

        public string TypeCommande { get { return typeCommande; } set { typeCommande = value; } }
        public string TelClient { get { return telClient; } set { telClient = value; } }
        public int Remise { get { return remise; } set { remise = value; } }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public int CalculerTotal()
        {
            int total = 0;
            foreach(ItemCommande item in itemsCommande)
            {
                total += item.PrixUClient * item.Quantite;
            }
            totalSansRemise.Text = $"{total} €";
            if (remise !=0)
            {
                int reduc = (total * remise) / 100;
                total -= reduc;
                reduction.Text = $"- {reduc} €";

            }
            totalAvecRemise.Text = $"{total} €";
            return total;
        }

        public void Ajouter(object sender, RoutedEventArgs e)
        {
            DataRowView dataTable = null;
            string idItem = ""; //identifiant de l'item à ajouter à la commande
            string description = "";
            string type = "";
            int prixU = 0;

            switch (((TabItem)tab.SelectedItem).Header)
            {
                case "pièces":
                    dataTable = (DataRowView)lvCataloguePieces.SelectedItem;
                    idItem = Convert.ToString(dataTable["numeroPiece"]);
                    description = $"{dataTable["description"]} - #{dataTable["numeroPiece"]}";
                    prixU = (int)dataTable["prix_P"];
                    type = "piece";

                    break;

                case "vélos":
                    dataTable = (DataRowView)lvCatalogueVelos.SelectedItem;
                    idItem = Convert.ToString(dataTable["numeroModele"]);
                    prixU = (int)dataTable["prix_M"];
                    description = $"{dataTable["ligne_produit"]} {dataTable["nom_M"]} {dataTable["grandeur_M"]} ({dataTable["date_intro_M"]} -> {dataTable["date_fin_M"]}) #{dataTable["numeroModele"]}";
                    type = "velo";

                    break;
            }
            int delai = (int)dataTable["delai"];

            //On regarde s'il s'y trouve déjà, si c'est le cas on l'enlève et le remplace par un nouveau avec une quantité incrémenté de 1
            //(en incrémentant directement sa quantité, la list view ne s'update pas)
            try
            {
                ItemCommande item = (ItemCommande)(itemsCommande.Where(x => x.IdItem == idItem).First()); //on récupère l'objet de la liste si il s'y trouve
                int index = itemsCommande.IndexOf(item); //On récupère son index
                int quantite = itemsCommande[index].Quantite + 1; //On récupère l'ancienne quantité avant d'enelever l'item
                itemsCommande.Remove(item);
                itemsCommande.Add(new ItemCommande(idItem, description, prixU, delai, type, quantite));
            }
            catch
            {
                //Item n'existe pas encore
                
                itemsCommande.Add(new ItemCommande(idItem, description, prixU, delai, type));
            }
            CalculerTotal(); //On réactualise le total
        }

        private void OuvrirSelectionnnerClient(object sender, RoutedEventArgs e)
        {
            Clients window = new Clients(connexion, this);
            window.Show();
        }

        private void Sousmettre(object sender, RoutedEventArgs e)
        {
            if (itemsCommande.Count == 0 || telClient=="")
            {
                MessageBox.Show("Veuillez sélectionner un client et des produits pour la commande");
            }
            else
            {
                //on créer un identifiant de commande
                int idCommande = Convert.ToInt32(SqlBD.SingleValueRequest(connexion, "SELECT MAX(numeroCommande) FROM commande")) + 1;

                string idAdresse = "";
                if (TypeCommande == "pro")
                {
                    idAdresse = SqlBD.SingleValueRequest(connexion, $"SELECT idAdresse FROM boutique WHERE telephone_B = {telClient}");
                    string ajoutCommande = $"INSERT INTO `veloMax`.`commande` (`numeroCommande`, `date_C`, `date_L`, `idAdresse`) " +
                                            $"VALUES ({idCommande}, '{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}', null,'{idAdresse}')";
                    SqlBD.NoAnswerRequest(connexion, ajoutCommande);
                    SqlBD.NoAnswerRequest(connexion, $"INSERT INTO `veloMax`.`commande_pro_effectuée` (`numeroCommande`,`telephone_B`) VALUES ('{idCommande}','{telClient}')");
                }
                else
                {
                    idAdresse = SqlBD.SingleValueRequest(connexion, $"SELECT idAdresse FROM individu WHERE telephone_C = {telClient}");
                    string ajoutCommande = $"INSERT INTO `veloMax`.`commande` (`numeroCommande`, `date_C`, `date_L`, `idAdresse`) " +
                                            $"VALUES ({idCommande}, '{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}', null,'{idAdresse}')";
                    SqlBD.NoAnswerRequest(connexion, ajoutCommande);
                    SqlBD.NoAnswerRequest(connexion, $"INSERT INTO `veloMax`.`commande_particulier_effectuée` (`numeroCommande`,`telephone_C`) VALUES ('{idCommande}','{telClient}')");
                }

                
         
                foreach (ItemCommande item in itemsCommande)
                {
                    if (item.Type == "piece")
                    {
                        string ajoutPiece = $"INSERT INTO `veloMax`.`compose_piece` (`numeroCommande`,`numeroPiece`, `qte`) VALUES ('{idCommande}','{item.IdItem}', '{item.Quantite}')";
                        SqlBD.NoAnswerRequest(connexion, ajoutPiece);   
                    }
                    else
                    {
                        string ajoutModele = $"INSERT INTO `veloMax`.`compose_modele` (`numeroCommande`,`numeroModele`, `qte`) VALUES ('{idCommande}','{item.IdItem}', '{item.Quantite}')";
                        SqlBD.NoAnswerRequest(connexion, ajoutModele);
                    }
                }
                MessageBox.Show($"Commande n°{idCommande} sousmise !");
            }
        }
    }
}
