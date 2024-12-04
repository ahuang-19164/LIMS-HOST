using Nito.AsyncEx;
using SqlSugar;
using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;

using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.Net.Caching.Manuals;
using Yichen.Net.Data;
using Yichen.Net.Utility.Extensions;
using Yichen.Other.IRepository;
using Yichen.System.IRepository;
using Yichen.System.Model;
using Yichen.System.Repository;
using Yichen.Test.IRepository;
using Yichen.Test.IServices;
using Yichen.Test.Model.table;

namespace Yichen.Test.Services
{
    /// <summary>
    /// 危急值信息处理
    /// </summary>
    public class ItemHandleServices : BaseServices<test_sampleInfo>, IItemHandleServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        public readonly IUnitOfWork _unitOfWork;
        public readonly IRecordRepository _recordRepository;
        public readonly ITestItemInfoRepository _itemCrisisRepository;
        public readonly ITestResultInfoRepository _testRepository;
        private readonly ICommRepository _commRepository;
        //private readonly IItemApplyRepository _itemApplyRepository;
        //private readonly IItemGroupRepository _itemGroupRepository;
        //private readonly IItemTestRepository _itemTestRepository;
        //private readonly IItemFlowRepository _itemFlowRepository;
        //private readonly IItemReferenceRepository _itemReferenceRepository;
        public ItemHandleServices(IUnitOfWork unitOfWork
            , IRecordRepository recordRepository
            , ITestItemInfoRepository itemCrisisRepository
            , ITestResultInfoRepository testRepository
            , ICommRepository commRepository
            , IItemApplyRepository itemApplyRepository
            , IItemGroupRepository itemGroupRepository
            , IItemTestRepository itemTestRepository
            , IItemFlowRepository itemFlowRepository
            , IItemReferenceRepository itemReferenceRepository
            )
        {
            _unitOfWork = unitOfWork;
            _recordRepository = recordRepository;
            _itemCrisisRepository = itemCrisisRepository;
            _testRepository= testRepository;
            _commRepository = commRepository;
            //_itemApplyRepository = itemApplyRepository;
            //_itemGroupRepository = itemGroupRepository;
            //_itemTestRepository = itemTestRepository;
            //_itemFlowRepository = itemFlowRepository;
            //_itemReferenceRepository = itemReferenceRepository;
        }


