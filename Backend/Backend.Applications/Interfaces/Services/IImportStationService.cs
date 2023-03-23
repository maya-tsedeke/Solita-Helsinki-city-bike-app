
namespace Backend.Applications.Interfaces.Services
{
    public interface IImportStationService<T>
    {
        Task<bool> ImportAsync(Stream stream);
        Task<bool> BulkInsertAsync(IEnumerable<T> entities);
    }
}
