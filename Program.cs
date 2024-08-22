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
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using BackendApp.auth.Filters;
using BackendApp.Auth;

var corsPolicyName = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Set DB context up.
builder.Services.AddDbContext<ApiContext>(
    opt => opt.UseInMemoryDatabase("LinkedOutDb"), 
    contextLifetime: ServiceLifetime.Singleton
);

//Add model services
builder.Services.AddSingleton<IRegularUserService, RegularUserService>();
builder.Services.AddSingleton<ILinkedOutPostService, LinkedOutPostService>();
builder.Services.AddSingleton<ILinkedOutJobService, LinkedOutJobService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<IInterestService, InterestService>();
builder.Services.AddSingleton<IConnectionService, ConnectionService>();
builder.Services.AddSingleton<IAdminUserService, AdminUserService>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

//Add other general use case services
builder.Services.AddHttpContextAccessor();

// Controllers
builder.Services.AddControllers();

// Add API explorer page
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( setup =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

// CORS

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

// Authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer( 
        options =>
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

// Add Authorization
builder.Services.AddSingleton<IAuthorizationHandler, HasIdHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, HasNotificationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SentMessageHandler>();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthConstants.PolicyNames.HasIdEqualToUserIdParamPolicyName, policy =>
        policy.Requirements.Add( new HasIdRequirement("userId")))
    .AddPolicy(AuthConstants.PolicyNames.HasIdEqualToSenderIdPolicyName, policy =>
        policy.Requirements.Add( new HasIdRequirement("senderId")))
    .AddPolicy(AuthConstants.PolicyNames.HasIdEqualToIdParamPolicyName, policy =>
        policy.Requirements.Add( new HasIdRequirement("id")))
    .AddPolicy(AuthConstants.PolicyNames.IsAdminPolicyName, policy => 
        policy.Requirements.Add( new IsAdminRequirement()))
    .AddPolicy(AuthConstants.PolicyNames.HasNotificationPolicyName, policy => 
        policy.Requirements.Add( new HasNotificationRequirement("id")))
    .AddPolicy(AuthConstants.PolicyNames.SentMessagePolicyName, policy => 
        policy.Requirements.Add( new SentMessageRequirement("id")));
    

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

app.Services.GetService<ApiContext>()?.
    AdminUsers
    .Add(new AdminUser("a@emailer.com", EncryptionUtility.HashPassword("bigchungusplayer6969f12"))
    { Id = 1 });
app.Services.GetService<ApiContext>()?.
    RegularUsers
    .Add(new RegularUser(
        "b@emailer.com",
        EncryptionUtility.HashPassword("bigchungusplayer6969f12"), 
        "name", "poop", "6900000000", "dadaby car",
        "The Rizzler", [], "")
    { Id = 1 });
app.Services.GetService<ApiContext>()?.SaveChanges();  

app.Run();



