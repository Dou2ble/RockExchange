namespace RockExchange;

public class Rock
{
    public RockKind Kind;
    
    public Rock()
    {
        RockKind[] values = new[] { RockKind.Blue, RockKind.Cyan, RockKind.Green, RockKind.Lime, RockKind.Navy };
        Kind = (RockKind)values.GetValue(Random.Shared.Next(values.Length));
    }
}