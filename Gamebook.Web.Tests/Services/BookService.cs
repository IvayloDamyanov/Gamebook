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

namespace Gamebook.Web.Tests.Services
{
    [TestClass]
    public class BookService
    {
        Mock<IEfRepository<Book>> bookRepoMock = new Mock<IEfRepository<Book>>();
        Mock<ISaveContext> contextMock = new Mock<ISaveContext>();
        
        [TestMethod]
        public void GetAllShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<Book>();
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = bookService.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<Book>));
        }

        [TestMethod]
        public void GetAllAndDeletedShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<Book>();
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.AllAndDeleted).Returns(list.AsQueryable);
            var result = bookService.GetAllAndDeleted();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<Book>));
        }

        [TestMethod]
        public void FindAllShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<Book>();
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = bookService.FindAll("1");

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<Book>));
        }

        [TestMethod]
        public void FindAllShould_ReturnCorrectTypeIfPassedEmptyString()
        {
            // Arrang
            var list = new List<Book>();
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = bookService.FindAll(string.Empty);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<Book>));
        }

        [TestMethod]
        public void FindAllShould_ReturnCorrectTypeIfPassedNullValue()
        {
            // Arrang
            var list = new List<Book>();
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = bookService.FindAll(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<Book>));
        }

        [TestMethod]
        public void FindAllShould_ReturnCorrectResultIfPassedValueCanBeParsedToInt()
        {
            // Arrang
            int catNum1 = 1;
            string title1 = "sometitle";
            int catNum2 = 2;
            string title2 = "othertitle";
            Book book1 = new Book() { CatalogueNumber = catNum1, Title = title1 };
            Book book2 = new Book() { CatalogueNumber = catNum2, Title = title2 };
            var list = new List<Book>() { book1, book2 };
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = bookService.FindAll(catNum2.ToString()).First();

            // Assert
            Assert.AreEqual(result, book2);
        }

        [TestMethod]
        public void FindAllShould_ReturnEmptyCollectionIfNothingIsFound()
        {
            // Arrang
            int catNum1 = 1;
            string title1 = "sometitle";
            int catNum2 = 2;
            string title2 = "othertitle";
            Book book1 = new Book() { CatalogueNumber = catNum1, Title = title1 };
            Book book2 = new Book() { CatalogueNumber = catNum2, Title = title2 };
            var list = new List<Book>() { book1, book2 };
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = bookService.FindAll("book");

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void FindSingleShould_ReturnCorrectType()
        {
            // Arrange
            int catNum = 1;
            Book book1 = new Book() { CatalogueNumber = catNum };
            var list = new List<Book>() { book1 };
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.AllAndDeleted).Returns(list.AsQueryable);
            var result = bookService.FindSingle(catNum);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Book));
        }

        [TestMethod]
        public void FindSingleShould_ReturnCorrectValue()
        {
            // Arrange
            int catNum = 1;
            Book book1 = new Book() { CatalogueNumber = catNum };
            Book book2 = new Book() { CatalogueNumber = catNum };
            var list = new List<Book>() { book1, book2 };
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.AllAndDeleted).Returns(list.AsQueryable);
            var result = bookService.FindSingle(catNum);

            // Assert
            Assert.AreEqual(book1, result);
        }

        [TestMethod]
        public void AddShould_ReturnValue()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            Book book = new Book();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = bookService.Add(book);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void DeleteShould_ReturnValue()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            Book book = new Book();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = bookService.Delete(book);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void UpdateShould_ReturnValue()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            Book book = new Book();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = bookService.Update(book);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }
    }
}
