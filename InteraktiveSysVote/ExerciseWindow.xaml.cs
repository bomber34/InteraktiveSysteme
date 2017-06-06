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
        private int goalPresent, goalVote,numberOfTasks, numberOfAssignements, currentNumOfAssignments, originalNumAssigns;
        private SubjectPanel parentField;
        private GeneralSubjectOverview generalOverview;

        public ExerciseWindow()
        {
            InitializeComponent();
        }

        public ExerciseWindow(SubjectPanel parent, string name,int minVote ,int minPresent, int numTasks, int numExercises)
        {
            InitializeComponent();

            //set class attributes
            subjectName = name;
            goalPresent = minPresent;
            numberOfTasks = numTasks;
            numberOfAssignements = numExercises;
            originalNumAssigns = numberOfAssignements;
            currentNumOfAssignments = 0;
            goalVote = minVote;

            parentField = parent;

            SubjectName.Content = subjectName;

            generalOverview = new GeneralSubjectOverview(goalVote,numExercises, numTasks);
            generalOverview.Height = 225;
            generalOverview.GoalPresent.Content = minPresent.ToString();
            
            //Workaround to place the overview perfectly into the window
            ExerciseStack.Children.Add(generalOverview);
        }

        /// <summary>
        /// Gets the sums of all voted and total Tasks fields in all exercisePanels
        /// </summary>
        /// <param name="totalDone"></param>
        /// <param name="totalAll"></param>
        private void GetTasksDoneAndTotal(out int totalDone, out int totalAll)
        {
            totalDone = 0;
            totalAll = 0;
            foreach (ExercisePanel exer in ExerciseStack.Children.OfType<ExercisePanel>())
            {
                totalDone += exer.DoneTasks;
                totalAll += exer.TotalTasks;
            }
        }

        private int AverageVoted()
        {
            GetTasksDoneAndTotal(out int totalDone, out int totalAll);
            
            //Prevent dividing by 0 later on
            if (totalDone == 0 && totalAll == 0)
                return 0;

            double average = (double)totalDone / (double) totalAll;
            average = Math.Round(average*100);
            
            return (int) average;
        }

        private void DeleteExerciseBtn_Click(object sender, RoutedEventArgs e)
        {
            //The first element is the general overview
            if (ExerciseStack.Children.Count > 1)
            {
                ExerciseStack.Children.RemoveAt(ExerciseStack.Children.Count - 1);
                currentNumOfAssignments--;

                if (currentNumOfAssignments <= originalNumAssigns)
                    numberOfAssignements = originalNumAssigns;
            }
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
            ExercisePanel exercise = new ExercisePanel(this, numberOfTasks);
            ExerciseStack.Children.Add(exercise);
            currentNumOfAssignments++;

            if (currentNumOfAssignments > numberOfAssignements)
                numberOfAssignements = currentNumOfAssignments;
        }

        /// <summary>
        /// Calculates the amount of remaining to do tasks per assignment
        /// </summary>
        public void CalculatedAverageLeftToDo()
        {
            int assignmentsLeft = (numberOfAssignements - currentNumOfAssignments);
            int totalLeftTasks = numberOfTasks * assignmentsLeft;
            GetTasksDoneAndTotal(out int totalDone, out int currentTotalTasks);
            int allTasks = totalLeftTasks + currentTotalTasks;
            int leftToDo = (int)Math.Ceiling((((double)(allTasks) / 100.0)*(double)goalVote)) - totalDone;

            if (assignmentsLeft != 0)
            {
                double tasksPerWeek = (double)leftToDo / (double)assignmentsLeft;
                tasksPerWeek = GeneralSubjectOverview.RoundTo2DecimalPoints(tasksPerWeek);
                generalOverview.SetAverageTasksLeftToDoLabel(tasksPerWeek);
            }
            else
                generalOverview.AvgToDoInfoLabel.Content = "Alle Übungen sind vorbei";
        }
    }
}
