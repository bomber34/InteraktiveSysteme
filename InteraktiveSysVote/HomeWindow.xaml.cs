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
    /// Interaktionslogik für HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : UserControl
    {
        private int noOfSubs = 1;
        public HomeWindow()
        {
            InitializeComponent();
        }

        private void AddSubjectBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteSubjectBtn_Click(object sender, RoutedEventArgs e)
        {
            //Only remove if there exist removable elements
            if (SubjectStack.Children.Count != 0)
            {
                SubjectStack.Children.RemoveAt(SubjectStack.Children.Count - 1);
                noOfSubs--;
            }
        }

        private void editSubjectBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addSubjectBtn_Click_1(object sender, RoutedEventArgs e)
        {
            //Simple test 
            // Just add elements dynamically to the stack
            ///TODO: change to a better listing
            SubjectPanel example = new SubjectPanel(noOfSubs++);
            SubjectStack.Children.Add(example);
            //Example for exercise panel Look
            ///Todo: remove later
            ExercisePanel exercise = new ExercisePanel();
            SubjectStack.Children.Add(exercise);

        }
    }
}
