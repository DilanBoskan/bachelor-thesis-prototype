using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebApi();

var app = builder.Build();

app.UseEndpointDefintions();

app.Run();

public partial class Program;