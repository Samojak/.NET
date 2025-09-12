using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;


public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    
    public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
        {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        }



    public async Task StartAsync()
    {
        Console.Clear();
        Console.WriteLine("=====================================");
        Console.WriteLine("          CREATE NEW POST");
        Console.WriteLine("=====================================");

        string username;
        User ? user;
        string title;
        string body;

        while (true)
        {
            Console.Write("Username: ");
            username = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username is empty");
                continue;
            }

            var existing = userRepository.GetMany();
            user = existing
                .FirstOrDefault(u => u.Username.Equals(username,
                    StringComparison.CurrentCultureIgnoreCase));

            if (user is not null)
            {
                Console.WriteLine(
                    $"User found: {user.Username} (Id={user.Id})");
                break; // exit the loop now that you have the user
            }
            Console.WriteLine("User not found ");
        }

        while (true)
                {
                    Console.Write("Title of the Post: ");
                    title = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(title))
                    {
                        Console.WriteLine("Title is empty");
                        continue;
                    }

                    break;
                }

                while (true)
                {
                    Console.Write("Text of the Post: ");
                    body = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(body))
                    {
                        Console.WriteLine("Text is empty");
                    }
                    break;
                }
                
            
        var post = new Post
        {
            UserId = user.Id,
            Title = title,
            Body = body,
            
        };

        await postRepository.AddAsync(post);
        Console.WriteLine();
        Console.WriteLine(" Post created successfully!");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine($"Post Id:   {post.Id}");
        Console.WriteLine($"Author:    {user.Username} (Id={user.Id})");
        Console.WriteLine($"Title:     {post.Title}");
        Console.WriteLine("Body:");
        Console.WriteLine(post.Body);
        Console.WriteLine("-------------------------------------");
        Console.WriteLine();
        Console.WriteLine("Press Enter to return to the main menu...");
        Console.ReadLine();

    }
}