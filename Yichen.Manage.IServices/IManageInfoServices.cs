using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Manage.Model;

namespace Yichen.Manage.IServices
{
    public interface IManageInfoServices : IBaseServices<object>
    {
        /// <summary>
        /// 获取委托样本信息
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetEntrustInfo(ManageInfoModel info);

        /// <summary>
        /// 获取IHC样本信息(免疫组化)
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetIHCInfo(ManageInfoModel info);


    }
}
