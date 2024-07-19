using Common.Results;
using static Common.Helpers.RepositoryHelper;

namespace VetAid.Repositories
{
    public class AnimalTypeRepository : IAnimalTypeRepository
    {
        public AnimalTypeRepository(ApplicationDbContext dbContext) =>
            (_dbContext) = (dbContext); 

        private readonly ApplicationDbContext _dbContext;

        public async Task<ServiceResult<AnimalTypeEntity>> AddAsync(AnimalTypeEntity entity)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return ServiceResult<AnimalTypeEntity>.Success(result.Entity);
            });
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var entity = await _dbContext.AnimalTypes.FindAsync(id);
                if (entity == null)
                {
                    return ServiceResult<bool>.Failure(new ServiceError("Entity not found", ServiceErrorType.NotFound));
                }
                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync();
                return ServiceResult<bool>.Success(true);
            });
        }

        public async Task<ServiceResult<IEnumerable<AnimalTypeEntity>>> GetAllAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var entities = await _dbContext.AnimalTypes.ToListAsync();
                return ServiceResult<IEnumerable<AnimalTypeEntity>>.Success(entities);
            });
        }

        public async Task<ServiceResult<AnimalTypeEntity>> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var entity = await _dbContext.AnimalTypes.FindAsync(id);
                if (entity == null)
                {
                    return ServiceResult<AnimalTypeEntity>.Failure(new ServiceError("Entity not found", ServiceErrorType.NotFound));
                }
                return ServiceResult<AnimalTypeEntity>.Success(entity);
            });
        }

        public async Task<ServiceResult<AnimalTypeEntity>> UpdateAsync(int id, AnimalTypeEntity entity)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var animalType = await _dbContext.AnimalTypes.FindAsync(id);
                if (animalType == null)
                {
                    return ServiceResult<AnimalTypeEntity>.Failure(new ServiceError("Entity not found", ServiceErrorType.NotFound));
                }
                animalType.Name = entity.Name ?? animalType.Name;
                await _dbContext.SaveChangesAsync();
                return ServiceResult<AnimalTypeEntity>.Success(animalType);
            });
        }
    }
}
