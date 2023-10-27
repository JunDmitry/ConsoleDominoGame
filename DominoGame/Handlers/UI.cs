using DominoGame.Models;
using System.Drawing;
using System.Text;

namespace DominoGame.Handlers;

public class UI
{
    private static readonly Point[] _points = { new(3, 0), new(3, 40), new(16, 0), new(16, 40) };

    private readonly Point _point;
    private readonly IList<Label> _dominoLabels;
    private readonly IList<Label> _playerInfo;

    public UI(int id, string nickname)
    {
        _point = _points[id];
        _dominoLabels = new List<Label>()
        {
            new(_point.X + 1, _point.Y + 1, " \n \n "),
            new(_point.X + 1, _point.Y + 2, " \n \n ")
        };
        _playerInfo = new List<Label>
        {
            new(_point.X + 4, _point.Y + 1),
            new(_point.X + 4, _point.Y + 11),
        };
        UpdateGameScore(0);
        UpdateHandScore(0);
        SetNickname(nickname);
        DrawUIBorder();
    }

    public void AddLabel(string text)
    {
        Point lastPoint = _dominoLabels[^1].Coordinate;
        _dominoLabels[^1].Coordinate = new(lastPoint.X, lastPoint.Y + 2);
        _dominoLabels.Insert(_dominoLabels.Count - 1, new(lastPoint.X, lastPoint.Y, text));
    }

    public void RemoveLabel(int index)
    {
        _dominoLabels.RemoveAt(++index);
        for (int i = index; i < _dominoLabels.Count; i++)
        {
            Label label = _dominoLabels[i];
            Point point = label.Coordinate;

            label.Coordinate = new(point.X, point.Y - 2);
        }
    }

    public void UpdateGameScore(int score)
    {
        _playerInfo[0].Text = $"Score:{score:000}";
    }

    public void UpdateHandScore(int score)
    {
        _playerInfo[1].Text = $"Hand score:{score:000}";
    }

    private void SetNickname(string nickname)
    {
        _ = new Label(_point.X + 4, _point.Y + 26, nickname);
    }

    private void DrawUIBorder()
    {
        StringBuilder horizontalBorder = new();
        horizontalBorder.Append('_', Settings.UIWidth - 2);
        StringBuilder verticalBorder = new();

        for (int i = 1; i < Settings.UIHeight; i++)
        {
            verticalBorder.Append("|\n");
        }

        char leftSymbol = _point.Y == 0 ? (_point.X == 3 ? '_' : '|') : '_';
        char rightSymbol = _point.Y == 0 ? '_' : (_point.X == 3 ? '_' : '|');
        _ = new Label(_point.X, _point.Y, $"{leftSymbol}{horizontalBorder}{rightSymbol}");
        _ = new Label(_point.X + Settings.UIHeight - 1, _point.Y + 1, horizontalBorder.ToString());
        _ = new Label(_point.X + 1, _point.Y, verticalBorder.ToString());
        _ = new Label(_point.X + 1, _point.Y + Settings.UIWidth - 1, verticalBorder.ToString());
    }
}