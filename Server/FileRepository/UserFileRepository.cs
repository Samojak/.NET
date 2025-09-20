using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepository;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "User.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    
    public async Task<User> AddAsync(User user)
    {
        string userAsJson = await File.ReadAllTextAsync(filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(userAsJson);
        users ??= new List<User>();
       
        int nextId = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
        user.Id = nextId;
       
        users.Add(user);
        userAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, userAsJson);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        // Deserilize the json
        string userAsJson = await File.ReadAllTextAsync(filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(userAsJson);
        users ??= new List<User>();
        
        // find the user
        User? userToUpdate = users.Find(u => u.Id == user.Id);
        if (userToUpdate is null)
        {
            throw new InvalidOperationException($"User with ID '{user.Id}' not found");
        }
        
        // update the user
        users.Remove(userToUpdate);
        users.Add(user);
        
        // save to file updated list
        userAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, userAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        // Deserilize the json
        string userAsJson = await File.ReadAllTextAsync(filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(userAsJson);
        users ??= new List<User>();
        
        // find the user
        User? userToRemove = users.Find(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }
        
        // remove the user
        users.Remove(userToRemove);
        
        // save to file updated list
        userAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, userAsJson);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        // Deserilize the json
        string userAsJson = await File.ReadAllTextAsync(filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(userAsJson);
        users ??= new List<User>();
        
        // find the user
        User? userToFind = users.Find(u => u.Id == id);
        if (userToFind is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }

        return await Task.FromResult(userToFind);
    }

    public IQueryable<User> GetMany()
    {
        // Deserilize the json
        string userAsJson = File.ReadAllTextAsync(filePath).Result;
        List<User>? users = JsonSerializer.Deserialize<List<User>>(userAsJson);
        users ??= new List<User>();

        return users.AsQueryable();
    }
}
