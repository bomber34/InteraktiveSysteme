using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private SubjectPanel parentField;

        public CreateSubjectWindow()
        {
            InitializeComponent();


            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, new Action(delegate ()
            {
                subjectName.Focus();
                Keyboard.Focus(subjectName);
                subjectName.Select(0, 0);
            }));
            parentField = null;
        }

        public CreateSubjectWindow(SubjectPanel subjectPan,string currentSubjectName,int currentGoalVoteAvg,int currentGoalPresantation,int currentAvgOfTasks,int currentNumOfExercises)
        {
            InitializeComponent();
            parentField = subjectPan;
            subjectName.Text = currentSubjectName;
            goalVoteAvg.Text = currentGoalVoteAvg.ToString();
            goalPresentation.Text = currentGoalPresantation.ToString();
            avgOfTasks.Text = currentAvgOfTasks.ToString();
            numOfExercises.Text = currentNumOfExercises.ToString();
            accept.Content = "Übernehmen";
        }

        /// <summary>
        /// Try parsing all TextBoxes to numbers. If it fails it will inform user which field failed
        /// </summary>
        /// <param name="goalVote"></param>
        /// <param name="goalPresent"></param>
        /// <param name="assignements"></param>
        /// <returns></returns>
        private bool TryParsingFields(string name, out int goalVote, out int goalPresent,out int avgNumTasks ,out int assignements)
        {
            bool parsedAll = true; //If one field fails, this will be set to false
            if(!Int32.TryParse(goalVoteAvg.Text,out goalVote))
            {
                GoalVoteInputError.Content = "Nur natürliche Zahlen erlaubt";
                goalVote = 0;
                parsedAll = false;
            }

            if (!Int32.TryParse(avgOfTasks.Text, out avgNumTasks))
            {
                TaskAmountInputError.Content = "Nur natürliche Zahlen erlaubt";
                avgNumTasks = 0;
                parsedAll = false;
            }

            if (!Int32.TryParse(goalPresentation.Text, out goalPresent))
            {
                MinimumPresentationInputError.Content = "Nur natürliche Zahlen erlaubt";
                goalPresent = 0;
                parsedAll = false;
            }

            if(!Int32.TryParse(numOfExercises.Text, out assignements))
            {
                ExerciseAmountInputError.Content = "Nur natürliche Zahlen erlaubt";
                assignements = 0;
                parsedAll = false;
            }

            bool isValid = IsValidInput(name ,goalVote, goalPresent, avgNumTasks, assignements);

            return parsedAll && isValid;
        }

        /// <summary>
        /// Checks if the name is not empty and the numbers are greater or equal 0 except goalVote which must be between 0 and 100
        /// </summary>
        /// <param name="name"></param>
        /// <param name="goalVote"></param>
        /// <param name="goalPresent"></param>
        /// <param name="avgTasks"></param>
        /// <param name="assigns"></param>
        /// <returns></returns>
        private bool IsValidInput(string name, int goalVote, int goalPresent, int avgTasks, int assigns)
        {
            bool isValid = true;

            if(name.Equals(""))
            {
                NameInputError.Content = "Darf nicht leer sein";
                isValid = false;
            }

            if (goalVote > 100 || goalVote < 0)
            {
                GoalVoteInputError.Content = "Nur Zahlen zwischen 0 und 100 erlaubt";
                isValid = false;
            }

            if(goalPresent < 0)
            {
                MinimumPresentationInputError.Content = "Nur positive Zahlen erlaubt";
                isValid = false;
            }

            if(avgTasks < 0)
            {
                TaskAmountInputError.Content = "Nur positive Zahlen erlaubt";
                isValid = false;
            }

            if(assigns < 0)
            {
                ExerciseAmountInputError.Content = "Nur positive Zahlen erlaubt";
                isValid = false;
            }

            return isValid;
        }

        //If parsing is successful, create and add a new Subject to the main menu
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            string subName = subjectName.Text;

            if (TryParsingFields(subName ,out int goalVote, out int goalPresent, out int averageTasks, out int assignements))
            {
                if (parentField == null) { 
                SubjectPanel newSubject = new SubjectPanel(subName, goalVote, goalPresent,averageTasks, assignements);
                MainWindow.homeView.SubjectStack.Children.Add(newSubject);
                }

                else
                {
                    parentField.SubjectName.Text = subName;
                    parentField.goalVoted.Content = goalVote.ToString() + "%";
                    parentField.GoalPresent.Content = goalPresent.ToString();
                  
                    parentField.exerciseMenu.ApplyChanges(subName, goalVote, goalPresent, averageTasks, assignements);
                }
                //return to main menu
                HomeWindow.ReturnToMainMenu();
            }

        }

        // Just return home without changes
        private void Abbrechen_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.ReturnToMainMenu();
        }

        private void subjectName_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }
    }
}
