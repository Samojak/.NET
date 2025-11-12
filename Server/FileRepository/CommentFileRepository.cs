using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepository;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "Comment.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            // Use [] because we store a List<Comment>
            File.WriteAllText(filePath, "[]");
        }
    }
    
    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson);
        if (comments == null)
        {
            comments = new List<Comment>();
        }

        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
        comment.Id = maxId + 1;

        comments.Add(comment);
        commentAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentAsJson);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        // Deserilize the json
        string commentAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson);
        if (comments == null)
        {
            comments = new List<Comment>();
        }
        
        // find the comment
        Comment? commentToUpdate = comments.Find(c => c.Id == comment.Id);
        if (commentToUpdate is null)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
        }
        
        // update the comment
        comments.Remove(commentToUpdate);
        comments.Add(comment);
        
        // save to file updated list
        commentAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        // Deserilize the json
        string commentAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson);
        if (comments == null)
        {
            comments = new List<Comment>();
        }
        
        // find the comment
        Comment? commentToRemove = comments.Find(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        
        // remove the comment
        comments.Remove(commentToRemove);
        
        // save to file updated list
        commentAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentAsJson);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        // Deserilize the json
        string commentAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson);
        if (comments == null)
        {
            comments = new List<Comment>();
        }
        
        // find the comment
        Comment? commentToFind = comments.Find(c => c.Id == id);
        if (commentToFind is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        return await Task.FromResult(commentToFind);
    }

    public IQueryable<Comment> GetMany()
    {
        // Deserilize the json
        string commentAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson);
        if (comments == null)
        {
            comments = new List<Comment>();
        }

        return comments.AsQueryable();
    }

    public Task<IEnumerable<Comment>> GetByPostIdAsync(int postId)
    {
        string commentAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson);
        if (comments == null)
        {
            comments = new List<Comment>();
        }
        
        var result = comments.Where(c => c.PostId == postId);
        return Task.FromResult(result);
    }
}
