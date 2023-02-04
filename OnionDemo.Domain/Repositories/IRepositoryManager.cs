namespace OnionDemo.Domain.Repositories
{
    public interface IRepositoryManager
    {
        IPingPongRepository PingPongRepository { get; }

        IUnitOfWork UnitOfWork { get; }
    }
}
