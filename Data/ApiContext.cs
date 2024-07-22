using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Data;

public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<LinkedOutUser> LinkedOutUsers {get; set;}
    public DbSet<LinkedOutPost> LinkedOutPosts {get; set;}
    public DbSet<LinkedOutJob> LinkedOutJobs {get; set;}
    public DbSet<Notification> Notifications {get; set;}
    public DbSet<Message> Messages {get; set;}
}
