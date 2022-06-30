using BusinessObject;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eOrderStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ODataController
    {
        private readonly IOrderRepository repository;
        public OrdersController(IOrderRepository _repository)
        {
            repository = _repository;
        }

        [EnableQuery(MaxExpansionDepth = 5)]
        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
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
        public async Task<ActionResult<Order>> GetOrder([FromODataUri] int key)
        {
            var obj = await repository.Get(key);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Post(Order obj)
        {
            try
            {
                await repository.Add(obj);
                return Created(obj);
            }
            catch (DbUpdateException)
            {
                if (await repository.Get(obj.OrderId) != null)
                {
                    return Conflict();
                }
                return BadRequest();
            }
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<Order>> Put([FromODataUri] int key, Order obj)
        {
            if (key != obj.OrderId)
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
                if (await repository.Get(obj.OrderId) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<Order>> Delete([FromODataUri] int key)
        {
            try
            {
                await repository.Delete(key);
                return NoContent();
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
