using Gamebook.Data.SaveContext.Contracts;

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
    }
}
