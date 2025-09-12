using Entities;
using RepositoryContracts;


namespace InMemoryepositories;

public class UserInMemomoryRepository : IUserRepository
{
    public List<User> Users = new List<User>();
    
    public UserInMemomoryRepository() => Seed();

    private void Seed()
    {
        Users.Clear();
        Users.AddRange(new[]
        {
            new User { Id = 1, Username = "alice", Password = "pwd123" },
            new User { Id = 2, Username = "bob",   Password = "hunter2" },
            new User { Id = 3, Username = "carol", Password = "secret" }
        });
    }
    
    public Task<User> AddAsync(User user)
    {
        user.Id = Users.Any()
            ? Users.Max(u => u.Id) + 1
            : 1;
        Users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = Users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}'not found");
        }

        Users.Remove(existingUser);
        Users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int Id)
    {
        User? userToRemove = Users.SingleOrDefault(u => u.Id == Id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{Id}' not found");
        }
        Users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int Id)
    {
        User? userToGet = Users.SingleOrDefault(u => u.Id == Id);
        if (userToGet is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{Id}' not found");
        }
        return Task.FromResult(userToGet);
    }

    public IQueryable<User> GetMany()
    {
        return Users.AsQueryable();
    }
}