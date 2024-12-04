using Nito.AsyncEx;
using System.Data;
using Yichen.BOM.IServices;
using Yichen.BOM.Model;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Table;

namespace Yichen.BOM.Services
{
    public partial class StatisticServices : BaseServices<object>, IStatisticServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IHttpContextUser _httpContextUser;
        private readonly ICommRepository _commRepository;
        //private readonly PermissionRequirement _permissionRequirement;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IUserLogServices _UserLogServices;

        public StatisticServices(IUnitOfWork unitOfWork
            , IHttpContextUser httpContextUser
            , ICommRepository commRepository
            //,PermissionRequirement permissionRequirement
            //,IHttpContextAccessor httpContextAccessor
            //,IUserLogServices UserLogServices
            )
        {
            _httpContextUser = httpContextUser;
            _commRepository = commRepository;
            //_permissionRequirement = permissionRequirement;
            //_httpContextAccessor = httpContextAccessor;
            //_UserLogServices=UserLogServices;
        }

        /// <summary>
        /// 综合查询
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<WebApiCallBack> StatisticHandle(StatisticModel infos)
      {
            WebApiCallBack jm = new WebApiCallBack();

            string sql = "";
            if (infos.typeName != null && infos.typeName != "")
            {
                switch (infos.typeName)
                {
                    case "perSampleInfo":
                        sql = "select * from WorkPer.SampleInfo where dstate=0 ";
                        sql += insertPairs(infos);
                        break;
                    case "testSampleInfo":
                        sql = "select * from WorkTest.SampleInfo where dstate=0 ";
                        sql += insertPairs(infos);
                        break;
                    case "perWorkStatistic":
                        sql = "select hospitalNO,hospitalNames,creater,count(*) as num from WorkPer.SampleInfo where dstate=0 ";
                        sql += insertPairs(infos);
                        sql += " group by hospitalNO,hospitalNames,creater order by hospitalNO";
                        break;
                    case "testWorkStatistic":
                        sql = "select groupNO,checker,count(*) as num from WorkTest.SampleInfo where dstate=0 ";
                        sql += insertPairs(infos);
                        sql += " group by groupNO,checker order by groupNO";
                        break;
                    case "clientStatistic":
                        sql = "select hospitalNO,hospitalNames,count(*) as num from WorkPer.SampleInfo where dstate=0 ";
                        sql += insertPairs(infos);
                        sql += " group by hospitalNO,hospitalNames order by hospitalNO";
                        break;
                    case "applyStatistic":
                        sql = "select applyItemCodes,applyItemNames,count(*) as num from WorkPer.SampleInfo where dstate=0 ";
                        sql += insertPairs(infos);
                        sql += " group by applyItemCodes,applyItemNames order by applyItemCodes";
                        break;
                    case "groupStatistic":
                        sql = "select groupCode,groupName,count(*) as num from Finance.GroupBillInfo where dstate=0 ";
                        sql += insertPairs(infos);
                        sql += " group by groupCode,groupName order by groupCode";
                        break;
                    case "itemStatistic":
                        sql = "select itemCodes,itemNames,count(*) as num from WorkTest.SampleResult where dstate=0 ";
                        sql += insertPairs(infos);
                        sql += " group by itemCodes,itemNames order by itemCodes";
                        break;
                    case "recordStatistic":
                        sql = "select * from WorkComm.SampleRecord where 1=1 ";
                        sql += insertPairs(infos);
                        //sql += "order by";
                        break;
                    default:
                        sql = "";
                        break;

                }
                if (sql != "")
                {

                    DataTable dataTable = await _commRepository.GetTable(sql);

                    jm.code = 0;
                    jm.data = DataTableHelper.DTToString(dataTable);
                }
                else
                {
                    jm.code = 1;
                    jm.data = "未找到相关信息";
                }
            }
            else
            {
                jm.code = 1;
                jm.data = "查询类型错误";
            }
            return jm;
        }


        private static string insertPairs(StatisticModel infos)
        {
            string sql = "";



            //string sql = "select * from Finance.groupBillView where ISNULL(item.FinAuditingNo,'''')='''' ";
            //string sql = "select * from Finance.groupBillView where dstate=0 ";
            if (infos.PairsInfo != null)
            {
                foreach (PairsStatisticModel pairsInfo in infos.PairsInfo)
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
            if (infos.perTimeStart != null)
                sql += $" and createTime >= '{infos.perTimeStart}'";
            if (infos.perTimeEnd != null)
                sql += $" and createTime <= '{infos.perTimeEnd}'";
            if (infos.sampleTimeStart != null)
                sql += $" and sampleTime >= '{infos.sampleTimeStart}'";
            if (infos.sampleTimeEnd != null)
                sql += $" and sampleTime <= '{infos.sampleTimeEnd}'";
            if (infos.receiveTimeStart != null)
                sql += $" and receiveTime >= '{infos.receiveTimeStart}'";
            if (infos.receiveTimeEnd != null)
                sql += $" and receiveTime <= '{infos.receiveTimeStart}'";
            return sql;
        }


    }
}
