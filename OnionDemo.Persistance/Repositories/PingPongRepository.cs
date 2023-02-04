using Microsoft.EntityFrameworkCore;
using OnionDemo.Domain.Entities;
using OnionDemo.Domain.Repositories;

namespace OnionDemo.Persistance.Repositories
{
    public sealed class PingPongRepository: IPingPongRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public PingPongRepository(RepositoryDbContext dbContext)
        {
            _dbContext= dbContext;
        }

        public async Task<IEnumerable<PingPong>> GetAllPingPongAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.PingPong.ToListAsync(cancellationToken);
        }
    }
}
