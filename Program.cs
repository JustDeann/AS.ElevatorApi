using ElevatorApi.Options;
using ElevatorApi.Services;

var builder = WebApplication.CreateBuilder(args);
//set the url to listen on all interfaces on port 8080
builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Elevator Dispatch API",
        Version = "v1",
        Description = "Minimal integration surface for elevator dispatch workflows."
    });
});

builder.Services.Configure<ElevatorDispatchOptions>(builder.Configuration.GetSection("Dispatching"));
builder.Services.AddSingleton<IElevatorDispatchService, ElevatorDispatchService>();
var app = builder.Build();
//I like to use swagger for testing APIs
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Elevator API V1");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
