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

namespace eOrderDetailStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ODataController
    {
        private readonly IOrderDetailRepository repository;
        public OrderDetailsController(IOrderDetailRepository _repository)
        {
            repository = _repository;
        }

        [EnableQuery(MaxExpansionDepth = 5)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<OrderDetail>>> Get()
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
        public async Task<ActionResult<OrderDetail>> GetOrderDetail([FromODataUri] int keyOrderId, [FromODataUri] int keyProductId)
        {
            var obj = await repository.Get(keyOrderId, keyProductId);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDetail>> Post(OrderDetail obj)
        {
            try
            {
                await repository.Add(obj);
                return Ok(obj);
            }
            catch (DbUpdateException)
            {
                if (await repository.Get(obj.OrderId, obj.ProductId) != null)
                {
                    return Conflict();
                }
                return BadRequest();
            }
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<OrderDetail>> Put([FromODataUri] int keyOrderId, [FromODataUri] int keyProductId, OrderDetail obj)
        {
            if (keyOrderId != obj.OrderId || keyProductId != obj.ProductId)
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
                if (await repository.Get(obj.OrderId, obj.ProductId) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<OrderDetail>> Delete([FromODataUri] int keyOrderId, [FromODataUri] int keyProductId)
        {
            try
            {
                await repository.Delete(keyOrderId, keyProductId);
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (await repository.Get(keyOrderId, keyProductId) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }
    }
}
