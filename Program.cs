using Microsoft.EntityFrameworkCore;
using BackendApp.Data;
using BackendApp.Controllers;
using BackendApp.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApiContext>(
    opt => opt.UseInMemoryDatabase("LinkOnDb"), 
    contextLifetime: ServiceLifetime.Singleton
);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRegularUserService, RegularUserService>();
builder.Services.AddSingleton<ILinkedOutPostService, LinkedOutPostService>();
builder.Services.AddSingleton<ILinkedOutJobService, LinkedOutJobService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<IInterestService, InterestService>();
builder.Services.AddSingleton<IConnectionService, ConnectionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); TODO: Add later
app.MapControllers();
app.Run();


