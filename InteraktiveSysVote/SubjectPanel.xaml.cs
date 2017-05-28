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
        private bool test = false;
        private string content;

        public SubjectPanel()
        {
            InitializeComponent();
        }

        public SubjectPanel(int subjectNo)
        {
            InitializeComponent();
            Fach.Content += subjectNo.ToString();
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //TODO: If double clicked it should go to the specific subject window
            if(!test)
            {
                content = Fach.Content.ToString();
                Fach.Content = "Is Selected";
                test = true;
            }

            else
            {
                Fach.Content = content;
                test = false;
            }
        }
    }
}
