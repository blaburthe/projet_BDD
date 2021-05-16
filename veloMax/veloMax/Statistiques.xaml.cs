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

            string fidelio1 = $@"SELECT i.nom_C, i.prenom_C, date_add(p.datePaiement, interval 1 year) y
                             FROM individu i, fidelio f, programme p 
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=1;";
            SqlBD.LoadData(connexion, fidelio1, "LoadDataBinding", lvClientAdhesion);

            string fidelio2 = $@"SELECT i.nom_C, i.prenom_C, date_add(p.datePaiement, interval 2 year) x
                             FROM individu i, fidelio f, programme p 
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=2;";
            SqlBD.LoadData(connexion, fidelio2, "LoadDataBinding", lvClientAdhesion2);

            string fidelio3 = $@"SELECT i.nom_C, i.prenom_C, date_add(p.datePaiement, interval 3 year) z
                             FROM individu i, fidelio f, programme p 
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=3;";
            SqlBD.LoadData(connexion, fidelio3, "LoadDataBinding", lvClientAdhesion3);

            string fidelio4 = $@"SELECT i.nom_C, i.prenom_C, date_add(p.datePaiement, interval 4 year) a
                             FROM individu i, fidelio f, programme p
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=4;";
            SqlBD.LoadData(connexion, fidelio4, "LoadDataBinding", lvClientAdhesion4);

        }


        public void Actualiser(object sender, RoutedEventArgs e)
        {
            string request1 = "SELECT numeroPiece, sum(qte) FROM compose_piece GROUP BY numeroPiece;";
            SqlBD.LoadData(connexion, request1, "LoadDataBindingPiece", lvQteVenduPiece);

            string request2 = "SELECT numeroModele, sum(qte) FROM compose_modele GROUP BY numeroModele;";
            SqlBD.LoadData(connexion, request2, "LoadDataBindingModele", lvQteModele);

            string requete3 = $@"SELECT i.nom_C, i.prenom_C
                             FROM individu i, fidelio f, programme p 
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=1;";
            SqlBD.LoadData(connexion, requete3, "LoadDataBindingProgramme1", lvClientAdhesion);

            string requete4 = $@"SELECT i.nom_C, i.prenom_C
                             FROM individu i, fidelio f, programme p 
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=2;";
            SqlBD.LoadData(connexion, requete4, "LoadDataBindingProgramme2", lvClientAdhesion2);

            string requete5 = $@"SELECT i.nom_C, i.prenom_C
                             FROM individu i, fidelio f, programme p 
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=3;";
            SqlBD.LoadData(connexion, requete5, "LoadDataProgramme3", lvClientAdhesion3);

            string requete6 = $@"SELECT i.nom_C, i.prenom_C
                             FROM individu i, fidelio f, programme p 
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=1;";
            SqlBD.LoadData(connexion, requete6, "LoadDataBindingProgramme4", lvClientAdhesion4);
        }
    }
        
}
