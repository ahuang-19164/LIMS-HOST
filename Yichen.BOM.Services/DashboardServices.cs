using Nito.AsyncEx;
using Npgsql.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yichen.BOM.IServices;
using Yichen.BOM.Model;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Services;
using Yichen.Net.Auth.HttpContextUser;

namespace Yichen.BOM.Services
{
    public class DashboardServices : BaseServices<object>, IDashboardServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IHttpContextUser _httpContextUser;
        private readonly ICommRepository _commRepository;
        public DashboardServices(IUnitOfWork unitOfWork
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

        public async Task<WebApiCallBack> DashboardInfo()
        {
            WebApiCallBack jm = new WebApiCallBack();

            try
            {
                jm.code = 0;
                DashboardModel dashboardModel = new DashboardModel();
                //录入信息数量
                int perinfocount = 0;
                //Pies数据查询sql
                string SqlPies = $"select hospitalNames,COUNT(*) as number from HLIMSDB.WorkPer.SampleInfo where createTime>='{DateTime.Now.ToString("yyyy-MM-dd")}' GROUP BY hospitalNames";


                viewSrouce piesSrouce = new viewSrouce();
                piesSrouce.name = "当日客户送检量";
                DataTable piesDT = await _commRepository.GetTable(SqlPies);
                if(piesDT!=null&&piesDT.Rows.Count>0)
                {
                    List<viewData> piesDatas = new List<viewData>();
                    foreach (DataRow dataRow in piesDT.Rows)
                    {
                        viewData viewData = new viewData();
                        viewData.name = dataRow["hospitalNames"].ToString();
                        viewData.number = dataRow["number"].ToString();
                        perinfocount = perinfocount + Convert.ToInt32(dataRow["number"]);
                        piesDatas.Add(viewData);
                    }
                    piesSrouce.data = piesDatas;





                    //card数据查询sql


                    viewSrouce cardSrouce = new viewSrouce();

                    cardSrouce.name = "当日各节点样本量";

                    string SqlCard = $"select groupNO,testStateNO,names as groupname,1 as no from HLIMSDB.WorkTest.SampleInfo as a  JOIN HLIMSDB.WorkComm.GroupTest as b on a.groupNO=b.NO where a.createTime>='{DateTime.Now.ToString("yyyy-MM-dd")}'";
                    DataTable testDT = await _commRepository.GetTable(SqlCard);


                    //DataTable dtGroupCount = testDT.AsEnumerable().GroupBy(r => new { Material = r["groupname"], TotalQTY = r["no"] }).Select(
                    DataTable dtTestCount = testDT.AsEnumerable().GroupBy(r => new { Material = r["testStateNO"] }).Select(g =>
                    {
                        var row = testDT.NewRow();

                        row["testStateNO"] = g.Key.Material;
                        //row["no"] = g.Key.TotalQTY;
                        row["no"] = g.Sum(r => (int)r["no"]);
                        return row;
                    }).CopyToDataTable();

                    List<viewData> cardDatas = new List<viewData>();

                    viewData percount = new viewData();
                    percount.name = "录入标本数";
                    percount.number = perinfocount.ToString();
                    cardDatas.Add(percount);
                    foreach (DataRow dr in dtTestCount.Rows)
                    {

                        viewData testcount = new viewData();
                        string testStateNO = dr["testStateNO"].ToString();
                        if (testStateNO == "1")
                        {
                            testcount.name = "检验标本数";
                            testcount.number = dr["no"].ToString();
                        }
                        if (testStateNO == "2")
                        {
                            testcount.name = "初审标本数";
                            testcount.number = dr["no"].ToString();
                        }
                        if (testStateNO == "3")
                        {
                            testcount.name = "审核标本数";
                            testcount.number = dr["no"].ToString();
                        }
                        if (testStateNO == "4")
                        {
                            testcount.name = "待检标本数";
                            testcount.number = dr["no"].ToString();
                        }
                        if (testStateNO == "5")
                        {
                            testcount.name = "委托标本数";
                            testcount.number = dr["no"].ToString();
                        }
                        if (testStateNO == "6")
                        {
                            testcount.name = "报告标本数";
                            testcount.number = dr["no"].ToString();
                        }
                        cardDatas.Add(testcount);
                    }
                    cardSrouce.data = cardDatas;




                    //Chart数据查询sql




                    viewSrouce chartSrouce = new viewSrouce();
                    chartSrouce.name = "当日各专业组标本量";

                    DataTable dtGroupCount = testDT.AsEnumerable().GroupBy(r => new { Material = r["groupname"] }).Select(g =>
                    {
                        var row = testDT.NewRow();

                        row["groupname"] = g.Key.Material;
                        //row["no"] = g.Key.TotalQTY;
                        row["no"] = g.Sum(r => (int)r["no"]);
                        return row;
                    }).CopyToDataTable();

                    List<viewData> chartDatas = new List<viewData>();
                    foreach (DataRow dr in dtGroupCount.Rows)
                    {
                        viewData groupcount = new viewData();
                        groupcount.name = dr["groupname"].ToString();
                        groupcount.number = dr["no"].ToString();
                        chartDatas.Add(groupcount);
                    }
                    chartSrouce.data = chartDatas;

                    dashboardModel.piesView = piesSrouce;
                    dashboardModel.chartView = chartSrouce;
                    dashboardModel.cardView = cardSrouce;
                    jm.data = dashboardModel;
                }
                else
                {
                    jm.code = 0;
                    jm.msg = "当日不存在样本信息";
                }

            }
            catch(Exception ex)
            {
                jm.code = 0;
                jm.msg= ex.Message;
            }
            return jm;
        }
    }
}
