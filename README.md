# Crypto Rates Coding Assignment


**Problem Statement**

An application that accepts a cryptocurrency code as input. The application should then display its current quote in the following currencies:

 - USD 
 - EUR 
 - BRL 
 - GBP 
 - AUD
---

**The Acceptance criteria**

For the known code BTC, results are returned Values for USD, EUR, BRL, GBP, and AUD are shown

---
**Tools required to run**

 1. Visual Studio 2022 OR VS Code + CSharp Extension [Download Here](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
 2. .NET SDK 6

**Steps To Run**

 1. Clone code locally
 2. Go to KnabFX.UI folder
 3. Replace **<ExchangeRates_APIKey>** and **<CryptoRates_LiveAPIKey>** on file ***appsettings.Production.json*** with API keys sent on email.
 4. Run `dotnet run --launch-profile="KnabFX_Live"` 
 5. Go to [Here](https://localhost:7169/) then go to Rates page and search with Crypto symbol (ex. BTC, ETH)

![image](https://user-images.githubusercontent.com/4711491/177808790-fe6462c3-3d6a-4b6a-aa9d-1322834c6eff.png)

---



**To Run Unit Tests**

 1. Go to ***KnabFX.Infrastructure.Tests*** folder 
 2. Run `dotnet test` and you should see unit tests running
 
 ![image](https://user-images.githubusercontent.com/4711491/177809780-10264a08-3b0d-4149-b6cb-debc9a66c9f7.png)


 
