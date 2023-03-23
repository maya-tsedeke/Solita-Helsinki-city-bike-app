using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Applications.Interfaces.Repositories
{
    public interface ICSV_Repository<T>
    {
        Task<bool> BulkInsertAsync(IEnumerable<T> entities);
    }
}
