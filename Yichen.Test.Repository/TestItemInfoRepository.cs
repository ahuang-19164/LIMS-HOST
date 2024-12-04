
using System.Data;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.Net.Caching.Manuals;
using Yichen.Net.Data;
using Yichen.Net.Utility.Extensions;
using Yichen.Other.IRepository;
using Yichen.Other.Model;
using Yichen.System.IRepository;
using Yichen.System.Model;
using Yichen.Test.IRepository;
using Yichen.Test.Model.table;

namespace Yichen.Test.Repository
{
    /// <summary>
    /// 项目信息处理接口
    /// </summary>
    public class TestItemInfoRepository : BaseRepository<object>, ITestItemInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecordRepository _recordRepository;
        //private readonly IItemGroupRepository _itemGroupRepository;
        //private readonly IItemTestRepository _itemTestRepository;
        //private readonly IItemFlowRepository _itemFlowRepository;
        //private readonly IItemReferenceRepository _itemReferenceRepository;
        public TestItemInfoRepository(IUnitOfWork unitOfWork
            , IRecordRepository recordRepository
            , IItemGroupRepository itemGroupRepository
            , IItemTestRepository itemTestRepository
            , IItemFlowRepository itemFlowRepository
            , IItemReferenceRepository itemReferenceRepository
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _recordRepository = recordRepository;
            //_itemGroupRepository = itemGroupRepository;
            //_itemTestRepository = itemTestRepository;
            //_itemFlowRepository = itemFlowRepository;
            //_itemReferenceRepository = itemReferenceRepository;
        }
        /// <summary>
        /// 项目参考值判断
        /// </summary>
        /// <param name="testid"></param>
        /// <param name="ItemResult"></param>
        /// <param name="defaultValue"></param>
        /// <returns>ok</returns>

