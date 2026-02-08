using SebTest.Data;
using SebTest.Interfaces;

namespace SebTest.Services;

public class LogServices : ILogServices
{
    private readonly TestDbContext _context;
    private readonly ILogger<LogServices> _logger;

    public LogServices(
        TestDbContext context, 
        ILogger<LogServices> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task LogAsync(
        string message,
        string level = "Info",
        int? userId = null,
        string? details = null)
    {
        var log = new Log
        {
            Timestamp = DateTime.UtcNow,  
            Level     = level,
            UserId    = userId,
            Message   = message,
            Details   = details
        };
        _context.Logs.Add(log);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}