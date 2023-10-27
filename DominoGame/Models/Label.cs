using DominoGame.Handlers;
using System.Drawing;

namespace DominoGame.Models;

public class Label : Observable
{
    private string _text;
    private Point _coordinate;

    public string Text
    {
        get { return _text; }
        set
        {
            _text = value;
            Notify();
        }
    }

    public Point Coordinate
    {
        get { return _coordinate; }
        set
        {
            _coordinate = value;
            Notify();
        }
    }

    public Label(int vertical, int horizontal, string text = "") : base()
    {
        Coordinate = new Point(vertical, horizontal);
        _text = text;
        Attach(Board.GetInstance());
        Notify();
    }
}