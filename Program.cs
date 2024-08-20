using Microsoft.EntityFrameworkCore;
using BackendApp.Data;
using BackendApp.Controllers;
using BackendApp.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using BackendApp.auth;
using BackendApp.Model;

var corsPolicyName = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApiContext>(
    opt => opt.UseInMemoryDatabase("LinkedOutDb"), 
    contextLifetime: ServiceLifetime.Singleton
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer( options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] 
                            ?? throw new Exception("Key has not been set up.")))
                    };
        }
    );

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: corsPolicyName,
        policy  =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); //TODO: Add only desired origins
        }
    );
});
// builder.Services.AddMvc();
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
builder.Services.AddSingleton<IAdminUserService, AdminUserService>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); TODO: Add later
app.UseCors(corsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Services.GetService<ApiContext>()?
    .AdminUsers
    .Add(new AdminUser("a", EncryptionUtility.HashPassword("bigchungusplayer6969f12")));
app.Run();



