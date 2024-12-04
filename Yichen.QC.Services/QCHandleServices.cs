using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.QC.IServices;
using Yichen.QC.Model;

namespace Yichen.QC.Services
{
    /// <summary>
    /// 危急值信息处理
    /// </summary>
    public class QCHandleServices : BaseServices<object>, IQCHandleServices
    {
        //private readonly AsyncLock _mutex = new AsyncLock();
        public readonly IUnitOfWork _unitOfWork;
        public readonly IQCResultJudge _qCResultJudge;
        private readonly ICommRepository _commRepository;
        //public readonly IRecordRepository _recordRepository;
        //public readonly IItemCrisisRepository _itemCrisisRepository;
        public QCHandleServices(IUnitOfWork unitOfWork
            , IQCResultJudge qCResultJudge
            , ICommRepository commRepository
            //, IRecordRepository recordRepository
            //, IItemCrisisRepository itemCrisisRepository
            )
        {
            _unitOfWork = unitOfWork;
            _qCResultJudge = qCResultJudge;
            _commRepository = commRepository;
            //_recordRepository = recordRepository;
            //_itemCrisisRepository = itemCrisisRepository;
        }

        public async Task<WebApiCallBack> QCResultInsert(commInfoModel<QCAddModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            commReInfo<commReItemInfo> commReInfo = new commReInfo<commReItemInfo>();
            List<commReItemInfo> commReItemInfos = new List<commReItemInfo>();
            if (info.infos != null)
            {
                string sql = "";
                foreach (QCAddModel result in info.infos)
                {
                    commReItemInfo commReItemInfo = new commReItemInfo();
                    if (result.qcTime != null && result.planid != null && result.planItemid != null && result.planGradeid != null && result.itemNO != null && result.itemResult != null && result.sort != 0)
                    {
                        sql = $"update QC.QCItemResult set dstate=1 where qcTime='{result.qcTime}' and sort='{result.sort}' and itemNO='{result.itemNO}' and planGradeid='{result.planGradeid}';\r\n";
                        sql += "insert into QC.QCItemResult(planid, planItemid, planGradeid, itemNO, itemResult, resultState, remark, sort, qcTime, creater, createTime, state, dstate) values";
                        sql += $"('{result.planid}', N'{result.planItemid}', N'{result.planGradeid}', N'{result.itemNO}', N'{result.itemResult}', N'0', N'{result.remark}', N'{result.sort}', N'{result.qcTime}', N'{info.UserName}', N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', N'1', N'0')";

                        int a = await _commRepository.sqlcommand(sql);
                        if (a > 0)
                        {

                            commReItemInfo.ItemNO = result.itemNO;
                            commReItemInfo.ItemMsg = "更新成功";
                            //commReItemInfo.ItemMsg =;
                            commReItemInfo.code = 0;
                            commReItemInfos.Add(commReItemInfo);
                            //QCItemInfo.produceQCInfo(result.planid, result.planGradeid, result.itemNO, result.sort,"","");

                            await _qCResultJudge.NewRestultJudge(result.planid, result.planGradeid, result.itemNO, result.sort);

                        }
                        else
                        {

                            commReItemInfo.ItemNO = result.itemNO;
                            commReItemInfo.ItemMsg = "更新失败";
                            //commReItemInfo.ItemMsg =;
                            commReItemInfo.state = 0;
                        }
                    }
                    else
                    {
                        commReItemInfo.ItemNO = result.itemNO;
                        commReItemInfo.ItemMsg = "提交信息不完整";
                        //commReItemInfo.ItemMsg =;
                        commReItemInfo.state = 0;
                    }

                }
                if (commReItemInfos.Count > 0)
                {
                    jm.code = 0;
                    jm.msg = "提交成功";
                }
                else
                {
                    jm.code = 1;
                    jm.msg = "提交失败";
                }
            }
            else
            {
                jm.code = 1;
                jm.msg = "提交项目信息为空";
            }
            return jm;
        }

