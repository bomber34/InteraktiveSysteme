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
        
        public int avgNumOfTasks, numberOfAssignements, currentNumOfAssignments, originalNumAssigns;
        private SubjectPanel parentField;
        private ScrollViewer ExerciseWindowScrollViewer;
        private StackPanel ExerciseStackPanel;
        private GeneralSubjectOverview generalOverview;

        //Constructors
        public ExerciseWindow()
        {
            InitializeComponent();
        }

        public ExerciseWindow(SubjectPanel parent, int numTasks, int numAssigns)
        {
            InitializeComponent();

            //set class attributes
            avgNumOfTasks = numTasks;

            numberOfAssignements = numAssigns;
            originalNumAssigns = numberOfAssignements;
            currentNumOfAssignments = 0;

            //Remove the %
            int goalVote = Int32.Parse(parent.GoalVotedLabel.Content.ToString()
                .Remove(parent.GoalVotedLabel.Content.ToString().Length - 1));

            parentField = parent;

            //Set Label Content
            SubjectNameLabel.Content = parent.SubjectNameTextBlock.Text;

            //GeneralOverview
            generalOverview = new GeneralSubjectOverview(goalVote, numAssigns, numTasks);
            generalOverview.GoalPresentLabel.Content = parent.GoalPresentLabel.Content;

            //Add generalOverview to DockPanel
            DockPanel.SetDock(generalOverview, Dock.Top);
            ExerciseWindowDockPanel.Children.Add(generalOverview);

            //Initialize dynamic Content
            ExerciseWindowScrollViewer = new ScrollViewer()
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            //Add ScrollViewer to DockPanel
            DockPanel.SetDock(ExerciseWindowScrollViewer, Dock.Top);
            ExerciseWindowDockPanel.Children.Add(ExerciseWindowScrollViewer);

            //Create StackPanel for the Assignments
            ExerciseStackPanel = new StackPanel();
            ExerciseWindowScrollViewer.Content = ExerciseStackPanel;
        }

        //Methods

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

        public int AverageVoted()
        {
            GetTasksDoneAndTotal(out int totalDone, out int totalAll);
            
            //Prevent dividing by 0 later on
            if (totalDone == 0 && totalAll == 0)
                return 0;

            double average = (double)totalDone / (double) totalAll;
            average = Math.Round(average*100);

            generalOverview.AverageVotedTasksLabel.Content = "Durchschnitt votierter Aufgaben: " + 
                                                             GeneralSubjectOverview.RoundTo2DecimalPoints(average) + "%";

            return (int) average;
        }

        /// <summary>
        /// If the done% is lower than goal% -> Colour Red else Black
        /// </summary>
        private void VotedPercentageColourChange()
        {
            // Parsing will work due to this information being checked at previous steps
            int avgVoted = parentField.GetAverageVoted();

            int goalVote = parentField.GetGoalVoted();

            if ( avgVoted >= goalVote)
            { 
                parentField.AvgVotedLabel.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
                parentField.AvgVotedLabel.Foreground = new SolidColorBrush(Colors.Red);
        }

        /// <summary>
        /// Calculates the amount of remaining to do tasks per assignment
        /// </summary>
        public void CalculatedAverageLeftToDo()
        {
            int goalVote = parentField.GetGoalVoted();

            int assignmentsLeft = (numberOfAssignements - currentNumOfAssignments);
            int totalLeftTasks = avgNumOfTasks * assignmentsLeft;

            GetTasksDoneAndTotal(out int totalDone, out int currentTotalTasks);

            int allTasks = totalLeftTasks + currentTotalTasks;
            int leftToDo = (int)Math.Ceiling((((double)(allTasks) / 100.0) * (double)goalVote)) - totalDone;

            if (assignmentsLeft != 0)
            {
                double tasksPerWeek = (double)leftToDo / (double)assignmentsLeft;
                tasksPerWeek = GeneralSubjectOverview.RoundTo2DecimalPoints(tasksPerWeek);
                generalOverview.SetAverageTasksLeftToDoLabel(tasksPerWeek);
            }
            else
                generalOverview.AvgToDoInfoLabel.Content = "Alle Übungen sind vorbei";
        }

        /// <summary>
        /// Use these new values to recalculate the average tasks the user needs to do
        /// </summary>
        /// <param name="averageTasks"></param>
        /// <param name="assignements"></param>
        public void ApplyChanges(int averageTasks, int assignements)
        {
            //Set Label Content
            SubjectNameLabel.Content = parentField.SubjectNameTextBlock.Text;

            avgNumOfTasks = averageTasks;
            numberOfAssignements = assignements;
            originalNumAssigns = numberOfAssignements;

            generalOverview.GoalPresentLabel.Content = parentField.GoalPresentLabel.Content;

            VotedPercentageColourChange();
            CalculatedAverageLeftToDo();
        }

        //Events - Buttons
        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            parentField.SetAverageVoted(AverageVoted());
            VotedPercentageColourChange();

            parentField.PresentedLabel.Content = generalOverview.NumPresentationsLabel.Content;
            HomeWindow.ReturnToMainMenu();
        }

        private void AddExerciseBtn_Click(object sender, RoutedEventArgs e)
        {
            currentNumOfAssignments++;
            ExercisePanel exercise = new ExercisePanel(this, avgNumOfTasks, currentNumOfAssignments);
            ExerciseStackPanel.Children.Insert(0, exercise);
            
            if (currentNumOfAssignments > numberOfAssignements)
                numberOfAssignements = currentNumOfAssignments;

            AverageVoted();
            CalculatedAverageLeftToDo();
        }

        private void DeleteExerciseBtn_Click(object sender, RoutedEventArgs e)
        {
            //Prevent OutOfIndex Exception
            if (ExerciseStackPanel.Children.Count > 0)
            {
                ExerciseStackPanel.Children.RemoveAt(0);
                currentNumOfAssignments--;

                if (currentNumOfAssignments <= originalNumAssigns)
                    numberOfAssignements = originalNumAssigns;

                CalculatedAverageLeftToDo();
            }
        }
    }
}
