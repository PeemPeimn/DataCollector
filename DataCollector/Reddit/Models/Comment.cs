// Kind: "t1"
public class Comment : Thing
{
    public required string Id { get; set; }

    public required string ParentId { get; set; }

    public required string Body { get; set; }

    public required Thing replies { get; set; }

}