using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BmiCalculator.Models;
using BmiCalculator.Services;

namespace BmiCalculator.Controllers
{
  public class HomeController : Controller
  {
    private readonly IBmiService _bmiService;

    public HomeController(IBmiService bmiService) {
      _bmiService = bmiService;
    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Calculate(Bmi bmi)
    {
      var calculatedBmi = _bmiService.Calculate(bmi);

      if (calculatedBmi >= 0 && calculatedBmi <= 999) {
        ViewData["calculatedBmi"] = calculatedBmi;
        return PartialView("Overview");
      }

      return Content("Fehlerhafte Daten");
    }

    public IActionResult Overview()
    {
      return View();
    }

    public IActionResult Error()
    {
      return View();
    }
  }
}
