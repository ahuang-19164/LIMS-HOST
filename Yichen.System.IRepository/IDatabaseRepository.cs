using SqlSugar;
using Yichen.Comm.IRepository;



namespace Yichen.System.IRepository
{
    public interface IDatabaseRepository : IBaseRepository<object>
    {
        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <returns></returns>
        Task<List<DbTableInfo>> GetDbTables();

        /// <summary>
        /// 获取视图表
        /// </summary>
        /// <returns></returns>
        Task<List<DbTableInfo>> GetDbViews();
        /// <summary>
        /// 获取表下面所有的字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<List<DbColumnInfo>> GetDbColumns(string tableName);
    }
}
