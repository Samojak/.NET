using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;


namespace WebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class UserController
{

    private readonly IUserRepository userRepo;

    public UserController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }
    
    //POST https://localhost:7080/user
    [HttpPost]
    public async Task<IResult> AddUser([FromBody] UserDto request)
    {
        
        var user = new User
        {
            Username = request.UserName,
            Password = request.Password
        };
        User created = await userRepo.AddAsync(user);
        
        return Results.Created($"/user/{created.Id}", created);
    }

    [HttpGet("{userId}")]
    public async Task<IResult> GetSingleUser([FromRoute] int userId)
    {
        try
        {
            User result = await userRepo.GetSingleAsync(userId);
            return Results.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }

    [HttpDelete("{userId}")]
    public async Task<IResult> DeleteUser([FromRoute] int userId)
    {
        await userRepo.DeleteAsync(userId);
        return Results.NoContent();
    }

    [HttpPut("{userId}")]
    public async Task<IResult> UpdateUser([FromRoute] int userId,
        [FromBody] UserDto request)
    {
        var user = new User
        {
            Id = userId,
            Username = request.UserName,
            Password = request.Password
        };
        await userRepo.UpdateAsync(user);
        return Results.NoContent();
    }

    [HttpGet]
    public async Task<IResult> GetAllUsers([FromQuery] string? nameContains)
    {
        var users = userRepo.GetMany();
        if (!string.IsNullOrEmpty(nameContains))
        {
            users = users.Where(u => u.Username.ToLower().Contains(nameContains.ToLower()));
        }
        return Results.Ok(users.ToList());
    }
    
}
