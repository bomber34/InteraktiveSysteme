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

        public GeneralSubjectOverview()
        {
            InitializeComponent();
        }

        public GeneralSubjectOverview(int averageVoteGoal,int numOfAssignements, int averageExpectedTasks)
        {
            InitializeComponent();
          

            //Calculate average of tasks that the user needs to do per assignment
            double allTasks = (double) (numOfAssignements * averageExpectedTasks);
            double avgTasksToDo = (allTasks / 100.0) * (double)averageVoteGoal;
            avgTasksToDo = avgTasksToDo / (double) numOfAssignements;
            avgTasksToDo = RoundTo2DecimalPoints(avgTasksToDo);

            SetAverageTasksLeftToDoLabel(avgTasksToDo);
        }

        /// <summary>
        /// Takes a double and rounds it to the 2nd decimal point
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double RoundTo2DecimalPoints(double value)
        {
            value *= 100.0;
            int integerVal = (int)value;
            value = (double)integerVal / 100.0;
            return value;
        }

        public void SetAverageTasksLeftToDoLabel(double value)
        {
            if (value == 1.00)
                AvgToDoInfoLabel.Content = "Im Schnitt fehlt noch " + value + " Aufgabe pro Übung";

            else
                AvgToDoInfoLabel.Content = "Im Schnitt fehlen noch " + value + " Aufgaben pro Übung";

        }

        private void PresentUp_Click(object sender, RoutedEventArgs e)
        {
            //NumPresentation is a label which is initialized with a digit, therefore parsing is safe
            int num = Int32.Parse(NumPresentationsLabel.Content.ToString());
            num++;
            NumPresentationsLabel.Content = num.ToString();
        }

        private void PresentDown_Click(object sender, RoutedEventArgs e)
        {
            //NumPresentation is a label which is initialized with a digit, therefore parsing is safe
            int num = Int32.Parse(NumPresentationsLabel.Content.ToString());
            if (num > 0) // There can't be negative presentations
                num--;
            NumPresentationsLabel.Content = num.ToString();
        }

        private void PresentTotalUp_Click(object sender, RoutedEventArgs e)
        {
            //NumPresentation is a label which is initialized with a digit, therefore parsing is safe
            int num = Int32.Parse(GoalPresentLabel.Content.ToString());
            num++;
            GoalPresentLabel.Content = num.ToString();
        }

        private void PresentTotalDown_Click(object sender, RoutedEventArgs e)
        {
            //NumPresentation is a label which is initialized with a digit, therefore parsing is safe
            int num = Int32.Parse(GoalPresentLabel.Content.ToString());
            if (num > 0) // There can't be negative presentations
                num--;
            GoalPresentLabel.Content = num.ToString();
        }
    }
}
