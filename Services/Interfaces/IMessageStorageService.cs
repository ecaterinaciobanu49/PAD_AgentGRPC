using AgentGrps.Models;

namespace AgentGrps.Services.Interfaces
{
    public interface IMessageStorageService
    {
        void Add(Message message);
        Message GetNext();
        bool IsEmpty();
    }
}
