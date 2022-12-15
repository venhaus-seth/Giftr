#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace Giftr.Models; // fill in ProjectName
public class MyContext : DbContext
{
    public MyContext(DbContextOptions options) : base(options) { }
    // create the following line for every model
    public DbSet<User> Users { get; set; } 
    public DbSet<GiftExchange> GiftExchanges { get; set; } 
    public DbSet<Member> Members { get; set; } 
    public DbSet<Item> Items { get; set; } 
}