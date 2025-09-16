using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;
    
    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    
    
    
    public async Task StartAsync()
    {
        var users = userRepository.GetMany().ToList();
        if (!users.Any())
        {
            Console.WriteLine("No users yet!");
        }
        else
        {
        
            Console.Clear();
            Console.WriteLine("=====================================");
            Console.WriteLine("          List of Users");
            Console.WriteLine("=====================================");



            foreach (var user in users)
            {
                Console.WriteLine($"[{user.Id}] {user.Username}");
                Console.WriteLine("-------------------------------------");
            }
        }

        Console.WriteLine("Press Enter to return to the main menu...");
        Console.ReadLine();
    }
}