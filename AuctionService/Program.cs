using AuctionService.Data;
using AuctionService.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();

builder.Services.AddControllers();

// Add Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Auction Service API",
        Description = "API for managing real estate auctions"
    });
});

builder.Services.AddDbContext<RealEstateContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


//Configure Add MassTransit and RabbitMQ
builder.Services.AddMassTransit(

    busConfig =>
    {
        //Add MassTransit configuration for RabbitMQ , Add Tables in database for MassTransit
        //This helps to store the messages in the database and ensures that messages are not lost in case of any failure.
        //It also provides a way to track the status of messages and retry them if necessary.
        busConfig.AddEntityFrameworkOutbox<RealEstateContext>(outboxConfig =>
        {
            // this is for retrying

            outboxConfig.QueryDelay = TimeSpan.FromSeconds(10);
            outboxConfig.UseSqlServer();
            outboxConfig.UseBusOutbox();
        });

        busConfig.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction-service", false));
        busConfig.UsingRabbitMq((Contex, config) =>
        {
            
            //cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
            config.ConfigureEndpoints(Contex);
        });
    }
);




//app is built after all the services are added to the container,
//and then the HTTP request pipeline is configured before running the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auction Service API v1");
        options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DbInitializer.InitializeData(app);

app.Run();
