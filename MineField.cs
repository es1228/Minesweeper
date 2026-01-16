using System;
using System.Drawing;

namespace Minesweeper
{
    //*************************************************************
    //Enum for the type of Mines
    //*************************************************************
    public enum FieldState { None, Protected, Flagged, AntiFlagged, Question, AntiQuestion, MineExplode, AntiMineExplode };
    public enum FieldType { None, Mine, AntiMine }
    public enum FieldVisibility { Hidden, Revealed }


    internal class MineField
    {
        //*************************************************************
        //Fields
        //*************************************************************
        private int mAdjacentMines;
        private bool mNextToMineType;
        private FieldType mType;
        private FieldState mState;
        private FieldVisibility mVisibility;
        private Image mBackImage;

        //*************************************************************
        //Constructors
        //*************************************************************
        public MineField()
        {
            // init all fields to defaults
            this.AdjacentMines = 0;
            this.mNextToMineType = false;
            this.mType = FieldType.None;
            this.mState = FieldState.None;
            this.mVisibility = FieldVisibility.Hidden;
            this.mBackImage = GameAssets.Hidden;
        }


        //*************************************************************
        //Properties
        //*************************************************************
        public int AdjacentMines { get { return mAdjacentMines; } set { mAdjacentMines = value; } }
        public bool NextToMineType { get { return mNextToMineType; } set { mNextToMineType = value; } }
        public FieldType Type { get { return mType; } set { mType = value; } }
        public FieldState State { get { return mState; } set { mState = value; } }
        public FieldVisibility Visibility { get { return mVisibility; } set { mVisibility = value; } }


        //*************************************************************
        //Methods
        //*************************************************************

        public Image GetBackground()
        {
            // explosions
            if (this.mState == FieldState.MineExplode)
                return GameAssets.MineExplode;

            if (this.mState == FieldState.AntiMineExplode)
                return GameAssets.AntiMineExplode;

            // unrevealed
            if (this.mVisibility != FieldVisibility.Revealed)
            {
                if (this.mState == FieldState.Flagged)
                    return GameAssets.Flag;

                if (this.mState == FieldState.Question)
                    return GameAssets.Question;

                if (this.mState == FieldState.AntiFlagged)
                    return GameAssets.AntiFlag;

                if (this.mState == FieldState.AntiQuestion)
                    return GameAssets.AntiQuestion;

                return GameAssets.Hidden;
            }      
            
            // revealed
            if (this.mState == FieldState.Flagged && this.mType == FieldType.None)
                return GameAssets.FlagWrong;

            if (this.mState == FieldState.AntiFlagged && this.mType == FieldType.None)
                return GameAssets.AntiFlagWrong;

            if (this.mState == FieldState.Flagged)
                return GameAssets.Flag;

            if (this.mState == FieldState.AntiFlagged)
                return GameAssets.AntiFlag;

            if (this.mType == FieldType.Mine)
                return GameAssets.Mine;

            if (this.mType == FieldType.AntiMine)
                return GameAssets.AntiMine;

            // has mines
            if (this.mNextToMineType)
            {
                switch (this.mAdjacentMines)
                {
                    case -1: return GameAssets.FieldN1;
                    case -2: return GameAssets.FieldN2;
                    case -3: return GameAssets.FieldN3;
                    case -4: return GameAssets.FieldN4;
                    case -5: return GameAssets.FieldN5;
                    case -6: return GameAssets.FieldN6;
                    case -7: return GameAssets.FieldN7;
                    case -8: return GameAssets.FieldN8;
                    case 0: return GameAssets.Field0;
                    case 1: return GameAssets.Field1;
                    case 2: return GameAssets.Field2;
                    case 3: return GameAssets.Field3;
                    case 4: return GameAssets.Field4;
                    case 5: return GameAssets.Field5;
                    case 6: return GameAssets.Field6;
                    case 7: return GameAssets.Field7;
                    case 8: return GameAssets.Field8;
                }
            }

            return GameAssets.Revealed;
        }
        public void Draw(Graphics g, int x, int y, int Size)
        {
            //this draws the mine on the surface g at
            //the location x and y
            Image Background = GetBackground();
            int Offset = 4;
            g.DrawImage(Background, x + Offset / 2, y + Offset / 2, Size - Offset, Size - Offset); 
        }

    }
}
