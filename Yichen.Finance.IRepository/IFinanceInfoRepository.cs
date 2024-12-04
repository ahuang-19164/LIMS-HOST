using Yichen.Comm.IRepository;
using Yichen.Finance.Model;

namespace Yichen.Finance.IRepository
{
    public interface IFinanceInfoRepository : IBaseRepository<object>
    {
        /// <summary>
        /// 获取指定客户的折扣信息
        /// </summary>
        /// <param name="clientNO"></param>
        /// <returns></returns>
        Task<FinanceClientModel> GetFinanceClient(string clientNO);
        /// <summary>
        /// 获取专业组折扣项目价格
        /// </summary>
        /// <param name="perid"></param>
        /// <param name="groupNO"></param>
        /// <param name="barcode"></param>
        /// <param name="userName"></param>
        /// <param name="agentNO"></param>
        /// <param name="hosNO"></param>
        /// <param name="patientType"></param>
        /// <param name="department"></param>
        /// <param name="groupCode"></param>
        /// <param name="groupName"></param>
        /// <param name="standerPirce"></param>
        /// <param name="settlementPirce"></param>
        /// <param name="chargePice"></param>
        /// <returns></returns>
        Task<DiscountPriceModel> GetGroupPrice(string agentNO, string hosNO, string patientType, string department, string groupCode, string chargeLevel, double discount);



    }
}
