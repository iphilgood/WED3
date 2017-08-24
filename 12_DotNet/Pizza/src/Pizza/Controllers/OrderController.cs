using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizza.Data;
using Pizza.Models;
using Pizza.Models.OrderViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pizza.Services;

namespace Pizza.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService; 

        public OrderController(OrderService orderService)
        {
            _orderService = orderService; 
        }

        public IActionResult Create()
        {   
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(NewOrderViewModel newOrder)
        {
            if (ModelState.IsValid)
            {
                var order = _orderService.Add(User, newOrder);
                TempData["Text"] = "Danke für ihre Bestellung:";
                return PartialView("Add", order);
            }
            else
            {
                return BadRequest();
            }
        }


        public IActionResult Show(long id)
        {
            var order = _orderService.Get(User, id);
            ViewBag.Text = "Ihre Bestellung:";
            return View("Add", order);
        }

        public IActionResult Delete(long id)
        {
            var order = _orderService.Delete(id);
            return View("Add", order);
        }
    }
}