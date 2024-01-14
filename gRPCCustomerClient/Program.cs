// See https://aka.ms/new-console-template for more information


using System.Threading.Tasks;
using Grpc.Net.Client;
using CustomerClient;
using gRPCCUstomerClient;
using Grpc.Core;
//The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("http://localhost:5065");
var client = new GrpcCustomer.GrpcCustomerClient(channel);

// Call GetAll method
try
{
    var allCustomersResponse = await client.GetAllAsync(new Empty());
    Console.WriteLine("All Customers:");
    foreach (var customer in allCustomersResponse.Customers)
    {
        Console.WriteLine($"Customer ID: {customer.Id}, Name: {customer.Name}, Address: {customer.Address}");
    }
}
catch (RpcException ex)
{
    Console.WriteLine($"Error calling GetAll: {ex.Status}");
}

// Call GetCustomer method
var customerIdRequest = new IDRequest { Id = "C001" }; // Provide the desired customer ID
try
{
    var singleCustomerResponse = await client.GetCustomerAsync(customerIdRequest);
    Console.WriteLine($"Single Customer - ID: {singleCustomerResponse.Id}, Name: {singleCustomerResponse.Name}");
}
catch (RpcException ex)
{
    Console.WriteLine($"Error calling GetCustomer: {ex.Status}");
}

// Close the channel
await channel.ShutdownAsync();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();