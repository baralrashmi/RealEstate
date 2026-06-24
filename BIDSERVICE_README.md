# BidService & RabbitMQ Integration Guide

## Overview
This document describes the BidService project added to the RealEstate solution with RabbitMQ integration for subscribing to auction data.

## Architecture

### Components
1. **AuctionService** - Publishes auction creation events to RabbitMQ
2. **BidService** - Consumes auction events and manages bids
3. **RabbitMQ** - Message broker for event distribution
4. **SQL Server** - Shared database for all services

### Message Flow
```
AuctionService (Create Auction) 
    → Publishes: "auction.created" event 
    → RabbitMQ Topic Exchange: "auction-events"
    → BidService Consumer (AuctionEventConsumer)
    → BidDbContext (stores bid data)
```

## Project Structure

### BidService Directory
```
BidService/
├── Controllers/
│   └── BidsController.cs         # REST API endpoints for bids
├── Data/
│   └── BidDbContext.cs           # Entity Framework DbContext
├── DTOs/
│   └── BidDTO.cs                 # Data transfer objects
├── Model/
│   ├── Bid.cs                    # Bid entity
│   └── AuctionMessage.cs         # Message contract from RabbitMQ
├── Repositories/
│   └── BidRepository.cs          # Data access layer
├── Services/
│   └── AuctionEventConsumer.cs   # RabbitMQ consumer background service
├── Properties/
│   └── AssemblyInfo.cs
├── Migrations/                   # EF Core migrations (auto-generated)
├── Program.cs                    # Dependency injection & config
├── BidService.csproj             # NuGet dependencies
├── Dockerfile                    # Docker image definition
├── appsettings.json             # Configuration
└── BidService.http              # HTTP test requests

Key Files Added to Other Projects:
├── AuctionService/
│   ├── Services/AuctionEventPublisher.cs  # RabbitMQ publisher
│   └── Controllers/AuctionsController.cs  # Updated to publish events
└── Docker-Compose.yml            # Added RabbitMQ service
```

## Key Features

### 1. Bid Management
- **Create Bid**: POST `/api/bids`
- **Get Bids by Auction**: GET `/api/bids/auction/{auctionId}`
- **Get Highest Bid**: GET `/api/bids/auction/{auctionId}/highest`
- **Get Bid by ID**: GET `/api/bids/{id}`

### 2. RabbitMQ Integration
- **Exchange**: `auction-events` (Topic)
- **Queue**: `bid-service-auction-created`
- **Routing Key**: `auction.created`
- **Consumer**: `AuctionEventConsumer` (background service)

### 3. Database Schema
```sql
Bids Table:
- Id (Guid) - Primary Key
- AuctionId (Guid) - Foreign reference to auction
- BidderName (string, max 255)
- BidAmount (decimal)
- PlacedAt (DateTime)
- IsWinning (bool)

Indexes:
- AuctionId
- (AuctionId, PlacedAt)
```

## Configuration

### Environment Variables
```yaml
ASPNETCORE_ENVIRONMENT: Development
ASPNETCORE_URLS: http://+:80
ConnectionStrings__DefaultConnection: Server=sqlserver;Database=RealEstate;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;
RABBITMQ_HOST: rabbitmq
RABBITMQ_PORT: 5672
```

### Docker Compose Services
```yaml
rabbitmq:
  - Image: rabbitmq:4.0.5-management-alpine
  - Port: 5672 (AMQP), 15672 (Management UI)
  - Credentials: guest/guest
  
bidservice:
  - Port: 5002:80
  - Depends on: sqlserver, rabbitmq
```

## Running the Application

### Using Docker Compose
```bash
# Build and start all services
docker-compose up -d

# View logs
docker-compose logs -f bidservice

# Stop services
docker-compose down
```

### Local Development
```bash
# 1. Ensure SQL Server and RabbitMQ are running
# 2. Update appsettings.json with correct connection strings
# 3. Run migrations
dotnet ef database update

# 4. Start the service
dotnet run
```

## API Examples

