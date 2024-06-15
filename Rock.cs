namespace RockExchange;

public class Rock
{
    public RockKind Kind;
    
    public Rock()
    {
        RockKind[] values = [RockKind.Green, RockKind.Purple, RockKind.Blue, RockKind.Red, RockKind.Cyan];
        Kind = (RockKind)(values.GetValue(Random.Shared.Next(values.Length)) ?? RockKind.Green);
    }
}