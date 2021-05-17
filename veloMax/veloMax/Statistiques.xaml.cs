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

            string classement1 = $@"SELECT i.nom_C, sum(c.qte) c
                                    FROM individu i, commande_particulier_effectuée cp, commande co , compose_piece c 
                                    WHERE i.telephone_C=cp.telephone_C      
                                    AND co.numeroCommande=cp.numeroCommande
                                    AND co.numeroCommande=c.numeroCommande 
                                    GROUP BY i.nom_C 
                                    ORDER BY sum(c.qte) DESC;";
            SqlBD.LoadData(connexion, classement1, "LoadDataBinding", lvClientClassement1);

            string classement2 = $@"SELECT i.nom_C, sum(p.prix_P) m
                                    FROM individu i, commande_particulier_effectuée cp, commande co , compose_piece c, piece p 
                                    WHERE p.numeroPiece=c.numeroPiece 
                                    AND i.telephone_C=cp.telephone_C 
                                    AND co.numeroCommande=cp.numeroCommande 
                                    AND co.numeroCommande=c.numeroCommande 
                                    GROUP BY i.nom_C ORDER BY sum(p.prix_P) DESC;";
            SqlBD.LoadData(connexion, classement2, "LoadDataBinding", lvClientClassement2);

            string classement3 = $@"SELECT b.nom_B, sum(c.qte) k
                                    FROM boutique b, commande_pro_effectuée cp, commande co , compose_piece c 
                                    WHERE b.telephone_B=cp.telephone_B 
                                    AND co.numeroCommande=cp.numeroCommande 
                                    AND co.numeroCommande=c.numeroCommande 
                                    GROUP BY b.nom_b 
                                    ORDER BY sum(c.qte) DESC;";
            SqlBD.LoadData(connexion, classement3, "LoadDataBinding", lvClientClassement3);

            string classement4 = $@"SELECT b.nom_B, sum(p.prix_P) L
                                    FROM boutique b, commande_pro_effectuée cp, commande co , compose_piece c, piece p 
                                    WHERE p.numeroPiece=c.numeroPiece 
                                    AND b.telephone_B=cp.telephone_B 
                                    AND co.numeroCommande=cp.numeroCommande 
                                    AND co.numeroCommande=c.numeroCommande 
                                    GROUP BY b.nom_b 
                                    ORDER BY sum(p.prix_P) DESC;";
            SqlBD.LoadData(connexion, classement4, "LoadDataBinding", lvClientClassement4);

        }


        public void Actualiser(object sender, RoutedEventArgs e)
        {
            string requete1 = "SELECT numeroPiece, sum(qte) FROM compose_piece GROUP BY numeroPiece;";
            SqlBD.LoadData(connexion, requete1, "LoadDataBinding", lvQteVenduPiece);

            string requete2 = "SELECT numeroModele, sum(qte) FROM compose_modele GROUP BY numeroModele;";
            SqlBD.LoadData(connexion, requete2, "LoadDataBinding", lvQteModele);

            string requete3 = $@"SELECT i.nom_C, i.prenom_C
                             FROM individu i, fidelio f, programme p 
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=1;";
            SqlBD.LoadData(connexion, requete3, "LoadDataBinding", lvClientAdhesion);

            string requete4 = $@"SELECT i.nom_C, i.prenom_C
                             FROM individu i, fidelio f, programme p 
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=2;";
            SqlBD.LoadData(connexion, requete4, "LoadDataBinding", lvClientAdhesion2);

            string requete5 = $@"SELECT i.nom_C, i.prenom_C
                             FROM individu i, fidelio f, programme p 
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=3;";
            SqlBD.LoadData(connexion, requete5, "LoadDataBinding", lvClientAdhesion3);

            string requete6 = $@"SELECT i.nom_C, i.prenom_C
                             FROM individu i, fidelio f, programme p 
                             WHERE i.numeroProgramme=p.numeroProgramme 
                             AND p.numeroFidelio=f.numeroFidelio 
                             AND f.numeroFidelio=1;";
            SqlBD.LoadData(connexion, requete6, "LoadDataBinding", lvClientAdhesion4);

            string requete7 = $@"SELECT i.nom_C, sum(c.qte) c
                                    FROM individu i, commande_particulier_effectuée cp, commande co , compose_piece c 
                                    WHERE i.telephone_C=cp.telephone_C      
                                    AND co.numeroCommande=cp.numeroCommande
                                    AND co.numeroCommande=c.numeroCommande 
                                    GROUP BY i.nom_C 
                                    ORDER BY sum(c.qte) DESC;";
            SqlBD.LoadData(connexion, requete7, "LoadDataBinding", lvClientClassement1);

            string requete8 = $@"SELECT i.nom_C, sum(p.prix_P) m
                                    FROM individu i, commande_particulier_effectuée cp, commande co , compose_piece c, piece p 
                                    WHERE p.numeroPiece=c.numeroPiece 
                                    AND i.telephone_C=cp.telephone_C 
                                    AND co.numeroCommande=cp.numeroCommande 
                                    AND co.numeroCommande=c.numeroCommande 
                                    GROUP BY i.nom_C ORDER BY sum(p.prix_P) DESC;";
            SqlBD.LoadData(connexion, requete8, "LoadDataBinding", lvClientClassement2);

            string requete9 = $@"SELECT b.nom_B, sum(c.qte) k
                                    FROM boutique b, commande_pro_effectuée cp, commande co , compose_piece c 
                                    WHERE b.telephone_B=cp.telephone_B 
                                    AND co.numeroCommande=cp.numeroCommande 
                                    AND co.numeroCommande=c.numeroCommande 
                                    GROUP BY b.nom_b 
                                    ORDER BY sum(c.qte) DESC;";
            SqlBD.LoadData(connexion, requete9, "LoadDataBinding", lvClientClassement3);

            string requete10 = $@"SELECT b.nom_B, sum(p.prix_P) L
                                    FROM boutique b, commande_pro_effectuée cp, commande co , compose_piece c, piece p 
                                    WHERE p.numeroPiece=c.numeroPiece 
                                    AND b.telephone_B=cp.telephone_B 
                                    AND co.numeroCommande=cp.numeroCommande 
                                    AND co.numeroCommande=c.numeroCommande 
                                    GROUP BY b.nom_b 
                                    ORDER BY sum(p.prix_P) DESC;";
            SqlBD.LoadData(connexion, requete10, "LoadDataBinding", lvClientClassement4);
        }

        
    }
        
}
