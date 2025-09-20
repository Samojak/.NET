using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepository;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "Post.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    
    public async Task<Post> AddAsync(Post post)
    {
        string postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post>? posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        if (posts == null)
        {
            posts = new List<Post>();
        }
       
        int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;
        post.Id = maxId + 1;

        posts.Add(post);
        postAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postAsJson);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post>? posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        if (posts == null)
        {
            posts = new List<Post>();
        }
        
        Post? postToUpdate = posts.Find(p => p.Id == post.Id);
        if (postToUpdate is null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
        }
        
        posts.Remove(postToUpdate);
        posts.Add(post);
        
        postAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post>? posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        if (posts == null)
        {
            posts = new List<Post>();
        }
        
        Post? postToRemove = posts.Find(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }
        
        posts.Remove(postToRemove);
        
        postAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postAsJson);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        string postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post>? posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        if (posts == null)
        {
            posts = new List<Post>();
        }
        
        Post? postToFind = posts.Find(p => p.Id == id);
        if (postToFind is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        return postToFind;
    }

    public IQueryable<Post> GetMany()
    {
        string postAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post>? posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        if (posts == null)
        {
            posts = new List<Post>();
        }

        return posts.AsQueryable();
    }
}
