using Nito.AsyncEx;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;

using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Services;
using Yichen.Manage.IRepository;
using Yichen.Manage.IServices;
using Yichen.Manage.Model;
using Yichen.System.Model;
using Yichen.Test.Model;

namespace Yichen.Manage.Services
{
    public class CRMServices : BaseServices<comm_samplerecord>, ICRMServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        public readonly IUnitOfWork _UnitOfWork;
        public readonly ICRMHandleRepository _crmHandleRepository;
        public CRMServices(IUnitOfWork unitOfWork
            , ICRMHandleRepository crmHandleRepository
            ) 
        {
            _UnitOfWork = unitOfWork;
            _crmHandleRepository = crmHandleRepository;
        }


        /// <summary>
        /// 客服信息处理
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CRMHandle(commInfoModel<TesthandleModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            commReInfo<commReSampleInfo> commReInfo = new commReInfo<commReSampleInfo>();

            if (info.infos != null && info.infos.Count > 0)
            {
                commReInfo.code = 0;
                List<commReSampleInfo> commReSampleInfo = new List<commReSampleInfo>();
                foreach (TesthandleModel cRMInfohandle in info.infos)
                {
                    commReSampleInfo commReSample = new commReSampleInfo();
                   CRMStateModel crmstate = await _crmHandleRepository.Clienthandle(cRMInfohandle, info.UserName);
                    commReSample.testid = cRMInfohandle.testid;
                    commReSample.barcode = cRMInfohandle.barcode;
                    commReSample.msg = crmstate.msg;
                    commReSample.handleState = crmstate.handleState;
                    commReSample.testState = crmstate.testStateNO;
                    commReSampleInfo.Add(commReSample);

                }
                commReInfo.infos = commReSampleInfo;
            }
            else
            {
                commReInfo.code = 1;
                commReInfo.msg = "未找到需要处理的样本信息。";
            }
            jm.data = commReInfo;
            return jm;
        }
        /// <summary>
        /// 危急值处理
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CrisisHandle(commInfoModel<TesthandleModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            commReInfo<commReSampleInfo> commReInfo = new commReInfo<commReSampleInfo>();
            if (info.infos != null && info.infos.Count > 0)
            {
                commReInfo.code = 1;
                List<commReSampleInfo> commReSampleInfo = new List<commReSampleInfo>();
                foreach (TesthandleModel crmInfohandle in info.infos)
                {
                    commReSampleInfo commReSample = new commReSampleInfo();
                  CRMStateModel crmstate = await _crmHandleRepository.Crisishandle(crmInfohandle, info.UserName);
                   commReSample.testid = crmInfohandle.testid;
                    commReSample.barcode = crmInfohandle.barcode;
                    commReSample.msg = crmstate.msg;
                    commReSample.handleState = crmstate.handleState;
                    commReSample.testState = crmstate.testStateNO;
                    commReSampleInfo.Add(commReSample);

                }
                commReInfo.infos = commReSampleInfo;
            }
            else
            {
                commReInfo.code = 0;
                commReInfo.msg = "未找到需要处理的样本信息。";
            }
            jm.data = commReInfo;
            return jm;
        }
    }
}
