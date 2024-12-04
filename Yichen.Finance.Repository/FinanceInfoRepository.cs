using Microsoft.AspNetCore.Http;
using Nito.AsyncEx;
using Nito.Disposables.Internals;
using System.Data;
using Yichen.Comm.IRepository.UnitOfWork;

using Yichen.Comm.Repository;
using Yichen.Finance.IRepository;
using Yichen.Finance.Model;
using Yichen.Finance.Model.table;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Auth.Policys;
using Yichen.Net.Caching.Manuals;
using Yichen.Net.Data;
using Yichen.System.IRepository;
using Yichen.System.Model;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECCouponCreateRequest.Types;

namespace Yichen.Finance.Repository
{
    public class FinanceInfoRepository : BaseRepository<object>, IFinanceInfoRepository
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextUser _httpContextUser;
        //private readonly PermissionRequirement _permissionRequirement;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IClientInfoRepository _clientInfoRepository;
        //private readonly IFinanceChargeRepository _financeChargeRepository;

        public FinanceInfoRepository(IUnitOfWork unitOfWork
       , IHttpContextUser httpContextUser
       //, PermissionRequirement permissionRequirement
       //, IHttpContextAccessor httpContextAccessor
        //,IUserLogServices UserLogServices
        ,IClientInfoRepository clientInfoRepository
        , IFinanceChargeRepository financeChargeRepository
       ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _httpContextUser = httpContextUser;
            //_permissionRequirement = permissionRequirement;
            //_httpContextAccessor = httpContextAccessor;
            //_UserLogServices=UserLogServices;
            //_clientInfoRepository = clientInfoRepository;
            //_financeChargeRepository = financeChargeRepository;
        }



        /// <summary>
        /// 获取客户收费信息
        /// </summary>
        /// <param name="clientNO"></param>
        /// <returns></returns>

        public async new Task<FinanceClientModel> GetFinanceClient(string clientNO)
        {
            FinanceClientModel financeClientModel = new FinanceClientModel();
            List<comm_client_info> clientinfos = ManualDataCache<comm_client_info>.MemoryCache.LIMSGetKeyValue(CommInfo.clientinfo);
            //List<comm_client_info> clientinfos =await _clientInfoRepository.GetCaChe();

            await Task.Run(() =>
            {
                var clientinfo = clientinfos.First(p => p.no == clientNO);
                if (clientinfo != null)
                {
                    financeClientModel.chargeLevelNO = clientinfo.chargeLevelNO;
                    financeClientModel.discount = Convert.ToDouble(!string.IsNullOrEmpty(clientinfo.discount) ? clientinfo.discount.ToString().Replace('。', '.') : "1");
                    financeClientModel.personNO = clientinfo.personNO != null ? clientinfo.personNO.ToString() : "";
                }
            });

            return financeClientModel;

        }

        /// <summary>
        /// 获取组合项目匹配的收费信息
        /// </summary>
        /// <param name="agentNO"></param>
        /// <param name="hosNO"></param>
        /// <param name="patientType"></param>
        /// <param name="department"></param>
        /// <param name="groupCode"></param>
        /// <param name="chargeLevel"></param>
        /// <param name="discount"></param>
        /// <returns></returns>

