using BusinessObject;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eCategoryStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ODataController
    {
        private readonly ICategoryRepository repository;
        public CategoriesController(ICategoryRepository _repository)
        {
            repository = _repository;
        }

        [EnableQuery(MaxExpansionDepth = 5)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Category>>> Get()
        {
            var list = await repository.GetList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [EnableQuery]
        [HttpGet("{key}")]
        [Authorize]
        public async Task<ActionResult<Category>> GetCategory([FromODataUri] int key)
        {
            var obj = await repository.Get(key);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Category>> Post(Category obj)
        {
            try
            {
                await repository.Add(obj);
                return Ok(obj);
            }
            catch (DbUpdateException)
            {
                if (await repository.Get(obj.CategoryId) != null)
                {
                    return Conflict();
                }
                return BadRequest();
            }
        }

        [HttpPut("{key}")]
        [Authorize]
        public async Task<ActionResult<Category>> Put([FromODataUri] int key, Category obj)
        {
            if (key != obj.CategoryId)
            {
                return BadRequest();
            }

            try
            {
                await repository.Update(obj);
                return Ok(obj);
            }
            catch (DbUpdateException)
            {
                if (await repository.Get(obj.CategoryId) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }

        [HttpDelete("{key}")]
        [Authorize]
        public async Task<ActionResult<Category>> Delete([FromODataUri] int key)
        {
            try
            {
                await repository.Delete(key);
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (await repository.Get(key) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }
    }
}
