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

            //Give the SubjectNameTextBox Cursor and Keyboard Focus 
            //so that the user can type without selecting it at start
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, new Action(delegate ()
            {
                SubjectNameTextBox.Focus();
                Keyboard.Focus(SubjectNameTextBox);
                SubjectNameTextBox.Select(0, 0);
            }));
            parentField = null;
        }

        public CreateSubjectWindow(SubjectPanel subjectPan)
        {
            InitializeComponent();
            //Attribute
            parentField = subjectPan;
            //Show current information in the textboxes
            SubjectNameTextBox.Text = subjectPan.SubjectNameTextBlock.Text;
            GoalVoteAvgComboBox.Text = subjectPan.GetGoalVoted().ToString();
            GoalPresentationTextBox.Text = subjectPan.GoalPresentLabel.Content.ToString();
            AvgOfTasksTextBox.Text = subjectPan.GetAverageNumTasks().ToString();
            NumOfAssignementsTextBox.Text = subjectPan.GetNumOfAssignements().ToString();
            
            //We don't create a new course, therefore a different text is used
            AcceptButton.Content = "Übernehmen";
        }

        /// <summary>
        /// Try parsing all TextBoxes to numbers. If it fails it will inform user which field failed
        /// </summary>
        /// <param name="goalVote"></param>
        /// <param name="goalPresent"></param>
        /// <param name="assignements"></param>
        /// <returns></returns>
        private bool TryParsingFields(string name, out int goalVote,
                                        out int goalPresent,out int avgNumTasks ,
                                        out int assignements)
        {
            bool parsedAll = true; //If any field fails, this will be set to false
            if(!Int32.TryParse(GoalVoteAvgComboBox.Text,out goalVote))
            {
                GoalVoteInputErrorLabel.Content = "Nur natürliche Zahlen erlaubt";
                goalVote = 0; //Set 0 for not failing again at the entry validation method
                parsedAll = false;
            }

            if (!Int32.TryParse(AvgOfTasksTextBox.Text, out avgNumTasks))
            {
                TaskAmountInputErrorLabel.Content = "Nur natürliche Zahlen erlaubt";
                avgNumTasks = 0;
                parsedAll = false;
            }

            if (!Int32.TryParse(GoalPresentationTextBox.Text, out goalPresent))
            {
                MinimumPresentationInputErrorLabel.Content = "Nur natürliche Zahlen erlaubt";
                goalPresent = 0;
                parsedAll = false;
            }

            if(!Int32.TryParse(NumOfAssignementsTextBox.Text, out assignements))
            {
                AssignmentAmountInputErrorLabel.Content = "Nur natürliche Zahlen erlaubt";
                assignements = 0;
                parsedAll = false;
            }

            //Check if the entries if parsable, are valid
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
            bool isValid = true; //If any field fails, this will be set to false

            if (name.Equals(""))
            {
                NameInputErrorLabel.Content = "Darf nicht leer sein";
                isValid = false;
            }

            if (goalVote > 100 || goalVote < 0)
            {
                GoalVoteInputErrorLabel.Content = "Nur Zahlen zwischen 0 und 100 erlaubt";
                isValid = false;
            }

            if(goalPresent < 0)
            {
                MinimumPresentationInputErrorLabel.Content = "Nur positive Zahlen erlaubt";
                isValid = false;
            }

            if(avgTasks < 0)
            {
                TaskAmountInputErrorLabel.Content = "Nur positive Zahlen erlaubt";
                isValid = false;
            }

            if(assigns < 0)
            {
                AssignmentAmountInputErrorLabel.Content = "Nur positive Zahlen erlaubt";
                isValid = false;
            }

            return isValid;
        }

        //If parsing is successful, create and add a new Subject to the main menu
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            string subName = SubjectNameTextBox.Text;

            if (TryParsingFields(subName ,out int goalVote, out int goalPresent, out int averageTasks, out int assignements))
            {

                if (parentField == null) { 
                SubjectPanel newSubject = new SubjectPanel(subName, goalVote, goalPresent,averageTasks, assignements);
                MainWindow.homeView.SubjectStackPanel.Children.Add(newSubject);
                }
                
                //Edit case
                else
                {
                    parentField.SubjectNameTextBlock.Text = subName;
                    parentField.SetGoalVotedLabel(goalVote);
                    parentField.GoalPresentLabel.Content = goalPresent.ToString();
                  
                    parentField.ApplyChanges(averageTasks, assignements);
                }
                //return to main menu
                HomeWindow.ReturnToMainMenu();
            }

        }

        // Just return home without changes
        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.ReturnToMainMenu();
        }
    }
}
