
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Entities;
// Station entity
[Keyless]
public class Station
{

    public int FID { get; set; }
    public int ID { get; set; }
    public string Nimi { get; set; } = string.Empty;
    public string Namn { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Osoite { get; set; } = string.Empty;
    public string Address { get; set; }
    public string Kaupunki { get; set; }
    public string Stad { get; set; }
    public string Operaattor { get; set; }
    public string Kapasiteet { get; set; }

    public double x{ get; set; }
    public double y { get; set; }

    public virtual ICollection<Journey> DepartureJourneys { get; set; }
    public virtual ICollection<Journey> ReturnJourneys { get; set; }
}
