using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;



[ApiController]
[Route("[controller]")]
public class PostController
{
    private readonly IPostRepository postRepo;

    public PostController(IPostRepository postRepo)
    {
        this.postRepo = postRepo;
    }


    [HttpPost]
    public async Task<IResult> AddPost([FromBody] PostDto request)
    {
        var post = new Post()
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserID
        };
        Post created = await postRepo.AddAsync(post);
        
        
        return Results.Created($"/post/{created.Id}", ToDto(created));
    }

    [HttpGet("{postId}")]
    public async Task<IResult> GetSinglePost([FromRoute] int postId)
    {
        try
        {
            Post result = await postRepo.GetSingleAsync(postId);
            
            
            return Results.Ok(ToDto(result));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }

    [HttpDelete("{postId}")]
    public async Task<IResult> DeletePost([FromRoute] int postId)
    {
        await postRepo.DeleteAsync(postId);
        return Results.NoContent();
    }

    [HttpPut("{postId}")]
    public async Task<IResult> UpdatePost([FromRoute] int postId,
        [FromBody] PostDto request)
    {
        var post = new Post
        {
            Id = postId,
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserID
        };
        await postRepo.UpdateAsync(post);
        return Results.NoContent();
    }

    [HttpGet]
    public IResult GetAllPosts([FromQuery] string? q, [FromQuery] int? userId)
    {
        var posts = postRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(q))
            posts = posts.Where(p => p.Title.Contains(q, StringComparison.OrdinalIgnoreCase));

        if (userId.HasValue)
            posts = posts.Where(p => p.UserId == userId.Value);

        var dtos = posts.Select(ToDto).ToList();
        return Results.Ok(dtos);
    }

    // helpers
    private static PostDto ToDto(Post p) => new()
    {
        Id = p.Id,
        Title = p.Title,
        Body  = p.Body,
        UserID = p.UserId   // map entity UserId -> DTO UserID
        
    };
    
    
    
    
}