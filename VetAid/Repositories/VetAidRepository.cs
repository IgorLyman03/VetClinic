using Common.Results;
using static Common.Helpers.RepositoryHelper;

namespace VetAid.Repositories
{
    public class VetAidRepository : IVetAidRepository
    {

        public VetAidRepository(ApplicationDbContext dbContext) =>
            (_dbContext) = (dbContext);

        private readonly ApplicationDbContext _dbContext;

        public async Task<ServiceResult<VetAidEntity>> AddAsync(VetAidEntity entity)
        {
            return await ExecuteSafeAsync(async () =>
            {
                foreach (var animalType in entity.AnimalTypes)
                {
                    _dbContext.Entry(animalType).State = EntityState.Unchanged;
                }

                if (entity.AnimalTypes != null)
                {
                    entity.AnimalTypes = entity.AnimalTypes.Select(e => _dbContext.AnimalTypes.Find(e.Id)).Where(a => a != null).ToList()!;
                }

                _dbContext.VetAids.Add(entity);
                await _dbContext.SaveChangesAsync();
                return ServiceResult<VetAidEntity>.Success(entity);
            });
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var entity = await _dbContext.FindAsync<VetAidEntity>(id);
                if (entity == null)
                {
                    return ServiceResult<bool>.Failure(new ServiceError("Entity not found", ServiceErrorType.NotFound));
                }
                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync();
                return ServiceResult<bool>.Success(true);
            });
        }

        public async Task<ServiceResult<IEnumerable<VetAidEntity>>> GetAllAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var entities = await _dbContext.VetAids.Include(i => i.AnimalTypes).ToListAsync();
                return ServiceResult<IEnumerable<VetAidEntity>>.Success(entities.AsEnumerable());
            });
        }

        public async Task<ServiceResult<VetAidEntity>> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var entity = await _dbContext.VetAids.Include(v => v.AnimalTypes).FirstOrDefaultAsync(v => v.Id == id);
                return entity != null
                    ? ServiceResult<VetAidEntity>.Success(entity)
                    : ServiceResult<VetAidEntity>.Failure(new ServiceError("Entity not found", ServiceErrorType.NotFound));
            });
        }

        public async Task<ServiceResult<VetAidEntity>> UpdateAsync(int id, VetAidEntity entity)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var vetAid = await _dbContext.VetAids.Include(i => i.AnimalTypes).FirstOrDefaultAsync(v => v.Id == id);
                if (vetAid == null)
                {
                    return ServiceResult<VetAidEntity>.Failure(new ServiceError("Entity not found", ServiceErrorType.NotFound));
                }

                vetAid.Name = entity.Name ?? vetAid.Name;
                vetAid.ServiceType = entity.ServiceType ?? vetAid.ServiceType;
                vetAid.Duration = entity.Duration ?? vetAid.Duration;
                vetAid.Description = entity.Description ?? vetAid.Description;
                vetAid.Price = entity.Price ?? vetAid.Price;

                if (entity.AnimalTypes != null)
                {
                    vetAid.AnimalTypes = entity.AnimalTypes.Select(e => _dbContext.AnimalTypes.Find(e.Id)).Where(a => a != null).ToList()!;
                }

                await _dbContext.SaveChangesAsync();
                return ServiceResult<VetAidEntity>.Success(vetAid);
            });
        }
    }
}
