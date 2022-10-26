using static TestGrpcNetCoreClient.Greeter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpcClient<GreeterClient>(o =>
{
    o.Address = new Uri("https://localhost:5001/");
})
.ConfigureChannel(configureChannel =>
{
    //����ͻ���ʹ��http���ӵĻ� �������ÿͻ��˲���ȫ����ѡ��=true
    configureChannel.UnsafeUseInsecureChannelCallCredentials = true;
})
.AddCallCredentials((context, metadata, serviceProvider) =>
{
    //����������ͳһ����֤��Ϣ������token�ȵȡ�����
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var token = httpContextAccessor.HttpContext?.Request.Headers["Cookie"];
    if (!string.IsNullOrEmpty(token))
    {
        metadata.Add("Authorization", $"Bearer {token}");
    }
    return Task.CompletedTask;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
