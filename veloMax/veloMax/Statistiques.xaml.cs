using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    /// Logique d'interaction pour Statistiques.xaml
    /// </summary>
    public partial class Statistiques : Window
    {
        MySqlConnection connexion;
        NouvelleCommande windowCommande;

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public Statistiques(MySqlConnection connexion)
        {
            this.connexion = connexion;
            InitializeComponent();
            SqlBD.LoadData(connexion, "SELECT numeroPiece, sum(qte) FROM compose_piece GROUP BY numeroPiece;", "LoadDataBinding", lvQteVenduPiece);
            SqlBD.LoadData(connexion, "SELECT numeroModele, sum(qte) FROM compose_modele GROUP BY numeroModele;", "LoadDataBinding", lvQteModele);
            SqlBD.LoadData(connexion, "SELECT i.nom_C, i.prenom_C FROM individu i, fidelio f, programme p WHERE i.numeroProgramme=p.numeroProgramme AND p.numeroFidelio=f.numeroFidelio AND f.numeroFidelio=1;", "LoadDataBinding", lvClientAdhesion);
            SqlBD.LoadData(connexion, "SELECT i.nom_C, i.prenom_C FROM individu i, fidelio f, programme p WHERE i.numeroProgramme=p.numeroProgramme AND p.numeroFidelio=f.numeroFidelio AND f.numeroFidelio=2;", "LoadDataBinding", lvClientAdhesion2);
            SqlBD.LoadData(connexion, "SELECT i.nom_C, i.prenom_C FROM individu i, fidelio f, programme p WHERE i.numeroProgramme=p.numeroProgramme AND p.numeroFidelio=f.numeroFidelio AND f.numeroFidelio=3;", "LoadDataBinding", lvClientAdhesion3);
            SqlBD.LoadData(connexion, "SELECT i.nom_C, i.prenom_C FROM individu i, fidelio f, programme p WHERE i.numeroProgramme=p.numeroProgramme AND p.numeroFidelio=f.numeroFidelio AND f.numeroFidelio=4;", "LoadDataBinding", lvClientAdhesion4);
        }
        

        public void Actualiser(object sender, RoutedEventArgs e)
        {
            string request1 = "SELECT numeroPiece, sum(qte) FROM compose_piece GROUP BY numeroPiece;";
            SqlBD.LoadData(connexion, request1, "LoadDataBindingPiece", lvQteVenduPiece);

            string request2 = "SELECT numeroModele, sum(qte) FROM compose_modele GROUP BY numeroModele;";
            SqlBD.LoadData(connexion, request2, "LoadDataBindingModele", lvQteModele);
        }


    }
        
}
