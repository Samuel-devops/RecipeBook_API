using Microsoft.EntityFrameworkCore;
using RecipeBook_API.Domain.Infrastructure;
using RecipeBook_API.Mapping;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=recipebook.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Recipe Book API")
    .WithTheme(ScalarTheme.DeepSpace);
});

app.Run();