using Backend.Domain.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Validation
{
    public class StationValidator : AbstractValidator<StationDto>
    {
        public StationValidator()
        {
            RuleFor(x => x.x).InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");
            RuleFor(x => x.y).InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
        }
    }

}
