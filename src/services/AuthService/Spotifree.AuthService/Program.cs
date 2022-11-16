using Microsoft.Extensions.Configuration;
using Spotifree.AuthService;
using Spotifree.AuthService.Composition.Installer;
using Spotifree.AuthService.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();
new DbInstaller().InstallServices(builder.Services, builder.Configuration);
new AuthInstaller(builder.Configuration["Jwt:Secret"]).InstallServices(builder.Services, builder.Configuration);
new ServicesInstaller().InstallServices(builder.Services, builder.Configuration);
new LogicInstaller().InstallServices(builder.Services, builder.Configuration);


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapControllers();

app.Run();
