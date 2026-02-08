namespace SebTest.Interfaces;

public interface ILogServices
{
    Task LogAsync(
        string message,
        string level = "Info",
        int? userId = null,
        string? details = null);
}