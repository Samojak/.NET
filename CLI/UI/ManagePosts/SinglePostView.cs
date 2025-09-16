using CLI.UI.Comments;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly CreateCommentView createCommentView;
    
    public SinglePostView(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository, CreateCommentView createCommentView)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.createCommentView = createCommentView;
    }
    
    public async Task StartAsync(Post post)
    {
        var user = await userRepository.GetSingleAsync(post.UserId);
        var comments = await commentRepository.GetByPostIdAsync(post.Id);
        
        Console.WriteLine($"{post.Title}");
        Console.WriteLine(
            $"By: {user.Username} (Id={post.UserId})");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine(post.Body);
        Console.WriteLine("-------------------------------------\n");
        Console.WriteLine("Comments:");
        Console.WriteLine("=====================================");
        if (!comments.Any())
        {
            Console.WriteLine("No comments yet.");
        }
        else
        {
            foreach (var comment in comments)
            {
                var commentUser = await userRepository.GetSingleAsync(comment.UserId);
                Console.WriteLine($"{commentUser?.Username ?? "Unknown"}: {comment.Body}");
                Console.WriteLine("-------------------------------------");
            }
        }

        Console.WriteLine(
            "Options: n = Write a Comment  q=Quit");
        Console.Write("Choice: ");
        string input = Console.ReadLine()?.Trim().ToLower() ?? "";

        if (input == "q")
        {
            return;
        }

        if (input == "n")
        {
            createCommentView.StartAsync(post);
        }
        
        
        
    }
}