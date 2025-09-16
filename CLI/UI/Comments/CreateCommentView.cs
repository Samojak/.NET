using Entities;
using RepositoryContracts;

namespace CLI.UI.Comments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    
    public CreateCommentView(
        ICommentRepository commentRepository,
        IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
    }
    
    public async Task StartAsync(Post post)
    {
        Console.Clear();
        Console.WriteLine("Add a comment");
        Console.WriteLine("Leave comment empty to cancel.");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine($"Post: {post.Title}");
        Console.WriteLine("-------------------------------------");

      
        int currentUserId = 0;
        User? user = null;

        while (true)
        {
            Console.Write("Your username: ");
            string username = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username is empty. Try again.");
                continue;
            }

          
            var existingUsers = userRepository.GetMany(); 
            user = existingUsers
                .FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user is null)
            {
                Console.WriteLine("User not found. Try again.");
                continue;
            }

            Console.WriteLine($"User found: {user.Username} (Id={user.Id})");
            currentUserId = user.Id;
            break;
        }

 
        Console.Write("Your comment: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Canceled. Press Enter to return.");
            Console.ReadLine();
            return;
        }

      
        var comment = new Comment
        {
            PostId = post.Id,
            UserId = currentUserId,
            Body = body.Trim(),
        };

        await commentRepository.AddAsync(comment);

        Console.WriteLine("Comment added. Press Enter to return.");
        Console.ReadLine();
    }
}
