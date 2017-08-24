using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pizza.Data;
using Pizza.Exceptions;
using Pizza.Models;
using Pizza.Models.OrderViewModels;

namespace Pizza.Services
{
    public class OrderService {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public OrderService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Order Add(ClaimsPrincipal user, NewOrderViewModel order)
        {
            var newOrder = new Order() {Name = order.Name, CustomerId = _userManager.GetUserId(user)};
            _context.Orders.Add(newOrder);
            _context.SaveChanges();
            return newOrder;
        }

        
        public Order Delete(long id)
        {
            var order = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                throw new ServiceException(ServiceExceptionType.NotFound);
            }
            if (order.State == OrderState.New)
            {
                order.State = OrderState.Deleted;
                _context.SaveChanges();
                return order;
            }
            else
            {
                throw new ServiceException(ServiceExceptionType.ForbiddenByRule);
            }

        } 
        public Order Get(ClaimsPrincipal user, long id)
        {
            var userId = _userManager.GetUserId(user);
            var order = _context.Orders.Where(x => x.Id == id && userId == x.CustomerId).Include(x => x.Customer).FirstOrDefault();
            if (order == null)
            {
                throw new ServiceException(ServiceExceptionType.NotFound);
            }
            return order;
        }
    }
}
