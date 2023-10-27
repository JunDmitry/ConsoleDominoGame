using DominoGame.Handlers;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DominoGame.Models;

public class Board : IUpdate
{
    private static Board? _instance;
    private readonly char[,] _board;

    private Board()
    {
        _instance = this;
        _board = new char[Settings.Height, Settings.Width];
        for (int i = 0; i < Settings.Height; i++)
        {
            for (int j = 0; j < Settings.Width; j++)
            {
                _board[i, j] = (j == 0 || j == Settings.Width - 1 || j == 79) && i > 2 ? (i == 3 ? '_' : '|') : ' ';
            }
        }
        SetFixedWindowSize();
    }

    public void Update<T>(T obj)
    {
        if (obj == null) return;
        if (obj is Label label)
        {
            UpdateBoard(label);
        }
    }

    public static Board GetInstance()
    {
        _instance ??= new Board();
        return _instance;
    }

    public void ClearBoard()
    {
        for (int i = 9; i < 16; i++)
        {
            for (int j = 1; j < 79; j++)
            {
                _board[i, j] = ' ';
            }
        }
    }

    public void DisplayBoard()
    {
        Console.ForegroundColor = ConsoleColor.White;
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                if (i < 3)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else if (char.IsDigit(_board[i, j]))
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else if (i > 3 && i < Settings.Height - 1 && j > 79 && j < Settings.Width - 1)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.BackgroundColor = _board[i, j] == ' ' ? ConsoleColor.Green : ConsoleColor.Blue;
                }
                Console.Write(_board[i, j]);
            }
            if (i != _board.GetLength(0) - 1)
            {
                Console.WriteLine();
            }
        }
        Console.BackgroundColor = default;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.SetCursorPosition(0, 0);
    }

    private void SetFixedWindowSize()
    {
        Console.CursorVisible = false;
        Console.Title = "Domino Game";

        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;
        [DllImport("user32.dll")]
        static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        static extern IntPtr GetConsoleWindow();

        Console.SetWindowSize(Settings.Width, Settings.Height + 1);
        Console.SetBufferSize(Settings.Width, Settings.Height + 1);
        DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
        DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
        DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
    }

    private void UpdateBoard(Label label)
    {
        Point point = label.Coordinate;
        string text = label.Text;
        int i = point.X;
        int j = point.Y;

        foreach (char ch in text)
        {
            if (ch == '\n')
            {
                i++;
                j = point.Y;
            }
            else
            {
                _board[i, j++] = ch;
            }
        }

        if (_board[_board.GetLength(0) - 1, 78] != ' ')
        {
            DisplayBoard();
        }
    }
}