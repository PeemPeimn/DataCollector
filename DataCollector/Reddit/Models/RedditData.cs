using System.ComponentModel.DataAnnotations.Schema;

[Table("reddit_data")]
public class RedditData
{
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("post_id")]
    public required string PostId { get; set; }

    [Column("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    [Column("messages", TypeName = "jsonb")]
    public required string Messages { get; set; }
}