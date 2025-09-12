using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostView
{
    private readonly IPostRepository postRepository;
    
    public ListPostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }
    
    public async Task StartAsync()
    {
        
    }
}