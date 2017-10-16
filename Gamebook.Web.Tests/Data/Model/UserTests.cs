using Gamebook.Data.Model;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamebook.Web.Tests.Data.Model
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void UserBooksGetSet()
        {
            // Arrange & Act
            ICollection<Book> books = new List<Book>();
            User user = new User() { Books = books };

            // Assert
            Assert.AreEqual(books, user.Books);
        }

        [Test]
        public void UserPagesGetSet()
        {
            // Arrange & Act
            ICollection<Page> pages = new List<Page>();
            User user = new User() { Pages = pages };

            // Assert
            Assert.AreEqual(pages, user.Pages);
        }
    }
}
