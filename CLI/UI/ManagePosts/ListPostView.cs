using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly SinglePostView singlePostView;



    public ListPostView(IPostRepository postRepository,
        IUserRepository userRepository, ICommentRepository commentRepository,
        SinglePostView singlePostView)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.singlePostView = singlePostView;
    }

    public async Task StartAsync()
    {


        var posts = postRepository.GetMany().ToList();
        int totalPosts = posts.Count;
        int pageSize = 10;
        int totalPages = (totalPosts + pageSize - 1) / pageSize;
        int currentPage = 1;



        if (totalPosts == 0)
        {
            Console.WriteLine("No posts yet!");
            Console.WriteLine("Press Enter to return to the main menu...");
            Console.ReadLine();
            return;
        }

        while (true)
        {

            Console.Clear();
            Console.WriteLine("=====================================");
            Console.WriteLine("          List of Posts");
            Console.WriteLine(" Page {currentPage} of {totalPages}  ");
            Console.WriteLine("=====================================");

            int startIndex = (currentPage - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, totalPosts);


            for (int i = startIndex; i < endIndex; i++)
            {
                var post = posts[i];
                var user = await userRepository.GetSingleAsync(post.UserId);

                int localIndex = i - startIndex + 1; // 1–10 numbering
                Console.WriteLine($"[{localIndex}] {post.Title}");
                Console.WriteLine(
                    $"By: {user?.Username ?? "Unknown"} (Id={post.UserId})");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine(post.Body);
                Console.WriteLine("-------------------------------------\n");
            }


            Console.WriteLine(
                "Options: [1-10]=Open post  n=Next  p=Prev  q=Quit");
            Console.Write("Choice: ");
            string input = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (input == "q") break;
            if (input == "n")
            {
                if (currentPage < totalPages) currentPage++;
                continue;
            }

            if (input == "p")
            {
                if (currentPage > 1) currentPage--;
                continue;
            }

            if (int.TryParse(input, out int choice))
            {
                int selectedIndex = startIndex + (choice - 1);
                if (choice >= 1 && choice <= (endIndex - startIndex))
                {
                    var selectedPost = posts[selectedIndex];
                    await singlePostView.StartAsync(selectedPost);
                }
                else
                {
                    Console.WriteLine("Invalid choice for this page.");
                    Console.ReadKey();
                }

            }
        }
    }
}
