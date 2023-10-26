using SmartMass.Controller.Mqtt;
using SmartMass.Controller.Web.Data;

namespace SmartMass.Controller.Web.Services
{
    public class MqttService : IHostedService
    {
        private readonly ILogger<MqttService> logger;
        private readonly MqttClient mqttClient;
        private readonly SmartMassDbContext dbContext;
        private readonly IConfigurationSection mqttConfig;
        private readonly IServiceProvider services;

        public MqttService(IConfiguration config, ILogger<MqttService> logger, IServiceProvider services, IMqttClient mqttClient)
        {
            this.logger = logger;
            this.services = services;
            this.mqttConfig = config.GetSection("mqtt");
            this.mqttClient = (MqttClient)mqttClient; //TODO: i feel bad for the hard cast
            this.mqttClient.OnMessageReceived += OnMessageReceived;
            this.mqttClient.OnClientDisconnected += OnClientDisconnected;
            this.mqttClient.OnClientConnected += OnClientConnected;
        }
        public async Task Listen(string topic)
        {
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.mqttClient.Connect(this.mqttConfig.GetValue<string>("host"), this.mqttConfig.GetValue<string>("clientid"),
                this.mqttConfig.GetValue<string>("user"), this.mqttConfig.GetValue<string>("password"));
            this.logger.LogInformation("Connected");
            await mqttClient.Subscribe($"{this.mqttConfig.GetValue<string>("topic")}/#");
            this.logger.LogInformation("subscribed");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await this.mqttClient.Disconnect();
            this.logger.LogInformation("Disconnected");
        }

        private void OnClientConnected(object sender, EventArgs e)
        {
            this.logger.LogInformation("onconnect");
        }

        private void OnClientDisconnected(object sender, EventArgs e)
        {
            this.logger.LogInformation("ondisconnect");
        }

        private void OnMessageReceived(object sender, MqttMessageReceivedEventArgs e)
        {
            this.logger.LogInformation($"message received from {e.ClientId} on topic {e.Topic}. Payload: {e.Payload}");

            //using var scope = this.services.CreateAsyncScope();
            //var scopedDbContext =  scope.ServiceProvider.GetRequiredService<SmartMassDbContext>(); 
            //dbContext.MqttValues.AddAsync()
            //this, or DBContextFactory (https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#using-a-dbcontext-factory-eg-for-blazor)
            //hopefully this works better, but might run into concurrency issues

        }



    }
}
