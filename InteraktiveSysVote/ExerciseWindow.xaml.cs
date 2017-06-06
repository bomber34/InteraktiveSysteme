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
    /// Interaktionslogik für ExerciseWindow.xaml
    /// </summary>
    public partial class ExerciseWindow : UserControl
    {
        private string subjectName;
        private int goalPresent, numberOfTasks, numberOfAssignements;
        private SubjectPanel parentField;
        private GeneralSubjectOverview generalOverview;

        public ExerciseWindow()
        {
            InitializeComponent();
        }

        public ExerciseWindow(SubjectPanel parent, string name, int minPresent, int numTasks, int numExercises)
        {
            InitializeComponent();

            //set class attributes
            subjectName = name;
            goalPresent = minPresent;
            numberOfTasks = numTasks;
            numberOfAssignements = numExercises;

            parentField = parent;

            SubjectName.Content = subjectName;

            //Remove % of number
            int goalVote = Int32.Parse(parent.goalVoted.Content.ToString().Remove(parent.goalVoted.Content.ToString().Length-1));

            generalOverview = new GeneralSubjectOverview(goalVote,numExercises, numTasks);
            generalOverview.Height = 225;
            generalOverview.GoalPresent.Content = minPresent.ToString();
            ExerciseStack.Children.Add(generalOverview);
        }

        private int AverageVoted()
        {
            int totalDone = 0, totalAll = 0;
            foreach(ExercisePanel exer in ExerciseStack.Children.OfType<ExercisePanel>())
            {
                totalDone += exer.DoneTasks;
                totalAll += exer.TotalTasks;
            }

            //Prevent dividing by 0 later on
            if (totalDone == 0 && totalAll == 0)
                return 0;

            double average = (double)totalDone / (double) totalAll;
            average = Math.Round(average*100);
            
            return (int) average;
        }

        private void DeleteExerciseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ExerciseStack.Children.Count > 1)
                ExerciseStack.Children.RemoveAt(ExerciseStack.Children.Count - 1);
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            parentField.avgVoted.Content = AverageVoted().ToString()+"%";

            if (Int32.Parse(parentField.avgVoted.Content.ToString().Remove(parentField.avgVoted.Content.ToString().Length-1)) >= (Int32.Parse(parentField.goalVoted.Content.ToString().Remove(parentField.goalVoted.Content.ToString().Length - 1))))
                parentField.avgVoted.Foreground = new SolidColorBrush(Colors.Black);

            parentField.presented.Content = generalOverview.NumPresentations.Content;
            HomeWindow.ReturnToMainMenu();
        }

        private void AddExerciseBtn_Click(object sender, RoutedEventArgs e)
        {
            ExercisePanel exercise = new ExercisePanel(numberOfTasks);
            ExerciseStack.Children.Add(exercise);
        }
    }
}
