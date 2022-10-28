// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Grpc.Net.Client;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using TestGrpcNetClient;

var options = new JsonSerializerOptions
{
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
};

using var channel = GrpcChannel.ForAddress("https://localhost:7128");
var client = new Greeter.GreeterClient(channel);

#region 一元方法
{
    Console.WriteLine($"=====================================一元方法-BEGIN==============================================");
    var res = await client.SayHelloAsync(new HelloRequest
    {
        Name = "哈哈哈1"
    });
    Console.WriteLine($"这是一元方法：{JsonSerializer.Serialize(res, options)}");
    Console.WriteLine($"=====================================一元方法-END==============================================");
    Console.WriteLine();
}
#endregion

#region 服务器向客户端推流
{
    Console.WriteLine($"=====================================服务器向客户端推流-BEGIN==============================================");
    using var call = client.StreamingFromServer(new ExampleRequest { PageIndex = 1, PageSize = 3, IsDescending = true });

    while (await call.ResponseStream.MoveNext())
    {
        Console.WriteLine("接收的服务器流: " + JsonSerializer.Serialize(call.ResponseStream.Current, options));
    }
    Console.WriteLine($"=====================================服务器向客户端推流-END==============================================");
    Console.WriteLine();
}
#endregion

#region 客户端向服务器推流
{
    Console.WriteLine($"=====================================客户端向服务器推流-BEGIN==============================================");
    using var clientStream = client.StreamingFromClient();

    for (var i = 0; i < 5; i++)
    {
        await clientStream.RequestStream.WriteAsync(new ExampleRequest { PageIndex = i, PageSize = i, IsDescending = true });

    }
    await clientStream.RequestStream.CompleteAsync();

    var response = await clientStream;
    Console.WriteLine($"服务器返回: {response.Name}");
    Console.WriteLine($"=====================================客户端向服务器推流-END==============================================");
    Console.WriteLine();
}
#endregion

#region 双向流
{
    Console.WriteLine($"=====================================双向流-BEGIN==============================================");
    using var bothStream = client.StreamingBothWays();

    //向服务器推流
    for (var i = 0; i < 3; i++)
    {
        await bothStream.RequestStream.WriteAsync(new ExampleRequest { PageIndex = i + 100, PageSize = i + 100, IsDescending = true });
    }
    await bothStream.RequestStream.CompleteAsync();
    //接收服务器流
    while (await bothStream.ResponseStream.MoveNext())
    {
        Console.WriteLine("接收的服务器流: " + JsonSerializer.Serialize(bothStream.ResponseStream.Current, options));
    }
    Console.WriteLine($"=====================================双向流-END==============================================");
}
#endregion

Console.ReadKey();