        public async Task<WebApiCallBack> QCReultSelect(commInfoModel<QCSelectValueModel> info)
        {

            WebApiCallBack jm = new WebApiCallBack();
            DataSet dataSet = new DataSet();
            foreach (QCSelectValueModel qCSelectValue in info.infos)
            {
                DataTable REDTQCItemInfo = new DataTable();

                REDTQCItemInfo.Columns.Add("gradeid", typeof(int));//质控品id
                REDTQCItemInfo.Columns.Add("gradecode", typeof(string));//质控品编号
                REDTQCItemInfo.Columns.Add("gradename", typeof(string));//质控品名称
                REDTQCItemInfo.Columns.Add("gradeproducer", typeof(string));//质控厂家
                REDTQCItemInfo.Columns.Add("gradelevelNO", typeof(string));//质控水平
                REDTQCItemInfo.Columns.Add("gradeavgValue", typeof(double));//质控 Avg值
                REDTQCItemInfo.Columns.Add("gradesdValue", typeof(double));//质控SD值
                REDTQCItemInfo.Columns.Add("gradecvValue", typeof(double));//质控CV值
                REDTQCItemInfo.Columns.Add("selavgValue", typeof(double));//当前平均值
                REDTQCItemInfo.Columns.Add("selsdValue", typeof(double));//当前SD值
                REDTQCItemInfo.Columns.Add("selcvValue", typeof(double)); //当前CV值
                REDTQCItemInfo.Columns.Add("selNum", typeof(int));//当前数量
                                                                  //REDTQCItemInfo.Columns.Add("upavgValue", typeof(double));
                                                                  //REDTQCItemInfo.Columns.Add("upsdValue", typeof(double));
                                                                  //REDTQCItemInfo.Columns.Add("upcvValue", typeof(double));
                                                                  //REDTQCItemInfo.Columns.Add("upNum", typeof(int));
                REDTQCItemInfo.Columns.Add("accavgValue", typeof(double));//累计平均值
                REDTQCItemInfo.Columns.Add("accsdValue", typeof(double));//累计SD值
                REDTQCItemInfo.Columns.Add("acccvValue", typeof(double));//累计CV值
                REDTQCItemInfo.Columns.Add("accNum", typeof(int));//累计数量
                REDTQCItemInfo.Columns.Add("gradeerror", typeof(string));//允许误差
                REDTQCItemInfo.Columns.Add("gradeerrorTEa", typeof(string));//允许TEa
                REDTQCItemInfo.Columns.Add("gradefactoryX", typeof(string));//允许靶值
                REDTQCItemInfo.Columns.Add("gradefactoryRange", typeof(string));//靶值范围

                //string sql = $"select planGradeid from QC.QCResultView where planid='{planid}' and planItemNo='{itemNO}' and itemResultdstate=0 group by planGradeid order by planGradesort;";
                string sql1 = $"select * from QC.QCResultView where planid='{qCSelectValue.planid}' and planItemid='{qCSelectValue.planItemid}' and itemResultdstate=0 order by planGradelevelNO,qcTime;";
                string sql2 = $"select * from QC.HandleRecord where planid='{qCSelectValue.planid}' and planItemid='{qCSelectValue.planItemid}' and qcTime>='{qCSelectValue.startTime}' and qcTime<='{qCSelectValue.endTime}' and dstate=0 order by qcTime,handleTime";
                //DataSet DTQCInfo = _commRepository.GetTable.ExecuteDataset(sql);

                DataTable DTQCItemInfo = await _commRepository.GetTable(sql1);
                DataTable DTQCHandleInfo = await _commRepository.GetTable(sql2);



                DataRow[] DRQCSelectItemInfo = DTQCItemInfo.Select($"qcTime>='{qCSelectValue.startTime}' and qcTime<='{qCSelectValue.endTime}'");

                if (DRQCSelectItemInfo.Count() > 0)
                {
                    //List<DataRow> ListReCheck = DTQCItemInfo.AsEnumerable().Where<DataRow>(p =>
                    //{

                    //    return infos.ListFinanceID.Contains(p["id"].ToString());

                    //    //return p["sexNO"].ToString().Trim().Equals(patientSex) && p["ProductId"].ToString().Trim().Equals("4");
                    //}).ToList();

                    DataTable DTQCSelectItemInfo = new DataTable();
                    DTQCSelectItemInfo = DRQCSelectItemInfo[0].Table.Clone();
                    foreach (DataRow row in DRQCSelectItemInfo)
                    {
                        DTQCSelectItemInfo.ImportRow(row); // 将DataRow添加到DataTable中
                    }




                    DTQCSelectItemInfo.TableName = "QCItemInfo";
                    dataSet.Tables.Add(DTQCSelectItemInfo);

                    //double accNum = DTQCItemInfo.AsEnumerable().Select(d => Convert.ToDouble(d.Field<string>("itemResult"))).Count();
                    //double accavgValue = Math.Round(DTQCItemInfo.AsEnumerable().Select(d => Convert.ToDouble(d.Field<string>("itemResult"))).Average(),4);
                    List<string> Gradeids = new List<string>();
                    foreach (DataRow DRGrade in DTQCItemInfo.Rows)
                    {

                        string Gradeid = DRGrade["planGradeid"] != DBNull.Value ? DRGrade["planGradeid"].ToString() : "";


                        if (!Gradeids.Contains(Gradeid))
                        {
                            #region 计算质控品选择范围信息累计平均值和SD值
                            //DataRow[] DRGradesSelect = DTQCItemInfo.Select($"planGradeid='{Gradeid}' and qcTime>='{startTime}' and qcTime<='{endTime}'");
                            DataRow[] DRGradesSelect = DTQCSelectItemInfo.Select($"planGradeid='{Gradeid}'");

                            int SelectGradeInfoCount = DRGradesSelect.Length;

                            //计算选中信息的平均值
                            double selectAvgInfo = 0;
                            foreach (DataRow DRResultInfo in DRGradesSelect)
                            {
                                double DRresultValue = DRResultInfo["itemResult"] != DBNull.Value ? Convert.ToDouble(DRResultInfo["itemResult"]) : 0;
                                selectAvgInfo += DRresultValue;
                            }
                            double selectInfoCount = DRGradesSelect.Length;
                            double scount = DRGradesSelect.Length - 1;
                            selectAvgInfo = Math.Round(selectAvgInfo / selectInfoCount, 4);
                            //try
                            //{
                            //    selectAvgInfo = Math.Round(DRGradesSelect.AsEnumerable().Select(d => Convert.ToDouble(d.Field<string>("itemResult"))).Average(), 4);
                            //}
                            //catch (Exception ex)
                            //{
                            //    selectAvgInfo = 0;
                            //}
                            //计算选中值的sd值
                            double SelectGradePingfangSD = 0;
                            foreach (DataRow DRResultInfoSelect in DRGradesSelect)
                            {
                                double DRresultValue = DRResultInfoSelect["itemResult"] != DBNull.Value ? Convert.ToDouble(DRResultInfoSelect["itemResult"]) : 0;
                                double chaReult = DRresultValue - selectAvgInfo;
                                double pingfangResult = chaReult * chaReult;
                                SelectGradePingfangSD += pingfangResult;
                            }

                            //double selectCount = DRGradesSelect.Length;
                            //double acount = DRGradesSelect.Length - 1;
                            double selectSD = Math.Round(Math.Sqrt(SelectGradePingfangSD / scount), 4);
                            double selectCV = Math.Round(selectSD / selectAvgInfo * 100, 4);


                            #endregion
                            #region 计算质控品累计平均值和SD值
                            DataRow[] DRGrades = DTQCItemInfo.Select($"planGradeid='{Gradeid}'");


                            //计算累计值的平均值
                            double GradeAvgInfo = 0;
                            foreach (DataRow DRResultInfo in DRGrades)
                            {
                                double DRresultValue = DRResultInfo["itemResult"] != DBNull.Value ? Convert.ToDouble(DRResultInfo["itemResult"]) : 0;
                                GradeAvgInfo += DRresultValue;
                            }
                            double GradeInfoCount = DRGrades.Length;
                            double bcount = DRGrades.Length - 1;
                            GradeAvgInfo = Math.Round(GradeAvgInfo / GradeInfoCount, 4);

                            //try
                            //{
                            //    GradeAvgInfo = Math.Round(DRGrades.AsEnumerable().Select(d => Convert.ToDouble(d.Field<string>("itemResult"))).Average(), 4);
                            //}
                            //catch (Exception ex)
                            //{
                            //    GradeAvgInfo = 0;
                            //}

                            double GradePingfangSD = 0;
                            foreach (DataRow DRResultInfo in DRGrades)
                            {
                                double DRresultValue = DRResultInfo["itemResult"] != DBNull.Value ? Convert.ToDouble(DRResultInfo["itemResult"]) : 0;
                                double chaReult = DRresultValue - GradeAvgInfo;
                                double pingfangResult = chaReult * chaReult;
                                GradePingfangSD += pingfangResult;
                            }
                            //double GradeInfoCount = DRGrades.Length;
                            //double bcount = DRGrades.Length - 1;
                            double GradeInfoSD = Math.Round(Math.Sqrt(GradePingfangSD / bcount), 4);
                            double GradeInfoCV = Math.Round(GradeInfoSD / GradeAvgInfo * 100, 4);
                            #endregion

                            DataRow DRQCInfo = REDTQCItemInfo.NewRow();

                            DRQCInfo["gradeid"] = DRGrade["planGradeid"];
                            DRQCInfo["gradecode"] = DRGrade["planGradeCode"];
                            DRQCInfo["gradename"] = DRGrade["planGradeName"];
                            DRQCInfo["gradeproducer"] = DRGrade["planGradeproducer"];
                            if (DRGrade["planGradelevelNO"] != DBNull.Value)
                            {

                                string levelNO = DRGrade["planGradelevelNO"].ToString();

                                string level = "";
                                switch (levelNO)
                                {
                                    case "1":
                                        level = "低水平";
                                        break;
                                    case "2":
                                        level = "中水平";
                                        break;
                                    case "3":
                                        level = "高水平";
                                        break;
                                    default:
                                        level = "";
                                        break;

                                }
                                DRQCInfo["gradelevelNO"] = level;
                            }





                            DRQCInfo["gradeavgValue"] = DRGrade["planGradeavgValue"];
                            DRQCInfo["gradesdValue"] = DRGrade["planGradesdValue"];
                            DRQCInfo["gradecvValue"] = DRGrade["planGradecvValue"];
                            DRQCInfo["gradeerror"] = DRGrade["planGradeerror"];
                            DRQCInfo["gradeerrorTEa"] = DRGrade["planGradeerrorTEa"];
                            DRQCInfo["gradefactoryX"] = DRGrade["planGradefactoryX"];
                            DRQCInfo["gradefactoryRange"] = DRGrade["planGradefactoryRange"];


                            DRQCInfo["selavgValue"] = selectAvgInfo;
                            DRQCInfo["selsdValue"] = selectSD;
                            DRQCInfo["selcvValue"] = selectCV;
                            DRQCInfo["selNum"] = selectInfoCount;
                            DRQCInfo["accavgValue"] = GradeAvgInfo;
                            DRQCInfo["accsdValue"] = GradeInfoSD;
                            DRQCInfo["acccvValue"] = GradeInfoCV;
                            DRQCInfo["accNum"] = GradeInfoCount;
                            REDTQCItemInfo.Rows.Add(DRQCInfo);
                        }




                        Gradeids.Add(Gradeid);
                    }
                    REDTQCItemInfo.TableName = "QCGradeidInfo";
                    dataSet.Tables.Add(REDTQCItemInfo);
                }

                DataTable DTHandles = DTQCHandleInfo.Copy();



                DTHandles.TableName = "QCHandleInfo";
                dataSet.Tables.Add(DTHandles);
                dataSet.DataSetName = "QCInfo";
                jm.data = dataSet;
            }



            return jm;

        }
    }
}
