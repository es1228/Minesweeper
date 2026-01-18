using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* for some reason winforms creates a copy of the image every time it is drawn
 * this causes lots of memory addresses to be allocated with the same image
 * this static class stores the images by themselves to only one memory address each
 * now drawing new images will not take up any more memory, drastically increasing performance
 */

namespace Minesweeper
{
    internal static class GameAssets
    {
        public static readonly Image AntiFlag = Assets.AntiFlag;
        public static readonly Image AntiFlagsRemaining = Assets.AntiFlagsRemaining;
        public static readonly Image AntiFlagWrong = Assets.AntiFlagWrong;
        public static readonly Image AntiMine = Assets.AntiMine;
        public static readonly Image AntiMineExplode = Assets.AntiMineExplode;
        public static readonly Image AntiQuestion = Assets.AntiQuestion;
        public static readonly Image Clock = Assets.Clock;
        public static readonly Image MineExplode = Assets.MineExplode;
        public static readonly Image Field0 = Assets.Field0;
        public static readonly Image Field1 = Assets.Field1;
        public static readonly Image Field2 = Assets.Field2;
        public static readonly Image Field3 = Assets.Field3;
        public static readonly Image Field4 = Assets.Field4;
        public static readonly Image Field5 = Assets.Field5;
        public static readonly Image Field6 = Assets.Field6;
        public static readonly Image Field7 = Assets.Field7;
        public static readonly Image Field8 = Assets.Field8;
        public static readonly Image FieldN1 = Assets.FieldN1;
        public static readonly Image FieldN2 = Assets.FieldN2;
        public static readonly Image FieldN3 = Assets.FieldN3;
        public static readonly Image FieldN4 = Assets.FieldN4;
        public static readonly Image FieldN5 = Assets.FieldN5;
        public static readonly Image FieldN6 = Assets.FieldN6;
        public static readonly Image FieldN7 = Assets.FieldN7;
        public static readonly Image FieldN8 = Assets.FieldN8;
        public static readonly Image Flag = Assets.Flag;
        public static readonly Image FlagsRemaining = Assets.FlagsRemaining;
        public static readonly Image Hidden = Assets.Hidden;
        public static readonly Image MenuStart = Assets.MenuStart;
        public static readonly Image MenuInstructions = Assets.MenuInstructions;
        public static readonly Image MenuRecords = Assets.MenuRecords;
        public static readonly Image Mine = Assets.Mine;
        public static readonly Icon Minesweeper = Assets.Minesweeper;
        public static readonly Image Question = Assets.Question;
        public static readonly Image Revealed = Assets.Revealed;
        public static readonly Image FlagWrong = Assets.FlagWrong;
    }
}
