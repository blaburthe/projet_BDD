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

namespace veloMax
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); //coucou
            //useless comment
        }

        private void OpenCommandes(object sender, RoutedEventArgs e)
        {
            Commandes window_commandes = new Commandes();
            window_commandes.Show();
        }

        private void close_window(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
