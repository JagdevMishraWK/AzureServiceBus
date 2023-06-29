using ASBQueueReceiver.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Background Service Dependenacy Injection
builder.Services.AddHostedService<BackgroundServiceHandler>();

builder.Services.AddSingleton<IQueueReceiverService, QueueReceiverService>();


builder.Services.AddAzureClients(clientsBuilder =>
{
    string connectionString = builder.Configuration.GetSection("ServiceBus").GetValue<string>("ConnectionString");
    clientsBuilder.AddServiceBusClient(connectionString)
      .ConfigureOptions(options =>
      {
          options.RetryOptions.Delay = TimeSpan.FromMilliseconds(50);
          options.RetryOptions.MaxDelay = TimeSpan.FromSeconds(5);
          options.RetryOptions.MaxRetries = 3;
      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();