using DominoGame.Handlers;
using DominoGame.Models;

namespace DominoGame;

public class GameCore
{
    private readonly int _gameOverScore;
    private readonly Bazaar _bazaar;
    private readonly LoggerWindow _loggerWindow;
    private readonly DominoEngine _engine;
    private readonly Player[] _players;
    private int _currentPlayer;

    public bool HasGameOverScore { get; private set; }
    public bool IsMatchEnd { get; private set; }

    public GameCore(int playerCount, int gameOverScore = 100)
    {
        _ = new Label(1, 0, "Player's turn: ");
        _gameOverScore = gameOverScore;
        _bazaar = new();
        _loggerWindow = new();
        _engine = new();
        _players = new Player[playerCount];

        for (int i = 0; i < playerCount; i++)
        {
            _players[i] = new($"Player {i + 1}", i);
        }
    }

    public void NewMatch()
    {
        IsMatchEnd = false;
        _bazaar.NewBazaar();
        _engine.NewMatch();
        for (int i = 0; i < _players.Length * Settings.StartHandSize; i++)
        {
            DominoPiece domino = _bazaar.GetDomino();
            _players[i % _players.Length].AddDomino(domino);
        }
        DefineFirstTurn();
        PrintPlayer();
        MakeFirstTurn();
    }

    public void MakeTurn(int countEmptyTurn = 0)
    {
        PrintPlayer();

        IList<IList<DominoPiece>> leftPart = _players[_currentPlayer].GetPlaceableDominoes(_engine.LeftDomino);
        IList<IList<DominoPiece>> rightPart = _players[_currentPlayer].GetPlaceableDominoes(_engine.RightDomino);
        int count = leftPart[0].Count + leftPart[1].Count + rightPart[0].Count + rightPart[1].Count;
        _loggerWindow.LogActions(leftPart, _engine.LeftDomino);
        _loggerWindow.LogActions(rightPart, _engine.RightDomino);

        if (countEmptyTurn == 4)
        {
            MatchEnd();
            return;
        }
        if (count == 0)
        {
            if (_bazaar.CanGetDomino)
            {
                _players[_currentPlayer].AddDomino(_bazaar.GetDomino());
            }
            else
            {
                NextPlayer();
                countEmptyTurn++;
            }
            MakeTurn(countEmptyTurn);
            return;
        }

        int index = GetInputIndex(count);
        _loggerWindow.ClearLog();

        if (index < leftPart[0].Count + leftPart[1].Count)
        {
            PlaceDominoFromIndex(index, leftPart, _engine.PlaceDominoToLeft);
        }
        else
        {
            PlaceDominoFromIndex(index - leftPart[0].Count - leftPart[1].Count, rightPart, _engine.PlaceDominoToRight);
        }

        if (_players[_currentPlayer].IsEmptyHand)
        {
            MatchEnd();
        }
        else
        {
            NextPlayer();
        }
    }

    private void PlaceDominoFromIndex(int index, IList<IList<DominoPiece>> dominoes, Action<DominoPiece> action)
    {
        DominoPiece domino;
        if (index < dominoes[0].Count)
        {
            domino = dominoes[0][index];
            domino.SetBusyLeft();
        }
        else
        {
            domino = dominoes[1][index - dominoes[0].Count];
            domino.SetBusyRight();
        }
        action.Invoke(domino);
        _players[_currentPlayer].RemoveDomino(domino);
    }

    private void MakeFirstTurn()
    {
        Player player = _players[_currentPlayer];

        for (int i = 0; i < player.CountDomino; i++)
        {
            _loggerWindow.LogAction(player[i], null);
        }

        int index = GetInputIndex(Settings.StartHandSize);
        _loggerWindow.ClearLog();
        DominoPiece leftDomino = player[index];
        DominoPiece rightDomino = leftDomino.Copy();
        leftDomino.SetBusyRight();
        rightDomino.SetBusyLeft();
        player.RemoveDomino(index);
        _engine.PlaceDominoToLeft(leftDomino);
        _engine.PlaceDominoToRight(rightDomino);

        NextPlayer();
    }

    private int GetInputIndex(int maxIndex)
    {
        string rawIndex;
        int index;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Black;

        do
        {
            Console.SetCursorPosition(0, 0);
            rawIndex = Console.ReadLine() ?? "";
        }
        while (string.IsNullOrEmpty(rawIndex) || !int.TryParse(rawIndex, out index) || index < 1 || index > maxIndex);

        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.White;

        return index - 1;
    }

    private void DefineFirstTurn()
    {
        DominoPiece best = BestPlayerDomino(_players[0]);
        int playerIndex = 0;

        for (int i = 1; i < _players.Length; i++)
        {
            DominoPiece domino = BestPlayerDomino(_players[i]);

            if (domino < best)
            {
                best = domino;
                playerIndex = i;
            }
        }
        _currentPlayer = playerIndex;
    }

    private DominoPiece BestPlayerDomino(Player player)
    {
        DominoPiece bestDomino = player[0];
        DominoPiece alterDomino = bestDomino;

        for (int i = 1; i < 7; i++)
        {
            if (player[i].IsDuplicate)
            {
                if (player[i] < bestDomino)
                {
                    bestDomino = player[i];
                }
            }
            else
            {
                if (player[i] < alterDomino)
                {
                    alterDomino = player[i];
                }
            }
        }

        return bestDomino.IsDuplicate ? bestDomino : alterDomino;
    }

    private void NextPlayer()
    {
        _currentPlayer = (_currentPlayer + 1) % _players.Length;
    }

    private void PrintPlayer()
    {
        _ = new Label(1, 14, _players[_currentPlayer].Name);
    }

    private void MatchEnd()
    {
        int maxScore = int.MinValue;
        int playerIndex = 0;
        IsMatchEnd = true;
        for (int i = 0; i < _players.Length; i++)
        {
            _players[i].NextMatch();
            if (maxScore < _players[i].TotalScore)
            {
                maxScore = _players[i].TotalScore;
                playerIndex = i;
            }
        }
        if (maxScore > _gameOverScore)
        {
            HasGameOverScore = true;
            _currentPlayer = playerIndex;
            _ = new Label(0, 0, $"Loser: {_players[_currentPlayer].Name}");
        }
    }
}