        public async Task ItemReferenceHandle(test_sampleInfo testSampleInfo, bool defaultValue = true)
        {
            #region  判断项目参考值
            await Task.Run(async () =>
            {
                //_unitOfWork.BeginTran();
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
                string flowNo = testSampleInfo.groupFlowNO;
                string upSampleItemInfo = "";
                if (testSampleInfo.groupCodes != null && flowNo == "2")
                {

                    List<string> groupCodes = testSampleInfo.groupCodes.Split(',').ToList();
                    //读取内存中的组和信息
                    List<comm_item_group> GroupInfoss = ManualDataCache<comm_item_group>.MemoryCache.LIMSGetKeyValue(CommInfo.itemgroup);
                    //List<comm_item_group> GroupInfoss =await _itemGroupRepository.GetCaChe();
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
                        //List<comm_item_test> testInfoss =await _itemTestRepository.GetCaChe();
                        //查询样本信息中包含的组和信息 并按照专业组进行排序
                        List<comm_item_test> testInfos = testInfoss.Where(p => ItemCodes.Contains(p.no.ToString()) && p.dstate == false).ToList();


                        //读取内存中的子项参考值信息
                        List<comm_item_reference> referenceInfoss = ManualDataCache<comm_item_reference>.MemoryCache.LIMSGetKeyValue(CommInfo.itemreference);
                        //List<comm_item_reference> referenceInfoss =await _itemReferenceRepository.GetCaChe();
                        //查询样本中的子项参考值信息
                        List<comm_item_reference> referenceInfos = referenceInfoss.Where(p => ItemCodes.Contains(p.testNO.ToString()) && p.dstate == false).ToList();


                        if (referenceInfos.Count > 0)
                        {

                            List<test_result_item> itemResultInfos = new List<test_result_item>();

                            //标本项目结果信息
                            List<test_result_item> sampleTestResults = DbClient.Queryable<test_result_item>().Where(p => ItemCodes.Contains(p.itemCodes.ToString()) && p.testid == testSampleInfo.id.ToString() && p.dstate == false).ToList();




                            foreach (test_result_item itemResultInfo in sampleTestResults)
                            {

                                //获取项目信息
                                comm_item_test ItemInfo = testInfos.FirstOrDefault(p => p.no == Convert.ToInt32(itemResultInfo.itemCodes));

                                //获取项目的参考值信息
                                List<comm_item_reference> ItemReferenceDT = referenceInfos.Where(p => p.testNO == Convert.ToInt32(ItemInfo.no)).ToList();
                                if (ItemInfo != null)
                                {
                                    //如果参考值为空，同步项目的属性信息
                                    if (ItemReferenceDT == null)
                                    {


                                        var items = reUpdateSql(itemResultInfo, ItemInfo, null, defaultValue);
                                        itemResultInfos.Add(items);
                                        //var items = reUpdateSql(testSampleInfo.id, itemNO, ItemInfoDT, ListReference, defaultValue);
                                        //DbClient.Updateable<test_result_item>(items).Where(p => p.testid == testSampleInfo.id.ToString() && p.itemCodes == itemNO).ExecuteCommandAsync();
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
                                        var items = reUpdateSql(itemResultInfo, ItemInfo, ListReference, defaultValue);
                                        itemResultInfos.Add(items);
                                    }

                                }
                            }

                            DbClient.Updateable<test_result_item>(itemResultInfos).ExecuteCommandAsync();


                        }
                    }
                }
            });
            #endregion
        }


        /// <summary>
        /// 生成同步项目信息sql
        /// </summary>
        /// <param name="ItemInfoDT">项目信息</param>
        /// <param name="ItemReferenceDT">参考值信息</param>
        /// <returns></returns>
        private static test_result_item reUpdateSql(test_result_item itemResult, comm_item_test ItemInfoDR, comm_item_reference ItemReferenceDR, bool defaultValue = false)
        {
            if (ItemReferenceDR != null)
            {

                string ReferenceDown = ItemReferenceDR.valueDown != null ? ItemReferenceDR.valueDown.ToString() : "";
                string ReferenceUp = ItemReferenceDR.valueUP != null ? ItemReferenceDR.valueUP.ToString() : "";
                string crisisDown = ItemReferenceDR.crisisDown != null ? ItemReferenceDR.crisisDown.ToString() : "";
                string crisisUP = ItemReferenceDR.crisisUP != null ? ItemReferenceDR.crisisUP.ToString() : "";
                string valueDescribe = ItemReferenceDR.valueDescribe != null ? ItemReferenceDR.valueDescribe.ToString() : "";
                itemResult.ReferenceDown=ReferenceDown;
                itemResult.ReferenceUp=ReferenceUp;
                itemResult.crisisDown=crisisDown;
                itemResult.crisisUp=crisisUP;
                itemResult.valueDescribe=valueDescribe;
            }
            bool visi = ItemInfoDR.visibleState != null ? Convert.ToBoolean(ItemInfoDR.visibleState) : false;
            itemResult.reportState= visi;
            bool res = ItemInfoDR.resultNullState != null ? Convert.ToBoolean(ItemInfoDR.resultNullState) : false;
            itemResult.resultNullState= res;
            int pre = ItemInfoDR.precision != null && ItemInfoDR.precision != "" ? Convert.ToInt32(ItemInfoDR.precision) : 0;
            itemResult.precision= pre;
            itemResult.channel=ItemInfoDR.channel;
            itemResult.instrumentNO=ItemInfoDR.instrumentNO;
            itemResult.itemCodes=ItemInfoDR.no.ToString();
            itemResult.itemEN=ItemInfoDR.namesEN;
            itemResult.itemNames=ItemInfoDR.names;
            itemResult.methodName=ItemInfoDR.methodName;
            itemResult.methodNO=ItemInfoDR.methodNO;
            itemResult.resultTypeNO=ItemInfoDR.resultTypeNO;
            itemResult.calculationFormula=ItemInfoDR.calculationFormula;
            itemResult.testTypeNO=ItemInfoDR.testTypeNO;
            itemResult.unit=ItemInfoDR.unit;
            if (defaultValue)
            {
                itemResult.itemResult=ItemInfoDR.defaultValue;
                itemResult.flag="";
            }

            //uInfo.values = pairs;
            //uInfo.wheres = $"testid='{testid}' and itemCodes={itemCode}";
            //return SqlFormartHelper.updateFormart(uInfo);
            return itemResult;
        }


        /// <summary>
        /// 危急值信息处理
        /// </summary>
        /// <param name="perid"></param>
        /// <param name="testid"></param>
        /// <param name="barcode"></param>
        /// <param name="itemCode"></param>
        /// <param name="itemName"></param>
        /// <param name="result"></param>
        /// <param name="Reference"></param>
        /// <param name="CrisisReference"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task ItemCrisisHandle(int perid, int testid, string barcode, string itemCode, string itemName, string result, string Reference, string CrisisReference, string UserName)
        {

            var crisisinfo = await DbClient.Queryable<other_crisisrecord>().FirstAsync(p => p.perid == perid.ToString() && p.testid == testid.ToString() && p.crisisItemCodes == itemCode);
            if (crisisinfo == null)
            {
                other_crisisrecord pairs = new other_crisisrecord();
                pairs.perid=perid.ToString();
                pairs.testid=testid.ToString();
                pairs.barcode=barcode;
                pairs.crisisItemCodes=itemCode;
                pairs.crisisItemNames=itemName;
                pairs.crisisItemResult= result;
                pairs.resultReference=Reference;
                pairs.crisisReference=CrisisReference;
                pairs.creater=UserName;
                pairs.createTime=DateTime.Now;
                pairs.state=false;
                pairs.dstate=false;
                await DbClient.Insertable<other_crisisrecord>(pairs).ExecuteCommandAsync();

            }
            else
            {
                if (crisisinfo.crisisItemResult != result)
                    crisisinfo.crisisItemResult = result;
                if (crisisinfo.resultReference != result)
                    crisisinfo.resultReference = result;
                if (crisisinfo.crisisReference != result)
                    crisisinfo.crisisReference = result;
                await DbClient.Updateable<other_crisisrecord>(crisisinfo).Where(p => p.perid == perid.ToString() && p.testid == testid.ToString() && p.crisisItemCodes == itemCode).ExecuteCommandAsync();
            }
        }

        public Task<int> InsertTestInfo(Dictionary<string, object> pairsinfo)
        {
            return DbClient.Insertable<test_sampleInfo>(pairsinfo).ExecuteReturnIdentityAsync();
        }

        public Task<int> InsertTestItemInfo(int testid, string tablaName, List<Dictionary<string, object>> pairsinfos)
        {
            pairsinfos.ForEach(p =>
            {
                p.Add("testid", testid);
            });
            return DbClient.Insertable(pairsinfos).AS(tablaName).ExecuteCommandAsync();
        }
    }
}