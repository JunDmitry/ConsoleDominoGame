namespace DominoGame.Models;

public class Bazaar
{
    private readonly Queue<DominoPiece> _bazar;
    private readonly Label _label;

    public bool CanGetDomino => _bazar.Count != 0;

    public Bazaar()
    {
        _bazar = new();
        _label = new(2, 0);
        UpdateLabelText();
    }

    public void NewBazaar()
    {
        _bazar.Clear();
        UpdateLabelText();
        IList<DominoPiece> dominoes = GenerateDomino();
        Random random = new(DateTime.Now.Millisecond);

        while (dominoes.Count > 0)
        {
            int index = random.Next(dominoes.Count);
            _bazar.Enqueue(dominoes[index]);
            dominoes.RemoveAt(index);
            UpdateLabelText();
        }
    }

    private IList<DominoPiece> GenerateDomino()
    {
        IList<DominoPiece> dominoes = new List<DominoPiece>();
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                dominoes.Add(new(i, j));
            }
        }

        return dominoes;
    }

    public DominoPiece GetDomino()
    {
        DominoPiece domino = _bazar.Dequeue();
        UpdateLabelText();

        return domino;
    }

    private void UpdateLabelText()
    {
        _label.Text = $"Count domino in Bazaar: {_bazar.Count:00}";
    }
}