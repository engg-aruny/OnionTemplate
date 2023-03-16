using OnionDemo.Domain.Entities;
using OnionDemo.Domain.Repositories;

namespace OnionDemo.Services
{
    public sealed class PingPongService : IPingPongService
    {
        private readonly IRepositoryManager _repositoryManager;
        public PingPongService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

        public async Task<IEnumerable<PingPong>> GetAllPingPongAsync(CancellationToken cancellationToken)
        {
            var pongPongs = await _repositoryManager.PingPongRepository.GetAllPingPongAsync(cancellationToken);
            return pongPongs;
        }
    }

    //We can seperate it out in Services.Abstractions
    public interface IPingPongService
    {
        Task<IEnumerable<PingPong>> GetAllPingPongAsync(CancellationToken cancellationToken);
    }
}
