using DominoGame.Enums;
using DominoGame.Models;
using System.Drawing;

namespace DominoGame;

public class DominoEngine
{
    private DominoPiece _leftDomino;
    private DominoPiece _rightDomino;

    private readonly Line[] _lines;
    private int _leftIndex;
    private int _rightIndex;

    public DominoPiece LeftDomino => _leftDomino;
    public DominoPiece RightDomino => _rightDomino;

    public DominoEngine()
    {
        _leftDomino = new(-1, -1);
        _rightDomino = new(-1, -1);
        _lines = DominoLine.GetFullLine();
    }

    public void NewMatch()
    {
        _leftIndex = _lines.Length / 2;
        _rightIndex = _leftIndex;
    }

    public void PlaceDominoToLeft(DominoPiece domino)
    {
        Point point = _lines[_leftIndex].Coordinate;
        DominoSides side = _leftIndex == _rightIndex ? DominoSides.Left : _lines[_leftIndex].Side;

        _ = new Label(point.X, point.Y, domino.GetString(side));
        _leftDomino = domino;
        _leftIndex--;
    }

    public void PlaceDominoToRight(DominoPiece domino)
    {
        Point point = _lines[_rightIndex].Coordinate;
        DominoSides side = _lines[_rightIndex].Side;

        _ = new Label(point.X, point.Y, domino.GetString(side));
        _rightDomino = domino;
        _rightIndex++;
    }
}