using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SebTest.Data;
using SebTest.Interfaces;

namespace SebTest.Controllers;
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly TestDbContext  _context;
    private readonly ILogServices  _logServices;
    

    public UserController(
        TestDbContext context, 
        ILogServices logServices)
    {
        _context = context;
        _logServices = logServices;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        await _logServices.LogAsync(
            message: $"Данные пользователей получены",
            level: "Info"
        );
       return await _context.Users.ToListAsync();
    }
}