namespace OnionDemo.Domain.Services.Abstractions
{
    public interface IServiceManager
    {
        IPingPongService PingPongService { get; }
    }
}
