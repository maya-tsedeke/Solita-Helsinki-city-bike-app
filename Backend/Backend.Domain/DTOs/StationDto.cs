
namespace Backend.Domain.DTOs
{
    public class StationDto
    {
        public int FID { get; set; }
        public int ID { get; set; }
        public string Nimi { get; set; } 
        public string Namn { get; set; } 
        public string Name { get; set; } 
        public string Osoite { get; set; } 
        public string Address { get; set; }
        public string Kaupunki { get; set; }
        public string Stad { get; set; }
        public string Operaattor { get; set; }
        public string Kapasiteet { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public StationDto() { }

    }
    public class addressDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public addressDto() { }

    }
    public class SIDRequestDto
    {
        public int ID { get; set; }
        public SIDRequestDto() { }
    }
    public static class StationDtoExtensions
    {
        public static string[] ToStringArray(this StationDto dto)
        {
            return new string[] {
            dto.FID.ToString(),
            dto.ID.ToString(),
            dto.Nimi,
            dto.Namn,
            dto.Name,
            dto.Osoite,
            dto.Address,
            dto.Kaupunki,
            dto.Stad,
            dto.Operaattor,
            dto.Kapasiteet.ToString(),
            dto.x.ToString(),
            dto.y.ToString()
        };
        }
    }



}
