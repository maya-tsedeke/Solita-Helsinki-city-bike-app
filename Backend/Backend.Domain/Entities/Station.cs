
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Backend.Domain.Entities;
// Station entity
[Keyless]
public class Station
{
    [AllowNull]
    public int FID { get; set; }
    public int ID { get; set; }
    public string Nimi { get; set; } = string.Empty;
    public string Namn { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Osoite { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Kaupunki { get; set; } = string.Empty;
    public string Stad { get; set; } = string.Empty;
    public string Operaattor { get; set; } = string.Empty;
    public string Kapasiteet { get; set; } = string.Empty;
    [AllowNull]
    public double x { get; set; }
    [AllowNull]
    public double y { get; set; } 

    public virtual ICollection<Journey> DepartureJourneys { get; set; }
    public virtual ICollection<Journey> ReturnJourneys { get; set; }
}
