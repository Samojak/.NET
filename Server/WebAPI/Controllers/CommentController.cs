using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using RepositoryContracts;

namespace WebAPI.Controllers;



[ApiController]
[Route("[controller]")]
public class CommentController
{
    
    private readonly ICommentRepository commentRepo;

    public CommentController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }

    [HttpPost]
    public async Task<IResult> AddComment([FromBody] CommentDto request)
    {
        var comment = new Comment
        {
            Body = request.Body,
            PostId = request.PostId,
            UserId = request.UserId
        };
        Comment created = await commentRepo.AddAsync(comment);
        
        return Results.Created($"Comment/ {created.Id}",created);
        
    }

    [HttpGet("{commentId}")]
    public async Task<IResult> GetSingleComment([FromRoute] int commentId)
    {
        try
        {
            Comment result = await commentRepo.GetSingleAsync(commentId);
            return Results.Ok(result);
        }
        catch (Exception e) 
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }

    [HttpDelete("{commentId}")]
    public async Task<IResult> DeleteComment([FromRoute] int commentId)
    {
        await commentRepo.DeleteAsync(commentId);
        return Results.NoContent();
    }

    [HttpPut("{commentId}")]
    public async Task<IResult> UpdateComment([FromRoute] int commentId,
        [FromBody] CommentDto request)
    {
        var comment = new Comment
        {
            Id = commentId,
            Body = request.Body,
            PostId = request.PostId,
            UserId = request.UserId
        };
        await commentRepo.UpdateAsync(comment);
        return Results.NoContent();
    }

    [HttpGet]
    public async Task<IResult> GetAllUsers([FromQuery] int? userId,[FromQuery] int? postId )

    {
        var comments = commentRepo.GetMany();
        if (userId.HasValue)
        {
            comments = comments.Where(c =>
                c.UserId==userId.Value);
        }
        if (postId.HasValue)
        {
            comments = comments.Where(c =>
                c.PostId==postId.Value);
        }

        return Results.Ok(comments.ToList());
    }
}