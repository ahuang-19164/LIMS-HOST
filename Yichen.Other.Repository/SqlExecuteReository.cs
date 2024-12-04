using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.Other.IRepository;

namespace Yichen.Other.Repository
{
    public class SqlExecuteReository : BaseRepository<object>, ISqlExecuteReository
    {
        public SqlExecuteReository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<int> SqlExecuteCommand(string sqlstring)
        {
            return await DbClient.Ado.ExecuteCommandAsync(sqlstring);
        }
    }
}
