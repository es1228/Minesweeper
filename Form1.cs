using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * Author: Evan Sidhu
 * Date: 1/16/2025
 * Purpose: A Simple MineSweeper Game
 */

// enums
public enum SortOption { Time, Difficulty, Outcome, AntiMineMode }
public enum SortType { Ascending, Descending }
public enum GameState { None, GameOver, Win, InProgress}
public enum Difficulty { Easy, Medium, Hard }

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        // global variables
        MineFields oMineFields;
        GameState oGameState = GameState.None;
        Menu oMenu = new Menu();
        MenuState oMenuState = MenuState.Start;
        Difficulty oDifficulty = Difficulty.Easy;
        int CellSize;
        int Rows;
        int Cols;
        bool FirstClick = false;
        bool SafeClick = true;
        bool AntiMines = false;
        bool AntiFlags = false;
        bool SortTimeAscending = false;
        bool SortDifficultyAscending = false;
        bool SortOutcomeAscending = false;
        bool SortAntiMineModeAscending = false;
        int ElapsedTime = 0;
        int FlagCount;
        int AntiFlagCount;
        int GridX;
        int GridY;
        string Time;
        string DifficultyLevel;
        string Outcome;

        public Form1()
        {
            InitializeComponent();
            this.Icon = GameAssets.Minesweeper;

            // fix scaling
            if (this.WindowState != FormWindowState.Maximized)
            {
                Rectangle ScreenArea = Screen.FromControl(this).WorkingArea;
                int TargetHeight = Math.Min(900, (int)(ScreenArea.Height * 0.9));
                int TargetWidth = (int)(TargetHeight * (1000 / 900));
                this.ClientSize = new Size(TargetWidth, TargetHeight);
                this.CenterToScreen();
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // initial checks
            if (oGameState != GameState.InProgress)
                return;

            // get row and col using relative x and y
            int RelX = e.X - GridX;
            int RelY = e.Y - GridY;

            if (RelX < 0 || RelY < 0)
                return;

            int Col = RelX / CellSize;
            int Row = RelY / CellSize;

            if (Row < 0 || Col < 0 || Row >= Rows || Col >= Cols)
                return;

            MineField Field = oMineFields.GetField(Row, Col);

            if (e.Button == MouseButtons.Left)
            {
                // first click logic
                if (FirstClick)
                {
                    if (Field.State == FieldState.None)
                    {
                        if (SafeClick)
                            oMineFields.SetProtectedSquares(Row, Col);

                        oMineFields.Shuffle();
                        oMineFields.AssignAdjacents();
                        tmrGame.Start();
                        FirstClick = false;
                    }
                    else
                        return;
                }

                // check for flags and question
                if (Field.State == FieldState.Flagged || Field.State == FieldState.AntiFlagged
                    || Field.State == FieldState.Question || Field.State == FieldState.AntiQuestion)
                    return;

                // check for game overs
                if (Field.Type == FieldType.Mine)
                {
                    Field.State = FieldState.MineExplode;
                    oGameState = GameState.GameOver;
                }
                if (Field.Type == FieldType.AntiMine)
                {
                    Field.State = FieldState.AntiMineExplode;
                    oGameState = GameState.GameOver;
                }

                // reveal clicked field
                oMineFields.Reveal(Row, Col);
            }
            else if (e.Button == MouseButtons.Right && !FirstClick)
            {
                // normal flag/question logic
                if (Field.Visibility != FieldVisibility.Revealed)
                {
                    if (!AntiFlags)
                    {
                        if (Field.State == FieldState.AntiFlagged)
                        {
                            Field.State = FieldState.Flagged;
                            AntiFlagCount++;
                            FlagCount--;
                        }
                        else if (Field.State != FieldState.Flagged && Field.State != FieldState.Question && FlagCount > 0)
                        {
                            Field.State = FieldState.Flagged;
                            FlagCount--;
                        }
                        else if (Field.State == FieldState.Flagged)
                        {
                            Field.State = FieldState.Question;
                            FlagCount++;
                        }
                        else
                            Field.State = FieldState.None;
                    }
                    // anti flag/question logic
                    else
                    {
                        if (Field.State == FieldState.Flagged)
                        {
                            Field.State = FieldState.AntiFlagged;
                            FlagCount++;
                            AntiFlagCount--;
                        }
                        else if (Field.State != FieldState.AntiFlagged && Field.State != FieldState.AntiQuestion && AntiFlagCount > 0)
                        {
                            Field.State = FieldState.AntiFlagged;
                            AntiFlagCount--;
                        }
                        else if (Field.State == FieldState.AntiFlagged)
                        {
                            Field.State = FieldState.AntiQuestion;
                            AntiFlagCount++;
                        }
                        else
                            Field.State = FieldState.None;
                    }
                }
            }

            // check for gamestate updates
            CheckWin();
            this.Refresh();
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // scaling fixes
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            if (oMineFields != null)
            {
                // relative x and y
                GridX = ClientSize.Width / 2 - CellSize * Cols / 2;
                GridY = ClientSize.Height / 2 - CellSize * Rows / 2;

                // position and size
                int CenterX = ClientSize.Width / 2;
                int TopY = ClientSize.Height / 40;
                int IconSize = Math.Max(30, ClientSize.Height / 18);
                int LabelY = TopY + (IconSize / 2) - (lblFlagsRemaining.Height / 2);

                // draw
                oMineFields.Draw(e.Graphics, GridX, GridY);

                // positioning logic for top bar icons
                if (AntiMines)
                {
                    int Spacing = IconSize + 60;

                    e.Graphics.DrawImage(GameAssets.FlagsRemaining, CenterX - Spacing - 80, TopY, IconSize, IconSize);
                    lblFlagsRemaining.Location = new Point(CenterX - Spacing - 35, LabelY);

                    e.Graphics.DrawImage(GameAssets.AntiFlagsRemaining, CenterX - 40, TopY, IconSize, IconSize);
                    lblAntiFlagsRemaining.Location = new Point(CenterX + 5, LabelY);

                    e.Graphics.DrawImage(GameAssets.Clock, CenterX + Spacing + 10, TopY, IconSize, IconSize);
                    lblTime.Location = new Point(CenterX + Spacing + 55, LabelY);
                }
                else
                {
                    int Spacing = 100;

                    e.Graphics.DrawImage(GameAssets.FlagsRemaining, CenterX - Spacing, TopY, IconSize, IconSize);
                    lblFlagsRemaining.Location = new Point(CenterX - Spacing + IconSize + 5, LabelY);

                    e.Graphics.DrawImage(GameAssets.Clock, CenterX + 20, TopY, IconSize, IconSize);
                    lblTime.Location = new Point(CenterX + 20 + IconSize + 5, LabelY);
                }
                
                // label updates
                lblFlagsRemaining.Text = FlagCount.ToString();
                lblAntiFlagsRemaining.Text = AntiFlagCount.ToString();
            }

            // menu management
            if (oMenuState == MenuState.Start)
            {
                oMenu.Draw(e.Graphics, oMenuState, ClientSize.Width, ClientSize.Height);
                lblInfo.Text = $"Safe First Click: {SafeClick.ToString()}, Anti Mines: {AntiMines.ToString()}";
            }
            if (oMenuState == MenuState.Instructions)
            {
                oMenu.Draw(e.Graphics, oMenuState, ClientSize.Width, ClientSize.Height);
                lblInfo.Text = "Press ESC To Return To Menu";
            }
            if (oMenuState == MenuState.Records)
            {
                // scaling fixes
                oMenu.Draw(e.Graphics, oMenuState, ClientSize.Width, ClientSize.Height);
                int RecY = (int)(ClientSize.Height * 0.33);
                txtRecords.Width = (int)(ClientSize.Width * 0.9);
                txtRecords.Height = (int)(ClientSize.Height * 0.6);
                txtRecords.Location = new Point((ClientSize.Width - txtRecords.Width) / 2, RecY);
                lblInfo.Text = "Press ESC To Return To Menu";
            }
            
            // update game info label
            if (oGameState == GameState.InProgress)
            {
                if (AntiMines)
                    lblInfo.Text = $"Anti (Blue) Flags: {AntiFlags.ToString()}\nPress ESC To Return To Menu\nPress 1 To Toggle Between Red/Blue Flags";
                else
                    lblInfo.Text = "Press ESC To Return To Menu";
            }
            // center info label
            int InfoYOffset = lblInfo.Height + 20;
            lblInfo.Location = new Point((ClientSize.Width - lblInfo.Width) / 2, ClientSize.Height - InfoYOffset);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // start menu options
            if (oMenuState == MenuState.Start)
            {
                if (e.KeyCode == Keys.D1)
                    oDifficulty = Difficulty.Easy;
                else if (e.KeyCode == Keys.D2)
                    oDifficulty = Difficulty.Medium;
                else if (e.KeyCode == Keys.D3)
                    oDifficulty = Difficulty.Hard;
                else if (e.KeyCode == Keys.D4)
                    SafeClick = !SafeClick;
                else if (e.KeyCode == Keys.D5)
                    AntiMines = !AntiMines;
                else if (e.KeyCode == Keys.D6)
                    DisplayRecords();
                else if (e.KeyCode == Keys.D7)
                    oMenuState = MenuState.Instructions;
                else if (e.KeyCode == Keys.D8)
                    Application.Exit();
                this.Refresh();

                if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.D2 || e.KeyCode == Keys.D3)
                    StartGame();
                return;
            }

            // game over keypress logic
            if (oGameState == GameState.Win || oGameState == GameState.GameOver)
            {
                if (e.KeyCode == Keys.D1)
                    ReturnToMenu();
                else
                    StartGame();
            }

            // record sorting
            if (oMenuState == MenuState.Records)
            {
                if (e.KeyCode == Keys.D0)
                {
                    // reset records and sorting variables
                    oMenu.LoadRecords();
                    SortTimeAscending = false;
                    SortDifficultyAscending = false;
                    SortOutcomeAscending = false;
                    SortAntiMineModeAscending = false;
                }
                if (e.KeyCode == Keys.D1)
                {
                    SortTimeAscending = !SortTimeAscending;
                    if (SortTimeAscending)
                        oMenu.SortRecords(SortOption.Time, SortType.Ascending);
                    else
                        oMenu.SortRecords(SortOption.Time, SortType.Descending);
                }
                if (e.KeyCode == Keys.D2)
                {
                    SortDifficultyAscending = !SortDifficultyAscending;
                    if (SortDifficultyAscending)
                        oMenu.SortRecords(SortOption.Difficulty, SortType.Ascending);
                    else
                        oMenu.SortRecords(SortOption.Difficulty, SortType.Descending);
                }
                if (e.KeyCode == Keys.D3)
                {
                    SortOutcomeAscending = !SortOutcomeAscending;
                    if (SortOutcomeAscending)
                        oMenu.SortRecords(SortOption.Outcome, SortType.Ascending);
                    else
                        oMenu.SortRecords(SortOption.Outcome, SortType.Descending);
                }
                if (e.KeyCode == Keys.D4)
                {
                    SortAntiMineModeAscending = !SortAntiMineModeAscending;
                    if (SortAntiMineModeAscending)
                        oMenu.SortRecords(SortOption.AntiMineMode, SortType.Ascending);
                    else
                        oMenu.SortRecords(SortOption.AntiMineMode, SortType.Descending);
                }
                txtRecords.Text = oMenu.Text;
                this.Refresh();
            }

            // return to menu
            if (oGameState == GameState.InProgress || oMenuState == MenuState.Instructions || oMenuState == MenuState.Records)
                if (e.KeyCode == Keys.Escape)
                    ReturnToMenu();

            // toggle between flag types
            if (oGameState == GameState.InProgress)
            {
                if (e.KeyCode == Keys.D1)
                    AntiFlags = !AntiFlags;               
                this.Refresh();
            }
        }

        private void tmrGame_Tick(object sender, EventArgs e)
        {
            // game timer and label updates
            ElapsedTime++;
            lblTime.Text = ElapsedTime.ToString();
        }

        public void StartGame()
        {
            // scaling fixes
            Rectangle ScreenArea = Screen.FromControl(this).WorkingArea;

            // specify grid size for difficulty
            if (oDifficulty == Difficulty.Easy)
            {
                Rows = 10;
                Cols = 10;
            }
            else if (oDifficulty == Difficulty.Medium)
            {
                Rows = 20;
                Cols = 20;
            }
            else if (oDifficulty == Difficulty.Hard)
            {
                Rows = 30;
                Cols = 30;
            }

            // scaling fixes
            CellSize = (ScreenArea.Height - 250) / Rows;

            if (oDifficulty == Difficulty.Easy)
                CellSize = Math.Min(CellSize, 60);
            else if (oDifficulty == Difficulty.Medium)
                CellSize = Math.Min(CellSize, 45);
            else if (oDifficulty == Difficulty.Hard)
                CellSize = Math.Min(CellSize, 30);

            if (CellSize < 16)
                CellSize = 16;

            // label updates
            lblInfo.Visible = true;
            lblFlagsRemaining.Visible = true;
            lblTime.Visible = true;
            ElapsedTime = 0;
            lblTime.Text = ElapsedTime.ToString();

            // flag count
            FlagCount = Rows * Cols / 10;
            if (AntiMines)
            {
                lblAntiFlagsRemaining.Visible = true;
                FlagCount /= 2;
                AntiFlagCount = FlagCount;
            }
            
            // initialize the game
            oMineFields = new MineFields(Rows, Cols, CellSize, AntiMines);
            oGameState = GameState.InProgress;
            oMenuState = MenuState.None;                       
            FirstClick = true;

            // update dimensions
            if (this.WindowState != FormWindowState.Maximized)
            {
                int GridWidth = Cols * CellSize;
                int GridHeight = Rows * CellSize;
                this.ClientSize = new Size(GridWidth + 200, GridHeight + 200);
                this.CenterToScreen();
            }

            this.Refresh();
        }

        public void ReturnToMenu()
        {
            // reset game variables
            oMineFields = null;
            oGameState = GameState.None;
            oMenuState = MenuState.Start;

            // update labels and stop game timer
            lblInfo.Text = $"Safe First Click: {SafeClick.ToString()}, Anti Mines: {AntiMines.ToString()}";
            lblFlagsRemaining.Visible = false;
            lblAntiFlagsRemaining.Visible = false;
            lblTime.Visible = false;
            txtRecords.Visible = false;
            tmrGame.Stop();

            // fix scaling
            if (this.WindowState != FormWindowState.Maximized)
            {
                Rectangle ScreenArea = Screen.FromControl(this).WorkingArea;
                int TargetHeight = Math.Min(900, (int)(ScreenArea.Height * 0.9));
                int TargetWidth = (int)(TargetHeight * (1000 / 900));
                this.ClientSize = new Size(TargetWidth, TargetHeight);
                this.CenterToScreen();
            }
                       
            this.Refresh();
        }

        public void CheckWin()
        {
            // variables
            int RevealedSquares = oMineFields.RevealedSquares;
            int SquaresRemaining = Rows * Cols - RevealedSquares;
            int MinesCount = oMineFields.Mines;
            int AntiMinesCount = oMineFields.AntiMines;

            // check if all hidden squares are mines
            if (SquaresRemaining == MinesCount + AntiMinesCount)
                oGameState = GameState.Win;

            // update labels based on win or loss and stop game timer
            if (oGameState != GameState.InProgress)
            {
                if (oGameState == GameState.Win)
                {
                    lblInfo.Text = "You Win\n";
                    Outcome = "Win";
                }
                else if (oGameState == GameState.GameOver)
                {
                    lblInfo.Text = "You Lose\n";
                    oMineFields.Reveal(true); // reveal hidden mines if loss
                    Outcome = "Loss";
                }
                tmrGame.Stop();

                // write records
                Time = ElapsedTime.ToString();
                switch ((int)oDifficulty)
                {
                    case 0: DifficultyLevel = "Easy"; break;
                    case 1: DifficultyLevel = "Medium"; break;
                    case 2: DifficultyLevel = "Hard"; break;
                }
                oMenu.WriteRecords(Time, DifficultyLevel, Outcome, AntiMines.ToString());

                lblInfo.Text += "Press Any Key To Play Again\nPress 1 To Return To Main Menu";
                lblInfo.Visible = true;
            }
        }

        public void DisplayRecords()
        {
            // update the menu and records label
            oMenuState = MenuState.Records;
            oMenu.LoadRecords();
            txtRecords.Text = oMenu.Text;
            txtRecords.Visible = true;           
            this.Refresh();
        }

        // check if resized
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
