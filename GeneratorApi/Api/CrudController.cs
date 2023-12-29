using AutoMapper;
using AutoMapper.QueryableExtensions;
using GeneratorApi.Contracts;
using GeneratorApi.Entities.Base;
using GeneratorApi.Extensions.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneratorApi.Api
{
    public class CrudController<TDto, TSelectDto, TEntity, TKey> : BaseController
        where TDto : BaseDto<TDto, TEntity, TKey>, new()
        where TSelectDto : BaseDto<TSelectDto, TEntity, TKey>, new()
        where TEntity : class, IEntity<TKey>, new()
    {
        protected readonly IRepository<TEntity> Repository;
        protected readonly IMapper Mapper;

        public CrudController(IRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }


        [HttpPost("Grid")]
        public virtual async Task<ActionResult<List<TSelectDto>>> Grid(BaseRequestGridDto? filter, CancellationToken cancellationToken)
        {
            var list = await Repository.TableNoTracking.ProjectTo<TSelectDto>(Mapper.ConfigurationProvider)
                .ApplySearchFilters(filter, cancellationToken);

            return Ok(list);
        }

        [HttpGet("{id}")]
        public virtual async Task<ApiResult<TSelectDto>> Get(TKey id, CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking.ProjectTo<TSelectDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id!.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }

        [HttpPost]
        public virtual async Task<ApiResult<TSelectDto>> Create(TDto dto, CancellationToken cancellationToken)
        {
            dto.Id = default!;
            var model = dto.ToEntity(Mapper);

            await Repository.AddAsync(model, cancellationToken);

            return Ok();

        }

        [HttpPut]
        public virtual async Task<ApiResult<TSelectDto>> Update(TKey id, TDto dto, CancellationToken cancellationToken)
        {
            var model = await Repository.GetByIdAsync(cancellationToken, id);

            if (model == null)
                return NotFound();

            model = dto.ToEntity(Mapper, model);

            await Repository.UpdateAsync(model, cancellationToken);

            return Ok();

        }

        [HttpDelete("{id}")]
        public virtual async Task<ApiResult> Delete(TKey id, CancellationToken cancellationToken)
        {
            var model = await Repository.GetByIdAsync(cancellationToken, id);

            if (model == null)
                return NotFound();

            await Repository.DeleteAsync(model, cancellationToken);

            return Ok();
        }

        [HttpDelete("DeleteAll")]
        public virtual async Task<IActionResult> DeleteAll([FromQuery] List<TKey> ids, CancellationToken cancellationToken)
        {
            var model = await Repository.TableNoTracking.Where(c=> ids.Contains(c.Id)).ToListAsync(cancellationToken);

            if (model == null || !model.Any())
                return NotFound();

            await Repository.DeleteRangeAsync(model, cancellationToken);

            return Ok();
        }
    }

    public class CrudController<TDto, TSelectDto, TEntity> : CrudController<TDto, TSelectDto, TEntity, int>
        where TDto : BaseDto<TDto, TEntity, int>, new()
        where TSelectDto : BaseDto<TSelectDto, TEntity, int>, new()
        where TEntity : class, IEntity<int>, new()
    {
        public CrudController(IRepository<TEntity> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }

    public class CrudController<TDto, TEntity> : CrudController<TDto, TDto, TEntity, int>
        where TDto : BaseDto<TDto, TEntity, int>, new()
        where TEntity : class, IEntity<int>, new()
    {
        public CrudController(IRepository<TEntity> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