### Create Auction (AuctionService)
```http
POST http://localhost:5000/api/auctions
Content-Type: application/json

{
  "seller": "john@example.com",
  "reservePrice": 100000,
  "auctionEnd": "2026-12-31T23:59:59Z",
  "propertyId": "00000000-0000-0000-0000-000000000001"
}
```

### Create Bid (BidService)
```http
POST http://localhost:5002/api/bids
Content-Type: application/json

{
  "auctionId": "00000000-0000-0000-0000-000000000001",
  "bidderName": "jane@example.com",
  "bidAmount": 120000
}
```

### Get Bids for Auction
```http
GET http://localhost:5002/api/bids/auction/00000000-0000-0000-0000-000000000001
```

### Get Highest Bid
```http
GET http://localhost:5002/api/bids/auction/00000000-0000-0000-0000-000000000001/highest
```

## RabbitMQ Management Console
Access RabbitMQ Management UI at: http://localhost:15672
- Username: guest
- Password: guest

## Development Workflow

### 1. Modify Models
- Update `Bid.cs` or `AuctionMessage.cs`
- Run: `dotnet ef migrations add {MigrationName}`
- Run: `dotnet ef database update`

### 2. Add New Endpoints
- Add methods to `BidsController.cs`
- Add repository methods to `IBidRepository`

### 3. Handle New Event Types
- Add routing key binding in `AuctionEventConsumer.cs`
- Add message handling logic

### 4. Test with HTTP Client
- Use `BidService.http` file in VS Code with REST Client extension
- Or use the provided `.http` files in each service

## Troubleshooting

### RabbitMQ Connection Issues
- Check `RABBITMQ_HOST` and `RABBITMQ_PORT` environment variables
- Verify RabbitMQ container is running: `docker ps | grep rabbitmq`
- Check RabbitMQ logs: `docker logs rabbitmq`

### Database Connection Issues
- Verify SQL Server is running and accessible
- Check connection string in appsettings.json
- Run migrations: `dotnet ef database update`

### Messages Not Being Consumed
- Check consumer logs in BidService
- Verify queue bindings in RabbitMQ Management console
- Ensure `AuctionEventConsumer` is registered in Program.cs

## Security Considerations

### Production Deployment
- Use strong RabbitMQ credentials (not guest/guest)
- Enable SSL/TLS for RabbitMQ connection
- Use managed SQL Server (Azure SQL)
- Implement API authentication (JWT, OAuth)
- Use secrets management (Azure Key Vault)

### Current Implementation (Development)
- Default RabbitMQ credentials: guest/guest
- No API authentication
- Connection strings in appsettings.json (not suitable for production)

## Future Enhancements

1. **Bid Validation**: Implement business logic to validate bids
2. **Auction End Event**: Handle auction end and determine winner
3. **Bid History**: Track bid modifications
4. **Event Sourcing**: Store all events for audit trail
5. **API Authentication**: Add JWT or OAuth2
6. **Monitoring**: Add Application Insights for telemetry
7. **Resilience**: Implement retry policies and circuit breakers
8. **Caching**: Add Redis for performance optimization

## Dependencies

### Key NuGet Packages
- RabbitMQ.Client (6.8.1) - AMQP client library
- Microsoft.EntityFrameworkCore (9.0.16) - ORM
- Microsoft.EntityFrameworkCore.SqlServer (9.0.16) - SQL Server provider
- Swashbuckle.AspNetCore (7.0.0) - Swagger/OpenAPI

## Files Modified/Created

### New Files
- `BidService/*` - Complete BidService project
- `AuctionService/Services/AuctionEventPublisher.cs` - RabbitMQ publisher
- `Docker-Compose.yml` - Updated with RabbitMQ service

### Modified Files
- `AuctionService/AuctionService.csproj` - Added RabbitMQ.Client package
- `AuctionService/Program.cs` - Registered IAuctionEventPublisher
- `AuctionService/Controllers/AuctionsController.cs` - Publish events on auction creation
- `RealEstate.slnx` - Added BidService project

## Contact & Support
For issues or questions, refer to the service-specific documentation or check the logs for diagnostic information.
