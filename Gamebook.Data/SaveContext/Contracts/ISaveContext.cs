using System.Threading.Tasks;

namespace Gamebook.Data.SaveContext.Contracts
{
    public interface ISaveContext
    {
        int Commit();
        Task<int> CommitAsync();
    }
}