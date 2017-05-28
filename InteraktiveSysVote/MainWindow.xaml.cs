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
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int noOfSubs = 1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddSubjectBtn_Click(object sender, RoutedEventArgs e)
        {
            //Simple test 
            // Just add elements dynamically to the stack
            SubjectPanel example = new SubjectPanel(noOfSubs++);
            SubjectStack.Children.Add(example);
            
        }

        private void EditSubjectBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSubjectBtn_Click(object sender, RoutedEventArgs e)
        {
            //Only remove if there exist removable elements
            if (SubjectStack.Children.Count != 0) {
                SubjectStack.Children.RemoveAt(SubjectStack.Children.Count - 1);
                noOfSubs--;
            }
        }
    }
}
