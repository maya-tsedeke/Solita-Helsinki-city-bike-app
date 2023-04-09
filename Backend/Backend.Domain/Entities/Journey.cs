using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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
    public int? ReturnStationId { get; set; }
    public double CoveredDistanceInMeters { get; set; }
    public double DurationInSeconds { get; set; }
    public int? UserId { get; set; }
    [ForeignKey(nameof(DepartureStationId))]
    public virtual Station DepartureStation { get; set; }

    [ForeignKey(nameof(ReturnStationId))]
    public virtual Station ReturnStation { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User users { get; set; }
}
