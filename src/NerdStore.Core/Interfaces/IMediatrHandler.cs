using NerdStore.Core.Messages;
using System.Threading.Tasks;

namespace NerdStore.Core.Interfaces
{
    public interface IMediatrHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
    }
}
