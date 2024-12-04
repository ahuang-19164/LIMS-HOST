using Nito.AsyncEx;
using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;

using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Services;
using Yichen.Files.IServices;
using Yichen.Flow.Model;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Caching.Manuals;
using Yichen.Net.Data;
using Yichen.Other.IRepository;
using Yichen.System.IRepository;
using Yichen.System.Model;
using Yichen.Test.IRepository;
using Yichen.Test.IServices;
using Yichen.Test.Model.Result;
using Yichen.Test.Model.table;

namespace Yichen.Test.Services
{
    public class ItemBLSaveServices : BaseServices<test_sampleInfo>, IItemBLSaveServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        public readonly IUnitOfWork _UnitOfWork;
        //public readonly IItemSaveRepository _itemSaveRepository;
        public readonly IHttpContextUser _httpContextUser;
        public readonly IRecordRepository _recordRepository;

        public readonly ITestItemSaveRepository _testSaveRepository;
        public readonly IItemHandleServices _itemCrisisServices;
        public readonly IFileHandleServices _fileHandleServices;
        public readonly ICommRepository _commRepository;
        public readonly ITestResultInfoRepository _testRepository;
        //private readonly IItemFlowRepository _itemFlowRepository;
        public ItemBLSaveServices(IUnitOfWork unitOfWork
            //, IItemSaveRepository itemSaveRepository
            , IHttpContextUser httpContextUser
            , IRecordRepository recordRepository
            , ITestItemSaveRepository testSaveRepository
            , IItemHandleServices itemCrisisServices
            , IFileHandleServices fileHandleServices
            , ICommRepository commRepository
            , ITestResultInfoRepository testRepository
            , IItemFlowRepository itemFlowRepository

            )
        {
            _UnitOfWork = unitOfWork;
            _httpContextUser = httpContextUser;
            _recordRepository = recordRepository;
            _itemCrisisServices = itemCrisisServices;
            _fileHandleServices = fileHandleServices;
            _testSaveRepository = testSaveRepository;
            _commRepository = commRepository;
            _testRepository = testRepository;
            //_itemFlowRepository = itemFlowRepository;

        }
        /// <summary>
        /// 蜡块信息处理
        /// </summary>
        /// <param name="testinfos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BlockHandle(CommResultModel<List<BlockInfoModel>> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                commReInfo<string> commReInfo = new commReInfo<string>();

