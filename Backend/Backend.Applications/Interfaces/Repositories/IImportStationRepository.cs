
namespace Backend.Applications.Interfaces.Repositories
{
    public interface IImportStationRepository<T> 
    {
        Task<bool> BulkInsertAsync(IEnumerable<T> entities);
    }
}
