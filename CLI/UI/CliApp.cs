using CLI.UI.Comments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    
    private CreateCommentView  createCommentView;
    private CreatePostView  createPostView;
    private ListPostView   listPostView;
    private SinglePostView  singlePostView;
    private CreateUserView  createUserView;
    private ListUsersView  listUsersView;
    
    
    
    
    
    public CliApp (IUserRepository userRepository,  ICommentRepository commentRepository, IPostRepository postRepository
    , CreateCommentView  createCommentView,CreatePostView createPostView, ListPostView  listPostView, SinglePostView  singlePostView, CreateUserView  createUserView
    , ListUsersView  listUsersView)
    {
        this.userRepository = userRepository;
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.createCommentView = createCommentView;
        this.createPostView = createPostView;
        this.listPostView = listPostView;
        this.singlePostView = singlePostView;
        this.createUserView = createUserView;
        this.listUsersView = listUsersView;
    }

    public async Task StartAsync()
    {

        while (true)
        {
          
            await StartMainMenu();
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": //  1. Create User
                {
                    Console.WriteLine("You chose 1");
                    createUserView.StartAsync();
                    break;
                }
                case "2": // 2. Create Post
                {
                    Console.WriteLine("You chose 2");
                    createPostView.StartAsync();
                    break;
                }case "3": // 3. View Posts
                {
                    Console.WriteLine("You chose 4");
                    listPostView.StartAsync();
                    break;
                }
                case "4": // 4. List Users
                {
                    Console.WriteLine("You chose 5");
                    listUsersView.StartAsync();
                    break;
                }
                case "0": //exit
                {
                    Console.WriteLine("You chose 0");
                    Environment.Exit(0);
                    break;
                }
                default:
                {
                    Console.WriteLine("Invalid choice.");
                    break;
                }
            }
            
        }
        
        
      
        
    }

    private async Task StartMainMenu()
    {
        Console.Clear();
        Console.WriteLine("=====================================");
        Console.WriteLine("      WELCOME TO THE FORUM APP       ");
        Console.WriteLine("=====================================");
        Console.WriteLine();
        Console.WriteLine("Please choose an option:");
        Console.WriteLine();
        Console.WriteLine("  1. Create User");
        Console.WriteLine("  2. Create Post");
        Console.WriteLine("  3. View Posts");
        Console.WriteLine("  4. View Users");
        Console.WriteLine("  0. Exit");
        Console.WriteLine();
        Console.Write("Enter your choice: ");
        
    }

}