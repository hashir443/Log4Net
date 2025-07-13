using System.Collections.Generic;
using Entity.Entity.Home;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Home> Homes { get; set; }
}
