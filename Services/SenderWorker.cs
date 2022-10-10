using AgentGrps.Services.Interfaces;
using GrpcAgent;

namespace AgentGrps.Services
{
    public class SenderWorker : IHostedService
    {
        private Timer _timer;
        private const int TimeToWait = 2000;
        private readonly IMessageStorageService _messageStorageService;
        private readonly IConnectionStorageService _connectionStorageService;

        public SenderWorker(IServiceScopeFactory serviceScopeFactory)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                _messageStorageService = scope.ServiceProvider.GetRequiredService<IMessageStorageService>();
                _connectionStorageService = scope.ServiceProvider.GetRequiredService<IConnectionStorageService>();
                
            }

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoSendWork, null, 0, TimeToWait);
                return Task.CompletedTask;
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        
        private void DoSendWork(object state)
        {
            while (!_messageStorageService.IsEmpty())
            {
                var message = _messageStorageService.GetNext();

                if(message != null)
                {
                    var connections = _connectionStorageService.GetConnectionsByTopic(message.Topic);

                    foreach (var connection in connections)
                    {
                        var client = new Notifier.NotifierClient(connection.Channel);
                        var reguest = new NotifyRequest () { Content = message.Content };
                        try
                        {
                            var reply = client.Notify(reguest);
                            Console.WriteLine($"Notified subscriber {connection.Address} with {message.Content}. Response : {reply.IsSuccess}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }

        }
    }
}
