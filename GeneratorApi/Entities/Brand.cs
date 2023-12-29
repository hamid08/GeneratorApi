using GeneratorApi.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace GeneratorApi.Entities
{
    public class Brand: BaseEntity
    {
        [Display(Name = "عنوان")]
        public string? Caption { get; set; }


        public int MaritalStatusId { get; set; }

        public MaritalStatus MaritalStatus { get; set; }

    }
}
