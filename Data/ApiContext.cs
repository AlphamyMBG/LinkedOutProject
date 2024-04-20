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
}
