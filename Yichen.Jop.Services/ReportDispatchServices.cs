using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model.ViewModels.UI; 
using Yichen.Comm.Services;
using Yichen.Jop.IServices;
using Yichen.Net.Configuration;
using Yichen.Net.Caching;

namespace Yichen.Jop.Services
{
    public class ReportDispatchServices : BaseServices<object>, IReportDispatchServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommRepository _commRepository;

        public ReportDispatchServices(
            IUnitOfWork unitOfWork
            ,ICommRepository commRepository
            )
        {
            _unitOfWork = unitOfWork;
            _commRepository = commRepository;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WebApiCallBack> ReportDispatchJOP()
        {
            var jm = new WebApiCallBack();
            #region 分发报告信息
            ///报告生成器数量
            int keyCount = AppSettingsConstVars.ReportCount;


            ///select * from WorkTest.SampleInfo  where realcheckTime>='2022-12-12 13:11:37' and realcheckTime<='2022-12-25 13:11:37' and reportState=0 and testStateNO='3' and dstate=0 and state=1  and hospitalNO in (select no from WorkComm.ClientInfo where reportstate=1)

            string WhereValues = $"realcheckTime>='{DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss")}' and realcheckTime<='{DateTime.Now.AddDays(+3).ToString("yyyy-MM-dd HH:mm:ss")}' and reportState=0 and testStateNO='3' and dstate=0 and state=1  and hospitalNO in (select no from WorkComm.ClientInfo where reportstate=1)";
            string sql = $"select * from WorkTest.SampleInfo  where {WhereValues}";//正常检验报告
            DataTable dt = await _commRepository.GetTable(sql);



            if (dt != null && dt.Rows.Count > 0)
            {
                //JKEntry = ConfigurationManager.AppSettings.GetValues("JKEntry")[0];

                List<string> keys = new List<string>();
                DataSet dataSet = new DataSet();

                for (int a = 0; a < keyCount;)
                {
                    a++;
                    keys.Add($"reportinfo{a}");
                    DataTable reportDT = dt.Clone();
                    reportDT.TableName = $"reportinfo{a}";
                    dataSet.Tables.Add(reportDT);
                }
                //int keyCounts = keyCount - 1;
                int keyCounts = keyCount;
                int rowscount = dt.Rows.Count / keyCounts;
                int i = 1;
                string ids = string.Empty;
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    ids += dt.Rows[r]["id"].ToString() + ",";
                    if (r < rowscount * i)
                    {

                        //dataSet.Tables[i-1].Rows.Add(dt.Rows[r].ItemArray);
                        dataSet.Tables[i - 1].ImportRow(dt.Rows[r]);
                    }
                    else
                    {


                        if (keyCounts > i)
                        {
                            i++;
                            dataSet.Tables[i - 1].ImportRow(dt.Rows[r]);
                        }
                        else
                        {
                            dataSet.Tables[0].ImportRow(dt.Rows[r]);
                        }
                        //dataSet.Tables[i - 1].Rows.Add(dt.Rows[r].ItemArray);


                    }
                }
                int w = 0;
                RedisHelper redisHelper = new RedisHelper(AppSettingsConstVars.RedisReportConnectionString);
                foreach (DataTable dataTable in dataSet.Tables)
                {
                    await redisHelper.ListRightPushAsync(dataSet.Tables[w].TableName, dataSet.Tables[w]);
                    w++;
                }

                //if (!string.IsNullOrEmpty(ids))
                //{
                //    string sqlinsert = $"update WorkTest.SampleInfo set reportState=1  where id in ({ids.Substring(0,ids.Length-1)})";//正常检验报告
                //    SqlHelper.ExecuteNonQuery(sqlconn, CommandType.Text, sqlinsert);
                //}
            }
            else
            {
                jm.code = 1;
                jm.msg = "获取报告信息失败";
            }
            #endregion

            return jm;
        }
    }
}
