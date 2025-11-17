using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private readonly IUserRepository userRepository;
    
    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IResult> Login([FromBody] LoginRequest request)
    {
        Console.WriteLine($"LOGIN DEBUG -> UserName = '{request.UserName}', Password = '{request.Password}'");

        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            return Results.Unauthorized();

        User user;
        try
        {
            user = await userRepository.GetUserByUsernameAsync(request.UserName);
            Console.WriteLine($"FOUND USER IN DB: Username={user.Username}, Password={(user.Password == null ? "NULL" : user.Password)}");

        }
        
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e);
            return Results.Unauthorized();
        }

        if (user.Password != request.Password)
        {
            Console.WriteLine($"FOUND USER IN DB: Username={user.Username}, Password={(user.Password == null ? "NULL" : user.Password)}");

            return Results.Unauthorized();
        }

        var dto = new UserDto
        {
            UserName = user.Username
        };
        
        Console.WriteLine($"FOUND USER IN DB: Username={user.Username}, Password={(user.Password == null ? "NULL" : user.Password)}");

        
    return Results.Ok(dto);    
    }
    
    
    

}