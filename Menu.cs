using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Minesweeper
{
    //*************************************************************
    //Enums
    //*************************************************************
    public enum MenuState { Start, Instructions, Records, None };

    internal class Menu
    {
        //*************************************************************
        //Fields
        //*************************************************************
        private string mText;
        private string mPath;
        private string[] mRecords;

        //*************************************************************
        //Constructors
        //*************************************************************
        public Menu()
        {
            this.mText = string.Empty; // empty string
            this.mPath = $"{Application.StartupPath}\\Records.txt"; // set records file path
        }

        //*************************************************************
        //Properties
        //*************************************************************
        public string Text { get { return this.mText; } }

        //*************************************************************
        //Methods
        //*************************************************************
        public void Draw(Graphics g, MenuState State)
        {
            if (State == MenuState.Start)
                g.DrawImage(GameAssets.MenuStart, 0, 0, 1000, 900);
            if (State == MenuState.Instructions)
                g.DrawImage(GameAssets.MenuInstructions, 0, 0, 1000, 900);
            if (State == MenuState.Records)
                g.DrawImage(GameAssets.MenuRecords, 0, 0, 1000, 900);

        }

        public void LoadRecords()
        {
            // init the streamreader
            using (StreamReader oSR = new StreamReader(this.mPath))
            {
                // get the number of records (topline)
                int TotalRecords = File.ReadAllLines(this.mPath).Length;
                mRecords = new string[TotalRecords];

                this.mText = string.Empty;

                string Header = $"{"Time:",-15} {"Difficulty:",-15} {"Outcome:",-15} {"Anti Mine Mode:",-15}";
                this.mText = Header + "\n";

                // for loop to store this data in the array
                for (int i = 0; i < TotalRecords; i++)
                {
                    // add to array
                    mRecords[i] = oSR.ReadLine();

                    // split string into individual components
                    string Time = mRecords[i].Split(';')[0];
                    string Difficulty = mRecords[i].Split(';')[1];
                    string Outcome = mRecords[i].Split(';')[2];
                    string AntiMineMode = mRecords[i].Split(';')[3];

                    // format time
                    string TimeFormatted = (int.Parse(Time) / 60).ToString("D2") + ":" + (int.Parse(Time) % 60).ToString("D2");

                    // append to string
                    string Content = $"{TimeFormatted, -15} {Difficulty, -15} {Outcome, -15} {AntiMineMode, -15}";
                    this.mText += Content + "\n";
                }
            }
        }

        public void WriteRecords(string Time, string Difficulty, string Outcome, string AntiMineMode)
        {
            // init the streamwriter
            using (StreamWriter oSW = new StreamWriter(this.mPath, append: true))
            {
                // write the new line
                oSW.WriteLine(string.Join(";", Time, Difficulty, Outcome, AntiMineMode));
            }
        }

        public void SortRecords(SortOption Option, SortType Type)
        {
            // variables
            int Difficulty = 0;
            int Outcome = 0;
            int AntiMineMode = 0;
            int NextDifficulty = 0;
            int NextOutcome = 0;
            int NextAntiMineMode = 0;

            // bubble sort
            for (int i = 0; i < mRecords.Length - 1; i++)
            {
                for (int j = 0; j < mRecords.Length - 1 - i; j++)
                {
                    // split string into individual components
                    string TimeString = mRecords[j].Split(';')[0];
                    string DifficultyString = mRecords[j].Split(';')[1];
                    string OutcomeString = mRecords[j].Split(';')[2];
                    string AntiMineModeString = mRecords[j].Split(';')[3];

                    // parse everything to an int
                    int Time = int.Parse(TimeString);
                    
                    if (DifficultyString == "Easy")
                        Difficulty = 1;
                    if (DifficultyString == "Medium")
                        Difficulty = 2;
                    if (DifficultyString == "Hard")
                        Difficulty = 3;

                    if (OutcomeString == "Lose")
                        Outcome = 0;
                    if (OutcomeString == "Win")
                        Outcome = 1;

                    if (AntiMineModeString == "False")
                        AntiMineMode = 0;
                    if (AntiMineModeString == "True")
                        AntiMineMode = 1;                    

                    // do again for the next line
                    string NextTimeString = mRecords[j + 1].Split(';')[0];
                    string NextDifficultyString = mRecords[j + 1].Split(';')[1];
                    string NextOutcomeString = mRecords[j + 1].Split(';')[2];
                    string NextAntiMineModeString = mRecords[j + 1].Split(';')[3];

                    // parse to int
                    int NextTime = int.Parse(NextTimeString);

                    if (NextDifficultyString == "Easy")
                        NextDifficulty = 1;
                    if (NextDifficultyString == "Medium")
                        NextDifficulty = 2;
                    if (NextDifficultyString == "Hard")
                        NextDifficulty = 3;

                    if (NextOutcomeString == "Lose")
                        NextOutcome = 0;
                    if (NextOutcomeString == "Win")
                        NextOutcome = 1;

                    if (NextAntiMineModeString == "False")
                        NextAntiMineMode = 0;
                    if (NextAntiMineModeString == "True")
                        NextAntiMineMode = 1;

                    // sorting by options and type
                    if (Option == SortOption.Time)
                    {
                        if (Type == SortType.Ascending)
                        {
                            if (Time > NextTime)
                            {
                                string Temp = mRecords[j];
                                mRecords[j] = mRecords[j + 1];
                                mRecords[j + 1] = Temp;
                            }
                        }
                        else
                        {
                            if (Time < NextTime)
                            {
                                string Temp = mRecords[j];
                                mRecords[j] = mRecords[j + 1];
                                mRecords[j + 1] = Temp;
                            }
                        }
                    }
                    if (Option == SortOption.Difficulty)
                    {
                        if (Type == SortType.Ascending)
                        {
                            if (Difficulty > NextDifficulty)
                            {
                                string Temp = mRecords[j];
                                mRecords[j] = mRecords[j + 1];
                                mRecords[j + 1] = Temp;
                            }
                        }
                        else
                        {
                            if (Difficulty < NextDifficulty)
                            {
                                string Temp = mRecords[j];
                                mRecords[j] = mRecords[j + 1];
                                mRecords[j + 1] = Temp;
                            }
                        }
                    }
                    if (Option == SortOption.Outcome)
                    {
                        if (Type == SortType.Ascending)
                        {
                            if (Outcome > NextOutcome)
                            {
                                string Temp = mRecords[j];
                                mRecords[j] = mRecords[j + 1];
                                mRecords[j + 1] = Temp;
                            }
                        }
                        else
                        {
                            if (Outcome < NextOutcome)
                            {
                                string Temp = mRecords[j];
                                mRecords[j] = mRecords[j + 1];
                                mRecords[j + 1] = Temp;
                            }
                        }
                    }
                    if (Option == SortOption.AntiMineMode)
                    {
                        if (Type == SortType.Ascending)
                        {
                            if (AntiMineMode > NextAntiMineMode)
                            {
                                string Temp = mRecords[j];
                                mRecords[j] = mRecords[j + 1];
                                mRecords[j + 1] = Temp;
                            }
                        }
                        else
                        {
                            if (AntiMineMode < NextAntiMineMode)
                            {
                                string Temp = mRecords[j];
                                mRecords[j] = mRecords[j + 1];
                                mRecords[j + 1] = Temp;
                            }
                        }
                    }
                }
            }
            // append
            this.mText = string.Empty;

            string Header = $"{"Time:",-15} {"Difficulty:",-15} {"Outcome:",-15} {"Anti Mine Mode:",-15}";
            this.mText = Header + "\n";

            for (int i = 0; i < mRecords.Length; i++)
            {
                // split string into individual components
                string TimeSorted = mRecords[i].Split(';')[0];
                string DifficultySorted = mRecords[i].Split(';')[1];
                string OutcomeSorted = mRecords[i].Split(';')[2];
                string AntiMineModeSorted = mRecords[i].Split(';')[3];

                // format time
                string TimeSortedFormatted = (int.Parse(TimeSorted) / 60).ToString("D2") + ":" + (int.Parse(TimeSorted) % 60).ToString("D2");

                // append to string
                string Content = $"{TimeSortedFormatted,-15} {DifficultySorted,-15} {OutcomeSorted,-15} {AntiMineModeSorted,-15}";
                this.mText += Content + "\n";
            }
        }
    }
}