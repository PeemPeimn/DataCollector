// Base class for Reddit's data
public class Thing
{
    public required string Kind { get; set; }
    public virtual required Thing Data { get; set; }

}