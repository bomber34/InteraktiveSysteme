﻿using System;
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
        private ExerciseWindow parentField;

        private bool isMinimized;

        public ExercisePanel()
        {
            InitializeComponent();
        }

        public ExercisePanel(ExerciseWindow parent,int allTasks, int currentAssignment)
        {
            InitializeComponent();
            DoneTasks = 0;
            TotalTasksLabel.Content = allTasks.ToString();
            TotalTasks = allTasks;

            ExerciseIDTextBlock.Text += currentAssignment.ToString();

            parentField = parent;
            isMinimized = false;
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

        private void MinimizePanel(bool minimize)
        {
            if (minimize) {
                ExercisePanelGrid.Children.Remove(IncDoneButton);
                ExercisePanelGrid.Children.Remove(IncTotalButton);
                ExercisePanelGrid.Children.Remove(DecDoneButton);
                ExercisePanelGrid.Children.Remove(DecTotalButton);
            }
            else
            {
                // TODO: Add buttons back to their original position dynamically
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            isMinimized = isMinimized ? false : true;
            MinimizePanel(isMinimized);
        }
    }
}
