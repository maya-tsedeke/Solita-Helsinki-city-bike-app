
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
    public class SIDRequestDto
    {
        public int ID { get; set; }
        public SIDRequestDto() { }
    }
   
    
}
