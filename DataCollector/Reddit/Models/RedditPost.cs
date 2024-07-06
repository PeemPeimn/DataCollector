using System.ComponentModel.DataAnnotations.Schema;

[Table("reddit_posts")]
public class RedditPost
{
    [Column("id")]
    public required string Id { get; set; }

    [Column("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    [Column("post", TypeName = "jsonb")]
    public required string Post { get; set; }
}