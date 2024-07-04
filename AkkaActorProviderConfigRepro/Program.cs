using Akka.Cluster.Hosting;
using Akka.Hosting;
using Akka.Remote.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAkka("repro", (configurationBuilder, _) =>
    configurationBuilder
        .WithRemoting("localhost", 8081)
        .WithClustering(new ClusterOptions
        {
            SeedNodes = [ "akka.tcp://repro@localhost:8081" ]
        })
        .ConfigureLoggers(loggerConfig =>
        {
            loggerConfig.AddDefaultLogger();
            loggerConfig.LogConfigOnStart = true;
        }));

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