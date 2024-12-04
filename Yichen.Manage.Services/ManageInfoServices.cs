using Nito.AsyncEx;
using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.Finance.IRepository;
using Yichen.Manage.IServices;
using Yichen.Manage.Model;
using Yichen.Net.Table;
using Yichen.Other.IRepository;

namespace Yichen.Manage.Services
{
    public class ManageInfoServices : BaseServices<object>, IManageInfoServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        public readonly IUnitOfWork _UnitOfWork;
        public readonly IRecordRepository _recordRepository;
        //public readonly IFinanceInfoRepository _financeInfoRepository;
        private readonly ICommRepository _commRepository;
        public ManageInfoServices(IUnitOfWork unitOfWork
           , IRecordRepository recordRepository
           // , IFinanceInfoRepository financeInfoRepository
            , ICommRepository commRepository
            //, ICRMHandleRepository crmHandleRepository
            ) 
        {
            _UnitOfWork = unitOfWork;
            _recordRepository = recordRepository;
            //_financeInfoRepository = _financeInfoRepository;
            _commRepository = commRepository;

        }

        public async Task<WebApiCallBack> GetEntrustInfo(ManageInfoModel info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            //DataTable DTInfo = null;
            if (info.sState == 0)
            {
                string selectDelSql = "";
                if (info.StartTime != null && info.EndTime != null)
                {
                    selectDelSql = $"select * from [HLIMSDB].[WorkOther].[delegateInfoView] where ((createTime between '{info.StartTime}' and '{info.EndTime}') or (reachTime between '{info.StartTime}' and '{info.EndTime}')) or delegateStateNO not in ('1','3') and delegateState=1 and state=1 and dstate=0";
                }
               if (info.patientName != null && info.patientName.Trim().Length > 0)
                {
                    selectDelSql += $" and patientName like '%{info.patientName}%'";
                }
                if (info.barcode != null && info.barcode.Trim().Length > 0)
                {
                    selectDelSql += $" and barcode like '%{info.barcode}%'";
                }
                DataTable dataTable= await _commRepository.GetTable(selectDelSql);
                jm.data = DataTableHelper.DTToString(dataTable);
            }
            if (info.sState == 1)
            {
                string selectIHCSql = "";
                if (info.barcode != null && info.barcode.Trim().Length > 0)
                {

                    selectIHCSql = $"select * from [HLIMSDB].[WorkOther].[IHCInfoView] where (createTime between '{info.StartTime}' and '{info.EndTime}')  and handleTypeNO=2 and state=1 and dstate=0  and barcode like '%{info.barcode}%';";
                }
                else
                {
                    selectIHCSql = $"select * from [HLIMSDB].[WorkOther].[IHCInfoView] where (createTime between '{info.StartTime}' and '{info.EndTime}')  and handleTypeNO=2 and state=1 and dstate=0;";
                }
                DataTable dataTable = await _commRepository.GetTable(selectIHCSql);
                jm.data = DataTableHelper.DTToString(dataTable);

            }
            return jm;
        }

        public async Task<WebApiCallBack> GetIHCInfo(ManageInfoModel info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            if (info.sState == 1)
            {
                string selectIHCSql = "";
                if (info.barcode != null && info.barcode.Trim().Length > 0)
                {

                    selectIHCSql = $"select * from [HLIMSDB].[WorkOther].[IHCInfoView] where (createTime between '{info.StartTime}' and '{info.EndTime}')  and handleTypeNO=2 and state=1 and dstate=0  and barcode like '%{info.barcode}%';";
                }
                else
                {
                    selectIHCSql = $"select * from [HLIMSDB].[WorkOther].[IHCInfoView] where (createTime between '{info.StartTime}' and '{info.EndTime}')  and handleTypeNO=2 and state=1 and dstate=0;";
                }
                DataTable dataTable = await _commRepository.GetTable(selectIHCSql);
                jm.data = DataTableHelper.DTToString(dataTable);

            }
            return jm;
        }
    }
}
