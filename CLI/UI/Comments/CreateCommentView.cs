using RepositoryContracts;

namespace CLI.UI.Comments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    
    public CreateCommentView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }
    
    public async Task StartAsync()
    {
        
    }
}