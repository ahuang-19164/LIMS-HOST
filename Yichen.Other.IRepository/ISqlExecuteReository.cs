using Yichen.Comm.IRepository;

namespace Yichen.Other.IRepository
{
    public interface ISqlExecuteReository : IBaseRepository<object>
    {
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sqlstring"></param>
        /// <returns></returns>
        Task<int> SqlExecuteCommand(string sqlstring);
    }
}
