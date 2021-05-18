﻿using System;
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
