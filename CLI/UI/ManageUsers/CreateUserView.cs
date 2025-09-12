using RepositoryContracts;
using Entities;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }



    public async Task StartAsync()
    {
        Console.Clear();
        Console.WriteLine("=====================================");
        Console.WriteLine("          CREATE NEW USER");
        Console.WriteLine("=====================================");

        string username;

        while (true)
        {
            Console.Write("Username: ");
            username = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username is empty");
                continue;
            }

            var exiting = userRepository.GetMany();
            if (exiting.Any(u => u.Username.Equals(username,
                    StringComparison.CurrentCultureIgnoreCase)))
            {
                Console.WriteLine("Username is already taken");
                continue;
            }

            break;
        }

        string password;
        while (true)
        {
            Console.Write("Password: ");
            password = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password is empty");
                continue;
            }

            break;

        }

        var user = new User
        {
            Username = username,
            Password = password
        };

        await userRepository.AddAsync(user);
        Console.WriteLine();
        Console.WriteLine("User created successfully!");
        Console.WriteLine($"Id: {user.Id}, Username: {user.Username}");
        Console.WriteLine();
        Console.WriteLine("Press Enter to return to main menu...");
        Console.ReadLine();
    }
    
}