using Microsoft.EntityFrameworkCore;
using ABTesting.Data;
using MediatR;
using AutoMapper;
using ABTesting.Repositories;
using ABTesting.Services;
using Serilog;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ABTestingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redisConnectionString))
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
        ConnectionMultiplexer.Connect(redisConnectionString));
}

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IExperimentRepository, ExperimentRepository>();
builder.Services.AddScoped<IExperimentService, ExperimentService>();
builder.Services.AddScoped<UnitOfWork>();

// Kafka producer
var kafkaBootstrapServers = builder.Configuration["Kafka:BootstrapServers"];
if (!string.IsNullOrEmpty(kafkaBootstrapServers))
{
    builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
    {
        var config = new ProducerConfig { BootstrapServers = kafkaBootstrapServers };
        return new ProducerBuilder<Null, string>(config).Build();
    });
}

// Quartz for scheduling
builder.Services.AddQuartz(q => q.UseMicrosoftDependencyInjectionJobFactory());
builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

// Background service for Kafka
builder.Services.AddHostedService<KafkaBackgroundService>();

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Auto migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ABTestingDbContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database");
    }
}

app.UseSerilogRequestLogging();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public class KafkaBackgroundService : BackgroundService
{
    private readonly IProducer<Null, string>? _producer;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<KafkaBackgroundService> _logger;

    public KafkaBackgroundService(IServiceProvider serviceProvider, ILogger<KafkaBackgroundService> logger, IProducer<Null, string>? producer = null)
    {
        _producer = producer;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // Producer example
            if (_producer != null)
            {
                await _producer.ProduceAsync("abtesting-events", new Message<Null, string> { Value = "Experiment event" });
            }

            // Consumer example
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "kafka:9092",
                GroupId = "abtesting-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            
            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe("usersegmentation-events");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(TimeSpan.FromSeconds(1));
                    if (cr != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var experimentService = scope.ServiceProvider.GetRequiredService<IExperimentService>();
                        await Task.Delay(1000, stoppingToken);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consuming message");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Kafka background service");
        }
    }
}
