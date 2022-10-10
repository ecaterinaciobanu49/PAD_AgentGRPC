using Grpc.Net.Client;

namespace AgentGrps.Models
{
    public class Connection
    {
        public Connection(string address, string topic)
        {
            Address = address;
            Topic = topic;
            Channel = GrpcChannel.ForAddress(address);
        }
        public string Address { get; }
        public string Topic { get; }
        public GrpcChannel Channel {get;}
    }
}
