using AutoMapper;
using GeneratorApi.Api;
using GeneratorApi.Contracts;
using GeneratorApi.Entities;
using GeneratorApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorApi.Controllers
{
    public class BrandController : CrudController<BrandDto, BrandSelectDto, Brand>
    {
        public BrandController(IRepository<Brand> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }


}
