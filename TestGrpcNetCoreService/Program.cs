using Microsoft.AspNetCore.Server.Kestrel.Core;

using TestGrpcNetCoreService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();


app.UseRouting();

app.UseAuthorization();

IWebHostEnvironment env = app.Environment;

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    endpoints.MapGrpcService<GreeterService>();
    endpoints.MapControllers();
    endpoints.MapGrpcReflectionService();
});

app.Run();
