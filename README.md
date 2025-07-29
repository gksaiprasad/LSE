This API receives trade notifications from brokers, stores the trade details. , and provides endpoints to retrieve average stock prices based on ticker symbols.

Features
Receive real-time trade data.
Compute average stock prices.
Retrieve average prices for specific tickers or all available tickers.
Auto-generated mock data using Bogus for testing.
Middleware-based request logging to .txt file.
Swagger support enabled.

Tech Stack
ASP.NET Core 8 Web API (Minimal hosting model)
Entity Framework Core
SQL Server
Bogus (for generating fake data)
Swagger UI
Custom Middleware for logging

| Method | Route             | Description                              |
| ------ | ----------------- | ---------------------------------------- |
| POST   | `/submit`         | Submit a new trade                       |
| GET    | `/stock/{ticker}` | Get average price for a specific stock   |
| GET    | `/stocks/all`     | Get average prices for all stocks        |
| POST   | `/stocks/range`   | Get average prices for a list of tickers |


Recommendations for Upgrade:
1)we can use client credential flow  request authentication we can use okta  or Azure ntra Id.
2)Since its logging in the text file , We can use AppInsights for logging. 
3)TradeService we can move it to Microservice. We can create multiple instances. 
4)DB connection we can use connection pooling.
5)For a few api we can use Fusion cache.
6)Enable EF Core batching and pooling.
7)Use asynchronous I/O operations and ConfigureAwait(false) in services.
8)Enable horizontal scaling via Docker.


Note : We need to create a .txt file for logging : path "/Logs/request_response_log.txt"

