using Nito.AsyncEx;
using System.Data;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;

using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Finance.IRepository;
using Yichen.Finance.Model;
using Yichen.Manage.IRepository;
using Yichen.Net.Data;
using Yichen.Other.IRepository;
using Yichen.System.IRepository;
using Yichen.System.Model;
using Yichen.Test.Model;
using Yichen.Net.Configuration;
using Yichen.Net.Caching.Manuals;
using Yichen.Flow.Model;
using Yichen.Other.Model.table;

namespace Yichen.Manage.Repository
{
    public class SampleHandleRepository : BaseRepository<SpecialRecord>, ISampleHandleRepository
    {
        /// <summary>
        /// 异步锁
        /// </summary>
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecordRepository _recordRepository;
        private readonly IFinanceInfoRepository _financeInfoRepository;
        //private readonly IItemApplyRepository _itemApplyRepository;
        //private readonly IItemGroupRepository _itemGroupRepository;
        //private readonly IItemTestRepository _itemTestRepository;
        //private readonly IItemFlowRepository _itemFlowRepository;
        public SampleHandleRepository(IUnitOfWork unitOfWork
            , IRecordRepository recordRepository
            , IFinanceInfoRepository financeInfoRepository
            , IItemApplyRepository itemApplyRepository
            , IItemGroupRepository itemGroupRepository
            , IItemTestRepository itemTestRepository
            , IItemFlowRepository itemFlowRepository
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _recordRepository = recordRepository;
            _financeInfoRepository = financeInfoRepository;
            //_itemApplyRepository = itemApplyRepository;
            //_itemGroupRepository = itemGroupRepository;
            //_itemTestRepository = itemTestRepository;
            //_itemFlowRepository = itemFlowRepository;
        }



