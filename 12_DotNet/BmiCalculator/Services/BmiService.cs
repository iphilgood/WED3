using System;
using BmiCalculator.Models;

namespace BmiCalculator.Services
{
  public interface IBmiService 
  {
    double Calculate(Bmi bmi);
  }

  public class BmiService : IBmiService
  {
    public double Calculate(Bmi bmi)
    {
      return Math.Round(bmi.Weight / Math.Pow(bmi.Height / 100, 2), 2);
    }
  }
}
