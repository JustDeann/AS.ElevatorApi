
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
