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
    /// Interaktionslogik für GeneralSubjectOverview.xaml
    /// </summary>
    public partial class GeneralSubjectOverview : UserControl
    {
        private int expectedAssignments, avgExpectedTasks, avgVoteGoal;
        string avgToDoInfo;
        double avgTasksToDo;

        public GeneralSubjectOverview()
        {
            InitializeComponent();
        }

        public GeneralSubjectOverview(int averageVoteGoal,int numOfAssignements, int averageExpectedTasks)
        {
            InitializeComponent();
            expectedAssignments = numOfAssignements;
            avgExpectedTasks = averageExpectedTasks;
            avgVoteGoal = averageVoteGoal;

            //Calculate average of tasks that the user needs to do per assignment
            double allTasks = (double) (expectedAssignments * avgExpectedTasks);
            avgTasksToDo = (allTasks / 100.0) * (double)averageVoteGoal;
            avgTasksToDo = avgTasksToDo / (double) expectedAssignments;

            // Cut down to only 2 decimals
            avgTasksToDo *= 100.0;
            int toDo = (int)Math.Round(avgTasksToDo);
            avgTasksToDo = (double)toDo / 100.0;
            avgToDoInfo = "Im Schnitt fehlen noch " + avgTasksToDo.ToString() + " Aufgaben";
            AvgToDoInfoLabel.Content = avgToDoInfo;
         
        }

        private void PresentUp_Click(object sender, RoutedEventArgs e)
        {
            //NumPresentation is a label which is initialized with a digit, therefore parsing is safe
            int num = Int32.Parse(NumPresentations.Content.ToString());
            num++;
            NumPresentations.Content = num.ToString();
        }

        private void PresentDown_Click(object sender, RoutedEventArgs e)
        {
            //NumPresentation is a label which is initialized with a digit, therefore parsing is safe
            int num = Int32.Parse(NumPresentations.Content.ToString());
            if (num > 0) // There can't be negative presentations
                num--;
            NumPresentations.Content = num.ToString();
        }
    }
}
