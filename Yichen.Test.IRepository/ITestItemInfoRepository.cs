using Yichen.Comm.IRepository;
using Yichen.Test.Model.table;

namespace Yichen.Test.IRepository
{
    /// <summary>
    /// 项目信息处理接口
    /// </summary>
    public interface ITestItemInfoRepository : IBaseRepository<object>
    {
        /// <summary>
        /// 参考值判断
        /// </summary>
        /// <param name="testid">检验中样本id</param>
        /// <param name="defaultValue">是否使用默认结果  true 使用  false  不使用</param>
        /// <returns></returns>
        Task ItemReferenceHandle(test_sampleInfo testSampleInfo, bool defaultValue = true);
        /// <summary>
        /// 危急值处理
        /// </summary>
        /// <returns></returns>
        Task ItemCrisisHandle(int perid, int testid, string barcode, string itemCode, string itemName, string result, string Reference, string CrisisReference, string UserName);

        /// <summary>
        /// 插入检验信息
        /// </summary>
        /// <param name="pairsinfo"></param>
        /// <returns>返回插入信息id</returns>
        Task<int> InsertTestInfo(Dictionary<string,object> pairsinfo);

        /// <summary>
        /// 插入检验项目信息
        /// </summary>
        /// <param name="tablaName">检验结果信息表名</param>
        /// <param name="pairsinfo">插入信息</param>
        /// <returns></returns>
        Task<int> InsertTestItemInfo(int testid, string tablaName, List<Dictionary<string, object>> pairsinfos);


    }
}