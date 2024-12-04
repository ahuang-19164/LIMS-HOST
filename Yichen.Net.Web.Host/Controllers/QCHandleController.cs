using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.QC.IServices;
using Yichen.QC.Model;

namespace Yichen.Net.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QCHandleController : ControllerBase
    {
        private readonly IQCHandleServices _iQCHandleServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QCHandleController(
            IQCHandleServices iQCHandleServices
            )
        {
            _iQCHandleServices = iQCHandleServices;
        }

        /// <summary>
        /// 插入一条指控数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost, Route("QCResultInsert")][Authorize]
        public async Task<WebApiCallBack> QCResultInsert(commInfoModel<QCAddModel> info)
        {
            return await _iQCHandleServices.QCResultInsert(info);
        }
        /// <summary>
        /// 查询质控数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost, Route("QCReultSelect")][Authorize]
        public async Task<WebApiCallBack> QCReultSelect(commInfoModel<QCSelectValueModel> info)
        {
            return await _iQCHandleServices.QCReultSelect(info);
        }


    }
}
