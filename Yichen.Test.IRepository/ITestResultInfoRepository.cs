using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Test.Model.table;

namespace Yichen.Test.IRepository
{
    public interface ITestResultInfoRepository : IBaseRepository<object>
    {

        /// <summary>
        /// 获取检验样本信息
        /// </summary>
        /// <param name="testid"></param>
        /// <returns></returns>
        Task<test_sampleInfo> GetTestSampleInfo(string barcodes);
        /// <summary>
        /// 获取检验样本信息
        /// </summary>
        /// <param name="testid"></param>
        /// <returns></returns>
        Task<test_sampleInfo> GetTestInfo(int testid);
        /// <summary>
        /// 常规检验样本结果
        /// </summary>
        /// <param name="testid">样本id</param>
        /// <param name="itemCodes">项目编号集合</param>
        /// <returns></returns>
        Task<DataTable> GetSampleResult(int testid, string itemCodes);
        /// <summary>
        /// 微生物样本结果
        /// </summary>
        /// <param name="testid"></param>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        Task<DataTable> GetMicrobeInfo(int testid, string itemCodes);
        /// <summary>
        /// 获取微生物项目结果
        /// </summary>
        /// <param name="testid"></param>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        Task<DataTable> GetMicrobeItem(int testid, string itemCodes);

        /// <summary>
        /// 获取特检项目结果
        /// </summary>
        /// <param name="testid"></param>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        Task<DataTable> GetGene(int testid, string itemCodes, string tableName);

        /// <summary>
        /// 插入检验中样本信息,返回插入id
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<long> AddTestInfo(test_sampleInfo infos);
    }
}
