using Microsoft.AspNetCore.Builder;

var appBuilder = WebApplication.CreateBuilder(args);

var services = appBuilder.Services;

using var app = appBuilder.Build();

app.UseRouting();

app.Run();

public partial class Program { }
