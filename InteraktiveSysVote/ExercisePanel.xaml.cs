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
       
        public ExercisePanel()
        {
            InitializeComponent();
        }

        /**
         * Checks if a text consists only of digits 
         */
        private bool IsDigitOnly(string text)
        {
            Regex regEx = new Regex("[0-9]+");
            return regEx.IsMatch(text);
        }

        /**
         * Checks if a textBlock already contains a 0 as a first symbol
         * true if number is 0, greater than 0 or not existing
         * false if number is for example 011
         */
        private bool IsValidNumber(object sender, string text)
        {
            //Only check if the sender is valid to check
            if (sender.Equals(totalTasks) && !totalTasks.Text.Equals(""))
            {
                return !totalTasks.Text.ElementAt(0).Equals('0');
            }

            else if (sender.Equals(votedTasks) && !votedTasks.Text.Equals(""))
            {
                return !votedTasks.Text.ElementAt(0).Equals('0');
            }

            else
            {
                return true;
            }
        }

        /**
         * Checks the input of an TextBox before it is shown
        */
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool abortAction = !IsDigitOnly(e.Text) || !IsValidNumber(sender, e.Text);
            e.Handled = abortAction;   //If set to true, the text will not be entered
        }

        //Increases the respective TextBox number by 1 if pressed
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            int num = (sender.Equals(incDone) ? Int32.Parse(votedTasks.Text): Int32.Parse(totalTasks.Text));

            num++;

            if (sender.Equals(incDone))
                votedTasks.Text = num.ToString();
            else
                totalTasks.Text = num.ToString();

            //VotedBox should not be greater than totalBox
            if (sender.Equals(incDone) && Int32.Parse(votedTasks.Text) > Int32.Parse(totalTasks.Text))
                totalTasks.Text = votedTasks.Text;
        }

        //Decreases the respective TextBox number by 1 if pressed
        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int num = (sender.Equals(decDone) ? Int32.Parse(votedTasks.Text) : Int32.Parse(totalTasks.Text));

            num--;
            if (num < 0) //no negative numbers allowed
                return;

           
            if (sender.Equals(decDone))
                votedTasks.Text = num.ToString();
            else
                totalTasks.Text = num.ToString();

            //TotalBox should not be smaller than VotedBox
            if (sender.Equals(decTotal) && Int32.Parse(totalTasks.Text) < Int32.Parse(votedTasks.Text))
                votedTasks.Text = totalTasks.Text;
        }
    }
}
