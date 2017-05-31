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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InteraktiveSysVote
{
    /// <summary>
    /// Interaktionslogik für CreateSubjectWindow.xaml
    /// </summary>
    public partial class CreateSubjectWindow : UserControl
    {
        public CreateSubjectWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            // TODO Exception Handling: Tell people if they fucked up
            string subName = subjectName.Text;
            int goalVote = Int32.Parse(goalVoteAvg.Text);
            int goalPresent = Int32.Parse(goalPresentation.Text);
            int exerciseAmount = Int32.Parse(numOfExercises.Text);
            SubjectPanel newSubject = new SubjectPanel(subName, goalVote, goalPresent, exerciseAmount);
            MainWindow.homeView.SubjectStack.Children.Add(newSubject);

            //return to main menu
            MainWindow.mainWindow.Children.RemoveAt(0);
            MainWindow.mainWindow.Children.Add(MainWindow.homeView);
        }

        // Just return home without changes
        private void Abbrechen_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.Children.RemoveAt(0);
            MainWindow.mainWindow.Children.Add(MainWindow.homeView);
        }
    }
}
