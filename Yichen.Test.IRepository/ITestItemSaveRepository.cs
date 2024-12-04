using Yichen.Comm.IRepository;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Test.Model.Result;

namespace Yichen.Test.IRepository
{
    public interface ITestItemSaveRepository : IBaseRepository<object>
    {
        /// <summary>
        /// 检验结果保存
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> Test(AutographInfo autographInfo, int perid, int testid, string barcode, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);
        /// <summary>
        /// 检验结果初审
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>

        Task<WebApiCallBack> reTest(AutographInfo autographInfo, int perid, int testid, string barcode, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);
        /// <summary>
        /// 检验结果审核
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> Check(AutographInfo autographInfo, int perid, int testid, string barcode, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);
        /// <summary>
        /// 检验结果反审核
        /// </summary>
        /// <param name="perid"></param>
        /// <param name="testid"></param>
        /// <param name="barcode"></param>
        /// <param name="operter"></param>
        /// <param name="nextFlowNO"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ReCheck(string perid, string testid, string barcode, string operter,int report=0, string nextFlowNO = "0");

        ///// <summary>
        ///// 批量检验结果保存
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> batchTest(AutographInfo autographInfo, string perid, string testid, string barcode, string groupCodes, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);
        ///// <summary>
        ///// 批量检验结果初审
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>

        //Task<WebApiCallBack> batchReTest(AutographInfo autographInfo, string perid, string testid, string barcode, string groupCodes, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);
        ///// <summary>
        ///// 批量检验结果审核
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> batchCheck(AutographInfo autographInfo, string perid, string testid, string barcode, string groupCodes, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);


        ///// <summary>
        ///// 检验结果保存
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> BLTest(AutographInfo autographInfo, int perid, int testid, string barcode, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);
        ///// <summary>
        ///// 检验结果初审
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>

        //Task<WebApiCallBack> BLreTest(AutographInfo autographInfo, int perid, int testid, string barcode, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);
        ///// <summary>
        ///// 检验结果审核
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> BLCheck(AutographInfo autographInfo, int perid, int testid, string barcode, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);

        ///// <summary>
        ///// 批量检验结果保存
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> BLbatchTest(AutographInfo autographInfo, string perid, string testid, string barcode, string groupCodes, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);
        ///// <summary>
        ///// 批量检验结果初审
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>

        //Task<WebApiCallBack> BLbatchReTest(AutographInfo autographInfo, string perid, string testid, string barcode, string groupCodes, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);
        ///// <summary>
        ///// 批量检验结果审核
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> BLbatchCheck(AutographInfo autographInfo, string perid, string testid, string barcode, string groupCodes, string operter, string nextFlowNO = "0", int pathologyStateNO = 0);
        ///// <summary>
        ///// 修改检验表检验状态
        ///// </summary>
        ///// <param name="testid"></param>
        ///// <param name="perstate"></param>
        ///// <returns></returns>
        //Task TestSampleStateChange(string testid, int perstate);



    }
}
