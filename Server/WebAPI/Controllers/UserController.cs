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
    public async Task<IResult> AddUser([FromBody] RegisterUserDto request)
    {
        
        var user = new User
        {
            Username = request.UserName,
            Password = request.Password
        };
        
        
        
        User created = await userRepo.AddAsync(user);
        
        var dto = new UserDto
        {
            UserName = created.Username,
           
        };
        
        return Results.Created($"/user/{created.Id}", dto);
    }

    [HttpGet("{userId}")]
    public async Task<IResult> GetSingleUser([FromRoute] int userId)
    {
        try
        {
            User result = await userRepo.GetSingleAsync(userId);
            
            var dto = new UserDto
            {
                UserName = result.Username,
            
            };
            
            return Results.Ok(dto);
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
        
        };
        await userRepo.UpdateAsync(user);
        return Results.NoContent();
    }

    // GET /user
    [HttpGet]
    public IResult GetAllUsers([FromQuery] string? nameContains)
    {
        var users = userRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(nameContains))
            users = users.Where(u => u.Username.Contains(nameContains, StringComparison.OrdinalIgnoreCase));

        var dtos = users.Select(u => new UserDto
        {
            UserName = u.Username,
           
        }).ToList();

        return Results.Ok(dtos);
    }

    
}
