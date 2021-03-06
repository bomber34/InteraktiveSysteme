﻿using System;
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

        //Save Files
        private static readonly string DIR = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\Votiergedöns";
        private static readonly int GENERAL_INFO_INTS = 6;
        private static readonly string GENERAL = "#####SUBJECT#####";
        private static readonly string EXERCISE = "#####EXERCISES#####";
        //GENERAL_INFO_ACCESS
        private static readonly int AVG_VOTED = 0, 
            GOAL_VOTED = 1, PRESENTED = 2, GOAL_PRESENTED = 3,
            AVG_TASKS = 4, NUM_ASSIGNS = 5; 

        public MainWindow()
        {
            InitializeComponent();
            mainViewGrid = new Grid();
            homeView = new HomeWindow();

            if(!Directory.Exists(DIR))
                Directory.CreateDirectory(DIR);

            mainViewGrid.Children.Add(homeView);
            MainWindowGrid.Children.Add(mainViewGrid);
            //Load previously created subjects
            LoadSubjectFiles();
        }

        private void LoadSubjectFiles()
        {
            string[] files = Directory.GetFiles(DIR);
            foreach(string file in files)
            {
                //ignore wrong file types
                if (!Regex.IsMatch(file, @"\.sbj$"))
                    continue;

                StreamReader subjFile = new StreamReader(file);

                if (subjFile.ReadLine() != GENERAL)
                    Console.WriteLine("Someone messed with the file"); //Figure a better way for debugging out ...

                LoadSubjectPanel(ref subjFile);
                subjFile.Close();
            }
        }

        //Load the SubjectPanel from the SaveFile
        private void LoadSubjectPanel(ref StreamReader subjFile)
        {
            //General info
            string subjName = subjFile.ReadLine();
            int[] saveInfo = new int[GENERAL_INFO_INTS];

            for (int i = 0; i < GENERAL_INFO_INTS; i++)
            {
                if (!Int32.TryParse(subjFile.ReadLine(), out int info))
                    info = 0; //reset attribute to 0 if someone messed with save File

                saveInfo[i] = info;
            }

            SubjectPanel subjectPan = new SubjectPanel(subjName, saveInfo[GOAL_VOTED],
                saveInfo[GOAL_PRESENTED], saveInfo[AVG_TASKS], saveInfo[NUM_ASSIGNS]);

            subjectPan.SetAverageVoted(saveInfo[AVG_VOTED]);
            subjectPan.PresentedLabel.Content = saveInfo[PRESENTED].ToString();

            //Start recreating the ExerciseWindow
            ExerciseWindow excWin = subjectPan.GetExcerciseWindow();
            excWin.SetGeneralOverviewPresentation(saveInfo[PRESENTED]);

            LoadExerciseWindow(ref subjFile, ref excWin);

            homeView.SubjectStackPanel.Children.Add(subjectPan);
        }

        //Load the ExerciseWindow from the SaveFile
        private void LoadExerciseWindow(ref StreamReader subjFile, ref ExerciseWindow excWin)
        {

            //Another file integrity check
            if (subjFile.ReadLine() != EXERCISE)
            {
                Console.WriteLine("Someone messed with the save file >.>");
            }

            string line;
            string pattern = @"(\d+)\/(\d+)"; //In file: (int)NUM_DONE/(int)NUM_TOTAL
            while ((line = subjFile.ReadLine()) != null)
            {
                if (Regex.IsMatch(line, pattern))
                {
                    Match match = Regex.Match(line, pattern);
                    int done = Int32.Parse(match.Groups[1].Value);
                    int total = Int32.Parse(match.Groups[2].Value);
                    
                    //Can't do more tasks than there exist
                    if (done > total)
                        total = done;
                    excWin.AddExercise(done, total);
                }
            }
        }

        //Saves subjects when program is closed
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Save information about subjects
            int subjectNum = 1;
            foreach(SubjectPanel subject in homeView.SubjectStackPanel.Children.OfType<SubjectPanel>())
            {
                string path = DIR + "\\" + "Subject"+ subjectNum + ".sbj";
                using (StreamWriter sw = File.CreateText(path))
                {
                    //general information -> 7 important dates
                    sw.WriteLine(GENERAL);
                    sw.WriteLine(subject.SubjectNameTextBlock.Text);
                    sw.WriteLine(subject.GetAverageVoted().ToString());
                    sw.WriteLine(subject.GetGoalVoted().ToString());
                    sw.WriteLine(subject.PresentedLabel.Content);
                    sw.WriteLine(subject.GoalPresentLabel.Content);
                    sw.WriteLine(subject.GetAverageNumTasks().ToString());
                    sw.WriteLine(subject.GetNumOfAssignements().ToString());
                    //information for subwindow
                    sw.WriteLine(EXERCISE);
                    Stack<ExercisePanel> exercises = subject.GetExcerciseWindow().GetExercises();
                    foreach (ExercisePanel exPan in exercises)
                    {
                        sw.WriteLine(exPan.VotedTasksLabel.Content + "/" + exPan.TotalTasksLabel.Content);
                    }
                }
                subjectNum++;
            }
            //delete unnecessary files like deleted subjects during usage
            DeleteUnnecessaryFiles(subjectNum);
        }

        //Deletes any file which is not needed for saving any information in the save folder
        private void DeleteUnnecessaryFiles(int limit)
        {
            string[] files = Directory.GetFiles(DIR);
            foreach(string file in files)
            {
                string subjNum = Regex.Match(file, @"(\d+)\.sbj$").Groups[1].Value;
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