        public async new Task<DiscountPriceModel> GetGroupPrice(string agentNO, string hosNO, string patientType, string department, string groupCode, string chargeLevel, double discount)
       {
            //_unitOfWork.BeginTran();
            DiscountPriceModel discountPriceModel = new DiscountPriceModel();

            await Task.Run(async () =>
            {
                string priceInfo = "";

                //bool groupDiscountState = ItemPrineSearch.GroupPrice(agentNO, hosNO, chargeLevelNO, patientType, department, groupCode, out string chargeTypeNO, out decimal standarCharge, out decimal settlementCharge);
                bool disPriceState = false;//折扣状态false不参与折扣，true 参与折扣
                string chargeType = "0";//收费类型
                decimal standarCharge = 0;//标准收费价格
                decimal settlementCharge = 0;//结算收费价格
                try
                {
                    ////获取组合项目的价格信息
                    //string GroupSql = $"select * from Finance.GroupChargeInfo  with(updlock) where groupCode='{groupCode}' and state=1;";
                    //DataTable groupPirceDT = DbClient.Ado.GetDataTable(GroupSql);

                    //读取内存中的组合项目的收费信息
                    List<finance_group_charge> GroupChargess = ManualDataCache<finance_group_charge>.MemoryCache.LIMSGetKeyValue(CommInfo.financeGroup);
                    //List<finance_group_charge> GroupChargess =await _financeChargeRepository.GetCaChe();
                    //读取匹配的组合项目收费信息
                    List<finance_group_charge> finance_Group_Charges = GroupChargess.Where(p => p.groupCode == groupCode && p.state == true).ToList();

                    //DataRow groupDR = groupPirceDR(groupPirceDT, agentNO, hosNO, chargeLevel, patientType, department);

                    finance_group_charge finance_Group_Charge = groupPirceDR(finance_Group_Charges, agentNO, hosNO, chargeLevel, patientType, department);

                    if (finance_Group_Charge != null)
                    {
                        standarCharge = finance_Group_Charge.standardCharge != null ? Convert.ToDecimal(finance_Group_Charge.standardCharge) : 0;
                        settlementCharge = finance_Group_Charge.settlementCharge != null ? Convert.ToDecimal(finance_Group_Charge.settlementCharge) : 0;
                        disPriceState = finance_Group_Charge.discountState != null ? Convert.ToBoolean(finance_Group_Charge.discountState) : false;
                        if (standarCharge == 0 && settlementCharge == 0)
                        {
                            chargeType = "3";
                        }
                        else
                        {
                            if (disPriceState)
                            {
                                chargeType = "1";
                            }
                            else
                            {
                                chargeType = "2";
                            }
                        }
                    }
                    else
                    {
                        chargeType = "0";
                        standarCharge = 0;
                        settlementCharge = 0;
                    }

                }
                catch
                {
                    chargeType = "0";
                    standarCharge = 0;
                    settlementCharge = 0;
                }

                discountPriceModel.chargeTypeNO = chargeType;
                discountPriceModel.groupDiscountState = disPriceState;
                discountPriceModel.standerPirce = standarCharge;
                discountPriceModel.settlementPirce = settlementCharge;
                discountPriceModel.chargePice = 0;
                double settlementCharges = Convert.ToDouble(settlementCharge);
                if (disPriceState)
                {
                    discountPriceModel.chargePice = Convert.ToDecimal(settlementCharges * discount);
                }
                else
                {
                    discountPriceModel.chargePice = Convert.ToDecimal(settlementCharges);
                }
                if (discountPriceModel.chargeTypeNO == "0")
                {
                    //未备案收费价格
                    if (discountPriceModel.chargePice == 0 && standarCharge == 0)
                    {
                        discountPriceModel.chargeTypeNO = "4";
                    }
                    else
                    {
                        if (standarCharge == discountPriceModel.chargePice)
                        {
                            discountPriceModel.chargeTypeNO = "5";
                        }
                    }
                }
            });
            return discountPriceModel;
        }
        /// <summary>
        /// 过滤样本信息对应价格信息
        /// </summary>
        /// <param name="groupPirceDT"></param>
        /// <param name="agentNO"></param>
        /// <param name="hosNO"></param>
        /// <param name="chargeLevel"></param>
        /// <param name="patientType"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        private static DataRow groupPirceDR(DataTable groupPirceDT, string agentNO, string hosNO, string chargeLevel, string patientType, string department)
        {
            if (groupPirceDT != null && groupPirceDT.Rows.Count > 0)
            {
                DataRow[] dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='{hosNO}' and chargeLevelNO='{chargeLevel}' and patientTypeNO='{patientType}' and department='{department}'");

                #region 一个参数

                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='{hosNO}' and chargeLevelNO='{chargeLevel}' and patientTypeNO='{patientType}' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='' and chargeLevelNO='{chargeLevel}' and patientTypeNO='{patientType}' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='{hosNO}' and chargeLevelNO='' and patientTypeNO='{patientType}' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='{hosNO}' and chargeLevelNO='{chargeLevel}' and patientTypeNO='' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='{hosNO}' and chargeLevelNO='{chargeLevel}' and patientTypeNO='{patientType}' and department=''");

                #endregion

                #region 两个参数
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='' and chargeLevelNO='{chargeLevel}' and patientTypeNO='{patientType}' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='{hosNO}' and chargeLevelNO='' and patientTypeNO='{patientType}' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='{hosNO}' and chargeLevelNO='{chargeLevel}' and patientTypeNO='' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='{hosNO}' and chargeLevelNO='{chargeLevel}' and patientTypeNO='{patientType}' and department=''");

                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='' and chargeLevelNO='' and patientTypeNO='{patientType}' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='' and chargeLevelNO='{chargeLevel}' and patientTypeNO='' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='' and chargeLevelNO='{chargeLevel}' and patientTypeNO='{patientType}' and department=''");


                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='{hosNO}' and chargeLevelNO='' and patientTypeNO='' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='{hosNO}' and chargeLevelNO='' and patientTypeNO='{patientType}' and department=''");


                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='{hosNO}' and chargeLevelNO='{chargeLevel}' and patientTypeNO='' and department=''");

                #endregion

                #region 三个参数
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='' and chargeLevelNO='' and patientTypeNO='{patientType}' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='' and chargeLevelNO='{chargeLevel}' and patientTypeNO='' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='' and chargeLevelNO='{chargeLevel}' and patientTypeNO='{patientType}' and department=''");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='{hosNO}' and chargeLevelNO='' and patientTypeNO='' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='{hosNO}' and chargeLevelNO='' and patientTypeNO='{patientType}' and department=''");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='{hosNO}' and chargeLevelNO='{chargeLevel}' and patientTypeNO='' and department=''");



                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='' and chargeLevelNO='' and patientTypeNO='' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='' and chargeLevelNO='' and patientTypeNO='{patientType}' and department=''");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='' and chargeLevelNO='{chargeLevel}' and patientTypeNO='' and department=''");


                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='{hosNO}' and chargeLevelNO='' and patientTypeNO='' and department=''");

