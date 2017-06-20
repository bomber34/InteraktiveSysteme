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
        //Constructor
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

        //Getter Setter

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
            string goal = GoalVotedLabel.Content.ToString();
            //There will always be a % at the end of the Label which has to be removed
            System.Diagnostics.Debug.Assert(goal.ElementAt(goal.Length-1) == '%');

            return Int32.Parse(goal.Remove(goal.Length-1));
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
        /// Gets the Number of AverageVoted Tasks without the %
        /// </summary>
        /// <returns></returns>
        public int GetAverageVoted()
        {
            string average = AvgVotedLabel.Content.ToString();
            //There will always be a % at the end of the Label which has to be removed
            System.Diagnostics.Debug.Assert(average.ElementAt(average.Length-1) == '%');
            return Int32.Parse(average.Remove(average.Length - 1));
        }

        /// <summary>
        /// Places a % at the end of the label after editing the new number
        /// </summary>
        /// <param name="average"></param>
        public void SetAverageVoted(int average)
        {
            AvgVotedLabel.Content = average.ToString() + "%";
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

        //Button Events
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
