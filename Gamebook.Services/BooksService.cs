using Gamebook.Data.Model;
using Gamebook.Data.Repositories.Contracts;
using Gamebook.Data.SaveContext.Contracts;
using Gamebook.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Gamebook.Services
{
    public class BooksService : IBooksService
    {
        private readonly IEfRepository<Book> booksRepo;
        private readonly ISaveContext context;

        public BooksService(IEfRepository<Book> booksRepo, ISaveContext context)
        {
            this.booksRepo = booksRepo;
            this.context = context;
        }

        public IQueryable<Book> GetAll()
        {
            return this.booksRepo.All;
        }

        public IQueryable<Book> FindAll(string searchTerm)
        {
            if (searchTerm == "" || searchTerm == null)
            {
                return this.GetAll();
            }
            else
            {
                int bookCatNum = -1;
                try
                {
                    bookCatNum = int.Parse(searchTerm);
                }
                catch { }
                return this.booksRepo
                    .All
                    .Where(book =>
                        book.Title.Contains(searchTerm)
                        || book.CatalogueNumber == bookCatNum);
            }
        }

        public Book FindSingle(int catalogueNumber)
        {
            return this.booksRepo
                .All
                .Where(book => book.CatalogueNumber == catalogueNumber)
                .First();
        }

        public int Update(Book book)
        {
            this.booksRepo.Update(book);
            return this.context.Commit();
        }
    }
}
