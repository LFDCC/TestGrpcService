using Grpc.Core;

using Microsoft.Extensions.Options;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace TestGrpcNetCoreService.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 一元方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello Asp Net Core ：" + request.Name
            });
        }

        /// <summary>
        /// 返回空对象
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<Empty> EmptyHello(EmptyHelloRequest request, ServerCallContext context)
        {
            foreach (var item in request.Peoples)
            {

            }
            return Task.FromResult(new Empty());
        }

        /// <summary>
        /// 服务器流式处理方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task StreamingFromServer(ExampleRequest request, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("服务器流开始响应。。。。");
            for (int i = 0; i < 5; i++)
            {
                await responseStream.WriteAsync(new ExampleResponse()
                {
                    Name = $"qq{i}",
                    Sex = $"woman{i}"
                });
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            Console.WriteLine("服务器流结束响应。。。。");
        }

        /// <summary>
        /// 客户端流式处理方法
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ExampleResponse> StreamingFromClient(IAsyncStreamReader<ExampleRequest> requestStream, ServerCallContext context)
        {
            Console.WriteLine("服务器开始响应客户端流。。。。");
            while (await requestStream.MoveNext())
            {
                Console.WriteLine("接收的客户端流: " + JsonSerializer.Serialize(requestStream.Current, options));
            }
            return new ExampleResponse
            {
                Name = "客户端流结束"
            };
        }

        /// <summary>
        /// 双向流式处理方法
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task StreamingBothWays(IAsyncStreamReader<ExampleRequest> requestStream, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("双向流开始响应。。。。");
            while (await requestStream.MoveNext())
            {
                Console.WriteLine("接收的客户端流: " + JsonSerializer.Serialize(requestStream.Current, options));
                await responseStream.WriteAsync(new ExampleResponse()
                {
                    Name = JsonSerializer.Serialize(requestStream.Current, options)
                });
            }
            Console.WriteLine("双向流结束响应。。。。");
        }
    }
}