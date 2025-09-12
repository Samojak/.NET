using Entities;
using RepositoryContracts;

namespace InMemoryepositories;

public class PostInMemoryRepository : IPostRepository
{
    public List<Post> Posts = new List<Post>();
    
    public PostInMemoryRepository() => Seed();

    private void Seed()
    {
        Posts.Clear();
        Posts.AddRange(new[]
        {
            new Post { Id = 1, UserId = 1, Title = "Welcome to the forum", Body = "Say hi and introduce yourself!" },
            new Post { Id = 2, UserId = 2, Title = "C# vs Java",           Body = "What surprised you moving from Java to C#?" },
            new Post { Id = 3, UserId = 2, Title = "Best IDE?",            Body = "Rider or Visual Studio—what do you like?" }
        });
    }
    
    public Task<Post> AddAsync(Post post)
    {
        post.Id = Posts.Any()
            ? Posts.Max(p => p.Id) + 1
            : 1;
        Posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = Posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
            {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}'not found");
            }

        Posts.Remove(existingPost);
        Posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int Id)
    {
        Post? postToRemove = Posts.SingleOrDefault(p => p.Id == Id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{Id}' not found");
        }
        Posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int Id)
    {
        Post? postToGet = Posts.SingleOrDefault(p => p.Id == Id);
        if (postToGet is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{Id}' not found");
        }
        return Task.FromResult(postToGet);
    }

    public IQueryable<Post> GetMany()
    {
        return Posts.AsQueryable();
    }
    
}