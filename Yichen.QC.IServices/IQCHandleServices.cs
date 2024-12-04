using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.QC.Model;

namespace Yichen.QC.IServices
{

    /// <summary>
    /// 危急值插入信息接口
    /// </summary>
    public interface IQCHandleServices : IBaseServices<object>
    {
        /// <summary>
        /// 插入一条指控数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<WebApiCallBack> QCResultInsert(commInfoModel<QCAddModel> info);
        /// <summary>
        /// 查询指控数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<WebApiCallBack> QCReultSelect(commInfoModel<QCSelectValueModel> info);
    }
}