                //CommResultModel<BlockInfoModel> info = JsonHandle.JsonConvertObject<CommResultModel<List<BlockInfoModel>>>(testinfos);
                try
                {


                    string sql = "";
                    string msg = "";

                    foreach (BlockInfoModel blockInfo in info.Result)
                    {
                        if (blockInfo.id != 0 && blockInfo.testid != 0)
                        {
                            switch (blockInfo.state)
                            {
                                case 2:
                                   sql += bmqr(blockInfo, info.UserName);
                                   break;
                                case 3:
                                   sql += qpqr(blockInfo, info.UserName);
                                    break;
                                case 4:
                                   sql += Rebmqr(blockInfo, info.UserName);
                                   break;
                                case 5:
                                   sql += Reqpqr(blockInfo, info.UserName);
                                    break;
                                case 6:
                                    sql += Reqc(blockInfo, info.UserName);
                                    break;
                            }

                        }
                        else
                        {
                            msg += $"编号：{blockInfo.id},检验编号：{blockInfo.testid},条码号:{blockInfo.barcode}。操作失败！";
                        }
                    }

                    if (sql != "")
                    {
                        //await DbClient.Ado.ExecuteCommandAsync(sql);
                        await _commRepository.sqlcommand(sql);
                        commReInfo.code = 0;
                        commReInfo.msg = $"操作成功" + msg;
                    }
                    else
                    {
                        commReInfo.code = 1;
                        commReInfo.msg = $"提交信息有误";
                    }
                }
                catch (Exception ex)
                {
                    commReInfo.code = 1;
                    commReInfo.msg = $"提交信息有误。{ex.Message}";
                }
                jm.data = commReInfo;
            }
            return jm;
        }
        /// <summary>
        /// 会诊结果保存
        /// </summary>
        /// <param name="Infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SaveConsultation(CommResultModel<PathnologyInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                commReInfo<commReItemInfo> commReInfo = new commReInfo<commReItemInfo>();
                try
                {


                    string nextFlowNO = "0";
                    if (info.Result != null)
                    {

                        string resultInfoString = "";
                        if (info.Result.testid != 0)
                        {

                            var testinfo = await _testRepository.GetTestInfo(info.Result.testid);//样本信息

                            if (testinfo != null)
                            {
                                if (info.ResultState < 4)
                                {

                                    if (info.testFlowNO != null && info.testFlowNO != "")
                                    {


                                        //List<comm_item_flow> FlowInfoss = await _itemFlowRepository.GetCaChe();
                                        List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
                                        comm_item_flow testFlowInfo = FlowInfoss.First(p => p.no == Convert.ToInt32(info.testFlowNO));


                                        nextFlowNO = testFlowInfo.nextFlow != null && testFlowInfo.nextFlow == "0" ? testFlowInfo.nextFlow : "0";
                                        if (nextFlowNO != "0")
                                        {
                                            string sqlUpFlowNO = $"update WorkTest.SampleInfoFlow set dstate=1 where testid='{info.Result.testid}' and flowNO='{info.testFlowNO}';";
                                            sqlUpFlowNO += "insert into WorkTest.SampleInfoFlow (perid,testid,barcode,flowNO,flowSort,creater,createTime,state,dstate)";
                                            sqlUpFlowNO += $"values (0,'{info.Result.testid}','{info.Result.barcode}','{info.testFlowNO}',0,'{info.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0)";
                                            resultInfoString += sqlUpFlowNO;
                                            commReInfo.nextFlowNO = nextFlowNO;
                                        }
                                        else
                                        {
                                            nextFlowNO = info.testFlowNO;
                                        }
                                    }

                                    string Ssql = "update WorkTest.ResultConsultation set ";
                                    Ssql += $"pathologyNo='{info.Result.PathologyNo}',";
                                    Ssql += $"eyeVisible='{info.Result.EyeVisible}',";
                                    if (info.Result.primaryDiagnosis != null)
                                        Ssql += $"primaryDiagnosis='{info.Result.primaryDiagnosis}',";
                                    if (info.Result.DescriptionLesions != null)
                                        Ssql += $"descriptionLesions='{info.Result.DescriptionLesions}',";
                                    if (info.Result.pathologicDiagnosis != null)
                                        Ssql += $"diagnosis='{info.Result.pathologicDiagnosis}',";
                                    if (info.Result.diagnosisRemark != null)
                                        Ssql += $"diagnosisRemark='{info.Result.diagnosisRemark}',";
                                    //Ssql += $"pictureCode='{info.Result.diagnosisRemark}',";
                                    Ssql += $"state=1,";
                                    Ssql += $"createTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'";
                                    Ssql += $"where testid='{info.Result.testid}' and barcode='{info.Result.barcode}'";
                                    //a = DbClient.Ado.ExecuteCommandAsync( Ssql);
                                    string Spsql = $"update WorkTest.ResultConsultationImg set state=0 ";
                                    Spsql += $"where testid='{info.Result.testid}' and barcode='{info.Result.barcode}'";
                                    //a = DbClient.Ado.ExecuteCommandAsync( Spsql);
                                    string sampleresult = "";
                                    if (info.Result.ListPicture != null)
                                    {
                                        if (info.Result.ListPicture.Count > 0)
                                        {
                                            foreach (PictureInfoModel pictureInfo in info.Result.ListPicture)
                                            {
                                                string Psql = "insert into WorkTest.ResultConsultationImg ";
                                                Psql += "(testid,pathologyNo,classNo,className,barcode,pictureNames,pictureDye,filestring,state,sort,createTime)";
                                                Psql += $" Values ('{info.Result.testid}','{pictureInfo.pathologyNo}','{pictureInfo.ClassNo}','{pictureInfo.ClassName}','{info.Result.barcode}','{pictureInfo.PictureNames}','{pictureInfo.PictureDye}','{pictureInfo.filestring}','1','{pictureInfo.Sort}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');";
                                                sampleresult += Psql;
                                            }
                                        }
                                    }


                                    resultInfoString = Ssql + Spsql + sampleresult;
                                    if (resultInfoString != "")

                                        await _commRepository.sqlcommand(resultInfoString);

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
                                }
                                if (info.ResultState == 4)
                                {
                                    int reportState = testinfo.report != null ? Convert.ToInt32(testinfo.report) : 0;
                                    jm = await _testSaveRepository.ReCheck(info.Result.perid.ToString(), info.Result.testid.ToString(), info.Result.barcode, info.UserName, reportState);
                                }
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

                    //commReInfo.msg = ex.Message;
                    commReInfo.msg = ex.Message;
                }
                jm.data = commReInfo;
            }
            return jm;
        }
        /// <summary>
        /// 组织结果保存
        /// </summary>
        /// <param name="Infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SavePathology(CommResultModel<PathnologyInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                commReInfo<commReItemInfo> commReInfo = new commReInfo<commReItemInfo>();

                //CommResultModel<PathnologyInfoModel> info = JsonHandle.JsonConvertObject<CommResultModel<PathnologyInfoModel>>(Infos);
                try
                {
                    string nextFlowNO = "0";
                    if (info.Result != null)
                    {
                        string resultInfoString = "";
                        if (info.Result.testid != 0)
                        {

                            var testinfo = await _testRepository.GetTestInfo(info.Result.testid);//样本信息

                            if (testinfo != null)
                            {
                                if (info.ResultState < 4)
                                {
                                    if (info.testFlowNO != null && info.testFlowNO != "")
                                    {
                                        //string SflowNO = $"select id,nextFlow from WorkComm.ItemFlow where no='{info.testFlowNO}'";
                                        //DataTable DTnextFlow = HLDBSqlHelper.ExecuteDataset( SflowNO).Tables[0];


                                        //List<comm_item_flow> FlowInfoss = await _itemFlowRepository.GetCaChe();
                                        List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
                                        comm_item_flow testFlowInfo = FlowInfoss.First(p => p.no == Convert.ToInt32(info.testFlowNO));


                                        nextFlowNO = testFlowInfo.nextFlow != null && testFlowInfo.nextFlow != "0" ? testFlowInfo.nextFlow : "0";
                                        if (nextFlowNO != null && nextFlowNO != "" && nextFlowNO != "0")
                                        {
                                            string sqlUpFlowNO = $"update WorkTest.SampleInfoFlow set dstate=1 where testid='{info.Result.testid}' and flowNO='{info.testFlowNO}';";
                                            sqlUpFlowNO += "insert into WorkTest.SampleInfoFlow (perid,testid,barcode,flowNO,flowSort,creater,createTime,state,dstate)";
                                            sqlUpFlowNO += $"values (0,'{info.Result.testid}','{info.Result.barcode}','{info.testFlowNO}',0,'{info.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0)";
                                            resultInfoString += sqlUpFlowNO;
                                            commReInfo.nextFlowNO = nextFlowNO;
                                        }
                                        else
                                        {
                                            nextFlowNO = info.testFlowNO;
                                        }

                                    }
                                    string Ssql = "update WorkTest.ResultPathology set ";
                                    Ssql += $"pathologyNo='{info.Result.PathologyNo}',";
                                    Ssql += $"eyeVisible='{info.Result.EyeVisible}',";
                                    if (info.Result.primaryDiagnosis != null)
                                        Ssql += $"primaryDiagnosis='{info.Result.primaryDiagnosis}',";
                                    if (info.Result.DescriptionLesions != null)
                                        Ssql += $"descriptionLesions='{info.Result.DescriptionLesions}',";
                                    if (info.Result.pathologicDiagnosis != null)
                                        Ssql += $"diagnosis='{info.Result.pathologicDiagnosis}',";
                                    if (info.Result.pathologicDiagnosis != null)
                                        Ssql += $"ihcResult='{info.Result.ihcResult}',";
                                    if (info.Result.diagnosisRemark != null)
                                        Ssql += $"diagnosisRemark='{info.Result.diagnosisRemark}',";
                                    //Ssql += $"pictureCode='{info.Result.diagnosisRemark}',";
                                    Ssql += $"state=1,";
                                    Ssql += $"createTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'";
                                    Ssql += $"where testid='{info.Result.testid}';";
                                    //a = DbClient.Ado.ExecuteCommandAsync( Ssql);
                                    string Spsql = $"update WorkTest.ResultPathologyImg set state=0 ";
                                    Spsql += $"where testid='{info.Result.testid}';";

                                    string sampleresult = "";
                                    if (info.Result.ListPicture != null)
                                    {
                                        if (info.Result.ListPicture.Count > 0)
                                        {


                                            foreach (PictureInfoModel pictureInfo in info.Result.ListPicture)
                                            {
                                                string Psql = "insert into WorkTest.ResultPathologyImg ";
                                                Psql += "(testid,pathologyNo,classNo,className,barcode,pictureNames,pictureDye,filestring,state,sort,createTime)";
                                                Psql += $" Values ('{info.Result.testid}','{pictureInfo.pathologyNo}','{pictureInfo.ClassNo}','{pictureInfo.ClassName}','{info.Result.barcode}','{pictureInfo.PictureNames}','{pictureInfo.PictureDye}','{pictureInfo.filestring}','1','{pictureInfo.Sort}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');";
                                                sampleresult += Psql;
                                            }
                                        }
                                    }
                                    if (info.Result.ListBlock != null)
                                    {
                                        if (info.Result.ListBlock.Count > 0)
                                        {
                                            Spsql += $"update WorkTest.ResultPathologyBlock set state=0,dstate=1 ";
                                            Spsql += $"where testid='{info.Result.testid}';";

                                            int blockNo = 0;
                                            foreach (BlockInfoModel block in info.Result.ListBlock)
                                            {
                                                blockNo += 1;
                                                string Psql = "insert into WorkTest.ResultPathologyBlock ";
                                                Psql += "(perid,testid,barcode,pathologyNo,blockNo,operater,operatTime,recorder,createTime,remark,state,dstate)";
                                                Psql += $" Values ('0','{info.Result.testid}','{info.Result.barcode}','{block.pathologyNo}','{block.pathologyNo + "-" + blockNo}','{block.operater}','{block.operatTime}','{block.recorder}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{block.remark}','1','0');";
                                                Psql += $" Values ('0','{info.Result.testid}','{info.Result.barcode}','{block.pathologyNo}','{block.pathologyNo + "-" + blockNo}','{block.operater}','{block.operatTime}','{block.recorder}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{block.remark}','1','0');";
                                                sampleresult += Psql;
                                            }
                                        }
                                    }
                                    resultInfoString += Ssql + Spsql + sampleresult;
                                    if (resultInfoString != "")

                                        await _commRepository.sqlcommand(resultInfoString);

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
                                }

                                if (info.ResultState == 4)
                                {
                                    int reportState = testinfo.report != null ? Convert.ToInt32(testinfo.report) : 0;
                                    jm = await _testSaveRepository.ReCheck(info.Result.perid.ToString(), info.Result.testid.ToString(), info.Result.barcode, info.UserName, reportState);
                                }
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
        /// <summary>
        /// 筛查结果保存
        /// </summary>
        /// <param name="Infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SaveScreen(CommResultModel<ScreenInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                commReInfo<commReItemInfo> commReInfo = new commReInfo<commReItemInfo>();
                try
                {
                    string nextFlowNO = "0";
                    if (info.Result != null)
                    {
                        string resultInfoString = "";
                        if (info.Result.testid != 0)
                        {
                            var testinfo = await _testRepository.GetTestInfo(info.Result.testid);//样本信息

                            if (testinfo != null)
                            {
                                if(info.ResultState<4)
                                {
                                    if (info.testFlowNO != null && info.testFlowNO != "")
                                    {
                                        //List<comm_item_flow> FlowInfoss = await _itemFlowRepository.GetCaChe();
                                        List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
                                        comm_item_flow testFlowInfo = FlowInfoss.First(p => p.no == Convert.ToInt32(info.testFlowNO));
                                        nextFlowNO = testFlowInfo.nextFlow != null && testFlowInfo.nextFlow == "0" ? testFlowInfo.nextFlow : "0";
                                        if (nextFlowNO != null && nextFlowNO != "" && nextFlowNO != "0")
                                        {
                                            string sqlUpFlowNO = $"update WorkTest.SampleInfoFlow set dstate=1 where testid='{info.Result.testid}' and flowNO='{info.testFlowNO}';";
                                            sqlUpFlowNO += "insert into WorkTest.SampleInfoFlow (perid,testid,barcode,flowNO,flowSort,creater,createTime,state,dstate)";
                                            sqlUpFlowNO += $"values (0,'{info.Result.testid}','{info.Result.barcode}','{info.testFlowNO}',0,'{info.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0)";
                                            resultInfoString += sqlUpFlowNO;
                                            commReInfo.nextFlowNO = nextFlowNO;
                                        }
                                        else
                                        {
                                            nextFlowNO = info.testFlowNO;
                                        }

                                    }
                                    string Ssql = "update WorkTest.ResultScreen set ";
                                    Ssql += $"pathologyNo='{info.Result.pathologyNo}',";
                                    //Ssql += $"eyeVisible='{info.Result.EyeVisible}',";
                                    if (info.Result.diagnosis != null)
                                        Ssql += $"diagnosis='{info.Result.diagnosis}',";
                                    if (info.Result.diagnosisRemark != null)
                                        Ssql += $"diagnosisRemark='{info.Result.diagnosisRemark}',";
                                    //Ssql += $"pictureCode='{info.Result.diagnosisRemark}',";
                                    Ssql += $"state=1,";
                                    Ssql += $"createTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'";
                                    Ssql += $"where testid='{info.Result.testid}' and barcode='{info.Result.barcode}'";
                                    //a = DbClient.Ado.ExecuteCommandAsync( Ssql);
                                    resultInfoString = resultInfoString + Ssql;
                                    if (resultInfoString != "")

                                        await _commRepository.sqlcommand(resultInfoString);

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
                                }
                                if (info.ResultState == 4)
                                {
                                    int reportState = testinfo.report != null? Convert.ToInt32(testinfo.report) : 0;
                                    jm = await _testSaveRepository.ReCheck(info.Result.perid.ToString(), info.Result.testid.ToString(), info.Result.barcode, info.UserName, reportState);
                                }
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
        /// <summary>
        /// TCT结果保存
        /// </summary>
        /// <param name="Infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SaveTCT(CommResultModel<TCTInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                commReInfo<commReItemInfo> commReInfo = new commReInfo<commReItemInfo>();
                try
                {



                    string nextFlowNO = "0";
                    if (info.Result != null)
                    {
                        string resultInfoString = "";
                        if (info.Result.testid != 0)
                        {
                            var testinfo = await _testRepository.GetTestInfo(info.Result.testid);//样本信息

                            if (testinfo != null)
                            {
                                if (info.ResultState < 4)
                                {
                                    if (info.testFlowNO != null && info.testFlowNO != "")
                                    {
                                        //List<comm_item_flow> FlowInfoss = await _itemFlowRepository.GetCaChe();
                                        List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
                                        comm_item_flow testFlowInfo = FlowInfoss.First(p => p.no == Convert.ToInt32(info.testFlowNO));
                                        nextFlowNO = testFlowInfo.nextFlow != null && testFlowInfo.nextFlow == "0" ? testFlowInfo.nextFlow : "0";

                                        if (nextFlowNO != null && nextFlowNO != "" && nextFlowNO != "0")
                                        {
                                            string sqlUpFlowNO = $"update WorkTest.SampleInfoFlow set dstate=1 where testid='{info.Result.testid}' and flowNO='{info.testFlowNO}';";
                                            sqlUpFlowNO += "insert into WorkTest.SampleInfoFlow (perid,testid,barcode,flowNO,flowSort,creater,createTime,state,dstate)";
                                            sqlUpFlowNO += $"values (0,'{info.Result.testid}','{info.Result.barcode}','{info.testFlowNO}',0,'{info.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0)";
                                            resultInfoString += sqlUpFlowNO;
                                            commReInfo.nextFlowNO = nextFlowNO;
                                        }
                                        else
                                        {
                                            nextFlowNO = info.testFlowNO;
                                        }
                                    }


                                    string Ssql = "update WorkTest.ResultTCT set ";
                                    Ssql += $"pathologyNo='{info.Result.PathologyNo}',";
                                    if (info.Result.diagnosis != null)
                                        Ssql += $"diagnosis='{info.Result.diagnosis}',";
                                    if (info.Result.diagnosisRemark != null)
                                        Ssql += $"diagnosisRemark='{info.Result.diagnosisRemark}',";
                                    if (info.Result.eyeVisible != null)
                                        Ssql += $"eyeVisible='{info.Result.eyeVisible}',";
                                    if (info.Result.descriptionLesions != null)
                                        Ssql += $"descriptionLesions='{info.Result.descriptionLesions}',";


                                    if (info.Result.TCT1 == true) { Ssql += $" tct1=1,"; } else { Ssql += $" tct1=0,"; };
                                    if (info.Result.TCT2 == true) { Ssql += $" tct2=1,"; } else { Ssql += $" tct2=0,"; };
                                    if (info.Result.TCT3 == true) { Ssql += $" tct3=1,"; } else { Ssql += $" tct3=0,"; };
                                    if (info.Result.TCT4 == true) { Ssql += $" tct4=1,"; } else { Ssql += $" tct4=0,"; };
                                    if (info.Result.TCT5 == true) { Ssql += $" tct5=1,"; } else { Ssql += $" tct5=0,"; };
                                    if (info.Result.TCT6 == true) { Ssql += $" tct6=1,"; } else { Ssql += $" tct6=0,"; };
                                    if (info.Result.TCT7 == true) { Ssql += $" tct7=1,"; } else { Ssql += $" tct7=0,"; };
                                    if (info.Result.TCT8 == true) { Ssql += $" tct8=1,"; } else { Ssql += $" tct8=0,"; };
                                    if (info.Result.TCT9 == true) { Ssql += $" tct9=1,"; } else { Ssql += $" tct9=0,"; };
                                    if (info.Result.TCT10 == true) { Ssql += $" tct10=1,"; } else { Ssql += $" tct10=0,"; };
                                    if (info.Result.TCT11 == true) { Ssql += $" tct11=1,"; } else { Ssql += $" tct11=0,"; };
                                    if (info.Result.TCT12 == true) { Ssql += $" tct12=1,"; } else { Ssql += $" tct12=0,"; };
                                    if (info.Result.TCT13 == true) { Ssql += $" tct13=1,"; } else { Ssql += $" tct13=0,"; };
                                    if (info.Result.TCT14 == true) { Ssql += $" tct14=1,"; } else { Ssql += $" tct14=0,"; };
                                    if (info.Result.TCT15 == true) { Ssql += $" tct15=1,"; } else { Ssql += $" tct15=0,"; };
                                    if (info.Result.TCT16 == true) { Ssql += $" tct16=1,"; } else { Ssql += $" tct16=0,"; };
                                    if (info.Result.TCT17 == true) { Ssql += $" tct17=1,"; } else { Ssql += $" tct17=0,"; };
                                    if (info.Result.TCT18 == true) { Ssql += $" tct18=1,"; } else { Ssql += $" tct18=0,"; };
                                    if (info.Result.TCT19 == true) { Ssql += $" tct19=1,"; } else { Ssql += $" tct19=0,"; };
                                    if (info.Result.TCT20 == true) { Ssql += $" tct20=1,"; } else { Ssql += $" tct20=0,"; };
                                    if (info.Result.TCT21 == true) { Ssql += $" tct21=1,"; } else { Ssql += $" tct21=0,"; };
                                    if (info.Result.TCT22 == true) { Ssql += $" tct22=1,"; } else { Ssql += $" tct22=0,"; };
                                    if (info.Result.TCT23 == true) { Ssql += $" tct23=1,"; } else { Ssql += $" tct23=0,"; };
                                    if (info.Result.TCT24 == true) { Ssql += $" tct24=1,"; } else { Ssql += $" tct24=0,"; };
                                    if (info.Result.TCT25 == true) { Ssql += $" tct25=1,"; } else { Ssql += $" tct25=0,"; };
                                    if (info.Result.TCT26 == true) { Ssql += $" tct26=1,"; } else { Ssql += $" tct26=0,"; };
                                    if (info.Result.TCT27 == true) { Ssql += $" tct27=1,"; } else { Ssql += $" tct27=0,"; };
                                    if (info.Result.TCT28 == true) { Ssql += $" tct28=1,"; } else { Ssql += $" tct28=0,"; };
                                    if (info.Result.TCT29 == true) { Ssql += $" tct29=1,"; } else { Ssql += $" tct29=0,"; };
                                    if (info.Result.TCT30 == true) { Ssql += $" tct30=1,"; } else { Ssql += $" tct30=0,"; };
                                    if (info.Result.TCT31 == true) { Ssql += $" tct31=1,"; } else { Ssql += $" tct31=0,"; };
                                    if (info.Result.TCT32 == true) { Ssql += $" tct32=1,"; } else { Ssql += $" tct32=0,"; };
                                    if (info.Result.TCT33 == true) { Ssql += $" tct33=1,"; } else { Ssql += $" tct33=0,"; };
                                    if (info.Result.TCT34 == true) { Ssql += $" tct34=1,"; } else { Ssql += $" tct34=0,"; };
                                    if (info.Result.TCT35 == true) { Ssql += $" tct35=1,"; } else { Ssql += $" tct35=0,"; };
                                    if (info.Result.TCT36 == true) { Ssql += $" tct36=1,"; } else { Ssql += $" tct36=0,"; };
                                    if (info.Result.TCT37 == true) { Ssql += $" tct37=1,"; } else { Ssql += $" tct37=0,"; };
                                    if (info.Result.TCT38 == true) { Ssql += $" tct38=1,"; } else { Ssql += $" tct38=0,"; };
                                    if (info.Result.TCT39 == true) { Ssql += $" tct39=1,"; } else { Ssql += $" tct39=0,"; };
                                    if (info.Result.TCT40 == true) { Ssql += $" tct40=1,"; } else { Ssql += $" tct40=0,"; };

                                    Ssql += $"state=1,";
                                    Ssql += $"createTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'";
                                    Ssql += $"where testid='{info.Result.testid}' and barcode='{info.Result.barcode}'";
                                    //a = DbClient.Ado.ExecuteCommandAsync( Ssql);
                                    string Spsql = $"update WorkTest.ResultTCTImg set state=0 ";
                                    Spsql += $"where testid='{info.Result.testid}' and barcode='{info.Result.barcode}'";
                                    //a = DbClient.Ado.ExecuteCommandAsync( Spsql);
                                    string sampleresult = "";
                                    if (info.Result.ListPicture != null)
                                    {
                                        if (info.Result.ListPicture.Count > 0)
                                        {

                                            foreach (PictureInfoModel pictureInfo in info.Result.ListPicture)
                                            {
                                                string Psql = "insert into WorkTest.ResultTCTImg ";
                                                Psql += "(testid,pathologyNo,classNo,className,barcode,pictureNames,pictureDye,filestring,state,sort,createTime)";
                                                Psql += $" Values ('{info.Result.testid}','{pictureInfo.pathologyNo}','{pictureInfo.ClassNo}','{pictureInfo.ClassName}','{info.Result.barcode}','{pictureInfo.PictureNames}','{pictureInfo.PictureDye}','{pictureInfo.filestring}','1','{pictureInfo.Sort}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');";
                                                sampleresult += Psql;
                                            }
                                        }
                                    }
                                    resultInfoString = Ssql + Spsql + sampleresult;

                                    if (resultInfoString != "")

                                        await _commRepository.sqlcommand(resultInfoString);

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
                                }
                                if (info.ResultState == 4)
                                {
                                    int reportState = testinfo.report != null ? Convert.ToInt32(testinfo.report) : 0;
                                    jm = await _testSaveRepository.ReCheck(info.Result.perid.ToString(), info.Result.testid.ToString(), info.Result.barcode, info.UserName, reportState);
                                }
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
        /// <summary>
        /// TCT结果筛查
        /// </summary>
        /// <param name="Infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SaveTCTScreen(CommResultModel<TCTInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                commReInfo<commReItemInfo> commReInfo = new commReInfo<commReItemInfo>();

                //CommResultModel<TCTInfoModel> info = JsonHandle.JsonConvertObject<CommResultModel<TCTInfoModel>>(Infos);
                try
                {



                    string nextFlowNO = "0";
                    if (info.Result != null)
                    {
                        string resultInfoString = "";
                        if (info.Result.testid != 0)
                        {
                            var testinfo = await _testRepository.GetTestInfo(info.Result.testid);//样本信息

                            if (testinfo != null)
                            {
                                if (info.ResultState < 4)
                                {
                                    if (info.testFlowNO != null && info.testFlowNO != "")
                                    {
                                        //List<comm_item_flow> FlowInfoss = await _itemFlowRepository.GetCaChe();
                                        List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
                                        comm_item_flow testFlowInfo = FlowInfoss.First(p => p.no == Convert.ToInt32(info.testFlowNO));
                                        nextFlowNO = testFlowInfo.nextFlow != null && testFlowInfo.nextFlow == "0" ? testFlowInfo.nextFlow : "0";
                                        if (nextFlowNO != null && nextFlowNO != "" && nextFlowNO != "0")
                                        {
                                            string sqlUpFlowNO = $"update WorkTest.SampleInfoFlow set dstate=1 where testid='{info.Result.testid}' and flowNO='{info.testFlowNO}';";
                                            sqlUpFlowNO += "insert into WorkTest.SampleInfoFlow (perid,testid,barcode,flowNO,flowSort,creater,createTime,state,dstate)";
                                            sqlUpFlowNO += $"values (0,'{info.Result.testid}','{info.Result.barcode}','{info.testFlowNO}',0,'{info.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0)";
                                            resultInfoString += sqlUpFlowNO;
                                            commReInfo.nextFlowNO = nextFlowNO;
                                        }
                                        else
                                        {
                                            nextFlowNO = info.testFlowNO;
                                        }
                                    }


                                    string Ssql = "update WorkTest.ResultTCTScreen set ";
                                    Ssql += $"pathologyNo='{info.Result.PathologyNo}',";
                                    if (info.Result.diagnosis != null)
                                        Ssql += $"diagnosis='{info.Result.diagnosis}',";
                                    if (info.Result.diagnosisRemark != null)
                                        Ssql += $"diagnosisRemark='{info.Result.diagnosisRemark}',";
                                    if (info.Result.eyeVisible != null)
                                        Ssql += $"eyeVisible='{info.Result.eyeVisible}',";
                                    if (info.Result.descriptionLesions != null)
                                        Ssql += $"descriptionLesions='{info.Result.descriptionLesions}',";


                                    if (info.Result.TCT1 == true) { Ssql += $" tct1=1,"; } else { Ssql += $" tct1=0,"; };
                                    if (info.Result.TCT2 == true) { Ssql += $" tct2=1,"; } else { Ssql += $" tct2=0,"; };
                                    if (info.Result.TCT3 == true) { Ssql += $" tct3=1,"; } else { Ssql += $" tct3=0,"; };
                                    if (info.Result.TCT4 == true) { Ssql += $" tct4=1,"; } else { Ssql += $" tct4=0,"; };
                                    if (info.Result.TCT5 == true) { Ssql += $" tct5=1,"; } else { Ssql += $" tct5=0,"; };
                                    if (info.Result.TCT6 == true) { Ssql += $" tct6=1,"; } else { Ssql += $" tct6=0,"; };
                                    if (info.Result.TCT7 == true) { Ssql += $" tct7=1,"; } else { Ssql += $" tct7=0,"; };
                                    if (info.Result.TCT8 == true) { Ssql += $" tct8=1,"; } else { Ssql += $" tct8=0,"; };
                                    if (info.Result.TCT9 == true) { Ssql += $" tct9=1,"; } else { Ssql += $" tct9=0,"; };
                                    if (info.Result.TCT10 == true) { Ssql += $" tct10=1,"; } else { Ssql += $" tct10=0,"; };
                                    if (info.Result.TCT11 == true) { Ssql += $" tct11=1,"; } else { Ssql += $" tct11=0,"; };
                                    if (info.Result.TCT12 == true) { Ssql += $" tct12=1,"; } else { Ssql += $" tct12=0,"; };
                                    if (info.Result.TCT13 == true) { Ssql += $" tct13=1,"; } else { Ssql += $" tct13=0,"; };
                                    if (info.Result.TCT14 == true) { Ssql += $" tct14=1,"; } else { Ssql += $" tct14=0,"; };
                                    if (info.Result.TCT15 == true) { Ssql += $" tct15=1,"; } else { Ssql += $" tct15=0,"; };
                                    if (info.Result.TCT16 == true) { Ssql += $" tct16=1,"; } else { Ssql += $" tct16=0,"; };
                                    if (info.Result.TCT17 == true) { Ssql += $" tct17=1,"; } else { Ssql += $" tct17=0,"; };
                                    if (info.Result.TCT18 == true) { Ssql += $" tct18=1,"; } else { Ssql += $" tct18=0,"; };
                                    if (info.Result.TCT19 == true) { Ssql += $" tct19=1,"; } else { Ssql += $" tct19=0,"; };
                                    if (info.Result.TCT20 == true) { Ssql += $" tct20=1,"; } else { Ssql += $" tct20=0,"; };
                                    if (info.Result.TCT21 == true) { Ssql += $" tct21=1,"; } else { Ssql += $" tct21=0,"; };
                                    if (info.Result.TCT22 == true) { Ssql += $" tct22=1,"; } else { Ssql += $" tct22=0,"; };
                                    if (info.Result.TCT23 == true) { Ssql += $" tct23=1,"; } else { Ssql += $" tct23=0,"; };
                                    if (info.Result.TCT24 == true) { Ssql += $" tct24=1,"; } else { Ssql += $" tct24=0,"; };
                                    if (info.Result.TCT25 == true) { Ssql += $" tct25=1,"; } else { Ssql += $" tct25=0,"; };
                                    if (info.Result.TCT26 == true) { Ssql += $" tct26=1,"; } else { Ssql += $" tct26=0,"; };
                                    if (info.Result.TCT27 == true) { Ssql += $" tct27=1,"; } else { Ssql += $" tct27=0,"; };
                                    if (info.Result.TCT28 == true) { Ssql += $" tct28=1,"; } else { Ssql += $" tct28=0,"; };
                                    if (info.Result.TCT29 == true) { Ssql += $" tct29=1,"; } else { Ssql += $" tct29=0,"; };
                                    if (info.Result.TCT30 == true) { Ssql += $" tct30=1,"; } else { Ssql += $" tct30=0,"; };
                                    if (info.Result.TCT31 == true) { Ssql += $" tct31=1,"; } else { Ssql += $" tct31=0,"; };
                                    if (info.Result.TCT32 == true) { Ssql += $" tct32=1,"; } else { Ssql += $" tct32=0,"; };
                                    if (info.Result.TCT33 == true) { Ssql += $" tct33=1,"; } else { Ssql += $" tct33=0,"; };
                                    if (info.Result.TCT34 == true) { Ssql += $" tct34=1,"; } else { Ssql += $" tct34=0,"; };
                                    if (info.Result.TCT35 == true) { Ssql += $" tct35=1,"; } else { Ssql += $" tct35=0,"; };
                                    if (info.Result.TCT36 == true) { Ssql += $" tct36=1,"; } else { Ssql += $" tct36=0,"; };
                                    if (info.Result.TCT37 == true) { Ssql += $" tct37=1,"; } else { Ssql += $" tct37=0,"; };
                                    if (info.Result.TCT38 == true) { Ssql += $" tct38=1,"; } else { Ssql += $" tct38=0,"; };
                                    if (info.Result.TCT39 == true) { Ssql += $" tct39=1,"; } else { Ssql += $" tct39=0,"; };
                                    if (info.Result.TCT40 == true) { Ssql += $" tct40=1,"; } else { Ssql += $" tct40=0,"; };

                                    Ssql += $"state=1,";
                                    Ssql += $"createTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'";
                                    Ssql += $"where testid='{info.Result.testid}' and barcode='{info.Result.barcode}'";
                                    //a = DbClient.Ado.ExecuteCommandAsync( Ssql);
                                    string Spsql = $"update WorkTest.ResultTCTScreenImg set state=0 ";
                                    Spsql += $"where testid='{info.Result.testid}' and barcode='{info.Result.barcode}'";
                                    //a = DbClient.Ado.ExecuteCommandAsync( Spsql);
                                    string sampleresult = "";
                                    if (info.Result.ListPicture != null)
                                    {
                                        if (info.Result.ListPicture.Count > 0)
                                        {

                                            foreach (PictureInfoModel pictureInfo in info.Result.ListPicture)
                                            {
                                                string Psql = "insert into WorkTest.ResultTCTScreenImg ";
                                                Psql += "(testid,pathologyNo,classNo,className,barcode,pictureNames,pictureDye,filestring,state,sort,createTime)";
                                                Psql += $" Values ('{info.Result.testid}','{pictureInfo.pathologyNo}','{pictureInfo.ClassNo}','{pictureInfo.ClassName}','{info.Result.barcode}','{pictureInfo.PictureNames}','{pictureInfo.PictureDye}','{pictureInfo.filestring}','1','{pictureInfo.Sort}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');";
                                                sampleresult += Psql;
                                            }
                                        }
                                    }
                                    resultInfoString = Ssql + Spsql + sampleresult;
                                    if (resultInfoString != "")

                                        await _commRepository.sqlcommand(resultInfoString);

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

                                }
                                if (info.ResultState == 4)
                                {
                                    int reportState = testinfo.report != null ? Convert.ToInt32(testinfo.report) : 0;
                                    jm = await _testSaveRepository.ReCheck(info.Result.perid.ToString(), info.Result.testid.ToString(), info.Result.barcode, info.UserName, reportState);
                                }
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

        /// <summary>
        /// 包埋确认
        /// </summary>
        /// <param name="blockInfo"></param>
        /// <param name="userName"></param>
        private string bmqr(BlockInfoModel blockInfo, string userName)
        {
            string sql = $"update WorkTest.ResultPathologyBlock set state=2,bmPerson='{userName}',bmTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{blockInfo.id}';";
            //string sql = $"update WorkTest.ResultPathologyBlock set state=2,bmPerson='{userName}',bmTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{blockInfo.id}';";
            //string updatelg = $"insert into WorkComm.SampleRecord (barcode,operatType,record,reason,operater,createTime,clientShow) values ('{barcode}','{operatType}','{record}','{reason}','{operater}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{clientShow}')";
            return sql;
        }
        /// <summary>
        /// 反包埋确认
        /// </summary>
        /// <param name="blockInfo"></param>
        /// <param name="userName"></param>
        private string Rebmqr(BlockInfoModel blockInfo, string userName)
        {
            string sql = $"update WorkTest.ResultPathologyBlock set state=1,bmPerson='',bmTime=null where id='{blockInfo.id}';";
            return sql;
        }

        /// <summary>
        /// 切片确认
        /// </summary>
        /// <param name="blockInfo"></param>
        /// <param name="userName"></param>
        private string qpqr(BlockInfoModel blockInfo, string userName)
        {

            string sql = $"update WorkTest.SampleInfo set pathologyStateNO='4',groupFlowNO='14' where id='{blockInfo.testid}';";
            sql += $"update WorkTest.ResultPathologyBlock set state=3,qpPerson='{userName}',qpTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{blockInfo.id}';";
            sql += "insert into WorkTest.SampleInfoFlow (perid,testid,barcode,flowNO,flowSort,creater,createTime,state,dstate)";
            sql += $"values (0,'{blockInfo.testid}','{blockInfo.barcode}','13',0,'{userName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
            return sql;
        }
        /// <summary>
        /// 反切片确认
        /// </summary>
        /// <param name="blockInfo"></param>
        /// <param name="userName"></param>
        private async Task<string> Reqpqr(BlockInfoModel blockInfo, string userName)
        {
            string sqlFlow = $"select top 1 * from WorkTest.SampleInfoFlow where testid='{blockInfo.testid}' and state=1 and dstate=0 order by createTime desc";
            //DataTable SampleFlowInfo = HLDBSqlHelper.ExecuteDataset( sqlFlow).Tables[0];
            //DataTable SampleFlowInfo = DbClient.Ado.GetDataTable(sqlFlow);
            DataTable SampleFlowInfo = await _commRepository.GetTable(sqlFlow);
            if (SampleFlowInfo.Rows.Count > 0)
            {


                string FlowNO = SampleFlowInfo.Rows[0]["flowNO"] != DBNull.Value ? SampleFlowInfo.Rows[0]["flowNO"].ToString() : "";


                string FlowID = SampleFlowInfo.Rows[0]["id"] != DBNull.Value ? SampleFlowInfo.Rows[0]["id"].ToString() : "";

                string sql = $"update WorkTest.SampleInfo set groupFlowNO='{FlowNO}' where id='{blockInfo.testid}';";
                sql += $"update WorkTest.SampleInfoFlow set dstate=0 where id='{FlowID}';";
                //sql += $"update WorkTest.ResultPathologyBlock set dstate=1 where testid='{blockInfo.testid}';";
                sql += $"update WorkTest.ResultPathologyBlock set state=2,qpPerson='',qpTime=null where id='{blockInfo.id}';";
                return sql;
            }
            else
            {
                return "";
            }

        }
        /// <summary>
        /// 退回取材
        /// </summary>
        /// <param name="blockInfo"></param>
        /// <param name="userName"></param>
        private async Task<string> Reqc(BlockInfoModel blockInfo, string userName)
        {
            string sqlFlow = $"select top 1 * from WorkTest.SampleInfoFlow where testid='{blockInfo.testid}' and state=1 and dstate=0 order by createTime";
            DataTable SampleFlowInfo = await _commRepository.GetTable(sqlFlow);
            if (SampleFlowInfo.Rows.Count > 0)
            {

                string FlowNO = SampleFlowInfo.Rows[0]["flowNO"] != DBNull.Value ? SampleFlowInfo.Rows[0]["flowNO"].ToString() : "";


                string FlowID = SampleFlowInfo.Rows[0]["id"] != DBNull.Value ? SampleFlowInfo.Rows[0]["id"].ToString() : "";

                string sql = $"update WorkTest.SampleInfo set groupFlowNO='{FlowNO}' where id='{blockInfo.testid}';";
                sql += $"update WorkTest.SampleInfoFlow set dstate=0 where id='{FlowID}';";
                sql += $"update WorkTest.ResultPathologyBlock set dstate=1 where testid='{blockInfo.testid}';";
                return sql;
            }
            else
            {
                return "";
            }
        }
    }
}
