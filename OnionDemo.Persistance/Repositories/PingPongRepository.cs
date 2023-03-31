using Microsoft.EntityFrameworkCore;
using OnionDemo.Domain.Entities;
using OnionDemo.Domain.Repositories;

namespace OnionDemo.Persistance.Repositories
{
    public sealed class PingPongRepository : IPingPongRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public PingPongRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PingPong>> GetAllPingPongAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.PingPong.ToListAsync(cancellationToken);
        }

        //Example - Transaction in entity framework
        public async Task Insert(CancellationToken cancellationToken)
        {
            //Example - the Manufacturer entity that was added earlier will still be persisted in the database, resulting in unused data.
            //var manufacturerEntity = new Manufacturer()
            //{
            //    Name = "XYZ Model",
            //    Country = "India",
            //};
            //await _dbContext.Manufacturers.AddAsync(manufacturerEntity, cancellationToken);
            //await _dbContext.SaveChangesAsync();

            //await _dbContext.PingPong.AddAsync(new PingPong()
            //{
            //    Model = "Testing Default Transaction",
            //    ManufactureDate = DateTime.Now,
            //    ManufacturerId = manufacturerEntity.Id
            //}, cancellationToken);

            //await _dbContext.SaveChangesAsync();

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var manufacturerEntity = new Manufacturer()
                {   
                    Name = "XYZ Model",
                    Country = "India",
                };
                await _dbContext.Manufacturers.AddAsync(manufacturerEntity, cancellationToken);
                await _dbContext.SaveChangesAsync();

                await transaction.CreateSavepointAsync("Manufacturer_Inserted", cancellationToken);

                var pingPongEntity = new PingPong()
                {
                    Model = "Testing Default Transaction",
                    ManufactureDate = DateTime.Now,
                    ManufacturerId = manufacturerEntity.Id
                };
                await _dbContext.PingPong.AddAsync(pingPongEntity, cancellationToken);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackToSavepointAsync("Manufacturer_Inserted", cancellationToken);
                throw;
            }
        }
    }
}