        /// <summary>
        /// 样本延迟
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> InfoDelay(commInfoModel<TesthandleModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {

                if (infos.infos != null)
                {
                    try
                    {
                        foreach (TesthandleModel testInfo in infos.infos)
                        {
                            if (testInfo.perid != 0 && testInfo.testid != 0)
                            {
                                string msg = "";
                                if (testInfo.handleTypeNO == "2")
                                {
                                    if (testInfo.infoState)
                                    {
                                        string sql = $"update WorkOther.SpecialRecord set state='{testInfo.state}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                        sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                        int a = DbClient.Ado.ExecuteCommand(sql);
                                        jm.data = 1;
                                        jm.otherData = 4;
                                        msg = "处理类型：延迟处理";
                                    }
                                    else
                                    {
                                        string sql = $"update WorkOther.SpecialRecord set state='{testInfo.state}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                        //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                        int a = DbClient.Ado.ExecuteCommand(sql);
                                        jm.data = 1;
                                        jm.otherData = 0;
                                        msg = "处理类型：拒绝延迟处理";
                                    }

                                }
                                else
                                {
                                    string sql = $"update WorkOther.SpecialRecord set state='{testInfo.state}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                    //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                    int a = DbClient.Ado.ExecuteCommand(sql);
                                    msg = "处理类型：等待延迟处理";
                                    jm.data = 1;
                                    jm.otherData = 0;
                                }



                                _recordRepository.SampleRecord(testInfo.barcode, RecordEnumVars.InfoDelay, $"{testInfo.barcode}标本延迟操作成功！" + msg, infos.UserName, false);



                                //_recordRepository.SampleRecord(testInfo.barcode, "标本延迟", $"{testInfo.barcode}标本延迟操作成功！" + msg, infos.UserName, false);
                                jm.msg = "操作成功";
                            }
                            else
                            {
                                jm.data = 0;
                                jm.otherData = 0;
                                jm.msg = "未读取到样本信息";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        jm.data = 0;
                        jm.otherData = 0;
                        jm.msg = ex.Message;
                    }
                }
                else
                {
                    jm.data = 0;
                    jm.otherData = 0;
                    jm.msg = "提交数据为空";
                }
            }
            return jm;
        }
        /// <summary>
        /// 样本重采
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> InfoAgin(commInfoModel<TesthandleModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                if (infos.infos != null)
                {
                    try
                    {
                        foreach (TesthandleModel testInfo in infos.infos)
                        {
                            if (testInfo.perid != 0 && testInfo.testid != 0)
                            {
                                string msg = "";
                                if (testInfo.handleTypeNO == "2")
                                {
                                    if (testInfo.infoState)
                                    {
                                        string sql = $"update WorkOther.SpecialRecord set state='{testInfo.state}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                        sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                        int a = DbClient.Ado.ExecuteCommand(sql);
                                        msg = "处理类型：重采处理";
                                        jm.otherData = 4;
                                        jm.data = 1;
                                    }
                                    else
                                    {
                                        string sql = $"update WorkOther.SpecialRecord set state='{testInfo.state}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                        //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                        int a = DbClient.Ado.ExecuteCommand(sql);
                                        msg = "处理类型：拒绝重采处理";
                                        jm.otherData = 0;
                                        jm.data = 1;
                                    }

                                }
                                else
                                {
                                    string sql = $"update WorkOther.SpecialRecord set state='{testInfo.state}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                    //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                    int a = DbClient.Ado.ExecuteCommand(sql);
                                    jm.data = 1;
                                    jm.otherData = 0;
                                    msg = "处理类型：等待重采处理";
                                }



                                _recordRepository.SampleRecord(testInfo.barcode, RecordEnumVars.InfoAgin, $"{testInfo.barcode}标本操作成功！{msg}", infos.UserName, false);



                                jm.msg = "操作成功";
                            }
                            else
                            {
                                jm.data = 0;
                                jm.otherData = 0;
                                jm.msg = "提交数据不完整";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        jm.data = 0;
                        jm.otherData = 0;
                        jm.msg = ex.Message;
                    }
                }
                else
                {
                    jm.data = 0;
                    jm.otherData = 0;
                    jm.msg = "提交数据为空";
                }
            }
            return jm;
        }
        /// <summary>
        /// 样本退回
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Infoback(commInfoModel<TesthandleModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                if (infos.infos != null)
                {
                    try
                    {
                        foreach (TesthandleModel testInfo in infos.infos)
                        {


                            if (testInfo.perid != 0 && testInfo.testid != 0)
                            {

                                string msg = "";
                                if (testInfo.handleTypeNO == "2")
                                {
                                    if (testInfo.infoState)
                                    {
                                        string SFinance = $"select perid,standerCharge,settlementCharge,charge from Finance.GroupBillInfo where testid={testInfo.testid} and dstate=0";
                                        DataTable DTFinance = DbClient.Ado.GetDataTable(SFinance);
                                        if (DTFinance.Rows.Count > 0)
                                        {

                                            string groupperid = DTFinance.Rows[0]["perid"] != DBNull.Value ? DTFinance.Rows[0]["perid"].ToString() : "0";

                                            if (groupperid != "0")
                                            {
                                                decimal standerCharge = 0;
                                                decimal settlementCharge = 0;
                                                decimal charge = 0;
                                                foreach (DataRow drFinance in DTFinance.Rows)
                                                {
                                                    standerCharge += drFinance["standerCharge"] != DBNull.Value ? Convert.ToDecimal(drFinance["standerCharge"]) : 0;
                                                    settlementCharge += drFinance["settlementCharge"] != DBNull.Value ? Convert.ToDecimal(drFinance["settlementCharge"]) : 0;
                                                    charge += drFinance["charge"] != DBNull.Value ? Convert.ToDecimal(drFinance["charge"]) : 0;
                                                }
                                                string sql = $"update WorkOther.SpecialRecord set state='{testInfo.state}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                                sql += $"update WorkTest.SampleInfo set tabTypeNO=4,testStateNO='8' where id='{testInfo.testid}';";
                                                //int a = DbClient.Ado.ExecuteCommand( sql);
                                                sql += $"update Finance.GroupBillInfo set tabTypeNO='4',state=0,dstate=1 where testid={testInfo.testid} ";
                                                sql += $"update Finance.ApplyBillInfo set standerCharge=standerCharge-{standerCharge},settlementCharge=settlementCharge-{settlementCharge},charge=charge-{charge} where perid={groupperid} ";
                                                int a = DbClient.Ado.ExecuteCommand(sql);
                                                //string sqlWx= $"update WorkTest.SampleInfo set tabTypeNO=4,testStateNO='8' where id='{testInfo.testid}';";
                                                //SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, sqlWx);
                                                msg = "处理类型：退单处理";
                                                jm.data = 1;
                                                jm.otherData = 8;
                                            }
                                            else
                                            {
                                                jm.data = 0;
                                                jm.otherData = 0;
                                                jm.msg = "未匹配到对应数据";
                                            }
                                        }
                                        else
                                        {
                                            jm.data = 1;
                                            jm.otherData = 0;
                                            string sql = $"update WorkPer.SampleInfo set dstate='1' where id='{testInfo.perid}';";
                                            int a = DbClient.Ado.ExecuteCommand(sql);
                                            //a = SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, sql);
                                        }
                                    }
                                    else
                                    {
                                        string sql = $"update WorkOther.SpecialRecord set state='{testInfo.state}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                        int a = DbClient.Ado.ExecuteCommand(sql);
                                        msg = "处理类型：拒绝退单处理";
                                        jm.data = 1;
                                        jm.otherData = 0;
                                    }


                                }
                                else
                                {
                                    string sql = $"update WorkOther.SpecialRecord set state='{testInfo.state}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                    //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                    int a = DbClient.Ado.ExecuteCommand(sql);
                                    msg = "处理类型：等待处理";
                                    jm.data = 1;
                                    jm.otherData = 0;
                                }



                                _recordRepository.SampleRecord(testInfo.barcode, RecordEnumVars.ItemCancel, $"{testInfo.barcode}标本操作成功！{msg}", infos.UserName, false);



                                jm.msg = "操作成功";
                            }
                            else
                            {
                                jm.data = 0;
                                jm.otherData = 0;
                                jm.msg = "提交数据不完整";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        jm.data = 0;
                        jm.otherData = 0;
                        jm.msg = ex.Message;
                    }
                }
                else
                {
                    jm.data = 0;
                    jm.otherData = 0;
                    jm.msg = "提交数据为空";
                }
            }
            return jm;
        }
        /// <summary>
        /// 退项申请
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GroupItemCancel(commInfoModel<TesthandleModel> infos)
        {

            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                try
                {
                    foreach (TesthandleModel testInfo in infos.infos)
                    {
                        if (testInfo.submitItemCodes != null)
                        {
                            ///需要退项的项目编号
                            string[] CancelgroupCodes = testInfo.submitItemCodes.Split(',');

                            DataTable DTsampleInfo = null;

                            string sql = $"select * from WorkPer.SampleInfo where id='{testInfo.perid}' and dstate=0";
                            DTsampleInfo = DbClient.Ado.GetDataTable(sql);

                            if (DTsampleInfo.Rows.Count == 1)
                            {

                                string barcode = DTsampleInfo.Rows[0]["barcode"] == DBNull.Value ? "" : DTsampleInfo.Rows[0]["barcode"].ToString();

                                if (testInfo.testid != 0)
                                {

                                    string sampleInfoPerState = DTsampleInfo.Rows[0]["perStateNO"] != DBNull.Value ? DTsampleInfo.Rows[0]["perStateNO"].ToString() : "1";

                                    if (sampleInfoPerState != "1")
                                    {
                                        string sqlApply = $"select id,sampleID,groupCodes from WorkTest.SampleInfo where id='{testInfo.testid}' and dstate=0;";
                                        ///获取检验中的样本信息
                                        DataTable DTTestInfo = DbClient.Ado.GetDataTable(sqlApply);
                                        //获取样本的检验信息
                                        if (DTTestInfo.Rows.Count > 0)
                                        {
                                            string InfoGroupCodes = "";//已有组合项目编号集合
                                            foreach (DataRow applyRow in DTTestInfo.Rows)
                                            {
                                                if (applyRow["groupCodes"] != DBNull.Value)
                                                {
                                                    InfoGroupCodes += applyRow["groupCodes"].ToString();
                                                }
                                            }
                                            string sqlGroupPrice = $"select * from Finance.GroupBillInfo where perid={testInfo.testid} and dstate=0;";
                                            //获取信息原项目组合项目价格信息
                                            DataTable DTGroupPrice = DbClient.Ado.GetDataTable(sqlGroupPrice);

                                            //读取内存中的组和信息
                                            List<comm_item_group> GroupInfoss = ManualDataCache<comm_item_group>.MemoryCache.LIMSGetKeyValue(CommInfo.itemgroup);
                                            //List<comm_item_group> GroupInfoss =await _itemGroupRepository.GetCaChe();
                                            //获取需要退项的组合项目信息 并按照专业组进行排序
                                            List<comm_item_group> GroupInfos = GroupInfoss.Where(p => CancelgroupCodes.Contains(p.no.ToString()) && p.dstate == false).OrderBy(p => p.groupNO).ToList();


                                            //读取内存中的组和信息
                                            List<comm_item_test> ItemInfoss = ManualDataCache<comm_item_test>.MemoryCache.LIMSGetKeyValue(CommInfo.itemtest);
                                            //List<comm_item_test> ItemInfoss = await _itemTestRepository.GetCaChe();



                                            if (GroupInfos != null)
                                            {
                                                ///需要退项的项目信息

                                                string listItemsCodes = "";

                                                //需要退项的组合项目流程信息
                                                //string listFlowCodes = "";

                                                List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
                                                //List<comm_item_flow> FlowInfoss = await _itemFlowRepository.GetCaChe();


                                                string UpdateApplyBillInfo = "";
                                                string UpdateGroupInfos = "";//专业组信息修改sql
                                                string UpdateGroupPirceInfo = "";//组合项目价格修改
                                                string UpdateItemInfo = "";

                                                decimal standerCharge = 0;//组合项目标准价格
                                                decimal settlementCharge = 0;//组合项目结算价格
                                                decimal charge = 0;//组合项目收费价格



                                                foreach (comm_item_group DRgroupInfo in GroupInfos)
                                                {

                                                    DataRow[] DRGroupPrice = DTGroupPrice.Select($"perid={testInfo.perid} and groupCode='{DRgroupInfo.no}'");//获取退项组合项目价格信息
                                                    string sampleTestid = "";
                                                    if (DRGroupPrice.Length > 0)
                                                    {

                                                        sampleTestid = DRGroupPrice[0]["testid"] != DBNull.Value ? DRGroupPrice[0]["testid"].ToString() : "";

                                                        standerCharge += DRGroupPrice[0]["standerCharge"] != DBNull.Value ? Convert.ToDecimal(DRGroupPrice[0]["standerCharge"]) : 0;
                                                        settlementCharge += DRGroupPrice[0]["settlementCharge"] != DBNull.Value ? Convert.ToDecimal(DRGroupPrice[0]["settlementCharge"]) : 0;
                                                        charge += DRGroupPrice[0]["charge"] != DBNull.Value ? Convert.ToDecimal(DRGroupPrice[0]["charge"]) : 0;
                                                    }
                                                    if (DRgroupInfo.groupNO != null)
                                                    {
                                                        string reflectionFile = "";//反射文件名称
                                                        string reflectionFrm = ""; //反射窗体名称
                                                        string dataSource = "";    //反射数据源
                                                        string imgSource = "";     //反射图片源
                                                                                   //bool delGroupState = DRgroupInfo.delegeteState"] != DBNull.Value ? Convert.ToBoolean(DRgroupInfo.delegeteState"]) : false;//判断组合项目样本委托状态
                                                                                   //string delGroupClientNO = delGroupState && DRgroupInfo.delegeteState"] != DBNull.Value ? DRgroupInfo.delegeteState"].ToString() : "";//获取组合项目委托单位编号
                                                                                   //                                                                                                                                       //string bbb = DRgroupInfo.groupFlowNO"].ToString();
                                                        if (DRgroupInfo.groupFlowNO == null)
                                                        {

                                                            comm_item_flow FlowInfos = FlowInfoss.FirstOrDefault(p => p.no == 2);



                                                            reflectionFile = FlowInfos.reflectionFile != null ? FlowInfos.reflectionFile.ToString() : "WorkTest.Test";

                                                            reflectionFrm = FlowInfos.reflectionFrm != null ? FlowInfos.reflectionFrm.ToString() : "FrmTest";
                                                            dataSource = FlowInfos.dataSource != null ? FlowInfos.dataSource.ToString() : "WorkTest.SampleResult";
                                                            imgSource = FlowInfos.imgSource != null ? FlowInfos.imgSource.ToString() : "WorkTest.SampleResultImg";
                                                        }
                                                        else
                                                        {

                                                            comm_item_flow FlowInfos = FlowInfoss.FirstOrDefault(p => p.no == Convert.ToInt32(DRgroupInfo.groupFlowNO));

                                                            //testFlowNO = DRFlowInfo.no != null ? DRFlowInfo.no.ToString() : "0";

                                                            reflectionFile = FlowInfos.reflectionFile != null ? FlowInfos.reflectionFile.ToString() : "";

                                                            if (reflectionFile != "")
                                                                reflectionFrm = FlowInfos.reflectionFrm != null ? FlowInfos.reflectionFrm.ToString() : "";
                                                            if (reflectionFrm != "")
                                                                dataSource = FlowInfos.dataSource != null ? FlowInfos.dataSource.ToString() : "";
                                                            if (dataSource != "")
                                                                imgSource = FlowInfos.imgSource != null ? FlowInfos.imgSource.ToString() : "";
                                                        }
                                                        ///判断是否读取的反射信息
                                                        if (reflectionFile != "" && reflectionFrm != "" && dataSource != "")
                                                        {

                                                            string itemCodes = DRgroupInfo.testItemList != null ? DRgroupInfo.testItemList.Substring(0, DRgroupInfo.testItemList.Length - 1) : "";

                                                            UpdateItemInfo += $"update {dataSource} set dstate=1 where perid={testInfo.perid} and itemCodes not in ({itemCodes});";

                                                            UpdateGroupPirceInfo += $"update Finance.GroupBillInfo set  tabTypeNO='3',state=0,dstate=1 where perid={testInfo.perid} and groupCode={DRgroupInfo.no};";
                                                        }
                                                    }
                                                }
                                                if (GroupInfos != null)//判断是否为该专业组，流程唯一项目。
                                                {
                                                    string grouCodes = "";//组合项目编号集合
                                                    string groupNames = "";//组合项目名称集合
                                                    //string itemCodes = "";


                                                    string[] oldGroupCodes = InfoGroupCodes.Split(',');

                                                    List<comm_item_group> OldGroupInfos = GroupInfoss.Where(p => oldGroupCodes.Contains(p.no.ToString())).ToList();



                                                    if (OldGroupInfos.Count == GroupInfos.Count)
                                                    {
                                                        UpdateGroupInfos += $"update WorkTest.SampleInfo set tabTypeNO=3,dstate=1 where id='{testInfo.testid}' and dstate=0;";
                                                        UpdateApplyBillInfo = $"update Finance.ApplyBillInfo set charge=charge-{charge},settlementCharge=settlementCharge-{settlementCharge},standerCharge=standerCharge-{standerCharge} where perid={testInfo.perid} and dstate=0;";
                                                    }
                                                    else
                                                    {
                                                        foreach (comm_item_group oldgroupItem in OldGroupInfos)
                                                        {
                                                            bool statess = false;
                                                            foreach (comm_item_group CancelgroupItem in GroupInfos)
                                                            {
                                                                if (CancelgroupItem.no == oldgroupItem.no)
                                                                {
                                                                    statess = true;
                                                                }
                                                            }
                                                            if (!statess)
                                                            {
                                                                grouCodes += oldgroupItem.no != null ? oldgroupItem.no.ToString() + "," : "";
                                                                groupNames += oldgroupItem.names != null ? oldgroupItem.names.ToString() + "," : "";
                                                            }
                                                        }
                                                        UpdateGroupInfos += $"update WorkTest.SampleInfo set tabTypeNO=3,groupCodes='{grouCodes}',groupNames='{groupNames}' where id='{testInfo.testid}' and dstate=0;";
                                                        UpdateApplyBillInfo = $"update Finance.ApplyBillInfo set charge=charge-{charge},settlementCharge=settlementCharge-{settlementCharge},standerCharge=standerCharge-{standerCharge} where perid={testInfo.perid} and dstate=0;";
                                                    }
                                                    string insertSampleInfoAll = UpdateGroupInfos + UpdateItemInfo + UpdateApplyBillInfo + UpdateGroupPirceInfo;
                                                    //insertSampleInfoWx += UpdateGroupInfos;
                                                    //sampleState = true;

                                                    await DbClient.Ado.ExecuteCommandAsync(insertSampleInfoAll);



                                                }
                                            }
                                        }
                                        else
                                        {
                                            jm.data = 0;
                                            jm.otherData = 0;
                                            jm.msg += $"此样本未录入组套信息";
                                        }


                                    }
                                    else
                                    {
                                        jm.data = 0;
                                        jm.otherData = 0;
                                        jm.msg += $"该信息未审核，请通过标本录入进行项目修改";
                                    }

                                }
                                else
                                {

                                    jm.data = 0;
                                    jm.otherData = 0;
                                    jm.msg += $"请提交正确的信息ID";
                                }
                            }
                            else
                            {

                                jm.data = 0;
                                jm.otherData = 0;
                                jm.msg += $"未找到需要修改的样本信息";
                            }
                        }
                        else
                        {

                            jm.data = 0;
                            jm.otherData = 0;
                            jm.msg += $"未检测到需要退项的项目信息";
                        }
                    }

                }
                catch (Exception ex)
                {
                    //
                    //commReInfo.msg += ex.ToString();
                    jm.data = 0;
                    jm.otherData = 0;
                    jm.msg = ex.Message;
                }

                //return ConvertDatatable.DataTableToString(commReInfo);
            }
            return jm;
        }
        /// <summary>
        /// 增项申请
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GorupItemAdd(commInfoModel<TesthandleModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                try
                {
                    List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                    foreach (TesthandleModel testInfo in infos.infos)
                    {
                        commReSampleInfo reSampleInfo = new commReSampleInfo();
                        string returnMsg = "";

                        if (testInfo.submitItemCodes != null)
                        {

                            DataTable DTsampleInfo = null;

                            string sql = $"select * from WorkPer.SampleInfo where id='{testInfo.perid}' and dstate=0";
                            DTsampleInfo = DbClient.Ado.GetDataTable(sql);
                            decimal standerchargeCount = 0;//标准收费总金额
                            decimal settlementChargeCount = 0;//结算收费总金额
                            decimal chargeCount = 0;//实际收费总金额
                            if (DTsampleInfo.Rows.Count == 1)
                            {


                                string agentNO = DTsampleInfo.Rows[0]["agentNO"] == DBNull.Value ? "" : DTsampleInfo.Rows[0]["agentNO"].ToString();


                                string hospitalNO = DTsampleInfo.Rows[0]["hospitalNO"] == DBNull.Value ? "" : DTsampleInfo.Rows[0]["hospitalNO"].ToString();


                                string patientType = DTsampleInfo.Rows[0]["patientTypeNO"] == DBNull.Value ? "" : DTsampleInfo.Rows[0]["patientTypeNO"].ToString();


                                string department = DTsampleInfo.Rows[0]["department"] == DBNull.Value ? "" : DTsampleInfo.Rows[0]["department"].ToString();


                                string barcode = DTsampleInfo.Rows[0]["barcode"] == DBNull.Value ? "" : DTsampleInfo.Rows[0]["barcode"].ToString();


                                string receiveTime = DTsampleInfo.Rows[0]["receiveTime"] == DBNull.Value ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : DTsampleInfo.Rows[0]["receiveTime"].ToString();

                                //string perid = DTsampleInfo.Rows[0]["perid"] == DBNull.Value ? "0" : DTsampleInfo.Rows[0]["perid"].ToString();
                                if (hospitalNO != "")
                                {

                                    FinanceClientModel financeClient = await _financeInfoRepository.GetFinanceClient(hospitalNO);//获取医院收费信息


                                    string samplePerid = DTsampleInfo.Rows[0]["id"].ToString();

                                    if (DTsampleInfo.Rows[0]["perStateNO"] != DBNull.Value)
                                    {


                                        if (DTsampleInfo.Rows[0]["perStateNO"].ToString() != "1")
                                        {

                                            string sqlApply = $"select groupNO,groupCodes,groupFlowNO from WorkTest.SampleInfo where perid='{testInfo.perid}' and dstate=0;";
                                            DataTable DTApplyInfo = DbClient.Ado.GetDataTable(sqlApply);
                                            if (DTApplyInfo.Rows.Count > 0)
                                            {

                                                string InfoGroupCodes = "";//已有组合项目编号集合
                                                foreach (DataRow applyRow in DTApplyInfo.Rows)
                                                {
                                                    if (applyRow["groupCodes"] != DBNull.Value)
                                                    {
                                                        InfoGroupCodes += applyRow["groupCodes"].ToString();
                                                    }
                                                }
                                                string[] groupcodes = testInfo.submitItemCodes.Split(",");
                                                //读取内存中的组和信息
                                                List<comm_item_group> GroupInfoss = ManualDataCache<comm_item_group>.MemoryCache.LIMSGetKeyValue(CommInfo.itemgroup);
                                                //List<comm_item_group> GroupInfoss =await _itemGroupRepository.GetCaChe();
                                                //查询样本信息中包含的组和信息 并按照专业组进行排序
                                                List<comm_item_group> DTOldGroupInfo = GroupInfoss.Where(p => groupcodes.Contains(p.no.ToString()) && p.dstate == false).OrderBy(p => p.groupNO).ToList();
                                                //新增组合项目信息
                                                List<comm_item_group> DTInsertGroupInfo = GroupInfoss.Where(p => groupcodes.Contains(p.no.ToString()) && p.dstate == false).OrderBy(p => p.groupNO).ToList();

                                                //读取子项信息获取子项集合
                                                List<comm_item_test> TestInfoss = ManualDataCache<comm_item_test>.MemoryCache.LIMSGetKeyValue(CommInfo.itemtest);
                                                //List<comm_item_test> TestInfoss = await _itemTestRepository.GetCaChe();
                                                //读取子项信息获取流程集合
                                                List<comm_item_flow> FlowInfoss = ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
                                                //List<comm_item_flow> FlowInfoss = await _itemFlowRepository.GetCaChe() ;

                                                ///检验信息存在的增项信息
                                                string sqlitem1 = "";
                                                //检验信息不存在的增项信息
                                                string sqlitem2 = "";
                                                //string sqlitem2 = "declare @testid varchar(500);";


                                                if (DTInsertGroupInfo != null)
                                                {
                                                    //string listItemsCodess = "";
                                                    //string listFlowCodes = "";




                                                    while (DTInsertGroupInfo.Count > 0)
                                                    {
                                                        comm_item_group InsertGroupInfo = DTInsertGroupInfo[0];



                                                        DataRow testSampleInfo = DTApplyInfo.Select($"groupNO='{InsertGroupInfo.groupNO}'&&groupFlowNO='{InsertGroupInfo.groupFlowNO}'").FirstOrDefault();


                                                        int ItemSort = 1;//项目排序


                                                        if (testSampleInfo != null)
                                                        {
                                                            //查询样本信息中包含的流程信息 并按照专业组进行排序

                                                            comm_item_flow FlowInfos = FlowInfoss.FirstOrDefault(p => p.no == Convert.ToInt32(InsertGroupInfo.groupFlowNO) && p.dstate == false);


                                                            string reflectionFile = "";//反射文件名称
                                                            string reflectionFrm = ""; //反射窗体名称
                                                            string dataSource = "";    //反射数据源
                                                            string imgSource = "";     //反射图片源
                                                                                       //bool delGroupState = DRgroupInfo.delegeteState"] != DBNull.Value ? Convert.ToBoolean(DRgroupInfo.delegeteState"]) : false;//判断组合项目样本委托状态
                                                                                       //string delGroupClientNO = delGroupState && DRgroupInfo.delegeteState"] != DBNull.Value ? DRgroupInfo.delegeteState"].ToString() : "";//获取组合项目委托单位编号
                                                                                       //                                                                                                                                       //string bbb = DRgroupInfo.groupFlowNO"].ToString();
                                                            if (FlowInfos == null)
                                                            {

                                                                reflectionFile = FlowInfos.reflectionFile != null ? FlowInfos.reflectionFile.ToString() : "WorkTest.Test";

                                                                reflectionFrm = FlowInfos.reflectionFrm != null ? FlowInfos.reflectionFrm.ToString() : "FrmTest";
                                                                dataSource = FlowInfos.dataSource != null ? FlowInfos.dataSource.ToString() : "WorkTest.SampleResult";
                                                                imgSource = FlowInfos.imgSource != null ? FlowInfos.imgSource.ToString() : "WorkTest.SampleResultImg";
                                                            }
                                                            else
                                                            {
                                                                reflectionFile = FlowInfos.reflectionFile != null ? FlowInfos.reflectionFile.ToString() : "";
                                                                if (reflectionFile != "")
                                                                    reflectionFrm = FlowInfos.reflectionFrm != null ? FlowInfos.reflectionFrm.ToString() : "";
                                                                if (reflectionFrm != "")
                                                                    dataSource = FlowInfos.dataSource != null ? FlowInfos.dataSource.ToString() : "";
                                                                if (dataSource != "")
                                                                    imgSource = FlowInfos.imgSource != null ? FlowInfos.imgSource.ToString() : "";
                                                            }

                                                            List<comm_item_group> InsertGroupInfos = DTInsertGroupInfo.Where(p => p.groupNO == InsertGroupInfo.groupNO && p.groupFlowNO == InsertGroupInfo.groupFlowNO).ToList();


                                                            int testid = testSampleInfo["id"] != DBNull.Value ? Convert.ToInt32(testSampleInfo["id"]) : 0;

                                                            string groupNO = testSampleInfo["groupNO"] != DBNull.Value ? testSampleInfo["groupNO"].ToString() : "";


                                                            string groupCodes = testSampleInfo["groupCodes"] != DBNull.Value ? testSampleInfo["groupCodes"].ToString() : "";

                                                            string getitemsql = $"select * from {dataSource} where testid='{testid}'";
                                                            DataTable testDT = await DbClient.Ado.GetDataTableAsync(getitemsql);
                                                            ItemSort = testDT.Rows.Count;

                                                            //新增项目的组合项目编号集合
                                                            string groupcodess = "";
                                                            //新增项目的组合项目名称集合
                                                            string groupNamess = "";
                                                            //bool delGroupState = false;
                                                            //string delegeteCompanyNO = "";
                                                            foreach (comm_item_group item_Group in InsertGroupInfos)
                                                            {
                                                                groupcodess += item_Group.no + ",";
                                                                groupNamess += item_Group.names + ",";
                                                                ///插入子项信息
                                                                string insertItemInfo = "";
                                                                //获取组合项目折扣信息
                                                               DiscountPriceModel GroupPriceinfo = await _financeInfoRepository.GetGroupPrice(agentNO, hospitalNO, patientType, department, item_Group.no.ToString(), financeClient.chargeLevelNO, financeClient.discount);
                                                               //记录项目收费价格
                                                                standerchargeCount += GroupPriceinfo.standerPirce;//标准收费总金额
                                                                settlementChargeCount += GroupPriceinfo.settlementPirce;//结算收费总金额
                                                                chargeCount += GroupPriceinfo.chargePice;//实际收费总金额

                                                                //生成组合项目收费信息
                                                                string FinanceGroupSql = "insert into Finance.GroupBillInfo (perid,testid,barcode,tabTypeNO,groupNO,chargeTypeNO,chargeLevelNO,discountState,discount,personNO,groupCode,groupName,operater,operatTime,standerCharge,settlementCharge,charge,dstate)";
                                                                //FinanceGroupSql += $" values ('{testInfo.perid}',@testid,'{testInfo.barcode}','1','{DRInsertGroupInfo.groupNO}','{GroupPriceinfo.chargeTypeNO}','{financeClient.chargeLevelNO}','{GroupPriceinfo.groupDiscountState}','{financeClient.discount}','{financeClient.personNO}','{DRInsertGroupInfo.no}','{DRInsertGroupInfo.names}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{GroupPriceinfo.standerPirce}','{GroupPriceinfo.settlementPirce}','{GroupPriceinfo.chargePice}',0);";
                                                                FinanceGroupSql += $" values ('{testInfo.perid}','{testid}','{testInfo.barcode}','1','{item_Group.groupNO}','{GroupPriceinfo.chargeTypeNO}','{financeClient.chargeLevelNO}','{GroupPriceinfo.groupDiscountState}','{financeClient.discount}','{financeClient.personNO}','{item_Group.no}','{item_Group.names}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{GroupPriceinfo.standerPirce}','{GroupPriceinfo.settlementPirce}','{GroupPriceinfo.chargePice}',0);";

                                                                string[] itemcodes = item_Group.testItemList.Split(',');


                                                                List<comm_item_test> itemInfo = TestInfoss.Where(p => itemcodes.Contains(p.no.ToString())).ToList();

                                                                foreach (comm_item_test item in itemInfo)
                                                                {
                                                                    ItemSort += 1;
                                                                    bool delItemState = item.delegeteState != null ? Convert.ToBoolean(item.delegeteState) : false;
                                                                    string delItemClientNO = delItemState && item.delegeteCompanyNO != null ? item.delegeteCompanyNO.ToString() : "";
                                                                    if (delItemState)
                                                                    {
                                                                        insertItemInfo += $"insert into {dataSource} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate)";
                                                                        insertItemInfo += $"values ({testInfo.perid},'{testid}','{barcode}','{item_Group.groupNO}','{item_Group.no}','{item_Group.names}','{item.no}','{item.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
                                                                    }
                                                                    else
                                                                    {
                                                                        insertItemInfo += $"insert into {dataSource} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate)";
                                                                        insertItemInfo += $"values ({testInfo.perid},'{testid}','{barcode}','{item_Group.groupNO}','{item_Group.no}','{item_Group.names}','{item.no}','{item.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
                                                                    }
                                                                }
                                                                string testinfoSql = $"update WorkTest.SampleInfo set groupCodes=groupCodes+'{groupcodess}',groupNames=groupNames+'{groupNamess}' where id={testid}";
                                                                sqlitem1 += testinfoSql + FinanceGroupSql + insertItemInfo;

                                                                DTInsertGroupInfo.Remove(item_Group);
                                                            }
                                                        }
                                                        else
                                                        {

                                                            string sqliteminfo = "";



                                                            //查询样本信息中包含的流程信息 并按照专业组进行排序

                                                            comm_item_flow FlowInfos = FlowInfoss.FirstOrDefault(p => p.no == Convert.ToInt32(InsertGroupInfo.groupFlowNO) && p.dstate == false);


                                                            string reflectionFile = "";//反射文件名称
                                                            string reflectionFrm = ""; //反射窗体名称
                                                            string dataSource = "";    //反射数据源
                                                            string imgSource = "";     //反射图片源
                                                                                       //bool delGroupState = DRgroupInfo.delegeteState"] != DBNull.Value ? Convert.ToBoolean(DRgroupInfo.delegeteState"]) : false;//判断组合项目样本委托状态
                                                                                       //string delGroupClientNO = delGroupState && DRgroupInfo.delegeteState"] != DBNull.Value ? DRgroupInfo.delegeteState"].ToString() : "";//获取组合项目委托单位编号
                                                                                       //                                                                                                                                       //string bbb = DRgroupInfo.groupFlowNO"].ToString();
                                                            if (FlowInfos == null)
                                                            {

                                                                reflectionFile = FlowInfos.reflectionFile != null ? FlowInfos.reflectionFile.ToString() : "WorkTest.Test";

                                                                reflectionFrm = FlowInfos.reflectionFrm != null ? FlowInfos.reflectionFrm.ToString() : "FrmTest";
                                                                dataSource = FlowInfos.dataSource != null ? FlowInfos.dataSource.ToString() : "WorkTest.SampleResult";
                                                                imgSource = FlowInfos.imgSource != null ? FlowInfos.imgSource.ToString() : "WorkTest.SampleResultImg";
                                                            }
                                                            else
                                                            {
                                                                reflectionFile = FlowInfos.reflectionFile != null ? FlowInfos.reflectionFile.ToString() : "";
                                                                if (reflectionFile != "")
                                                                    reflectionFrm = FlowInfos.reflectionFrm != null ? FlowInfos.reflectionFrm.ToString() : "";
                                                                if (reflectionFrm != "")
                                                                    dataSource = FlowInfos.dataSource != null ? FlowInfos.dataSource.ToString() : "";
                                                                if (dataSource != "")
                                                                    imgSource = FlowInfos.imgSource != null ? FlowInfos.imgSource.ToString() : "";
                                                            }

                                                            List<comm_item_group> InsertGroupInfos = DTInsertGroupInfo.Where(p => p.groupNO == InsertGroupInfo.groupNO && p.groupFlowNO == InsertGroupInfo.groupFlowNO).ToList();



                                                            int testid = testSampleInfo["id"] != DBNull.Value ? Convert.ToInt32(testSampleInfo["id"]) : 0;


                                                            string groupNO = testSampleInfo["groupNO"] != DBNull.Value ? testSampleInfo["groupNO"].ToString() : "";


                                                            string groupCodes = testSampleInfo["groupCodes"] != DBNull.Value ? testSampleInfo["groupCodes"].ToString() : "";

                                                            //string getitemsql = $"select * from {dataSource} where testid='{testid}'";
                                                            //DataTable testDT = await _commRepository.GetTable(getitemsql);
                                                            //ItemSort = testDT.Rows.Count;

                                                            //新增项目的组合项目编号集合
                                                            string groupcodess = "";
                                                            //新增项目的组合项目名称集合
                                                            string groupNamess = "";
                                                            bool delGroupState = false;
                                                            string delegeteCompanyNO = "";
                                                            foreach (comm_item_group item_Group in InsertGroupInfos)
                                                            {
                                                                groupcodess += item_Group.no + ",";
                                                                groupNamess += item_Group.names + ",";

                                                                delGroupState = item_Group.delegeteState != null ? Convert.ToBoolean(item_Group.delegeteState) : false;

                                                                delegeteCompanyNO = item_Group.delegeteCompanyNO;



                                                                ///插入子项信息
                                                                string insertItemInfo = "";
                                                                //获取组合项目折扣信息
                                                               DiscountPriceModel GroupPriceinfo = await _financeInfoRepository.GetGroupPrice(agentNO, hospitalNO, patientType, department, item_Group.no.ToString(), financeClient.chargeLevelNO, financeClient.discount);
                                                                //记录项目收费价格
                                                                standerchargeCount += GroupPriceinfo.standerPirce;//标准收费总金额
                                                                settlementChargeCount += GroupPriceinfo.settlementPirce;//结算收费总金额
                                                                chargeCount += GroupPriceinfo.chargePice;//实际收费总金额

                                                                //生成组合项目收费信息
                                                                string FinanceGroupSql = "insert into Finance.GroupBillInfo (perid,testid,barcode,tabTypeNO,groupNO,chargeTypeNO,chargeLevelNO,discountState,discount,personNO,groupCode,groupName,operater,operatTime,standerCharge,settlementCharge,charge,dstate)";
                                                                //FinanceGroupSql += $" values ('{testInfo.perid}',@testid,'{testInfo.barcode}','1','{DRInsertGroupInfo.groupNO}','{GroupPriceinfo.chargeTypeNO}','{financeClient.chargeLevelNO}','{GroupPriceinfo.groupDiscountState}','{financeClient.discount}','{financeClient.personNO}','{DRInsertGroupInfo.no}','{DRInsertGroupInfo.names}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{GroupPriceinfo.standerPirce}','{GroupPriceinfo.settlementPirce}','{GroupPriceinfo.chargePice}',0);";
                                                                FinanceGroupSql += $" values ('{testInfo.perid}','{testid}','{testInfo.barcode}','1','{item_Group.groupNO}','{GroupPriceinfo.chargeTypeNO}','{financeClient.chargeLevelNO}','{GroupPriceinfo.groupDiscountState}','{financeClient.discount}','{financeClient.personNO}','{item_Group.no}','{item_Group.names}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{GroupPriceinfo.standerPirce}','{GroupPriceinfo.settlementPirce}','{GroupPriceinfo.chargePice}',0);";

                                                                string[] itemcodes = item_Group.testItemList.Split(',');


                                                                List<comm_item_test> itemInfo = TestInfoss.Where(p => itemcodes.Contains(p.no.ToString())).ToList();

                                                                foreach (comm_item_test item in itemInfo)
                                                                {
                                                                    ItemSort += 1;
                                                                    bool delItemState = item.delegeteState != null ? Convert.ToBoolean(item.delegeteState) : false;
                                                                    string delItemClientNO = delItemState && item.delegeteCompanyNO != null ? item.delegeteCompanyNO.ToString() : "";
                                                                    if (delItemState)
                                                                    {
                                                                        insertItemInfo += $"insert into {dataSource} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate)";
                                                                        insertItemInfo += $"values ({testInfo.perid},'{testid}','{barcode}','{item_Group.groupNO}','{item_Group.no}','{item_Group.names}','{item.no}','{item.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
                                                                    }
                                                                    else
                                                                    {
                                                                        insertItemInfo += $"insert into {dataSource} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate)";
                                                                        insertItemInfo += $"values ({testInfo.perid},'{testid}','{barcode}','{item_Group.groupNO}','{item_Group.no}','{item_Group.names}','{item.no}','{item.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
                                                                    }
                                                                }
                                                                sqliteminfo += FinanceGroupSql + insertItemInfo;
                                                                DTInsertGroupInfo.Remove(item_Group);
                                                            }
                                                            //唯一标识
                                                            string strGuidN = Guid.NewGuid().ToString("N");
                                                           string testinfoSql = InsertTestSampleInfo(DTsampleInfo.Rows[0], strGuidN, receiveTime, groupcodess, groupNamess, "", InsertGroupInfo.groupNO, InsertGroupInfo.groupFlowNO, delGroupState, delegeteCompanyNO, infos.UserName);
                                                          sqlitem2 += testinfoSql + "set @testid=@@IDENTITY;" + sqliteminfo;
                                                        }
                                                    }
                                                    //更新条码收费价格
                                                    string updateApplyBillInfo = $"update Finance.ApplyBillInfo set charge=charge+{chargeCount},settlementCharge=settlementCharge+{settlementChargeCount},standerCharge=standerCharge+{standerchargeCount} where perid={testInfo.perid}  and dstate=0;";

                                                    //string sqla = "set @testid=@@IDENTITY;"
                                                    string sqlAll = "declare @testid varchar(500)" + sqlitem2 + sqlitem1 + updateApplyBillInfo;

                                                    await DbClient.Ado.ExecuteCommandAsync(sqlAll);
                                                    jm.code = 0;
                                                    jm.msg = "增项成功";
                                                }
                                                else
                                                {
                                                    jm.code = 1;
                                                    jm.msg = $"此样本没有可退组合项目信息";
                                                }
                                            }
                                            else
                                            {
                                                jm.code = 1;
                                                jm.msg = $"此样本未录入组套信息";
                                            }
                                        }
                                        else
                                        {
                                            jm.code = 1;
                                            jm.msg = $"该信息未审核，请通过标本录入进行项目修改";
                                        }
                                    }
                                    else
                                    {
                                        jm.code = 1;
                                        jm.msg = $"该信息未审核，请通过标本录入进行项目修改";
                                    }

                                }
                                else
                                {
                                    jm.code = 1;
                                    jm.msg = $"增项信息客户编号不能为空";
                                }
                            }
                            else
                            {
                                jm.code = 1;
                                jm.msg = $"未找到需要增项的样本信息";
                            }
                        }
                        else
                        {
                            jm.code = 1;
                            jm.msg = $"未检测到需要增项的项目信息";
                        }
                    }
                }
                catch (Exception ex)
                {
                    jm.code = 1;
                    jm.msg = ex.Message;
                }
            }
            return jm;
            #region
            //WebApiCallBack jm = new WebApiCallBack();
            //using (await _mutex.LockAsync())
            //{
            //    try
            //    {
            //        List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
            //        foreach (TesthandleModel testInfo in infos.infos)
            //        {
            //            commReSampleInfo reSampleInfo = new commReSampleInfo();
            //            string returnMsg = "";

            //            if (testInfo.submitItemCodes != null)
            //            {
            //                DataTable DTsampleInfo = null;
            //                //string[] insertgroupCodes = groupCodes.Split(',');
            //                string sql = $"select * from WorkPer.SampleInfo where id='{testInfo.perid}' and dstate=0";
            //                //if(perid>1000000000)
            //                //{
            //                //    DTsampleInfo = SqlHelper.ExecuteDataset(CommonData.sqlconnW, CommandType.Text, sql);
            //                //}
            //                //else
            //                //{
            //                //    DTsampleInfo = DbClient.Ado.GetDataTable( sql);
            //                //}
            //                DTsampleInfo = DbClient.Ado.GetDataTable(sql);
            //                decimal standerchargeCount = 0;//标准收费总金额
            //                decimal settlementChargeCount = 0;//结算收费总金额
            //                decimal chargeCount = 0;//实际收费总金额
            //                if (DTsampleInfo.Rows.Count == 1)
            //                {

            //                    string agentNO = DTsampleInfo.Rows[0]["agentNO"] == DBNull.Value ? "" : DTsampleInfo.Rows[0]["agentNO"].ToString();
            //                    string hospitalNO = DTsampleInfo.Rows[0]["hospitalNO"] == DBNull.Value ? "" : DTsampleInfo.Rows[0]["hospitalNO"].ToString();
            //                    string patientType = DTsampleInfo.Rows[0]["patientTypeNO"] == DBNull.Value ? "" : DTsampleInfo.Rows[0]["patientTypeNO"].ToString();
            //                    string department = DTsampleInfo.Rows[0]["department"] == DBNull.Value ? "" : DTsampleInfo.Rows[0]["department"].ToString();
            //                    string barcode = DTsampleInfo.Rows[0]["barcode"] == DBNull.Value ? "" : DTsampleInfo.Rows[0]["barcode"].ToString();
            //                    string receiveTime = DTsampleInfo.Rows[0]["receiveTime"] == DBNull.Value ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : DTsampleInfo.Rows[0]["receiveTime"].ToString();
            //                    //string perid = DTsampleInfo.Rows[0]["perid"] == DBNull.Value ? "0" : DTsampleInfo.Rows[0]["perid"].ToString();
            //                    if (hospitalNO != "")
            //                    {
            //                        FinanceClientModel financeClient = await _financeInfoRepository.GetFinanceClient(hospitalNO);//获取医院收费信息
            //                        string samplePerid = DTsampleInfo.Rows[0]["id"].ToString();
            //                        if (DTsampleInfo.Rows[0]["perStateNO"] != DBNull.Value)
            //                        {


            //                            if (DTsampleInfo.Rows[0]["perStateNO"].ToString() != "1")
            //                            {

            //                                string sqlApply = $"select groupCodes from WorkTest.SampleInfo where perid='{testInfo.perid}' and dstate=0;";
            //                                DataTable DTApplyInfo = DbClient.Ado.GetDataTable(sqlApply);
            //                                if (DTApplyInfo.Rows.Count > 0)
            //                                {
            //                                    string InfoGroupCodes = "";//已有组合项目编号集合
            //                                    foreach (DataRow applyRow in DTApplyInfo.Rows)
            //                                    {
            //                                        if (applyRow["groupCodes"] != DBNull.Value)
            //                                        {
            //                                            InfoGroupCodes += applyRow["groupCodes"].ToString();
            //                                        }
            //                                    }

            //                                    //string sqlGroup = $"select * from WorkComm.ItemGroup where no in ({InfoGroupCodes.Substring(0, InfoGroupCodes.Length - 1)}) and dstate=0 order by groupNO,groupFlowNO;";
            //                                    //string AddsqlGroup = $"select * from WorkComm.ItemGroup where no in ({groupCodes}) and dstate=0 order by groupNO,groupFlowNO;";
            //                                    //DataSet DSGroupInfo = DbClient.Ado.GetDataTable( sqlGroup + AddsqlGroup);//获取组合项目信息
            //                                    //DataTable DTOldGroupInfo = DSGroupInfo;
            //                                    //DataTable DTInsertGroupInfo = DSGroupInfo.Tables[1];


            //                                    //IRedisCache redisCache = new RedisCache();
            //                                    //DataTable DTGroupInfosss = redisCache.dataSet(CommonData.ItemGroup);
            //                                    //DataTable DTOldGroupInfo = DTGroupInfosss.Select($"no in ({groupCodes.Substring(0, groupCodes.Length - 1)}) and dstate=0", "groupNO,groupFlowNO").CopyToDataTable();
            //                                    //DataTable DTInsertGroupInfo = DTGroupInfosss.Select($"no in ({groupCodes}) and dstate=0", "groupNO,groupFlowNO").CopyToDataTable();


            //                                    string[] groupcodes = testInfo.submitItemCodes.Split(",");
            //                                    //读取内存中的组和信息
            //                                    List<comm_item_group> GroupInfoss = ManualDataCache<comm_item_group>.MemoryCache.LIMSGetKeyValue(CommInfo.itemgroup);
            //                                    //查询样本信息中包含的组和信息 并按照专业组进行排序
            //                                    List<comm_item_group> DTOldGroupInfo = GroupInfoss.Where(p => groupcodes.Contains(p.no.ToString()) && p.dstate == false).OrderBy(p => p.groupNO).ToList();
            //                                    List<comm_item_group> DTInsertGroupInfo = GroupInfoss.Where(p => groupcodes.Contains(p.no.ToString()) && p.dstate == false).OrderBy(p => p.groupNO).ToList();



            //                                    if (DTInsertGroupInfo != null)
            //                                    {
            //                                        string listItemsCodess = "";
            //                                        string listFlowCodes = "";
            //                                        foreach (comm_item_group DRInsertGroupInfo in DTInsertGroupInfo)
            //                                        {
            //                                            //string InsertgroupCode = DRInsertGroupInfo["no"] != DBNull.Value ? DRInsertGroupInfo["no"].ToString() : "oldgroupCode";
            //                                            //DataRow[] DrOldGroup = DTOldGroupInfo.Select($"no={InsertgroupCode}");
            //                                            //if (DrOldGroup.Length==0)//样本信息包含的组合项目不在新增
            //                                            //{
            //                                            if (DRInsertGroupInfo.testItemList != null)
            //                                            {
            //                                                listItemsCodess += DRInsertGroupInfo.testItemList.ToString();
            //                                            }
            //                                            if (DRInsertGroupInfo.groupFlowNO == null || DRInsertGroupInfo.groupFlowNO.ToString() == "")
            //                                            {
            //                                                DRInsertGroupInfo.groupFlowNO = "1";
            //                                                listFlowCodes += "1,";
            //                                            }
            //                                            else
            //                                            {
            //                                                listFlowCodes += DRInsertGroupInfo.groupFlowNO.ToString() + ",";
            //                                            }
            //                                            //}
            //                                        }
            //                                        if (listItemsCodess != "")
            //                                        {

            //                                            //查询所有子项信息
            //                                            listItemsCodess = listItemsCodess.Substring(0, listItemsCodess.Length - 1);
            //                                            List<string> listItemsCodes = listItemsCodess.Split(",").Distinct().ToList();


            //                                            //DataTable DTItemTestsss = redisCache.dataSet(CommonData.ItemTest);
            //                                            //DataTable DTItemInfo = DTItemTestsss.Select($"no in ({listItemsCodes})", "groupNO").CopyToDataTable();


            //                                            ////string ItemFlowString = RedisHelper.GetStringValue("ItemFlow");
            //                                            ////DataTable DTItemFlowsss = ConvertDatatable.StringToDataTable(ItemFlowString);


            //                                            ////IRedisCache redisCache = new RedisCache();
            //                                            //DataTable DTItemFlowsss = redisCache.dataSet(CommonData.ItemFlow);
            //                                            //DataTable DTFlowInfo = DTItemFlowsss.Select($"no in ({listFlowCodes.Substring(0, listFlowCodes.Length - 1)})").CopyToDataTable();



            //                                            //读取子项信息获取子项集合
            //                                            List<comm_item_test> TestInfoss = ManualDataCache<comm_item_test>.MemoryCache.LIMSGetKeyValue(CommInfo.itemtest);
            //                                            //查询样本信息中包含的子项信息 并按照专业组进行排序
            //                                            List<comm_item_test> TestInfos = TestInfoss.Where(p => listItemsCodes.Contains(p.no.ToString()) && p.dstate == false).OrderBy(p => p.groupNO).ToList();


            //                                            //读取子项信息获取流程集合
            //                                            List<comm_item_flow> FlowInfoss = CommInfo.itemflow.data
            //                                            //查询样本信息中包含的流程信息 并按照专业组进行排序
            //                                            List<comm_item_flow> FlowInfos = FlowInfoss.Where(p => listItemsCodes.Contains(p.no.ToString()) && p.dstate == false).ToList();










            //                                            string insertApplyPirceInfo = "";

            //                                            //string insertSampleInfo = "";
            //                                            //string insertItemSql = "";


            //                                            //string reflectionFile = "";//反射文件名称
            //                                            //string reflectionFrm = ""; //反射窗体名称

            //                                            List<CheckGroupBarcode> checkBarcodes = new List<CheckGroupBarcode>();
            //                                            if (TestInfos.Count > 0)
            //                                            {
            //                                                string insertSampleInfoAll = "";
            //                                                string insertGroupInfosWx = "";//专业组信息和项目信息插入方法
            //                                                string groupFlowNO = "";//组合项目流程编号
            //                                                string groupNO = "";//专业组编号
            //                                                string InsertGroupCodes = "";
            //                                                string InsertGroupNames = "";
            //                                                bool sampleState = false;//判断信息是否全部正确
            //                                                                         //bool groupFlowState = true;//获取组流程默认状态
            //                                                foreach (comm_item_group DRInsertGroupInfo in DTInsertGroupInfo)
            //                                                {
            //                                                    string InsertGroupCode = DRInsertGroupInfo.no != null ? DRInsertGroupInfo.no.ToString() : "NullgroupCode";
            //                                                    string InsertGroupName = DRInsertGroupInfo.names != null ? DRInsertGroupInfo.names.ToString() : "NullgroupCode";
            //                                                    //InsertGroupCodes += InsertGroupCode+",";
            //                                                    //InsertGroupNames += InsertGroupName + ",";

            //                                                    string insertGroupInfos = "";//专业组信息和项目信息插入方法
            //                                                    string insertGroupPirceInfo = "";
            //                                                    string groupItemGroupNO = DRInsertGroupInfo.groupNO != null ? DRInsertGroupInfo.groupNO.ToString() : "";
            //                                                    string testFlowNO = DRInsertGroupInfo.groupFlowNO != null ? DRInsertGroupInfo.groupFlowNO.ToString() : "1";

            //                                                    if (DTOldGroupInfo.Any(p => p.no == Convert.ToInt32(InsertGroupCode)))///判断组合项目是否存在
            //                                                    {


            //                                                        if (DRInsertGroupInfo.groupNO != null && DRInsertGroupInfo.groupNO.ToString() != "")
            //                                                        {

            //                                                            //string groupItemGroupNO = "";
            //                                                            //string testFlowNO = "";
            //                                                            string reflectionFile = "";//反射文件名称
            //                                                            string reflectionFrm = ""; //反射窗体名称
            //                                                            string dataSource = "";    //反射数据源
            //                                                            string imgSource = "";     //反射图片源
            //                                                            bool delGroupState = DRInsertGroupInfo.delegeteState != null ? Convert.ToBoolean(DRInsertGroupInfo.delegeteState) : false;//判断组合项目样本委托状态
            //                                                            string delGroupClientNO = delGroupState && DRInsertGroupInfo.delegeteState != null ? DRInsertGroupInfo.delegeteState.ToString() : "";//获取组合项目委托单位编号
            //                                                                                                                                                                                                 //string bbb = DRgroupInfo.groupFlowNO.ToString();

            //                                                            if (groupFlowNO != testFlowNO || groupNO != groupItemGroupNO)
            //                                                            {


            //                                                                comm_item_flow DRFlowInfo = FlowInfos.First(p => p.no == Convert.ToInt32(DRInsertGroupInfo.groupFlowNO));
            //                                                                groupNO = DRInsertGroupInfo.groupNO.ToString();
            //                                                                groupFlowNO = DRFlowInfo.no != null ? DRFlowInfo.no.ToString() : "0";
            //                                                                if (groupFlowNO != "0")
            //                                                                    reflectionFile = DRFlowInfo.reflectionFile != null ? DRFlowInfo.reflectionFile.ToString() : "";
            //                                                                if (reflectionFile != "")
            //                                                                    reflectionFrm = DRFlowInfo.reflectionFrm != null ? DRFlowInfo.reflectionFrm.ToString() : "";
            //                                                                if (reflectionFrm != "")
            //                                                                    dataSource = DRFlowInfo.dataSource != null ? DRFlowInfo.dataSource.ToString() : "";
            //                                                                if (dataSource != "")
            //                                                                    imgSource = DRFlowInfo.imgSource != null ? DRFlowInfo.imgSource.ToString() : "";

            //                                                                ///判断是否读取的反射信息
            //                                                                if (reflectionFile != "" && reflectionFrm != "" && dataSource != "")
            //                                                                {



            //                                                                    string sampleInfosql = $"select * from WorkTest.SampleInfo where perid={testInfo.perid} and groupNO='{groupNO}' and groupFlowNO='{groupFlowNO}' and dstate=0;";
            //                                                                    DataTable DTsampleInfoa = DbClient.Ado.GetDataTable(sampleInfosql);//判断专业组信息是否存在
            //                                                                    string testinfoid = "";
            //                                                                    string sampleid = "";
            //                                                                    if (DTsampleInfoa.Rows.Count > 0)
            //                                                                    {
            //                                                                        testinfoid = DTsampleInfoa.Rows[0]["id"] != DBNull.Value ? DTsampleInfoa.Rows[0]["id"].ToString() : "";
            //                                                                        sampleid = DTsampleInfoa.Rows[0]["sampleID"] != DBNull.Value ? DTsampleInfoa.Rows[0]["sampleID"].ToString() : "";
            //                                                                    }

            //                                                                    //DataRow[] groupItemInfos = DTGroupInfo.Select($"groupNO='{DRgroupInfo.groupNO"].ToString()}' and groupFlowNO='{groupFlowNO}' and state=1", "sort");
            //                                                                    //DataRow[] DRInsertGroupItems = DTInsertGroupInfo.Select($"groupNO='{groupNO}' and groupFlowNO='{groupFlowNO}' and state=1", "sort");
            //                                                                    //DataRow[] DRInsertGroupItems = DTInsertGroupInfo.Select($"groupNO='{groupNO}' and groupFlowNO='{groupFlowNO}' and state=1", "sort");
            //                                                                    List<comm_item_group> DRInsertGroupItems = DTInsertGroupInfo.Where(p => p.groupNO == groupNO && p.groupFlowNO == groupFlowNO && p.state == true).OrderBy(p => p.sort).ToList();

            //                                                                    if (DRInsertGroupItems.Count > 0)
            //                                                                    {
            //                                                                        string grouCodes = "";//组合项目编号集合
            //                                                                        string groupNames = "";//组合项目名称集合
            //                                                                        string insertItemInfo = "";
            //                                                                        int ItemSort = 1;
            //                                                                        //if (groupFlowNO == "1")//获取项目排序号
            //                                                                        //{
            //                                                                        //    ItemSort += DTOldGroupInfo.Where($"groupNO='{groupNO}' and (groupFlowNO='1' or groupFlowNO is null)  and state=1", "sort").Length;

            //                                                                        //}
            //                                                                        //else
            //                                                                        //{
            //                                                                        //    ItemSort += DTOldGroupInfo.Select($"groupNO='{groupNO}' and groupFlowNO='{groupFlowNO}' and state=1", "sort").Length;
            //                                                                        //}
            //                                                                        ItemSort += DTOldGroupInfo.Where(p => p.groupNO == groupNO && p.groupFlowNO == groupFlowNO && p.state == true).Count();
            //                                                                        string strGuidN = "";
            //                                                                        foreach (comm_item_group DRInsertGroupItem in DRInsertGroupItems)
            //                                                                        {
            //                                                                            strGuidN = Guid.NewGuid().ToString("N");
            //                                                                            DiscountPriceModel GroupPriceinfo = await _financeInfoRepository.GetGroupPrice(agentNO, hospitalNO, patientType, department, DRInsertGroupItem.no.ToString(), financeClient.chargeLevelNO, financeClient.discount);

            //                                                                            insertGroupPirceInfo = "insert into Finance.GroupBillInfo (perid,testid,barcode,tabTypeNO,groupNO,chargeTypeNO,chargeLevelNO,discountState,discount,personNO,groupCode,groupName,operater,operatTime,standerCharge,settlementCharge,charge,dstate)";
            //                                                                            insertGroupPirceInfo += $" values ('{DTsampleInfoa.Rows[0]["perid"]}',@testid,'{DTsampleInfoa.Rows[0]["barcode"]}','1','{groupNO}','{GroupPriceinfo.chargeTypeNO}','{financeClient.chargeLevelNO}','{GroupPriceinfo.groupDiscountState}','{financeClient.discount}','{financeClient.personNO}','{DRInsertGroupItem.no}','{DRInsertGroupItem.names}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{GroupPriceinfo.standerPirce}','{GroupPriceinfo.settlementPirce}','{GroupPriceinfo.chargePice}',0);";



            //                                                                            settlementChargeCount += GroupPriceinfo.settlementPirce;
            //                                                                            standerchargeCount += GroupPriceinfo.standerPirce;
            //                                                                            chargeCount += GroupPriceinfo.chargePice;

            //                                                                            grouCodes += DRInsertGroupItem.no != null ? DRInsertGroupItem.no.ToString() + "," : "";
            //                                                                            groupNames += DRInsertGroupItem.names != null ? DRInsertGroupItem.names.ToString() + "," : "";
            //                                                                            if (DRInsertGroupItem.testItemList != null && DRInsertGroupItem.testItemList.ToString() != "")
            //                                                                            {
            //                                                                                string itemList = DRInsertGroupItem.testItemList.ToString();
            //                                                                                itemList = itemList.Substring(0, itemList.Length - 1);
            //                                                                                string[] itemArry = itemList.Split(',');
            //                                                                                if (delGroupState)
            //                                                                                {

            //                                                                                    foreach (string itemCode in itemArry)
            //                                                                                    {
            //                                                                                        ItemSort += 1;
            //                                                                                        comm_item_test itemInfo = TestInfos.First(p => p.no == Convert.ToInt32(itemCode));
            //                                                                                        bool delItemState = itemInfo.delegeteState != null ? Convert.ToBoolean(itemInfo.delegeteState) : false;
            //                                                                                        string delItemClientNO = delItemState && itemInfo.delegeteCompanyNO != null ? itemInfo.delegeteCompanyNO.ToString() : "";
            //                                                                                        if (delItemState)
            //                                                                                        {
            //                                                                                            insertItemInfo += $"insert into {dataSource} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate)";
            //                                                                                            if (testinfoid == "")
            //                                                                                            {
            //                                                                                                insertItemInfo += $"values ({testInfo.perid},@testid,'{barcode}','{groupNO}','{DRInsertGroupItem.no}','{DRInsertGroupItem.names}','{itemInfo.no}','{itemInfo.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
            //                                                                                            }
            //                                                                                            else
            //                                                                                            {
            //                                                                                                insertItemInfo += $"values ({testInfo.perid},'{testinfoid}','{barcode}','{groupNO}','{DRInsertGroupItem.no}','{DRInsertGroupItem.names}','{itemInfo.no}','{itemInfo.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
            //                                                                                            }

            //                                                                                        }
            //                                                                                        else
            //                                                                                        {
            //                                                                                            insertItemInfo += $"insert into {dataSource} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate)";
            //                                                                                            if (testinfoid == "")
            //                                                                                            {
            //                                                                                                insertItemInfo += $"values ({testInfo.perid},@testid,'{barcode}','{groupNO}','{DRInsertGroupItem.no}','{DRInsertGroupItem.names}','{itemInfo.no}','{itemInfo.names}','{ItemSort}','{delGroupState}','{delGroupClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
            //                                                                                            }
            //                                                                                            else
            //                                                                                            {
            //                                                                                                insertItemInfo += $"values ({testInfo.perid},'{testinfoid}','{barcode}','{groupNO}','{DRInsertGroupItem.no}','{DRInsertGroupItem.names}','{itemInfo.no}','{itemInfo.names}','{ItemSort}','{delGroupState}','{delGroupClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
            //                                                                                            }

            //                                                                                        }


            //                                                                                    }
            //                                                                                }
            //                                                                                else
            //                                                                                {

            //                                                                                    foreach (string itemCode in itemArry)
            //                                                                                    {
            //                                                                                        ItemSort += 1;
            //                                                                                        comm_item_test itemInfo = TestInfos.First(p => p.no == Convert.ToInt32(itemCode));
            //                                                                                        bool delItemState = itemInfo.delegeteState != null ? Convert.ToBoolean(itemInfo.delegeteState) : false;
            //                                                                                        if (delItemState)
            //                                                                                        {
            //                                                                                            delGroupState = true;
            //                                                                                        }
            //                                                                                        string delItemClientNO = delItemState && itemInfo.delegeteCompanyNO != null ? itemInfo.delegeteCompanyNO.ToString() : "";
            //                                                                                        insertItemInfo += $"insert into {dataSource} (perid, testid, barcode, groupNO, groupCode, groupName, itemCodes, itemNames, itemSort,delstate,delstateClientNO,creater, createTime, state, dstate)";
            //                                                                                        if (testinfoid == "")
            //                                                                                        {
            //                                                                                            insertItemInfo += $"values ({testInfo.perid},@testid,'{barcode}','{groupNO}','{DRInsertGroupItem.no}','{DRInsertGroupItem.names}','{itemInfo.no}','{itemInfo.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
            //                                                                                        }
            //                                                                                        else
            //                                                                                        {
            //                                                                                            insertItemInfo += $"values ({testInfo.perid},'{testinfoid}','{barcode}','{groupNO}','{DRInsertGroupItem.no}','{DRInsertGroupItem.names}','{itemInfo.no}','{itemInfo.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
            //                                                                                        }
            //                                                                                        //insertItemInfo += $"values ({testInfo.perid},@testid,'{barcode}','{groupNO}','{DRgroupItem["no"]}','{DRgroupItem["names"]}','{itemInfo.no}','{itemInfo.names}','{ItemSort}','{delItemState}','{delItemClientNO}','{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',1,0);";
            //                                                                                    }
            //                                                                                }
            //                                                                            }
            //                                                                        }

            //                                                                        if (insertItemInfo != "")
            //                                                                        {



            //                                                                            string updateApplyBillInfo = $"update Finance.ApplyBillInfo set charge=charge+{chargeCount},settlementCharge=settlementCharge+{settlementChargeCount},standerCharge=standerCharge+{standerchargeCount} where perid={testInfo.perid}  and dstate=0;";
            //                                                                            //string updateTestInfo = $"update WorkTest.SampleInfo set groupCodes=groupCodes+{},settlementCharge=settlementCharge+{settlementChargeCount},standerCharge=standerCharge+{standerchargeCount} where perid={testInfo.perid}  and dstate=0;";
            //                                                                            //string UpdateGroupInfos = "";


            //                                                                            if (testinfoid == "")
            //                                                                            {
            //                                                                                //insertGroupInfos += InsertTestSampleInfo(DTsampleInfo.Rows[0], grouCodes, groupNames, "", groupNO, groupFlowNO, delGroupState, delGroupClientNO, infos.UserName) + "set @testid=@@IDENTITY;";
            //                                                                                insertGroupInfos += InsertTestSampleInfo(DTsampleInfo.Rows[0], strGuidN, receiveTime, grouCodes, groupNames, "", groupNO, groupFlowNO, delGroupState, delGroupClientNO, infos.UserName);
            //                                                                            }
            //                                                                            else
            //                                                                            {
            //                                                                                insertGroupInfos += $"update WorkTest.SampleInfo set tabTypeNO=2,groupCodes=groupCodes+'{grouCodes}',groupNames=groupNames+'{groupNames}' where sampleID='{sampleid}' and dstate=0;";
            //                                                                            }
            //                                                                            //insertGroupInfosWx += insertGroupInfos;
            //                                                                            insertSampleInfoAll += insertGroupInfos + insertItemInfo + updateApplyBillInfo + insertGroupPirceInfo;
            //                                                                            ////RecheckInfo.code = 1;
            //                                                                            //CheckGroupBarcode testInfoBarcode1 = new CheckGroupBarcode();
            //                                                                            //testInfoBarcode1.sampleID = perid;
            //                                                                            //testInfoBarcode1.groupNO = groupNO;
            //                                                                            //testInfoBarcode1.barcode = barcode;
            //                                                                            //testInfoBarcode1.groupCodes = grouCodes;
            //                                                                            //testInfoBarcode1.groupNames = groupNames;
            //                                                                            //checkBarcodes.Add(testInfoBarcode1);
            //                                                                            sampleState = true;
            //                                                                            jm.code = 0;
            //                                                                            jm.msg = "增项成功";
            //                                                                        }
            //                                                                        else
            //                                                                        {

            //                                                                            returnMsg += $"组合项目编号:{InsertGroupCode},没有获取到检验项目信息,请检查配置！";
            //                                                                            sampleState = false; ;
            //                                                                            break;
            //                                                                        }
            //                                                                    }
            //                                                                    else
            //                                                                    {
            //                                                                        //RecheckInfo.code = 0;
            //                                                                        //RecheckInfo.msg += $"组合项目编号:{InsertGroupCode},项目已停用或不存在,请检查配置！";
            //                                                                        returnMsg += $"组合项目编号:{InsertGroupCode},项目已停用或不存在,请检查配置！";
            //                                                                        sampleState = false; ;
            //                                                                        break;
            //                                                                    }
            //                                                                }
            //                                                                else
            //                                                                {
            //                                                                    //RecheckInfo.code = 0;
            //                                                                    //RecheckInfo.msg += $"组合项目编号:{InsertGroupCode},未指定流程信息,请检查配置！";
            //                                                                    returnMsg += $"组合项目编号:{InsertGroupCode},未配置流程信息,请检查配置！";
            //                                                                    sampleState = false; ;
            //                                                                    break;
            //                                                                }
            //                                                            }
            //                                                        }
            //                                                        else
            //                                                        {
            //                                                            //RecheckInfo.code = 0;
            //                                                            //RecheckInfo.msg += $"组合项目编号:{InsertGroupCode},未指定专业组信息,请检查配置！";
            //                                                            returnMsg += $"组合项目编号:{InsertGroupCode},未指定专业组信息,请检查配置！";
            //                                                            sampleState = false; ;
            //                                                            break;
            //                                                        }
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        returnMsg += $"组合项目编号:{InsertGroupCode},样本中存在该组合项目信息！";
            //                                                        sampleState = false; ;
            //                                                        break;
            //                                                    }
            //                                                }



            //                                                if (sampleState)
            //                                                {
            //                                                    int aaa = DbClient.Ado.ExecuteCommand("declare @testid varchar(500);" + insertSampleInfoAll);
            //                                                    //SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, insertGroupInfosWx);

            //                                                    if (aaa > 0)
            //                                                    {
            //                                                        jm.data = 0;
            //                                                        jm.otherData = 0;
            //                                                        jm.msg = $"增项成功";
            //                                                        _recordRepository.SampleRecord(barcode, RecordEnumVars.ItemAdd, $"{barcode}增项成功:新增项目编号:{InsertGroupCodes},新增项目名称:{InsertGroupNames}", infos.UserName, false);
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        jm.data = 0;
            //                                                        jm.otherData = 0;
            //                                                        jm.msg = $"增项失败";
            //                                                    }
            //                                                }
            //                                                else
            //                                                {
            //                                                    jm.data = 0;
            //                                                    jm.otherData = 0;
            //                                                    jm.msg = $"增项失败";
            //                                                }

            //                                                //}
            //                                            }
            //                                            else
            //                                            {
            //                                                jm.data = 0;
            //                                                jm.otherData = 0;
            //                                                jm.msg += $"未找到信息的项目信息！";
            //                                            }

            //                                        }
            //                                        else
            //                                        {
            //                                            jm.data = 0;
            //                                            jm.otherData = 0;
            //                                            jm.msg += $"未找到信息的项目信息！";
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        jm.data = 0;
            //                                        jm.otherData = 0;
            //                                        jm.msg += $"此样本没有可退组合项目信息";
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    jm.data = 0;
            //                                    jm.otherData = 0;
            //                                    jm.msg += $"此样本未录入组套信息";
            //                                }
            //                            }
            //                            else
            //                            {
            //                                jm.data = 0;
            //                                jm.otherData = 0;
            //                                jm.msg += $"该信息未审核，请通过标本录入进行项目修改";
            //                            }
            //                        }
            //                        else
            //                        {
            //                            jm.data = 0;
            //                            jm.otherData = 0;
            //                            jm.msg += $"该信息未审核，请通过标本录入进行项目修改";
            //                        }

            //                    }
            //                    else
            //                    {
            //                        jm.data = 0;
            //                        jm.otherData = 0;
            //                        jm.msg += $"请提交正确的信息ID";
            //                    }
            //                }
            //                else
            //                {
            //                    jm.data = 0;
            //                    jm.otherData = 0;
            //                    jm.msg += $"未找到需要修改的样本信息";
            //                }
            //            }
            //            else
            //            {
            //                jm.data = 0;
            //                jm.otherData = 0;
            //                jm.msg += $"未检测到需要增项的项目信息";
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        jm.data = 0;
            //        jm.otherData = 0;
            //        jm.msg = ex.Message;
            //    }
            //}
            //return jm;

            #endregion
        }
        /// <summary>
        /// 更新样本申请信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ReSpecialRecord(commInfoModel<TesthandleModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                if (infos.infos != null)
                {
                    try
                    {
                        foreach (TesthandleModel testInfo in infos.infos)
                        {
                            string sql = $"update WorkOther.SpecialRecord set  submitItemCodes='{testInfo.submitItemCodes}',submitItemNames='{testInfo.submitItemNames}',state='{testInfo.state}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                            int a = DbClient.Ado.ExecuteCommand(sql);
                            //string msg = "处理类型：拒绝退单处理";
                            jm.data = 1;
                            jm.otherData = 0;
                            //return msg;
                            jm.msg = "更新成功";
                        }
                    }
                    catch (Exception ex)
                    {
                        jm.data = 0;
                        jm.otherData = 0;
                        jm.msg = ex.Message;
                    }
                }
                else
                {
                    jm.data = 0;
                    jm.otherData = 0;
                    jm.msg = "提交数据为空";
                }
            }
            return jm;

        }
        /// <summary>
        /// 更新免疫组化信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ReIHCRecord(commInfoModel<TesthandleModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                if (infos.infos != null)
                {
                    try
                    {
                        foreach (TesthandleModel testInfo in infos.infos)
                        {
                            string ResultState = testInfo.state == true ? "1" : "0";
                            string sql = $"update WorkOther.IHCRecord set resultTypeNO='',submitItemCodes='{testInfo.submitItemCodes}',submitItemNames='{testInfo.submitItemNames}',state='{testInfo.state}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                            if (testInfo.state)
                                sql += $"update WorkTest.SampleInfo set  reportState=null,pathologyStateNO='5' where id='{testInfo.testid}';";

                            int a = DbClient.Ado.ExecuteCommand(sql);
                            //string msg = "处理类型：拒绝退单处理";
                            jm.data = 1;
                            jm.otherData = 0;
                            //return msg;
                            jm.msg = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        jm.data = 0;
                        jm.otherData = 0;
                        jm.msg = ex.Message;
                    }
                }
                else
                {
                    jm.data = 0;
                    jm.otherData = 0;
                    jm.msg = "提交数据为空";
                }
            }
            return jm;
        }


