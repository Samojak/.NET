using System.Collections;
using Entities;
using RepositoryContracts;

namespace InMemoryepositories;

public class CommentInMemomoryRepository : ICommentRepository
{
    public List<Comment> Comments = new List<Comment>();
    
    public CommentInMemomoryRepository() => Seed();

    private void Seed()
    {
        Comments.Clear();
        Comments.AddRange(new[]
        {
            new Comment { Id = 1, Body = "Hi everyone! 👋",             PostId = 1, UserId = 2 },
            new Comment { Id = 2, Body = "I prefer Rider for C#.",      PostId = 3, UserId = 1 },
            new Comment { Id = 3, Body = "LINQ + records are awesome.", PostId = 2, UserId = 3 }
        });
    }

    
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = Comments.Any()
            ? Comments.Max(c => c.Id) + 1
            : 1;
        Comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = Comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}'not found");
        }

        Comments.Remove(existingComment);
        Comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int Id)
    {
        Comment? commentToRemove = Comments.SingleOrDefault(c => c.Id == Id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{Id}' not found");
        }
        Comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int Id)
    {
        Comment? commentToGet = Comments.SingleOrDefault(c => c.Id == Id);
        if (commentToGet is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{Id}' not found");
        }
        return Task.FromResult(commentToGet);
    }

    public IQueryable<Comment> GetMany()
    {
        return Comments.AsQueryable();
    }

    public Task<IEnumerable<Comment>> GetByPostIdAsync(int postId)
    {
        var result = Comments.Where(c => c.PostId == postId);
        return Task.FromResult(result);
    }
}