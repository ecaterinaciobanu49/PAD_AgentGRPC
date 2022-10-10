using AgentGrps.Models;
using AgentGrps.Services.Interfaces;
using Grpc.Core;
using GrpcAgent;

namespace AgentGrps.Services
{
    public class PublisherService : Publisher.PublisherBase
    {
        private readonly IMessageStorageService _messageStorage;
        public PublisherService(IMessageStorageService messagestorageservice)
        {
            _messageStorage = messagestorageservice;

        }
        public override Task<PublishReply> PublishMessage(PublishRequest request, ServerCallContext context)
        {
            Console.WriteLine($"Received : {request.Topic} {request.Content}");

            var message = new Message(request.Topic, request.Content);
            _messageStorage.Add(message);

            return Task.FromResult(new PublishReply()
            {
                IsSuccess = true
            });
        }

    }
}
