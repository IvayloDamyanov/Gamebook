using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Gamebook.Web;
using Gamebook.Web.Controllers;
using Gamebook.Services.Contracts;
using Moq;
using NUnit.Framework;

namespace Gamebook.Web.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        private Mock<IBooksService> bookServiceMock = new Mock<IBooksService>();
        private Mock<IPagesService> pagesServiceMock = new Mock<IPagesService>();

        [Test]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(bookServiceMock.Object, pagesServiceMock.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController(bookServiceMock.Object, pagesServiceMock.Object);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [Test]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController(bookServiceMock.Object, pagesServiceMock.Object);

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Stats()
        {
            // Arrange
            HomeController controller = new HomeController(bookServiceMock.Object, pagesServiceMock.Object);

            // Act
            PartialViewResult result = controller.Stats() as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Error()
        {
            // Arrange
            HomeController controller = new HomeController(bookServiceMock.Object, pagesServiceMock.Object);

            // Act
            ViewResult result = controller.Error() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
