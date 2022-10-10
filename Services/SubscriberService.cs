using AgentGrps.Models;
using AgentGrps.Services.Interfaces;
using Grpc.Core;
using GrpcAgent;

namespace AgentGrps.Services
{
    public class SubscriberService : Subscriber.SubscriberBase
    {
        private readonly IConnectionStorageService _connectionStorageService;
        public SubscriberService(IConnectionStorageService connectionStorage)
        {
            _connectionStorageService = connectionStorage;
        }
        public override Task<SubscribeReply> Subscribe(SubscribeRequest request, ServerCallContext context)
        {
            Console.WriteLine($"New subscriber {request.Address} {request.Topic}");

            try
            {
                var connection = new Connection(request.Address, request.Topic);

                _connectionStorageService.Add(connection);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            

            return Task.FromResult(new SubscribeReply()
                {
                IsSuccess = true
                });
        }
    }
}