                #endregion

                #region 四个参数

                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='' and chargeLevelNO='' and patientTypeNO='' and department='{department}'");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='' and chargeLevelNO='' and patientTypeNO='{patientType}' and department=''");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='' and chargeLevelNO='{chargeLevel}' and patientTypeNO='' and department=''");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='' and hospitalNO='{hosNO}' and chargeLevelNO='' and patientTypeNO='' and department=''");
                if (dataRows.Count() != 1)
                    dataRows = groupPirceDT.Select($"agentNO='{agentNO}' and hospitalNO='' and chargeLevelNO='' and patientTypeNO='' and department=''");



                #endregion

                if (dataRows.Count() == 1)
                {
                    return dataRows[0];
                }
                else
                {
                    return groupPirceDT.Rows[0];
                }
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 过滤样本信息对应价格信息
        /// </summary>
        /// <param name="groupPirceDT"></param>
        /// <param name="agentNO"></param>
        /// <param name="hosNO"></param>
        /// <param name="chargeLevel"></param>
        /// <param name="patientType"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        private static finance_group_charge groupPirceDR(List<finance_group_charge> groupPirces, string agentNO, string hosNO, string chargeLevel, string patientType, string department)
        {
            if (groupPirces != null && groupPirces.Count > 0)
            {
                //List<finance_group_charge> dataRows = groupPirces.Select($"agentNO='{agentNO}' and hospitalNO='{hosNO}' and chargeLevelNO='{chargeLevel}' and patientTypeNO='{patientType}' and department='{department}'");
                List<finance_group_charge> dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == hosNO && p.chargeLevelNO == chargeLevel && p.patientTypeNO == patientType && p.department == department).ToList();
                #region 一个参数

                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == hosNO && p.chargeLevelNO == chargeLevel && p.patientTypeNO == patientType && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == "" && p.chargeLevelNO == chargeLevel && p.patientTypeNO == patientType && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == hosNO && p.chargeLevelNO == "" && p.patientTypeNO == patientType && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == hosNO && p.chargeLevelNO == chargeLevel && p.patientTypeNO == "" && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == hosNO && p.chargeLevelNO == chargeLevel && p.patientTypeNO == patientType && p.department == "").ToList();
                #endregion

                #region 两个参数
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == "" && p.chargeLevelNO == chargeLevel && p.patientTypeNO == patientType && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == hosNO && p.chargeLevelNO == "" && p.patientTypeNO == patientType && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == hosNO && p.chargeLevelNO == chargeLevel && p.patientTypeNO == "" && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == hosNO && p.chargeLevelNO == chargeLevel && p.patientTypeNO == patientType && p.department == "").ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == "" && p.chargeLevelNO == "" && p.patientTypeNO == patientType && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == "" && p.chargeLevelNO == chargeLevel && p.patientTypeNO == "" && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == "" && p.chargeLevelNO == chargeLevel && p.patientTypeNO == patientType && p.department == "").ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == hosNO && p.chargeLevelNO == "" && p.patientTypeNO == "" && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == hosNO && p.chargeLevelNO == "" && p.patientTypeNO == patientType && p.department == "").ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == hosNO && p.chargeLevelNO == chargeLevel && p.patientTypeNO == "" && p.department == "").ToList();

                #endregion

                #region 三个参数
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == "" && p.chargeLevelNO == "" && p.patientTypeNO == patientType && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == "" && p.chargeLevelNO == chargeLevel && p.patientTypeNO == "" && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == "" && p.chargeLevelNO == chargeLevel && p.patientTypeNO == patientType && p.department == "").ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == hosNO && p.chargeLevelNO == "" && p.patientTypeNO == "" && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == hosNO && p.chargeLevelNO == "" && p.patientTypeNO == patientType && p.department == "").ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == hosNO && p.chargeLevelNO == chargeLevel && p.patientTypeNO == "" && p.department == "").ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == "" && p.chargeLevelNO == "" && p.patientTypeNO == "" && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == "" && p.chargeLevelNO == "" && p.patientTypeNO == patientType && p.department == "").ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == "" && p.chargeLevelNO == chargeLevel && p.patientTypeNO == "" && p.department == "").ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == hosNO && p.chargeLevelNO == "" && p.patientTypeNO == "" && p.department == "").ToList();

                #endregion

                #region 四个参数
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == "" && p.chargeLevelNO == "" && p.patientTypeNO == "" && p.department == department).ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == "" && p.chargeLevelNO == "" && p.patientTypeNO == patientType && p.department == "").ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == "" && p.chargeLevelNO == chargeLevel && p.patientTypeNO == "" && p.department == "").ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == "" && p.hospitalNO == hosNO && p.chargeLevelNO == "" && p.patientTypeNO == "" && p.department == "").ToList();
                if (dataRows.Count() != 1)
                    dataRows = groupPirces.Where(p => p.agentNO == agentNO && p.hospitalNO == "" && p.chargeLevelNO == "" && p.patientTypeNO == "" && p.department == "").ToList();
                #endregion

                if (dataRows.Count() == 1)
                {
                    return groupPirces.First();
                }
                else
                {

                    return null;

                }
            }
            else
            {

                return null;

            }

        }

    }
}
