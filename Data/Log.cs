namespace SebTest.Data;

public class Log
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = "Info";
    public int? UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
}