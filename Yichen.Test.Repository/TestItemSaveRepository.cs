using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Other.IRepository;
using Yichen.Per.Model.table;
using Yichen.Test.IRepository;
using Yichen.Test.Model.Result;
using Yichen.Test.Model.table;
using Yichen.Net.Configuration;

namespace Yichen.Test.Repository
{
    public class TestItemSaveRepository : BaseRepository<object>, ITestItemSaveRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecordRepository _recordRepository;
        public TestItemSaveRepository(IUnitOfWork unitOfWork
            , IRecordRepository recordRepository
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _recordRepository = recordRepository;
        }


        #region 检验结果保存
        /// <summary>
        /// 结果保存
        /// </summary>
        /// <param name="autographInfo">代审核信息</param>
        /// <param name="nextFlowNO">流程节点编号</param>
        /// <param name="testid">检验信息ID</param>
        /// <param name="operter">操作人</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Test(AutographInfo autographInfo, int perid, int testid, string barcode, string operter, string nextFlowNO = "0", int pathologyStateNO = 0)
        {

            WebApiCallBack jm = new WebApiCallBack();
            //_unitOfWork.BeginTran();
            try
            {
                int a = 0;

                if (autographInfo.state == false)
                {
                    //nextFlowNO = nextFlowNO = "" || nextFlowNO = "0" || nextFlowNO = null ? "0" : nextFlowNO;

                    a = await DbClient.Updateable<test_sampleInfo>().SetColumns(p => new test_sampleInfo()
                    {
                        groupFlowNO = nextFlowNO,
                        tester = operter,
                        testTime = DateTime.Now,
                        realtester = operter,
                        realtestTime = DateTime.Now,
                        disState = autographInfo.state,
                        testRemark = autographInfo.testRemark
                    }).Where(p => p.id == testid).ExecuteCommandAsync();

                }
                else
                {
                    //nextFlowNO = nextFlowNO = "" || nextFlowNO = "0" || nextFlowNO = null ? "0" : nextFlowNO;


                    a = await DbClient.Updateable<test_sampleInfo>().SetColumns(p => new test_sampleInfo()
                    {
                        groupFlowNO = nextFlowNO,
                        tester = autographInfo.tester,
                        testTime = autographInfo.testTime,
                        realtester = operter,
                        realtestTime = DateTime.Now,
                        disState = autographInfo.state,
                        testRemark = autographInfo.testRemark
                    }).Where(p => p.id == testid).ExecuteCommandAsync();


                }
                if (a > 0)
                {
                    jm.msg = "保存成功！";

                    _recordRepository.SampleRecord(barcode, RecordEnumVars.Save, $"样本ID:{testid}-保存样本结果!", operter, true);

                }
                else
                {
                    jm.code = 1;
                    jm.msg = "保存失败！";

                    _recordRepository.SampleRecord(barcode, RecordEnumVars.Save, $"样本ID:{testid}-保存样本结果失败", operter, false);

                }
            }
            catch (Exception ex)
            {
                jm.code = 1;
                jm.msg = ex.Message + ex.StackTrace;
            }
            //_unitOfWork.CommitTran();
            //}
            return jm;
        }
        /// <summary>
        /// 结果初审
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WebApiCallBack> reTest(AutographInfo autographInfo, int perid, int testid, string barcode, string operter, string nextFlowNO = "0", int pathologyStateNO = 0)
        {
            WebApiCallBack jm = new WebApiCallBack();
            //_unitOfWork.BeginTran();
            try
            {
                int a = 0;

                if (autographInfo.state == false)
                {


                    a = await DbClient.Updateable<test_sampleInfo>().SetColumns(p => new test_sampleInfo()
                    {
                        testStateNO = 2,
                        reTester = operter,
                        reTestTime = DateTime.Now,
                        realreTester = operter,
                        realreTestTime = DateTime.Now,
                        disState = autographInfo.state,
                        testRemark = autographInfo.testRemark
                    }).Where(p => p.id == testid).ExecuteCommandAsync();

                }
                else
                {


                    a = await DbClient.Updateable<test_sampleInfo>().SetColumns(p => new test_sampleInfo()
                    {
                        testStateNO = 2,
                        reTester = autographInfo.tester,
                        reTestTime = autographInfo.testTime,
                        realreTester = operter,
                        realreTestTime = DateTime.Now,
                        disState = autographInfo.state,
                        testRemark = autographInfo.testRemark
                    }).Where(p => p.id == testid).ExecuteCommandAsync();


                }
                if (a > 0)
                {
                    jm.msg = "保存成功！";

                    _recordRepository.SampleRecord(barcode, RecordEnumVars.Save1, $"样本ID:{testid}-初审样本结果!", operter, true);

                }
                else
                {
                    jm.code = 1;
                    jm.msg = "保存失败！";

                    _recordRepository.SampleRecord(barcode, RecordEnumVars.Save1, $"样本ID:{testid}-初审样本结果失败", operter, false);

                }
            }
            catch (Exception ex)
            {
                jm.msg = ex.Message + ex.StackTrace;
            }
            //_unitOfWork.CommitTran();
            return jm;
        }
        /// <summary>
        /// 结果审核
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WebApiCallBack> Check(AutographInfo autographInfo, int perid, int testid, string barcode, string operter, string nextFlowNO = "0", int pathologyStateNO = 0)
        {
            WebApiCallBack jm = new WebApiCallBack();
            //_unitOfWork.BeginTran();
            try
            {

                if (nextFlowNO == "" || nextFlowNO == "0")
                {
                    nextFlowNO = "0";
                    //查询混采信息是否存在
                    //int numm = await _sampleblendservices.GetBlendCount(0, barcode);
                    int numm = await DbClient.Queryable<per_sampleblend>().CountAsync(p => p.barcode == barcode);
                    int a = 0;

                    if (numm != null || numm != 0)
                    {
                        if (autographInfo.state == false)
                        {


                            a = await DbClient.Updateable<test_sampleInfo>().SetColumns(p => new test_sampleInfo()
                            {
                                //reportState = false,
                                number = numm.ToString(),
                                testStateNO = 3,
                                //groupFlowNO = nextFlowNO,
                                checker = operter,
                                checkTime = DateTime.Now,
                                realchecker = operter,
                                realcheckTime = DateTime.Now,
                                disState = autographInfo.state,
                                testRemark = autographInfo.testRemark
                            }).Where(p => p.id == testid).ExecuteCommandAsync();

                        }
                        else
                        {


                            a = await DbClient.Updateable<test_sampleInfo>().SetColumns(p => new test_sampleInfo()
                            {
                                //reportState = false,
                                number = numm.ToString(),
                                testStateNO = 3,
                                //groupFlowNO = nextFlowNO,
                                checker = autographInfo.checker,
                                checkTime = autographInfo.checkTime,
                                realchecker = operter,
                                realcheckTime = DateTime.Now,
                                disState = autographInfo.state,
                                testRemark = autographInfo.testRemark
                            }).Where(p => p.id == testid).ExecuteCommandAsync();


                        }
                    }
                    else
                    {
                        if (autographInfo.state == false)
                        {

                            a = await DbClient.Updateable<test_sampleInfo>().SetColumns(p => new test_sampleInfo()
                            {
                                //reportState = false,
                                testStateNO = 3,
                                //groupFlowNO = nextFlowNO,
                                checker = operter,
                                checkTime = DateTime.Now,
                                realchecker = operter,
                                realcheckTime = DateTime.Now,
                                disState = autographInfo.state,
                                testRemark = autographInfo.testRemark
                            }).Where(p => p.id == testid).ExecuteCommandAsync();

                        }
                        else
                        {


                            a = await DbClient.Updateable<test_sampleInfo>().SetColumns(p => new test_sampleInfo()
                            {
                                //reportState = false,
                                testStateNO = 3,
                                //groupFlowNO = nextFlowNO,
                                checker = autographInfo.checker,
                                checkTime = autographInfo.checkTime,
                                realchecker = operter,
                                realcheckTime = DateTime.Now,
                                disState = autographInfo.state,
                                testRemark = autographInfo.testRemark
                            }).Where(p => p.id == testid).ExecuteCommandAsync();


                        }
                    }

                    if (a > 0)
                    {

                        if (numm != null || numm != 0)
                        {
                            DbClient.Updateable<per_sampleInfo>().SetColumns(p => new per_sampleInfo() { perStateNO = 4, number = numm }).Where(p => p.id == perid).ExecuteCommandAsync();
                        }
                        else
                        {

                            DbClient.Updateable<per_sampleInfo>().SetColumns(p => p.perStateNO == 4).Where(p => p.id == perid).ExecuteCommandAsync();

                        }


                        _recordRepository.SampleRecord(barcode, RecordEnumVars.Save2, $"样本ID:{testid}-审核样本结果！", operter, true);

                        jm.msg = "保存成功！";
                    }
                    else
                    {
                        jm.code = 1;
                        jm.msg = "保存失败！";

                        _recordRepository.SampleRecord(barcode, RecordEnumVars.Save2, $"样本ID:{testid}-审核样本结果失败", operter, false);

                    }
                }
                else
                {
                    jm.msg = "当前样本没完成流程,不能进行初审！";
                }

            }
            catch (Exception ex)
            {
                jm.msg = ex.Message + ex.StackTrace;
            }
            //_unitOfWork.BeginTran();
            return jm;
        }
        /// <summary>
        /// 结果反审核
        /// </summary>
        /// <param name="perid"></param>
        /// <param name="testid"></param>
        /// <param name="barcode"></param>
        /// <param name="operter"></param>
        /// <param name="report">0为报告，1上传报告，2接口报告，3生成报告</param>
        /// <param name="nextFlowNO"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ReCheck(string perid, string testid, string barcode, string operter,int reportState=0, string nextFlowNO = "0")
        {
            WebApiCallBack jm = new WebApiCallBack();
            //_unitOfWork.BeginTran();
            commReInfo<commReItemInfo> commReInfo = new commReInfo<commReItemInfo>();
            //判断报告是否由系统自动生成
            if (reportState == 3)
                reportState = 0;
            int a = await DbClient.Updateable<test_sampleInfo>().SetColumns(p => new test_sampleInfo()
            {
                report= reportState,
                testStateNO = 1,
                tester = "",
                testTime = null,
                realtester = "",
                realtestTime = null,
                reTester = "",
                reTestTime = null,
                realreTester = "",
                realreTestTime = null,
                checker = "",
                checkTime = null,
                realchecker = "",
                realcheckTime = null
            }).Where(p => p.id == Convert.ToInt32(testid)).ExecuteCommandAsync();

            DbClient.Updateable<per_sampleInfo>().SetColumns(p => p.perStateNO == 3).Where(p => p.id == Convert.ToInt32(perid)).ExecuteCommandAsync();

            if (a > 0)
            {
                commReInfo.code = 0;
                commReInfo.msg = "反审成功";
                _recordRepository.SampleRecord(barcode, RecordEnumVars.ReSave, "反审成功！", operter, false);

            }
            else
            {
                commReInfo.code = 1;
                commReInfo.msg = "反审失败";
                _recordRepository.SampleRecord(barcode, RecordEnumVars.ReSave, "反审失败！", operter, false);

            }
            //_unitOfWork.CommitTran();
            jm.data = commReInfo;
            return jm;
        }


        #endregion
    }
}
