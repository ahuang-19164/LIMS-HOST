using Nito.AsyncEx;
using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Services;
using Yichen.Finance.IRepository;
using Yichen.Finance.Model;
using Yichen.Net.Caching.Manual;
using Yichen.Net.Configuration;
using Yichen.Net.Data;
using Yichen.Net.Table;
using Yichen.Other.IRepository;
using Yichen.Per.IServices;
using Yichen.Per.Model;
using Yichen.Per.Model.table;
using Yichen.System.IRepository;
using Yichen.System.Model;
using Yichen.Test.IRepository;
using Yichen.Test.Model.table;
using Yichen.Net.Caching.Manuals;
using Yichen.Flow.Model;
using Yichen.Per.IRepository;
using SqlSugar;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Model.Entities.Expression;
using Yichen.Finance.Model.table;
using NPOI.Util;
using Yichen.Other.Model.table;

namespace Yichen.Per.Services
{
    /// <summary>
    /// 修改录入信息方法
    /// </summary>
    public class PerInfoHandleServices : BaseServices<per_sampleInfo>, IPerInfoHandleServices
    {
        /// <summary>
        /// 异步锁
        /// </summary>
        private readonly AsyncLock _mutex = new AsyncLock();
        public readonly IUnitOfWork _UnitOfWork;
        private readonly IHttpContextUser _httpContextUser;
        //private readonly PermissionRequirement _permissionRequirement;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRecordRepository _recordRepository;
        private readonly IFinanceInfoRepository _financeInfoRepository;
        private readonly ITestItemInfoRepository _itemHandleRepository;
        private readonly ICommRepository _commRepository;
        private readonly ITestResultInfoRepository _testRepository;
        private readonly IPerSampleInfoRepository _perSampleInfoRepository;
        private readonly ISampleInfoOtherRepository _sampleInfoOtherRepository;
        private readonly ITestSampleInfoRepository _testSampleInfoRepository;
        private readonly ITestItemInfoRepository _testItemInfoRepository;
        private readonly IApplyBillInfoRepository _applyBillInfoRepository;
        private readonly IGroupBillInfoRepository _groupBillInfoRepository;
        private readonly IDelegeteRecordRepository _delegeteRecordRepository;

        //private readonly IItemApplyRepository _itemApplyRepository;
        //private readonly IItemGroupRepository _itemGroupRepository;
        //private readonly IItemTestRepository _itemTestRepository;
        //private readonly IItemFlowRepository _itemFlowRepository;


        public PerInfoHandleServices(IUnitOfWork unitOfWork
            , IRecordRepository recordRepository
            //, IHttpContextAccessor httpContextAccessor
            //, PermissionRequirement permissionRequirement
            , IHttpContextUser httpContextUser
            , IFinanceInfoRepository financeInfoRepository
            , ITestItemInfoRepository itemHandleRepository
            , ICommRepository commRepository
            , ITestResultInfoRepository testRepository
            , IItemApplyRepository itemApplyRepository
            , IItemGroupRepository itemGroupRepository
            , IItemTestRepository itemTestRepository
            , IItemFlowRepository itemFlowRepository
            , IPerSampleInfoRepository perSampleInfoRepository
            , ISampleInfoOtherRepository sampleInfoOtherRepository
            , ITestSampleInfoRepository testSampleInfoRepository
            , ITestItemInfoRepository testItemInfoRepository
            , IApplyBillInfoRepository applyBillInfoRepository
            , IGroupBillInfoRepository groupBillInfoRepository
            , IDelegeteRecordRepository delegeteRecordRepository
            )
        {
            _UnitOfWork = unitOfWork;
            _httpContextUser = httpContextUser;
            //_httpContextAccessor = httpContextAccessor;
            //_permissionRequirement = permissionRequirement;
            _recordRepository = recordRepository;
            _itemHandleRepository = itemHandleRepository;
            _financeInfoRepository = financeInfoRepository;
            _commRepository = commRepository;
            _testRepository = testRepository;
            _perSampleInfoRepository = perSampleInfoRepository;
            _sampleInfoOtherRepository = sampleInfoOtherRepository;
            _testSampleInfoRepository = testSampleInfoRepository;
            _testItemInfoRepository= testItemInfoRepository;
            //_itemApplyRepository = itemApplyRepository;
            //_itemGroupRepository = itemGroupRepository;
            //_itemTestRepository = itemTestRepository;
            //_itemFlowRepository = itemFlowRepository;
            _applyBillInfoRepository = applyBillInfoRepository;
            _groupBillInfoRepository= groupBillInfoRepository;
            _delegeteRecordRepository= delegeteRecordRepository;
        }

        /// <summary>
        /// 插入检验表样本信息
        /// </summary>
        /// <param name="DRsampleInfo">录入样本信息表</param>
        /// <param name="sampleInfoId"></param>
        /// <param name="receiveTime"></param>
        /// <param name="GroupCodes">同一个工作组组合项目编号集合</param>
        /// <param name="GgroupNames">同一个工作组组合项目名称集合</param>
        /// <param name="sampleID"></param>
        /// <param name="GroupNO"></param>
        /// <param name="groupFlowNO"></param>
        /// <param name="delGroupState"></param>
        /// <param name="delGroupClientNO"></param>
        /// <param name="UserName"></param>
        /// <param name="ReceiveState">是否需要接收操作 true 为是   false 不用</param>
        /// <returns></returns>
        private static string InsertTestSampleInfo(per_sampleInfo perinfo, string sampleInfoId, DateTime receiveTime, string GroupCodes, string GroupNames, string sampleID, string GroupNO, string groupFlowNO, bool delGroupState, string delGroupClientNO, string UserName, bool ReceiveState = true)
        {
            iInfo insertInfo = new iInfo();
            insertInfo.TableName = "WorkTest.SampleInfo";
            Dictionary<string, object> pairsInfo = new Dictionary<string, object>();
            pairsInfo.Add("perid", perinfo.id);
            pairsInfo.Add("sampleID", sampleInfoId);
            pairsInfo.Add("disState", 0);
            pairsInfo.Add("dstate", 0);
            pairsInfo.Add("visible", 1);
            int recive = ReceiveState ? 0 : 1;
            pairsInfo.Add("state", recive);
            pairsInfo.Add("connstate", perinfo.connstate);//数据来源 0本地1疾控接收2微信
            int visible = AppSettingsConstVars.SampleSort ? 0 : 1;//开启分拣功能 开启分拣
            pairsInfo.Add("report", 0);
            pairsInfo.Add("sortState", visible);
            pairsInfo.Add("urgent", perinfo.urgent);
            pairsInfo.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            pairsInfo.Add("receiveTime", receiveTime);
            pairsInfo.Add("sampleTime", perinfo.sampleTime);
            pairsInfo.Add("ageDay", perinfo.ageDay);
            pairsInfo.Add("ageMoth", perinfo.ageMoth);
            pairsInfo.Add("pathologyStateNO", 1);
            pairsInfo.Add("testStateNO", 1);
            pairsInfo.Add("patientName", perinfo.patientName);
            pairsInfo.Add("agentNO", perinfo.agentNO);
            pairsInfo.Add("ageYear", perinfo.ageYear);
            pairsInfo.Add("applyItemCodes", perinfo.applyItemCodes);
            pairsInfo.Add("applyItemNames", perinfo.applyItemNames);
            pairsInfo.Add("barcode", perinfo.barcode);
            pairsInfo.Add("bedNo", perinfo.bedNo);
            pairsInfo.Add("clinicalDiagnosis", perinfo.clinicalDiagnosis);
            pairsInfo.Add("creater", UserName);
            pairsInfo.Add("cutPart", perinfo.cutPart);
            pairsInfo.Add("department", perinfo.department);
            pairsInfo.Add("doctorPhone", perinfo.doctorPhone);
            pairsInfo.Add("groupCodes", GroupCodes);
            pairsInfo.Add("groupNames", GroupNames);
            pairsInfo.Add("hospitalBarcode", perinfo.hospitalBarcode);
            pairsInfo.Add("frameNo", perinfo.frameNo);
            pairsInfo.Add("hospitalNames", perinfo.hospitalNames);
            pairsInfo.Add("hospitalNO", perinfo.hospitalNO);
            pairsInfo.Add("medicalNo", perinfo.medicalNo);
            pairsInfo.Add("menstrualTime", perinfo.menstrualTime);
            pairsInfo.Add("number", perinfo.number);
            pairsInfo.Add("passportNo", perinfo.passportNo);
            pairsInfo.Add("pathologyNo", perinfo.pathologyNo);
            pairsInfo.Add("patientAddress", perinfo.patientAddress);
            pairsInfo.Add("patientCardNo", perinfo.patientCardNo);
            pairsInfo.Add("patientPhone", perinfo.patientPhone);
            pairsInfo.Add("patientSexNames", perinfo.patientSexNames);
            pairsInfo.Add("patientSexNO", perinfo.patientSexNO);
            pairsInfo.Add("patientTypeNames", perinfo.patientTypeNames);
            pairsInfo.Add("patientTypeNO", perinfo.patientTypeNO);
            pairsInfo.Add("perRemark", perinfo.perRemark);
            pairsInfo.Add("sampleAddress", perinfo.sampleAddress);
            pairsInfo.Add("sampleLocation", perinfo.sampleLocation);
            pairsInfo.Add("sampleShapeNames", perinfo.sampleShapeNames);
            pairsInfo.Add("sampleShapeNO", perinfo.sampleShapeNO);
            pairsInfo.Add("sampleTypeNames", perinfo.sampleTypeNames);
            pairsInfo.Add("sampleTypeNO", perinfo.sampleTypeNO);
            pairsInfo.Add("sendDoctor", perinfo.sendDoctor);
            pairsInfo.Add("groupNO", GroupNO);
            pairsInfo.Add("groupFlowNO", groupFlowNO);
            pairsInfo.Add("delegateState", delGroupState);
            pairsInfo.Add("delstateClientNO", delGroupClientNO);
            pairsInfo.Add("tabTypeNO", 1);

            insertInfo.values = pairsInfo;
            string aaa = SqlFormartHelper.insertFormart(insertInfo);

            return aaa;

        }

        /// <summary>
        /// 获取审核信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> GetCheckInfo(CheckSelectModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack() { code=0,status=true};
            var wheres = PredicateBuilder.True<per_sampleInfo>();
            if (infos.barcode != null || infos.hosbarcode != null)
            {
                if(infos.barcode != null)
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
                else
                {
                    wheres = wheres.And(p => p.hospitalBarcode == infos.hosbarcode);
                }
            }
            else
            {
                wheres = wheres.And(p => SqlFunc.Between(p.createTime, infos.startTime, infos.endTime));
                if (infos.entryUserName!=null&&infos.entryUserName != "")
                    wheres = wheres.And(p => p.creater == infos.entryUserName);
                if (infos.sampleState!=null&&infos.sampleState != "0")
                    wheres = wheres.And(p => p.perStateNO == Convert.ToInt32(infos.sampleState));
            }
            DataTable perinfo = await _perSampleInfoRepository.QueryDTByClauseAsync(wheres,true);
            jm.data = DataTableHelper.DTToString(perinfo);
            return jm;
        }

