using GeneratorApi.Api;
using GeneratorApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace GeneratorApi.Models
{
    public class BrandDto : BaseDto<BrandDto, Brand>, IValidatableObject
    {
        [Display(Name = "عنوان")]
        public string? Caption { get; set; }

        public int MaritalStatusId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MaritalStatusId <= 0)
                yield return new ValidationResult("الزامی می باشد", new[] { nameof(Caption) });
        }
    }

    public class BrandSelectDto : BaseDto<BrandSelectDto, Brand>
    {
        [Display(Name = "عنوان")]
        public string? Caption { get; set; }

        public string MaritalStatusCaption { get; set; }
        public string MaritalStatusDescription { get; set; }
        public int MaritalStatusId { get; set; }


    }
}
