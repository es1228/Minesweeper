using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Minesweeper
{
    internal class MineFields
    {
        //*************************************************************
        //Fields
        //*************************************************************
        private int mRows;
        private int mCols;
        private int mSize;
        private int mMines;
        private int mAntiMines;
        private int mRevealedSquares;
        private MineField[,] mMineFields;



        //*************************************************************
        //Constructors
        //*************************************************************
        public MineFields(int Rows, int Cols, int Size, bool AntiMines)
        {
            this.mRows = Rows;
            this.mCols = Cols;
            this.mSize = Size;
            this.mMines = Rows * Cols / 10; // 1 mine for every 10 squares
            this.mAntiMines = 0;
            if (AntiMines)
            {
                this.mMines /= 2;
                this.mAntiMines = this.mMines;
            }
            mMineFields = new MineField[Rows, Cols];

            // init mine Fields
            for (int i = 0; i < mRows; i++)
            {
                for (int j = 0; j < Cols; j++)
                    mMineFields[i, j] = new MineField();                 
            }
        }


        //*************************************************************
        //Properties
        //*************************************************************

        public int Mines { get { return mMines; } }
        public int AntiMines { get { return mAntiMines; } }
        public int RevealedSquares {  get { return mRevealedSquares; } }

        //*************************************************************
        //Methods
        //*************************************************************

        // ensure the 9 squares (initial and surrounding) are safe
        public void SetProtectedSquares(int Row, int Col)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int r = Row + i;
                    int c = Col + j;                    

                    if (r >= 0 && r < mRows && c >= 0 && c < mCols)
                    {
                        MineField Field = mMineFields[r, c];
                        Field.State = FieldState.Protected;
                        Field.Type = FieldType.None;
                    }
                }
            }
        }

        // handle initial board shuffling logic
        public void Shuffle()
        {
            // create a random object
            Random oRandom = new Random();
            MineField Field;

            // initially place the mines
            int MinesPlaced = 0;
            int AntiMinesPlaced = 0;
            for (int i = 0; i < mRows; i++)
            {
                for (int j = 0; j < mCols; j++)
                {
                    Field = mMineFields[i, j];
                    if (MinesPlaced < mMines && Field.State != FieldState.Protected)
                    {
                        Field.Type = FieldType.Mine;
                        MinesPlaced++;
                    }
                    else if (AntiMinesPlaced < mAntiMines && Field.State != FieldState.Protected)
                    {
                        Field.Type = FieldType.AntiMine;
                        AntiMinesPlaced++;
                    }
                }
            }

            // shuffling logic
            for (int i = 0; i < mRows; i++)
            {
                for (int j = 0; j < mCols; j++)
                {
                    Field = mMineFields[i, j];
                    // ensure the original Field is not marked as protected
                    if (Field.State == FieldState.Protected)
                        continue;

                    // variable declarations
                    int RandRow = oRandom.Next(mRows);
                    int RandCol = oRandom.Next(mCols);
                    MineField RandomField = mMineFields[RandRow, RandCol];

                    // make sure its not protected
                    if (RandomField.State != FieldState.Protected && Field.State != FieldState.Protected)
                    {
                        // swap the original and random Fields
                        FieldType Temp = Field.Type;
                        Field.Type = RandomField.Type;
                        RandomField.Type = Temp;
                    }
                }
            }
        }

        // assign each square the number of adjacent mines
        public void AssignAdjacents()
        {
            // iterate through the grid
            for (int i = 0; i < mRows; i++)
            {
                for (int j = 0; j < mCols; j++)
                {
                    // keep track of the number of adjacent squares
                    MineField Field = mMineFields[i, j];

                    // create a for loop to travel across the 9 squares
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            int r = i + k;
                            int c = j + l;
                            if (r >= 0 && r < mRows && c >= 0 && c < mCols)
                            {   
                                MineField AdjacentField = mMineFields[r, c];
                                if (AdjacentField != Field)
                                {
                                    if (AdjacentField.Type == FieldType.Mine)
                                    {
                                        Field.NextToMineType = true;
                                        Field.AdjacentMines++;
                                    }
                                    if (AdjacentField.Type == FieldType.AntiMine)
                                    {
                                        Field.NextToMineType = true;
                                        Field.AdjacentMines--;
                                    }
                                }                               
                            }                           
                        }
                    }
                }
            }
        }

        // reveal squares on click
        public void Reveal(int Row, int Col)
        {
            // initial checks
            if (Row < 0 || Col < 0 || Row >= mRows || Col >= mCols 
                || mMineFields[Row, Col].Visibility == FieldVisibility.Revealed 
                || mMineFields[Row, Col].State == FieldState.Flagged
                || mMineFields[Row, Col].State == FieldState.AntiFlagged
                || mMineFields[Row, Col].State == FieldState.Question
                || mMineFields[Row, Col].State == FieldState.AntiQuestion)
                return;

            // set visibility as revealed
            MineField Field = mMineFields[Row, Col];
            Field.Visibility = FieldVisibility.Revealed;
            this.mRevealedSquares++;

            // recursive reveal if a 0 adjacents is clicked
            if (Field.AdjacentMines == 0 && !Field.NextToMineType)
            {
                // create a for loop to travel across the 9 squares
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                        Reveal(Row + i, Col + j);
                }
            }           
        }
        public void Reveal(bool GameOver)
        {
            // check if reveal mines was selected
            if (GameOver)
            {
                // mark all squares in the grid as revealed
                for (int i = 0; i < mRows; i++)
                {
                    for (int j = 0; j < mCols; j++)
                    {
                        MineField Field = mMineFields[i, j];
                        if (Field.Type == FieldType.Mine || Field.Type == FieldType.AntiMine
                            || Field.State == FieldState.Flagged || Field.State == FieldState.AntiFlagged)
                            mMineFields[i, j].Visibility = FieldVisibility.Revealed;
                    }
                }
                // end method
                return;
            }
        }

        // draw the mine Fields on the form
        public void Draw(Graphics g, int x, int y)
        {
            //this draws the grid of mines on the surface g at
            //the location x and y
            //

            // iterate through the grid
            for (int i = 0; i < mRows; i++)
            {
                for (int j = 0; j < mCols; j++)
                    mMineFields[i, j].Draw(g, x + j * mSize, y + i * mSize, mSize);
            }
        }

        // return a specific mine Field
        public MineField GetField(int Row, int Col)
        {
            return mMineFields[Row, Col];
        }

    }
}
