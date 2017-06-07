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
        private ExerciseWindow exerciseMenu;

        public SubjectPanel()
        {
            InitializeComponent();
        }

        public SubjectPanel(string subjectName, int avgVoteGoal, int minPresent, int numOfTasks, int numOfExercises)
        {
            InitializeComponent();
            Fach.Content = subjectName;
            goalVoted.Content = avgVoteGoal.ToString()+"%";
            GoalPresent.Content = minPresent.ToString();

            OpenExerciseWindowBtn.Content = "ABC";

            exerciseMenu = new ExerciseWindow(this, subjectName, avgVoteGoal ,minPresent, numOfTasks , numOfExercises);
            
        }


        ///private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        private void OpenExerciseWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowGrid.Children.RemoveAt(0);
            MainWindow.mainWindowGrid.Children.Add(exerciseMenu);
        }
    }
}
