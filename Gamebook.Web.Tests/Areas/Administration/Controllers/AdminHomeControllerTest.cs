using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Gamebook.Web;
using Gamebook.Web.Areas.Administration.Controllers;
using Gamebook.Services.Contracts;
using Moq;
using NUnit.Framework;

namespace Gamebook.Web.Tests.Areas.Administration.Controllers
{
    [TestFixture]
    public class AdminHomeControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
