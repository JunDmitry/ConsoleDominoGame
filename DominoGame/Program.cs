using DominoGame;
using DominoGame.Models;

Board board = Board.GetInstance();
GameCore game = new(4, 5);

while (!game.HasGameOverScore)
{
    board.ClearBoard();
    game.NewMatch();
    while (!game.IsMatchEnd)
    {
        game.MakeTurn();
    }
}

Console.ReadKey();