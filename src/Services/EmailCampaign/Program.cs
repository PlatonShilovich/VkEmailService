using Microsoft.EntityFrameworkCore;
using EmailCampaign.Data;
using MediatR;
using AutoMapper;
using EmailCampaign.Repositories;
using EmailCampaign.Services;
using Serilog;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Quartz;
using HandlebarsDotNet;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmailCampaignDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redisConnectionString))
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
        ConnectionMultiplexer.Connect(redisConnectionString));
}

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
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

// Quartz для планирования кампаний - ИСПРАВЛЕННЫЙ СИНТАКСИС
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    
    // Создание Job
    var jobKey = new JobKey("SendCampaignJob", "EmailGroup");
    q.AddJob<SendCampaignJob>(opts => opts.WithIdentity(jobKey));
    
    // Создание Trigger
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("SendCampaignTrigger", "EmailGroup")
        .WithSimpleSchedule(s => s
            .WithIntervalInMinutes(5)
            .RepeatForever()));
});

builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

// Register Handlebars
builder.Services.AddSingleton<IHandlebars>(Handlebars.Create());

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
    var dbContext = scope.ServiceProvider.GetRequiredService<EmailCampaignDbContext>();
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

public class SendCampaignJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        // Логика отправки кампаний по расписанию
        await Task.CompletedTask;
    }
}

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
            if (_producer != null)
            {
                await _producer.ProduceAsync("email-events", new Message<Null, string> { Value = "Campaign sent" });
            }

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "kafka:9092",
                GroupId = "emailcampaign-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            
            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe("abtesting-events");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(TimeSpan.FromSeconds(1));
                    if (cr != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var campaignService = scope.ServiceProvider.GetRequiredService<ICampaignService>();
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
