using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SebTest.Data;
using SebTest.Interfaces;
using SebTest.Services;
using SebTest.Models.Requests;

namespace SebTest.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly TestDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly ILogServices _logServices;

    public AuthController(
        TestDbContext context,
        PasswordService passwordService,
        ITokenService tokenService,
        ILogServices logServices)
    {
        _context = context;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _logServices = logServices;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "Заполните все поля" });
        }

        bool exists = await _context.Users
            .AnyAsync(u => u.Username == request.Username);

        if (exists)
        {
            return Conflict(new { error = "Такой логин уже занят" });
        }

        string hashedPassword = _passwordService.Hash(request.Password);

        var user = new User
        {
            Username = request.Username,
            Password = hashedPassword,
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        await _logServices.LogAsync(
            message: $"Регистрация успешна: "+request.Username,
            level: "Info",
            userId: user.Id
        );
        return Ok(new { message = "Регистрация прошла успешно" });


    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !_passwordService.Verify(request.Password, user.Password))
        {
            await _logServices.LogAsync(
                message: "Неудачная попытка входа",
                level: "Warning",
                details: $"Username: {request.Username}"
            );
            return Unauthorized(new { error = "Неверный логин или пароль" });

        }

        var token = _tokenService.BuildToken(user);

        await _logServices.LogAsync(
            message: "Успешный вход",
            level: "Info",
            userId: user.Id
        );
        return Ok(new { token });

    }
}



