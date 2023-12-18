using AutoMapper;
using GeneratorApi.Api;
using GeneratorApi.Contracts;
using GeneratorApi.Entities;
using GeneratorApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorApi.Controllers
{
    public class MaritalStatusController : CrudController<MaritalStatusDto, MaritalStatusSelectDto, MaritalStatus>
    {
        public MaritalStatusController(IRepository<MaritalStatus> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }


}
