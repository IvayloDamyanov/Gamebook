using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace Gamebook.Web.Tests.Controllers
{
    [TestClass]
    public class AdminPageControllerTest
    {
        private Mock<IBooksService> booksServiceMock = new Mock<IBooksService>();
        private Mock<IPagesService> pagesServiceMock = new Mock<IPagesService>();
        private Mock<IPageConnectionsService> pageConnectionsServiceMock = new Mock<IPageConnectionsService>();
        private Mock<IUsersService> usersServiceMock = new Mock<IUsersService>();

        [TestMethod]
        public void EditGet()
        {
            // Arrange
            PageController controller = new PageController(
                booksServiceMock.Object, 
                pagesServiceMock.Object, 
                pageConnectionsServiceMock.Object, 
                usersServiceMock.Object
            );

            // Act
            pagesServiceMock.Setup(x => x.Find(1, 1)).Returns(new Page() { Book = new Book(), Author = new User()});
            ViewResult result = controller.Edit(1, 1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditPost()
        {
            // Arrange
            PageController controller = new PageController(
                booksServiceMock.Object,
                pagesServiceMock.Object,
                pageConnectionsServiceMock.Object,
                usersServiceMock.Object
            );
            var pageVM = new PageFullViewModel() { BookCatNum = 1, Number = 1};
            var page = new Page() { Book = new Book(), Author = new User() };

            // Act
            pagesServiceMock.Setup(x => x.Find(1, 1)).Returns(page);
            pagesServiceMock.Setup(x => x.Update(page)).Returns(1);
            ActionResult result = controller.Edit(pageVM, "") as ActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void List()
        {
            // Arrange
            PageController controller = new PageController(
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

        [TestMethod]
        public void PageTable()
        {
            // Arrange
            PageController controller = new PageController(
                booksServiceMock.Object,
                pagesServiceMock.Object,
                pageConnectionsServiceMock.Object,
                usersServiceMock.Object
            );
            Mock<IDataTablesRequest> dtRequestMock = new Mock<IDataTablesRequest>();
            var list = new List<Page>() { new Page() { Book = new Book(), Author = new User() } };

            // Act
            dtRequestMock.Setup(x => x.Search).Returns(new Search(string.Empty, false));
            pagesServiceMock.Setup(x => x.GetAllAndDeleted()).Returns(list.AsQueryable);
            dtRequestMock.Setup(x => x.Columns).Returns(new ColumnCollection(new List<Column>() { new Column("", "", true, true, "", false) }));
            ActionResult result = controller.PageTable(dtRequestMock.Object) as ActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateGet()
        {
            // Arrange
            PageController controller = new PageController(
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

        [TestMethod]
        public void CreatePost()
        {
            // Arrange
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            PageController controller = new PageController(
                booksServiceMock.Object,
                pagesServiceMock.Object,
                pageConnectionsServiceMock.Object,
                usersServiceMock.Object
            );
            controller.ControllerContext = controllerContext.Object;

            // Act
            usersServiceMock.Setup(x => x.FindSingle("User")).Returns(new User());
            booksServiceMock.Setup(x => x.FindSingle(1)).Returns(new Book());
            pagesServiceMock.Setup(x => x.Add(new Page())).Returns(1);
            ActionResult result = controller.Create(new PageCreateViewModel() { BookCatNum = 1});

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
