using GeneratorApi.Entities.Base;
using GeneratorApi.Enums.Base;
using System.ComponentModel.DataAnnotations;

namespace GeneratorApi.Entities
{
    [Display(Name = "نوع گواهی نامه")]
    public class LicenseType : BaseEntity
    {
        [Display(Name = "عنوان")]
        public string? Caption { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Display(Name = "وضعیت")]
        public ActiveStatus ActivateStatus { get; set; }
    }
}
