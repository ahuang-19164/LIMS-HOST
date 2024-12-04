using Nito.AsyncEx;
using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;

using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Services;
using Yichen.Files.IServices;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Configuration;
using Yichen.Net.Data;
using Yichen.Net.Utility.Extensions;
using Yichen.Other.IRepository;
using Yichen.System.IRepository;
using Yichen.System.Model;
using Yichen.System.Repository;
using Yichen.Test.IRepository;
using Yichen.Test.IServices;
using Yichen.Test.Model.Result;
using Yichen.Test.Model.table;
using Yichen.Net.Configuration;
using Yichen.Net.Caching.Manuals;
using Yichen.Flow.Model;

namespace Yichen.Test.Services
{
    public class ItemSaveOldServices : BaseServices<test_sampleInfo>, IItemSaveOldServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        public readonly IUnitOfWork _UnitOfWork;
        public readonly IHttpContextUser _httpContextUser;
        public readonly IRecordRepository _recordRepository;
        public readonly ITestItemInfoRepository _itemHandleRepository;
        public readonly IFileHandleServices _fileHandleServices;
        public readonly ITestItemSaveRepository _testSaveRepository;
        public readonly ITestResultInfoRepository _testRepository;
        public readonly ICommRepository _commRepository;
        //private readonly IItemTestRepository _itemTestRepository;
        //private readonly IItemFlowRepository _itemFlowRepository;
        //private readonly IGroupTestRepository _groupTestRepository;
        public ItemSaveOldServices(IUnitOfWork unitOfWork
            , IHttpContextUser httpContextUser
            , IRecordRepository recordRepository
            , ITestItemInfoRepository itemHandleRepository
            , IFileHandleServices fileHandleServices
            , ITestItemSaveRepository testSaveRepository
            , ITestResultInfoRepository testRepository
            , ICommRepository commRepository
            //, IItemTestRepository itemTestRepository
            //, IItemFlowRepository itemFlowRepository
            //, IGroupTestRepository groupTestRepository
            )
        {
            _UnitOfWork = unitOfWork;
            _httpContextUser = httpContextUser;
            _recordRepository = recordRepository;
            _itemHandleRepository = itemHandleRepository;
            _fileHandleServices = fileHandleServices;
            _testSaveRepository = testSaveRepository;
            _testRepository = testRepository;
            _commRepository = commRepository;
            //_itemTestRepository = itemTestRepository;
            //_itemFlowRepository = itemFlowRepository;
            //_groupTestRepository = groupTestRepository;
        }

