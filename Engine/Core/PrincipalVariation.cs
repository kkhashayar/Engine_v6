using Engine;

public class PrincipalVariation
{
    public List<MoveObject> Moves { get; private set; }
    public int Count { get; set; }

    public PrincipalVariation()
    {
        Moves = new List<MoveObject>();
        Count = 0;
    }

    public void AddMove(MoveObject move)
    {
        if (Count < Moves.Count)
        {
            Moves[Count] = move;
        }
        else
        {
            Moves.Add(move);
        }
        Count++;
    }

    public void CopyFrom(PrincipalVariation other)
    {
        Count = other.Count;
        for (int i = 0; i < other.Count; i++)
        {
            if (i < Moves.Count)
            {
                Moves[i] = other.Moves[i];
            }
            else
            {
                Moves.Add(other.Moves[i]);
            }
        }
    }
}
