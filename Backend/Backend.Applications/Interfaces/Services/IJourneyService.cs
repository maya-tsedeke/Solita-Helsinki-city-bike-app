using Backend.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Applications.Interfaces.Services
{
    public interface IJourneyService
    {
        Task<IEnumerable<JourneyDto>> ListJourneys(int limit = 100, int offset = 0, string orderBy = null, string search = null);
        Task<bool> ImportJourneysFromCsv(string filePath);
    }
}
