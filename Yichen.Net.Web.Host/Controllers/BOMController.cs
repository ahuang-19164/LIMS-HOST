using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using System.Threading.Tasks;
using Yichen.BOM.IServices;
using Yichen.BOM.Model;
using Yichen.Comm.Model.ViewModels.UI;

namespace Yichen.Net.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  public class BOMController : ControllerBase
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        //private readonly IUserServices _userServices;
        private readonly IStatisticServices _statisticServices;
        private readonly IDashboardServices _dashboardServices;
        //private readonly IItemBLSaveServices _itemBLSaveServices;




        /// <summary>
        /// 构造函数
        /// </summary>
        public BOMController(
            IStatisticServices statisticServices
            , IDashboardServices dashboardServices
            //, IItemBLSaveServices itemBLSaveServices
            )
        {
            _statisticServices = statisticServices;
            _dashboardServices = dashboardServices;
        }
        /// <summary>
        /// 综合查询
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("GetStatistic")][Authorize]
        public async Task<WebApiCallBack> GetStatistic(StatisticModel infos)
        {
            return await _statisticServices.StatisticHandle(infos);
        }
        /// <summary>
        /// 首页报表  http://localhost:9600/api/BOM/GetDashboard
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        //[HttpPost, Route("GetDashboard")][Authorize]
        [HttpPost, Route("GetDashboard")][Authorize]
        public async Task<WebApiCallBack> GetDashboard()
        {
            return await _dashboardServices.DashboardInfo();
        }

    }
}
