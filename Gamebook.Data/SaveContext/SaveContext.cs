using Gamebook.Data.SaveContext.Contracts;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Gamebook.Data.SaveContext
{
    public class SaveContext : ISaveContext
    {
        private readonly MsSqlDbContext context;

        public SaveContext(MsSqlDbContext context)
        {
            this.context = context;
        }

        public int Commit()
        {
            int result = 0;
            try
            {
                result = this.context.SaveChanges();
            }
            catch
            {
                result = -1;
            }
            return result;
        }

        public Task<int> CommitAsync()
        {
            try
            {
                return this.context.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}
