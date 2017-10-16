using Gamebook.Data.Model;
using Gamebook.Data.Repositories.Contracts;
using Gamebook.Data.SaveContext.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamebook.Services;
using NUnit.Framework;

namespace Gamebook.Web.Tests.Services
{
    [TestFixture]
    public class BookService
    {
        Mock<IEfRepository<Book>> bookRepoMock = new Mock<IEfRepository<Book>>();
        Mock<ISaveContext> contextMock = new Mock<ISaveContext>();
        
        [Test]
        public void GetAllShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<Book>();
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = bookService.GetAll();

            // Assert
            Assert.IsInstanceOf(typeof(IQueryable<Book>), result);
        }

        [Test]
        public void GetAllAndDeletedShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<Book>();
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.AllAndDeleted).Returns(list.AsQueryable);
            var result = bookService.GetAllAndDeleted();

            // Assert
            Assert.IsInstanceOf(typeof(IQueryable<Book>), result);
        }

        [Test]
        public void FindAllShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<Book>();
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = bookService.FindAll("1");

            // Assert
            Assert.IsInstanceOf(typeof(IQueryable<Book>), result);
        }

        [Test]
        public void FindAllShould_ReturnCorrectTypeIfPassedEmptyString()
        {
            // Arrang
            var list = new List<Book>();
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = bookService.FindAll(string.Empty);

            // Assert
            Assert.IsInstanceOf(typeof(IQueryable<Book>), result);
        }

        [Test]
        public void FindAllShould_ReturnCorrectTypeIfPassedNullValue()
        {
            // Arrang
            var list = new List<Book>();
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);

            // Act
            bookRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = bookService.FindAll(null);

            // Assert
            Assert.IsInstanceOf(typeof(IQueryable<Book>), result);
        }

        [Test]
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

        [Test]
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

        [Test]
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
            Assert.IsInstanceOf(typeof(Book), result);
        }

        [Test]
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

        [Test]
        public void AddShould_ReturnValue()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            Book book = new Book();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = bookService.Add(book);

            // Assert
            Assert.IsInstanceOf(typeof(int), result);
        }

        [Test]
        public void DeleteShould_ReturnValue()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            Book book = new Book();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = bookService.Delete(book);

            // Assert
            Assert.IsInstanceOf(typeof(int), result);
        }

        [Test]
        public void UpdateShould_ReturnValue()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            Book book = new Book();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = bookService.Update(book);

            // Assert
            Assert.IsInstanceOf(typeof(int), result);
        }

        [Test]
        public void PageNavShould_ReturnCorrectValueWhenPassedPositiveBookCount()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            int booksCount = 50;
            int resultsPerPage = 5;
            int page = 1;
            int[] expectedResult = new int[] { 1, 2, 3 };

            // Arrange
            var result = bookService.PagesNav(booksCount, resultsPerPage, page);

            // Assert
            Assert.AreEqual(result[2], expectedResult[2]);
        }

        [Test]
        public void PageNavShould_ReturnCorrectValueWhenPassedZeroBookCount()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            int booksCount = 0;
            int resultsPerPage = 5;
            int page = 1;
            int[] expectedResult = new int[] { };

            // Arrange
            var result = bookService.PagesNav(booksCount, resultsPerPage, page);

            // Assert
            Assert.AreEqual(result.Length, expectedResult.Length);
        }

        [Test]
        public void PagenationShould_ReturnCorrectValueWhenPassedPositiveBookCount()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            int booksCount = 50;
            int resultsPerPage = 5;
            int page = 1;
            Tuple<int, int> expectedResult = new Tuple<int, int>(0, 5);

            // Arrange
            var result = bookService.Pagination(booksCount, resultsPerPage, page);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void PagenationShould_ReturnCorrectValueWhenPassedPositiveBookCount2()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            int booksCount = 50;
            int resultsPerPage = 15;
            int page = 4;
            Tuple<int, int> expectedResult = new Tuple<int, int>(45, 5);

            // Arrange
            var result = bookService.Pagination(booksCount, resultsPerPage, page);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void PagenationShould_ReturnCorrectValueWhenPassedPositiveBookCount3()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            int booksCount = 50;
            int resultsPerPage = 10;
            int page = 2;
            Tuple<int, int> expectedResult = new Tuple<int, int>(10, 10);

            // Arrange
            var result = bookService.Pagination(booksCount, resultsPerPage, page);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void PagenationShould_ReturnCorrectValueWhenPassedZeroBookCount()
        {
            // Arrange
            var bookService = new BooksService(bookRepoMock.Object, contextMock.Object);
            int booksCount = 0;
            int resultsPerPage = 5;
            int page = 1;
            Tuple<int, int> expectedResult = new Tuple<int, int>(0, 0);

            // Arrange
            var result = bookService.Pagination(booksCount, resultsPerPage, page);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }
    }
}
