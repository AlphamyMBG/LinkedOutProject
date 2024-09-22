using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApp.auth;
using BackendApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Data;

public class ApiContext
(DbContextOptions<ApiContext> options, IConfiguration configuration) 
: DbContext(options)
{

    private readonly IConfiguration configuration = configuration;

    public DbSet<AdminUser> AdminUsers {get; private set;} 
    public DbSet<RegularUser> RegularUsers {get; private set;}
    public DbSet<Post> Posts {get; private set;}
    public DbSet<JobPost> JobPosts {get; private set;}
    public DbSet<Notification> Notifications {get; private set;}
    public DbSet<Message> Messages {get; private set;}
    public DbSet<Connection> Connections {get; private set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityByDefaultColumns();
        modelBuilder.Entity<RegularUser>()
            .HasOne( u => u.HideableInfo )
            .WithOne()
            .HasForeignKey<RegularUserHideableInfo>("UserId")
            .IsRequired();

        modelBuilder.Entity<Message>()
            .HasOne( m => m.SentBy )
            .WithMany()
            .HasForeignKey("SentId");
        modelBuilder.Entity<Message>()
            .HasOne( m => m.SentTo )
            .WithMany()
            .HasForeignKey("ReceivedId");
            
        modelBuilder.Entity<PostBase>()
            .HasOne( p => p.PostedBy )
            .WithMany()
            .HasForeignKey("PostedById");
        modelBuilder.Entity<PostBase>()
            .HasMany(p => p.InterestedUsers)
            .WithMany("LikedPosts");
        modelBuilder.Entity<PostBase>()
            .HasMany(p => p.PostFiles)
            .WithMany("PostsUsedIn");
        
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Replies)
            .WithOne()
            .HasForeignKey("OriginalPost");
        
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.ToUser)
            .WithMany()
            .HasForeignKey("NotificationsIds");
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.AssociatedPost)
            .WithMany()
            .HasForeignKey("NotificIds");

        modelBuilder.Entity<Connection>()
            .HasOne( n => n.SentBy)
            .WithMany()
            .HasForeignKey("UsersSentNotificationId");
        modelBuilder.Entity<Connection>()
            .HasOne( n => n.SentTo)
            .WithMany()
            .HasForeignKey("UsersReceivedNotificationId");
        


        // modelBuilder
    }
}
