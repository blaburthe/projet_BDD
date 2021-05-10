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
using System.Configuration;
using System.Windows.Media.Animation;

namespace veloMax
{
    /// <summary>
    /// Logique d'interaction pour ClientsParticuliers.xaml
    /// </summary>
    public partial class Clients : Window
    {
        MySqlConnection connexion;

        public Clients(MySqlConnection connexion)
        {
            this.connexion = connexion;

            InitializeComponent();
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
                if (ds.Tables[0].Rows.Count != 0)
                {
                    lvClientPart.DataContext = ds;
                }
                else
                {
                    Storyboard sb = Resources["sbHideAnimation"] as Storyboard;
                    sb.Begin(error);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            connexion.Close();
        }

        private void SelectionChangedTri(object sender, RoutedEventArgs e)
        {
            string request = "Select * from adresse";
            if (boutiquesRadio.IsChecked==true)
            {
                request += " natural join boutique";
                switch (((ComboBoxItem)triSelect.SelectedItem).Content)
                {
                    case "Alphabétique":
                        request += " order by nom_B";
                        break;

                    case "Ville":
                        request += " order by ville";
                        break;

                    case "Fidélité":
                        request += " order by remise asc";
                        break;
                }
            }
            else
            {
                request += " natural join individu natural join programme";
                switch (((ComboBoxItem)triSelect.SelectedItem).Content)
                {
                    case "Alphabétique":
                        request += " order by nom_C, prenom_C";
                        break;

                    case "Ville":
                        request += " order by ville";
                        break;

                    case "Fidélité":
                        request += " order by descr";
                        break;
                }
            }
            LoadData(request); //reload data ordered by chosen order
        }

        private void RechercherClient(object sender, RoutedEventArgs e)
        {
            string hint = rechercheText.Text;
            string request = "";
            if (boutiquesRadio.IsChecked==true)
            {
                request =
                $@"select * from boutique natural join adresse 
                                where nom_B like '%{hint}%'
                                or contact_B like '%{hint}%'
                                or telephone_B like '%{hint}%'
                                or courriel_B like '%{hint}%'
                                or ville like '%{hint}%'
                                or codeP like '%{hint}%'
                                or rue like '%{hint}%'";
            }
            else
            {
                request =
                $@"select * from individu natural join adresse 
                                where nom_C like '%{hint}%'
                                or prenom_C like '%{hint}%'
                                or telephone_C like '%{hint}%'
                                or courriel_C like '%{hint}%'
                                or ville like '%{hint}%'
                                or codeP like '%{hint}%'
                                or rue like '%{hint}%'";
            }
            LoadData(request);
        }

        private void NouveauClientOpen(object sender, RoutedEventArgs e)
        {
            NouveauClient window = new NouveauClient();
            window.Show();
        }

        private void SelectionChangedTypeClient(object sender, RoutedEventArgs e)
        {
            if(particuliersRadio.IsChecked ==true) 
            {
                gridClient.Columns[0].Header = "Nom"; //Nom- > boutique
                gridClient.Columns[1].Header = "Prénom"; //Prénom -> Nom contact
                gridClient.Columns[8].Header = "Fidélio";

                gridClient.Columns[0].DisplayMemberBinding = new Binding("nom_C");
                gridClient.Columns[1].DisplayMemberBinding = new Binding("prenom_C");
                gridClient.Columns[2].DisplayMemberBinding = new Binding("telephone_C");
                gridClient.Columns[3].DisplayMemberBinding = new Binding("courriel_C");
                gridClient.Columns[8].DisplayMemberBinding = new Binding("descr");
                gridClient.Columns[9].Width = 100;
                gridClient.Columns[10].Width = 100;



                LoadData("Select * from individu natural join adresse natural join programme");
               
            }
            else
            {
                //il faut alors passer en affichage boutique
                //inversé car lorsque SelectionChangedTypeClient est déclenché, le changement au niveau de radio buttons n'est pas encore fait
                gridClient.Columns[0].Header = "Boutique"; //Nom- > boutique
                gridClient.Columns[1].Header = "Nom contact"; //Prénom -> Nom contact
                gridClient.Columns[8].Header = "Rabais";


                gridClient.Columns[0].DisplayMemberBinding = new Binding("nom_B");
                gridClient.Columns[1].DisplayMemberBinding = new Binding("contact_B");
                gridClient.Columns[2].DisplayMemberBinding = new Binding("telephone_B");
                gridClient.Columns[3].DisplayMemberBinding = new Binding("courriel_B");
                gridClient.Columns[8].DisplayMemberBinding = new Binding("remise");
                gridClient.Columns[9].Width = 0;
                gridClient.Columns[10].Width = 0;




                LoadData("Select * from boutique natural join adresse");
            }
        }

        private void OuvrirInfoClient(object sender, RoutedEventArgs e)
        {
            if(lvClientPart.SelectedItem != null)
            {
                DataRowView dataTable = (DataRowView)lvClientPart.SelectedItem;

                if (boutiquesRadio.IsChecked == true)
                {
                    InfoClient window = new InfoClient(connexion, dataTable["nom_B"].ToString(), dataTable["contact_B"].ToString(), dataTable["telephone_B"].ToString(), dataTable["courriel_B"].ToString(),
                        dataTable["numeroRue"].ToString(), dataTable["rue"].ToString(), dataTable["codeP"].ToString(), dataTable["ville"].ToString(), dataTable["remise"].ToString());
                    window.Show();
                }
                else
                {
                    InfoClient window = new InfoClient(connexion, dataTable["nom_C"].ToString(), dataTable["prenom_C"].ToString(), dataTable["telephone_C"].ToString(), dataTable["courriel_C"].ToString(),
                        dataTable["numeroRue"].ToString(), dataTable["rue"].ToString(), dataTable["codeP"].ToString(), dataTable["ville"].ToString(), dataTable["descr"].ToString(),
                        dataTable["date_paiement"].ToString(), dataTable["duree"].ToString());
                    window.Show();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client à modifier");
            }
            
        }

        private void Actualiser(object sender, RoutedEventArgs e)
        {
            if (particuliersRadio.IsChecked == true)
            {
                LoadData("Select * from individu natural join adresse natural join programme");
            }
            else
            {
                LoadData("Select * from boutique natural join adresse");
            }
                
        }
    }
}
