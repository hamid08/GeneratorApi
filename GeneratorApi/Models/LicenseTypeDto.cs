using AutoMapper;
using AutoMapper.Configuration;
using GeneratorApi.Api;
using GeneratorApi.Entities;
using GeneratorApi.Enums.Base;
using GeneratorApi.Utilities;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace GeneratorApi.Models
{
    public class LicenseTypeDto : BaseDto<LicenseTypeDto, LicenseType>, IValidatableObject
    {
        [Display(Name = "عنوان")]
        public string? Caption { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Display(Name = "وضعیت")]
        public ActiveStatus ActivateStatus { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Caption == "" || Caption == null)
                yield return new ValidationResult("الزامی می باشد", new[] { nameof(Caption) });
         
            //if (ActivateStatus != ActiveStatus.Active || ActivateStatus != ActiveStatus.NotActive)
            //    yield return new ValidationResult("وضعیت ارسالی معتبر نمی باشد", new[] { nameof(ActiveStatus) });
        }
    }

    public class LicenseTypeSelectDto : BaseDto<LicenseTypeSelectDto, LicenseType>
    {
        [Display(Name = "عنوان")]
        public string? Caption { get; set; }

        public string? FullText { get; set; }


        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Display(Name = "وضعیت")]
        public ActiveStatus ActivateStatus { get; set; }

        public string ActivateStatusStr => ActivateStatus.ToDisplay();


        public override void CustomMappings(IMappingExpression<LicenseType, LicenseTypeSelectDto> mapping)
        {
            mapping.ForMember(
                   dest => dest.FullText,
                   config => config.MapFrom(src => $"{src.Caption} ({src.Description})"));
        }

    }
}
