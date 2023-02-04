using OnionDemo.Domain.Repositories;

namespace OnionDemo.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IPingPongService> _lazyPingPongService;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _lazyPingPongService = new Lazy<IPingPongService>(() => new PingPongService(repositoryManager));
        }

        public IPingPongService PingPongService => _lazyPingPongService.Value;
    }

    public interface IServiceManager
    {
        IPingPongService PingPongService { get; }
    }
}
