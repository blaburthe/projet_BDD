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

namespace veloMax
{
    /// <summary>
    /// Logique d'interaction pour NouveauClient.xaml
    /// </summary>
    public partial class NouveauClient : Window
    {
        public NouveauClient()
        {
            InitializeComponent();
        }
        private void SelectionChangedTypeClient(object sender, RoutedEventArgs e)
        {
            if(particulierRadio.IsChecked==true)
            {
                //On passe en configuration particulier
                label1.Content = "Nom";
                label2.Content = "Prénom";
                fidelioCombobox.IsEnabled = true;
                remise.IsEnabled = false;
            }
            else
            {
                //On passe en configuration boutique
                label1.Content = "Boutique";
                label2.Content = "Nom contact";
                fidelioCombobox.IsEnabled = false;
                remise.IsEnabled = true;
            }
        }

        private void ajouterClient(object sender, RoutedEventArgs e)
        {
            if(nom.Text =="" || prenom.Text == "" || mail.Text == "" || tel.Text == "" || rue.Text == "" || numeroRue.Text == "" || Ville.Text == "" || codePostal.Text == "")
            {
                //Please fill all the required fields
                MessageBox.Show("Tous les champs doivent être remplis");
            }
            else
            {
                if(tel.Text.Length!=10)
                {
                    //Telephone number has to be 10 in length
                    MessageBox.Show("Ce numéro de téléphone doit comporter 10 chiffres");
                }
                else
                {
                    if(particulierRadio.IsChecked==true)
                    {
                        //Commande pour les particuliers
                    }
                    else
                    {
                        //Commande pour les boutique
                    }
                    //commande sql pour ajouter le client en fonction des infos fournies
                    MessageBox.Show("Client ajouté avec succès");

                    nom.Text = "";
                    prenom.Text = "";
                    mail.Text = "";
                    tel.Text = "";
                    rue.Text = "";
                    numeroRue.Text = "";
                    Ville.Text = "";
                    codePostal.Text = "";
                    remise.Text = "";
                }
            }
            
            
        }
    }
}
