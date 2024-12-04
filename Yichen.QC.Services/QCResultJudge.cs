using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.QC.IServices;

namespace Yichen.QC.Services
{
    public class QCResultJudge : BaseServices<object>, IQCResultJudge
    {
        /// <summary>
        /// 指控规则
        /// </summary>

        static DataTable DTQCRule = null;

        /// <summary>
        /// 记录遍历结果个数
        /// </summary>
        static int resultNo = 0;
        /// <summary>
        /// 0为在x轴上，1为正数，-1为负数
        /// </summary>
        static int resultState = 0;
        /// <summary>
        /// 上一个点的正负状态（0为在x轴上）
        /// </summary>
        static int NTresultStates = 0;
        /// <summary>
        /// 存储上一个结果
        /// </summary>
        static double NTbeforezValue = 0;
        /// <summary>
        /// 存储上一个结果的正负值
        /// </summary>
        static int beforeresultState = 0;
        /// <summary>
        /// 满足（NT）条件计数器
        /// </summary>
        static List<int> NTResultStart = new List<int>();//满足条件计数器
        /// <summary>
        /// 满足（NxS）条件计数器
        /// </summary>
        static Dictionary<double, int> NxSStarts = new Dictionary<double, int>();//xvalue 为key值，starResult为value值
        /// <summary>
        /// 满足（NX）条件计数器
        /// </summary>
        static Dictionary<double, int> NXStarts = new Dictionary<double, int>();//xvalue 为key值，starResult为value值
        /// <summary>
        /// 满足（MNxS）条件计数器
        /// </summary>
        static List<MNxSa> MNxSValuePairs = new List<MNxSa>();//int 项目序号  string+nxm值


        public readonly IUnitOfWork _unitOfWork;
        private readonly ICommRepository _commRepository;

        public QCResultJudge(IUnitOfWork unitOfWork
            , ICommRepository commRepository

            )
        {
            _unitOfWork = unitOfWork;
            _commRepository = commRepository;

        }














        ///// <summary>
        ///// 读取质控规则信息
        ///// </summary>
        ///// <param name="ruleNO"></param>
        //public static void GetQCRule(string ruleNO)
        //{
        //    string listQCDT = $"select listQC from QC.RuleGroup where no='{ruleNO}'";
        //    DataTable DTGroupRule = HLDBSqlHelper.ExecuteDataset(listQCDT).Tables[0];
        //    if (DTGroupRule != null && DTGroupRule.Rows.Count > 0)
        //    {
        //        string listQC = DTGroupRule.Rows[0][0] != DBNull.Value ? DTGroupRule.Rows[0][0].ToString() : "";
        //        if (listQC != "")
        //        {
        //            string QCDT = $"select * from QC.RuleQC where no in ({listQC})";
        //            DTQCRule = HLDBSqlHelper.ExecuteDataset(QCDT).Tables[0];
        //        }
        //    }
        //}

