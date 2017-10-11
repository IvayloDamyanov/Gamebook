using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gamebook.Web;
using Gamebook.Web.Controllers;
using Gamebook.Services.Contracts;
using Moq;

namespace Gamebook.Web.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private Mock<IBooksService> bookServiceMock = new Mock<IBooksService>();
        private Mock<IPagesService> pagesServiceMock = new Mock<IPagesService>();

        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(bookServiceMock.Object, pagesServiceMock.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController(bookServiceMock.Object, pagesServiceMock.Object);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController(bookServiceMock.Object, pagesServiceMock.Object);

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
