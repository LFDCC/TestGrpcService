using Grpc.Core;

namespace TestGrpcNetCoreService.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello Asp Net Core £º" + request.Name
            });
        }

        public override Task<Empty> EmptyHello(EmptyHelloRequest request, ServerCallContext context)
        {
            foreach (var item in request.Peoples)
            {

            }
            return Task.FromResult(new Empty());
        }
    }
}