using DominoGame.Enums;
using DominoGame.Models;
using System.Drawing;
using System.Text;

namespace DominoGame.Handlers;

public class LoggerWindow
{
    private readonly Point _startWindow = new(80, 4);
    private readonly Label _emptyLog;

    public LoggerWindow()
    {
        _emptyLog = new(_startWindow.Y, _startWindow.X, "                  ");
        DrawBorder();
    }

    private void DrawBorder()
    {
        StringBuilder sb = new();
        sb.Append('_', Settings.Width - _startWindow.X - 1);
        _ = new Label(_startWindow.Y - 1, _startWindow.X, sb.ToString());
        _ = new Label(Settings.Height - 1, _startWindow.X, sb.ToString());
    }

    public void LogActions(IList<IList<DominoPiece>> dominoes, DominoPiece boardDomino)
    {
        for (int i = 0; i < dominoes.Count; i++)
        {
            foreach (DominoPiece domino in dominoes[i])
            {
                LogAction(domino, boardDomino);
            }
        }
    }

    public void LogAction(DominoPiece handDomino, DominoPiece? boardDomino)
    {
        Point lastPoint = _emptyLog.Coordinate;
        int count = _emptyLog.Coordinate.X - 3;

        string from = handDomino.GetString(DominoSides.Horizontal);
        string to = boardDomino != null ? boardDomino.GetString(DominoSides.Horizontal) : "   ";
        string action = $"{from} => {to};Press {count}";

        _emptyLog.Coordinate = new(lastPoint.X + 1, lastPoint.Y);
        _ = new Label(lastPoint.X, lastPoint.Y, action);
    }

    public void ClearLog()
    {
        Point lastPoint = _emptyLog.Coordinate;
        while (lastPoint.X != _startWindow.Y)
        {
            lastPoint.X--;
            _emptyLog.Coordinate = lastPoint;
        }
    }
}