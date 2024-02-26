using FilmesApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Adiciona o AutoMapper a aplica��o
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Definido conex�o com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("FilmesConnection");

builder.Services.AddDbContext<FilmeContext>(opts => opts.UseMySql (connectionString, ServerVersion.AutoDetect(connectionString)));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
