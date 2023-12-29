using AutoMapper;
using GeneratorApi.Api;
using GeneratorApi.Entities;
using GeneratorApi.Filters;
using System.ComponentModel.DataAnnotations;

namespace GeneratorApi.Models
{
    public class MaritalStatusDto : BaseDto<MaritalStatusDto, MaritalStatus>
    {
        [Display(Name = "عنوان")]
        public string? Caption { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
        public int Port { get; set; }


    }

    public class MaritalStatusSelectDto : BaseDto<MaritalStatusSelectDto, MaritalStatus>
    {
        [Display(Name = "عنوان")]
        public string? Caption { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
        public List<string> BrandCaptions { get; set; }
        public string BrandCaptionsWithJoin { get; set; }

        public int Port { get; set; }


        public override void CustomMappings(IMappingExpression<MaritalStatus, MaritalStatusSelectDto> mapping)
        {
            mapping.ForMember(
                   dest => dest.BrandCaptions,
                   config => config.MapFrom(src => src.Brands.Select(c=> c.Caption).ToList()));

            mapping.ForMember(
                dest => dest.BrandCaptionsWithJoin,
                config => config.MapFrom(src => 
                string.Join(",",src.Brands.Select(c => c.Caption).ToList())
                
                ));
        }

    }

    

}
