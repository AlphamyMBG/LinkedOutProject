using Microsoft.EntityFrameworkCore;
using BackendApp.Data;
using BackendApp.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BackendApp.auth;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using BackendApp.auth.Filters;
using BackendApp.Auth;
using ImageManipulation.Data.Services;
using Util.DataFeeding;

var corsPolicyName = "_myAllowAllOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiContext>(
    opt => opt
        .UseLazyLoadingProxies()
        .UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")), 
    contextLifetime: ServiceLifetime.Scoped
);

//Add model services
builder.Services.AddSingleton<IRegularUserService, RegularUserService>();
builder.Services.AddSingleton<IPostService, PostService>();
builder.Services.AddSingleton<IJobService, JobService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<IInterestService, InterestService>();
builder.Services.AddSingleton<IConnectionService, ConnectionService>();
builder.Services.AddSingleton<IAdminUserService, AdminUserService>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<IRecommendationService, RecommendationService>();
builder.Services.AddSingleton<ITimelineService, TimelineService>();

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
builder.Services.AddSingleton<IAuthorizationHandler, SentConnectionRequestHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ReceivedConnectionRequestHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CreatedJobHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CreatedPostHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, IsMemberOfConversationHandler>();
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
        policy.Requirements.Add( new SentMessageRequirement("id")))
    .AddPolicy(AuthConstants.PolicyNames.SentConnectionRequestPolicyName, policy => 
        policy.Requirements.Add( new SentConnectionRequestRequirement("id")))
    .AddPolicy(AuthConstants.PolicyNames.ReceivedConnectionRequestPolicyName, policy => 
        policy.Requirements.Add( new ReceivedConnectionRequestRequirement("id")))
    .AddPolicy(AuthConstants.PolicyNames.CreatedJobPolicyName, policy => 
        policy.Requirements.Add( new CreatedJobRequirement("id")))
    .AddPolicy(AuthConstants.PolicyNames.CreatedPostPolicyName, policy => 
        policy.Requirements.Add( new CreatedPostRequirement("id")))
    .AddPolicy(AuthConstants.PolicyNames.IsMemberOfConversationPolicyName, policy => 
        policy.Requirements.Add( new IsMemberOfConversationRequirement("userAId","userBId")));
    

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// DummyDataFeeder.FillWithDummyData(app.Services.GetService<ApiContext>() ?? throw new Exception("Api Context was not set up correctly."));
app.Run();