        /// <summary>
        /// <summary>
        /// 插入检验表样本信息
        /// </summary>
        /// <param name="DRsampleInfo">录入样本信息表</param>
        /// <param name="GgrouCodes">同一个工作组组合项目编号集合</param>
        /// <param name="GgroupNames">同一个工作组组合项目名称集合</param>
        /// <param name="GgroupNO">专业组编号</param>
        /// <param name="UserName">创建人</param>
        /// <returns></returns>
        private static string InsertTestSampleInfo(DataRow DRsampleInfo, string sampleInfoId, string receiveTime, string GgrouCodes, string GgroupNames, string sampleID, string GgroupNO, string groupFlowNO, bool delGroupState, string delGroupClientNO, string UserName)
        {
            iInfo insertInfo = new iInfo();
            insertInfo.TableName = "WorkTest.SampleInfo";
            Dictionary<string, object> pairsInfo = new Dictionary<string, object>();
            pairsInfo.Add("perid", DRsampleInfo["id"]);
            pairsInfo.Add("sampleID", sampleInfoId);
            pairsInfo.Add("disState", 0);
            pairsInfo.Add("dstate", 0);
            pairsInfo.Add("state", 0);
            pairsInfo.Add("urgent", DRsampleInfo["urgent"]);
            pairsInfo.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            pairsInfo.Add("receiveTime", receiveTime);
            pairsInfo.Add("sampleTime", DRsampleInfo["sampleTime"]);
            pairsInfo.Add("ageDay", DRsampleInfo["ageDay"]);
            pairsInfo.Add("ageMoth", DRsampleInfo["ageMoth"]);
            pairsInfo.Add("pathologyStateNO", 1);
            pairsInfo.Add("testStateNO", 1);
            pairsInfo.Add("patientName", DRsampleInfo["patientName"]);
            pairsInfo.Add("agentNO", DRsampleInfo["agentNO"]);
            pairsInfo.Add("ageYear", DRsampleInfo["ageYear"]);
            pairsInfo.Add("applyItemCodes", DRsampleInfo["applyItemCodes"]);
            pairsInfo.Add("applyItemNames", DRsampleInfo["applyItemNames"]);
            pairsInfo.Add("barcode", DRsampleInfo["barcode"]);
            pairsInfo.Add("bedNo", DRsampleInfo["bedNo"]);
            pairsInfo.Add("clinicalDiagnosis", DRsampleInfo["clinicalDiagnosis"]);
            pairsInfo.Add("creater", UserName);
            pairsInfo.Add("cutPart", DRsampleInfo["cutPart"]);
            pairsInfo.Add("department", DRsampleInfo["department"]);
            pairsInfo.Add("doctorPhone", DRsampleInfo["doctorPhone"]);
            pairsInfo.Add("groupCodes", GgrouCodes);
            pairsInfo.Add("groupNames", GgroupNames);
            pairsInfo.Add("hospitalBarcode", DRsampleInfo["hospitalBarcode"]);
            pairsInfo.Add("frameNo", DRsampleInfo["frameNo"]);
            pairsInfo.Add("hospitalNames", DRsampleInfo["hospitalNames"]);
            pairsInfo.Add("hospitalNO", DRsampleInfo["hospitalNO"]);
            pairsInfo.Add("medicalNo", DRsampleInfo["medicalNo"]);
            pairsInfo.Add("menstrualTime", DRsampleInfo["menstrualTime"]);
            pairsInfo.Add("number", DRsampleInfo["number"]);
            pairsInfo.Add("passportNo", DRsampleInfo["passportNo"]);
            pairsInfo.Add("pathologyNo", DRsampleInfo["pathologyNo"]);
            pairsInfo.Add("patientAddress", DRsampleInfo["patientAddress"]);
            pairsInfo.Add("patientCardNo", DRsampleInfo["patientCardNo"]);
            pairsInfo.Add("patientPhone", DRsampleInfo["patientPhone"]);
            pairsInfo.Add("patientSexNames", DRsampleInfo["patientSexNames"]);
            pairsInfo.Add("patientSexNO", DRsampleInfo["patientSexNO"]);
            pairsInfo.Add("patientTypeNames", DRsampleInfo["patientTypeNames"]);
            pairsInfo.Add("patientTypeNO", DRsampleInfo["patientTypeNO"]);
            pairsInfo.Add("perRemark", DRsampleInfo["perRemark"]);
            pairsInfo.Add("sampleAddress", DRsampleInfo["sampleAddress"]);
            pairsInfo.Add("sampleLocation", DRsampleInfo["sampleLocation"]);
            pairsInfo.Add("sampleShapeNames", DRsampleInfo["sampleShapeNames"]);
            pairsInfo.Add("sampleShapeNO", DRsampleInfo["sampleShapeNO"]);
            pairsInfo.Add("sampleTypeNames", DRsampleInfo["sampleTypeNames"]);
            pairsInfo.Add("sampleTypeNO", DRsampleInfo["sampleTypeNO"]);
            pairsInfo.Add("sendDoctor", DRsampleInfo["sendDoctor"]);
            pairsInfo.Add("groupNO", GgroupNO);
            pairsInfo.Add("groupFlowNO", groupFlowNO);
            pairsInfo.Add("delegateState", delGroupState);
            pairsInfo.Add("delstateClientNO", delGroupClientNO);
            pairsInfo.Add("tabTypeNO", 1);
            pairsInfo.Add("sortState", 0);
            insertInfo.values = pairsInfo;
            string aaa = SqlFormartHelper.insertFormart(insertInfo);

            return aaa;

        }

    }
}
