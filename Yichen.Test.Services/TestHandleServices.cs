using Nito.AsyncEx;
using SqlSugar;
using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.Flow.Model;
using Yichen.Net.Caching.Manuals;
using Yichen.Net.Configuration;
using Yichen.Net.Data;
using Yichen.Net.Model.Entities.Expression;
using Yichen.Net.Table;
using Yichen.Other.IRepository;
using Yichen.Per.IRepository;
using Yichen.System.IRepository;
using Yichen.System.Model;
using Yichen.Test.IRepository;
using Yichen.Test.IServices;
using Yichen.Test.Model;
using Yichen.Test.Model.Result;
using Yichen.Test.Model.table;

namespace Yichen.Test.Services
{
    public class TestHandleServices : BaseServices<test_sampleInfo>, ITestHandleServices
    {

        private readonly AsyncLock _mutex = new AsyncLock();
        public readonly IUnitOfWork _unitOfWork;
        public readonly IRecordRepository _recordRepository;

        public readonly ITestItemInfoRepository _itemCrisisRepository;
        public readonly ITestSampleInfoRepository _testSampleInfoRepository;
        public readonly IItemHandleServices _itemHandleServices;
        private readonly ICommRepository _commRepository;
        private readonly IItemFlowRepository _itemFlowRepository;

        public TestHandleServices(IUnitOfWork unitOfWork
            , IRecordRepository recordRepository
            , ITestItemInfoRepository itemCrisisRepository
            , ITestSampleInfoRepository testSampleInfoRepository
            , IItemHandleServices itemHandleServices
            , ICommRepository commRepository
             , IItemFlowRepository itemFlowRepository
            )
        {
            _unitOfWork = unitOfWork;
            _recordRepository = recordRepository;
            _itemCrisisRepository = itemCrisisRepository;
            _itemHandleServices = itemHandleServices;
            _commRepository = commRepository;
            _itemFlowRepository = itemFlowRepository;
            _testSampleInfoRepository= testSampleInfoRepository;
        }


        ///// <summary>
        ///// 插入检验中样本信息
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //public async Task<WebApiCallBack> AddTestInfo(test_sampleInfo infos)
        //{
        //    WebApiCallBack jm = new WebApiCallBack();
        //    try
        //    {
        //        jm.code = 0;
        //        jm.data = DbClient.Insertable(infos).ExecuteReturnBigIdentity();

        //    }
        //    catch (Exception ex)
        //    {
        //        jm.code = 1;
        //        jm.msg = ex.Message;
        //    }
        //    return jm;

        //}


