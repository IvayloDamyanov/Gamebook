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
    public class AdminUserControllerTest
    {
        private Mock<IUsersService> usersServiceMock = new Mock<IUsersService>();

        [TestMethod]
        public void EditGet()
        {
            // Arrange
            UserController controller = new UserController(usersServiceMock.Object);
            var list = new List<User>() { new User() { Id = "" } };
            // Act
            usersServiceMock.Setup(x => x.GetAllAndDeleted()).Returns(list.AsQueryable());
            ViewResult result = controller.Edit("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditPost()
        {
            // Arrange
            UserController controller = new UserController(usersServiceMock.Object);
            var user = new UserFullViewModel() { UserName = "user"};

            // Act
            usersServiceMock.Setup(x => x.FindSingle("user")).Returns(new User());
            ActionResult result = controller.Edit(user, "") as ActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void List()
        {
            // Arrange
            UserController controller = new UserController(usersServiceMock.Object);

            // Act
            ViewResult result = controller.List() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UserTable()
        {
            // Arrange
            UserController controller = new UserController(usersServiceMock.Object);
            Mock<IDataTablesRequest> dtRequestMock = new Mock<IDataTablesRequest>();
            var list = new List<User>();

            // Act
            dtRequestMock.Setup(x => x.Search).Returns(new Search(string.Empty, false));
            usersServiceMock.Setup(x => x.GetAllAndDeleted()).Returns(list.AsQueryable);
            dtRequestMock.Setup(x => x.Columns).Returns(new ColumnCollection(new List<Column>() { new Column("", "", true, true, "", false) }));
            ActionResult result = controller.UserTable(dtRequestMock.Object) as ActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateGet()
        {
            // Arrange
            UserController controller = new UserController(usersServiceMock.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePost()
        {
            // Arrange
            //var fakeHttpContext = new Mock<HttpContextBase>();
            //var fakeIdentity = new GenericIdentity("User");
            //var principal = new GenericPrincipal(fakeIdentity, null);

            //fakeHttpContext.Setup(t => t.User).Returns(principal);
            //var controllerContext = new Mock<ControllerContext>();
            //controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            UserController controller = new UserController(usersServiceMock.Object);
            //controller.ControllerContext = controllerContext.Object;

            // Act
            var result = controller.Create(new UserCreateViewModel());

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
