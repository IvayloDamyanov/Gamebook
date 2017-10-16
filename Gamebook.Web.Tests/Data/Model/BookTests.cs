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
    public class BookTests
    {
        [Test]
        public void BookpagesGetSet()
        {
            // Arrange & Act
            ICollection<Page> pages = new List<Page>();
            Book book = new Book() { Pages = pages };

            // Assert
            Assert.AreEqual(pages, book.Pages);
        }
    }
}