        /// <summary>
        /// 获取检验中的样本信息（专业组）
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetTestInfo(GetTestInfoModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack() { code=0,status=true};
            using (await _mutex.LockAsync())
            {
                try
                {
                    if (infos.GroupNO == null || infos.StartTime == null || infos.EndTime == null)
                    {
                        jm.code = 1;
                        jm.status = false;
                        jm.msg = "请选择专业组";
                    }
                    else
                    {

                        var wheres = PredicateBuilder.True<test_sampleInfo>();
                        wheres = wheres.And(p => p.visible == true);
                        wheres = wheres.And(p => p.dstate == false);
                        wheres = wheres.And(p => p.groupNO == infos.GroupNO);
                        if (infos.GroupNO != "9")
                            wheres = wheres.And(p => p.state == true);
                        if (infos.FlowNO != null && infos.FlowNO != "")
                            wheres = wheres.And(p => p.groupFlowNO == infos.FlowNO);
                        if (infos.frameNo != null && infos.frameNo != "")
                            wheres = wheres.And(p => p.frameNo == infos.frameNo);
                        if (infos.barcode != null && infos.barcode.Count > 0)
                        {
                            if (infos.barcode.Count == 1)
                            {
                                wheres = wheres.And(p => p.barcode.Contains(infos.barcode[0]));
                                
                            }
                            else
                            {
                                wheres = wheres.And(p => infos.barcode.Contains(p.barcode));
                            }

                        }
                       wheres = wheres.And(p => SqlFunc.Between(p.reachTime, Convert.ToDateTime(infos.StartTime), Convert.ToDateTime(infos.EndTime)));
                       DataTable infoDT= await  _testSampleInfoRepository.QueryDTByClauseAsync(wheres, true);
                       jm.data=DataTableHelper.DTToString(infoDT);

                    }
                }
                catch (Exception ex)
                {
                    jm.code = 1;
                    jm.status = false;
                    jm.msg = ex.Message;
                }
            }
            return jm;
        }
        /// <summary>
        /// 获取检验中的样本项目（专业组）
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetItemInfo(GetItemInfoModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                try
                {

                    DataSet dataSet = null;


                    string flowNO = infos.FlowNO != null ? infos.FlowNO.ToString() : "1";
                    //int ResultState = infos.ResultState != null ? Convert.ToInt32(infos.ResultState) :0;
                    //string flowInfo = $"select * from WorkComm.ItemFlow where no={flowNO}";
                    //DataTable DTFlow = DbClient.Ado.GetDataTable( flowInfo);


                    //读取子项信息获取流程集合
                    //List<comm_item_flow> FlowInfoss = await _itemFlowRepository.GetCaChe();
                    List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
                    //查询样本信息中包含的流程信息 并按照专业组进行排序
                    comm_item_flow FlowInfos = FlowInfoss.First(p => p.no == Convert.ToInt32(flowNO) && p.dstate == false);
                    //DataRow[] DTFlow = DTFlows.Select($"no={flowNO}");

                    if (FlowInfos != null)
                    {
                        string dataSource = FlowInfos.dataSource != null ? FlowInfos.dataSource.ToString() : "";
                        string imgSource = FlowInfos.imgSource != null ? FlowInfos.imgSource.ToString() : "";
                        if (dataSource != "")
                        {

                            string dataSql = "";
                            string imgSql = "";
                            string[] dataSources = dataSource.Split(',');

                            foreach (string Sources in dataSources)
                            {
                                if (infos.GroupNO != "1")
                                {
                                    dataSql += $"select * from {Sources} where testid={infos.Testid} and groupNO='{infos.GroupNO}' and state=1 and dstate=0 order by itemSort ;";
                                }
                                else
                                {
                                    dataSql += $"select * from {Sources} where testid={infos.Testid} and groupNO='{infos.GroupNO}' and state=1 and dstate=0 and delstate=1 order by itemSort ;";
                                }
                            }

                            if (dataSql != "")
                            {
                                string[] imgSources = imgSource.Split(',');
                                foreach (string imgs in imgSources)
                                {
                                    imgSql += $"select * from {imgs} where testid={infos.Testid} and groupNO='{infos.GroupNO}'and state=1 and dstate=0 order by sort;";
                                }
                                try
                                {
                                    dataSet =await _commRepository.GetDataSet(dataSql + imgSql);
                                    int a = 0;
                                    foreach (DataTable dt in dataSet.Tables)
                                    {
                                        if (a == dataSources.Length)
                                        {
                                            dt.TableName = dataSources[a];
                                        }
                                        else
                                        {
                                            dt.TableName = imgSources[a - dataSources.Length];
                                        }
                                        a = a + 1;
                                    }
                                }
                                catch
                                {
                                    dataSet = await _commRepository.GetDataSet(dataSql);
                                    int a = 0;
                                    foreach (DataTable dt in dataSet.Tables)
                                    {
                                        dt.TableName = dataSources[a];
                                        a = a + 1;
                                    }
                                }
                            }
                        }
                    }

                    jm.data = dataSet;

                }
                catch
                {

                    return null;

                }
            }
            return jm;
        }



