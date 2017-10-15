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
using Gamebook.Data.Model;
using Gamebook.Web.Models.Book;

namespace Gamebook.Web.Tests.Controllers
{
    [TestClass]
    public class BookControllerTest
    {
        private Mock<IBooksService> booksServiceMock = new Mock<IBooksService>();
        private Mock<IPagesService> pagesServiceMock = new Mock<IPagesService>();
        private Mock<IPageConnectionsService> pageConnectionsServiceMock = new Mock<IPageConnectionsService>();

        [TestMethod]
        public void Index()
        {
            // Arrange
            BookController controller = new BookController(booksServiceMock.Object, pagesServiceMock.Object, pageConnectionsServiceMock.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Detailed()
        {
            // Arrange
            BookController controller = new BookController(booksServiceMock.Object, pagesServiceMock.Object, pageConnectionsServiceMock.Object);
            int id = 1;
            Mock<Book> bookMock = new Mock<Book>();

            // Act
            booksServiceMock.Setup(x => x.FindSingle(id)).Returns(bookMock.Object);
            ViewResult result = controller.Detailed(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void List()
        {
            // Arrange
            BookController controller = new BookController(booksServiceMock.Object, pagesServiceMock.Object, pageConnectionsServiceMock.Object);
            string searchTerm = "searchTerm";
            var list = new List<Book>() { new Book() };

            // Act
            booksServiceMock.Setup(x => x.FindAll(searchTerm)).Returns(list.AsQueryable);
            booksServiceMock.Setup(x => x.Pagination(1, 5, 1)).Returns(new Tuple<int, int>(0, 0));
            booksServiceMock.Setup(x => x.PagesNav(1, 5, 1)).Returns(new int[1] { 1});
            ViewResult result = controller.List(searchTerm) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Read()
        {
            // Arrange
            BookController controller = new BookController(booksServiceMock.Object, pagesServiceMock.Object, pageConnectionsServiceMock.Object);
            int book = 1;
            int page = 1;
            var list = new List<PageConnection>() { new PageConnection() };

            // Act
            pagesServiceMock.Setup(x => x.Find(book, page)).Returns(new Page() { Book = new Book() });
            pageConnectionsServiceMock.Setup(x => x.getChildPages(book, page)).Returns(list.AsQueryable);
            ViewResult result = controller.Read(book, page) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

    }
}
