namespace DominoGame.Enums;

public enum DominoSides
{
    None,
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8,
    Horizontal = Right | Left,
    Vertical = Up | Down
}