        /// <summary>
        /// 插入一条危急值记录
        /// </summary>
        /// <param name="perid">样本信息id</param>
        /// <param name="testid">检验信息id</param>
        /// <param name="itemCode">项目编号</param>
        /// <param name="itemName">项目名称</param>
        /// <param name="Reference">正常参考值</param>
        /// <param name="CrisisReference">危机参考值</param>
        /// <param name="UserName">用户</param>
        public async Task<bool> ItemCrisisHandle(int perid, int testid, string barcode, string itemCode, string itemName, string result, string Reference, string CrisisReference, string UserName)
        {
            using (await _mutex.LockAsync())
            {
                string sql = $"update WorkOther.CrisisRecord set dstate=1 where perid='{perid}' and testid='{testid}';";
                _itemCrisisRepository.Update(sql);
                string sql1 = $"insert into WorkOther.CrisisRecord (perid,testid,barcode,crisisItemCodes,crisisItemNames,crisisItemResult,resultReference,crisisReference,creater,createTime,state,dstate) " +
                $"values ('{perid}','{testid}','{barcode}','{itemCode}','{itemName}','{result}','{Reference}','{CrisisReference}','{UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',0,0);";
                bool state = _itemCrisisRepository.Insert(sql);
                return state;
            }
            //int a = HLDBSqlHelper.ExecuteNonQuery(sql);

        }
        /// <summary>
        /// 检验中样本项目参考值判断
        /// </summary>
        /// <param name="testSampleInfoDto">检验中样本信息对象</param>
        /// <param name="defaultValue">是否启用默认值</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ItemReferenceHandle(int testid, string barcode, bool defaultValue = true)
        {

            WebApiCallBack jm = new WebApiCallBack();

            using (await _mutex.LockAsync())
            {
                //test_sampleInfo testSampleInfo = DbClient.Queryable<test_sampleInfo>().First(p => p.id == testid);
                test_sampleInfo testSampleInfo =await _testRepository.GetTestInfo(testid);
                if (testSampleInfo != null)
                {
                    int patientSex = 0;
                    int YAge = 0;
                    int MAge = 0;
                    int DAge = 0;

                    //patientSex = testSampleInfo.patientSexNO != null && testSampleInfo.patientSexNO != "" ? ObjectToInt(testSampleInfo.patientSexNO) : 0;
                    patientSex = testSampleInfo.patientSexNO.ObjectToInt(0);
                    //YAge = Convert.ToInt32(testSampleInfo.ageYear);
                    YAge = testSampleInfo.ageYear.ObjectToInt(0);
                    MAge = testSampleInfo.ageMoth;
                    DAge = testSampleInfo.ageDay;


                    if (testSampleInfo.groupCodes != null)
                    {

                        List<string> groupCodes = testSampleInfo.groupCodes.Split(',').ToList();
                        //读取内存中的组和信息
                        List<comm_item_group> GroupInfoss = ManualDataCache<comm_item_group>.MemoryCache.LIMSGetKeyValue(CommInfo.itemgroup);
                        //List<comm_item_group> GroupInfoss = await _itemGroupRepository.GetCaChe();
                        //查询样本信息中包含的组和信息 并按照专业组进行排序

                        List<comm_item_group> GroupInfos = GroupInfoss.Where(p => groupCodes.Contains(p.no.ToString()) && p.dstate == false).ToList();


                        string listItemCode = "";
                        foreach (comm_item_group groupItemDR in GroupInfos)
                        {
                            listItemCode += groupItemDR.testItemList != null ? groupItemDR.testItemList.ToString() : "";
                        }
                        if (listItemCode.Length > 0)
                        {
                            List<string> ItemCodes = listItemCode.Split(',').ToList();


                            //读取内存中的组和信息
                            List<comm_item_test> testInfoss = ManualDataCache<comm_item_test>.MemoryCache.LIMSGetKeyValue(CommInfo.itemtest);
                            //List<comm_item_test> testInfoss = await _itemTestRepository.GetCaChe();
                            //查询样本信息中包含的组和信息 并按照专业组进行排序
                            List<comm_item_test> testInfos = testInfoss.Where(p => ItemCodes.Contains(p.no.ToString()) && p.dstate == false).ToList();


                            //读取内存中的子项参考值信息
                            List<comm_item_reference> referenceInfoss = ManualDataCache<comm_item_reference>.MemoryCache.LIMSGetKeyValue(CommInfo.itemreference);
                            //List<comm_item_reference> referenceInfoss =await _itemReferenceRepository.GetCaChe();
                            //查询样本中的子项参考值信息
                            List<comm_item_reference> referenceInfos = referenceInfoss.Where(p => ItemCodes.Contains(p.testNO.ToString()) && p.dstate == false).ToList();


                            if (referenceInfos.Count > 0)
                            {
                                string upSampleItemInfo = "";
                                foreach (string itemNO in ItemCodes)
                                {
                                    if (itemNO != "")
                                    {
                                        //获取项目信息

                                        comm_item_test ItemInfoDT = testInfos.FirstOrDefault(p => p.no == Convert.ToInt32(itemNO));

                                        //获取项目的参考值信息
                                        List<comm_item_reference> ItemReferenceDT = referenceInfos.Where(p => p.testNO == Convert.ToInt32(itemNO)).ToList();
                                        if (ItemInfoDT != null)
                                        {
                                            //如果参考值为空，同步项目的属性信息
                                            if (ItemReferenceDT == null)
                                            {

                                                upSampleItemInfo += reUpdateSql(testid, itemNO, ItemInfoDT, null, defaultValue);

                                            }
                                            else
                                            {
                                                var ListReference = (from ItemReference in ItemReferenceDT
                                                                     where ItemReference.sexNO == patientSex
                                                                     && ItemReference.ageYearDown < YAge
                                                                     && ItemReference.ageYearUP > YAge
                                                                     && ItemReference.ageMothDown > MAge
                                                                     && ItemReference.ageMothUP > MAge
                                                                     select ItemReference).FirstOrDefault();
                                                if (ListReference == null)
                                                {
                                                    ListReference = (from ItemReference in ItemReferenceDT
                                                                     where ItemReference.sexNO == patientSex
                                                                     && ItemReference.ageYearDown < YAge
                                                                     && ItemReference.ageYearUP > YAge
                                                                     select ItemReference).FirstOrDefault();

                                                    if (ListReference == null)
                                                    {
                                                        ListReference = (from ItemReference in ItemReferenceDT
                                                                         where ItemReference.sexNO == patientSex
                                                                         && ItemReference.ageMothDown < MAge
                                                                         && ItemReference.ageMothUP > MAge
                                                                         && ItemReference.ageDayDown < DAge
                                                                         && ItemReference.ageMothUP > DAge
                                                                         select ItemReference).FirstOrDefault();
                                                        if (ListReference == null)
                                                        {
                                                            ListReference = (from ItemReference in ItemReferenceDT
                                                                             where ItemReference.sexNO == patientSex
                                                                             && ItemReference.ageMothDown < MAge
                                                                             && ItemReference.ageMothUP > MAge
                                                                             select ItemReference).FirstOrDefault();
                                                            if (ListReference == null)
                                                            {
                                                                ListReference = (from ItemReference in ItemReferenceDT
                                                                                 where ItemReference.sexNO == patientSex
                                                                                 && ItemReference.ageDayDown < DAge
                                                                                 && ItemReference.ageMothUP > DAge
                                                                                 select ItemReference).FirstOrDefault();
                                                                if (ListReference == null)
                                                                {
                                                                    ListReference = (from ItemReference in ItemReferenceDT
                                                                                     where ItemReference.ageYearDown < YAge
                                                                                     && ItemReference.ageYearUP > YAge
                                                                                     && ItemReference.ageMothDown > MAge
                                                                                     && ItemReference.ageMothUP > MAge
                                                                                     select ItemReference).FirstOrDefault();
                                                                    if (ListReference == null)
                                                                    {
                                                                        ListReference = (from ItemReference in ItemReferenceDT
                                                                                         where ItemReference.ageYearDown < YAge
                                                                                         && ItemReference.ageYearUP > YAge
                                                                                         select ItemReference).FirstOrDefault();

                                                                        if (ListReference == null)
                                                                        {
                                                                            ListReference = (from ItemReference in ItemReferenceDT
                                                                                             where ItemReference.ageMothDown < MAge
                                                                                             && ItemReference.ageMothUP > MAge
                                                                                             && ItemReference.ageDayDown < DAge
                                                                                             && ItemReference.ageMothUP > DAge
                                                                                             select ItemReference).FirstOrDefault();
                                                                            if (ListReference == null)
                                                                            {
                                                                                ListReference = (from ItemReference in ItemReferenceDT
                                                                                                 where ItemReference.ageMothDown < MAge
                                                                                                 && ItemReference.ageMothUP > MAge
                                                                                                 select ItemReference).FirstOrDefault();
                                                                                if (ListReference == null)
                                                                                {
                                                                                    ListReference = (from ItemReference in ItemReferenceDT
                                                                                                     where ItemReference.ageDayDown < DAge
                                                                                                     && ItemReference.ageMothUP > DAge
                                                                                                     select ItemReference).FirstOrDefault();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                upSampleItemInfo += reUpdateSql(testid, itemNO, ItemInfoDT, ListReference, defaultValue);
                                            }

                                        }
                                        else
                                        {
                                            jm.msg += $"项目编号：{itemNO}没有找到匹配的项目信息。";
                                        }

                                    }
                                }
                                if (upSampleItemInfo != "")
                                {
                                    int a = await _commRepository.sqlcommand(upSampleItemInfo);
                                    if (a != 0)
                                    {

                                        jm.code = 1;
                                        jm.msg = $"参考值更新成功";

                                        //jm.code = 0;
                                        //jm.msg = "参考值判断成功";
                                    }
                                    else
                                    {
                                        jm.code = 1;
                                        jm.msg = $"参考值更新失败";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    jm.code = 1;
                    jm.msg = $"条码号：{barcode}未找到样本信息";
                }
                return jm;
            }

        }


        /// <summary>
        /// 生成同步项目信息sql
        /// </summary>
        /// <param name="ItemInfoDT">项目信息</param>
        /// <param name="ItemReferenceDT">参考值信息</param>
        /// <returns></returns>
        private static string reUpdateSql(int testid, string itemCode, comm_item_test ItemInfoDR, comm_item_reference ItemReferenceDR, bool defaultValue = false)
        {
            //string ReferenceDown = "";//参考值上线
            //string ReferenceUp = "";//参考值下线
            //string crisisDown = "";//、危急值下限
            //string crisisUP = "";//危急值上限
            //string valueDescribe = "";///参考值描述..

            uInfo uInfo = new uInfo();
            uInfo.TableName = "WorkTest.SampleResult";
            Dictionary<string, object> pairs = new Dictionary<string, object>();



            if (ItemReferenceDR != null)
            {

                string ReferenceDown = ItemReferenceDR.valueDown != null ? ItemReferenceDR.valueDown.ToString() : "";


                string ReferenceUp = ItemReferenceDR.valueUP != null ? ItemReferenceDR.valueUP.ToString() : "";


                string crisisDown = ItemReferenceDR.crisisDown != null ? ItemReferenceDR.crisisDown.ToString() : "";


                string crisisUP = ItemReferenceDR.crisisUP != null ? ItemReferenceDR.crisisUP.ToString() : "";

                string valueDescribe = ItemReferenceDR.valueDescribe != null ? ItemReferenceDR.valueDescribe.ToString() : "";



                pairs.Add("ReferenceDown", ReferenceDown);


                pairs.Add("ReferenceUp", ReferenceUp);


                pairs.Add("crisisDown", crisisDown);


                pairs.Add("crisisUP", crisisUP);

                pairs.Add("valueDescribe", valueDescribe);
            }





            //pairs.Add("delegateState", dataTable.Rows[0].delegateState);
            //pairs.Add("dstate", dataTable.Rows[0].dstate);

            pairs.Add("reportState", ItemInfoDR.visibleState);


            pairs.Add("resultNullState", ItemInfoDR.resultNullState);

            //pairs.Add("state", dataTable.Rows[0].state);
            //pairs.Add("createTime", dataTable.Rows[0].createTime);
            //pairs.Add("id", dataTable.Rows[0].id);
            //pairs.Add("itemSort", dataTable.Rows[0].itemSort);

            pairs.Add("precision", ItemInfoDR.precision);

            //pairs.Add("creater", dataTable.Rows[0].creater);
            //pairs.Add("barcode", dataTable.Rows[0].barcode);

            pairs.Add("channel", ItemInfoDR.channel);

            //pairs.Add("crisisDown", dataTable.Rows[0].crisisDown);
            //pairs.Add("crisisUp", dataTable.Rows[0].crisisUp);
            //pairs.Add("delegeteCompanyNO", dataTable.Rows[0].delegeteCompanyNO);
            //pairs.Add("flag", dataTable.Rows[0].flag);
            //pairs.Add("groupCode", dataTable.Rows[0].groupCode);
            //pairs.Add("groupName", dataTable.Rows[0].groupName);
            //pairs.Add("groupNO", dataTable.Rows[0].groupNO);

            pairs.Add("instrumentNO", ItemInfoDR.instrumentNO);

            pairs.Add("itemCodes", ItemInfoDR.no);

            pairs.Add("itemEN", ItemInfoDR.namesEN);


            pairs.Add("itemNames", ItemInfoDR.names);


            if (defaultValue)

                pairs.Add("itemResult", ItemInfoDR.defaultValue);

            //pairs.Add("itemResultLog", dataTable.Rows[0].itemResultLog);

            pairs.Add("methodName", ItemInfoDR.methodName);


            pairs.Add("methodNO", ItemInfoDR.methodNO);

            //pairs.Add("perid", dataTable.Rows[0].perid);
            //pairs.Add("ReferenceDown", dataTable.Rows[0].ReferenceDown);
            //pairs.Add("ReferenceUp", dataTable.Rows[0].ReferenceUp);

            pairs.Add("resultTypeNO", ItemInfoDR.resultTypeNO);


            pairs.Add("calculationFormula", ItemInfoDR.calculationFormula);

            //pairs.Add("testFlowNO", ItemInfoDR.testFlowNO);
            //pairs.Add("testid", dataTable.Rows[0].testid);

            pairs.Add("testTypeNO", ItemInfoDR.testTypeNO);


            pairs.Add("unit", ItemInfoDR.unit);

            uInfo.values = pairs;
            uInfo.wheres = $"testid='{testid}' and itemCodes={itemCode}";
            return SqlFormartHelper.updateFormart(uInfo);
        }
    }
}
