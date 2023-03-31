using OnionDemo.Domain.Entities;

namespace OnionDemo.Domain.Repositories
{
    public interface IPingPongRepository
    {
        Task<IEnumerable<PingPong>> GetAllPingPongAsync(CancellationToken cancellation);

        Task Insert(CancellationToken cancellationToken);
    }
}
