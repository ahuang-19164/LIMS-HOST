using Yichen.Comm.IRepository;

namespace Yichen.Other.IRepository
{
    public interface IRecordRepository : IBaseRepository<object>
    {
        /// <summary>
        /// 检验操作记录
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="operatType"></param>
        /// <param name="record"></param>
        /// <param name="operater"></param>
        /// <param name="clientShow"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task SampleRecord(string barcode, string operatType, string record, string operater, bool clientShow = true, string reason = null);
    }
}
