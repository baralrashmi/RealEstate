using Microsoft.EntityFrameworkCore;
using Polly;
using SearchService.Data;
using SearchService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<SearchDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
// Marked as scoped to ensure that the database context is properly disposed after use
//builder.Services.AddScoped<AuctionServiceHttpClient>();

// Register the HttpClient for AuctionServiceHttpClient
builder.Services.AddHttpClient<AuctionServiceHttpClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//await DbInitializer.InitDb(app); 
// This line calls the InitDb method of the DbInitializer class, passing the app instance as a parameter. The InitDb method is responsible for initializing the database, which may include applying migrations and seeding initial data. By calling this method, the application ensures that the database is set up and ready to use before handling any incoming requests.
//await keep in mind that the InitDb method is asynchronous, so it is awaited to ensure that the database initialization process completes before the application starts accepting requests.
//If there is no await keyword, the application may start accepting requests before the database is fully initialized, which could lead to errors if the database is not ready to handle those requests. Therefore, using await ensures that the initialization process is completed before the application is fully operational.
await DbInitializer.InitDb(app);

// .NET application lifetime 
// The .NET application lifetime refers to the various stages that an application goes
//through from the moment it starts until it shuts down. These stages include:
// 1. Application Start: This is the initial stage when the application is launched. During this stage, the application sets up its environment, loads necessary resources, and initializes any required services or components.
// 2. Application Running: After the application has started, it enters the running stage. In this stage, the application is actively processing requests, performing tasks, and providing its intended functionality to users.
// 3. Application Shutdown: When the application is signaled to shut down (e.g., due to a user action, system signal, or an error), it enters the shutdown stage. During this stage, the application performs cleanup tasks, releases resources, and gracefully terminates any ongoing processes before exiting.

// Add Polly nuget package
app.Lifetime.ApplicationStarted.Register(async ()
    => await Policy.Handle<TimeoutException>()
    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(10))
    .ExecuteAndCaptureAsync(async () => await DbInitializer.InitDb(app))

    );


app.Run();
