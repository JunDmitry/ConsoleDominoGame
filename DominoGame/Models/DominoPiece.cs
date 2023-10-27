using DominoGame.Enums;

namespace DominoGame.Models;

public class DominoPiece
{
    private int _left;
    private int _right;
    private int _busySide;

    public bool IsDuplicate => _left == _right;
    public bool IsPlaced => _busySide != 0;

    public DominoPiece(int left, int right)
    {
        _left = left;
        _right = right;
    }

    public int GetScore()
    {
        return _left + _right;
    }

    public string GetString(DominoSides freeSide = DominoSides.Up)
    {
        string separator = (freeSide | DominoSides.Horizontal) == DominoSides.Horizontal ? "|" : "\n-\n";
        if (_busySide == -1)
        {
            (_left, _right) = (_right, _left);
            _busySide *= -1;
        }
        if (freeSide == DominoSides.Down || freeSide == DominoSides.Right)
        {
            (_left, _right) = (_right, _left);
            _busySide *= -1;
        }

        return $"{_left}{separator}{_right}";
    }

    public void SetBusyLeft()
    {
        if (IsPlaced)
        {
            throw new InvalidOperationException("This domino have been placed on the board!");
        }
        _busySide = -1;
    }

    public void SetBusyRight()
    {
        if (IsPlaced)
        {
            throw new InvalidOperationException("This domino have been placed on the board!");
        }
        _busySide = 1;
    }

    public int GetFreeSide()
    {
        return _busySide == 1 ? _left : _right;
    }

    public bool[] CanPlaced(DominoPiece domino)
    {
        if (!domino.IsPlaced)
        {
            throw new InvalidOperationException("This domino don't placed on the board!");
        }
        int freeSide = domino.GetFreeSide();

        return new[] { freeSide == _left, freeSide == _right };
    }

    private int GetLargestSide()
    {
        return Math.Max(_left, _right);
    }

    public static bool operator <(DominoPiece domino1, DominoPiece domino2)
    {
        if (domino1.IsDuplicate && !domino2.IsDuplicate)
        {
            return true;
        }
        if (!domino1.IsDuplicate && domino2.IsDuplicate)
        {
            return false;
        }
        if (domino1.GetLargestSide() > domino2.GetLargestSide())
        {
            return false;
        }
        if (domino1.GetLargestSide() < domino2.GetLargestSide())
        {
            return true;
        }

        return domino1.GetScore() < domino2.GetScore();
    }

    public static bool operator >(DominoPiece domino1, DominoPiece domino2)
    {
        return !(domino1 < domino2);
    }

    public DominoPiece Copy()
    {
        return new(_left, _right);
    }
}