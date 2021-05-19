using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
using MySql.Data.MySqlClient;

namespace veloMax
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Info : Window
    { 
            MySqlConnection connexion;
            NouvelleCommande windowCommande;
            string numeroModele;

            private void CloseWindow(object sender, RoutedEventArgs e)
            {
                this.Close();
            }

            public Info(MySqlConnection connexion, string num)
            {
                InitializeComponent();
                this.connexion = connexion;

                this.numeroModele = num;
                string requete = "SELECT p.numeroPiece, p.description, p.stock, p.prix_P " +
                              "FROM piece p, modele m, assemble a " +
                              "WHERE p.numeroPiece=a.numeroPiece " +
                              "AND m.numeroModele=a.numeroModele " +
                              $"AND m.numeroModele='{numeroModele}';";
                SqlBD.LoadData(connexion, requete, "LoadDataBindingAssembl", lvAssemblage);
            }
       
    }
}
