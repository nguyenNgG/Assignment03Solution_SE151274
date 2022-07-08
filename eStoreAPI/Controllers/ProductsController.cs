using BusinessObject;
using DataAccess.Repositories.Interfaces;
using eStoreAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eProductStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ODataController
    {
        private readonly IProductRepository repository;
        public ProductsController(IProductRepository _repository)
        {
            repository = _repository;
        }

        [EnableQuery(MaxExpansionDepth = 5)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Product>>> Get()
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
        public async Task<ActionResult<Product>> GetProduct([FromODataUri] int key)
        {
            var obj = await repository.Get(key);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Administrator)]
        public async Task<ActionResult<Product>> Post(Product obj)
        {
            try
            {
                await repository.Add(obj);
                return Ok(obj);
            }
            catch (DbUpdateException)
            {
                if (await repository.Get(obj.ProductId) != null)
                {
                    return Conflict();
                }
                return BadRequest();
            }
        }

        [HttpPut("{key}")]
        [Authorize(Roles = RoleName.Administrator)]
        public async Task<ActionResult<Product>> Put([FromODataUri] int key, Product obj)
        {
            if (key != obj.ProductId)
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
                if (await repository.Get(obj.ProductId) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }

        [HttpDelete("{key}")]
        [Authorize(Roles = RoleName.Administrator)]
        public async Task<ActionResult<Product>> Delete([FromODataUri] int key)
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