        /// <summary>
        /// 获取微生物结果
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetTestMicrobeInfo(commInfoModel<GetMicrobeItemModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            if (infos.infos != null)
            {
                if (infos.infos.Count > 0)
                {
                    commReInfo<MicrobeInfoModel> commReInfo = new commReInfo<MicrobeInfoModel>();
                    List<MicrobeInfoModel> testInfos = new List<MicrobeInfoModel>();
                    foreach (GetMicrobeItemModel testWork in infos.infos)
                    {

                        //testinfo.listResult = GetMicrobeInfo.GetInfo(testItem.testid, testItem.Barcode, testItem.groupNO, testItem.flowNO);
                        string sql1 = $"select  * from  WorkTest.ResultMicrobeInfo where testid='{testWork.testid}' and groupNO='{testWork.groupNO}' and state=1 and dstate=0 order by itemSort;";
                        string sql2 = $"select  * from  WorkTest.ResultMicrobeItem where testid='{testWork.testid}' and groupNO='{testWork.groupNO}' and state=1 and dstate=0 order by itemSort;";
                        DataTable DTMicrobeInfo = await _commRepository.GetTable(sql1);
                        DataTable DTMicrobeItem = await _commRepository.GetTable(sql2);
                        if (DTMicrobeInfo != null && DTMicrobeInfo.Rows.Count > 0)
                        {
                            MicrobeInfoModel testinfo = new MicrobeInfoModel();
                            List<MicrobeResultModel> microbeInfos = new List<MicrobeResultModel>();
                            foreach (DataRow DRinfo in DTMicrobeInfo.Rows)
                            {
                                MicrobeResultModel microbeInfo = new MicrobeResultModel();
                                microbeInfo.id = DRinfo["id"] != DBNull.Value ? Convert.ToInt32(DRinfo["id"]) : 0;
                                microbeInfo.itemSort = DRinfo["itemSort"] != DBNull.Value ? Convert.ToInt32(DRinfo["itemSort"]) : 0;
                                microbeInfo.perid = DRinfo["perid"] != DBNull.Value ? Convert.ToInt32(DRinfo["perid"]) : 0;
                                microbeInfo.testid = DRinfo["testid"] != DBNull.Value ? Convert.ToInt32(DRinfo["testid"]) : 0;
                                microbeInfo.groupNO = DRinfo["groupNO"] != DBNull.Value ? DRinfo["groupNO"].ToString() : "";
                                microbeInfo.groupCode = DRinfo["groupCode"] != DBNull.Value ? DRinfo["groupCode"].ToString() : "";
                                microbeInfo.groupName = DRinfo["groupName"] != DBNull.Value ? DRinfo["groupName"].ToString() : "";
                                microbeInfo.itemCodes = DRinfo["itemCodes"] != DBNull.Value ? DRinfo["itemCodes"].ToString() : "";
                                microbeInfo.itemNames = DRinfo["itemNames"] != DBNull.Value ? DRinfo["itemNames"].ToString() : "";
                                microbeInfo.barcode = DRinfo["barcode"] != DBNull.Value ? DRinfo["barcode"].ToString() : "";
                                microbeInfo.resultType = DRinfo["resultType"] != DBNull.Value ? DRinfo["resultType"].ToString() : "";
                                microbeInfo.itemResult = DRinfo["itemResult"] != DBNull.Value ? DRinfo["itemResult"].ToString() : "";
                                microbeInfo.channel = DRinfo["channel"] != DBNull.Value ? DRinfo["channel"].ToString() : "";
                                microbeInfo.itemRemark = DRinfo["itemRemark"] != DBNull.Value ? DRinfo["itemRemark"].ToString() : "";
                                microbeInfo.delstate = DRinfo["delstate"] != DBNull.Value ? Convert.ToBoolean(DRinfo["delstate"]) : false;
                                microbeInfo.reportState = DRinfo["reportState"] != DBNull.Value ? Convert.ToBoolean(DRinfo["reportState"]) : true;
                                microbeInfo.resultNullState = DRinfo["resultNullState"] != DBNull.Value ? Convert.ToBoolean(DRinfo["resultNullState"]) : false;
                                microbeInfo.state = DRinfo["state"] != DBNull.Value ? Convert.ToBoolean(DRinfo["state"]) : false;
                                microbeInfo.delstate = DRinfo["delstate"] != DBNull.Value ? Convert.ToBoolean(DRinfo["delstate"]) : false;
                                if (DTMicrobeItem != null && DTMicrobeItem.Rows.Count > 0)
                                {

                                    DataRow[] DRItems = DTMicrobeItem.Select($"itemCodes='{microbeInfo.itemCodes}'", "itemSort");
                                    if (DRItems != null && DRItems.Length > 0)
                                    {
                                        List<MicrobeAntibioticModel> antibioticInfos = new List<MicrobeAntibioticModel>();
                                        foreach (DataRow DRItem in DRItems)
                                        {
                                            MicrobeAntibioticModel antibioticInfo = new MicrobeAntibioticModel();
                                            antibioticInfo.id = DRItem["id"] != DBNull.Value ? Convert.ToInt32(DRItem["id"]) : 0;
                                            antibioticInfo.itemSort = DRItem["itemSort"] != DBNull.Value ? Convert.ToInt32(DRItem["itemSort"]) : 0;
                                            antibioticInfo.perid = DRItem["perid"] != DBNull.Value ? Convert.ToInt32(DRItem["perid"]) : 0;
                                            antibioticInfo.testid = DRItem["testid"] != DBNull.Value ? Convert.ToInt32(DRItem["testid"]) : 0;
                                            antibioticInfo.groupNO = DRItem["groupNO"] != DBNull.Value ? DRItem["groupNO"].ToString() : "";
                                            //antibioticInfo.groupCode = DRItem["groupCode"] != DBNull.Value ? DRItem["groupCode"].ToString() : "";
                                            //antibioticInfo.groupName = DRItem["groupName"] != DBNull.Value ? DRItem["groupName"].ToString() : "";
                                            antibioticInfo.itemCodes = DRItem["itemCodes"] != DBNull.Value ? DRItem["itemCodes"].ToString() : "";
                                            antibioticInfo.itemNames = DRItem["itemNames"] != DBNull.Value ? DRItem["itemNames"].ToString() : "";
                                            antibioticInfo.antibioticNo = DRItem["antibioticNo"] != DBNull.Value ? DRItem["antibioticNo"].ToString() : "";
                                            antibioticInfo.antibioticNames = DRItem["antibioticNames"] != DBNull.Value ? DRItem["antibioticNames"].ToString() : "";
                                            antibioticInfo.antibioticEN = DRItem["antibioticEN"] != DBNull.Value ? DRItem["antibioticEN"].ToString() : "";
                                            antibioticInfo.micValue = DRItem["micValue"] != DBNull.Value ? DRItem["micValue"].ToString() : "";
                                            antibioticInfo.kbValue = DRItem["kbValue"] != DBNull.Value ? DRItem["kbValue"].ToString() : "";
                                            antibioticInfo.itemResult = DRItem["itemResult"] != DBNull.Value ? DRItem["itemResult"].ToString() : "";
                                            antibioticInfo.methodName = DRItem["methodName"] != DBNull.Value ? DRItem["methodName"].ToString() : "";
                                            antibioticInfo.aqualitative = DRItem["aqualitative"] != DBNull.Value ? DRItem["aqualitative"].ToString() : "";
                                            antibioticInfo.itemSort = DRItem["itemSort"] != DBNull.Value ? Convert.ToInt32(DRItem["itemSort"]) : 0;
                                            antibioticInfo.channel = DRItem["channel"] != DBNull.Value ? DRItem["channel"].ToString() : "";
                                            antibioticInfo.state = DRItem["state"] != DBNull.Value ? Convert.ToBoolean(DRItem["state"]) : false;
                                            antibioticInfo.remark = DRItem["remark"] != DBNull.Value ? DRItem["remark"].ToString() : "";
                                            antibioticInfos.Add(antibioticInfo);
                                        }
                                        microbeInfo.AntibioticInfos = antibioticInfos;
                                    }
                                }
                                microbeInfos.Add(microbeInfo);


                            }
                            testinfo.listResult = microbeInfos;
                            testInfos.Add(testinfo);
                        }

                    }
                    commReInfo.infos = testInfos;
                    jm.data = commReInfo;
                }
                else
                {
                    jm.code = 0;
                    jm.msg = "未读取到访问信息";
                }
            }
            else
            {
                jm.code = 0;
                jm.msg = "未读取到访问信息";
            }
            return jm;
        }


