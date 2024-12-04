using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yichen.BOM.Model;
using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;

namespace Yichen.BOM.IServices
{
    public interface IDashboardServices : IBaseServices<object>
    {
        /// <summary>
        /// 首页展示
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> DashboardInfo();
    }
}
