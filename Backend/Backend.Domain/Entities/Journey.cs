using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;
namespace Backend.Domain.Entities;
// Journey entity

public class Journey
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Ignore]
    public int Id { get; set; }
    public DateTime Departure { get; set; }
    public DateTime Return { get; set; }
    public int DepartureStationId { get; set; }
    public int ReturnStationId { get; set; }
    public int CoveredDistanceInMeters { get; set; }
    public int DurationInSeconds { get; set; }

    public virtual Station DepartureStation { get; set; }
    public virtual Station ReturnStation { get; set; }
}
