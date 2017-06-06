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
            // TODO add average voting in % and maybe how many tasks are expected
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
