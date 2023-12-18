using AutoMapper;
using GeneratorApi.Api;
using GeneratorApi.Entities;
using GeneratorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace GeneratorApi.Controllers
{
    public class LicenseTypeController : CrudController<LicenseTypeDto, LicenseTypeSelectDto, LicenseType>
    {
        public LicenseTypeController(Contracts.IRepository<LicenseType> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public override Task<ApiResult<LicenseTypeSelectDto>> Update(int id, LicenseTypeDto dto, CancellationToken cancellationToken)
        {
            return base.Update(id, dto, cancellationToken);
        }
    }
}
