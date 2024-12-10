using Api.Infrastructure.Data;
using Api.Configuration;
using Microsoft.EntityFrameworkCore;
using Amazon.SQS;
using Microsoft.Extensions.Options;
using Api.Infrastructure.Queue;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<ActivityContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
    npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
    }));

builder.Services.Configure<AwsOptions>(builder.Configuration.GetSection(AwsOptions.Section));
builder.Services.Configure<QueueOptions>(builder.Configuration.GetSection(QueueOptions.Section));

builder.Services.AddScoped<IQueueService, SqsQueueService>();
builder.Services.AddHostedService(sp =>
{
    var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
    return new QueueProcessorService(scopeFactory);
});

// AWS Client Registration
builder.Services.AddSingleton<IAmazonSQS>(sp =>
{
    var awsOptions = sp.GetRequiredService<IOptions<AwsOptions>>().Value;

    var config = new AmazonSQSConfig
    {
        ServiceURL = awsOptions.ServiceUrl,
        UseHttp = true
    };

    return new AmazonSQSClient(
        awsOptions.AccessKey,
        awsOptions.SecretKey,
        config
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();