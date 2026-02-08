using Microsoft.EntityFrameworkCore;

namespace SebTest.Data;

public class TestDbContext: DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Log> Logs { get; set; } = null!;    
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
}