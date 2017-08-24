using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizza.Data;
using Pizza.Exceptions;
using Pizza.Models;
using Pizza.Models.OrderViewModels;
using Pizza.Services;

namespace Pizza.Controllers.api
{

    [Route("api/orders")]
    [Authorize]
    public class OrderApiController : Controller
    {
        private readonly OrderService _orderSerivce;
        
        public OrderApiController(OrderService orderSerivce)
        {
            _orderSerivce = orderSerivce;
        }

        [HttpPost]
        public IActionResult Create([FromBody]NewOrderViewModel newOrder)
        {
            if (ModelState.IsValid)
            {
                var order = _orderSerivce.Add(User, newOrder);
                return new CreatedAtActionResult("Show", "OrderApi", new {Id = order.Id}, order);
            }
            else
            {
                throw new ServiceException(ServiceExceptionType.ForbiddenByRule);
            }
        }

    
        [HttpGet("{id}")]
        public IActionResult Show(long id)
        {
            return new OkObjectResult(_orderSerivce.Get(User, id));
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            return new OkObjectResult(_orderSerivce.Delete(id));
        }
    }
}