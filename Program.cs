using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using api;
using api.Data;
using api.Interfaces;
using api.Repository;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens; // Replace 'api.Data' with the actual namespace where 'ApplicationDBContext' is defined


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); //pre core 8 need to update later for minimal api
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<ApplicationDBContext>();

var signingKey = builder.Configuration["JwtSigningKey"];

builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme =
    Options.DefaultChallengeScheme =
    Options.DefaultScheme =
    Options.DefaultSignInScheme =
    Options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(signingKey)
        )

    };
});

//getting the secret CLI connection string info 
var _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Configuration.GetConnectionString("DefaultConnection")
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(_connectionString));


builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/HelloWorld", APIendPoints.GetHelloMessage);

app.MapControllers();// for controllers PRE .netcore 8...
app.Run();
