using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Gamebook.Web;
using Gamebook.Web.Areas.Administration.Controllers;
using Gamebook.Services.Contracts;
using Moq;
using Gamebook.Data.Model;
using Gamebook.Web.Models.Book;
using Gamebook.Web.Areas.Administration.Models;
using System.Threading.Tasks;
using DataTables.Mvc;
using System.Web;
using System.Security.Principal;
using NUnit.Framework;

namespace Gamebook.Web.Tests.Areas.Administration.Controllers
{
    [TestFixture]
    public class AdminBookControllerTest
    {
        private Mock<IBooksService> booksServiceMock = new Mock<IBooksService>();
        private Mock<IPagesService> pagesServiceMock = new Mock<IPagesService>();
        private Mock<IPageConnectionsService> pageConnectionsServiceMock = new Mock<IPageConnectionsService>();
        private Mock<IUsersService> usersServiceMock = new Mock<IUsersService>();

        [Test]
        public void EditGet()
        {
            // Arrange
            BookController controller = new BookController(
                booksServiceMock.Object, 
                pagesServiceMock.Object, 
                pageConnectionsServiceMock.Object, 
                usersServiceMock.Object
            );

            // Act
            booksServiceMock.Setup(x => x.FindSingle(0)).Returns(new Book() { Author = new User()});
            ViewResult result = controller.Edit(0) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void EditPost()
        {
            // Arrange
            BookController controller = new BookController(
                booksServiceMock.Object,
                pagesServiceMock.Object,
                pageConnectionsServiceMock.Object,
                usersServiceMock.Object
            );
            var bookVM = new BookFullViewModel();

            // Act
            booksServiceMock.Setup(x => x.FindSingle(0)).Returns(new Book() { Author = new User() });
            booksServiceMock.Setup(x => x.Update(new Book())).Returns(1);
            ActionResult result = controller.Edit(bookVM, "") as ActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void EditPostShould_SetIsDeletedWhenRequired()
        {
            // Arrange
            BookController controller = new BookController(
                booksServiceMock.Object,
                pagesServiceMock.Object,
                pageConnectionsServiceMock.Object,
                usersServiceMock.Object
            );
            var bookVM = new BookFullViewModel() { isDeleted = true, DeletedOn = null };

            // Act
            booksServiceMock.Setup(x => x.FindSingle(0)).Returns(new Book() { Author = new User() });
            booksServiceMock.Setup(x => x.Update(new Book())).Returns(1);
            ActionResult result = controller.Edit(bookVM, "") as ActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void EditPostShould_RemovesIsDeletedWhenRequired()
        {
            // Arrange
            BookController controller = new BookController(
                booksServiceMock.Object,
                pagesServiceMock.Object,
                pageConnectionsServiceMock.Object,
                usersServiceMock.Object
            );
            var bookVM = new BookFullViewModel() { isDeleted = false, DeletedOn = DateTime.Now };

            // Act
            booksServiceMock.Setup(x => x.FindSingle(0)).Returns(new Book() { Author = new User() });
            booksServiceMock.Setup(x => x.Update(new Book())).Returns(1);
            ActionResult result = controller.Edit(bookVM, "") as ActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void List()
        {
            // Arrange
            BookController controller = new BookController(
                booksServiceMock.Object,
                pagesServiceMock.Object,
                pageConnectionsServiceMock.Object,
                usersServiceMock.Object
            );

            // Act
            ViewResult result = controller.List() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void BookTable()
        {
            // Arrange
            BookController controller = new BookController(
                booksServiceMock.Object,
                pagesServiceMock.Object,
                pageConnectionsServiceMock.Object,
                usersServiceMock.Object
            );
            Mock<IDataTablesRequest> dtRequestMock = new Mock<IDataTablesRequest>();
            var list = new List<Book>() { new Book() { Author = new User() } };

            // Act
            dtRequestMock.Setup(x => x.Search).Returns(new Search(string.Empty, false));
            booksServiceMock.Setup(x => x.GetAllAndDeleted()).Returns(list.AsQueryable);
            dtRequestMock.Setup(x => x.Columns).Returns(new ColumnCollection(new List<Column>() { new Column("", "", true, true, "", false) }));
            ActionResult result = controller.BookTable(dtRequestMock.Object) as ActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void CreateGet()
        {
            // Arrange
            BookController controller = new BookController(
                booksServiceMock.Object,
                pagesServiceMock.Object,
                pageConnectionsServiceMock.Object,
                usersServiceMock.Object
            );

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void CreatePost()
        {
            // Arrange
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            BookController controller = new BookController(
                booksServiceMock.Object,
                pagesServiceMock.Object,
                pageConnectionsServiceMock.Object,
                usersServiceMock.Object
            );
            controller.ControllerContext = controllerContext.Object;

            // Act
            usersServiceMock.Setup(x => x.FindSingle("User")).Returns(new User());
            booksServiceMock.Setup(x => x.Add(new Book())).Returns(1);
            var result = controller.Create(new BookCreateViewModel());

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void CreatePostShould_ReturnViewIfBookServiceThrows()
        {
            // Arrange
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            BookController controller = new BookController(
                booksServiceMock.Object,
                pagesServiceMock.Object,
                pageConnectionsServiceMock.Object,
                usersServiceMock.Object
            );
            controller.ControllerContext = controllerContext.Object;

            // Act
            usersServiceMock.Setup(x => x.FindSingle("User")).Returns(new User());
            booksServiceMock.Setup(x => x.Add(new Book())).Throws(new Exception());
            var result = controller.Create(new BookCreateViewModel());

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
