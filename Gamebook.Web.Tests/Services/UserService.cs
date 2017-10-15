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
    public class UserService
    {
        Mock<IEfRepository<User>> userRepoMock = new Mock<IEfRepository<User>>();
        Mock<ISaveContext> contextMock = new Mock<ISaveContext>();
        
        [TestMethod]
        public void GetAllShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<User>();
            var userService = new UsersService(userRepoMock.Object, contextMock.Object);

            // Act
            userRepoMock.Setup(x => x.All).Returns(list.AsQueryable);
            var result = userService.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<User>));
        }

        [TestMethod]
        public void GetAllAndDeletedShould_ReturnCorrectType()
        {
            // Arrang
            var list = new List<User>();
            var userService = new UsersService(userRepoMock.Object, contextMock.Object);

            // Act
            userRepoMock.Setup(x => x.AllAndDeleted).Returns(list.AsQueryable);
            var result = userService.GetAllAndDeleted();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<User>));
        }

        [TestMethod]
        public void FindSingleShould_ReturnCorrectType()
        {
            // Arrange
            string username = "username";
            User user = new User() { UserName = username };
            var list = new List<User>() { user };
            var userService = new UsersService(userRepoMock.Object, contextMock.Object);

            // Act
            userRepoMock.Setup(x => x.AllAndDeleted).Returns(list.AsQueryable);
            var result = userService.FindSingle(username);

            // Assert
            Assert.IsInstanceOfType(result, typeof(User));
        }

        [TestMethod]
        public void FindSingleShould_ReturnCorrectValue()
        {
            // Arrange
            string username1 = "someusername";
            string username2 = "otherusername";
            User user1 = new User() { UserName = username1 };
            User user2 = new User() { UserName = username2 };
            var list = new List<User>() { user1, user2 };
            var userService = new UsersService(userRepoMock.Object, contextMock.Object);

            // Act
            userRepoMock.Setup(x => x.AllAndDeleted).Returns(list.AsQueryable);
            var result = userService.FindSingle(username2);

            // Assert
            Assert.AreEqual(user2, result);
        }

        [TestMethod]
        public void AddShould_ReturnValue()
        {
            // Arrange
            var userService = new UsersService(userRepoMock.Object, contextMock.Object);
            User user = new User();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = userService.Add(user);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void DeleteShould_ReturnValue()
        {
            // Arrange
            var userService = new UsersService(userRepoMock.Object, contextMock.Object);
            User user = new User();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = userService.Delete(user);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void UpdateShould_ReturnValue()
        {
            // Arrange
            var userService = new UsersService(userRepoMock.Object, contextMock.Object);
            User user = new User();

            // Arrange
            contextMock.Setup(x => x.Commit()).Returns(1);
            var result = userService.Update(user);

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }
    }
}
