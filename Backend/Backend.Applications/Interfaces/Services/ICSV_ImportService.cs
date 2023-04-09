namespace Backend.Applications.Interfaces.Services
{
    public interface ICSV_ImportService<T>
    {
        Task<bool> ImportAsync(Stream stream);
        Task<bool> BulkInsertAsync(IEnumerable<T> entities);
    }
}
