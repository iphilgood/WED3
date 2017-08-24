using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizza.Controllers;
using Xunit;

namespace Pizza.Test
{
    public class HomeControllerTest
    {
        [Fact]
        public void TestName()
        {

            var settingUpper = new NameSettings();
            var settingLower = new NameSettings() { LetterCase = "lower" };
            var name = ("Pizzza Shop - The Best!").ToUpper();

            var controller = new HomeController();
            Assert.Equal(name, ((ContentResult) controller.Name(settingUpper)).Content);
            Assert.NotEqual(name, ((ContentResult) controller.Name(settingLower)).Content);
        }
    }
}
