using EventSourcing.PostImpression.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.PostImpression.Infrastructure.Persistent;

public class NewsImpressionDbContext(DbContextOptions<NewsImpressionDbContext> options) : DbContext(options)
{
    public virtual DbSet<NewsEventStore> NewsEventStores { get; set; }
    public virtual DbSet<NewsImpressionSummary> NewsImpressionSummaries { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewsEventStore>().HasKey(e => e.Id);
        modelBuilder.Entity<NewsEventStore>().Property(p=>p.EventType).HasMaxLength(100);
        
        modelBuilder.Entity<NewsImpressionSummary>().HasKey(e => e.NewsId);
    }
}