namespace ApiContracts;

public class CommentDto
{
    public required int UserId { get; set; }
    public required int PostId { get; set; }
    public required string Body { get; set; }
}