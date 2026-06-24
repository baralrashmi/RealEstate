# BidService & RabbitMQ - Quick Start Guide

## 30-Second Overview

✅ **BidService** added to RealEstate solution  
✅ **RabbitMQ** integration for auction events  
✅ **Docker Compose** configured for all services  
✅ **REST API** endpoints for bid management  

## Get Started in 5 Minutes

### 1. Start All Services
```bash
cd /Users/rashmibaral/Documents/GitHub/RealEstate.worktrees/copilot-add-bidservice-rabbitmq-integration
docker-compose up -d
```

### 2. Access Services
| Service | URL | Purpose |
|---------|-----|---------|
| AuctionService | http://localhost:5000 | Create auctions |
| BidService | http://localhost:5002 | Manage bids |
| RabbitMQ UI | http://localhost:15672 | Monitor messages |
| SQL Server | localhost:1435 | Database |

### 3. Create Test Data

**Step 1: Create an Auction** (in AuctionService Swagger)
```json
POST /api/auctions
{
  "seller": "test@example.com",
  "reservePrice": 100000,
  "auctionEnd": "2026-12-31T23:59:59Z",
  "propertyId": "00000000-0000-0000-0000-000000000001"
}
```
Copy the returned `id` (AuctionId)

**Step 2: Create a Bid** (in BidService Swagger)
```json
POST /api/bids
{
  "auctionId": "00000000-0000-0000-0000-000000000001",
  "bidderName": "bidder@example.com",
  "bidAmount": 120000
}
```

**Step 3: View Bids**
```
GET /api/bids/auction/00000000-0000-0000-0000-000000000001
```

## Key Endpoints

### AuctionService (Port 5000)
```
POST   /api/auctions                    → Create auction
GET    /api/auctions                    → List auctions
GET    /api/auctions/{id}               → Get auction details
```

### BidService (Port 5002)
```
POST   /api/bids                        → Create bid
GET    /api/bids/{id}                   → Get bid
GET    /api/bids/auction/{auctionId}    → Get all bids for auction
GET    /api/bids/auction/{auctionId}/highest → Get highest bid
```

## Monitor Message Flow

### RabbitMQ Management UI
1. Open http://localhost:15672
2. Login: guest / guest
3. Go to **Exchanges** → `auction-events`
4. Go to **Queues** → `bid-service-auction-created`

**What to expect:**
- When you create an auction, message count increases
- BidService consumes the message (count goes back to 0)

## Common Commands

### View Service Logs
```bash
docker logs -f bidservice          # BidService logs
docker logs -f auctionservice      # AuctionService logs
docker logs -f rabbitmq            # RabbitMQ logs
```

### Stop Services
```bash
docker-compose down                # Stop all services
docker-compose down -v             # Stop and remove volumes
```

### Restart Services
```bash
docker-compose restart bidservice  # Restart specific service
docker-compose up -d               # Restart all
```

### Database Access
```bash
# Connect to SQL Server
sqlcmd -S localhost,1435 -U sa -P YourStrong@Passw0rd

# Then in sqlcmd:
USE RealEstate
SELECT * FROM Bids
SELECT COUNT(*) FROM Bids WHERE AuctionId = 'your-auction-id'
GO
```

## Troubleshooting

### "Connection refused" when accessing services
**Solution**: Wait 30 seconds for containers to start, then try again
```bash
docker-compose logs bidservice  # Check logs for errors
```

### No messages in RabbitMQ queue
**Possible causes:**
1. Auction wasn't created successfully
2. BidService consumer already consumed the message
3. Check logs: `docker logs -f bidservice`

### Database connection failed
**Solution**: Restart SQL Server
```bash
docker-compose restart sqlserver
# Wait 30 seconds, then try again
```

## Architecture in Plain English

```
1. You create an Auction → AuctionService saves it
2. AuctionService publishes event → RabbitMQ receives it
3. BidService listens → Gets the auction data
4. You create bids → BidService saves them
5. You query bids → BidService returns the data
```

## File Structure
```
BidService/
├── Controllers/BidsController.cs      ← API endpoints
├── Data/BidDbContext.cs               ← Database mapping
├── Services/AuctionEventConsumer.cs   ← Listens to RabbitMQ
├── Repositories/BidRepository.cs      ← Data access
├── Model/                             ← Domain models
└── Program.cs                         ← Configuration

Key modification:
├── AuctionService/Services/AuctionEventPublisher.cs  ← Sends to RabbitMQ
```

## Environment Variables
```yaml
RABBITMQ_HOST: rabbitmq          # In Docker: "rabbitmq"
RABBITMQ_PORT: 5672             # Default AMQP port
ASPNETCORE_URLS: http://+:80     # API listens on port 80
```

## Next Steps

1. **Read full documentation**: See `BIDSERVICE_README.md`
2. **Review deployment checklist**: See `DEPLOYMENT_CHECKLIST.md`
3. **Run smoke tests**: Create auction → create bid → get bids
4. **Check logs**: Monitor for any errors or warnings

## Quick Help

| Question | Answer |
|----------|--------|
| How do I stop the services? | `docker-compose down` |
| How do I see what's happening? | Check logs: `docker logs -f bidservice` |
| Where is the database? | `localhost:1435` (SA/YourStrong@Passw0rd) |
| Is it production ready? | No - add authentication and validation first |
| Can I test locally without Docker? | Yes - need SQL Server and RabbitMQ running separately |
| How do I add more fields to Bid? | Edit Model/Bid.cs, run EF migration, update DTO |

---

**Ready to go!** 🚀

For detailed documentation, see `BIDSERVICE_README.md`
