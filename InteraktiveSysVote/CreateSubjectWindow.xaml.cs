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

        /// <summary>
        /// Try parsing all TextBoxes to numbers. If it fails it will inform user which field failed
        /// </summary>
        /// <param name="goalVote"></param>
        /// <param name="goalPresent"></param>
        /// <param name="assignements"></param>
        /// <returns></returns>
        private bool TryParsingFields(out int goalVote, out int goalPresent,out int avgNumTasks ,out int assignements)
        {
            bool parsedAll = true; //If one field fails, this will be set to false
            if(!Int32.TryParse(goalVoteAvg.Text,out goalVote))
            {
                GoalVoteInputError.Content = "Nur natürliche Zahlen erlaubt";
                parsedAll = false;
            }

            if (!Int32.TryParse(avgOfTasks.Text, out avgNumTasks))
            {
                TaskAmountInputError.Content = "Nur natürliche Zahlen erlaubt";
                parsedAll = false;
            }

            if (!Int32.TryParse(goalPresentation.Text, out goalPresent))
            {
                MinimumPresentationInputError.Content = "Nur natürliche Zahlen erlaubt";
                parsedAll = false;
            }

            if(!Int32.TryParse(numOfExercises.Text, out assignements))
            {
                ExerciseAmountInputError.Content = "Nur natürliche Zahlen erlaubt";
                parsedAll = false;
            }

            return parsedAll;
        }

        //If parsing is successful, create and add a new Subject to the main menu
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            string subName = subjectName.Text;

            if (TryParsingFields(out int goalVote, out int goalPresent, out int averageTasks, out int assignements))
            {
                SubjectPanel newSubject = new SubjectPanel(subName, goalVote, goalPresent,averageTasks, assignements);
                MainWindow.homeView.SubjectStack.Children.Add(newSubject);

                //return to main menu
                HomeWindow.ReturnToMainMenu();
            }

        }

        // Just return home without changes
        private void Abbrechen_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.ReturnToMainMenu();
        }
    }
}
