// See https://aka.ms/new-console-template for more information

using Grpc.Net.Client;

using TestGrpcNetClient;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Greeter.GreeterClient(channel);
var res = await client.SayHelloAsync(new HelloRequest
{
    Name = "哈哈哈1"
});
Console.WriteLine(res.Message);
Console.ReadKey();
