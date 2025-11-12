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
        
        return Results.Created($"api/Post/{created.Id}", created);
    }

    [HttpGet("{userId}")]
    public async Task<IResult> GetSinglePost([FromRoute] int userId)
    {
        try
        {
            Post result = await postRepo.GetSingleAsync(userId);
            return Results.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }

    [HttpDelete("{userId}")]
    public async Task<IResult> DeletePost([FromRoute] int userId)
    {
        await postRepo.DeleteAsync(userId);
        return Results.Ok();
    }

    [HttpPut("{userId}")]
    public async Task<IResult> UpdatePost([FromRoute] int userId,
        [FromBody] PostDto request)
    {
        var post = new Post
        {
            Id = userId,
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserID
        };
        await postRepo.UpdateAsync(post);
        return Results.NoContent();
    }

    [HttpGet]
    public async Task<IResult> GetAllPosts([FromQuery] string? TitleContains,
        [FromQuery] int? userIdContains)
    {
        var posts = postRepo.GetMany();
        
        if (!string.IsNullOrEmpty(TitleContains))
        {
            posts = posts.Where(p => p.Title.ToLower().Contains(TitleContains.ToLower()));
        }
        if (userIdContains.HasValue)
        {
            posts = posts.Where(p => p.UserId == userIdContains.Value);
        }
        return Results.Ok(posts.ToList());
    }
    
    
    
    
}