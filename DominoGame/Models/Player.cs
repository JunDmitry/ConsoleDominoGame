using DominoGame.Handlers;

namespace DominoGame.Models;

public class Player
{
    private readonly string _name;
    private readonly List<DominoPiece> _dominoes;
    private readonly UI _ui;
    private int _totalScore;
    private int _handScore;

    public string Name => _name;
    public int CountDomino => _dominoes.Count;
    public bool IsEmptyHand => _dominoes.Count == 0;
    public int TotalScore => _totalScore;
    public int HandScore => _handScore;

    public DominoPiece this[int index]
    {
        get { return _dominoes[index]; }
    }

    public Player(string name, int id)
    {
        _name = name;
        _dominoes = new();
        _ui = new(id, name);
    }

    public void AddDomino(DominoPiece domino)
    {
        _dominoes.Add(domino);
        _ui.AddLabel(domino.GetString());
        UpdateHandScore(domino.GetScore());
    }

    public void RemoveDomino(DominoPiece domino)
    {
        int index = _dominoes.IndexOf(domino);
        RemoveDomino(index);
    }

    public void RemoveDomino(int index)
    {
        UpdateHandScore(-_dominoes[index].GetScore());
        _dominoes.RemoveAt(index);
        _ui.RemoveLabel(index);
    }

    private void UpdateHandScore(int score)
    {
        _handScore += score;
        _ui.UpdateHandScore(_handScore);
    }

    private void UpdateTotalScore(int score = 0)
    {
        _totalScore += _handScore + score;
        _handScore = 0;
        _ui.UpdateGameScore(_totalScore);
    }

    /// <summary>
    /// Get domino placed on the board and define, which player dominoes can be placed to this board domino
    /// </summary>
    /// <param name="boardDomino"></param>
    /// <returns>0 index - left placeable domino; 1 index - right placeable domino;</returns>
    public IList<IList<DominoPiece>> GetPlaceableDominoes(DominoPiece boardDomino)
    {
        IList<IList<DominoPiece>> dominos = new List<IList<DominoPiece>> { new List<DominoPiece>(), new List<DominoPiece>() };

        for (int i = 0; i < _dominoes.Count; i++)
        {
            bool[] placeableSide = _dominoes[i].CanPlaced(boardDomino);
            if (placeableSide.Any(k => k))
            {
                dominos[placeableSide[0] ? 0 : 1].Add(_dominoes[i]);
            }
        }

        return dominos;
    }

    public void NextMatch()
    {
        int handScore = _handScore;
        while (_dominoes.Count > 0)
        {
            RemoveDomino(0);
        }
        UpdateTotalScore(handScore);
    }
}