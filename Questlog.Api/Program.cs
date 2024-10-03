using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Questlog.Api;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Services.Implementations;
using Questlog.Application.Services.Interfaces;
using Questlog.Application.Services.IServices;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;
using Questlog.Infrastructure.Repositories;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Add services to the container.
var connectionString = builder.Configuration["DefaultConnectionString"];


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<TokenValidationFilter>();


builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMainQuestService, MainQuestService>();
builder.Services.AddScoped<IQuestBoardService, QuestBoardService>();
builder.Services.AddScoped<IQuestService, QuestService>();
builder.Services.AddScoped<IUserLevelService, UserLevelService>();





builder.Services.AddControllers(options =>
{
    options.Filters.Add<TokenValidationFilter>();
});

var jwtKey = builder.Configuration["JwtSecret"];
var jwtIssuer = builder.Configuration["JwtIssuer"];
var jwtAudience = builder.Configuration["JwtAudience"];

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ClockSkew = TimeSpan.Zero,
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    Console.WriteLine($"Request Path: {context.Request.Path}");
//    if (context.Request.Headers.ContainsKey("Authorization"))
//    {
//        Console.WriteLine($"Authorization Header: {context.Request.Headers["Authorization"]}");
//    }
//    await next();
//    Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");
//});



app.MapControllers();

app.Run();
