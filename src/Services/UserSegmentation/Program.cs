using Microsoft.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;
using UserSegmentation.Data;
using MediatR;
using AutoMapper;
using FluentValidation;
using UserSegmentation.Repositories;
using UserSegmentation.Services;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// PostgreSQL
builder.Services.AddDbContext<UserSegmentationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Repositories and Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISegmentRepository, SegmentRepository>();
builder.Services.AddScoped<ISegmentService, SegmentService>();
builder.Services.AddScoped<UnitOfWork>();

// Kafka producer
builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
{
    var config = new ProducerConfig { BootstrapServers = builder.Configuration["Kafka:BootstrapServers"] };
    return new ProducerBuilder<Null, string>(config).Build();
});

// Background service for Kafka (consumer/producer example)
builder.Services.AddHostedService<KafkaBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Auto migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserSegmentationDbContext>();
    dbContext.Database.Migrate();
}

app.UseSerilogRequestLogging();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public class KafkaBackgroundService : BackgroundService
{
    private readonly IProducer<Null, string> _producer;
    private readonly IServiceProvider _serviceProvider;

    public KafkaBackgroundService(IProducer<Null, string> producer, IServiceProvider serviceProvider)
    {
        _producer = producer;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Producer example
        await _producer.ProduceAsync("usersegmentation-events", new Message<Null, string> { Value = "New segment created" });

        // Consumer example
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "kafka:9092",
            GroupId = "usersegmentation-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        consumer.Subscribe("abtesting-events"); // Listen to events from other services

        while (!stoppingToken.IsCancellationRequested)
        {
            var cr = consumer.Consume(stoppingToken);
            if (cr != null)
            {
                // Process event (e.g., update segment based on AB test event)
                using var scope = _serviceProvider.CreateScope();
                var segmentService = scope.ServiceProvider.GetRequiredService<ISegmentService>();
                // Example logic
                await Task.Delay(1000); // Simulate processing
            }
        }
    }
}
