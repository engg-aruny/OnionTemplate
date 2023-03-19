
using OnionDemo.Domain.Entities;
using OnionDemo.Domain.Repositories;
using OnionDemo.Domain.Services.Abstractions;

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
}
