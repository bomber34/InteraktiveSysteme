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
using System.IO;
using System.Text.RegularExpressions;

namespace InteraktiveSysVote
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static HomeWindow homeView;
        public static Grid mainViewGrid;
        private static readonly int NUM_SUBJECT_INFOS = 7;
        private static readonly string DIR = "subjects";

        public MainWindow()
        {
            InitializeComponent();
            mainViewGrid = new Grid();
            homeView = new HomeWindow();

            if(!Directory.Exists(DIR))
                Directory.CreateDirectory(DIR);

            mainViewGrid.Children.Add(homeView);
            MainWindowGrid.Children.Add(mainViewGrid);
            LoadFiles();
        }

        private void LoadFiles()
        {
            string[] files = Directory.GetFiles(DIR);
            foreach(string file in files)
            {
                //TODO: Add loading stuff
            }
        }

        //Saves subjects
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Save information about subjects
            int subjectNum = 1;
            foreach(SubjectPanel subject in homeView.SubjectStackPanel.Children.OfType<SubjectPanel>())
            {
                string path = "subjects/subj" + subjectNum+".sbj";
                using (StreamWriter sw = File.CreateText(path))
                {
                    //general information -> 7 important dates
                    sw.WriteLine("#####SUBJECT#####");
                    sw.WriteLine(subject.SubjectNameTextBlock.Text);
                    sw.WriteLine(subject.GetAverageVoted().ToString());
                    sw.WriteLine(subject.GetGoalVoted().ToString());
                    sw.WriteLine(subject.PresentedLabel.Content);
                    sw.WriteLine(subject.GoalPresentLabel.Content);
                    sw.WriteLine(subject.GetAverageNumTasks().ToString());
                    sw.WriteLine(subject.GetNumOfAssignements().ToString());
                    //information for subwindow
                    sw.WriteLine("#####EXERCISES#####");
                    Stack<ExercisePanel> exercises = subject.GetExcerciseWindow().GetExercises();
                    foreach (ExercisePanel exPan in exercises)
                    {
                        sw.WriteLine(exPan.VotedTasksLabel.Content + "/" + exPan.TotalTasksLabel.Content);
                    }
                }
                subjectNum++;
            }
            //delete unnecessary files
            DeleteUnnecessaryFiles(subjectNum);
        }

        //Deletes any file which is not needed for saving any information
        private void DeleteUnnecessaryFiles(int limit)
        {
            string[] files = Directory.GetFiles(DIR);
            foreach(string file in files)
            {
                string subjNum = Regex.Match(file, @"\d+").Value;
                if (Int32.TryParse(subjNum, out int check))
                {
                    if (check >= limit)
                    {
                        File.Delete(DIR+"/" + file);
                    }
                }
                else
                    File.Delete(DIR"/" + file);
            }
        }
    }
}