        /// <summary>
        /// 插入指控结果判断
        /// </summary>
        /// <param name="planid">指控计划编号</param>
        /// <param name="planGradeid">质控品编号</param>
        /// <param name="itemNO">项目编号</param>
        /// <param name="sort">排序</param>
        /// <returns></returns>
        public async Task<bool> NewRestultJudge(string planid, string planGradeid, string itemNO, int sort)
        {
            string ruleNOs = "";

            DTQCRule = null;

            resultNo = 0;
            resultState = 0;
            NTresultStates = 0;
            NTbeforezValue = 0;
            beforeresultState = 0;
            NxSStarts.Clear();
            NXStarts.Clear();
            MNxSValuePairs.Clear();
            NTResultStart.Clear();



            if (planid != null && itemNO != null)
            {
                if (planid != "" && itemNO != "")
                {
                    resultNo = 0;

                    string sql = $"select * from QC.QCResultView where planid='{planid}' and planGradeid='{planGradeid}' and planItemNo='{itemNO}' and itemResultdstate=0 order by qcTime";
                    //string sql = $"select * from QC.QCResultView where planid='{planid}' and planItemNo='{itemNO}' and itemResultdstate=0 order by planGradeid,qcTime";
                    //获取指定指控计划疾控品的结果相关信息
                    DataTable DTQCItemResult = await _commRepository.GetTable(sql);


                    if (DTQCItemResult != null && DTQCItemResult.Rows.Count > 0)
                    {

                        #region 计算选择数据的SD值
                        //数据库修改语句
                        string updateResultSql = "";
                        //选择信息的值总和（查询信息的结果总和）
                        double selectCountValue = 0;
                        //判断改指控结果的平均值
                        double selectAvgValue = selectCountValue / DTQCItemResult.Rows.Count;//计算平均数
                        //选择项目结果与平均值差的平方总和
                        double selectPingfangSD = 0;
                        //遍历所有结果值
                        foreach (DataRow DRResultInfo in DTQCItemResult.Rows)
                        {

                            //string SetavgValue = DRResultInfo["avgValue"] != DBNull.Value ? DRResultInfo["avgValue"].ToString() : "";
                            //string SetsdValue = DRResultInfo["sdValue"] != DBNull.Value ? DRResultInfo["sdValue"].ToString() : "";
                            //string SetcvValue = DRResultInfo["cvValue"] != DBNull.Value ? DRResultInfo["cvValue"].ToString() : "";
                            selectCountValue += DRResultInfo["itemResult"] != DBNull.Value ? Convert.ToDouble(DRResultInfo["itemResult"]) : 0;
                            //历史结果
                            double DRresultValue = DRResultInfo["itemResult"] != DBNull.Value ? Convert.ToDouble(DRResultInfo["itemResult"]) : 0;
                            //与平均值的差
                            double chaReult = DRresultValue - selectAvgValue;
                            //历史结果与平均的平方
                            double pingfangResult = chaReult * chaReult;
                            selectPingfangSD += pingfangResult;
                        }
                        //结果个数-1（计算sd值的除数）
                        double n = DTQCItemResult.Rows.Count - 1;
                        //计算选择数据中的SD值取小数点后4位
                        double selectSD = Math.Round(Math.Sqrt(selectPingfangSD / n), 4);

                        #endregion

                        //初始化数据

                        //更新结果z分数值数据和
                        foreach (DataRow DRResultInfo in DTQCItemResult.Rows)
                        {
                            //项目id

                            string itemID = DRResultInfo["itemResultid"] != DBNull.Value ? DRResultInfo["itemResultid"].ToString() : "";

                            //指控规则

                            string ruleNO = DRResultInfo["ruleNO"] != DBNull.Value ? DRResultInfo["ruleNO"].ToString() : "";

                            //项目结果
                            double itemResult = DRResultInfo["itemResult"] != DBNull.Value ? Convert.ToDouble(DRResultInfo["itemResult"]) : 0;
                            //项目结果z分数值
                            double zVlaue = DRResultInfo["zVlaue"] != DBNull.Value ? Convert.ToDouble(DRResultInfo["zVlaue"]) : 0;
                            //计算结果与平均值的差值
                            double chazhiValue = itemResult - selectAvgValue;
                            //计算当前值的Z分数值
                            double NewzValue = chazhiValue / selectSD;
                            NewzValue = Math.Round(NewzValue, 4);

                            //记录遍历的结果个数
                            resultNo += 1;
                            //记录结果在X轴上下的个数
                            resultState = NewzValue > 0 ? 1 : -1;//判断结果的正负值
                            //结果失控状态
                            string resultType = "";
                            //结果规则
                            string resultRule = "";
                            if (ruleNOs != ruleNO)
                            {

                                ruleNOs = ruleNO;

                                //获取计划质控规则
                                //GetQCRule(ruleNO);
                                string listQCDT = $"select listQC from QC.RuleGroup where no='{ruleNO}'";
                                DataTable DTGroupRule = await _commRepository.GetTable(listQCDT);
                                if (DTGroupRule != null && DTGroupRule.Rows.Count > 0)
                                {

                                    string listQC = DTGroupRule.Rows[0][0] != DBNull.Value ? DTGroupRule.Rows[0][0].ToString() : "";

                                    if (listQC != "")
                                    {
                                        string QCDT = $"select * from QC.RuleQC where no in ({listQC})";
                                        DTQCRule = await _commRepository.GetTable(QCDT);
                                    }
                                }
                            }
                            if (DTQCRule != null)
                            {
                                //string aaaa = itemResult.ToString();
                                resultRule += ResultJudge(NewzValue, out resultType);
                            }
                            if (itemID != "")
                            {
                                string resultTypeString = "";
                                if (resultType == "1")
                                    resultTypeString = "在控";
                                if (resultType == "2")
                                    resultTypeString = "警告";
                                if (resultType == "3")
                                    resultTypeString = "失控";
                                updateResultSql += $"update QC.QCItemResult set resultType='{resultTypeString}',resultRule='{resultRule}',zVlaue='{NewzValue}' where id='{itemID}';\r\n";
                            }
                            //
                        }
                        if (updateResultSql != "")
                        {
                          await  _commRepository.sqlcommand(updateResultSql);
                          return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }





        /// <summary>
        /// 判断质控规则
        /// </summary>
        /// <param name="zValue">z分数值</param>
        /// <param name="resultTypes"></param>
        /// <returns></returns>
        public static string ResultJudge(double zValue, out string resultTypes)
        {

            string reRuleString = "";
            //判定状态（获取判定编号，1在控，2警告，3失控）
            resultTypes = "1";
            foreach (DataRow DRQCRule in DTQCRule.Rows)
            {
                //规则编号

                string classNO = DRQCRule["classNO"] != DBNull.Value ? DRQCRule["classNO"].ToString() : "";

                //质控最小值
                int nValue = DRQCRule["nValue"] != DBNull.Value ? Convert.ToInt32(DRQCRule["nValue"]) : 0;
                //质控最大值
                int mValue = DRQCRule["mValue"] != DBNull.Value ? Convert.ToInt32(DRQCRule["mValue"]) : 0;
                //预设x轴值
                double xValue = DRQCRule["xValue"] != DBNull.Value ? Math.Round(Convert.ToDouble(DRQCRule["xValue"]), 4) : 0;
                //预设y轴值
                double yValue = DRQCRule["yValue"] != DBNull.Value ? Math.Round(Convert.ToDouble(DRQCRule["yValue"]), 4) : 0;
                //判断值是否失控（true 在空，false失控）
                bool vlaueState = true;
                switch (classNO)
                {
                    case "1":
                        vlaueState = NxS(zValue, nValue, xValue);
                        break;
                    case "2":
                        //vlaueState = RxS(zValue, nValue, xValue,yValue);
                        break;
                    case "3":
                        vlaueState = NT(zValue, nValue, xValue);
                        break;
                    case "4":
                        vlaueState = NX(zValue, nValue, xValue);
                        break;
                    case "5":
                        vlaueState = MNxS(zValue, nValue, xValue, mValue);
                        break;
                    default:
                        //vlaueState = NxS(zValue, nValue, xValue);
                        break;
                }
                if (!vlaueState)
                {
                    //违反规则名称
                    reRuleString += DRQCRule["names"] != DBNull.Value ? DRQCRule["names"].ToString() + "," : "";
                    //判定状态（获取判定编号，1在控，2警告，3失控）

                    string resultType = DRQCRule["criteriaNO"] != DBNull.Value ? DRQCRule["criteriaNO"].ToString() : "";

                    if (resultType != "1")
                    {
                        if (resultTypes != "3")

                            resultTypes = resultType;

                    }

                }

            }
            return reRuleString;
        }




        /// <summary>
        /// N-xS类型质控结果判定
        /// </summary>
        /// <param name="Result">项目结果</param>
        /// <param name="AvgValue">查询平均值</param>
        /// <param name="SDValue">标准差值</param>
        /// <param name="nValue">n个点</param>
        /// <param name="xValue">x轴值</param>
        /// <returns></returns>
        public static bool NxS(double zValue, int nValue, double xValue)
        {

            if (zValue != 0)
            {
                if (Math.Abs(zValue) > Math.Abs(xValue))
                {
                    if (!NxSStarts.ContainsKey(xValue))
                    {
                        NxSStarts.Add(xValue, resultNo);
                    }
                }
                else
                {
                    if (NxSStarts.ContainsKey(xValue))
                    {
                        NxSStarts.Remove(xValue);
                    }
                }


                if (NxSStarts.ContainsKey(xValue))
                {
                    if (NxSStarts.TryGetValue(xValue, out int RNo))
                    {
                        if (resultNo - RNo >= nValue - 1)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        NxSStarts[xValue] = 0;
                        return true;
                    }
                }
                else
                {
                    return true;
                }

            }
            else
            {
                NxSStarts.Clear();
                return true;
            }
        }


        //static double RxScontinueState = 0;//满足条件计数器
        //static int RxSresultStates = 0;//上一个点的正负状态（0为在x轴上）
        //static double RxSbeforezValue = 0;

        ///// <summary>
        ///// R-xS类型质控结果判定(连续R个点的值差超过xS)
        ///// </summary>
        ///// <param name="Result">项目结果</param>
        ///// <param name="AvgValue">查询平均值</param>
        ///// <param name="SDValue">标准差值</param>
        ///// <param name="nValue">n个点</param>
        ///// <param name="xValue">x轴值</param>
        ///// <param name="yValue">y轴值</param>
        ///// <returns></returns>
        //public static bool RxS(double zValue, double nValue, double xValue, double yValue)
        //{
        //    //double zValue = ((Result - AvgValue) / SDValue);
        //    if (zValue != 0)
        //    {

        //        zValue = Math.Round(zValue, 4);//保留小数后四位

        //        int RxSresultState = zValue > 0 ? 1 : -1;//判断结果的正负值
        //        if (RxSresultState != RxSresultStates)
        //        {
        //            if (Math.Abs(zValue) > Math.Abs(xValue))
        //            {


        //                if (Math.Abs(RxSbeforezValue) + Math.Abs(zValue) > Math.Abs(yValue))
        //                {
        //                    RxScontinueState += 1;//记录连续结果正值或负值状态。
        //                }
        //                else
        //                {
        //                    RxScontinueState = 0;
        //                }

        //            }
        //            else
        //            {
        //                RxScontinueState = 0;
        //            }
        //        }
        //        else
        //        {
        //            RxScontinueState = 0;
        //        }
        //        RxSresultStates = RxSresultState;//记录当前正负值状态

        //        if (RxScontinueState == nValue - 1&& RxScontinueState!=0)//判断当前是第几个点
        //        {

        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        RxScontinueState = 0;
        //        return true;
        //    }

        //}




        /// <summary>
        /// NT类型质控结果判定(连续N个点中出现连续向上和向下的趋势)
        /// </summary>
        /// <param name="Result">项目结果</param>
        /// <param name="AvgValue">查询平均值</param>
        /// <param name="SDValue">标准差值</param>
        /// <param name="nValue">n个点</param>
        /// <param name="xValue">x轴值</param>
        /// <param name="yValue">y轴值</param>
        /// <returns></returns>
        public static bool NT(double zValue, int nValue, double xValue)
        {

            if (zValue != 0)
            {
                //int NTresultState = zValue > 0 ? 1 : -1;//判断结果的正负值
                if (resultState == NTresultStates)//判断值的正负值
                {
                    if (Math.Abs(zValue) > Math.Abs(xValue))
                    {
                        if (Math.Abs(zValue) > Math.Abs(NTbeforezValue))
                        {
                            NTResultStart.Add(resultNo);//记录连续结果正值或负值状态。
                        }
                    }
                }
                else
                {
                    NTResultStart.Clear();
                    if (Math.Abs(zValue) > Math.Abs(xValue))
                    {
                        NTResultStart.Add(resultNo);
                    }
                }

                NTresultStates = resultState;//记录当前正负值状态
                NTbeforezValue = zValue;//记录当前结果值
                if (NTResultStart.Count >= nValue)
                {

                    int a = NTResultStart[NTResultStart.Count - 1];
                    int b = NTResultStart[NTResultStart.Count - nValue];


                    if (NTResultStart[NTResultStart.Count - 1] - NTResultStart[NTResultStart.Count - nValue] == nValue - 1)//判断当前是第几个点
                    {

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return true;
            }
        }




        /// <summary>
        /// NX类型质控结果判定
        /// </summary>
        /// <param name="Result">项目结果</param>
        /// <param name="AvgValue">查询平均值</param>
        /// <param name="SDValue">标准差值</param>
        /// <param name="nValue">n个点</param>
        /// <param name="xValue">x轴值</param>
        /// <returns></returns>
        public static bool NX(double zValue, int nValue, double xValue)
        {

            if (zValue != 0)
            {
                if (beforeresultState == resultState)
                {


                    if (Math.Abs(zValue) > Math.Abs(xValue))
                    {
                        if (!NXStarts.ContainsKey(xValue))
                        {
                            NXStarts.Add(xValue, resultNo);
                        }
                    }
                    else
                    {
                        if (NXStarts.ContainsKey(xValue))
                        {
                            NXStarts.Remove(xValue);
                        }
                    }


                    if (NXStarts.ContainsKey(xValue))
                    {
                        if (NXStarts.TryGetValue(xValue, out int RNo))
                        {
                            if (resultNo - RNo >= nValue - 1)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            NXStarts[xValue] = 0;
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    beforeresultState = resultState;
                    NXStarts.Clear();
                    return true;
                }
            }
            else
            {
                NXStarts.Clear();
                return true;
            }
        }


        /// <summary>
        /// (M-N)xS类型质控结果判定(连续N个点中有M个Z分大于x值)
        /// </summary>
        /// <param name="zValue"></param>
        /// <param name="nValue">n个点</param>
        /// <param name="xValue">x轴值</param>
        /// <param name="mValue"></param>
        /// <returns></returns>
        public static bool MNxS(double zValue, int nValue, double xValue, int mValue)
        {
            if (zValue != 0)
            {
                if (Math.Abs(zValue) > Math.Abs(xValue))
                {
                    MNxSa mNxSa = new MNxSa()
                    {
                        nValues = nValue,
                        xValues = xValue,
                        mValues = mValue,
                        resultNos = resultNo
                    };
                    MNxSValuePairs.Add(mNxSa);
                    if (MNxSValuePairs.Count > mValue)
                    {


                        List<MNxSa> MNxSValuePair = new List<MNxSa>();
                        foreach (MNxSa nxS in MNxSValuePairs)
                        {
                            if (nxS.nValues == nValue && nxS.xValues == xValue && nxS.mValues == mValue)
                            {
                                MNxSValuePair.Add(nxS);
                            }
                        }
                        MNxSValuePair.Sort((a, b) => a.resultNos.CompareTo(b.resultNos));
                        if (MNxSValuePair[MNxSValuePair.Count - 1].resultNos - MNxSValuePair[MNxSValuePair.Count - mValue].resultNos < nValue)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                MNxSValuePairs.Clear();
                return true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public class MNxSa
        {
            /// <summary>
            /// 
            /// </summary>
            public int nValues { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double xValues { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int mValues { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int resultNos { get; set; }
        }
    }
}
