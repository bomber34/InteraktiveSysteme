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
using System.Diagnostics;

namespace InteraktiveSysVote
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static HomeWindow homeView;
        public static Grid mainViewGrid;

        //Save Files
        private static readonly string DIR = "subjects";
        private static readonly string GENERAL = "#####SUBJECT#####";
        private static readonly string EXERCISE = "#####EXERCISES#####";

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
                string path = Directory.GetCurrentDirectory() + "\\" + file;
                StreamReader subjFile = new StreamReader(path);
                if (subjFile.ReadLine() != GENERAL) {
                    continue;
                }

                //General info
                string subjName = subjFile.ReadLine();

                if (!Int32.TryParse(subjFile.ReadLine(), out int avgVoted)) {
                    continue;
                }

                if (!Int32.TryParse(subjFile.ReadLine(), out int goalVoted)) {
                    continue;
                }

                if (!Int32.TryParse(subjFile.ReadLine(), out int presented)) {
                    continue;
                }

                if (!Int32.TryParse(subjFile.ReadLine(), out int goalPresented))
                {
                    continue;
                }
                   

                if (!Int32.TryParse(subjFile.ReadLine(), out int avgTasks)) {
                    continue;
                }
                if (!Int32.TryParse(subjFile.ReadLine(), out int assignments)) {
                    continue;
                }

                SubjectPanel subjectPan = new SubjectPanel(subjName, goalVoted, goalPresented, avgTasks, assignments);
                subjectPan.SetAverageVoted(avgVoted);
                subjectPan.PresentedLabel.Content = presented.ToString();

                ExerciseWindow excPan = subjectPan.GetExcerciseWindow();
                excPan.SetGeneralOverviewPresentation(presented);

                //Exercises
                if (subjFile.ReadLine() != EXERCISE) {
                    continue;
                }

                string line;
                string pattern = @"(\d+)\/(\d+)";
                while((line = subjFile.ReadLine()) != null)
                {
                    if (Regex.IsMatch(line, pattern))
                    { 
                        Match match = Regex.Match(line, pattern);
                        int done = Int32.Parse(match.Groups[1].Value);
                        int total = Int32.Parse(match.Groups[2].Value);
                        if (done > total)
                            total = done;
                        excPan.AddExercise(done, total);
                    }
                }
                homeView.SubjectStackPanel.Children.Add(subjectPan);
                subjFile.Close();
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
                        File.Delete(file);
                    }
                }
                else
                    File.Delete(file);
            }
        }
    }
}
