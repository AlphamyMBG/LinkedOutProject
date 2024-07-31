using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Data;

public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<RegularUser> RegularUsers {get; set;}
    public DbSet<Post> Posts {get; set;}
    public DbSet<JobPost> JobPost {get; set;}
    public DbSet<Notification> Notifications {get; set;}
    public DbSet<Message> Messages {get; set;}
    public DbSet<Connection> Connections {get; set;}
}
