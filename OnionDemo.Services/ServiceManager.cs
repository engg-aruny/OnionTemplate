using OnionDemo.Domain.Services.Abstractions;

namespace OnionDemo.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly IPingPongService _pingPongService;

        public ServiceManager(IPingPongService pingPongService)
        {
            _pingPongService = pingPongService;
        }

        public IPingPongService PingPongService => _pingPongService;
    }
}
