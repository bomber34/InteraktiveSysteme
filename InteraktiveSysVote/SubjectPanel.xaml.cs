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
    /// Interaktionslogik für SubjectPanel.xaml
    /// </summary>
    public partial class SubjectPanel : UserControl
    {
        
        private ExerciseWindow exerciseMenuWindow;

        public SubjectPanel()
        {
            InitializeComponent();
        }

        public SubjectPanel(string subjectName, int avgVoteGoal,
            int minPresent, int numOfTasks, int numOfExercises)
        {
            InitializeComponent();
            SubjectNameTextBlock.Text = subjectName;
            SetGoalVotedLabel(avgVoteGoal);
            GoalPresentLabel.Content = minPresent.ToString();

            exerciseMenuWindow = new ExerciseWindow(this, numOfTasks , numOfExercises);
        }

        public int GetAverageNumTasks()
        {
            return exerciseMenuWindow.avgNumOfTasks;
        }

        public int GetNumOfAssignements()
        {
            return exerciseMenuWindow.numberOfAssignements;
        }

        /// <summary>
        /// Gets the Number of GoalVoted without the %
        /// </summary>
        /// <returns></returns>
        public int GetGoalVoted()
        {
            //There will always be a % at the end of the Label which has to be removed
            System.Diagnostics.Debug.Assert(Int32.TryParse(GoalVotedLabel.Content.ToString().Remove(GoalVotedLabel.Content.ToString().Length - 1), out int result));

            return Int32.Parse(GoalVotedLabel.Content.ToString().Remove(GoalVotedLabel.Content.ToString().Length - 1));
        }

        /// <summary>
        /// Places a % at the end of the number
        /// </summary>
        /// <param name="goalVote"></param>
        public void SetGoalVotedLabel(int goalVote)
        {
            GoalVotedLabel.Content = goalVote.ToString() + "%";
        }

        /// <summary>
        /// direct these values to the exerciseMenuWindow.ApplyChanges
        /// </summary>
        /// <param name="averageTasks"></param>
        /// <param name="assignments"></param>
        public void ApplyChanges(int averageTasks, int assignments)
        {
            exerciseMenuWindow.ApplyChanges(averageTasks, assignments);
        }

        private void EditSubjectBtn_Click(object sender, RoutedEventArgs e)
        { 
            //Switch the CreateSubject View
            MainWindow.mainViewGrid.Children.RemoveAt(0);
            MainWindow.mainViewGrid.Children.Add(new CreateSubjectWindow(this));
        }

        private void DeleteSubjectBtn_Click(object sender, RoutedEventArgs e)
        {
            //Create dialog with user to be sure they want to delete the subject
            MessageBoxResult result = MessageBox.Show("Möchtest du das Fach löschen?",
                "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow.homeView.SubjectStackPanel.Children
                    .RemoveAt(MainWindow.homeView.SubjectStackPanel.Children.IndexOf(this));
            }
        }
        
        private void OpenExerciseWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            //Switch to exercise view
            MainWindow.mainViewGrid.Children.RemoveAt(0);
            MainWindow.mainViewGrid.Children.Add(exerciseMenuWindow);
        }
    }
}
