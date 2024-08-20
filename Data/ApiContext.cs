using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApp.auth;
using BackendApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Data;

public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<AdminUser> AdminUsers {get; private set;} 
    public DbSet<RegularUser> RegularUsers {get; private set;}
    public DbSet<Post> Posts {get; private set;}
    public DbSet<JobPost> JobPost {get; private set;}
    public DbSet<Notification> Notifications {get; private set;}
    public DbSet<Message> Messages {get; private set;}
    public DbSet<Connection> Connections {get; private set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // var admin = new AdminUser("a", EncryptionUtility.HashPassword("bigchungusplayer6969f12"))
        // {
        //     Id = 1
        // };
        // modelBuilder.Entity<AdminUser>().HasData(
        //     admin
        // );
    }
}
