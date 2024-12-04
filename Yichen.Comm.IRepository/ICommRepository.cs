using System.Data;

namespace Yichen.Comm.IRepository
{
    public interface ICommRepository : IBaseRepository<object>
    {
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        new Task<int> sqlcommand(string sql);
        /// <summary>
        /// 执行sql语句返回Datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
         new Task<DataTable> GetTable(string sql);
        /// <summary>
        /// 执行sql语句返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        new Task<DataSet> GetDataSet(string sql);
    }
}
