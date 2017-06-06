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

        public ExercisePanel()
        {
            InitializeComponent();
        }

        public ExercisePanel(int allTasks)
        {
            InitializeComponent();
            DoneTasks = 0;
            TotalTasksLabel.Content = allTasks.ToString();
            TotalTasks = allTasks;
        }
        
        /// <summary>
        /// Increases respective textbox number by 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            //Label is initialized with legit numbers, therefore parsing will be successful
            int num = (sender.Equals(incDone) ? Int32.Parse(VotedTasksLabel.Content.ToString()) : Int32.Parse(TotalTasksLabel.Content.ToString()));

            num++;

            if (sender.Equals(incDone)) { 
                VotedTasksLabel.Content = num.ToString();
                DoneTasks++;
            }
            else { 
                TotalTasksLabel.Content = num.ToString();
                TotalTasks++;
            }

            //VotedBox should not be greater than totalBox
            if (DoneTasks > TotalTasks)
            {
                TotalTasksLabel.Content = VotedTasksLabel.Content;
                TotalTasks = DoneTasks;
            }
              
        }

        /// <summary>
        /// decreases respective textBox number by 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            //Label is initialized with legit numbers, therefore parsing will be successful
            int num = (sender.Equals(decDone) ? Int32.Parse(VotedTasksLabel.Content.ToString()) : Int32.Parse(TotalTasksLabel.Content.ToString()));

            num--;
            if (num < 0) //no negative numbers allowed
                return;

            if (sender.Equals(decDone))
            {
                VotedTasksLabel.Content = num.ToString();
                DoneTasks--;
            }
            else { 
                TotalTasksLabel.Content = num.ToString();
                TotalTasks--;
            }

            //TotalBox should not be smaller than VotedBox
            if (TotalTasks < DoneTasks)
            { 
                VotedTasksLabel.Content = TotalTasksLabel.Content;
                DoneTasks = TotalTasks;
            }
        }
    }
}