        /// <summary>
        /// 刷新样本项目的参考值信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetReferenceRefresh(commInfoModel<TestWorkModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                try
                {
                    List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                    foreach (TestWorkModel testInfo in infos.infos)
                    {
                        commReSampleInfo reSampleInfo = new commReSampleInfo();
                        reSampleInfo.code = 0;
                        reSampleInfo.testid = testInfo.id;
                        reSampleInfo.barcode = testInfo.barcode;
                        //commSampleInfo.sampleMsg = ""
                        await _itemHandleServices.ItemReferenceHandle(testInfo.id, testInfo.barcode, false);
                        reSampleInfos.Add(reSampleInfo);
                    }
                    jm.data = reSampleInfos;
                }
                catch (Exception ex)
                {
                    jm.code = 1;
                    jm.msg = ex.Message;
                }
            }
            return jm;
        }
        /// <summary>
        /// 获取结果图片上传信息
        /// </summary>
        /// <param name="Downinfos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetTestImg(commInfoModel<TestDownModel> Downinfos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                try
                {

                    if (Downinfos.infos != null)
                    {
                        List<string> filepaths = new List<string>();
                        foreach (TestDownModel testDown in Downinfos.infos)
                        {
                            //TestDownModel downInfos = JsonHandle.JsonConvertObject<TestDownModel>(downInfo);
                            string filepath = "";
                            if (!Directory.Exists(AppSettingsConstVars.TestFilePath))
                                Directory.CreateDirectory(AppSettingsConstVars.TestFilePath);
                            string dirpath = filepath + "\\" + testDown.dirname;
                            if (!Directory.Exists(dirpath))
                                Directory.CreateDirectory(dirpath);
                            string fileFullPath = dirpath + "\\" + testDown.fileName;
                            if (File.Exists(fileFullPath))
                            {
                                filepaths.Add(fileFullPath);
                            }
                        }
                        jm.data = filepaths;
                    }


                }
                catch (Exception ex)
                {
                    jm.code = 1;
                    jm.msg = ex.Message;
                }
            }
            return jm;

        }

    }
}
