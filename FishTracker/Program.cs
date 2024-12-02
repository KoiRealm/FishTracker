using FishTracker.Manager;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.TryAddSingleton<IBitTorrentManager, BitTorrentManager>();

builder.Services.AddQuartz(q =>
{
    // base quartz scheduler, job and trigger configuration
});

//// ASP.NET Core hosting
//builder.Services.AddQuartzServer(options =>
//{
//    // when shutting down we want jobs to complete gracefully
//    options.WaitForJobsToComplete = true;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
