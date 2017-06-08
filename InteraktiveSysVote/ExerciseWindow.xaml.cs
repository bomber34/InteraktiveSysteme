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
        public int goalPresent, goalVote,numberOfTasks, numberOfAssignements, currentNumOfAssignments, originalNumAssigns;
        private SubjectPanel parentField;
        private ScrollViewer ExerciseWindowScrollViewer;
        private StackPanel ExerciseStackPanel;
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

            //Set Label Content
            SubjectName.Content = subjectName;

            //GeneralOverview
            generalOverview = new GeneralSubjectOverview(goalVote, numExercises, numTasks);
            generalOverview.GoalPresent.Content = minPresent.ToString();

            //Add generalOverview to DockPanel
            DockPanel.SetDock(generalOverview, Dock.Top);
            MainExerciseWindowDockPanel.Children.Add(generalOverview);

            //Initialize dynamic Content
            ExerciseWindowScrollViewer = new ScrollViewer()
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            //Add ScrollViewer to DockPanel
            DockPanel.SetDock(ExerciseWindowScrollViewer, Dock.Top);
            MainExerciseWindowDockPanel.Children.Add(ExerciseWindowScrollViewer);

            //Create StackPanel for the Assignments
            ExerciseStackPanel = new StackPanel();
            ExerciseWindowScrollViewer.Content = ExerciseStackPanel;
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
            foreach (ExercisePanel exer in ExerciseStackPanel.Children.OfType<ExercisePanel>())
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
            if (ExerciseStackPanel.Children.Count > 0)
            {
                ExerciseStackPanel.Children.RemoveAt(ExerciseStackPanel.Children.Count - 1);
                currentNumOfAssignments--;

                if (currentNumOfAssignments <= originalNumAssigns)
                    numberOfAssignements = originalNumAssigns;

                CalculatedAverageLeftToDo();
            }
        }

        /// <summary>
        /// If the done% is lower than goal% -> Colour Red else Black
        /// </summary>
        private void VotedPercentageColourChange()
        {
            if (Int32.Parse(parentField.avgVoted.Content.ToString().Remove(parentField.avgVoted.Content.ToString().Length - 1)) >= (Int32.Parse(parentField.goalVoted.Content.ToString().Remove(parentField.goalVoted.Content.ToString().Length - 1))))
                parentField.avgVoted.Foreground = new SolidColorBrush(Colors.Black);
            else
                parentField.avgVoted.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            parentField.avgVoted.Content = AverageVoted().ToString()+"%";
            VotedPercentageColourChange();
            //TODO change colour also after editing

            parentField.presented.Content = generalOverview.NumPresentations.Content;
            HomeWindow.ReturnToMainMenu();
        }

        private void AddExerciseBtn_Click(object sender, RoutedEventArgs e)
        {
            currentNumOfAssignments++;
            ExercisePanel exercise = new ExercisePanel(this, numberOfTasks, currentNumOfAssignments);
            ExerciseStackPanel.Children.Add(exercise);
            
            if (currentNumOfAssignments > numberOfAssignements)
                numberOfAssignements = currentNumOfAssignments;

            CalculatedAverageLeftToDo();
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
            int leftToDo = (int)Math.Ceiling((((double)(allTasks) / 100.0)*(double) goalVote)) - totalDone;

            if (assignmentsLeft != 0)
            {
                double tasksPerWeek = (double)leftToDo / (double)assignmentsLeft;
                tasksPerWeek = GeneralSubjectOverview.RoundTo2DecimalPoints(tasksPerWeek);
                generalOverview.SetAverageTasksLeftToDoLabel(tasksPerWeek);
            }
            else
                generalOverview.AvgToDoInfoLabel.Content = "Alle Übungen sind vorbei";
        }

        public void ApplyChanges(string subName, int goalVote, int goalPresent, int averageTasks, int assignements)
        {
            //Set Label Content
            subjectName = subName;
            SubjectName.Content = subjectName;

            //Reset class Attributes
            this.goalVote = goalVote;
            this.goalPresent = goalPresent;
            numberOfTasks = averageTasks;
            numberOfAssignements = assignements;
            originalNumAssigns = numberOfAssignements;

            generalOverview.GoalPresent.Content = goalPresent.ToString();
            generalOverview.ApplyChanges(goalVote, assignements, averageTasks);

            VotedPercentageColourChange();

            CalculatedAverageLeftToDo();
        }
    }
}
