using System;
using System.ComponentModel.DataAnnotations;

namespace BmiCalculator.Models
{
  public class Bmi
  {
    [Required]
    [Display(Name = "Gewicht in kg")]
    public double Weight { get; set; }

    [Required]
    [Display(Name = "Höhe in cm")]
    public double Height { get; set; }
  }
}
