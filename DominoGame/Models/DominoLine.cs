using DominoGame.Enums;
using System.Drawing;

namespace DominoGame.Models
{
    public struct Line
    {
        public Point Coordinate { get; private set; }

        public DominoSides Side { get; private set; }

        public Line(int vertical, int horizontal, DominoSides side)
        {
            Coordinate = new Point(vertical, horizontal);
            Side = side;
        }
    }

    public static class DominoLine
    {
        public static Line[] GetFullLine()
        {
            Line[] fullLine = new Line[Settings.BazaarSize * 2 - 1];
            int counter = 0;
            int vertical = 9;
            int horizontal = 44;

            while (counter < 14)
            {
                horizontal -= 3;
                fullLine[counter++] = new Line(vertical, horizontal, DominoSides.Right);
            }
            fullLine[counter++] = new Line(vertical, --horizontal, DominoSides.Up);
            horizontal -= 3;
            vertical += 3;

            for (int i = 0; i < 12; i++)
            {
                horizontal += 3;
                fullLine[counter++] = new Line(vertical, horizontal, DominoSides.Left);
            }
            horizontal += 3;
            fullLine[counter++] = new Line(vertical, horizontal, DominoSides.Right);

            for (int i = 0; i < 13; i++)
            {
                horizontal += 3;
                fullLine[counter++] = new Line(vertical, horizontal, DominoSides.Right);
            }
            horizontal += 2;
            fullLine[counter++] = new Line(++vertical, horizontal, DominoSides.Down);
            vertical += 2;

            for (int i = 0; i < 13; i++)
            {
                horizontal -= 3;
                fullLine[counter++] = new Line(vertical, horizontal, DominoSides.Left);
            }

            return fullLine;
        }
    }
}