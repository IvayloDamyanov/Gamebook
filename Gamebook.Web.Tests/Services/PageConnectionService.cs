using Gamebook.Data.Model;
using Gamebook.Data.Repositories.Contracts;
using Gamebook.Data.SaveContext.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamebook.Services;
using Gamebook.Services.Contracts;

namespace Gamebook.Web.Tests.Services
{
    [TestClass]
    public class PageConnectionService
    {
        Mock<IEfRepository<PageConnection>> pageConnectionsRepoMock = new Mock<IEfRepository<PageConnection>>();
        Mock<IBooksService> bookServiceMock = new Mock<IBooksService>();
        Mock<ISaveContext> contextMock = new Mock<ISaveContext>();
        
        [TestMethod]
        public void GetAllShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<PageConnection>();
            var pageConnectionsService = new PageConnectionsService(pageConnectionsRepoMock.Object, bookServiceMock.Object, contextMock.Object);

            // Act
            pageConnectionsRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = pageConnectionsService.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<PageConnection>));
        }

        [TestMethod]
        public void GetAllAndDeletedShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<PageConnection>();
            var pageConnectionsService = new PageConnectionsService(pageConnectionsRepoMock.Object, bookServiceMock.Object, contextMock.Object);

            // Act
            pageConnectionsRepoMock.Setup(x => x.AllAndDeleted).Returns(list.AsQueryable);
            var result = pageConnectionsService.GetAllAndDeleted();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<PageConnection>));
        }

        [TestMethod]
        public void GetChildPagesShould_ReturnCorrectType()
        {
            // Arrange
            var pageConnectionsService = new PageConnectionsService(pageConnectionsRepoMock.Object, bookServiceMock.Object, contextMock.Object);

            // Act
            var result = pageConnectionsService.GetChildPages(1, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<PageConnection>));
        }

        [TestMethod]
        public void GetChildPagesShould_ReturnCorrectValue()
        {
            // Arrange
            int bookCatNum = 1;
            Guid bookId = Guid.NewGuid();
            Book book = new Book() { Id = bookId, CatalogueNumber = bookCatNum };
            int parentPageNum1 = 1;
            int parentPageNum2 = 2;
            PageConnection pageConnection1 = new PageConnection() { Book = book, ParentPageNumber = parentPageNum1 };
            PageConnection pageConnection2 = new PageConnection() { Book = book, ParentPageNumber = parentPageNum2 };
            var listBoth = new List<PageConnection>() { pageConnection1, pageConnection2 };
            var pageConnectionsService = new PageConnectionsService(pageConnectionsRepoMock.Object, bookServiceMock.Object, contextMock.Object);

            // Act
            pageConnectionsRepoMock.Setup(x => x.All).Returns(listBoth.AsQueryable);
            bookServiceMock.Setup(x => x.FindSingle(bookCatNum)).Returns(book);
            var result = pageConnectionsService.GetChildPages(bookCatNum, parentPageNum2);

            // Assert
            Assert.AreEqual(pageConnection2, result.First());
        }

        [TestMethod]
        public void AddShould_ReturnValue()
        {
            // Arrange
            var pageConnectionsService = new PageConnectionsService(pageConnectionsRepoMock.Object, bookServiceMock.Object, contextMock.Object);
            PageConnection pageConnection = new PageConnection();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = pageConnectionsService.Add(pageConnection);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void DeleteShould_ReturnValue()
        {
            // Arrange
            var pageConnectionsService = new PageConnectionsService(pageConnectionsRepoMock.Object, bookServiceMock.Object, contextMock.Object);
            PageConnection pageConnection = new PageConnection();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = pageConnectionsService.Delete(pageConnection);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void UpdateShould_ReturnValue()
        {
            // Arrange
            var pageConnectionsService = new PageConnectionsService(pageConnectionsRepoMock.Object, bookServiceMock.Object, contextMock.Object);
            PageConnection pageConnection = new PageConnection();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = pageConnectionsService.Update(pageConnection);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }
    }
}
