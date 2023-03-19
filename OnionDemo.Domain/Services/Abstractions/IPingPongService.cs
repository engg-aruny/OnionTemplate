using OnionDemo.Domain.Entities;

namespace OnionDemo.Domain.Services.Abstractions
{
    public interface IPingPongService
    {
        Task<IEnumerable<PingPong>> GetAllPingPongAsync(CancellationToken cancellationToken);
    }
}
