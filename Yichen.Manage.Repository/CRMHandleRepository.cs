/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


using Nito.AsyncEx;
using System.Data;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.Manage.IRepository;
using Yichen.Manage.Model;
using Yichen.Other.IRepository;
using Yichen.Test.Model;
using Yichen.Net.Configuration;

namespace Yichen.Manage.Repository
{
    /// <summary>
    ///     用户日志 接口实现
    /// </summary>
    public class CRMHandleRepository : BaseRepository<object>, ICRMHandleRepository
    {
        /// <summary>
        /// 异步锁
        /// </summary>
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecordRepository _recordRepository;
        public CRMHandleRepository(IUnitOfWork unitOfWork
            , IRecordRepository recordRepository
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _recordRepository = recordRepository;
        }
        /// <summary>
        /// 客服信息处理记录保存
        /// </summary>
        /// <param name="testInfo">处理信息</param>
        /// <param name="UserName">操作人姓名</param>
        /// <param name="handleState">0失败1成功</param>
        /// <returns></returns>
        public async Task<CRMStateModel> Clienthandle(TesthandleModel testInfo, string UserName)
        {
            CRMStateModel cRMState = new CRMStateModel();
            if (testInfo != null)
            {
                try
                {
                    if (testInfo.perid != 0)
                    {
                        _unitOfWork.BeginTran();
                        if (testInfo.handleTypeNO == "2")
                        {
                            if (testInfo.infoState)
                            {
                                string sql = $"update WorkOther.ClientRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                                cRMState.code = 0;
                                cRMState.handleState = 1;
                                cRMState.testStateNO = 0;
                                cRMState.msg = "处理类型：延迟处理";
                            }
                            else
                            {
                                string sql = $"update WorkOther.ClientRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                                cRMState.code = 0;
                                cRMState.handleState = 1;
                                cRMState.testStateNO = 0;
                                cRMState.msg = "处理类型：拒绝延迟处理";
                            }

                        }
                        else
                        {
                            string sql = $"update WorkOther.ClientRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                            //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                            int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                            cRMState.code = 0;
                            cRMState.handleState = 1;
                            cRMState.testStateNO = 0;
                            cRMState.msg = "处理类型：等待延迟处理";
                        }


                        _recordRepository.SampleRecord(testInfo.barcode, RecordEnumVars.ClientHandle, $"{testInfo.barcode}客服信息处理\r\n" + cRMState.msg, UserName, false);

                        _unitOfWork.CommitTran();
                    }
                    else
                    {
                        cRMState.code = 1;
                        cRMState.handleState = 0;
                        cRMState.testStateNO = 0;
                        cRMState.msg = "提交数据不完整";
                    }

                }
                catch (Exception ex)
                {
                    cRMState.code = 1;
                    cRMState.handleState = 0;
                    cRMState.testStateNO = 0;
                    cRMState.msg = ex.ToString();
                }
            }
            else
            {
                cRMState.code = 1;
                cRMState.handleState = 0;
                cRMState.testStateNO = 0;
                cRMState.msg = "提交数据为空";
            }
            return cRMState;
        }
        /// <summary>
        /// 危急值信息处理
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CRMStateModel> Crisishandle(TesthandleModel testInfo, string UserName)
        {
            CRMStateModel crmState = new CRMStateModel();
            if (testInfo != null)
            {
                try
                {
                    if (testInfo.perid != 0)
                    {
                        _unitOfWork.BeginTran();
                        string msg = "";
                        //if (testInfo.handleTypeNO == "2")
                        //{
                        //if (testInfo.infoState)
                        if (testInfo.infoState)
                        {
                            string sql = $"update WorkOther.CrisisRecord set state='{testInfo.infoState}',contact='{testInfo.contact}',contactMode='{testInfo.contactMode}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                            //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                            int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                            crmState.code = 0;
                            crmState.handleState = 1;
                            crmState.testStateNO = 0;
                            crmState.msg = "危急值处理：已通知到客户";
                        }
                        else
                        {
                            string sql = $"update WorkOther.CrisisRecord set state='{testInfo.infoState}',contact='{testInfo.contact}',contactMode='{testInfo.contactMode}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                            //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                            int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                            crmState.code = 0;
                            crmState.handleState = 1;
                            crmState.testStateNO = 0;
                            crmState.msg = "危急值处理：未联系到客户";
                        }


                        _recordRepository.SampleRecord(testInfo.barcode, RecordEnumVars.CrisisHandle, $"{testInfo.barcode}客服信息处理\r\n" + msg, UserName, false);

                        _unitOfWork.CommitTran();

                    }
                    else
                    {
                        crmState.code = 1;
                        crmState.handleState = 0;
                        crmState.testStateNO = 0;
                        crmState.msg = "提交数据不完整";
                    }

                }
                catch (Exception ex)
                {
                    crmState.code = 1;
                    crmState.handleState = 0;
                    crmState.testStateNO = 0;
                    crmState.msg = ex.ToString();
                }
            }
            else
            {
                crmState.code = 1;
                crmState.handleState = 0;
                crmState.testStateNO = 0;
                crmState.msg = "提交数据为空";
            }
            return crmState;
        }
        /// <summary>
        /// 委托样本处理
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>


