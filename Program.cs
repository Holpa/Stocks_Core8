using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using api;
using api.Data;
using api.Interfaces;
using api.Repository; // Replace 'api.Data' with the actual namespace where 'ApplicationDBContext' is defined


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); //pre core 8 need to update later for minimal api
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapGet("/HelloWorld", APIendPoints.GetHelloMessage);

app.MapControllers();// for controllers PRE .netcore 8...
app.Run();
