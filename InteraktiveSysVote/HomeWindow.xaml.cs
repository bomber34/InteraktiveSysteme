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

namespace InteraktiveSysVote
{
    /// <summary>
    /// Interaktionslogik für HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : UserControl
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Returns back to the main menu
        /// </summary>
        public static void ReturnToMainMenu()
        {
            //The window itself only has a single grid which is used to display the current menu
            MainWindow.mainViewGrid.Children.RemoveAt(0);
            MainWindow.mainViewGrid.Children.Add(MainWindow.homeView);
        }

        private void AddSubjectBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateSubjectWindow createNewSub = new CreateSubjectWindow();
            //Switch to course creation menu
            MainWindow.mainViewGrid.Children.RemoveAt(0);
            MainWindow.mainViewGrid.Children.Add(createNewSub);
        }

    }
}