        public async Task<WebApiCallBack> SaveTest(CommResultModel<TestInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                commReInfo<commReItemInfo> commReInfo = new commReInfo<commReItemInfo>();
                //CommResultModel<TestInfoModel> info = JsonHandle.JsonConvertObject<CommResultModel<TestInfoModel>>(TestInfo);
                try
                {

                    {
                        if (info.Result != null)
                        {
                            if (info.Result.testid != 0)
                            {
                                var TestSampleInfo = await _testRepository.GetTestInfo(info.Result.testid);//样本信息
                                //DataTable sampleInfoDT = await _testRepository.GetSampleInfo(info.Result.testid);//样本信息
                                if(TestSampleInfo != null)
                                {
                                    if (info.ResultState < 4)
                                    {
                                        if (info.AutographInfo != null)
                                        {
                                            if (info.Result.ListResult != null)
                                            {
                                                List<commReItemInfo> commReItemInfos = new List<commReItemInfo>();
                                                string resultRecordString = "";//项目结果修改记录
                                                string resultInfoString = "";//项目结果保存信息
                                                string listItemCode = "";
                                                int testState = 0;
                                                string testFlowNO = "1";
                                                string nextFlowNO = "0";
                                                bool? trequal = false;
                                                bool? tcequal = false;
                                                bool? rcequal = false;
                                                bool? reviewState = false;

                                                int perid = 0;

                                                int testid = 0;
                                                string sampleID = "";
                                                string barcode = "";
                                                string tester = "";
                                                string reTester = "";
                                                string checker = "";



                                                DateTime testTime = DateTime.MinValue;
                                                DateTime reTestTime = DateTime.MinValue;
                                                DateTime checkTime = DateTime.MinValue;


                                                foreach (TestResultModel itemResult in info.Result.ListResult)
                                                {
                                                    listItemCode += itemResult.itemCode + ",";

                                                }
                                                if (listItemCode.Length > 0)
                                                {
                                                    DataTable oldItemInfoDT = await _testRepository.GetSampleResult(info.Result.testid, listItemCode);///原始结果
                                                    if (oldItemInfoDT.Rows.Count > 0 && TestSampleInfo!=null)
                                                    {
                                                        testid =TestSampleInfo.id;
                                                        perid = TestSampleInfo.perid != null ? Convert.ToInt32(TestSampleInfo.perid) : 0;
                                                        barcode = TestSampleInfo.barcode != null ? TestSampleInfo.barcode.ToString() : "";
                                                        sampleID = TestSampleInfo.sampleID != null ? TestSampleInfo.sampleID.ToString() : "";
                                                        //读取样本状态
                                                        testState = TestSampleInfo.testStateNO != null ? Convert.ToInt32(TestSampleInfo.testStateNO) : 0;
                                                        //读取样本流程
                                                        testFlowNO = TestSampleInfo.groupFlowNO != null ? TestSampleInfo.groupFlowNO.ToString() : "1";
                                                        //读取专业组编号
                                                        string groupNO = TestSampleInfo.groupNO != null ? TestSampleInfo.groupNO.ToString() : "0";

                                                        //片段标本状态是检验中和初审 审核  和已委托
                                                        if (testState < 4 || testState == 5)
                                                        {
                                                            tester = TestSampleInfo.tester != null ? TestSampleInfo.tester.ToString() : "";
                                                            reTester = TestSampleInfo.reTester != null ? TestSampleInfo.reTester.ToString() : "";
                                                            checker = TestSampleInfo.checker != null ? TestSampleInfo.checker.ToString() : "";
                                                            testTime = TestSampleInfo.testTime != null ? Convert.ToDateTime(TestSampleInfo.testTime) : DateTime.MinValue;
                                                            reTestTime = TestSampleInfo.reTestTime != null ? Convert.ToDateTime(TestSampleInfo.reTestTime) : DateTime.MinValue;
                                                            checkTime = TestSampleInfo.checkTime != null ? Convert.ToDateTime(TestSampleInfo.checkTime) : DateTime.MinValue;


                                                            List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
                                                            comm_item_flow testFlowInfo = FlowInfoss.FirstOrDefault(p => p.no == Convert.ToInt32(testFlowNO));


                                                            //List<comm_group_test> groupinfoss =await _groupTestRepository.GetCaChe();
                                                            List<comm_group_test> groupinfoss = ManualDataCache<comm_group_test>.MemoryCache.LIMSGetKeyValue(CommInfo.grouptest);
                                                            comm_group_test grouptestInfo = groupinfoss.FirstOrDefault(p => p.no == Convert.ToInt32(groupNO));



                                                            if (testFlowInfo != null && grouptestInfo != null)
                                                            {
                                                                trequal = grouptestInfo.trequal != null ? grouptestInfo.trequal : false;
                                                                tcequal = grouptestInfo.tcequal != null ? grouptestInfo.tcequal : false;
                                                                rcequal = grouptestInfo.rcequal != null ? grouptestInfo.rcequal : false;
                                                                reviewState = grouptestInfo.reviewState != null ? grouptestInfo.reviewState : false;

                                                                string resultTableName = testFlowInfo.dataSource != null ? testFlowInfo.dataSource : "";
                                                                string resultImgName = testFlowInfo.imgSource != null ? testFlowInfo.imgSource : "";
                                                                nextFlowNO = testFlowInfo.nextFlow != null && testFlowInfo.nextFlow == "0" ? testFlowInfo.nextFlow : "0";
                                                                if (nextFlowNO != "0")
                                                                {
                                                                    string sqlUpFlowNO = $"update WorkTest.SampleInfoFlow set dstate=1 where testid='{info.Result.testid}' and flowNO='{info.testFlowNO}';";
                                                                    sqlUpFlowNO += "insert into WorkTest.SampleInfoFlow (perid,testid,barcode,flowNO,flowSort,creater,createTime,state,dstate)";
                                                                    sqlUpFlowNO += $"values (0,'{testid}','{barcode}','{testFlowNO}',0,'{info.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
                                                                    resultInfoString += sqlUpFlowNO;
                                                                    commReInfo.nextFlowNO = nextFlowNO;

                                                                }
                                                                else
                                                                {

                                                                    nextFlowNO = testFlowNO;

                                                                }

                                                                if (resultTableName != "")
                                                                {
                                                                    foreach (TestResultModel itemResult in info.Result.ListResult)
                                                                    {
                                                                        commReItemInfo commReItemInfo = new commReItemInfo();
                                                                        DataRow[] itemDR = oldItemInfoDT.Select($"itemCodes='{itemResult.itemCode}'");
                                                                        if (itemDR != null)
                                                                        {

                                                                            string itemNames = itemDR[0]["itemNames"] != DBNull.Value ? itemDR[0]["itemNames"].ToString() : "";

                                                                            bool itemNullState = itemDR[0]["resultNullState"] != DBNull.Value ? Convert.ToBoolean(itemDR[0]["resultNullState"]) : false;

                                                                            string oldItemResult = itemDR[0]["itemResult"] != DBNull.Value ? itemDR[0]["itemResult"].ToString() : "";

                                                                            #region 判断参考值信息
                                                                            double ReferenceDown = -9999;
                                                                            double ReferenceUp = -9999;
                                                                            double crisisDown = -9999;
                                                                            double crisisUp = -9999;

                                                                            ReferenceDown = itemDR[0]["ReferenceDown"].ObjectToDouble();
                                                                            ReferenceUp = itemDR[0]["ReferenceUp"].ObjectToDouble();
                                                                            crisisDown = itemDR[0]["crisisDown"].ObjectToDouble();
                                                                            crisisUp = itemDR[0]["crisisUp"].ObjectToDouble();
                                                                            string flag = "";
                                                                            string result = "";
                                                                            try
                                                                            {

                                                                                result = itemResult.itemResult;

                                                                                double itemResulta = Convert.ToDouble(itemResult.itemResult);
                                                                                if (ReferenceDown != -9999 && ReferenceUp != -9999)
                                                                                {
                                                                                    if (ReferenceDown > itemResulta)
                                                                                    {
                                                                                        flag = "↓";
                                                                                    }
                                                                                    if (ReferenceUp < itemResulta)
                                                                                    {
                                                                                        flag = "↑";
                                                                                    }
                                                                                }
                                                                                if (crisisDown != -9999 && crisisUp != -9999)
                                                                                {
                                                                                    if (crisisDown > itemResulta)
                                                                                    {
                                                                                        flag = "↓↓";
                                                                                        //可加危急值方法记录
                                                                                        await _itemHandleRepository.ItemCrisisHandle(perid, testid, barcode, itemResult.itemCode, itemNames, itemResult.itemResult, ReferenceDown + "-" + ReferenceUp, crisisDown + "-" + crisisUp, info.UserName);
                                                                                    }
                                                                                    if (crisisUp < itemResulta)
                                                                                    {
                                                                                        flag = "↑↑";
                                                                                        await _itemHandleRepository.ItemCrisisHandle(perid, testid, barcode, itemResult.itemCode, itemNames, itemResult.itemResult, ReferenceDown + "-" + ReferenceUp, crisisDown + "-" + crisisUp, info.UserName);
                                                                                    }
                                                                                }
                                                                            }
                                                                            catch
                                                                            {
                                                                                result = itemResult.itemResult != null && itemResult.itemResult.Trim().Length > 0 ? itemResult.itemResult : "";
                                                                            }

                                                                            #endregion


                                                                            #region 生成项目结果修改记录
                                                                            if (!itemNullState)
                                                                            {
                                                                                if (result != "")
                                                                                {

                                                                                    if (itemResult.itemResult != oldItemResult)
                                                                                    {
                                                                                        if (oldItemResult != "")
                                                                                        {
                                                                                            resultRecordString += $"项目编号:{itemResult.itemCode}结果由：[{oldItemResult}]更改为[{result}];";
                                                                                            resultInfoString += $"update {resultTableName} set itemResult='{result}',flag='{flag}',reportState='{itemResult.itemReportState}',itemSort={itemResult.itemSort},itemResultLog='{itemResult.itemResult}|{itemDR[0]["itemResultLog"]}' where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';\r\n";
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemFlag = flag;
                                                                                            commReItemInfo.ItemMsg = "更新结果成功";
                                                                                            commReItemInfo.state = 1;
                                                                                            commReItemInfos.Add(commReItemInfo);

                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            resultInfoString += $"update {resultTableName} set itemResult='{result}',flag='{flag}',reportState='{itemResult.itemReportState}',itemSort={itemResult.itemSort},itemResultLog='{itemResult.itemResult}|{itemDR[0]["itemResultLog"]}' where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';\r\n";
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemFlag = flag;
                                                                                            commReItemInfo.ItemMsg = "更新结果成功";
                                                                                            commReItemInfo.state = 1;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //resultInfoString += $"update WorkTest.SampleResult set itemResult='{result}',flag='{flag}',reportState='{itemResult.itemReportState}',itemSort={itemResult.itemSort},itemResultLog='{itemResult.itemResult}|'+itemResultLog where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';";
                                                                                        commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                        commReItemInfo.ItemFlag = flag;
                                                                                        commReItemInfo.ItemMsg = "更新结果成功";
                                                                                        commReItemInfo.state = 1;
                                                                                        commReItemInfos.Add(commReItemInfo);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {

                                                                                    commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                    commReItemInfo.ItemMsg = "结果不能为空";
                                                                                    commReItemInfo.state = 0;
                                                                                    commReItemInfos.Add(commReItemInfo);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (itemResult.itemResult != oldItemResult)
                                                                                {

                                                                                    if (oldItemResult != "")
                                                                                    {
                                                                                        resultRecordString += $"项目编号:{itemResult.itemCode}结果由：[{oldItemResult}]更改为[{result}];";
                                                                                        resultInfoString += $"update {resultTableName} set itemResult='{result}',flag='{flag}',reportState='{itemResult.itemReportState}',itemSort={itemResult.itemSort},itemResultLog='{itemResult.itemResult}|{itemDR[0]["itemResultLog"]}' where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';\r\n";
                                                                                        commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                        commReItemInfo.ItemFlag = flag;
                                                                                        commReItemInfo.ItemMsg = "更新结果成功";
                                                                                        commReItemInfo.state = 1;
                                                                                        commReItemInfos.Add(commReItemInfo);

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        resultInfoString += $"update {resultTableName} set itemResult='{result}',flag='{flag}',reportState='{itemResult.itemReportState}',itemSort={itemResult.itemSort},itemResultLog='{itemResult.itemResult}|{itemDR[0]["itemResultLog"]}' where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';\r\n";
                                                                                        commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                        commReItemInfo.ItemFlag = flag;
                                                                                        commReItemInfo.ItemMsg = "更新结果成功";
                                                                                        commReItemInfo.state = 1;
                                                                                        commReItemInfos.Add(commReItemInfo);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    //commReInfo.code = 2;
                                                                                    //resultInfoString += $"update WorkTest.SampleResult set itemResult='{result}',flag='{flag}',reportState='{itemResult.itemReportState}',itemSort={itemResult.itemSort},itemResultLog='{itemResult.itemResult}|'+itemResultLog where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';";
                                                                                    commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                    commReItemInfo.ItemFlag = flag;
                                                                                    commReItemInfo.ItemMsg = "更新结果成功";
                                                                                    commReItemInfo.state = 1;
                                                                                    commReItemInfos.Add(commReItemInfo);
                                                                                }
                                                                            }
                                                                            #endregion
                                                                        }
                                                                        else
                                                                        {
                                                                            commReItemInfo.code = 1;
                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                            commReItemInfo.ItemMsg = "未找到对应项目信息";
                                                                            commReItemInfo.state = 0;
                                                                            commReItemInfos.Add(commReItemInfo);
                                                                        }

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    commReInfo.code = 1;
                                                                    commReInfo.msg = "流程绑定错误,请检查流程配置";
                                                                }
                                                            }
                                                            else
                                                            {

                                                                commReInfo.code = 1;
                                                                if (FlowInfoss.Count() == 0)
                                                                    commReInfo.msg = "未找到匹配的流程信息,请检查流程配置";
                                                                if (groupinfoss.Count() == 0)
                                                                    commReInfo.msg = "未找到匹配的专业组信息,请检查专业组配置";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            commReInfo.code = 1;
                                                            commReInfo.msg = "请刷新当前样本状态";
                                                        }

                                                    }
                                                    else
                                                    {
                                                        commReInfo.code = 1;
                                                        commReInfo.msg = "未找到样本记录";
                                                    }
                                                }
                                                else
                                                {
                                                    commReInfo.code = 1;
                                                    commReInfo.msg = "未找到对应项目信息";
                                                }

                                                if (resultRecordString.Length > 0)



                                                    await _recordRepository.SampleRecord(info.Result.barcode, RecordEnumVars.ResultEdit, resultRecordString, info.UserName, false);



                                                if (commReInfo.code == 0)
                                                {
                                                    //更新项目结果信息
                                                    if (resultInfoString != "" && commReInfo.code != 2)
                                                    {

                                                        await _commRepository.sqlcommand(resultInfoString);

                                                    }
                                                    //DbClient.Ado.ExecuteCommandAsync(resultInfoString);

                                                    if (info.ResultState == 1)
                                                        jm = await _testSaveRepository.Test(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName, nextFlowNO.ToString());
                                                    if (info.ResultState == 2)
                                                        jm = await _testSaveRepository.reTest(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName);
                                                    if (info.ResultState == 3)
                                                        jm = await _testSaveRepository.Check(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName);
                                                    //检验状态修改成功返回项目状态
                                                    if (jm.code == 0)
                                                        jm.data = commReInfo;
                                                    commReInfo.code = jm.code;






                                                    //WebApiCallBack jms = await _resultSaveService.ResultSave(info.ResultState, info.Result.perid, info.Result.testid, info.Result.sampleID, info.Result.barcode, nextFlowNO, info.UserName, info.AutographInfo);
                                                    //commReInfo.msg = jms.msg;
                                                }
                                                else
                                                {
                                                    commReInfo.code = 1;
                                                    commReInfo.msg = "修改结果失败";
                                                }
                                            }
                                            else
                                            {
                                                commReInfo.code = 1;
                                                commReInfo.msg = "提交项目信息，修改结果失败";
                                            }
                                        }
                                        else
                                        {
                                            commReInfo.code = 1;
                                            commReInfo.msg = "未提交检验者信息";
                                        }
                                    }


                                    #region 反审结果信息
                                    if (info.ResultState == 4)
                                    {
                                        int reportState =TestSampleInfo.report!=null ? Convert.ToInt32(TestSampleInfo.report) : 0;
                                        jm = await _testSaveRepository.ReCheck(info.Result.perid.ToString(), info.Result.testid.ToString(), info.Result.barcode, info.UserName, reportState, "1");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    commReInfo.code = 1;
                                    commReInfo.msg = "未找到样本信息，修改结果失败";
                                }

                            }
                            else
                            {
                                commReInfo.code = 1;
                                commReInfo.msg = "样本信息错误，修改结果失败";
                            }
                        }
                        else
                        {
                            commReInfo.code = 1;
                            commReInfo.msg = "未提交样本信息，修改结果失败";
                        }
                    }
                }
                catch (Exception ex)
                {

                    commReInfo.msg = ex.Message;
                }
                jm.data = commReInfo;
            }
            return jm;

        }

        public async Task<WebApiCallBack> SaveMicrobe(CommResultModel<MicrobeInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {


                commReInfo<commReItemInfo> commReInfo = new commReInfo<commReItemInfo>();
                //CommResultModel<MicrobeInfoModel> info = JsonHandle.JsonConvertObject<CommResultModel<MicrobeInfoModel>>(TestInfo);
                try
                {
                    if (info.Result != null)
                    {
                        if (info.Result.barcode != null && info.Result.testid != 0)
                        {
                            //DataTable sampleInfoDT = await _testRepository.GetSampleInfo(info.Result.testid);//样本信息
                            var TestSampleInfo = await _testRepository.GetTestInfo(info.Result.testid);//样本信息
                            if(TestSampleInfo != null)
                            {
                                if (info.ResultState < 4)
                                {
                                    if (info.AutographInfo != null)
                                    {
                                        if (info.Result != null)
                                        {
                                            List<commReItemInfo> commReItemInfos = new List<commReItemInfo>();
                                            string resultRecordString = "";//项目结果修改记录
                                            string resultInfoString = "";//项目结果保存信息

                                            string listItemCode = "";
                                            int testState = 0;
                                            string testFlowNO = "1";
                                            string nextFlowNO = "0";
                                            bool? trequal = false;
                                            bool? tcequal = false;
                                            bool? rcequal = false;
                                            bool? reviewState = false;

                                            int perid = 0;
                                            int testid = 0;
                                            string sampleID = "";
                                            string barcode = "";
                                            string tester = "";
                                            string reTester = "";
                                            string checker = "";
                                            //DataRow sampleInfoDR;
                                            DateTime testTime = DateTime.MinValue;
                                            DateTime reTestTime = DateTime.MinValue;
                                            DateTime checkTime = DateTime.MinValue;
                                            if (info.Result.listResult != null)
                                            {

                                                foreach (MicrobeResultModel itemResult in info.Result.listResult)
                                                {
                                                    listItemCode += itemResult.itemCodes + ",";

                                                }
                                                if (listItemCode.Length > 0)
                                                {

                                                    DataTable oldTestInfoDT = await _testRepository.GetMicrobeInfo(info.Result.testid, listItemCode);//样本信息
                                                    DataTable oldItemInfoDT = await _testRepository.GetMicrobeItem(info.Result.testid, listItemCode);//样本信息



                                                    //sampleInfoDR = sampleInfoDT.Rows[0];
                                                    testid = TestSampleInfo.id;
                                                    perid = TestSampleInfo.perid !=null ? Convert.ToInt32(TestSampleInfo.perid) : 0;

                                                    sampleID = TestSampleInfo.sampleID !=null ? TestSampleInfo.sampleID.ToString() : "";


                                                    barcode = TestSampleInfo.barcode !=null ? TestSampleInfo.barcode.ToString() : "";

                                                    //读取样本状态
                                                    testState = TestSampleInfo.testStateNO !=null ? Convert.ToInt32(TestSampleInfo.testStateNO) : 0;
                                                    ////读取样本流程
                                                    //testFlowNO = TestSampleInfo.groupFlowNO !=null ? TestSampleInfo.groupFlowNO.ToString() : "1";
                                                    //读取专业组编号

                                                    string groupNO = TestSampleInfo.groupNO !=null ? TestSampleInfo.groupNO.ToString() : "0";


                                                    //片段标本状态是检验中和初审 审核  和已委托
                                                    if (testState < 4 || testState == 5)
                                                    {
                                                        tester = TestSampleInfo.tester !=null ? TestSampleInfo.tester.ToString() : "";
                                                        reTester = TestSampleInfo.reTester !=null ? TestSampleInfo.reTester.ToString() : "";
                                                        checker = TestSampleInfo.checker !=null ? TestSampleInfo.checker.ToString() : "";
                                                        testTime = TestSampleInfo.testTime !=null ? Convert.ToDateTime(TestSampleInfo.testTime) : DateTime.MinValue;
                                                        reTestTime = TestSampleInfo.reTestTime !=null ? Convert.ToDateTime(TestSampleInfo.reTestTime) : DateTime.MinValue;
                                                        checkTime = TestSampleInfo.checkTime !=null ? Convert.ToDateTime(TestSampleInfo.checkTime) : DateTime.MinValue;
                                                        
                                                        
                                                        List<comm_group_test> groupinfoss = ManualDataCache<comm_group_test>.MemoryCache.LIMSGetKeyValue(CommInfo.grouptest);
                                                        comm_group_test grouptestInfo = groupinfoss.FirstOrDefault(p => p.no == Convert.ToInt32(groupNO));



                                                        //if (testFlowInfo != null && grouptestInfo != null)
                                                        if (grouptestInfo != null)
                                                        {
                                                            trequal = grouptestInfo.trequal != null ? grouptestInfo.trequal : false;
                                                            tcequal = grouptestInfo.tcequal != null ? grouptestInfo.tcequal : false;
                                                            rcequal = grouptestInfo.rcequal != null ? grouptestInfo.rcequal : false;
                                                            reviewState = grouptestInfo.reviewState != null ? grouptestInfo.reviewState : false;




                                                            //nextFlowNO = testFlowInfo.nextFlow != null && testFlowInfo.nextFlow == "0" ? testFlowInfo.nextFlow : "0";
                                                            if (nextFlowNO != "0")
                                                            {
                                                                string sqlUpFlowNO = $"update WorkTest.SampleInfoFlow set dstate=1 where testid='{info.Result.testid}' and flowNO='{info.testFlowNO}';";
                                                                sqlUpFlowNO += "insert into WorkTest.SampleInfoFlow (perid,testid,barcode,flowNO,flowSort,creater,createTime,state,dstate)";
                                                                sqlUpFlowNO += $"values (0,'{testid}','{barcode}','{testFlowNO}',0,'{info.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
                                                                resultInfoString += sqlUpFlowNO;
                                                                commReInfo.nextFlowNO = nextFlowNO;

                                                            }
                                                            else
                                                            {
                                                                nextFlowNO = testFlowNO;
                                                            }
                                                            foreach (MicrobeResultModel resultMicrobe in info.Result.listResult)
                                                            {
                                                                Dictionary<string, object> pairsMicrobe = new Dictionary<string, object>();
                                                                pairsMicrobe.Add("delstate", 0);
                                                                pairsMicrobe.Add("dstate", 0);
                                                                pairsMicrobe.Add("reportState", resultMicrobe.reportState);
                                                                pairsMicrobe.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                                                pairsMicrobe.Add("itemSort", resultMicrobe.itemSort);
                                                                pairsMicrobe.Add("creater", info.UserName);
                                                                pairsMicrobe.Add("groupCode", resultMicrobe.groupCode);
                                                                pairsMicrobe.Add("groupName", resultMicrobe.groupName);
                                                                pairsMicrobe.Add("itemCodes", resultMicrobe.itemCodes);
                                                                pairsMicrobe.Add("itemNames", resultMicrobe.itemNames);
                                                                pairsMicrobe.Add("itemRemark", resultMicrobe.itemRemark);
                                                                pairsMicrobe.Add("itemResult", resultMicrobe.itemResult);
                                                                pairsMicrobe.Add("resultType", resultMicrobe.resultType);
                                                                pairsMicrobe.Add("perid", perid);
                                                                pairsMicrobe.Add("testid", testid);
                                                                pairsMicrobe.Add("groupNO", groupNO);

                                                                //pairsMicrobe.Add("resultNullState", dataTable.Rows[0]["resultNullState"]);
                                                                //pairsMicrobe.Add("state", dataTable.Rows[0]["state"]);
                                                                //pairsMicrobe.Add("id", dataTable.Rows[0]["id"]);
                                                                //pairsMicrobe.Add("diagnosis", dataTable.Rows[0]["diagnosis"]);
                                                                //pairsMicrobe.Add("diagnosisRemark", dataTable.Rows[0]["diagnosisRemark"]);

                                                                if (resultMicrobe.id == 0)
                                                                {
                                                                    iInfo iInfo = new iInfo();
                                                                    iInfo.TableName = "WorkTest.ResultMicrobeInfo";
                                                                    iInfo.values = pairsMicrobe;
                                                                    resultInfoString += SqlFormartHelper.insertFormart(iInfo);

                                                                }
                                                                else
                                                                {
                                                                    uInfo uInfo = new uInfo();
                                                                    uInfo.TableName = "WorkTest.ResultMicrobeInfo";
                                                                    uInfo.values = pairsMicrobe;
                                                                    uInfo.wheres = $"id='{resultMicrobe.id}'";
                                                                    resultInfoString += SqlFormartHelper.updateFormart(uInfo);

                                                                    if (oldTestInfoDT != null && oldTestInfoDT.Rows.Count > 0)
                                                                    {
                                                                        DataRow[] dataRows = oldTestInfoDT.Select($"id='{resultMicrobe.id}'");
                                                                        if (dataRows.Length > 0)
                                                                        {
                                                                            resultRecordString += dataRows[0]["resultType"] != DBNull.Value && dataRows[0]["resultType"].ToString() != resultMicrobe.resultType ? $"项目编号:{resultMicrobe.itemCodes}结果类型由：[{dataRows[0]["resultType"]}]更改为[{resultMicrobe.resultType}];" : "";
                                                                            resultRecordString += dataRows[0]["itemResult"] != DBNull.Value && dataRows[0]["itemResult"].ToString() != resultMicrobe.resultType ? $"项目编号:{resultMicrobe.itemCodes}结果由：[{dataRows[0]["itemResult"]}]更改为[{resultMicrobe.itemResult}];" : "";
                                                                            resultRecordString += dataRows[0]["itemRemark"] != DBNull.Value && dataRows[0]["itemRemark"].ToString() != resultMicrobe.itemRemark ? $"项目编号:{resultMicrobe.itemCodes}结果备注由：[{dataRows[0]["itemRemark"]}]更改为[{resultMicrobe.itemRemark}];" : "";
                                                                            //resultRecordString += dataRows[0]["resultType"]!=DBNull.Value&&dataRows[0]["resultType"].ToString()!= resultMicrobe.resultType ? $"项目编号:{resultMicrobe.itemCodes}结果类型由：[{dataRows[0]["resultType"]}]更改为[{resultMicrobe.resultType}];":"";
                                                                        }
                                                                    }
                                                                }
                                                                if (resultMicrobe.AntibioticInfos != null)
                                                                {
                                                                    foreach (MicrobeAntibioticModel resultAntibiotic in resultMicrobe.AntibioticInfos)
                                                                    {
                                                                        Dictionary<string, object> pairsAntibiotic = new Dictionary<string, object>();
                                                                        pairsAntibiotic.Add("dstate", 0);
                                                                        pairsAntibiotic.Add("state", 1);
                                                                        //pairsAntibiotic.Add("id", dataTable.Rows[0]["id"]);
                                                                        pairsAntibiotic.Add("itemSort", resultAntibiotic.itemSort);
                                                                        pairsAntibiotic.Add("antibioticEN", resultAntibiotic.antibioticEN);
                                                                        pairsAntibiotic.Add("antibioticNames", resultAntibiotic.antibioticNames);
                                                                        pairsAntibiotic.Add("antibioticNo", resultAntibiotic.antibioticNo);
                                                                        pairsAntibiotic.Add("aqualitative", resultAntibiotic.aqualitative);
                                                                        pairsAntibiotic.Add("channel", resultAntibiotic.channel);
                                                                        pairsAntibiotic.Add("itemCodes", resultAntibiotic.itemCodes);
                                                                        pairsAntibiotic.Add("itemNames", resultAntibiotic.itemNames);
                                                                        pairsAntibiotic.Add("itemResult", resultAntibiotic.itemResult);
                                                                        pairsAntibiotic.Add("kbValue", resultAntibiotic.kbValue);
                                                                        pairsAntibiotic.Add("methodName", resultAntibiotic.methodName);
                                                                        pairsAntibiotic.Add("micValue", resultAntibiotic.micValue);
                                                                        pairsAntibiotic.Add("remark", resultAntibiotic.remark);
                                                                        pairsAntibiotic.Add("perid", perid);
                                                                        pairsAntibiotic.Add("testid", testid);
                                                                        pairsAntibiotic.Add("barcode", barcode);
                                                                        pairsAntibiotic.Add("groupNO", groupNO);
                                                                        if (resultAntibiotic.id == 0)
                                                                        {
                                                                            iInfo iInfo = new iInfo();
                                                                            iInfo.TableName = "WorkTest.ResultMicrobeItem";
                                                                            iInfo.values = pairsAntibiotic;
                                                                            resultInfoString += SqlFormartHelper.insertFormart(iInfo);

                                                                        }
                                                                        else
                                                                        {
                                                                            uInfo uInfo = new uInfo();
                                                                            uInfo.TableName = "WorkTest.ResultMicrobeItem";
                                                                            uInfo.values = pairsAntibiotic;
                                                                            uInfo.wheres = $"id='{resultAntibiotic.id}'";
                                                                            resultInfoString += SqlFormartHelper.updateFormart(uInfo);

                                                                            DataRow[] dataRows = oldItemInfoDT.Select($"id='{resultMicrobe.id}'");
                                                                            if (dataRows.Length > 0)
                                                                            {
                                                                                resultRecordString += dataRows[0]["micValue"] != DBNull.Value && dataRows[0]["micValue"].ToString() != resultAntibiotic.micValue ? $"项目编号:{resultMicrobe.itemCodes}-抗生素编号:{resultAntibiotic.antibioticNo}-MIC值由：[{dataRows[0]["micValue"]}]更改为[{resultAntibiotic.micValue}];" : "";
                                                                                resultRecordString += dataRows[0]["kbValue"] != DBNull.Value && dataRows[0]["kbValue"].ToString() != resultAntibiotic.kbValue ? $"项目编号:{resultMicrobe.itemCodes}-抗生素编号:{resultAntibiotic.antibioticNo}-KB值由：[{dataRows[0]["kbValue"]}]更改为[{resultAntibiotic.kbValue}];" : "";
                                                                                resultRecordString += dataRows[0]["methodName"] != DBNull.Value && dataRows[0]["methodName"].ToString() != resultAntibiotic.methodName ? $"项目编号:{resultMicrobe.itemCodes}-抗生素编号:{resultAntibiotic.antibioticNo}-类型由：[{dataRows[0]["methodName"]}]更改为[{resultAntibiotic.methodName}];" : "";
                                                                                resultRecordString += dataRows[0]["itemResult"] != DBNull.Value && dataRows[0]["itemResult"].ToString() != resultAntibiotic.itemResult ? $"项目编号:{resultMicrobe.itemCodes}-抗生素编号:{resultAntibiotic.antibioticNo}-结果由：[{dataRows[0]["itemResult"]}]更改为[{resultAntibiotic.itemResult}];" : "";
                                                                                resultRecordString += dataRows[0]["remark"] != DBNull.Value && dataRows[0]["remark"].ToString() != resultAntibiotic.remark ? $"项目编号:{resultMicrobe.itemCodes}-抗生素编号:{resultAntibiotic.antibioticNo}-备注由：[{dataRows[0]["remark"]}]更改为[{resultAntibiotic.remark}];" : "";
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            commReInfo.code = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        commReInfo.code = 1;
                                                        commReInfo.msg = "请刷新当前样本状态";
                                                    }

                                                }
                                                else
                                                {

                                                    commReInfo.msg = "未找到对应项目信息";
                                                }
                                                if (resultRecordString.Length > 0)


                                                    await _recordRepository.SampleRecord(info.Result.barcode, RecordEnumVars.ResultEdit, resultRecordString, info.UserName, false);


                                            }
                                            if (commReInfo.code == 0)
                                            {
                                                if (resultInfoString != "")
                                                    await _commRepository.sqlcommand(resultInfoString);
                                                if (info.ResultState == 1)
                                                    jm = await _testSaveRepository.Test(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName, nextFlowNO.ToString());
                                                if (info.ResultState == 2)
                                                    jm = await _testSaveRepository.reTest(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName);
                                                if (info.ResultState == 3)
                                                    jm = await _testSaveRepository.Check(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName);

                                                //检验状态修改成功返回项目状态
                                                if (jm.code == 0)
                                                    jm.data = commReInfo;
                                                commReInfo.code = jm.code;

                                            }
                                            else
                                            {

                                                commReInfo.msg = "修改结果失败";
                                            }


                                        }
                                    }
                                    else
                                    {

                                        commReInfo.msg = "非法操作，身份验证失败";
                                    }
                                }
                                #region 反审结果信息
                                if (info.ResultState == 4)
                                {
                                    int reportState = TestSampleInfo.report!= null ? Convert.ToInt32(TestSampleInfo.report) : 0;
                                    jm = await _testSaveRepository.ReCheck(info.Result.perid.ToString(), info.Result.testid.ToString(), info.Result.barcode, info.UserName, reportState);
                                }
                                #endregion
                            }
                            else
                            {
                                commReInfo.code = 1;
                                commReInfo.msg = "未找到样本信息，修改结果失败";
                            }
                        }
                        else
                        {
                            commReInfo.code = 1;
                            commReInfo.msg = "样本信息错误，修改结果失败";
                        }
                    }
                    else
                    {
                        commReInfo.code = 1;
                        commReInfo.msg = "未提交样本信息，修改结果失败";
                    }
                }
                catch (Exception ex)
                {

                    commReInfo.msg = ex.Message;
                }
                jm.data = commReInfo;
            }
            return jm;
        }

        public async Task<WebApiCallBack> SaveGeneXG(CommResultModel<GeneInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            //using (await _mutex.LockAsync())
            //{
            commReInfo<commReItemInfo> commReInfo = new commReInfo<commReItemInfo>();
            try
            {

                {
                    if (info.Result != null)
                    {
                        if (info.Result.testid != 0)
                        {

                            //DataTable sampleInfoDT = await _testRepository.GetSampleInfo(info.Result.testid);//样本信息
                            var testinfo = await _testRepository.GetTestInfo(info.Result.testid);//样本信息

                            if (testinfo != null)
                            {
                                //判断结果是否已审核
                                if (info.ResultState < 4)
                                {
                                    if (info.AutographInfo != null)
                                    {
                                        if (info.Result.ListResult != null)
                                        {
                                            List<commReItemInfo> commReItemInfos = new List<commReItemInfo>();

                                            //string upsql = "";
                                            int testState = 0;
                                            string resultRecordString = "";//项目结果修改记录
                                            string resultInfoString = "";//项目结果保存信息
                                            string resultTableName = "WorkTest.ResultXG";
                                            if (info.Result.ListResult.Count > 0)
                                            {
                                                //遍历所有子项的信息
                                                foreach (GeneItemModel itemResult in info.Result.ListResult)
                                                {
                                                    //判断是否跳出内循环
                                                    if (commReInfo.code != 1)
                                                    {
                                                        commReItemInfo commReItemInfo = new commReItemInfo();
                                                        if (itemResult.itemResults.Count > 0)
                                                        {
                                                            //遍历子项相关的字段结果信息
                                                            foreach (GeneResultModel result in itemResult.itemResults)
                                                            {
                                                                if (result.value == null || result.value == "")
                                                                {
                                                                    commReInfo.code = 1;
                                                                    resultRecordString = "";
                                                                    commReItemInfo.code = 1;
                                                                    commReItemInfo.ItemNO = itemResult.itemCode;
                                                                    commReItemInfo.ItemMsg = "结果不能为空";
                                                                    commReItemInfos.Add(commReItemInfo);
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    commReItemInfo.code = 0;
                                                                    commReItemInfo.ItemNO = itemResult.itemCode;
                                                                    commReItemInfo.ItemMsg = "结果保存";
                                                                    resultRecordString += $"项目：{result.names}-{result.value};";
                                                                    resultInfoString += $"{result.key}='{result.value}',";

                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            commReInfo.msg = "请填写检验结果";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        resultRecordString = "";
                                                        break;
                                                    }
                                                }

                                                if (resultInfoString != "")
                                                {
                                                    string slqcomm = $"update {resultTableName} set {resultInfoString.Substring(0, resultInfoString.Length - 1)} where testid='{info.Result.testid}';";
                                                    await _commRepository.sqlcommand(slqcomm);

                                                }
                                                if (resultRecordString.Length > 0)
                                                    await _recordRepository.SampleRecord(info.Result.barcode, RecordEnumVars.ResultEdit, resultRecordString, info.UserName, false);

                                                testState = testinfo.testStateNO != null ? Convert.ToInt32(testinfo.testStateNO) : 1;
                                                int groupFlowNO = testinfo.groupFlowNO !=null? Convert.ToInt32(testinfo.groupFlowNO) : 0;
                                                if (info.ResultState == 1)
                                                    jm = await _testSaveRepository.Test(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName, groupFlowNO.ToString());
                                                if (info.ResultState == 2)
                                                    jm = await _testSaveRepository.reTest(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName);
                                                if (info.ResultState == 3)
                                                    jm = await _testSaveRepository.Check(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName);
                                                //检验状态修改成功返回项目状态
                                                if (jm.code == 0)
                                                    jm.data = commReInfo;
                                                commReInfo.code = jm.code;




                                            }
                                            else
                                            {

                                                commReInfo.msg = "提交项目信息，修改结果失败";
                                            }
                                        }
                                        else
                                        {

                                            commReInfo.msg = "未提交检验者信息";
                                        }
                                    }


                                }
                                #region 反审结果信息
                                if (info.ResultState == 4)
                                {
                                    int reportState = testinfo.report != null ? Convert.ToInt32(testinfo.report) : 0;
                                    jm = await _testSaveRepository.ReCheck(info.Result.perid.ToString(), info.Result.testid.ToString(), info.Result.barcode, info.UserName, reportState);
                                }
                                #endregion

                            }
                            else
                            {

                                commReInfo.msg = "未找到样本信息，修改结果失败";
                            }
                        }
                        else
                        {

                            commReInfo.msg = "未找到样本信息，修改结果失败";
                        }
                    }
                    else
                    {

                        commReInfo.msg = "未提交样本信息，修改结果失败";
                    }

                }

            }
            catch (Exception ex)
            {

                commReInfo.msg = ex.Message;
            }
            jm.data = commReInfo;
            //}
            return jm;
        }

        public async Task<WebApiCallBack> SaveGene(CommResultModel<GeneInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                commReInfo<commReItemInfo> commReInfo = new commReInfo<commReItemInfo>();
                //CommResultModel<GeneInfoModel> info = JsonHandle.JsonConvertObject<CommResultModel<GeneInfoModel>>(TestInfo);
                try
                {

                    {
                        if (info.Result != null)
                        {
                            if (info.Result.testid != 0)
                            {
                                var testinfo = await _testRepository.GetTestInfo(info.Result.testid);

                                if (info.ResultState < 4)
                                {
                                    if (info.AutographInfo != null)
                                    {
                                        if (info.Result.ListResult != null)
                                        {
                                            List<commReItemInfo> commReItemInfos = new List<commReItemInfo>();
                                            string resultRecordString = "";//项目结果修改记录
                                            string resultInfoString = "";//项目结果保存信息
                                            string listItemCode = "";
                                            int testState = 0;
                                            string testFlowNO = "1";
                                            string nextFlowNO = "0";
                                            bool trequal = false;
                                            bool tcequal = false;
                                            bool rcequal = false;
                                            bool reviewState = false;

                                            int perid = 0;

                                            int testid = 0;
                                            string sampleid = "";
                                            string barcode = "";
                                            string tester = "";
                                            string reTester = "";
                                            string checker = "";

                                            DataRow sampleInfoDR = null;
                                            DateTime testTime = DateTime.MinValue;
                                            DateTime reTestTime = DateTime.MinValue;
                                            DateTime checkTime = DateTime.MinValue;


                                            foreach (GeneItemModel itemResult in info.Result.ListResult)
                                            {
                                                listItemCode += itemResult.itemCode + ",";

                                            }
                                            if (listItemCode.Length > 0)
                                            {

                                                //var testinfo = await DbClient.Queryable<test_sampleInfo>().FirstAsync(p => p.id == info.Result.testid);
                                                
                                                if (testinfo != null)
                                                {

                                                    testid =testinfo.id;

                                                    perid = testinfo.perid != null ? Convert.ToInt32(testinfo.perid) : 0;
                                                    sampleid = testinfo.sampleID != null ? testinfo.sampleID : "";
                                                    barcode = testinfo.barcode != null ? testinfo.barcode : "";
                                                    //读取样本状态
                                                    testState = testinfo.testStateNO != null ? testinfo.testStateNO : 0;
                                                    //读取样本流程
                                                    testFlowNO = testinfo.groupFlowNO != null ? testinfo.groupFlowNO : "1";
                                                    //读取专业组编号
                                                    string groupNO = testinfo.groupNO != null ? testinfo.groupNO : "0";
                                                    //片段标本状态是检验中和初审 审核  和已委托
                                                    if (testState < 4 || testState == 5)
                                                    {
                                                        tester = testinfo.tester != null ? testinfo.tester : "";
                                                        reTester = testinfo.reTester != null ? testinfo.reTester : "";
                                                        checker = testinfo.checker != null ? testinfo.checker : "";
                                                        testTime = testinfo.testTime != null ? Convert.ToDateTime(testinfo.testTime) : DateTime.MinValue;
                                                        reTestTime = testinfo.reTestTime != null ? Convert.ToDateTime(testinfo.reTestTime) : DateTime.MinValue;
                                                        checkTime = testinfo.checkTime != null ? Convert.ToDateTime(testinfo.checkTime) : DateTime.MinValue;

                                                        //List<comm_item_flow> FlowInfoss = await _itemFlowRepository.GetCaChe();
                                                        List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
                                                        comm_item_flow testFlowInfo = FlowInfoss.First(p => p.no == Convert.ToInt32(testFlowNO));

                                                        //List<comm_group_test> groupNODT = await _groupTestRepository.GetCaChe();
                                                        List<comm_group_test> groupinfoss = ManualDataCache<comm_group_test>.MemoryCache.LIMSGetKeyValue(CommInfo.grouptest);
                                                        comm_group_test grouptestInfo = groupinfoss.First(p => p.no == Convert.ToInt32(groupNO));


                                                        if (FlowInfoss.Count() > 0 && groupinfoss.Count() > 0)
                                                        {

                                                            trequal = grouptestInfo.trequal != null ? Convert.ToBoolean(grouptestInfo.trequal) : false;
                                                            tcequal = grouptestInfo.tcequal != null ? Convert.ToBoolean(grouptestInfo.tcequal) : false;
                                                            rcequal = grouptestInfo.rcequal != null ? Convert.ToBoolean(grouptestInfo.rcequal) : false;
                                                            reviewState = grouptestInfo.reviewState != null ? Convert.ToBoolean(grouptestInfo.reviewState) : false;




                                                            string resultTableName = testFlowInfo.dataSource != null ? testFlowInfo.dataSource.ToString().Trim() : "";
                                                            string resultImgTableName = testFlowInfo.imgSource != null ? testFlowInfo.imgSource.ToString().Trim() : "";
                                                            nextFlowNO = testFlowInfo.nextFlow != null && testFlowInfo.nextFlow.ToString() != "0" ? testFlowInfo.nextFlow.ToString() : "0";
                                                            string resultTableSql = $"select * from {resultTableName} with(updlock) where testid='{info.Result.testid}' and barcode='{info.Result.barcode}';";
                                                            DataTable oldTestInfoDT = await _commRepository.GetTable(resultTableSql);
                                                            DataTable oldImgInfoDT = null;
                                                            if (!string.IsNullOrEmpty(resultImgTableName))
                                                            {
                                                                string resultImgSql = $"select * from {resultImgTableName} with(updlock) where testid='{info.Result.testid}' and barcode='{info.Result.barcode}';";
                                                                //oldImgInfoDT = await _commRepository.GetTable(resultImgSql);
                                                                oldImgInfoDT = await _commRepository.GetTable(resultImgSql);
                                                            }
                                                            nextFlowNO = nextFlowNO != null && nextFlowNO != "" ? nextFlowNO : "0";

                                                            if (nextFlowNO != "0")
                                                            {
                                                                string sqlUpFlowNO = $"update WorkTest.SampleInfoFlow set dstate=1 where testid='{info.Result.testid}' and flowNO='{info.testFlowNO}';";
                                                                sqlUpFlowNO += "insert into WorkTest.SampleInfoFlow (perid,testid,barcode,flowNO,flowSort,creater,createTime,state,dstate)";
                                                                sqlUpFlowNO += $"values (0,'{testid}','{barcode}','{testFlowNO}',0,'{info.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
                                                                resultInfoString += sqlUpFlowNO;
                                                                commReInfo.nextFlowNO = nextFlowNO;
                                                            }
                                                            else
                                                            {
                                                                nextFlowNO = testFlowNO;
                                                            }
                                                            if (oldTestInfoDT != null || oldImgInfoDT != null)
                                                            {
                                                                if (oldTestInfoDT != null && oldTestInfoDT.Rows.Count > 0)
                                                                {
                                                                    foreach (GeneItemModel itemResult in info.Result.ListResult)
                                                                    {
                                                                        commReItemInfo commReItemInfo = new commReItemInfo();
                                                                        DataRow[] itemDR = oldTestInfoDT.Select($"itemCodes='{itemResult.itemCode}'");

                                                                        if (itemDR != null)
                                                                        {

                                                                            string itemNames = itemDR[0]["itemNames"] != DBNull.Value ? itemDR[0]["itemNames"].ToString() : "";

                                                                            bool itemNullState = itemDR[0]["resultNullState"] != DBNull.Value ? Convert.ToBoolean(itemDR[0]["resultNullState"]) : false;
                                                                            //string oldItemResult = itemDR[0]["itemResult"] != DBNull.Value ? itemDR[0]["itemResult"].ToString() : "";

                                                                            if (!itemNullState)
                                                                            {

                                                                                if (itemResult.itemResults.Count > 0)
                                                                                {

                                                                                    //if (itemResult.itemResults != oldItemResult)
                                                                                    //{
                                                                                    string upsql = "";
                                                                                    string uplog = "";
                                                                                    foreach (GeneResultModel result in itemResult.itemResults)
                                                                                    {
                                                                                        if (result.value == null || result.value == "")
                                                                                        {
                                                                                            commReInfo.code = 2;
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "结果不能为空";
                                                                                            commReItemInfo.state = 0;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                            break;
                                                                                        }
                                                                                        else
                                                                                        {

                                                                                            if (oldTestInfoDT.Columns.Contains(result.key))
                                                                                            {
                                                                                                upsql += $"{result.key}='{result.value}',";

                                                                                                string oldItemResult = itemDR[0][result.key] != DBNull.Value ? itemDR[0][result.key].ToString() : "";

                                                                                                if (oldItemResult != "")
                                                                                                {
                                                                                                    if (result.value != oldItemResult)
                                                                                                    {
                                                                                                        uplog += $"[{result.names}:{oldItemResult}]更改为[{result.value}]";
                                                                                                    }
                                                                                                }
                                                                                            }

                                                                                        }

                                                                                    }
                                                                                    if (uplog != "")
                                                                                    {
                                                                                        if (upsql != "")
                                                                                        {
                                                                                            resultRecordString += $"项目编号:{itemResult.itemCode}:{itemNames}结果由：{uplog};";
                                                                                            resultInfoString += $"update {resultTableName} set {upsql.Substring(0, upsql.Length - 1)} where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';";
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "更新结果成功";
                                                                                            commReItemInfo.state = 1;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            commReInfo.code = 2;
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "结果不能为空";
                                                                                            commReItemInfo.state = 0;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                            break;
                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (upsql != "")
                                                                                        {
                                                                                            //resultRecordString += $"项目编号:{itemResult.itemCode}结果由：[{uplog}];";
                                                                                            resultInfoString += $"update {resultTableName} set {upsql.Substring(0, upsql.Length - 1)} where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';";
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "更新结果成功";
                                                                                            commReItemInfo.state = 1;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            commReInfo.code = 2;
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "结果不能为空";
                                                                                            commReItemInfo.state = 0;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                            break;
                                                                                        }
                                                                                    }

                                                                                }
                                                                                else
                                                                                {

                                                                                    commReInfo.msg = "未读取到结果信息";
                                                                                }

                                                                            }
                                                                            else
                                                                            {

                                                                                if (itemResult.itemResults.Count > 0)
                                                                                {

                                                                                    //if (itemResult.itemResults != oldItemResult)
                                                                                    //{
                                                                                    string upsql = "";
                                                                                    string uplog = "";
                                                                                    foreach (GeneResultModel result in itemResult.itemResults)
                                                                                    {

                                                                                        if (oldTestInfoDT.Columns.Contains(result.key))
                                                                                        {
                                                                                            string itemResultaaa = result.value != null ? result.value : "";
                                                                                            upsql += $"{result.key}='{itemResultaaa}',";

                                                                                            string oldItemResult = itemDR[0][result.key] != DBNull.Value ? itemDR[0][result.key].ToString() : "";


                                                                                            if (oldItemResult != result.value)
                                                                                            {
                                                                                                uplog += $"{result.names}:{oldItemResult}]更改为{result.value},";
                                                                                            }
                                                                                        }

                                                                                    }
                                                                                    if (uplog != "")
                                                                                    {
                                                                                        if (upsql != "")
                                                                                        {
                                                                                            resultRecordString += $"项目编号:{itemResult.itemCode}结果由：[{uplog}];";
                                                                                            resultInfoString += $"update {resultTableName} set {upsql.Substring(0, upsql.Length - 1)} where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';";
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "更新结果成功";
                                                                                            commReItemInfo.state = 1;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            commReInfo.code = 2;
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "结果不能为空";
                                                                                            commReItemInfo.state = 0;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                            break;
                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (upsql != "")
                                                                                        {
                                                                                            //resultRecordString += $"项目编号:{itemResult.itemCode}结果由：[{uplog}];";
                                                                                            resultInfoString += $"update {resultTableName} set {upsql.Substring(0, upsql.Length - 1)} where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';";
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "更新结果成功";
                                                                                            commReItemInfo.state = 1;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            commReInfo.code = 2;
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "结果不能为空";
                                                                                            commReItemInfo.state = 0;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                            break;
                                                                                        }
                                                                                    }

                                                                                }
                                                                                else
                                                                                {

                                                                                    commReInfo.msg = "未读取到上传结果";
                                                                                }


                                                                            }

                                                                        }
                                                                        else
                                                                        {

                                                                            commReInfo.msg = "未找到样本记录";
                                                                        }
                                                                    }



                                                                }
                                                                ///检验结果图片
                                                                if (oldImgInfoDT != null && oldImgInfoDT.Rows.Count > 0)
                                                                {
                                                                    foreach (GeneItemModel itemResult in info.Result.ListResult)
                                                                    {
                                                                        commReItemInfo commReItemInfo = new commReItemInfo();

                                                                        DataRow[] itemDR = oldTestInfoDT.Select($"itemCodes='{itemResult.itemCode}'");


                                                                        if (itemDR != null)
                                                                        {

                                                                            string itemNames = itemDR[0]["itemNames"] != DBNull.Value ? itemDR[0]["itemNames"].ToString() : "";

                                                                            bool itemNullState = itemDR[0]["resultNullState"] != DBNull.Value ? Convert.ToBoolean(itemDR[0]["resultNullState"]) : false;
                                                                            //string oldItemResult = itemDR[0]["itemResult"] != DBNull.Value ? itemDR[0]["itemResult"].ToString() : "";

                                                                            if (!itemNullState)
                                                                            {

                                                                                if (itemResult.itemResults.Count > 0)
                                                                                {

                                                                                    //if (itemResult.itemResults != oldItemResult)
                                                                                    //{
                                                                                    string upsql = "";
                                                                                    string uplog = "";
                                                                                    foreach (GeneResultModel result in itemResult.itemResults)
                                                                                    {
                                                                                        if (result.value == null || result.value == "")
                                                                                        {
                                                                                            commReInfo.code = 2;
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "结果不能为空";
                                                                                            commReItemInfo.state = 0;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                            break;
                                                                                        }
                                                                                        else
                                                                                        {

                                                                                            if (oldImgInfoDT.Columns.Contains(result.key))
                                                                                            {
                                                                                                string imgName = barcode + "-" + Guid.NewGuid().ToString() + ".png";
                                                                                                string itemResultaaa = result.value != null ? DateTime.Now.ToString("yyyy-MM-dd") + "\\" + imgName : "";

                                                                                                if (itemResultaaa != "")
                                                                                                {

                                                                                                    if (!Directory.Exists(AppSettingsConstVars.TestFilePath))
                                                                                                    {
                                                                                                        Directory.CreateDirectory(AppSettingsConstVars.TestFilePath);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        string dirpath = AppSettingsConstVars.TestFilePath + "\\" + resultImgTableName;
                                                                                                        if (!Directory.Exists(dirpath))
                                                                                                        {
                                                                                                            Directory.CreateDirectory(dirpath);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            string filepath = dirpath + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                                                                                                            if (!Directory.Exists(filepath))
                                                                                                            {
                                                                                                                Directory.CreateDirectory(filepath);
                                                                                                            }
                                                                                                            string fileFullPath = filepath + "\\" + imgName;

                                                                                                            if (await _fileHandleServices.fileSave(fileFullPath, result.value))
                                                                                                            {
                                                                                                                upsql += $"{result.key}='{itemResultaaa}',";

                                                                                                                string oldItemResult = itemDR[0][result.key] != DBNull.Value ? itemDR[0][result.key].ToString() : "";

                                                                                                                if (oldItemResult != result.value)
                                                                                                                {
                                                                                                                    uplog += $"{result.names}:{oldItemResult}]更改为{itemResultaaa},";
                                                                                                                }
                                                                                                            }

                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    upsql += $"{result.key}='{itemResultaaa}',";

                                                                                                    string oldItemResult = itemDR[0][result.key] != DBNull.Value ? itemDR[0][result.key].ToString() : "";

                                                                                                    if (oldItemResult != result.value)
                                                                                                    {
                                                                                                        uplog += $"{result.names}:{oldItemResult}]更改为{itemResultaaa},";
                                                                                                    }
                                                                                                }


                                                                                            }

                                                                                        }

                                                                                    }
                                                                                    if (uplog != "")
                                                                                    {
                                                                                        if (upsql != "")
                                                                                        {
                                                                                            resultRecordString += $"项目编号:{itemResult.itemCode}:{itemNames}结果由：{uplog};";
                                                                                            resultInfoString += $"update {resultImgTableName} set {upsql.Substring(0, upsql.Length - 1)} where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';";
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "更新结果成功";
                                                                                            commReItemInfo.state = 1;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            commReInfo.code = 2;
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "结果不能为空";
                                                                                            commReItemInfo.state = 0;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                            break;
                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (upsql != "")
                                                                                        {
                                                                                            //resultRecordString += $"项目编号:{itemResult.itemCode}结果由：[{uplog}];";
                                                                                            resultInfoString += $"update {resultImgTableName} set {upsql.Substring(0, upsql.Length - 1)} where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';";
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "更新结果成功";
                                                                                            commReItemInfo.state = 1;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            commReInfo.code = 2;
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "结果不能为空";
                                                                                            commReItemInfo.state = 0;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                            break;
                                                                                        }
                                                                                    }

                                                                                }
                                                                                else
                                                                                {

                                                                                    commReInfo.msg = "未读取到结果信息";
                                                                                }

                                                                            }
                                                                            else
                                                                            {

                                                                                if (itemResult.itemResults.Count > 0)
                                                                                {

                                                                                    //if (itemResult.itemResults != oldItemResult)
                                                                                    //{
                                                                                    string upsql = "";
                                                                                    string uplog = "";
                                                                                    foreach (GeneResultModel result in itemResult.itemResults)
                                                                                    {

                                                                                        if (oldImgInfoDT.Columns.Contains(result.key))
                                                                                        {
                                                                                            string imgName = barcode + "-" + Guid.NewGuid().ToString() + ".png";
                                                                                            string itemResultaaa = result.value != null ? DateTime.Now.ToString("yyyy-MM-dd") + "\\" + imgName : "";

                                                                                            if (itemResultaaa != "")
                                                                                            {

                                                                                                if (!Directory.Exists(AppSettingsConstVars.TestFilePath))
                                                                                                {
                                                                                                    Directory.CreateDirectory(AppSettingsConstVars.TestFilePath);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    string dirpath = AppSettingsConstVars.TestFilePath + "\\" + resultImgTableName;
                                                                                                    if (!Directory.Exists(dirpath))
                                                                                                    {
                                                                                                        Directory.CreateDirectory(dirpath);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        string filepath = dirpath + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                                                                                                        if (!Directory.Exists(filepath))
                                                                                                        {
                                                                                                            Directory.CreateDirectory(filepath);
                                                                                                        }
                                                                                                        string fileFullPath = filepath + "\\" + imgName;

                                                                                                        if (await _fileHandleServices.fileSave(fileFullPath, result.value))
                                                                                                        {
                                                                                                            upsql += $"{result.key}='{itemResultaaa}',";

                                                                                                            string oldItemResult = itemDR[0][result.key] != DBNull.Value ? itemDR[0][result.key].ToString() : "";

                                                                                                            if (oldItemResult != result.value)
                                                                                                            {
                                                                                                                uplog += $"{result.names}:{oldItemResult}]更改为{itemResultaaa},";
                                                                                                            }
                                                                                                        }

                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                upsql += $"{result.key}='{itemResultaaa}',";

                                                                                                string oldItemResult = itemDR[0][result.key] != DBNull.Value ? itemDR[0][result.key].ToString() : "";

                                                                                                if (oldItemResult != result.value)
                                                                                                {
                                                                                                    uplog += $"{result.names}:{oldItemResult}]更改为{itemResultaaa},";
                                                                                                }
                                                                                            }


                                                                                        }

                                                                                    }
                                                                                    if (uplog != "")
                                                                                    {
                                                                                        if (upsql != "")
                                                                                        {
                                                                                            resultRecordString += $"项目编号:{itemResult.itemCode}结果由：[{uplog}];";
                                                                                            resultInfoString += $"update {resultImgTableName} set {upsql.Substring(0, upsql.Length - 1)} where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';";
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "更新结果成功";
                                                                                            commReItemInfo.state = 1;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            commReInfo.code = 2;
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "结果不能为空";
                                                                                            commReItemInfo.state = 0;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                            break;
                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (upsql != "")
                                                                                        {
                                                                                            //resultRecordString += $"项目编号:{itemResult.itemCode}结果由：[{uplog}];";
                                                                                            resultInfoString += $"update {resultImgTableName} set {upsql.Substring(0, upsql.Length - 1)} where testid='{info.Result.testid}' and itemCodes='{itemResult.itemCode}';";
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "更新结果成功";
                                                                                            commReItemInfo.state = 1;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            commReInfo.code = 2;
                                                                                            commReItemInfo.ItemNO = itemResult.itemCode;
                                                                                            commReItemInfo.ItemMsg = "结果不能为空";
                                                                                            commReItemInfo.state = 0;
                                                                                            commReItemInfos.Add(commReItemInfo);
                                                                                            break;
                                                                                        }
                                                                                    }

                                                                                }
                                                                                else
                                                                                {

                                                                                    commReInfo.msg = "未读取到上传结果";
                                                                                }


                                                                            }

                                                                        }
                                                                        else
                                                                        {

                                                                            commReInfo.msg = "未找到样本记录";
                                                                        }
                                                                    }
                                                                }
                                                                if (resultRecordString.Length > 0)
                                                                    await _recordRepository.SampleRecord(info.Result.barcode, RecordEnumVars.ResultEdit, resultRecordString, info.UserName, false);
                                                                if (commReInfo.code == 0)
                                                                {
                                                                    if (resultInfoString != "")
                                                                    {
                                                                        await _commRepository.sqlcommand(resultInfoString);

                                                                    }
                                                                    if (info.ResultState == 1)
                                                                       jm = await _testSaveRepository.Test(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName, nextFlowNO.ToString());
                                                                   if (info.ResultState == 2)
                                                                       jm = await _testSaveRepository.reTest(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName);
                                                                   if (info.ResultState == 3)
                                                                        jm = await _testSaveRepository.Check(info.AutographInfo, info.Result.perid, info.Result.testid, info.Result.barcode, info.UserName);


                                                                    //检验状态修改成功返回项目状态
                                                                    if (jm.code == 0)
                                                                        jm.data = commReInfo;
                                                                    commReInfo.code = jm.code;
                                                                }
                                                                else
                                                                {

                                                                    commReInfo.msg = "修改结果失败";
                                                                }
                                                            }
                                                            else
                                                            {

                                                                if (FlowInfoss.Count() == 0)
                                                                    commReInfo.msg = "未找到匹配的流程信息,请检查流程配置";
                                                                if (groupinfoss.Count() == 0)
                                                                    commReInfo.msg = "未找到匹配的专业组信息,请检查专业组配置";
                                                            }



                                                        }
                                                        else
                                                        {


                                                            if (FlowInfoss.Count() == 0)
                                                                commReInfo.msg = "未找到匹配的流程信息,请检查流程配置";
                                                            if (groupinfoss.Count() == 0)
                                                                commReInfo.msg = "未找到匹配的专业组信息,请检查专业组配置";
                                                        }
                                                    }
                                                    else
                                                    {

                                                        commReInfo.msg = "请刷新当前样本状态";
                                                    }
                                                }
                                                else
                                                {

                                                    commReInfo.msg = "未提交检验者信息";
                                                }
                                            }
                                            else
                                            {

                                                commReInfo.msg = "未找到对应项目信息";
                                            }
                                        }
                                        else
                                        {

                                            commReInfo.msg = "提交项目信息，修改结果失败";
                                        }
                                    }
                                    else
                                    {

                                        commReInfo.msg = "未提交检验者信息";
                                    }
                                }
                                #region 反审结果信息
                                if (info.ResultState == 4)
                                {
                                    int reportState = testinfo.report!= null ? Convert.ToInt32(testinfo.report) : 0;
                                    jm = await _testSaveRepository.ReCheck(info.Result.perid.ToString(), info.Result.testid.ToString(), info.Result.barcode, info.UserName, reportState);
                                }
                                #endregion

                            }
                            else
                            {

                                commReInfo.msg = "样本信息错误，修改结果失败";
                            }
                        }
                        else
                        {

                            commReInfo.msg = "未找到样本信息，修改结果失败";
                        }
                    }

                }
                catch (Exception ex)
                {

                    commReInfo.msg = ex.Message;
                }
                jm.data = commReInfo;
            }
            return jm;
        }
    }
}
