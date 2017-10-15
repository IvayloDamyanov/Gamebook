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
    public class PageService
    {
        Mock<IEfRepository<Page>> pageRepoMock = new Mock<IEfRepository<Page>>();
        Mock<IBooksService> bookServiceMock = new Mock<IBooksService>();
        Mock<ISaveContext> contextMock = new Mock<ISaveContext>();
        
        [TestMethod]
        public void GetAllShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<Page>();
            var pagesService = new PagesService(pageRepoMock.Object, bookServiceMock.Object, contextMock.Object);

            // Act
            pageRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = pagesService.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<Page>));
        }

        [TestMethod]
        public void GetAllAndDeletedShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<Page>();
            var pagesService = new PagesService(pageRepoMock.Object, bookServiceMock.Object, contextMock.Object);

            // Act
            pageRepoMock.Setup(x => x.AllAndDeleted).Returns(list.AsQueryable);
            var result = pagesService.GetAllAndDeleted();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<Page>));
        }

        [TestMethod]
        public void FindShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<Page>();
            var pagesService = new PagesService(pageRepoMock.Object, bookServiceMock.Object, contextMock.Object);

            // Act
            pageRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = pagesService.Find(1, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Page));
        }

        [TestMethod]
        public void FindShould_ReturnCorrectValue()
        {
            // Arrange
            int bookCatNum = 1;
            Guid bookId = Guid.NewGuid();
            Book book = new Book() { Id = bookId, CatalogueNumber = bookCatNum };
            int pageNum1 = 1;
            int pageNum2 = 2;
            Page page1 = new Page() { Book = book, Number = pageNum1 };
            Page page2 = new Page() { Book = book, Number = pageNum2 };
            var list = new List<Page>() { page1, page2 };
            var pagesService = new PagesService(pageRepoMock.Object, bookServiceMock.Object, contextMock.Object);

            // Act
            bookServiceMock.Setup(x => x.FindSingle(bookCatNum)).Returns(book);
            pageRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = pagesService.Find(bookCatNum, pageNum2);

            // Assert
            Assert.AreEqual(page2, result);
        }

        [TestMethod]
        public void AddShould_ReturnValue()
        {
            // Arrange
            var pagesService = new PagesService(pageRepoMock.Object, bookServiceMock.Object, contextMock.Object);
            Page page = new Page();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = pagesService.Add(page);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void DeleteShould_ReturnValue()
        {
            // Arrange
            var pagesService = new PagesService(pageRepoMock.Object, bookServiceMock.Object, contextMock.Object);
            Page page = new Page();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = pagesService.Delete(page);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void UpdateShould_ReturnValue()
        {
            // Arrange
            var pagesService = new PagesService(pageRepoMock.Object, bookServiceMock.Object, contextMock.Object);
            Page page = new Page();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = pagesService.Update(page);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }
    }
}
