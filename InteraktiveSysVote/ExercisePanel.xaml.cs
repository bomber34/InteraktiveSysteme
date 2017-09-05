using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaktionslogik für ExercisePanel.xaml
    /// </summary>
    public partial class ExercisePanel : UserControl
    {
        public int DoneTasks { get; set; }
        public int TotalTasks { get; set; }
        private readonly int indexID;
        private ExerciseWindow parentField;

        public bool IsMinimized { get; set; }
        private Button incDoneBtn, incTotalBtn, decDoneBtn, decTotalBtn;

        public ExercisePanel()
        {
            InitializeComponent();
        }

        public ExercisePanel(ExerciseWindow parent, int allTasks, int currentAssignment)
            : this(parent, 0, allTasks, currentAssignment)
        {}

        public ExercisePanel(ExerciseWindow parent,int done,int allTasks, int currentAssignment)
        {
            InitializeComponent();

            if (done < 0)
                done = 0;

            DoneTasks = done;
            VotedTasksLabel.Content = DoneTasks.ToString();
            TotalTasksLabel.Content = allTasks.ToString();
            TotalTasks = allTasks;

            ExerciseIDTextBlock.Text += currentAssignment.ToString();
            indexID = currentAssignment-1;

            parentField = parent;
            IsMinimized = false;

            incDoneBtn = IncDoneButton;
            incTotalBtn = IncTotalButton;
            decDoneBtn = DecDoneButton;
            decTotalBtn = DecTotalButton;
        }

        /// <summary>
        /// Increases respective textbox number by 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(IncDoneButton)) { 
                VotedTasksLabel.Content = (++DoneTasks).ToString();
            }
            else { 
                TotalTasksLabel.Content = (++TotalTasks).ToString();
            }

            //VotedBox should not be greater than totalBox
            if (DoneTasks > TotalTasks)
            {
                TotalTasksLabel.Content = VotedTasksLabel.Content;
                TotalTasks = DoneTasks;
            }

            parentField.AverageVoted();
            parentField.CalculatedAverageLeftToDo();
              
        }

        /// <summary>
        /// decreases respective textBox number by 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            //Check if action is legit
            int num = (sender.Equals(DecDoneButton) ? DoneTasks : TotalTasks);
            if (num <= 0) //no negative numbers allowed
                return;

            if (sender.Equals(DecDoneButton))
            {
                VotedTasksLabel.Content = (--DoneTasks).ToString();
            }
            else {
                TotalTasksLabel.Content = (--TotalTasks).ToString();
            }

            //TotalBox should not be smaller than VotedBox
            if (TotalTasks < DoneTasks)
            { 
                VotedTasksLabel.Content = TotalTasksLabel.Content;
                DoneTasks = TotalTasks;
            }

            parentField.AverageVoted();
            parentField.CalculatedAverageLeftToDo();
        }

        /// <summary>
        /// If called it will remove the Buttons of this Panel in order of filling less space in the window
        /// </summary>
        /// <param name="minimize"></param>
        public void MinimizePanel(bool minimized)
        {
            if (!minimized) {
                ExercisePanelGrid.Children.Remove(incDoneBtn);
                ExercisePanelGrid.Children.Remove(incTotalBtn);
                ExercisePanelGrid.Children.Remove(decDoneBtn);
                ExercisePanelGrid.Children.Remove(decTotalBtn);
            }
            else
            {
                ReAddButtons();
            }
            IsMinimized = !minimized;
        }

        private void ReAddButtons()
        {
            ExercisePanelGrid.Children.Add(incDoneBtn);
            Grid.SetRow(incDoneBtn, 1);
            Grid.SetColumn(incDoneBtn, 1);

            ExercisePanelGrid.Children.Add(incTotalBtn);
            Grid.SetRow(incTotalBtn, 1);
            Grid.SetColumn(incTotalBtn, 3);

            ExercisePanelGrid.Children.Add(decDoneBtn);
            Grid.SetRow(decDoneBtn, 3);
            Grid.SetColumn(decDoneBtn, 1);

            ExercisePanelGrid.Children.Add(decTotalBtn);
            Grid.SetRow(decTotalBtn, 3);
            Grid.SetColumn(decTotalBtn, 3);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            MinimizePanel(IsMinimized);
        }
    }
}
