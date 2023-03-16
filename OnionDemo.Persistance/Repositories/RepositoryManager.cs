using OnionDemo.Domain.Repositories;

namespace OnionDemo.Persistance.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly IPingPongRepository _pingPongRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RepositoryManager(IPingPongRepository pingPongRepository, IUnitOfWork unitOfWork)
        {
            _pingPongRepository = pingPongRepository;
            _unitOfWork = unitOfWork;
        }

        public IPingPongRepository PingPongRepository => _pingPongRepository;

        public IUnitOfWork UnitOfWork => _unitOfWork;
    }
}
