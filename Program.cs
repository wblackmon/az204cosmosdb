// See https://aka.ms/new-console-template for more information
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
Console.WriteLine("Hello, Cosmos DB!");

// Azure CLI Setup
/*
az group create --location centralus --name az204-cosmos-rg

az cosmosdb create --name az204-cosmos-db --resource-group az204-cosmos-rg
*/

// C# code Connection String and Key for cosmos db
// Use ConfigurationBuilder to read the connection string from the appsettings.json file
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var connectionString = configuration["CosmosDb:URI"];
Console.WriteLine(connectionString);
var key = configuration["CosmosDB:Key"];
Console.WriteLine(key);

// Create a new CosmosClient
var cosmosClient = new CosmosClient(connectionString, key);

// Create database and container
Database database;
Container container;

try
{
    Console.WriteLine("Beginning operations...\n");
    Program p = new Program();

    database = await cosmosClient.CreateDatabaseIfNotExistsAsync("az204-cosmos-db");
    Console.WriteLine($"Created database: {database.Id}");

    container = await database.CreateContainerIfNotExistsAsync("az204-container", "/id");
    Console.WriteLine($"Created container: {container.Id}");

}
catch (CosmosException ce)
{
    Exception baseException = ce.GetBaseException();
    Console.WriteLine($"{ce.StatusCode} error occurred: {ce.Message}, Message: {baseException.Message}");
}
catch(Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    Console.WriteLine("End of operations");
    Console.ReadKey();
}






