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
            TotalTasksTextBox.Text = allTasks.ToString();
            TotalTasks = allTasks;
        }
        
        /// <summary>
        /// Checks if text only consists of digits
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsDigitOnly(string text)
        {
            Regex regEx = new Regex("[0-9]+");
            return regEx.IsMatch(text);
        }

        /// <summary>
        /// Checks if a valid number is entered i.e. a number with sensible digits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="text"></param>
        /// <returns>Returns false if number starts with a 0 followed by more digits</returns>
        private bool IsValidNumber(object sender, string text)
        {
            //Only check if the sender is valid to check
            if (sender.Equals(TotalTasksTextBox) && !TotalTasksTextBox.Text.Equals(""))
            {
                return !TotalTasksTextBox.Text.ElementAt(0).Equals('0');
            }

            else if (sender.Equals(VotedTasksTextBox) && !VotedTasksTextBox.Text.Equals(""))
            {
                return !VotedTasksTextBox.Text.ElementAt(0).Equals('0');
            }

            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks text before it is shown on screen. If invalid it won't be displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool abortAction = !IsDigitOnly(e.Text) || !IsValidNumber(sender, e.Text);
            e.Handled = abortAction;   //If set to true, the text will not be entered
        }

        /// <summary>
        /// Tries to parse a respective TextBox, if it fails it sets the Text back to the previous valid value
        /// </summary>
        /// <param name="txt2Parse">Textbox text that should only contain digits</param>
        /// <param name="votedTasksBox">if it is the votedTasks or totalTasks text box</param>
        /// <returns></returns>
        private int ParseTextBox(string txt2Parse, bool votedTasksBox)
        {
            if(Int32.TryParse(txt2Parse, out int num))
            {
                return num;
            }
            else
            {
                if (votedTasksBox)
                {
                    VotedTasksTextBox.Text = DoneTasks.ToString();
                    return DoneTasks;
                }
                else
                {
                    TotalTasksTextBox.Text = TotalTasks.ToString();
                    return TotalTasks;
                }
            }
        }

        
        /// <summary>
        /// Increases respective textbox number by 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            int num = (sender.Equals(incDone) ? ParseTextBox(VotedTasksTextBox.Text, true): ParseTextBox(TotalTasksTextBox.Text, false));

            num++;

            if (sender.Equals(incDone)) { 
                VotedTasksTextBox.Text = num.ToString();
                DoneTasks++;
            }
            else { 
                TotalTasksTextBox.Text = num.ToString();
                TotalTasks++;
            }

            //VotedBox should not be greater than totalBox
            if (DoneTasks > TotalTasks)
            {
                TotalTasksTextBox.Text = VotedTasksTextBox.Text;
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
            int num = (sender.Equals(decDone) ? ParseTextBox(VotedTasksTextBox.Text, true) : ParseTextBox(TotalTasksTextBox.Text, false));

            num--;
            if (num < 0) //no negative numbers allowed
                return;

            if (sender.Equals(decDone))
            {
                VotedTasksTextBox.Text = num.ToString();
                DoneTasks--;
            }
            else { 
                TotalTasksTextBox.Text = num.ToString();
                TotalTasks--;
            }

            //TotalBox should not be smaller than VotedBox
            if (TotalTasks < DoneTasks)
            { 
                VotedTasksTextBox.Text = TotalTasksTextBox.Text;
                DoneTasks = TotalTasks;
            }
        }
    }
}
