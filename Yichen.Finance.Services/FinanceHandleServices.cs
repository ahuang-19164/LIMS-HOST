using Microsoft.AspNetCore.Http;
using Nito.AsyncEx;
using SqlSugar;
using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.Finance.IRepository;
using Yichen.Finance.IServices;
using Yichen.Finance.Model;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Auth.Policys;
using Yichen.Net.Data;
using Yichen.Net.Table;

namespace Yichen.Finance.Services
{
    public class FinanceHandleServices : BaseServices<object>, IFinanceHandleServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IHttpContextUser _httpContextUser;
        private readonly PermissionRequirement _permissionRequirement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommRepository _commRepository;
        private readonly IGroupBillInfoRepository _groupBillInfoRepository;
        public FinanceHandleServices(IUnitOfWork unitOfWork
       , IHttpContextUser httpContextUser
       , PermissionRequirement permissionRequirement
       , IHttpContextAccessor httpContextAccessor
       , ICommRepository commRepository
       ,IGroupBillInfoRepository groupBillInfoRepository
       //,IUserLogServices UserLogServices
       )
        {
            _httpContextUser = httpContextUser;
            _permissionRequirement = permissionRequirement;
            _httpContextAccessor = httpContextAccessor;
            _commRepository = commRepository;
            _groupBillInfoRepository = groupBillInfoRepository;
            //_UserLogServices=UserLogServices;
        }
        /// <summary>
        /// 创建账单流水号
        /// </summary>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> CheckGetSerial()
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                var info =await _groupBillInfoRepository.GetCheckSerial();
                jm.code = 0;
                jm.msg = "读取流水号成功！";
                jm.data = info;
                return jm;
            }
        }
        /// <summary>
        /// 查询收费信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetFinanceInfo(commInfoModel<SelectFinanceModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                foreach (SelectFinanceModel getFinanceModel in infos.infos)
                {
                    string sql = "select * from Finance.groupBillView where checkState=0 and dstate=0 and billdstate=0 ";
                    if (getFinanceModel.PairsInfo != null)
                    {
                        foreach (PairsFinanceModel pairsInfo in getFinanceModel.PairsInfo)
                        {

                            if (pairsInfo.type == "0")
                            {
                                if (pairsInfo.keyNO != null)
                                {

                                    sql += $" and ({pairsInfo.keyName} = '{pairsInfo.keyValue}' or {pairsInfo.keyName.Replace("Names", "NO")} = '{pairsInfo.keyNO}')";

                                }
                                else
                                {
                                    sql += $" and {pairsInfo.keyName} = '{pairsInfo.keyValue}'";
                                }
                            }
                            if (pairsInfo.type == "1")
                            {
                                if (pairsInfo.keyNO != null)
                                {



                                    string[] aaaa = pairsInfo.keyValue.Split(',');

                                    string values = "";
                                    foreach (string a in aaaa)
                                    {
                                        values += $"'{a}',";
                                    }
                                    string[] ssss = pairsInfo.keyNO.Split(',');
                                    string nos = "";
                                    foreach (string a in ssss)
                                    {
                                        nos += $"'{a}',";
                                    }
                                    values = values.Substring(0, values.Length - 1);
                                    nos = nos.Substring(0, nos.Length - 1);

                                    sql += $" and ({pairsInfo.keyName} in ({values}) or {pairsInfo.keyName.Replace("Names", "NO")} in ({nos}))";


                                }
                                else
                                {

                                    string[] aaaa = pairsInfo.keyValue.Split(',');

                                    string values = "";
                                    foreach (string a in aaaa)
                                    {
                                        values += $"'{a}',";
                                    }
                                    values = values.Substring(0, values.Length - 1);
                                    sql += $" and {pairsInfo.keyName} in ({values})";
                                }
                            }
                            if (pairsInfo.type == "2")
                            {
                                if (pairsInfo.keyNO != null)
                                {

                                    sql += $" and ({pairsInfo.keyName} like '%{pairsInfo.keyValue}%' or {pairsInfo.keyName.Replace("Names", "NO")} like '%{pairsInfo.keyNO}%')";

                                }
                                else
                                {
                                    sql += $" and {pairsInfo.keyName} like '%{pairsInfo.keyValue}%'";
                                }
                            }
                            if (pairsInfo.type == "3")
                            {
                                sql += $" and {pairsInfo.keyName} < '{pairsInfo.keyValue}'";
                            }
                            if (pairsInfo.type == "4")
                            {
                                sql += $" and {pairsInfo.keyName} > '{pairsInfo.keyValue}'";
                            }
                            if (pairsInfo.type == "5")
                            {
                                sql += $" and {pairsInfo.keyName} <= '{pairsInfo.keyValue}'";
                            }
                            if (pairsInfo.type == "6")
                            {
                                sql += $" and {pairsInfo.keyName} >= '{pairsInfo.keyValue}'";
                            }
                        }
                    }
                    if (getFinanceModel.perTimeStart != null)
                        sql += $" and createTime >= '{getFinanceModel.perTimeStart}'";
                    if (getFinanceModel.perTimeEnd != null)
                        sql += $" and createTime <= '{getFinanceModel.perTimeEnd}'";
                    if (getFinanceModel.sampleTimeStart != null)
                        sql += $" and sampleTime >= '{getFinanceModel.sampleTimeStart}'";
                    if (getFinanceModel.sampleTimeEnd != null)
                        sql += $" and sampleTime <= '{getFinanceModel.sampleTimeEnd}'";
                    if (getFinanceModel.receiveTimeStart != null)
                        sql += $" and receiveTime >= '{getFinanceModel.receiveTimeStart}'";
                    if (getFinanceModel.receiveTimeEnd != null)
                        sql += $" and receiveTime <= '{getFinanceModel.receiveTimeEnd}'";
                    //sql += " order by hospitalNO,barcode,createTime";
                    DataTable dataTable = await _commRepository.GetTable(sql);
                    if (dataTable.Rows.Count > 0)
                    {
                        jm.code = 0;
                        jm.data = DataTableHelper.DTToString(dataTable);
                    }
                    else
                    {
                        jm.code = 1;
                        jm.msg = "未找到样本信息";
                    }
                }
                return jm;
            }
        }
        /// <summary>
        /// 获取审核账单信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetCheckInfo(commInfoModel<SelectFinanceCheckModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                foreach (SelectFinanceCheckModel getFinanceModel in infos.infos)
                {
                    string clientCodes = "";
                    if (getFinanceModel.ListClientCodes != null && getFinanceModel.checkNo == null)
                    {
                        //string sql = "select department,financeID,barcode,testid,patientTypeNames,patientName,patientSexNames,ageYear,applyItemCodes,applyItemNames,groupNO,groupCode,groupName,standerCharge,settlementCharge,charge,checkState,checkNo,discountState,discount,createTime  from Finance.groupBillView where checkState=0 and dstate=0 and billdstate=0 ";
                        string sql = "select *  from Finance.groupBillView where checkState=0 and dstate=0 and billdstate=0 ";

                        foreach (string pairsInfo in getFinanceModel.ListClientCodes)
                        {
                            clientCodes += $"'{pairsInfo}',";
                        }
                        if (clientCodes.Length > 1)
                        {
                            clientCodes = clientCodes.Substring(0, clientCodes.Length - 1);
                            sql += $" and hospitalNO in ({clientCodes})";
                            if (getFinanceModel.operatTimeStart != null)
                                sql += $" and operatTime >= '{getFinanceModel.operatTimeStart}'";
                            if (getFinanceModel.operatTimeEnd != null)
                                sql += $" and operatTime <= '{getFinanceModel.operatTimeEnd}'";
                            sql += " order by hospitalNO,barcode,createTime";
                            //DataTable b = HLDBSqlHelper.ExecuteDataset(sql).Tables[0];
                            DataTable dataTable = await _commRepository.GetTable(sql);
                            if (dataTable.Rows.Count > 0)
                            {
                                jm.code = 0;
                                jm.data = DataTableHelper.DTToString(dataTable);
                            }
                            else
                            {
                                jm.code = 1;
                                jm.msg = "未找到样本信息";
                            }
                        }
                        else
                        {
                            jm.code = 1;
                            jm.msg = "未找到样本信息";
                        }
                    }
                    else
                    {
                        if (getFinanceModel.ListClientCodes != null && getFinanceModel.checkNo != null)
                        {
                            //string sql = "select department,financeID,barcode,testid,patientTypeNames,patientName,patientSexNames,ageYear,applyItemCodes,applyItemNames,groupNO,groupCode,groupName,standerCharge,settlementCharge,charge,checkState,checkNo,discountState,discount,createTime  from Finance.groupBillView where checkState=1 and dstate=0 and billdstate=0 ";
                            string sql = "select *  from Finance.groupBillView where checkState=1 and dstate=0 and billdstate=0 ";

                            foreach (string pairsInfo in getFinanceModel.ListClientCodes)
                            {
                                clientCodes += $"'{pairsInfo}',";
                            }
                            if (clientCodes.Length > 1)
                            {
                                clientCodes = clientCodes.Substring(0, clientCodes.Length - 1);
                                sql += $" and hospitalNO in ({clientCodes}) and checkNo='{getFinanceModel.checkNo}'";
                                if (getFinanceModel.operatTimeStart != null)
                                    sql += $" and operatTime >= '{getFinanceModel.operatTimeStart}'";
                                if (getFinanceModel.operatTimeEnd != null)
                                    sql += $" and operatTime <= '{getFinanceModel.operatTimeEnd}'";
                                sql += " order by hospitalNO,createTime";
                                DataTable dataTable = await _commRepository.GetTable(sql);
                                if (dataTable.Rows.Count > 0)
                                {
                                    jm.code = 0;
                                    jm.data = DataTableHelper.DTToString(dataTable);
                                }
                                else
                                {
                                    jm.code = 1;
                                    jm.msg = "未找到样本信息";
                                }
                            }
                            else
                            {
                                jm.code = 1;
                                jm.msg = "未找到样本信息";
                            }
                        }
                        else
                        {
                            if (getFinanceModel.checkNo != null)
                            {
                                string sql = "select * from Finance.GroupBillList where  dstate=0";
                                if (getFinanceModel.checkNo != "")
                                    sql += $" and checkNo like '%{getFinanceModel.checkNo}%'";
                                if (getFinanceModel.operatTimeStart != null)
                                    sql += $" and operatTime >= '{getFinanceModel.operatTimeStart}'";
                                if (getFinanceModel.operatTimeEnd != null)
                                    sql += $" and operatTime <= '{getFinanceModel.operatTimeEnd}'";
                                sql += " order by clientNO,operatTime desc";
                                DataTable dataTable = await _commRepository.GetTable(sql);
                                if (dataTable!=null&&dataTable.Rows.Count > 0)
                                {
                                    jm.code = 0;
                                    jm.data = DataTableHelper.DTToString(dataTable);
                                }
                                else
                                {
                                    jm.code = 1;
                                    jm.msg = "未找到样本信息";
                                }
                            }
                            else
                            {
                                jm.code = 1;
                                jm.msg = "未找到样本信息";
                            }
                        }

                    }

                }
                return jm;
            }
        }
        /// <summary>
        /// 获取修改收费信息
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> FinancePrice(commInfoModel<priceChangeModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();


            string sql = "";
            //string sql = "select * from Finance.groupBillView where checkState=0 ";
            if (info.infos != null)
            {
                foreach (priceChangeModel pairsInfo in info.infos)
                {
                    sql += $"update Finance.groupBillView set charge='{pairsInfo.charge}',chargeTypeNO='8' where financeID='{pairsInfo.financeID}';";
                }
            }
            if (sql != "")
            {
                //int b = await DbClient.Ado.ExecuteCommandAsync(sql);
                int b = await _commRepository.sqlcommand(sql);
                if (b > 0)
                {

                    jm.code = 0;
                    jm.msg = "修改成功";
                }
                else
                {
                    jm.code = 1;
                    jm.msg = "修改失败";
                }
            }
            else
            {
                jm.code = 1;
                jm.msg = "未找到需要修改的信息";
            }


            return jm;
        }
        /// <summary>
        /// 账单审核
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BillCheck(commInfoModel<FinanceCheckInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            foreach (FinanceCheckInfoModel financeCheckInfo in info.infos)
            {



                if (financeCheckInfo.clientNO != null && financeCheckInfo.checkNo != "")
                {


                    if (financeCheckInfo.checkNo != null && financeCheckInfo.checkNo.Length > 0)
                    {


                        //string sql = "select * from Finance.groupBillView where ISNULL(item.FinAuditingNo,'''')='''' ";
                        string sql = $"select 1 from Finance.groupBillView where checkState=0 and dstate=0 and billdstate=0 and checkNo='{financeCheckInfo.checkNo}'";
                        DataTable b = await _commRepository.GetTable(sql);
                        if (b.Rows.Count == 0)
                        {


                            string upcheckNo = "";
                            if (financeCheckInfo.ListFinanceID != null)
                            {
                                string financeIDs = "";
                                foreach (string finaceID in financeCheckInfo.ListFinanceID)
                                {
                                    financeIDs += finaceID + ",";
                                }
                                financeIDs = financeIDs.Substring(0, financeIDs.Length - 1);

                                string ListCount = $"select SUM(standerCharge),SUM(settlementCharge),SUM(charge) from  Finance.GroupBillInfo where id in ({financeIDs});";
                                DataTable countDT = await _commRepository.GetTable(ListCount);
                                decimal standerCount = countDT.Rows[0][0] != DBNull.Value ? Convert.ToDecimal(countDT.Rows[0][0]) : 0;
                                decimal settlementCount = countDT.Rows[0][1] != DBNull.Value ? Convert.ToDecimal(countDT.Rows[0][1]) : 0;
                                decimal priceCount = countDT.Rows[0][2] != DBNull.Value ? Convert.ToDecimal(countDT.Rows[0][2]) : 0;




                                upcheckNo += "INSERT INTO Finance.GroupBillList(clientNO,checkNo,standerCount,settlementCount,priceCount,remain,number,operater,operatTime,dstate) VALUES ";
                                upcheckNo += $"('{financeCheckInfo.clientNO}','{financeCheckInfo.checkNo}','{standerCount}','{settlementCount}','{priceCount}','{priceCount}','{financeCheckInfo.ListFinanceID.Count}','{info.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',0);";




                                upcheckNo += $"update Finance.GroupBillInfo set checkState=1,checkNo='{financeCheckInfo.checkNo}',checker='{info.UserName}',checkTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id in ({financeIDs});";
                                int s = await _commRepository.sqlcommand(upcheckNo);
                                if (s > 0)
                                {
                                    jm.code = 0;
                                    jm.msg = $"成功反审核{s}条数据！";
                                }
                                else
                                {
                                    jm.code = 1;
                                    jm.msg = "审核失败！";
                                }

                            }
                            else
                            {
                                jm.code = 0;
                                jm.msg = "审核信息列表为空！";
                            }

                        }
                        else
                        {
                            jm.code = 0;
                            jm.msg = "审核单号重复！";
                        }
                    }
                    else
                    {
                        jm.code = 0;
                        jm.msg = "审核单号不能为空重复！";
                    }
                }
                else
                {
                    jm.code = 0;
                    jm.msg = "客户编号不能为空！";
                }
            }
            return jm;
        }
        /// <summary>
        /// 账单反审核
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BillReCheck(commInfoModel<FinanceCheckInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();


            foreach (FinanceCheckInfoModel financeCheckInfo in info.infos)
            {

                if (financeCheckInfo.checkNo != null && financeCheckInfo.checkNo.Length > 0)
                {
                    if (financeCheckInfo.checkNo != "")
                    {


                        if (financeCheckInfo.ListFinanceID != null)
                        {
                            string financeIDs = "";
                            foreach (string finaceID in financeCheckInfo.ListFinanceID)
                            {
                                financeIDs += finaceID + ",";
                            }
                            financeIDs = financeIDs.Substring(0, financeIDs.Length - 1);



                            string checkNoSql = $" select id,standerCharge,settlementCharge,charge from  Finance.GroupBillInfo where checkNo='{financeCheckInfo.checkNo}' and dstate=0;";
                            DataTable CheckNoDT = await _commRepository.GetTable(checkNoSql);


                            List<DataRow> ListReCheck = CheckNoDT.AsEnumerable().Where<DataRow>(p =>
                            {


                                return financeCheckInfo.ListFinanceID.Contains(p["id"].ToString());


                                //return p["sexNO"].ToString().Trim().Equals(patientSex) && p["ProductId"].ToString().Trim().Equals("4");
                            }).ToList();





                            //DataTable ReCheckDT = CheckNoDT.Select($"id in ({financeIDs})");

                            decimal standerCount = 0;
                            decimal settlementCount = 0;
                            decimal priceCount = 0;

                            foreach (DataRow dataRow in ListReCheck)
                            {
                                if (dataRow["charge"] != DBNull.Value)
                                    priceCount += Convert.ToDecimal(dataRow["charge"]);
                                if (dataRow["standerCharge"] != DBNull.Value)
                                    standerCount += Convert.ToDecimal(dataRow["standerCharge"]);
                                if (dataRow["settlementCharge"] != DBNull.Value)
                                    settlementCount += Convert.ToDecimal(dataRow["settlementCharge"]);
                            }

                            string upcheckNo = $"update Finance.GroupBillInfo set checkState=0,checkNo='',checker='',checkTime='' where id in ({financeIDs});";
                            if (CheckNoDT.Rows.Count == ListReCheck.Count)
                            {
                                upcheckNo += $"update Finance.GroupBillList set number=0,priceCount=0,settlementCount=0,standerCount=0,dstate=1,operater='{info.UserName}',operatTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id in ({financeIDs});";
                            }
                            else
                            {
                                upcheckNo += $"update Finance.GroupBillList set number={CheckNoDT.Rows.Count}-{ListReCheck.Count},priceCount=priceCount-{priceCount},remain=priceCount-{priceCount},settlementCount=settlementCount-{settlementCount},standerCount=standerCount-{standerCount},operater='{info.UserName}',operatTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where checkNo='{financeCheckInfo.checkNo}' and dstate=0;";
                            }

                            int s = await _commRepository.sqlcommand(upcheckNo);
                            if (s > 0)
                            {
                                jm.code = 0;
                                jm.msg = $"成功反审核{s}条数据！";
                            }
                            else
                            {
                                jm.code = 1;
                                jm.msg = "反审核失败！";
                            }

                        }
                        else
                        {
                            jm.code = 1;
                            jm.msg = "反审核信息列表为空！";
                        }
                    }
                    else
                    {
                        jm.code = 1;
                        jm.msg = "客户编号不能为空！";
                    }
                }
                else
                {
                    jm.code = 0;
                    jm.msg = "反审核单号不能为空！";
                }
            }
            return jm;
        }
        /// <summary>
        /// 回款处理
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> FundHandle(commInfoModel<FundInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            string sql = "";
            decimal oldFundCharge = 0;
            decimal subFundCharge = 0;
            decimal NowFundCharge = 0;

            foreach (FundInfoModel item in info.infos)
            {
                string sfundinfo = $"select fundCharge from {tableName} where id={item.id}";
                DataTable DToldFund = await _commRepository.GetTable(sfundinfo);
                if (DToldFund!=null&& DToldFund.Rows.Count>0)
                {
                    oldFundCharge = DToldFund.Rows[0]["fundCharge"] != DBNull.Value ? Convert.ToDecimal(DToldFund.Rows[0]["fundCharge"]) : 0;
                    subFundCharge = item.fundCharge != null ? Convert.ToDecimal(item.fundCharge) : 0;
                    NowFundCharge = oldFundCharge - subFundCharge;
                }

                if (item.fundState == 1)
                {
                    sql += insertFound(item, info.UserName);
                }
                if (item.fundState == 2)
                {
                    sql += updateFound(item, NowFundCharge, info.UserName);
                }
                if (item.fundState == 3)
                {

                    sql += deleteFund(item, oldFundCharge, info.UserName);
                }

            }
            if (sql != "")
            {
                //int a =  HLDBSqlHelper.ExecuteNonQuery(sql);
                int a = await _commRepository.sqlcommand(sql);
                if (a > 0)
                {
                    jm.code = 0;
                    jm.data = NowFundCharge;
                    jm.msg = "数据处理成功";
                }
                else
                {
                    jm.code = 1;
                    jm.msg = "数据处理失败";
                }
            }
            return jm;

        }

        private static string tableName = "Finance.FundInfo";
        /// <summary>
        /// 新增回款记录
        /// </summary>
        /// <param name="fundInfo"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static string insertFound(FundInfoModel fundInfo, string userName)
        {
            //string sql = "";
            if (fundInfo != null && fundInfo.no != 0)
            {
                iInfo Info = new iInfo();
                Info.TableName = tableName;
                Dictionary<string, object> pairs = new Dictionary<string, object>();
                pairs.Add("dstate", 0);
                pairs.Add("state", 1);
                pairs.Add("checkTime", fundInfo.checkTime);
                pairs.Add("fundTime", fundInfo.fundTime);
                pairs.Add("operatTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                pairs.Add("fundCharge", fundInfo.fundCharge);
                //pairs.Add("id", dataTable.Rows[0]["id"]);
                pairs.Add("operater", userName);
                //pairs.Add("operater", dataTable.Rows[0]["operater"]);
                pairs.Add("billNo", fundInfo.billNo);
                pairs.Add("clientNO", fundInfo.clientNO);
                pairs.Add("no", fundInfo.no);
                pairs.Add("person", fundInfo.person);
                pairs.Add("remark", fundInfo.remark);
                Info.values = pairs;
                string a = SqlFormartHelper.insertFormart(Info) + "\r\n";
                a += $"update Finance.GroupBillList set remain=remain-{fundInfo.fundCharge} where id={fundInfo.no}";

                return a;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 修改回款记录
        /// </summary>
        /// <param name="fundInfo"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static string updateFound(FundInfoModel fundInfo, decimal NowFundCharge, string userName)
        {
            //string sql = "";
            if (fundInfo != null && fundInfo.no != 0)
            {

                uInfo Info = new uInfo();
                Info.TableName = tableName;
                Dictionary<string, object> pairs = new Dictionary<string, object>();
                pairs.Add("dstate", 0);
                pairs.Add("state", 1);
                //pairs.Add("checkTime", fundInfo.checkTime);
                pairs.Add("fundTime", fundInfo.fundTime);
                pairs.Add("operatTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                pairs.Add("fundCharge", fundInfo.fundCharge);
                //pairs.Add("id", dataTable.Rows[0]["id"]);
                pairs.Add("operater", userName);
                //pairs.Add("operater", dataTable.Rows[0]["operater"]);
                //pairs.Add("billNo", fundInfo.billNo);
                //pairs.Add("clientNO", fundInfo.clientNO);
                pairs.Add("no", fundInfo.no);
                pairs.Add("person", fundInfo.person);
                pairs.Add("remark", fundInfo.remark);
                Info.values = pairs;
                Info.DataValueID = fundInfo.id;
                string a = SqlFormartHelper.updateFormart(Info) + "\r\n";
                if (NowFundCharge >= 0)
                {
                    a += $"update Finance.GroupBillList set remain=remain+{NowFundCharge} where id={fundInfo.no}";
                }
                else
                {
                    NowFundCharge = Math.Abs(NowFundCharge);
                    a += $"update Finance.GroupBillList set remain=remain-{NowFundCharge} where id={fundInfo.no}";
                }


                return a;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 删除回款记录
        /// </summary>
        /// <param name="fundInfo"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static string deleteFund(FundInfoModel fundInfo, decimal oldFundCharge, string userName)
        {
            if (fundInfo != null && fundInfo.no != 0)
            {

                //decimal NowFundCharge = oldFundCharge - fundInfo.fundCharge;
                uInfo Info = new uInfo();
                Info.TableName = tableName;
                Dictionary<string, object> pairs = new Dictionary<string, object>();
                pairs.Add("dstate", 1);
                pairs.Add("state", 1);
                pairs.Add("operatTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                pairs.Add("fundCharge", fundInfo.fundCharge);
                //pairs.Add("id", dataTable.Rows[0]["id"]);
                pairs.Add("operater", userName);
                Info.values = pairs;
                Info.DataValueID = fundInfo.id;
                string a = SqlFormartHelper.updateFormart(Info) + "\r\n";
                //if (NowFundCharge >= 0)
                //{
                //    a += $"update Finance.GroupBillList set remain=remain-{NowFundCharge} where id={fundInfo.no}";
                //}
                //else
                //{
                a += $"update Finance.GroupBillList set remain=remain+{oldFundCharge} where id={fundInfo.no}";
                //}


                return a;
            }
            else
            {
                return "";
            }
        }



    }
}