        /// <summary>
        /// 编辑样本信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> EditInfo(EntryInfoModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                if (infos.sampleInfos == null)
                {
                    jm.code = 1;
                    jm.msg = "提交信息不能为空";
                }
                else
                {
                    commReInfo<commReSampleInfo> commReInfo = new commReInfo<commReSampleInfo>();
                    List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                    foreach (SampleInfoModel sampleinfo in infos.sampleInfos)
                    {
                        commReSampleInfo reSampleInfo = new commReSampleInfo();
                        string sampleRecord = "";
                        var perinfo = await _perSampleInfoRepository.GetByBarcode(sampleinfo.barcode);
                        if (perinfo == null)
                        {
                            reSampleInfo.code = 1;
                            reSampleInfo.barcode = sampleinfo.barcode;
                            reSampleInfo.msg = "未找到对应样本信息";
                        }
                        else
                        {
                            var perotherinfo = await _sampleInfoOtherRepository.GetInfoByPerid(perinfo.id);
                            DateTime receiveTime = DateTime.MinValue;//接收时间
                            DateTime sampleTime = DateTime.MinValue;//采样时间
                            bool receiveTimestate = false;
                            Dictionary<string, object> perPairs = new Dictionary<string, object>();
                            if (sampleinfo.pairsInfo != null)
                            {
                                //遍历改动的字段
                                foreach (PairsInfoModel item in sampleinfo.pairsInfo)
                                {
                                    reSampleInfo.code = 0;
                                    //条码号修改验证
                                    if (item.columnName == "barcode")
                                    {
                                        if (perinfo.perStateNO != 1)
                                        {
                                            reSampleInfo.code = 1;
                                            reSampleInfo.barcode = sampleinfo.barcode;
                                            reSampleInfo.msg = "条码已审核，不能修改条码信息！";
                                            break;
                                        }
                                        else
                                        {

                                            if (perinfo.barcode != item.valueString.ToString())
                                            {
                                                //条码是否存在
                                                var perstate = await _perSampleInfoRepository.BarcodeExist(item.valueString.ToString());
                                                if (!perstate)
                                                {
                                                    perPairs.Add(item.columnName, item.valueString);

                                                    //setValue += item.columnName + "=N'" + item.valueString + "',";
                                                    sampleRecord += $"[条码号]由[{perinfo.barcode}]更改为[{item.valueString}]。";
                                                }
                                                else
                                                {
                                                    reSampleInfo.code = 1;
                                                    reSampleInfo.barcode = sampleinfo.barcode;
                                                    reSampleInfo.msg = $":修改条码{item.valueString.ToString()}已存在！";
                                                    break;
                                                }

                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (item.columnName == "hospitalNO")
                                        {
                                            if (perinfo.perStateNO != 1)
                                            {
                                                reSampleInfo.code = 1;
                                                reSampleInfo.barcode = sampleinfo.barcode;
                                                reSampleInfo.msg = "条码已审核，不能修改客户信息！";
                                                break;
                                            }
                                            else
                                            {
                                                if (perinfo.hospitalNO != item.valueString.ToString())
                                                {
                                                    perPairs.Add(item.columnName, item.valueString);
                                                    sampleRecord += $"[{item.caption}]由[{perinfo.hospitalNO}]更改为[{item.valueString}]。";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.columnName == "sampleTime")
                                            {
                                                sampleTime = Convert.ToDateTime(item.valueString);
                                            }
                                            if (item.columnName == "receiveTime")
                                            {
                                                receiveTimestate = true;
                                                receiveTime = Convert.ToDateTime(item.valueString);
                                            }
                                            string oldVlue = perinfo.GetType().GetProperty(item.columnName).GetValue(perinfo, null) != null ? perinfo.GetType().GetProperty(item.columnName).GetValue(perinfo, null).ToString() : "";
                                            ///判断修改字段信息
                                            if (oldVlue != item.valueString.ToString())
                                            {
                                                perPairs.Add(item.columnName, item.valueString);
                                                sampleRecord += $"[{item.caption}]由[{oldVlue}]更改为[{item.valueString}]。";
                                            }
                                        }
                                    }
                                }
                                perPairs.Add("editer", infos.UserName);
                                perPairs.Add("editTime", DateTime.Now);
                            }

                            if (receiveTimestate && sampleTime > receiveTime)
                            {
                                reSampleInfo.code = 1;
                                reSampleInfo.barcode = sampleinfo.barcode;
                                reSampleInfo.msg = "采样时间大于接收时间";
                                continue;
                            }

                            //判断项目是否有改动
                            if (perinfo.perStateNO == 1 && sampleinfo.applyCodes != "" && sampleinfo.applyCodes != perinfo.applyItemCodes)
                            {
                                perPairs.Add("applyItemCodes", sampleinfo.applyCodes);
                                perPairs.Add("applyItemNames", sampleinfo.applyNames);
                                sampleRecord += $"申请项目：由[{perinfo.applyItemCodes}][{perinfo.applyItemNames}]更改为[{sampleinfo.applyCodes}][{sampleinfo.applyNames}]。";
                            }

                            if (perinfo.perStateNO == 1)
                            {
                                int perEdit = await _perSampleInfoRepository.EntryInfoEdit(perinfo.id, perPairs);
                                if (perEdit > 0)
                                {
                                    reSampleInfo.code = 0;
                                    reSampleInfo.barcode = sampleinfo.barcode;
                                    reSampleInfo.msg = "修改成功";
                                    if (sampleRecord.Length > 0)
                                        await _recordRepository.SampleRecord(sampleinfo.barcode, RecordEnumVars.EditInfo, sampleRecord, infos.UserName, false);
                                }
                            }
                            else
                            {
                                int perEdit = await _perSampleInfoRepository.EntryInfoEdit(perinfo.id, perPairs);
                                if (perEdit > 0)
                                {
                                    int testEdit = await _testSampleInfoRepository.TestInfoEdit(perinfo.id.ToString(), perPairs);
                                    if (testEdit > 0)
                                    {
                                        reSampleInfo.code = 0;
                                        reSampleInfo.barcode = sampleinfo.barcode;
                                        reSampleInfo.msg = "修改成功";
                                        if (sampleRecord.Length > 0)
                                            await _recordRepository.SampleRecord(sampleinfo.barcode, RecordEnumVars.EditInfo, sampleRecord, infos.UserName, false);
                                    }
                                }
                            }

                            reSampleInfos.Add(reSampleInfo);
                        }
                    }

                    jm.code = 0;
                    commReInfo.infos = reSampleInfos;
                }
            }
            return jm;
        }
        /// <summary>
        /// 补打条码
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> CheckBc(CheckInfoModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            commReInfo<ReCheckeModel> commReInfo = new commReInfo<ReCheckeModel>();
            List<ReCheckeModel> reCheckeModels = new List<ReCheckeModel>();
            try
            {
                if (infos.checkSampleInfos == null && infos.checkSampleInfos.Count == 0)
                {
                    jm.code = 1;
                    jm.msg = "未提交样本信息";
                }
                else
                {

                    ReCheckeModel reCheckeModel = new ReCheckeModel();
                    List<CheckGroupBarcode> listInfos = new List<CheckGroupBarcode>();
                    foreach (CheckSampleInfoModel checkSample in infos.checkSampleInfos)
                    {
                        var testinfo = await _testRepository.GetTestSampleInfo(checkSample.barcode);
                        if (testinfo != null)
                        {
                            CheckGroupBarcode info = new CheckGroupBarcode();
                            info.barcode = testinfo.barcode;
                            info.hospitalNames = testinfo.hospitalNames;
                            info.sampleID = Convert.ToInt32(testinfo.id);
                            info.patientSexNames = testinfo.patientSexNames;
                            info.patientName = testinfo.patientName;
                            info.ageYear = testinfo.ageYear;
                            info.groupNO = testinfo.groupNO;
                            info.groupCodes = testinfo.groupCodes;
                            info.groupNames = testinfo.groupNames;
                            listInfos.Add(info);
                        }
                    }
                    reCheckeModel.groupcodeInfo = listInfos;
                    reCheckeModels.Add(reCheckeModel);
                    commReInfo.infos = reCheckeModels;
                    jm.data = commReInfo;


                }
            }
            catch (Exception ex)
            {
                jm.code = 1;
                jm.msg = ex.Message;
            }
            return jm;
        }


        /// <summary>
        /// 审核信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CheckInfos(CheckInfoModel infos)
        {

            WebApiCallBack jm = new WebApiCallBack();
            //using (await _mutex.LockAsync())
            //{
            if (infos.checkSampleInfos == null)
            {
                jm.code = 1;
                jm.msg = $"没有找到审核信息";
            }
            else
            {
                commReInfo<ReCheckeModel> commReInfo = new commReInfo<ReCheckeModel>();
                List<ReCheckeModel> reCheckeModels = new List<ReCheckeModel>();
                foreach (CheckSampleInfoModel sampleInfo in infos.checkSampleInfos)
                {
                    //返回条码处理状态
                    ReCheckeModel reCheckeModel = new ReCheckeModel();
                    reCheckeModel.code = 0;
                    //string sql = "";
                    if (sampleInfo.barcode == null && sampleInfo.perid == 0)
                    {
                        reCheckeModel.code = 1;
                        reCheckeModel.barcode = sampleInfo.barcode;
                        reCheckeModel.msg = $"提交的样本信息未包含条码信息";
                    }
                    else
                    {
                        int blendConut = 1;
                        var perinfo = await _perSampleInfoRepository.QueryByIdAsync(sampleInfo.perid);

                        //判断录入信息是否存在
                        if (perinfo == null)
                        {
                            reCheckeModel.code = 1;
                            reCheckeModel.barcode = sampleInfo.barcode;
                            reCheckeModel.msg = $"没有找到审核信息/样本已审核";
                        }
                        else
                        {

                            //判断录入信息是否存在
                            if (perinfo.perStateNO != 1 || perinfo.dstate == true)
                            {
                                reCheckeModel.code = 1;
                                reCheckeModel.barcode = sampleInfo.barcode;
                                reCheckeModel.msg = $"没有找到审核信息/样本已审核";
                            }
                            else
                            {
                                string agentNO = perinfo.agentNO != null ? perinfo.agentNO.ToString() : "";
                                string hospitalNO = perinfo.hospitalNO != null ? perinfo.hospitalNO.ToString() : "";
                                string patientType = perinfo.patientTypeNO != null ? perinfo.patientTypeNO.ToString() : "";
                                string department = perinfo.department != null ? perinfo.department.ToString() : "";
                                DateTime sampleTime = perinfo.sampleTime != null ? Convert.ToDateTime(perinfo.sampleTime) : DateTime.MinValue;
                                DateTime receiveTime = perinfo.receiveTime != null ? Convert.ToDateTime(perinfo.receiveTime) : DateTime.Now;

                                if (sampleTime > receiveTime)
                                {
                                    reCheckeModel.code = 1;
                                    reCheckeModel.barcode = sampleInfo.barcode;
                                    reCheckeModel.msg = "采样时间不能大于接收时间";
                                }
                                else
                                {

                                    if (hospitalNO == "")
                                    {
                                        reCheckeModel.code = 1;
                                        reCheckeModel.barcode = sampleInfo.barcode;
                                        reCheckeModel.msg = "未找到匹配的客户信息";
                                    }
                                    else
                                    {


                                        FinanceClientModel financeClient = await _financeInfoRepository.GetFinanceClient(hospitalNO);//获取医院收费信息


                                        int perid = perinfo.id;

                                        if (perinfo.perStateNO != 1)
                                        {
                                            reCheckeModel.code = 1;
                                            reCheckeModel.barcode = sampleInfo.barcode;
                                            reCheckeModel.msg = "样本信息不能审核";
                                        }
                                        else
                                        {

                                            if (perinfo.applyItemCodes == null)
                                            {
                                                reCheckeModel.code = 1;
                                                reCheckeModel.barcode = perinfo.barcode;
                                                reCheckeModel.msg = "样本信息不能审核";
                                            }
                                            else
                                            {


                                                //当前条码所有组套项目编号
                                                List<string> applyItemCodes = perinfo.applyItemCodes.ToString().Split(",").ToList();
                                                //读取内存中的组套信息
                                                List<comm_item_apply> ApplyInfoss = ManualDataCache<comm_item_apply>.MemoryCache.LIMSGetKeyValue(CommInfo.itemapply);
                                                //List<comm_item_apply> ApplyInfoss =await _itemApplyRepository.GetCaChe();
                                                //查询样本信息中包含的组套信息
                                                List<comm_item_apply> ApplyInfos = ApplyInfoss.Where(p => applyItemCodes.Contains(p.no.ToString()) && p.dstate == false).ToList();


                                                if (ApplyInfos==null|| ApplyInfos.Count ==0)
                                                {
                                                    reCheckeModel.code = 1;
                                                    reCheckeModel.barcode = sampleInfo.barcode;
                                                    reCheckeModel.msg = "未找到样本项目信息";
                                                }
                                                else
                                                {

                                                    decimal standerchargeCount = 0;//标准收费总金额
                                                    decimal settlementChargeCount = 0;//结算收费总金额
                                                    decimal chargeCount = 0;//实际收费总金额

                                                    string sqlAll = "";



                                                    //条码集合对象
                                                    List<CheckGroupBarcode> checkBarcodes = new List<CheckGroupBarcode>();
                                                    //存放所有组合项目编号
                                                    string groupItemCodeString = "";
                                                    foreach (comm_item_apply comm_Item_Apply in ApplyInfos)
                                                    {
                                                        groupItemCodeString += comm_Item_Apply.groupItemList;
                                                    }
                                                    //存放所有组合项目编号信息
                                                    List<string> groupItemCodes = groupItemCodeString.Split(",").ToList();

                                                    //读取内存中的组和信息
                                                    List<comm_item_group> GroupInfoss = ManualDataCache<comm_item_group>.MemoryCache.LIMSGetKeyValue(CommInfo.itemgroup);
                                                    //List<comm_item_group> GroupInfoss = await _itemGroupRepository.GetCaChe();
                                                    //读取子项信息获取子项集合
                                                    List<comm_item_test> TestInfoss = ManualDataCache<comm_item_test>.MemoryCache.LIMSGetKeyValue(CommInfo.itemtest);
                                                    //读取子项信息获取流程集合
                                                    //List<comm_item_flow> FlowInfoss =await _itemFlowRepository.GetCaChe();
                                                    List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);





                                                    //查询样本信息中包含的组和信息
                                                    List<comm_item_group> GroupInfos = GroupInfoss.Where(p => groupItemCodes.Contains(p.no.ToString()) && p.dstate == false).ToList();

                                                    if (GroupInfos==null||GroupInfos.Count ==0)
                                                    {
                                                        reCheckeModel.code = 1;
                                                        reCheckeModel.barcode = sampleInfo.barcode;
                                                        reCheckeModel.msg = "未找到组套绑定的组合项目信息";
                                                    }
                                                    else
                                                    {

                                                        while (GroupInfos.Count > 0)
                                                        {
                                                            //foreach (comm_item_group comm_Item_Group in GroupInfos)
                                                            //{
                                                            comm_item_group comm_Item_Group = GroupInfos.First();



                                                            ///获取同一专业组的组合项目信息
                                                            List<comm_item_group> TestGroupCodes = GroupInfos.Where(p => p.groupNO == comm_Item_Group.groupNO && p.dstate == false).ToList();


                                                            //foreach (comm_item_group testgroupinfo in TestGroupCodes)
                                                            //{
                                                            ///获取同一专业组同一流程的项目的组合项目信息
                                                            List<comm_item_group> TestGroupFlowCodes = TestGroupCodes.Where(p => p.groupFlowNO == comm_Item_Group.groupFlowNO && p.dstate == false).ToList();

                                                            //唯一标识
                                                            string strGuidN = Guid.NewGuid().ToString("N");


                                                            //存放组合项目的编号信息
                                                            string groupcodes = "";
                                                            //存放组合项目的名称信息
                                                            string groupNames = "";

                                                            //存放同一专业组同一流程的所有子项项目编号
                                                            string testItemCodeString = "";

                                                            //插入到检验中的样本信息
                                                            string testinfoSql = "";
                                                            //结果插入信息
                                                            string ItemResultInfoSql = "";
                                                            //委托插入信息
                                                            string DelegeteRecordSql = "";
                                                            //收费插入信息
                                                            string FinanceGroupSql = "";

                                                            bool delGroupState = false;


                                                            string grouoNo = "";
                                                            string delegeteCompanyNO = "";
                                                            foreach (comm_item_group TestGroupFlowCode in TestGroupFlowCodes)
                                                            {
                                                                groupcodes += TestGroupFlowCode.no + ",";
                                                                groupNames += TestGroupFlowCode.names + ",";
                                                                testItemCodeString += TestGroupFlowCode.testItemList;

                                                                grouoNo = TestGroupFlowCode.groupNO;


                                                                delegeteCompanyNO = TestGroupFlowCode.delegeteCompanyNO;


                                                                //获取组合项目折扣信息
                                                                DiscountPriceModel GroupPriceinfo = await _financeInfoRepository.GetGroupPrice(agentNO, hospitalNO, patientType, department, TestGroupFlowCode.no.ToString(), financeClient.chargeLevelNO, financeClient.discount);
                                                                //记录项目收费价格
                                                                standerchargeCount += GroupPriceinfo.standerPirce;//标准收费总金额
                                                                settlementChargeCount += GroupPriceinfo.settlementPirce;//结算收费总金额
                                                                chargeCount += GroupPriceinfo.chargePice;//实际收费总金额

                                                                //生成组合项目收费信息
                                                                FinanceGroupSql = "insert into Finance.GroupBillInfo (perid,testid,barcode,tabTypeNO,groupNO,chargeTypeNO,chargeLevelNO,discountState,discount,personNO,groupCode,groupName,operater,operatTime,standerCharge,settlementCharge,charge,dstate)";
                                                                FinanceGroupSql += $" values ('{sampleInfo.perid}',@testid,'{sampleInfo.barcode}','1','{TestGroupFlowCode.groupNO}','{GroupPriceinfo.chargeTypeNO}','{financeClient.chargeLevelNO}','{GroupPriceinfo.groupDiscountState}','{financeClient.discount}','{financeClient.personNO}','{TestGroupFlowCode.no}','{TestGroupFlowCode.names}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{GroupPriceinfo.standerPirce}','{GroupPriceinfo.settlementPirce}','{GroupPriceinfo.chargePice}',0);";

                                                                //移除遍历过的组合项目信息
                                                                //TestGroupCodes.Remove(TestGroupFlowCode);
                                                                GroupInfos.Remove(TestGroupFlowCode);
                                                            }


                                                            //记录条码信息
                                                            CheckGroupBarcode testInfoBarcode1 = new CheckGroupBarcode();
                                                            testInfoBarcode1.sampleID = sampleInfo.perid;
                                                            testInfoBarcode1.groupNO = grouoNo;
                                                            testInfoBarcode1.patientName = perinfo.patientName != null ? perinfo.patientName.ToString() : "";
                                                            testInfoBarcode1.barcode = sampleInfo.barcode;
                                                            testInfoBarcode1.groupCodes = groupcodes;
                                                            testInfoBarcode1.groupNames = groupNames;
                                                            checkBarcodes.Add(testInfoBarcode1);





                                                            //存放同一专业组同一流程下的子项项目编号信息
                                                            List<string> TestItemCodes = testItemCodeString.Split(",").ToList();


                                                            //List<comm_item_test> TestInfoss =await _itemTestRepository.GetCaChe();
                                                            //查询样本信息中包含的子项信息 并按照专业组进行排序
                                                            List<comm_item_test> TestInfos = TestInfoss.Where(p => TestItemCodes.Contains(p.no.ToString()) && p.dstate == false).OrderBy(p => p.sort).ToList();

                                                            //查询样本信息中包含的流程信息 并按照专业组进行排序
                                                            comm_item_flow FlowInfos = FlowInfoss.First(p => p.no == Convert.ToInt32(comm_Item_Group.groupFlowNO) && p.dstate == false);
                                                            //获取流程中存放结果数据表名
                                                            string srouceTableName = FlowInfos.dataSource != null ? FlowInfos.dataSource.ToString() : "";
                                                            //获取流程中存放图片数据表名
                                                            string imgTableName = FlowInfos.imgSource != null ? FlowInfos.imgSource.ToString() : "";

                                                            //判断组合项目是否在委托组
                                                            if (grouoNo != "1")
                                                            {
                                                                //项目排序计数器
                                                                int ItemSort = 0;
                                                                string itemcodes = "";
                                                                string itemNames = "";
                                                                string delitemcodes = "";
                                                                string delitemNames = "";

                                                                //判断是否存在委托项目
                                                                bool delItemState = false;
                                                                //项目委托医院编号
                                                                string delItemClientNO = "";
                                                                foreach (comm_item_test testiteminfo in TestInfos)
                                                                {
                                                                    ItemSort += 1;
                                                                    itemcodes += testiteminfo.no + ",";
                                                                    itemNames += testiteminfo.names + ",";
                                                                    //comm_item_test itemInfo = TestInfos.First(p => p.no == Convert.ToInt32(itemCode));
                                                                    delItemState = testiteminfo.delegeteState != null ? Convert.ToBoolean(testiteminfo.delegeteState) : false;
                                                                    bool resultNullState = testiteminfo.resultNullState != null ? Convert.ToBoolean(testiteminfo.resultNullState) : false;
                                                                    bool reportState = testiteminfo.visibleState != null ? Convert.ToBoolean(testiteminfo.visibleState) : false;
                                                                    delItemClientNO = delItemState && testiteminfo.delegeteCompanyNO != null ? testiteminfo.delegeteCompanyNO.ToString() : "";
                                                                    if (delItemState)
                                                                    {
                                                                        delitemcodes += testiteminfo.no + ",";
                                                                        delitemNames += testiteminfo.names + ",";
                                                                        ItemResultInfoSql += $"insert into {srouceTableName} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate,resultNullState,reportState)";
                                                                        ItemResultInfoSql += $"values ({sampleInfo.perid},@testid,'{sampleInfo.barcode}','{grouoNo}','{groupcodes}','{groupNames}','{testiteminfo.no}','{testiteminfo.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0,'{resultNullState}','{reportState}');";
                                                                    }
                                                                    else
                                                                    {
                                                                        ItemResultInfoSql += $"insert into {srouceTableName} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate,resultNullState,reportState)";
                                                                        ItemResultInfoSql += $"values ({sampleInfo.perid},@testid,'{sampleInfo.barcode}','{grouoNo}','{groupcodes}','{groupNames}','{testiteminfo.no}','{testiteminfo.names}','{ItemSort}','0','','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0,'{resultNullState}','{reportState}');";
                                                                    }
                                                                    //if (comm_Item_Group.groupFlowNO != "2")
                                                                    //{
                                                                    //    testinfoSql = InsertTestSampleInfo(DTsampleInfo.Rows[0], strGuidN, receiveTime, groupcodes, groupNames, "", comm_Item_Group.groupNO, comm_Item_Group.groupFlowNO, delGroupState, delegeteCompanyNO, infos.UserName);
                                                                    //    sqlAll += testinfoSql + "set @testid=@@IDENTITY;" + ItemResultInfoSql + FinanceGroupSql;
                                                                    //}

                                                                }
                                                                if (delItemState)
                                                                {

                                                                    DelegeteRecordSql += "insert into WorkOther.DelegeteRecord (perid,testid,barcode,delegateStateNO,delegateClientNO,reason,itemCodes,itemNames,checker,checkTime,createTime.dstate)";
                                                                    DelegeteRecordSql += $"values ('{sampleInfo.perid}',@testid,'{sampleInfo.barcode}','2',{delItemClientNO},'默认委托','{itemcodes}','{itemNames}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',0);";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //项目排序计数器
                                                                int ItemSort = 0;
                                                                string delitemcodes = "";
                                                                string delitemNames = "";


                                                                foreach (comm_item_test testiteminfo in TestInfos)
                                                                {
                                                                    ItemSort += 1;
                                                                    delitemcodes += testiteminfo.no + ",";
                                                                    delitemNames += testiteminfo.names + ",";

                                                                    bool delItemState = testiteminfo.delegeteState != null ? Convert.ToBoolean(testiteminfo.delegeteState) : false;
                                                                    bool resultNullState = testiteminfo.resultNullState != null ? Convert.ToBoolean(testiteminfo.resultNullState) : false;
                                                                    bool reportState = testiteminfo.visibleState != null ? Convert.ToBoolean(testiteminfo.visibleState) : false;
                                                                    if (delItemState)
                                                                    {
                                                                        delGroupState = true;
                                                                    }
                                                                    string delItemClientNO = delItemState && testiteminfo.delegeteCompanyNO != null ? testiteminfo.delegeteCompanyNO.ToString() : "";
                                                                    ItemResultInfoSql += $"insert into {srouceTableName} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate,resultNullState,reportState)";
                                                                    ItemResultInfoSql += $"values ({sampleInfo.perid},@testid,'{sampleInfo.barcode}','{grouoNo}','{groupcodes}','{groupNames}','{testiteminfo.no}','{testiteminfo.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0,'{resultNullState}','{reportState}');";

                                                                    DelegeteRecordSql += "insert into WorkOther.DelegeteRecord (perid,testid,barcode,delegateStateNO,delegateClientNO,reason,itemCodes,itemNames,checker,checkTime,createTime,dstate)";
                                                                    DelegeteRecordSql += $"values ('{sampleInfo.perid}',@testid,'{sampleInfo.barcode}','2',{delItemClientNO},'默认委托','{testiteminfo.no}','{testiteminfo.names}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',0);";

                                                                    //if (comm_Item_Group.groupFlowNO != "2")
                                                                    //{
                                                                    //    testinfoSql = InsertTestSampleInfo(DTsampleInfo.Rows[0], strGuidN, receiveTime, groupcodes, groupNames, "", comm_Item_Group.groupNO, comm_Item_Group.groupFlowNO, delGroupState, delegeteCompanyNO, infos.UserName);
                                                                    //    sqlAll += testinfoSql + "set @testid=@@IDENTITY;" + ItemResultInfoSql + FinanceGroupSql;
                                                                    //}
                                                                }


                                                            }
                                                            //if (comm_Item_Group.groupFlowNO == "2")
                                                            //{
                                                            testinfoSql = InsertTestSampleInfo(perinfo, strGuidN, receiveTime, groupcodes, groupNames, "", comm_Item_Group.groupNO, comm_Item_Group.groupFlowNO, delGroupState, delegeteCompanyNO, infos.UserName);
                                                            sqlAll += testinfoSql + "set @testid=@@IDENTITY;" + ItemResultInfoSql + FinanceGroupSql + DelegeteRecordSql;
                                                            //}
                                                        }

                                                        ///插入收费总和信息
                                                        if (sqlAll != "")
                                                        {
                                                            iInfo insertInfo = new iInfo();
                                                            insertInfo.TableName = "Finance.ApplyBillInfo";
                                                            Dictionary<string, object> pairs = new Dictionary<string, object>();
                                                            pairs.Add("discount", financeClient.discount);
                                                            pairs.Add("operatTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                                            pairs.Add("charge", chargeCount);
                                                            pairs.Add("settlementCharge", settlementChargeCount);
                                                            pairs.Add("standerCharge", standerchargeCount);
                                                            pairs.Add("operater", infos.UserName);
                                                            pairs.Add("barcode", sampleInfo.barcode);
                                                            pairs.Add("chargeLevel", financeClient.chargeLevelNO);
                                                            pairs.Add("chargeTypeNO", "1");
                                                            pairs.Add("perid", perid);

                                                            insertInfo.values = pairs;
                                                            string insertApplyPirceInfo = SqlFormartHelper.insertFormart(insertInfo);

                                                            //insertGroupInfos += InsertTestSampleInfo(DTsampleInfo.Rows[0], grouCodes, groupNames, "", groupNO, groupFlowNO, delGroupState, delGroupClientNO, infos.UserName) + "set @testid=@@IDENTITY;";
                                                            sqlAll += insertApplyPirceInfo;
                                                        }
                                                        if (reCheckeModel.code == 0)
                                                        {
                                                            //int aaa = HLDBSqlHelper.ExecuteNonQuery( "declare @testid varchar(500);" + insertSampleInfoAll);
                                                            await _commRepository.sqlcommand("declare @testid varchar(500);" + sqlAll);
                                                            //int aaa = HLDBSqlHelper.ExecuteNonQuery( "declare @testid varchar(500);" + insertSampleInfoAll);
                                                            //Result = "审核成功！";
                                                            reCheckeModel.code = 0;
                                                            reCheckeModel.barcode = sampleInfo.barcode;
                                                            reCheckeModel.groupcodeInfo = checkBarcodes;
                                                            reCheckeModel.msg = $"审核成功！";

                                                            string updateperInfo = "";
                                                            if (blendConut == 1)
                                                            {
                                                                updateperInfo = $"update WorkPer.SampleInfo set receiveTime='{receiveTime}',perStateNO=2,state=1,checker='{infos.UserName}',checkTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where barcode='{sampleInfo.barcode}' and id='{sampleInfo.perid}';";
                                                            }
                                                            else
                                                            {
                                                                updateperInfo = $"update WorkPer.SampleInfo set  number='{blendConut}',receiveTime='{receiveTime}',perStateNO=2,state=1,checker='{infos.UserName}',checkTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where barcode='{sampleInfo.barcode}' and id='{sampleInfo.perid}';";
                                                            }

                                                            //updateperInfo += $"update WorkPer.SampleInfo set perStateNO=2,state=1,checker='{infos.UserName}',checkTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where barcode='{sampleInfo.barcode}' and id='{sampleInfo.perid}'";

                                                            await _commRepository.sqlcommand(updateperInfo);
                                                            await _recordRepository.SampleRecord(sampleInfo.barcode, RecordEnumVars.Check, "审核成功！", infos.UserName, true);


                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    reCheckeModels.Add(reCheckeModel);
                }
                commReInfo.infos = reCheckeModels;
                jm.data = commReInfo;
            }
            //}
            return jm;
        }

        /// <summary>
        /// 审核信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CheckInfoss(CheckInfoModel infos)
        {

            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                if (infos.checkSampleInfos != null)
                {
                    commReInfo<ReCheckeModel> commReInfo = new commReInfo<ReCheckeModel>();
                    List<ReCheckeModel> reCheckeModels = new List<ReCheckeModel>();
                    //_UnitOfWork.BeginTran();
                    try
                    {
                        foreach (CheckSampleInfoModel sampleInfo in infos.checkSampleInfos)
                        {

                            //返回条码处理状态
                            ReCheckeModel reCheckeModel = new ReCheckeModel();
                            reCheckeModel.code = 0;
                            //string sql = "";
                            if (sampleInfo.perid == 0)
                            {
                                reCheckeModel.code = 1;
                                reCheckeModel.barcode = sampleInfo.barcode;
                                reCheckeModel.msg = $"提交的样本信息未包含条码信息";
                            }
                            else
                            {
                                int blendConut = 1;
                                var perinfo = await _perSampleInfoRepository.QueryByIdAsync(sampleInfo.perid);

                                //判断录入信息是否存在
                                if (perinfo.perStateNO != 1 || perinfo.dstate == true)
                                {
                                    reCheckeModel.code = 1;
                                    reCheckeModel.barcode = sampleInfo.barcode;
                                    reCheckeModel.msg = $"没有找到审核信息/样本已审核";
                                }
                                else
                                {
                                    string agentNO = perinfo.agentNO != null ? perinfo.agentNO.ToString() : "";
                                    string hospitalNO = perinfo.hospitalNO != null ? perinfo.hospitalNO.ToString() : "";
                                    string patientType = perinfo.patientTypeNO != null ? perinfo.patientTypeNO.ToString() : "";
                                    string department = perinfo.department != null ? perinfo.department.ToString() : "";
                                    DateTime sampleTime = perinfo.sampleTime != null ? Convert.ToDateTime(perinfo.sampleTime) : DateTime.MinValue;
                                    DateTime receiveTime = perinfo.receiveTime != null ? Convert.ToDateTime(perinfo.receiveTime) : DateTime.Now;

                                    if (sampleTime > receiveTime)
                                    {
                                        reCheckeModel.code = 1;
                                        reCheckeModel.barcode = sampleInfo.barcode;
                                        reCheckeModel.msg = "采样时间不能大于接收时间";
                                    }
                                    else
                                    {

                                        if (hospitalNO == "")
                                        {
                                            reCheckeModel.code = 1;
                                            reCheckeModel.barcode = sampleInfo.barcode;
                                            reCheckeModel.msg = "未找到匹配的客户信息";
                                        }
                                        else
                                        {
                                            //获取医院收费信息
                                            FinanceClientModel financeClient = await _financeInfoRepository.GetFinanceClient(hospitalNO);

                                            int perid = perinfo.id;

                                            if (perinfo.perStateNO != 1)
                                            {
                                                reCheckeModel.code = 1;
                                                reCheckeModel.barcode = perinfo.barcode;
                                                reCheckeModel.msg = "样本信息不能审核";
                                            }
                                            else
                                            {
                                                if (perinfo.applyItemCodes == null)
                                                {
                                                    reCheckeModel.code = 1;
                                                    reCheckeModel.barcode = perinfo.barcode;
                                                    reCheckeModel.msg = "样本信息不能审核";
                                                }
                                                else
                                                {

                                                    //当前条码所有组套项目编号
                                                    List<string> applyItemCodes = perinfo.applyItemCodes.ToString().Split(",").ToList();
                                                    //读取内存中的组套信息
                                                    List<comm_item_apply> ApplyInfoss = ManualDataCache<comm_item_apply>.MemoryCache.LIMSGetKeyValue(CommInfo.itemapply);
                                                    //List<comm_item_apply> ApplyInfoss =await _itemApplyRepository.GetCaChe();
                                                    //查询样本信息中包含的组套信息
                                                    List<comm_item_apply> ApplyInfos = ApplyInfoss.Where(p => applyItemCodes.Contains(p.no.ToString()) && p.dstate == false).ToList();
                                                    if (ApplyInfos.Count == 0)
                                                    {
                                                        reCheckeModel.code = 1;
                                                        reCheckeModel.barcode = sampleInfo.barcode;
                                                        reCheckeModel.msg = "未找到样本项目信息";
                                                    }
                                                    else
                                                    {

                                                        decimal standerchargeCount = 0;//标准收费总金额
                                                        decimal settlementChargeCount = 0;//结算收费总金额
                                                        decimal chargeCount = 0;//实际收费总金额
                                                                                //条码集合对象
                                                        List<CheckGroupBarcode> checkBarcodes = new List<CheckGroupBarcode>();
                                                        //存放所有组合项目编号
                                                        string groupItemCodeString = "";
                                                        foreach (comm_item_apply comm_Item_Apply in ApplyInfos)
                                                        {
                                                            groupItemCodeString += comm_Item_Apply.groupItemList;
                                                        }
                                                        //存放所有组合项目编号信息
                                                        List<string> groupItemCodes = groupItemCodeString.Split(",").ToList();
                                                        //读取内存中的组和信息
                                                        List<comm_item_group> GroupInfoss = ManualDataCache<comm_item_group>.MemoryCache.LIMSGetKeyValue(CommInfo.itemgroup);

                                                        //读取子项信息获取子项集合
                                                        List<comm_item_test> TestInfoss = ManualDataCache<comm_item_test>.MemoryCache.LIMSGetKeyValue(CommInfo.itemtest);

                                                        //读取子项信息获取流程集合
                                                        List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);

                                                        //查询样本信息中包含的组和信息

                                                        List<comm_item_group> GroupInfos = GroupInfoss.Where(p => groupItemCodes.Contains(p.no.ToString()) && p.dstate == false).ToList();

                                                        if (GroupInfos.Count > 0)
                                                        {
                                                            while (GroupInfos.Count > 0)
                                                            {
                                                                comm_item_group comm_Item_Group = GroupInfos.First();
                                                                ///获取同一专业组的组合项目信息
                                                                List<comm_item_group> GroupItemAll = GroupInfos.Where(p => p.groupNO == comm_Item_Group.groupNO && p.dstate == false).ToList();

                                                                ///获取同一专业组同一流程的项目的组合项目信息
                                                                List<comm_item_group> GroupItems = GroupItemAll.Where(p => p.groupFlowNO == comm_Item_Group.groupFlowNO && p.dstate == false).ToList();

                                                                //唯一标识
                                                                string strGuidN = Guid.NewGuid().ToString("N");
                                                                //存放组合项目的编号信息
                                                                string groupcodes = "";
                                                                //存放组合项目的名称信息
                                                                string groupNames = "";
                                                                //存放同一专业组同一流程的所有子项项目编号
                                                                string testItemCodeString = "";
                                                                //插入到检验中的样本信息
                                                                string testinfoSql = "";
                                                                //结果插入信息
                                                                string ItemResultInfoSql = "";
                                                                //委托插入信息
                                                                string DelegeteRecordSql = "";
                                                                //收费插入信息
                                                                string FinanceGroupSql = "";
                                                                //是否委托
                                                                bool delGroupState = false;
                                                                //专业组编号
                                                                string groupNO = "";
                                                                //委托单位编号
                                                                string delegeteCompanyNO = "";
                                                                //组合项目收费记录
                                                                List<GroupBillInfo> GroupBillInfoPairs = new List<GroupBillInfo>();
                                                                //子项目插入集合
                                                                List<Dictionary<string, object>> ItemResultInfoPairs = new List<Dictionary<string, object>>();
                                                                //委托项目插入集合
                                                                List<DelegeteRecord> DelegeteRecordPairs = new List<DelegeteRecord>();

                                                                foreach (comm_item_group GroupItem in GroupItems)
                                                                {
                                                                    groupcodes += GroupItem.no + ",";
                                                                    groupNames += GroupItem.names + ",";
                                                                    testItemCodeString += GroupItem.testItemList;
                                                                    groupNO = GroupItem.groupNO;
                                                                    delegeteCompanyNO = GroupItem.delegeteCompanyNO;
                                                                    //获取组合项目折扣信息
                                                                    DiscountPriceModel GroupPriceinfo = await _financeInfoRepository.GetGroupPrice(agentNO, hospitalNO, patientType, department, GroupItem.no.ToString(), financeClient.chargeLevelNO, financeClient.discount);
                                                                    //记录项目收费价格
                                                                    standerchargeCount += GroupPriceinfo.standerPirce;//标准收费总金额
                                                                    settlementChargeCount += GroupPriceinfo.settlementPirce;//结算收费总金额
                                                                    chargeCount += GroupPriceinfo.chargePice;//实际收费总金额

                                                                    //生成组合项目收费信息
                                                                    GroupBillInfo GroupBillInfoPair = new GroupBillInfo()
                                                                    {
                                                                        perid = perid.ToString(),
                                                                        barcode = sampleInfo.barcode,
                                                                        tabTypeNO = "1",
                                                                        groupNO = groupNO,
                                                                        chargeTypeNO = GroupPriceinfo.chargeTypeNO,
                                                                        chargeLevelNO = financeClient.chargeLevelNO,
                                                                        discountState = GroupPriceinfo.groupDiscountState,
                                                                        discount = financeClient.discount.ToString(),
                                                                        personNO = financeClient.personNO,
                                                                        groupCode = GroupItem.no.ToString(),
                                                                        groupName = GroupItem.names,
                                                                        operater = infos.UserName,
                                                                        operatTime = DateTime.Now,
                                                                        standerCharge = GroupPriceinfo.standerPirce,
                                                                        settlementCharge = GroupPriceinfo.settlementPirce,
                                                                        charge = GroupPriceinfo.chargePice,
                                                                        dstate = false
                                                                    };
                                                                    GroupBillInfoPairs.Add(GroupBillInfoPair);

                                                                    //移除遍历过的组合项目信息
                                                                    //TestGroupCodes.Remove(TestGroupFlowCode);
                                                                    GroupInfos.Remove(GroupItem);
                                                                }

                                                                //记录条码信息
                                                                CheckGroupBarcode testInfoBarcode1 = new CheckGroupBarcode();
                                                                testInfoBarcode1.sampleID = sampleInfo.perid;
                                                                testInfoBarcode1.groupNO = groupNO;
                                                                testInfoBarcode1.patientName = perinfo.patientName != null ? perinfo.patientName.ToString() : "";
                                                                testInfoBarcode1.barcode = sampleInfo.barcode;
                                                                testInfoBarcode1.groupCodes = groupcodes;
                                                                testInfoBarcode1.groupNames = groupNames;
                                                                checkBarcodes.Add(testInfoBarcode1);

                                                                //存放同一专业组同一流程下的子项项目编号信息
                                                                List<string> TestItemCodes = testItemCodeString.Split(",").ToList();

                                                                //List<comm_item_test> TestInfoss =await _itemTestRepository.GetCaChe();
                                                                //查询样本信息中包含的子项信息 并按照专业组进行排序
                                                                List<comm_item_test> TestInfos = TestInfoss.Where(p => TestItemCodes.Contains(p.no.ToString()) && p.dstate == false).OrderBy(p => p.sort).ToList();

                                                                //查询样本信息中包含的流程信息 并按照专业组进行排序
                                                                comm_item_flow FlowInfos = FlowInfoss.First(p => p.no == Convert.ToInt32(comm_Item_Group.groupFlowNO) && p.dstate == false);
                                                                //获取流程中存放结果数据表名
                                                                string srouceTableName = FlowInfos.dataSource != null ? FlowInfos.dataSource.ToString() : "";
                                                                ////获取流程中存放图片数据表名
                                                                //string imgTableName = FlowInfos.imgSource != null ? FlowInfos.imgSource.ToString() : "";

                                                                //判断组合项目是否在委托组
                                                                if (groupNO != "1")
                                                                {
                                                                    //项目排序计数器
                                                                    int ItemSort = 0;
                                                                    string itemcodes = "";
                                                                    string itemNames = "";
                                                                    string delitemcodes = "";
                                                                    string delitemNames = "";

                                                                    //判断是否存在委托项目
                                                                    bool delItemState = false;
                                                                    //项目委托医院编号
                                                                    string delItemClientNO = "";
                                                                    foreach (comm_item_test testiteminfo in TestInfos)
                                                                    {
                                                                        ItemSort += 1;
                                                                        itemcodes += testiteminfo.no + ",";
                                                                        itemNames += testiteminfo.names + ",";
                                                                        //comm_item_test itemInfo = TestInfos.First(p => p.no == Convert.ToInt32(itemCode));
                                                                        delItemState = testiteminfo.delegeteState != null ? Convert.ToBoolean(testiteminfo.delegeteState) : false;
                                                                        bool resultNullState = testiteminfo.resultNullState != null ? Convert.ToBoolean(testiteminfo.resultNullState) : false;
                                                                        bool reportState = testiteminfo.visibleState != null ? Convert.ToBoolean(testiteminfo.visibleState) : false;
                                                                        delItemClientNO = delItemState && testiteminfo.delegeteCompanyNO != null ? testiteminfo.delegeteCompanyNO.ToString() : "";

                                                                        Dictionary<string, object> ItemResultInfoPair = new Dictionary<string, object>()
                                                                {
                                                                    {"perid", perid},
                                                                    //{ "testid", @testid },
                                                                    {"barcode", sampleInfo.barcode},
                                                                    {"groupNO", groupNO},
                                                                    {"groupCode", groupcodes},
                                                                    {"groupName", groupNames},
                                                                    {"itemCodes", testiteminfo.no},
                                                                    {"itemNames", testiteminfo.names},
                                                                    {"itemSort", ItemSort},
                                                                    {"creater", infos.UserName},
                                                                    {"createTime", DateTime.Now},
                                                                    {"state", 1},
                                                                    {"dstate", 0},
                                                                    {"resultNullState", resultNullState},
                                                                    {"reportState", reportState},
                                                                    {"delstate", delItemState }
                                                                };
                                                                        if (delItemState)
                                                                        {
                                                                            delitemcodes += testiteminfo.no + ",";
                                                                            delitemNames += testiteminfo.names + ",";
                                                                            ItemResultInfoPair.Add("delstateClientNO", "");
                                                                        }
                                                                        ItemResultInfoPairs.Add(ItemResultInfoPair);
                                                                    }
                                                                    if (delItemState)
                                                                    {
                                                                        DelegeteRecord DelegeteRecordPair = new DelegeteRecord()
                                                                        {
                                                                            //{ "testid", @testid },
                                                                            perid = perid,
                                                                            barcode = sampleInfo.barcode,
                                                                            delegateStateNO = "2",
                                                                            delegateClientNO = delItemClientNO,
                                                                            reason = "默认委托",
                                                                            itemCodes = itemcodes,
                                                                            itemNames = itemNames,
                                                                            checker = infos.UserName,
                                                                            checkTime = DateTime.Now,
                                                                            createTime = DateTime.Now,
                                                                            dstate = false
                                                                        };
                                                                        DelegeteRecordPairs.Add(DelegeteRecordPair);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //项目排序计数器
                                                                    int ItemSort = 0;
                                                                    string delitemcodes = "";
                                                                    string delitemNames = "";


                                                                    foreach (comm_item_test testiteminfo in TestInfos)
                                                                    {
                                                                        ItemSort += 1;
                                                                        delitemcodes += testiteminfo.no + ",";
                                                                        delitemNames += testiteminfo.names + ",";

                                                                        bool delItemState = testiteminfo.delegeteState != null ? Convert.ToBoolean(testiteminfo.delegeteState) : false;
                                                                        bool resultNullState = testiteminfo.resultNullState != null ? Convert.ToBoolean(testiteminfo.resultNullState) : false;
                                                                        bool reportState = testiteminfo.visibleState != null ? Convert.ToBoolean(testiteminfo.visibleState) : false;
                                                                        if (delItemState)
                                                                        {
                                                                            delGroupState = true;
                                                                        }
                                                                        string delItemClientNO = delItemState && testiteminfo.delegeteCompanyNO != null ? testiteminfo.delegeteCompanyNO.ToString() : "";
                                                                        Dictionary<string, object> ItemResultInfoPair = new Dictionary<string, object>()
                                                                {
                                                                    {"perid", perid},
                                                                    //{ "testid", @testid },
                                                                    {"barcode", sampleInfo.barcode},
                                                                    {"groupNO", groupNO},
                                                                    {"groupCode", groupcodes},
                                                                    {"groupName", groupNames},
                                                                    {"itemCodes", testiteminfo.no},
                                                                    {"itemNames", testiteminfo.names},
                                                                    {"itemSort", ItemSort},
                                                                    {"creater", infos.UserName},
                                                                    {"createTime", DateTime.Now},
                                                                    {"state", 1},
                                                                    {"dstate", 0},
                                                                    {"resultNullState", resultNullState},
                                                                    {"reportState", reportState},
                                                                    {"delstate", delItemState },
                                                                    {"delstateClientNO", delItemClientNO }
                                                                };
                                                                        ItemResultInfoPairs.Add(ItemResultInfoPair);

                                                                        DelegeteRecord DelegeteRecordPair = new DelegeteRecord()
                                                                        {
                                                                            //{ "testid", @testid },
                                                                            perid = perid,
                                                                            barcode = sampleInfo.barcode,
                                                                            delegateStateNO = "2",
                                                                            delegateClientNO = delItemClientNO,
                                                                            reason = "默认委托",
                                                                            itemCodes = testiteminfo.no.ToString(),
                                                                            itemNames = testiteminfo.names,
                                                                            checker = infos.UserName,
                                                                            checkTime = DateTime.Now,
                                                                            createTime = DateTime.Now,
                                                                            dstate = false
                                                                        };
                                                                        DelegeteRecordPairs.Add(DelegeteRecordPair);

                                                                    }
                                                                }

                                                                int testid = await _testSampleInfoRepository.InsertTestSampleInfo(perinfo, strGuidN, receiveTime, groupcodes, groupNames, comm_Item_Group.groupNO, comm_Item_Group.groupFlowNO, delGroupState, delegeteCompanyNO, infos.UserName);
                                                                _testItemInfoRepository.InsertTestItemInfo(testid, srouceTableName, ItemResultInfoPairs);
                                                                _groupBillInfoRepository.InsertGroupBill(testid, GroupBillInfoPairs);
                                                                if (DelegeteRecordPairs.Count > 0)
                                                                    _delegeteRecordRepository.InsertDelegeteRecord(testid, DelegeteRecordPairs);
                                                            }
                                                            ApplyBillInfo pairs = new ApplyBillInfo()
                                                            {
                                                                discount = financeClient.discount.ToString(),
                                                                operatTime = DateTime.Now,
                                                                charge = chargeCount,
                                                                settlementCharge = settlementChargeCount,
                                                                standerCharge = standerchargeCount,
                                                                operater = infos.UserName,
                                                                barcode = sampleInfo.barcode,
                                                                chargeLevel = financeClient.chargeLevelNO,
                                                                chargeTypeNO = "1",
                                                                perid = perid.ToString(),
                                                            };
                                                            int aa = await _applyBillInfoRepository.InsertApplyBill(pairs);

                                                            if (aa > 0)
                                                            {
                                                                var testid = await _perSampleInfoRepository.PerCheckInfo(perid, receiveTime, infos.UserName, blendConut);
                                                                reCheckeModel.code = 0;
                                                                reCheckeModel.barcode = sampleInfo.barcode;
                                                                reCheckeModel.groupcodeInfo = checkBarcodes;
                                                                reCheckeModel.msg = $"审核成功！";
                                                                _recordRepository.SampleRecord(sampleInfo.barcode, RecordEnumVars.Check, "审核成功！", infos.UserName, true);


                                                            }
                                                            else
                                                            {
                                                                reCheckeModel.code = 1;
                                                                reCheckeModel.barcode = sampleInfo.barcode;
                                                                reCheckeModel.groupcodeInfo = checkBarcodes;
                                                                reCheckeModel.msg = $"审核失败！";
                                                                //_recordRepository.SampleRecord(sampleInfo.barcode, RecordEnumVars.Check, "审核成功！", infos.UserName, true);
                                                            }


                                                        }
                                                        else
                                                        {
                                                            reCheckeModel.code = 1;
                                                            reCheckeModel.barcode = sampleInfo.barcode;
                                                            reCheckeModel.msg = "未找到组套绑定的组合项目信息";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            reCheckeModels.Add(reCheckeModel);
                        }
                        //_UnitOfWork.CommitTran();
                        commReInfo.infos = reCheckeModels;
                        jm.data = commReInfo;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        //_UnitOfWork.RollbackTran();
                    }

                }
                else
                {
                    jm.code = 1;
                    jm.msg = $"没有找到审核信息";
                }
            }
            return jm;
        }


        #region  旧方法
        ///// <summary>
        ///// 审核信息（不用交接审核）
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //public async Task<WebApiCallBack> CheckInfoXG(CheckInfoModel infos)
        //{

        //    WebApiCallBack jm = new WebApiCallBack();
        //    //using (await _mutex.LockAsync())
        //    //{
        //    if (infos.checkSampleInfos != null)
        //    {
        //        commReInfo<ReCheckeModel> commReInfo = new commReInfo<ReCheckeModel>();
        //        List<ReCheckeModel> reCheckeModels = new List<ReCheckeModel>();
        //        foreach (CheckSampleInfoModel sampleInfo in infos.checkSampleInfos)
        //        {
        //            //返回条码处理状态
        //            ReCheckeModel reCheckeModel = new ReCheckeModel();
        //            reCheckeModel.code = 0;
        //            //string sql = "";
        //            if (sampleInfo.barcode != null && sampleInfo.perid != 0)
        //            {

        //                //DataTable DTsampleInfo = null;
        //                //DataTable DTBlendInfo = null;

        //                int blendConut = 1;
        //                //判断是客户端信息还是微信信息
        //                //if (infos.checkType == 1)
        //                //{
        //                //    string sql1 = $"select * from WorkPer.SampleInfo where id='{sampleInfo.perid}' and perStateNO=1 and dstate=0";
        //                //    //sql += $"select * from WorkPer.SampleBlendInfo where barcode='{sampleInfo.barcode}' and dstate=0";
        //                //    string sql2 = $"select 1 from WorkPer.SampleBlendInfo where barcode='{sampleInfo.barcode}' and dstate=0";
        //                //    DTsampleInfo = await _commRepository.GetTable(sql1);

        //                //    //string sqlBlend = $"update WorkPer.SampleBlendInfo set dstate=1 where barcode='{sampleInfo.barcode}';";//删除已有混采信息
        //                //    //DTBlendInfo = dsInfo.Tables[1];
        //                //    blendConut = await _commRepository.sqlcommand(sql2); ;


        //                //}
        //                //else
        //                //{
        //                //    string sql1 = $"select * from WorkPer.SampleInfo where id='{sampleInfo.perid}' and perStateNO=1 and dstate=0";
        //                //    //sql += $"select * from WorkPer.SampleBlendInfo where barcode='{sampleInfo.barcode}' and dstate=0";
        //                //    string sql2 = $"select 1 from WorkPer.SampleBlendInfo where barcode='{sampleInfo.barcode}' and dstate=0";
        //                //    DTsampleInfo = await _commRepository.GetTable(sql1);
        //                //}

        //                var perinfo = await _perSampleInfoRepository.QueryByIdAsync(sampleInfo.perid);

        //                //判断录入信息是否存在
        //                if (perinfo !=null)
        //                {



        //                    //string agentNO = "";
        //                    //string hospitalNO = "";
        //                    //string patientType = "";
        //                    //string department = "";
        //                    //DateTime receiveTime = "";
        //                    //DateTime sampleTime = "";

        //                    string agentNO = perinfo.agentNO != null ? perinfo.agentNO.ToString() : "";
        //                    string hospitalNO = perinfo.hospitalNO != null ? perinfo.hospitalNO.ToString() : "";
        //                    string patientType = perinfo.patientTypeNO != null ? perinfo.patientTypeNO.ToString() : "";
        //                    string department = perinfo.department != null ? perinfo.department.ToString() : "";
        //                    DateTime sampleTime = perinfo.sampleTime != null ? Convert.ToDateTime(perinfo.sampleTime) : DateTime.MinValue;
        //                    DateTime receiveTime = perinfo.receiveTime != null ? Convert.ToDateTime(perinfo.receiveTime) : DateTime.Now;
        //                    if (Convert.ToDateTime(sampleTime) <= Convert.ToDateTime(receiveTime))
        //                    {
        //                        if (hospitalNO != "")
        //                        {

        //                            FinanceClientModel financeClient = await _financeInfoRepository.GetFinanceClient(hospitalNO);//获取医院收费信息


        //                            int sampleID = perinfo.id;

        //                            //int lsh = 1;
        //                            if (perinfo.perStateNO !=null)
        //                            {


        //                                if (perinfo.perStateNO.ToString() == "1")
        //                                {
        //                                    if (perinfo.applyItemCodes !=null)
        //                                    {


        //                                        List<string> applyItemCodes = perinfo.applyItemCodes.ToString().Split(",").ToList();


        //                                        //读取内存中的组套信息
        //                                        List<comm_item_apply> ApplyInfoss = ManualDataCache<comm_item_apply>.MemoryCache.LIMSGetKeyValue(CommInfo.itemapply);
        //                                        //List<comm_item_apply> ApplyInfoss = await _itemApplyRepository.GetCaChe();
        //                                        //查询样本信息中包含的组套信息
        //                                        List<comm_item_apply> ApplyInfos = ApplyInfoss.Where(p => applyItemCodes.Contains(p.no.ToString()) && p.dstate == false).ToList();


        //                                        if (ApplyInfos.Count > 0)
        //                                        {
        //                                            decimal standerchargeCount = 0;//标准收费总金额
        //                                            decimal settlementChargeCount = 0;//结算收费总金额
        //                                            decimal chargeCount = 0;//实际收费总金额

        //                                            string sqlAll = "";



        //                                            //条码集合对象
        //                                            List<CheckGroupBarcode> checkBarcodes = new List<CheckGroupBarcode>();
        //                                            //存放所有组合项目编号
        //                                            string groupItemCodeString = "";
        //                                            foreach (comm_item_apply comm_Item_Apply in ApplyInfos)
        //                                            {
        //                                                groupItemCodeString += comm_Item_Apply.groupItemList;
        //                                            }
        //                                            //存放所有组合项目编号信息
        //                                            List<string> groupItemCodes = groupItemCodeString.Split(",").ToList();

        //                                            //读取内存中的组和信息
        //                                            List<comm_item_group> GroupInfoss = ManualDataCache<comm_item_group>.MemoryCache.LIMSGetKeyValue(CommInfo.itemgroup);
        //                                            //List<comm_item_group> GroupInfoss = await _itemGroupRepository.GetCaChe();
        //                                            //查询样本信息中包含的组和信息

        //                                            List<comm_item_group> GroupInfos = GroupInfoss.Where(p => groupItemCodes.Contains(p.no.ToString()) && p.dstate == false).ToList();

        //                                            if (GroupInfos.Count > 0)
        //                                            {
        //                                                while (GroupInfos.Count > 0)
        //                                                {
        //                                                    //foreach (comm_item_group comm_Item_Group in GroupInfos)
        //                                                    //{
        //                                                    comm_item_group comm_Item_Group = GroupInfos.First();



        //                                                    ///获取同一专业组的组合项目信息
        //                                                    List<comm_item_group> TestGroupCodes = GroupInfos.Where(p => p.groupNO == comm_Item_Group.groupNO && p.dstate == false).ToList();


        //                                                    //foreach (comm_item_group testgroupinfo in TestGroupCodes)
        //                                                    //{
        //                                                    ///获取同一专业组同一流程的项目的组合项目信息
        //                                                    List<comm_item_group> TestGroupFlowCodes = TestGroupCodes.Where(p => p.groupFlowNO == comm_Item_Group.groupFlowNO && p.dstate == false).ToList();

        //                                                    //唯一标识
        //                                                    string strGuidN = Guid.NewGuid().ToString("N");


        //                                                    //存放组合项目的编号信息
        //                                                    string groupcodes = "";
        //                                                    //存放组合项目的名称信息
        //                                                    string groupNames = "";

        //                                                    //存放同一专业组同一流程的所有子项项目编号
        //                                                    string testItemCodeString = "";

        //                                                    //插入到检验中的样本信息
        //                                                    string testinfoSql = "";
        //                                                    //结果插入信息
        //                                                    string ItemResultInfoSql = "";
        //                                                    //委托插入信息
        //                                                    string DelegeteRecordSql = "";
        //                                                    //收费插入信息
        //                                                    string FinanceGroupSql = "";

        //                                                    bool delGroupState = false;


        //                                                    string grouoNo = "";
        //                                                    string delegeteCompanyNO = "";
        //                                                    foreach (comm_item_group TestGroupFlowCode in TestGroupFlowCodes)
        //                                                    {
        //                                                        groupcodes += TestGroupFlowCode.no + ",";
        //                                                        groupNames += TestGroupFlowCode.names + ",";
        //                                                        testItemCodeString += TestGroupFlowCode.testItemList;

        //                                                        grouoNo = TestGroupFlowCode.groupNO;


        //                                                        delegeteCompanyNO = TestGroupFlowCode.delegeteCompanyNO;


        //                                                        //获取组合项目折扣信息
        //                                                        DiscountPriceModel GroupPriceinfo = await _financeInfoRepository.GetGroupPrice(agentNO, hospitalNO, patientType, department, TestGroupFlowCode.no.ToString(), financeClient.chargeLevelNO, financeClient.discount);
        //                                                        //记录项目收费价格
        //                                                        standerchargeCount += GroupPriceinfo.standerPirce;//标准收费总金额
        //                                                        settlementChargeCount += GroupPriceinfo.settlementPirce;//结算收费总金额
        //                                                        chargeCount += GroupPriceinfo.chargePice;//实际收费总金额

        //                                                        //生成组合项目收费信息
        //                                                        FinanceGroupSql = "insert into Finance.GroupBillInfo (perid,testid,barcode,tabTypeNO,groupNO,chargeTypeNO,chargeLevelNO,discountState,discount,personNO,groupCode,groupName,operater,operatTime,standerCharge,settlementCharge,charge,dstate)";
        //                                                        FinanceGroupSql += $" values ('{sampleInfo.perid}',@testid,'{sampleInfo.barcode}','1','{TestGroupFlowCode.groupNO}','{GroupPriceinfo.chargeTypeNO}','{financeClient.chargeLevelNO}','{GroupPriceinfo.groupDiscountState}','{financeClient.discount}','{financeClient.personNO}','{TestGroupFlowCode.no}','{TestGroupFlowCode.names}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{GroupPriceinfo.standerPirce}','{GroupPriceinfo.settlementPirce}','{GroupPriceinfo.chargePice}',0);";



        //                                                        //移除遍历过的组合项目信息
        //                                                        //TestGroupCodes.Remove(TestGroupFlowCode);
        //                                                        GroupInfos.Remove(TestGroupFlowCode);
        //                                                    }


        //                                                    //记录条码信息
        //                                                    CheckGroupBarcode testInfoBarcode1 = new CheckGroupBarcode();
        //                                                    testInfoBarcode1.sampleID = sampleInfo.perid;
        //                                                    testInfoBarcode1.groupNO = grouoNo;
        //                                                    testInfoBarcode1.patientName = perinfo.patientName!= null ? perinfo.patientName : "";
        //                                                    testInfoBarcode1.barcode = sampleInfo.barcode;
        //                                                    testInfoBarcode1.groupCodes = groupcodes;
        //                                                    testInfoBarcode1.groupNames = groupNames;
        //                                                    checkBarcodes.Add(testInfoBarcode1);





        //                                                    //存放同一专业组同一流程下的子项项目编号信息
        //                                                    List<string> TestItemCodes = testItemCodeString.Split(",").ToList();


        //                                                    //读取子项信息获取子项集合
        //                                                    //List<comm_item_test> TestInfoss = await _itemTestRepository.GetCaChe();
        //                                                    List<comm_item_test> TestInfoss = ManualDataCache<comm_item_test>.MemoryCache.LIMSGetKeyValue(CommInfo.itemtest);
        //                                                    //查询样本信息中包含的子项信息 并按照专业组进行排序
        //                                                    List<comm_item_test> TestInfos = TestInfoss.Where(p => TestItemCodes.Contains(p.no.ToString()) && p.dstate == false).OrderBy(p => p.sort).ToList();


        //                                                    //读取子项信息获取流程集合
        //                                                    //List<comm_item_flow> FlowInfoss = await _itemFlowRepository.GetCaChe();
        //                                                    List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
        //                                                    //查询样本信息中包含的流程信息 并按照专业组进行排序
        //                                                    comm_item_flow FlowInfos = FlowInfoss.First(p => p.no == Convert.ToInt32(comm_Item_Group.groupFlowNO) && p.dstate == false);
        //                                                    //获取流程中存放结果数据表名
        //                                                    string srouceTableName = FlowInfos.dataSource != null ? FlowInfos.dataSource.ToString() : "";
        //                                                    //获取流程中存放图片数据表名
        //                                                    string imgTableName = FlowInfos.imgSource != null ? FlowInfos.imgSource.ToString() : "";

        //                                                    //判断组合项目是否在委托组
        //                                                    if (grouoNo != "1")
        //                                                    {
        //                                                        //项目排序计数器
        //                                                        int ItemSort = 0;
        //                                                        string itemcodes = "";
        //                                                        string itemNames = "";
        //                                                        string delitemcodes = "";
        //                                                        string delitemNames = "";

        //                                                        //判断是否存在委托项目
        //                                                        bool delItemState = false;
        //                                                        //项目委托医院编号
        //                                                        string delItemClientNO = "";
        //                                                        foreach (comm_item_test testiteminfo in TestInfos)
        //                                                        {
        //                                                            ItemSort += 1;
        //                                                            itemcodes += testiteminfo.no + ",";
        //                                                            itemNames += testiteminfo.names + ",";
        //                                                            //comm_item_test itemInfo = TestInfos.First(p => p.no == Convert.ToInt32(itemCode));
        //                                                            delItemState = testiteminfo.delegeteState != null ? Convert.ToBoolean(testiteminfo.delegeteState) : false;
        //                                                            bool resultNullState = testiteminfo.resultNullState != null ? Convert.ToBoolean(testiteminfo.resultNullState) : false;
        //                                                            bool reportState = testiteminfo.visibleState != null ? Convert.ToBoolean(testiteminfo.visibleState) : false;
        //                                                            delItemClientNO = delItemState && testiteminfo.delegeteCompanyNO != null ? testiteminfo.delegeteCompanyNO.ToString() : "";
        //                                                            if (delItemState)
        //                                                            {
        //                                                                delitemcodes += testiteminfo.no + ",";
        //                                                                delitemNames += testiteminfo.names + ",";
        //                                                                ItemResultInfoSql += $"insert into {srouceTableName} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate,resultNullState,reportState)";
        //                                                                ItemResultInfoSql += $"values ({sampleInfo.perid},@testid,'{sampleInfo.barcode}','{grouoNo}','{groupcodes}','{groupNames}','{testiteminfo.no}','{testiteminfo.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0,'{resultNullState}','{reportState}');";
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                ItemResultInfoSql += $"insert into {srouceTableName} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate,resultNullState,reportState)";
        //                                                                ItemResultInfoSql += $"values ({sampleInfo.perid},@testid,'{sampleInfo.barcode}','{grouoNo}','{groupcodes}','{groupNames}','{testiteminfo.no}','{testiteminfo.names}','{ItemSort}','0','','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0,'{resultNullState}','{reportState}');";
        //                                                            }
        //                                                            //if (comm_Item_Group.groupFlowNO != "2")
        //                                                            //{
        //                                                            //    testinfoSql = InsertTestSampleInfo(DTsampleInfo.Rows[0], strGuidN, receiveTime, groupcodes, groupNames, "", comm_Item_Group.groupNO, comm_Item_Group.groupFlowNO, delGroupState, delegeteCompanyNO, infos.UserName);
        //                                                            //    sqlAll += testinfoSql + "set @testid=@@IDENTITY;" + ItemResultInfoSql + FinanceGroupSql;
        //                                                            //}

        //                                                        }
        //                                                        if (delItemState)
        //                                                        {

        //                                                            DelegeteRecordSql += "insert into WorkOther.DelegeteRecord (perid,testid,barcode,delegateStateNO,delegateClientNO,reason,itemCodes,itemNames,checker,checkTime,createTime.dstate)";
        //                                                            DelegeteRecordSql += $"values ('{sampleInfo.perid}',@testid,'{sampleInfo.barcode}','2',{delItemClientNO},'默认委托','{itemcodes}','{itemNames}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',0);";
        //                                                        }
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        //项目排序计数器
        //                                                        int ItemSort = 0;
        //                                                        string delitemcodes = "";
        //                                                        string delitemNames = "";


        //                                                        foreach (comm_item_test testiteminfo in TestInfos)
        //                                                        {
        //                                                            ItemSort += 1;
        //                                                            delitemcodes += testiteminfo.no + ",";
        //                                                            delitemNames += testiteminfo.names + ",";

        //                                                            bool delItemState = testiteminfo.delegeteState != null ? Convert.ToBoolean(testiteminfo.delegeteState) : false;
        //                                                            bool resultNullState = testiteminfo.resultNullState != null ? Convert.ToBoolean(testiteminfo.resultNullState) : false;
        //                                                            bool reportState = testiteminfo.visibleState != null ? Convert.ToBoolean(testiteminfo.visibleState) : false;
        //                                                            if (delItemState)
        //                                                            {
        //                                                                delGroupState = true;
        //                                                            }
        //                                                            string delItemClientNO = delItemState && testiteminfo.delegeteCompanyNO != null ? testiteminfo.delegeteCompanyNO.ToString() : "";
        //                                                            ItemResultInfoSql += $"insert into {srouceTableName} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate,resultNullState,reportState)";
        //                                                            ItemResultInfoSql += $"values ({sampleInfo.perid},@testid,'{sampleInfo.barcode}','{grouoNo}','{groupcodes}','{groupNames}','{testiteminfo.no}','{testiteminfo.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0,'{resultNullState}','{reportState}');";

        //                                                            DelegeteRecordSql += "insert into WorkOther.DelegeteRecord (perid,testid,barcode,delegateStateNO,delegateClientNO,reason,itemCodes,itemNames,checker,checkTime,createTime,dstate)";
        //                                                            DelegeteRecordSql += $"values ('{sampleInfo.perid}',@testid,'{sampleInfo.barcode}','2',{delItemClientNO},'默认委托','{testiteminfo.no}','{testiteminfo.names}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',0);";

        //                                                            //if (comm_Item_Group.groupFlowNO != "2")
        //                                                            //{
        //                                                            //    testinfoSql = InsertTestSampleInfo(DTsampleInfo.Rows[0], strGuidN, receiveTime, groupcodes, groupNames, "", comm_Item_Group.groupNO, comm_Item_Group.groupFlowNO, delGroupState, delegeteCompanyNO, infos.UserName);
        //                                                            //    sqlAll += testinfoSql + "set @testid=@@IDENTITY;" + ItemResultInfoSql + FinanceGroupSql;
        //                                                            //}
        //                                                        }


        //                                                    }
        //                                                    //if (comm_Item_Group.groupFlowNO == "2")
        //                                                    //{
        //                                                    testinfoSql = InsertTestSampleInfo(perinfo, strGuidN, receiveTime, groupcodes, groupNames, "", comm_Item_Group.groupNO, comm_Item_Group.groupFlowNO, delGroupState, delegeteCompanyNO, infos.UserName, false);
        //                                                    sqlAll += testinfoSql + "set @testid=@@IDENTITY;" + ItemResultInfoSql + FinanceGroupSql + DelegeteRecordSql;
        //                                                    //}
        //                                                }

        //                                                ///插入收费总和信息
        //                                                if (sqlAll != "")
        //                                                {
        //                                                    iInfo insertInfo = new iInfo();
        //                                                    insertInfo.TableName = "Finance.ApplyBillInfo";
        //                                                    Dictionary<string, object> pairs = new Dictionary<string, object>();
        //                                                    pairs.Add("discount", financeClient.discount);
        //                                                    pairs.Add("operatTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //                                                    pairs.Add("charge", chargeCount);
        //                                                    pairs.Add("settlementCharge", settlementChargeCount);
        //                                                    pairs.Add("standerCharge", standerchargeCount);
        //                                                    pairs.Add("operater", infos.UserName);
        //                                                    pairs.Add("barcode", sampleInfo.barcode);
        //                                                    pairs.Add("chargeLevel", financeClient.chargeLevelNO);
        //                                                    pairs.Add("chargeTypeNO", "1");
        //                                                    pairs.Add("perid", sampleID);

        //                                                    insertInfo.values = pairs;
        //                                                    string insertApplyPirceInfo = SqlFormartHelper.insertFormart(insertInfo);

        //                                                    //insertGroupInfos += InsertTestSampleInfo(DTsampleInfo.Rows[0], grouCodes, groupNames, "", groupNO, groupFlowNO, delGroupState, delGroupClientNO, infos.UserName) + "set @testid=@@IDENTITY;";
        //                                                    sqlAll += insertApplyPirceInfo;
        //                                                }
        //                                                if (reCheckeModel.code == 0)
        //                                                {
        //                                                    //int aaa = HLDBSqlHelper.ExecuteNonQuery( "declare @testid varchar(500);" + insertSampleInfoAll);
        //                                                    await _commRepository.sqlcommand("declare @testid varchar(500);" + sqlAll);
        //                                                    //int aaa = HLDBSqlHelper.ExecuteNonQuery( "declare @testid varchar(500);" + insertSampleInfoAll);
        //                                                    //Result = "审核成功！";
        //                                                    reCheckeModel.code = 0;
        //                                                    reCheckeModel.barcode = sampleInfo.barcode;
        //                                                    reCheckeModel.groupcodeInfo = checkBarcodes;
        //                                                    reCheckeModel.msg = $"审核成功！";

        //                                                    string updateperInfo = "";
        //                                                    if (blendConut == 1)
        //                                                    {
        //                                                        updateperInfo = $"update WorkPer.SampleInfo set receiveTime='{receiveTime}',perStateNO=2,state=1,checker='{infos.UserName}',checkTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where barcode='{sampleInfo.barcode}' and id='{sampleInfo.perid}';";
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        updateperInfo = $"update WorkPer.SampleInfo set  number='{blendConut}',receiveTime='{receiveTime}',perStateNO=2,state=1,checker='{infos.UserName}',checkTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where barcode='{sampleInfo.barcode}' and id='{sampleInfo.perid}';";
        //                                                    }

        //                                                    //updateperInfo += $"update WorkPer.SampleInfo set perStateNO=2,state=1,checker='{infos.UserName}',checkTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where barcode='{sampleInfo.barcode}' and id='{sampleInfo.perid}'";

        //                                                    await _commRepository.sqlcommand(updateperInfo);
        //                                                    await _recordRepository.SampleRecord(sampleInfo.barcode, RecordEnumVars.Check, "审核成功！", infos.UserName, true);


        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                reCheckeModel.code = 1;
        //                                                reCheckeModel.barcode = sampleInfo.barcode;
        //                                                reCheckeModel.msg = "未找到组套绑定的组合项目信息";
        //                                            }

        //                                        }
        //                                        else
        //                                        {
        //                                            reCheckeModel.code = 1;
        //                                            reCheckeModel.barcode = sampleInfo.barcode;
        //                                            reCheckeModel.msg = "未找到样本项目信息";
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        reCheckeModel.code = 1;
        //                                        reCheckeModel.barcode = sampleInfo.barcode;
        //                                        reCheckeModel.msg = "样本信息不能审核";
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    reCheckeModel.code = 1;
        //                                    reCheckeModel.barcode = sampleInfo.barcode;
        //                                    reCheckeModel.msg = "样本信息不能审核";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                reCheckeModel.code = 1;
        //                                reCheckeModel.barcode = sampleInfo.barcode;
        //                                reCheckeModel.msg = "未匹配到送检单位";
        //                            }

        //                        }
        //                        else
        //                        {
        //                            reCheckeModel.code = 1;
        //                            reCheckeModel.barcode = sampleInfo.barcode;
        //                            reCheckeModel.msg = "未找到匹配的客户信息";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        reCheckeModel.code = 1;
        //                        reCheckeModel.barcode = sampleInfo.barcode;
        //                        reCheckeModel.msg = "采样时间不能大于接收时间";
        //                    }
        //                }
        //                else
        //                {
        //                    reCheckeModel.code = 1;
        //                    reCheckeModel.barcode = sampleInfo.barcode;
        //                    reCheckeModel.msg = $"没有找到审核信息/样本已审核";
        //                }
        //            }
        //            else
        //            {
        //                reCheckeModel.code = 1;

        //                reCheckeModel.barcode = sampleInfo.barcode;

        //                reCheckeModel.msg = $"提交的样本信息未包含条码信息";
        //            }
        //            reCheckeModels.Add(reCheckeModel);
        //        }
        //        commReInfo.infos = reCheckeModels;
        //        jm.data = commReInfo;
        //    }
        //    else
        //    {
        //        jm.code = 1;
        //        jm.msg = $"没有找到审核信息";
        //    }
        //    //}
        //    return jm;
        //}

        #endregion
        /// <summary>
        /// 反审核信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CheckRe(CheckInfoModel infos)
        {

            WebApiCallBack jm = new WebApiCallBack();
            //using (await _mutex.LockAsync())
            //{
            try
            {


                string SqltestInfo = $"select * from INFORMATION_SCHEMA.TABLES    where TABLE_SCHEMA='WorkTest'";
                DataTable testTableDT = await _commRepository.GetTable(SqltestInfo);
                if (testTableDT.Rows.Count > 0)
                {
                    commReInfo<commReSampleInfo> commReInfo = new commReInfo<commReSampleInfo>();
                    List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                    foreach (CheckSampleInfoModel sampleInfo in infos.checkSampleInfos)
                    {

                        commReSampleInfo reSampleInfo = new commReSampleInfo();

                        string SqlperInfo = $"select id,perStateNO from WorkPer.SampleInfo   where barcode='{sampleInfo.barcode}' and dstate=0";
                        //if (infos.checkType == 0)
                        DataTable perInfoDT = await _commRepository.GetTable(SqlperInfo);
                        //if (infos.checkType == 1)
                        //    perInfoDT = SqlHelper.ExecuteDataset(CommonData.sqlconnW, CommandType.Text, SqlperInfo).Tables[0];
                        if (perInfoDT.Rows.Count > 0)
                        {

                            string perid = perInfoDT.Rows[0]["id"] != null ? perInfoDT.Rows[0]["id"].ToString() : "0";

                            ///读取当前样本状态

                            string perState = perInfoDT.Rows[0]["perStateNO"] != null ? perInfoDT.Rows[0]["perStateNO"].ToString() : "0";


                            if (perState == "2")
                            {
                                string sql = "";
                                foreach (DataRow row in testTableDT.Rows)
                                {
                                    if (!row["TABLE_NAME"].ToString().Contains("View"))
                                        sql += "update WorkTest." + row["TABLE_NAME"] + $" set dstate=1 where barcode='{sampleInfo.barcode}';";

                                }
                                sql += "update Finance.GroupBillInfo" + $" set dstate=1 where barcode='{sampleInfo.barcode}';";
                                sql += "update Finance.ApplyBillInfo" + $" set dstate=1 where barcode='{sampleInfo.barcode}';";

                                int ssss = 0;
                                if (infos.checkType == 1)
                                {
                                    string updateperInfo = $"update WorkPer.SampleInfo set dstate=1,perStateNO=1,checker='',checkTime=null where barcode='{sampleInfo.barcode}';";
                                    updateperInfo += $"update WorkPer.SampleBlendInfo set dstate=1 where barcode='{sampleInfo.barcode}' and dstate=0;";
                                    ssss = await _commRepository.sqlcommand(updateperInfo + sql);
                                }
                                else
                                {
                                    string updateperInfo = $"update WorkPer.SampleInfo set perStateNO=1,checker='',checkTime=null where barcode='{sampleInfo.barcode}';";
                                    ssss = await _commRepository.sqlcommand(updateperInfo + sql);
                                    await _recordRepository.SampleRecord(sampleInfo.barcode, RecordEnumVars.ReCheck, $"信息反审核成功！", infos.UserName, true);
                                }
                                if (ssss != 0)
                                {

                                    reSampleInfo.code = 0;
                                    reSampleInfo.barcode = sampleInfo.barcode;
                                    reSampleInfo.msg = "反审核成功";
                                }
                                else
                                {
                                    reSampleInfo.code = 1;
                                    reSampleInfo.barcode = sampleInfo.barcode;
                                    reSampleInfo.msg = "反审核失败";

                                }
                            }
                            else
                            {

                                reSampleInfo.code = 1;
                                reSampleInfo.barcode = sampleInfo.barcode;
                                reSampleInfo.msg = "当前样本状态不能反审核";

                            }
                        }
                        else
                        {
                            reSampleInfo.code = 1;
                            reSampleInfo.barcode = sampleInfo.barcode;
                            reSampleInfo.msg = "未找到样本信息";

                        }
                        reSampleInfos.Add(reSampleInfo);
                    }
                    commReInfo.infos = reSampleInfos;
                    jm.code = 0;
                    jm.data = commReInfo;
                }
                else
                {
                    jm.code = 1;
                    jm.msg = "请检查数据库是否正常";


                }
            }
            catch (Exception ex)
            {
                jm.code = 1;
                jm.msg = ex.Message;
            }

            //}

            return jm;

        }


        /// <summary>
        /// 接收信息查询
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> GetReceiveInfo(ReceiveSelectModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack() { code = 1, msg = "获取接收信息失败" };
            string sql = "";
            var wheres = PredicateBuilder.True<test_sampleInfo>();
            wheres = wheres.And(p => p.visible == true);
            wheres = wheres.And(p => p.state == false);
            wheres = wheres.And(p => p.dstate == false);
            if (AppSettingsConstVars.SampleSort)
            {
                wheres = wheres.And(p => p.sortState == true);
                //jm.msg = "查询错误";
                //return jm;
            }

            if (infos.groupNO == null)
            { 
                jm.msg = "查询错误";
                return jm;
            }
            wheres = wheres.And(p => p.groupNO == infos.groupNO);
            if (infos.barcode != null)
            {
                wheres = wheres.And(p => p.barcode == infos.barcode);
            }
            else
            {
                if (infos.hosbarcode != null)
                {
                    wheres = wheres.And(p => p.hospitalBarcode == infos.hosbarcode);
                }
                else
                {
                    if (infos.startTime != null && infos.endTime != null)
                        wheres = wheres.And(p => SqlFunc.Between(p.createTime, Convert.ToDateTime(infos.startTime), Convert.ToDateTime(infos.endTime)));
                }
            }
            var sortDT = await _testSampleInfoRepository.QueryDTByClauseAsync(wheres, true);
            if (sortDT.Rows.Count > 0)
            {
                jm.code = 0;
                jm.msg = "查询接受信息成";
                jm.data = DataTableHelper.DTToString(sortDT);
            }
            else
            {
                jm.code = 1;
                jm.msg = "未查询到接收信息";
            }
            return jm;
        }

        /// <summary>
        /// 检验接收信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> ReceiveInfo(ReceiveInfoModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            if (infos.ReceiveInfos != null)
            {
                if (infos.ReceiveTime != null)
                {
                    if (infos.ReceiveInfos.Count > 0)
                    {
                        commReInfo<commReSampleInfo> commReInfo = new commReInfo<commReSampleInfo>();
                        List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                        ReceiveSampleInfoModel receiveInfo = infos.ReceiveInfos[0];
                        commReSampleInfo reSampleInfo = new commReSampleInfo();
                        test_sampleInfo testSampleInfo = await _testRepository.GetTestInfo(receiveInfo.testid);
                        if (testSampleInfo != null)
                        {

                            #region 判断项目信息参考值
                            await _itemHandleRepository.ItemReferenceHandle(testSampleInfo);//判断项目信息参考值
                            #endregion
                            var testState = await _testSampleInfoRepository.UpdateAsync(p => new test_sampleInfo { reachTime = Convert.ToDateTime(infos.ReceiveTime), testNo = receiveInfo.testNo, frameNo = receiveInfo.frameNo, testReceive = _httpContextUser.Name, testReceiveTime = DateTime.Now, state = true }, p => p.id==receiveInfo.testid);

                            if(testState)
                            {
                                var perstate = await _perSampleInfoRepository.UpdateAsync(p => new per_sampleInfo { perStateNO = 3, sortState = true }, p => p.id == receiveInfo.perid);
                                if(perstate)
                                {
                                    _recordRepository.SampleRecord(receiveInfo.barcode, RecordEnumVars.Receive, $"专业组：{receiveInfo.groupName},检验接收成功！", infos.UserName, true);
                                    reSampleInfo.code = 0;
                                    reSampleInfo.barcode = receiveInfo.barcode;
                                    reSampleInfo.testid = receiveInfo.testid;
                                    reSampleInfo.msg = "接收成功";
                                }
                            }
                            reSampleInfos.Add(reSampleInfo);
                        }
                        else
                        {
                            reSampleInfo.code = 1;
                            reSampleInfo.barcode = reSampleInfo.barcode;
                            reSampleInfo.msg = "条码信息不存在";
                        }
                        commReInfo.code = 0;
                        commReInfo.infos = reSampleInfos;
                        jm.data = commReInfo;
                    }
                }
                else
                {
                    jm.code = 1;
                    jm.msg = "接收时间不能为空";
                }
            }
            else
            {
                jm.code = 1;
                jm.msg = "未提交样本信息";
            }
            return jm;
        }


        /// <summary>
        /// 检验反接收（退回）
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> ReceiveRe(ReceiveInfoModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack();

            //using (await _mutex.LockAsync())
            //{
            if (infos.ReceiveInfos == null)
            {
                jm.code = 1;
                jm.msg = "退回失败";
            }
            else
            {
                commReInfo<commReSampleInfo> commReInfo = new commReInfo<commReSampleInfo>();
                List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                //string updateTestInfo = "";
                foreach (ReceiveSampleInfoModel receiveInfo in infos.ReceiveInfos)
                {
                    _UnitOfWork.BeginTran();
                    commReSampleInfo reSampleInfo = new commReSampleInfo();
                    reSampleInfo.barcode = receiveInfo.barcode;
                    //updateTestInfo += $"update WorkTest.SampleInfo set sortState=0,reachTime='',testNo='',frameNo='',testReceive='{infos.UserName}',testReceiveTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',state=0 where sampleID='{receiveInfo.sampleid}' and barcode='{receiveInfo.barcode}' ;";
                    var testState = await _testSampleInfoRepository.UpdateAsync(p => new test_sampleInfo { sortState = false, reachTime = null, testNo = "", testReceive = _httpContextUser.Name, testReceiveTime = DateTime.Now, state = false }, p => p.sampleID == receiveInfo.sampleid && p.barcode == receiveInfo.barcode);
                    if (!testState)
                    {
                        reSampleInfo.code = 1;
                        reSampleInfo.msg = "退回失败";
                    }
                    else
                    {

                        //updateTestInfo += $"update WorkPer.SampleInfo set sortState=0,perStateNO='2' where barcode='{receiveInfo.barcode}' and dstate=0;";
                        var perstate = await _perSampleInfoRepository.UpdateAsync(p => new per_sampleInfo { sortState = false, perStateNO = 2 }, p => p.barcode == receiveInfo.barcode && p.dstate == false);
                        
                        if (!perstate)
                        {
                            _UnitOfWork.RollbackTran();
                            reSampleInfo.code = 1;
                            reSampleInfo.msg = "退回失败";

                        }
                        else
                        {
                            _UnitOfWork.CommitTran();
                            reSampleInfo.code = 0;
                            reSampleInfo.msg = "退回成功";
                            reSampleInfos.Add(reSampleInfo);
                            await _recordRepository.SampleRecord(receiveInfo.barcode, RecordEnumVars.ReReceive, $"{receiveInfo.barcode}检验退回成功！", infos.UserName, false);

                        }
                    }
                }
                commReInfo.infos = reSampleInfos;
                //await _commRepository.sqlcommand(updateTestInfo);
                jm.code = 0;
                jm.data = commReInfo;
                jm.msg = "退回成功";
            }
            return jm;
        }

        /// <summary>
        /// 查询分拣信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> GetSortInfo(SortSelectModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack() { code = 0, status = true };
            var perWheres = PredicateBuilder.True<per_sampleInfo>();
            perWheres = perWheres.And(p => p.perStateNO == 2);
            perWheres = perWheres.And(p => p.sortState == false);
            perWheres = perWheres.And(p => p.state == true);
            perWheres = perWheres.And(p => p.dstate == false);
            if (infos.barcode != null)
            {
                perWheres = perWheres.And(p => p.barcode == infos.barcode);
            }
            else
            {
                if(infos.hosbarcode != null)
                {
                    perWheres = perWheres.And(p => p.hospitalBarcode == infos.hosbarcode);
                }
                else
                {
                    if (infos.startTime != null && infos.endTime != null)
                        perWheres = perWheres.And(p => SqlFunc.Between(p.createTime,infos.startTime,infos.endTime));
                }
            }
            DataTable perDT = await _perSampleInfoRepository.QueryDTByClauseAsync(perWheres,true);

            if (perDT == null)
            {
                jm.code = 1;
                jm.status = false;
                jm.msg = "未查询到分拣信息";
            }
            else
            { 
                List<string> perids = new List<string>();
                foreach(DataRow dataRow in perDT.Rows)
                {
                    perids.Add(dataRow["id"].ToString());
                }
                DataTable testDT=await _testSampleInfoRepository.QueryDTByClauseAsync(p => perids.Contains(p.perid),true);
                jm.data = DataTableHelper.DTToString(perDT); ;
                jm.otherData = DataTableHelper.DTToString(testDT); ;
            }
            return jm;
        }

        /// <summary>
        /// 前处理分拣
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> SortInfo(SortInfoModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            if (infos.sampleSortInfos != null)
            {
                jm.code = 1;
                jm.msg = "未提交样本信息";
            }
            else
            {
                commReInfo<commReSampleInfo> commReInfo = new commReInfo<commReSampleInfo>();
                List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                string updateTestInfo = "";
                foreach (SampleInfoSortModel receiveInfo in infos.sampleSortInfos)
                {
                    commReSampleInfo reSampleInfo = new commReSampleInfo();
                    reSampleInfo.barcode = receiveInfo.barcode;
                    reSampleInfo.perid = receiveInfo.perid;

                    //string selectSql = $"select 1 from  WorkTest.SampleInfo   where perid='{receiveInfo.perid}';";
                    //DataTable DTInfo = await _commRepository.GetTable(selectSql);

                    _UnitOfWork.BeginTran();


                    var persampleinfo = _perSampleInfoRepository.Query().First(p => p.id == receiveInfo.perid);

                    if (persampleinfo != null && persampleinfo.perStateNO == 2)
                    {
                        //updateTestInfo = $"update WorkTest.SampleInfo set sortState=1,state=0 where perid='{receiveInfo.perid}';";

                        var testState = await _testSampleInfoRepository.UpdateAsync(it => new test_sampleInfo() { sortState = true, state = false }, p => p.perid == persampleinfo.id.ToString());

                        if (testState)
                        {
                            var perState = await _perSampleInfoRepository.UpdateAsync(it => new per_sampleInfo() { sortState = true, state = false, perStateNO = 3 }, p => p.id == persampleinfo.id);
                            //await _commRepository.sqlcommand(updateTestInfo);
                            if (perState)
                            {
                                _UnitOfWork.CommitTran();
                                reSampleInfo.code = 0;
                                reSampleInfo.msg = "分拣成功";
                                await _recordRepository.SampleRecord(receiveInfo.barcode, RecordEnumVars.Sorts, $"专业组：{receiveInfo.groupName},分拣成功！", infos.UserName, true);
                                reSampleInfos.Add(reSampleInfo);

                            }
                            else
                            {
                                _UnitOfWork.RollbackTran();
                                reSampleInfo.code = 1;
                                reSampleInfo.msg = "分拣失败";
                            }

                        }
                        else
                        {
                            _UnitOfWork.RollbackTran();
                            reSampleInfo.code = 1;
                            reSampleInfo.msg = "分拣失败";
                        }
                    }
                    else
                    {
                        reSampleInfo.code = 1;
                        reSampleInfo.msg = "未找到样本信息";
                    }
                }
                jm.code = 0;
                commReInfo.infos = reSampleInfos;
                jm.data = commReInfo;
            }
            return jm;
        }
    }
}