        public async Task<CRMStateModel> EntrustHandle(TesthandleModel testInfo, string UserName)
       {
            CRMStateModel crmState = new CRMStateModel();
            throw new NotImplementedException();
        }


        /// <summary>
        /// 样本延迟操作
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public async Task<CRMStateModel> DelayHandle(TesthandleModel testInfo, string UserName)
        {
            CRMStateModel crmState = new CRMStateModel();
            if (testInfo != null)
            {
                try
                {
                    if (testInfo.perid != 0 && testInfo.testid != 0)
                    {
                        _unitOfWork.BeginTran();
                        string msg = "";
                        if (testInfo.handleTypeNO == "2")
                        {
                            if (testInfo.infoState)
                            {
                                string sql = $"update WorkOther.SpecialRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                                crmState.code = 0;
                                crmState.handleState = 1;
                                crmState.testStateNO = 4;
                                crmState.msg = "处理类型：延迟处理";
                            }
                            else
                            {
                                string sql = $"update WorkOther.SpecialRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                                crmState.code = 0;
                                crmState.handleState = 1;
                                crmState.testStateNO = 0;
                                crmState.msg = "处理类型：拒绝延迟处理";
                            }

                        }
                        else
                        {
                            string sql = $"update WorkOther.SpecialRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                            //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                            int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                            crmState.code = 0;
                            crmState.handleState = 1;
                            crmState.testStateNO = 0;
                            crmState.msg = "处理类型：等待延迟处理";
                        }


                        _recordRepository.SampleRecord(testInfo.barcode, "标本延迟", $"{testInfo.barcode}标本延迟操作成功！\r\n" + msg, UserName, false);

                        _unitOfWork.CommitTran();

                    }
                    else
                    {
                        crmState.code = 1;
                        crmState.handleState = 0;
                        crmState.testStateNO = 0;
                        crmState.msg = "提交数据不完整";
                    }

                }
                catch (Exception ex)
                {
                    crmState.code = 1;
                    crmState.handleState = 0;
                    crmState.testStateNO = 0;
                    crmState.msg = ex.ToString();
                }
            }
            else
            {
                crmState.code = 1;
                crmState.handleState = 0;
                crmState.testStateNO = 0;
                crmState.msg = "提交数据为空";
            }
            return crmState;
        }
        /// <summary>
        /// 标本重采操作
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public async Task<CRMStateModel> AginHandle(TesthandleModel testInfo, string UserName)
        {
            CRMStateModel crmState = new CRMStateModel();
            if (testInfo != null)
            {
                try
                {
                    if (testInfo.perid != 0 && testInfo.testid != 0)
                    {
                        _unitOfWork.BeginTran();
                        string msg = "";
                        if (testInfo.handleTypeNO == "2")
                        {
                            if (testInfo.infoState)
                            {
                                string sql = $"update WorkOther.SpecialRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                                crmState.code = 0;
                                crmState.testStateNO = 4;
                                crmState.handleState = 1;
                                crmState.msg = "处理类型：重采处理";
                            }
                            else
                            {
                                string sql = $"update WorkOther.SpecialRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                                int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                                crmState.code = 0;
                                crmState.testStateNO = 0;
                                crmState.handleState = 1;
                                crmState.msg = "处理类型：拒绝重采处理";
                            }

                        }
                        else
                        {
                            string sql = $"update WorkOther.SpecialRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                            //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                            int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                            crmState.code = 0;
                            crmState.testStateNO = 0;
                            crmState.handleState = 1;
                            crmState.msg = "处理类型：等待重采处理";
                        }


                        _recordRepository.SampleRecord(testInfo.barcode, "标本重采", $"{testInfo.barcode}标本操作成功！{msg}", UserName, false);

                        _unitOfWork.CommitTran();
                    }
                    else
                    {
                        crmState.code = 1;
                        crmState.handleState = 0;
                        crmState.testStateNO = 0;
                        crmState.msg = "提交数据不完整";
                    }
                }
                catch (Exception ex)
                {
                    crmState.code = 1;
                    crmState.handleState = 0;
                    crmState.testStateNO = 0;
                    crmState.msg = ex.ToString();
                }
            }
            else
            {
                crmState.code = 1;
                crmState.handleState = 0;
                crmState.testStateNO = 0;
                crmState.msg = "提交数据为空";
            }
            return crmState;
        }
        /// <summary>
        /// 标本退单操作
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public async Task<CRMStateModel> backHandle(TesthandleModel testInfo, string UserName)
        {
            CRMStateModel crmState = new CRMStateModel();
            if (testInfo != null)
            {
                try
                {
                    if (testInfo.perid != 0 && testInfo.testid != 0)
                    {
                        _unitOfWork.BeginTran();
                        if (testInfo.handleTypeNO == "2")
                        {
                            if (testInfo.infoState)
                            {
                                string SFinance = $"select perid,standerCharge,settlementCharge,charge from Finance.GroupBillInfo where testid={testInfo.testid} and dstate=0";
                                DataTable DTFinance = await DbClient.Ado.GetDataTableAsync(SFinance);
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
                                        string sql = $"update WorkOther.SpecialRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                        sql += $"update WorkTest.SampleInfo set tabTypeNO=4,testStateNO='8' where id='{testInfo.testid}';";
                                        //int a = HLDBSqlHelper.ExecuteNonQuery( sql);
                                        sql += $"update Finance.GroupBillInfo set tabTypeNO='4',state=0,dstate=1 where testid={testInfo.testid} ";
                                        sql += $"update Finance.ApplyBillInfo set standerCharge=standerCharge-{standerCharge},settlementCharge=settlementCharge-{settlementCharge},charge=charge-{charge} where perid={groupperid} ";
                                        int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                                        //string sqlWx= $"update WorkTest.SampleInfo set tabTypeNO=4,testStateNO='8' where id='{testInfo.testid}';";
                                        //SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, sqlWx);
                                        crmState.code = 0;
                                        crmState.handleState = 1;
                                        crmState.testStateNO = 8;
                                        crmState.msg = "处理类型：退单处理";
                                    }
                                    else
                                    {
                                        crmState.code = 1;
                                        crmState.handleState = 0;
                                        crmState.testStateNO = 0;
                                        crmState.msg = "未匹配到对应数据";
                                    }
                                }
                                else
                                {
                                    //信息为审核直接删除录入信息
                                    crmState.code = 0;
                                    crmState.handleState = 1;
                                    crmState.testStateNO = 0;
                                    crmState.msg = "处理类型：退单处理";
                                    string sql = $"update WorkPer.SampleInfo set dstate='1' where id='{testInfo.perid}';";
                                    int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                                }
                            }
                            else
                            {
                                string sql = $"update WorkOther.SpecialRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                                int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                                crmState.code = 0;
                                crmState.handleState = 1;
                                crmState.testStateNO = 0;
                                crmState.msg = "处理类型：拒绝退单处理";
                            }


                        }
                        else
                        {
                            string sql = $"update WorkOther.SpecialRecord set state='{testInfo.infoState}',handleTypeNO='{testInfo.handleTypeNO}',handleResult='{testInfo.handleResult}',remark='{testInfo.remark}',handler='{testInfo.handler}',handleTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id='{testInfo.id}';";
                            //sql += $"update WorkTest.SampleInfo set testStateNO='4' where id='{testInfo.testid}';";
                            int a = await DbClient.Ado.ExecuteCommandAsync(sql);
                            crmState.code = 0;
                            crmState.handleState = 1;
                            crmState.testStateNO = 0;
                            crmState.msg = "处理类型：等待处理";
                        }


                        _recordRepository.SampleRecord(testInfo.barcode, "标本退单", $"{testInfo.barcode}标本操作成功！{crmState.msg}", UserName, false);

                        _unitOfWork.CommitTran();
                    }
                    else
                    {
                        crmState.code = 1;
                        crmState.handleState = 0;
                        crmState.testStateNO = 0;
                        crmState.msg = "提交数据不完整";
                    }
                }
                catch (Exception ex)
                {
                    crmState.code = 1;
                    crmState.handleState = 0;
                    crmState.testStateNO = 0;
                    crmState.msg = ex.ToString();
                }
            }
            else
            {
                crmState.code = 1;
                crmState.handleState = 0;
                crmState.testStateNO = 0;
                crmState.msg = "提交数据为空";
            }
            return crmState;
        }
    }
}