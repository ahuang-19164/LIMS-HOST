using Nito.AsyncEx;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.Finance.IRepository;
using Yichen.Manage.IRepository;
using Yichen.Manage.IServices;
using Yichen.Other.IRepository;
using Yichen.Test.Model;
using Yichen.Net.Configuration;
using Yichen.Other.IServices;

namespace Yichen.Manage.Services
{
    public class SampleHandleServices : BaseServices<object>, ISampleHandleServices
    {

        private readonly AsyncLock _mutex = new AsyncLock();
        public readonly IUnitOfWork _UnitOfWork;
        public readonly IRecordRepository _recordRepository;
        public readonly IFinanceInfoRepository _financeInfoRepository;
        public readonly ISampleHandleRepository _sampleHandleRepository;
        //public readonly ICRMHandleRepository _crmHandleRepository;
        public SampleHandleServices(IUnitOfWork unitOfWork
            , IRecordRepository recordRepository
            , IFinanceInfoRepository financeInfoRepository
            , ISampleHandleRepository sampleHandleRepository
            //, ICRMHandleRepository crmHandleRepository
            )
        {
            _UnitOfWork = unitOfWork;
            _recordRepository = recordRepository;
            _financeInfoRepository = financeInfoRepository;
            _sampleHandleRepository = sampleHandleRepository;
            //_crmHandleRepository = crmHandleRepository;
        }

