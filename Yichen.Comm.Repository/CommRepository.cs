using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;

namespace Yichen.Comm.Repository
{
    public class CommRepository : BaseRepository<object>, ICommRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IRecordRepository _recordRepository;
        public CommRepository(IUnitOfWork unitOfWork
            //, IRecordRepository recordRepository
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //_recordRepository = recordRepository;
        }
        /// <summary>
        /// 执行sql语句返回datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DataTable> GetTable(string sql)
        {
            return await DbClient.Ado.GetDataTableAsync(sql);
        }


        /// <summary>
        /// 执行sql语句返回dataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DataSet> GetDataSet(string sql)
        {
            return await DbClient.Ado.GetDataSetAllAsync(sql);
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> sqlcommand(string sql)
        {

            return await DbClient.Ado.ExecuteCommandAsync(sql);
        }
    }
}
