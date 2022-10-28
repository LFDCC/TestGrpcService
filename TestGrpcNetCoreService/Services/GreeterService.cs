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
        /// һԪ����
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello Asp Net Core ��" + request.Name
            });
        }

        /// <summary>
        /// ���ؿն���
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
        /// ��������ʽ������
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task StreamingFromServer(ExampleRequest request, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("����������ʼ��Ӧ��������");
            for (int i = 0; i < 5; i++)
            {
                await responseStream.WriteAsync(new ExampleResponse()
                {
                    Name = $"qq{i}",
                    Sex = $"woman{i}"
                });
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            Console.WriteLine("��������������Ӧ��������");
        }

        /// <summary>
        /// �ͻ�����ʽ������
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ExampleResponse> StreamingFromClient(IAsyncStreamReader<ExampleRequest> requestStream, ServerCallContext context)
        {
            Console.WriteLine("��������ʼ��Ӧ�ͻ�������������");
            while (await requestStream.MoveNext())
            {
                Console.WriteLine("���յĿͻ�����: " + JsonSerializer.Serialize(requestStream.Current, options));
            }
            return new ExampleResponse
            {
                Name = "�ͻ���������"
            };
        }

        /// <summary>
        /// ˫����ʽ������
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task StreamingBothWays(IAsyncStreamReader<ExampleRequest> requestStream, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("˫������ʼ��Ӧ��������");
            while (await requestStream.MoveNext())
            {
                Console.WriteLine("���յĿͻ�����: " + JsonSerializer.Serialize(requestStream.Current, options));
                await responseStream.WriteAsync(new ExampleResponse()
                {
                    Name = JsonSerializer.Serialize(requestStream.Current, options)
                });
            }
            Console.WriteLine("˫����������Ӧ��������");
        }
    }
}