        /// <summary>
        /// 样本信息特殊处理信息提交
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SampleHandle(commInfoModel<TesthandleModel> infos)
        {

            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                try
                {


                    if (infos.infos != null && infos.infos.Count > 0)
                    {
                        commReInfo<commReSampleInfo> commReInfo = new commReInfo<commReSampleInfo>();
                        List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                        if (infos.state == 1)//延迟操作
                        {

                            foreach (TesthandleModel info in infos.infos)
                            {
                                commReSampleInfo reSampleInfo = new commReSampleInfo();
                                reSampleInfo.testid = info.testid;
                                reSampleInfo.barcode = info.barcode;
                                //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                //commInfo.UserName = infos.UserName;
                                //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                //testhandles.Add(info);
                                //commInfo.infos = testhandles;

                                WebApiCallBack handleInfo = await _sampleHandleRepository.InfoDelay(infos);
                                reSampleInfo.msg = handleInfo.msg;
                                reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                reSampleInfo.testState = Convert.ToInt32(handleInfo.otherData);
                                reSampleInfos.Add(reSampleInfo);
                                //_recordRepository.SampleRecord(info.barcode, "延迟处理", $"拒绝增项，项目：[{info.submitItemCodes}][{info.submitItemNames}]", infos.UserName, false);

                            }

                        }
                        if (infos.state == 2)//样本重采
                        {

                            foreach (TesthandleModel info in infos.infos)
                            {
                                commReSampleInfo reSampleInfo = new commReSampleInfo();
                                reSampleInfo.testid = info.testid;
                                reSampleInfo.barcode = info.barcode;
                                ////reSampleInfo.msg = TestItemHandle.InfoAgin(info, infos.UserName, out int handleState);
                                ////reSampleInfo.msg = TestItemHandle.InfoAgin(info, infos.UserName, out int handleState, out int testStateNO);
                                //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                //commInfo.UserName = infos.UserName;
                                //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                //testhandles.Add(info);
                                //commInfo.infos = testhandles;

                                WebApiCallBack handleInfo = await _sampleHandleRepository.InfoAgin(infos);
                                reSampleInfo.msg = handleInfo.msg;
                                reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                reSampleInfo.testState = Convert.ToInt32(handleInfo.otherData);
                                reSampleInfos.Add(reSampleInfo);
                                //_recordRepository.SampleRecord(info.barcode, "延迟处理", $"同意增项，项目：[{info.submitItemCodes}][{info.submitItemNames}]", infos.UserName, false);
                            }

                        }
                        if (infos.state == 3)//样本退单
                        {

                            foreach (TesthandleModel info in infos.infos)
                            {
                                commReSampleInfo reSampleInfo = new commReSampleInfo();
                                reSampleInfo.testid = info.testid;
                                reSampleInfo.barcode = info.barcode;
                                ////reSampleInfo.msg = TestItemHandle.Infoback(info, infos.UserName, out int handleState);
                                ////reSampleInfo.msg = TestItemHandle.Infoback(info, infos.UserName, out int handleState, out int testStateNO);
                                //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                //commInfo.UserName = infos.UserName;
                                //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                //testhandles.Add(info);
                                //commInfo.infos = testhandles;

                                WebApiCallBack handleInfo = await _sampleHandleRepository.Infoback(infos);
                                reSampleInfo.msg = handleInfo.msg;
                                reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                reSampleInfo.testState = Convert.ToInt32(handleInfo.otherData);
                                reSampleInfos.Add(reSampleInfo);
                            }

                        }
                        if (infos.state == 4)//增项申请
                        {



                            foreach (TesthandleModel info in infos.infos)
                            {
                                if (info.perid != 0)
                                {
                                    if (info.handleTypeNO == "2")
                                    {
                                        if (info.state)
                                        {
                                            commReSampleInfo reSampleInfo = new commReSampleInfo();
                                            reSampleInfo.testid = info.testid;
                                            reSampleInfo.barcode = info.barcode;

                                            //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                            //commInfo.UserName = infos.UserName;
                                            //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                            //testhandles.Add(info);
                                            //commInfo.infos = testhandles;
                                            await _sampleHandleRepository.ReSpecialRecord(infos);
                                            WebApiCallBack handleInfo = await _sampleHandleRepository.GorupItemAdd(infos);
                                            reSampleInfo.msg = handleInfo.msg;
                                            reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                            reSampleInfo.testState = Convert.ToInt32(handleInfo.otherData);
                                            reSampleInfos.Add(reSampleInfo);



                                            await _recordRepository.SampleRecord(info.barcode, RecordEnumVars.ItemAdd, $"同意增项，项目：[{info.submitItemCodes}][{info.submitItemNames}]", infos.UserName, true);




                                        }
                                        else
                                        {
                                            commReSampleInfo reSampleInfo = new commReSampleInfo();
                                            reSampleInfo.testid = info.testid;
                                            reSampleInfo.barcode = info.barcode;

                                            //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                            //commInfo.UserName = infos.UserName;
                                            //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                            //testhandles.Add(info);
                                            //commInfo.infos = testhandles;
                                            WebApiCallBack handleInfo = await _sampleHandleRepository.ReSpecialRecord(infos);
                                            //WebApiCallBack DelayInfo = await GorupItemAdd(commInfo);
                                            reSampleInfo.msg = handleInfo.msg;
                                            reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                            reSampleInfo.testState = Convert.ToInt32(handleInfo.otherData);
                                            reSampleInfos.Add(reSampleInfo);



                                            await _recordRepository.SampleRecord(info.barcode, RecordEnumVars.ItemAdd, $"拒绝增项，项目：[{info.submitItemCodes}][{info.submitItemNames}]", infos.UserName, true);



                                        }
                                    }
                                    else
                                    {
                                        commReSampleInfo reSampleInfo = new commReSampleInfo();
                                        reSampleInfo.testid = info.testid;
                                        reSampleInfo.barcode = info.barcode;

                                        //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                        //commInfo.UserName = infos.UserName;
                                        //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                        //testhandles.Add(info);
                                        //commInfo.infos = testhandles;
                                        WebApiCallBack handleInfo = await _sampleHandleRepository.ReSpecialRecord(infos);
                                        //WebApiCallBack DelayInfo = await GorupItemAdd(commInfo);
                                        reSampleInfo.msg = handleInfo.msg;
                                        reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                        reSampleInfo.testState = Convert.ToInt32(handleInfo.otherData);
                                        reSampleInfos.Add(reSampleInfo);



                                        await _recordRepository.SampleRecord(info.barcode, RecordEnumVars.ItemAdd, $"拒绝增项，项目：[{info.submitItemCodes}][{info.submitItemNames}]", infos.UserName, true);



                                    }
                                }
                            }

                        }
                        if (infos.state == 5)//退项申请
                        {

                            foreach (TesthandleModel info in infos.infos)
                            {
                                if (info.perid != 0)
                                {
                                    if (info.handleTypeNO == "2")
                                    {
                                        if (info.state)
                                        {
                                            commReSampleInfo reSampleInfo = new commReSampleInfo();
                                            reSampleInfo.testid = info.testid;
                                            reSampleInfo.barcode = info.barcode;

                                            //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                            //commInfo.UserName = infos.UserName;
                                            //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                            //testhandles.Add(info);
                                            //commInfo.infos = testhandles;
                                            await _sampleHandleRepository.ReSpecialRecord(infos);
                                            WebApiCallBack handleInfo = await _sampleHandleRepository.GroupItemCancel(infos);
                                            reSampleInfo.msg = handleInfo.msg;
                                            reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                            reSampleInfo.testState = Convert.ToInt32(handleInfo.otherData);
                                            reSampleInfos.Add(reSampleInfo);



                                            await _recordRepository.SampleRecord(info.barcode, "退项处理", $"同意退项，项目：[{info.submitItemCodes}][{info.submitItemNames}]", infos.UserName, true);



                                        }
                                        else
                                        {
                                            commReSampleInfo reSampleInfo = new commReSampleInfo();
                                            reSampleInfo.testid = info.testid;
                                            reSampleInfo.barcode = info.barcode;
                                            //reSampleInfo.msg = TestItemHandle.Infoback(info, infos.UserName, out int handleState);
                                            //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                            //commInfo.UserName = infos.UserName;
                                            //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                            //testhandles.Add(info);
                                            //commInfo.infos = testhandles;
                                            WebApiCallBack handleInfo = await _sampleHandleRepository.ReSpecialRecord(infos);
                                            //WebApiCallBack DelayInfo = await GorupItemAdd(commInfo);
                                            reSampleInfo.msg = handleInfo.msg;
                                            //reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                            reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                            reSampleInfo.testState = Convert.ToInt32(handleInfo.otherData);
                                            reSampleInfos.Add(reSampleInfo);



                                            await _recordRepository.SampleRecord(info.barcode, "退项处理", $"拒绝退项，项目：[{info.submitItemCodes}][{info.submitItemNames}]", infos.UserName, true);



                                        }
                                    }
                                    else
                                    {
                                        commReSampleInfo reSampleInfo = new commReSampleInfo();
                                        reSampleInfo.testid = info.testid;
                                        reSampleInfo.barcode = info.barcode;
                                        ////reSampleInfo.msg = TestItemHandle.Infoback(info, infos.UserName, out int handleState);
                                        //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                        //commInfo.UserName = infos.UserName;
                                        //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                        //testhandles.Add(info);
                                        //commInfo.infos = testhandles;
                                        WebApiCallBack handleInfo = await _sampleHandleRepository.ReSpecialRecord(infos);
                                        //WebApiCallBack DelayInfo = await GorupItemAdd(commInfo);
                                        reSampleInfo.msg = handleInfo.msg;
                                        reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                        reSampleInfo.testState = Convert.ToInt32(handleInfo.otherData);
                                        reSampleInfos.Add(reSampleInfo);



                                        await _recordRepository.SampleRecord(info.barcode, "退项处理", $"拒绝退项，项目：[{info.submitItemCodes}][{info.submitItemNames}]", infos.UserName, true);



                                    }
                                }
                            }

                        }
                        if (infos.state == 6)//增加免疫组化申请
                        {

                            foreach (TesthandleModel info in infos.infos)
                            {
                                if (info.perid != 0)
                                {
                                    if (info.handleTypeNO == "2")
                                    {
                                        if (info.state)
                                        {
                                            commReSampleInfo reSampleInfo = new commReSampleInfo();
                                            reSampleInfo.testid = info.testid;
                                            reSampleInfo.barcode = info.barcode;
                                            ////reSampleInfo.msg = TestItemHandle.Infoback(info, infos.UserName, out int handleState);
                                            ////reSampleInfo.msg = ItemChangeHandle.GorupItemAdd(info.perid, 0, info.submitItemCodes, infos.UserName, infos.state, out int handleState, out int testStateNO);
                                            ////ReSampleRecord.ReIHCRecord(info, infos.UserName, info.state, out int handleStatea, out int testStateNOa);
                                            //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                            //commInfo.UserName = infos.UserName;
                                            //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                            //testhandles.Add(info);
                                            //commInfo.infos = testhandles;
                                            //await _sampleHandleRepository.ReIHCRecord(commInfo);
                                            WebApiCallBack handleInfo = await _sampleHandleRepository.GorupItemAdd(infos);
                                            reSampleInfo.msg = handleInfo.msg;
                                            reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                            reSampleInfo.testState = Convert.ToInt32(handleInfo.otherData);
                                            reSampleInfos.Add(reSampleInfo);



                                            await _recordRepository.SampleRecord(info.barcode, RecordEnumVars.ItemAdd, $"同意新增免疫组化项目：[{info.submitItemCodes}][{info.submitItemNames}]", infos.UserName, true);



                                        }
                                        else
                                        {
                                            commReSampleInfo reSampleInfo = new commReSampleInfo();
                                            reSampleInfo.testid = info.testid;
                                            reSampleInfo.barcode = info.barcode;
                                            ////reSampleInfo.msg = TestItemHandle.Infoback(info, infos.UserName, out int handleState);
                                            ////reSampleInfo.msg = ReSampleRecord.ReIHCRecord(info, infos.UserName, false, out int handleState, out int testStateNO);
                                            //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                            //commInfo.UserName = infos.UserName;
                                            //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                            //testhandles.Add(info);
                                            //commInfo.infos = testhandles;
                                            WebApiCallBack handleInfo = await _sampleHandleRepository.ReIHCRecord(infos);
                                            //WebApiCallBack DelayInfo = await GorupItemAdd(commInfo);
                                            reSampleInfo.msg = handleInfo.msg;
                                            reSampleInfo.handleState = Convert.ToInt32(handleInfo.data);
                                            reSampleInfo.testState = Convert.ToInt32(handleInfo.otherData);
                                            reSampleInfos.Add(reSampleInfo);



                                            await _recordRepository.SampleRecord(info.barcode, RecordEnumVars.ItemAdd, $"拒绝新增免疫组化项目：[{info.submitItemCodes}][{info.submitItemNames}]", infos.UserName, false);



                                        }
                                    }
                                    else
                                    {
                                        commReSampleInfo reSampleInfo = new commReSampleInfo();
                                        reSampleInfo.testid = info.testid;
                                        reSampleInfo.barcode = info.barcode;
                                        ////reSampleInfo.msg = TestItemHandle.Infoback(info, infos.UserName, out int handleState);
                                        //commInfoModel<TesthandleModel> commInfo = new commInfoModel<TesthandleModel>();
                                        //commInfo.UserName = infos.UserName;
                                        //List<TesthandleModel> testhandles = new List<TesthandleModel>();
                                        //testhandles.Add(info);
                                        //commInfo.infos = testhandles;
                                        WebApiCallBack IHCInfo = await _sampleHandleRepository.ReIHCRecord(infos);
                                        //WebApiCallBack DelayInfo = await GorupItemAdd(commInfo);
                                        reSampleInfo.msg = IHCInfo.msg;
                                        reSampleInfo.handleState = Convert.ToInt32(IHCInfo.data);
                                        reSampleInfo.testState = Convert.ToInt32(IHCInfo.otherData);
                                        reSampleInfos.Add(reSampleInfo);



                                        await _recordRepository.SampleRecord(info.barcode, RecordEnumVars.ItemAdd, $"拒绝新增免疫组化项目：[{info.submitItemCodes}][{info.submitItemNames}]", infos.UserName, false);



                                    }
                                }
                            }
                        }

                        commReInfo.infos = reSampleInfos;
                        jm.data = commReInfo;
                    }
                    else
                    {
                        jm.code = 1;
                        jm.msg = "未找到需要处理的样本信息。";
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
