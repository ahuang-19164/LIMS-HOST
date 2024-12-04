/***********************************************************************
 *            Project: Yichen
 *        ProjectName: 屹辰智禾管理系统                                
 *                Web: https://www.zui51.com                 
 *             Author: 屹辰                                       
 *              Email: 499715561@qq.com                              
 *         CreateTime: 
 *        Description: 暂无
 ***********************************************************************/


using System.Linq.Expressions;
using Yichen.Net.Configuration;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;
using Yichen.Comm.Repository;
using Yichen.Per.Model.table;
using Yichen.Per.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using System.Data;
using Yichen.Test.Model.table;
using NPOI.SS.Formula.Functions;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaGetWxaGameFrameResponse.Types.Data.Types;
using Yichen.System.Model;

namespace Yichen.Per.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class TestSampleInfoRepository : BaseRepository<test_sampleInfo>, ITestSampleInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public TestSampleInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public  async Task<bool> BarcodeExist(string barcode)
        {
            if (barcode != null && barcode != "")
                return false;
            return await DbClient.Queryable<test_sampleInfo>().AnyAsync(p => p.barcode == barcode && p.dstate == false);
        }

        /// <summary>
        /// 根据条码获取样本信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public  async Task<test_sampleInfo> GetByBarcode(string barcode)
        {
            return await DbClient.Queryable<test_sampleInfo>().FirstAsync(p => p.barcode == barcode && p.dstate == false);
        }

        /// <summary>
        /// 修改test表中的信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public async Task<int> TestInfoEdit(string perid, Dictionary<string, object> pairs)
        {
            return await DbClient.Updateable<test_sampleInfo>(pairs).Where(p => p.perid == perid).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除录入条码信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public  async Task<bool> BarcodeDelete(test_sampleInfo sampleinfo)
        {
            int a = await DbClient.Insertable<test_sampleInfo>(sampleinfo).ExecuteCommandAsync();
            return a == 0 ? false : true;
        }

        /// <summary>
        /// 查询录入样本信息
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public  async Task<DataTable> GetEntryInfo(DateTime startTime, DateTime endTime, string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return await DbClient.Queryable<test_sampleInfo>()
                    .Where(p => SqlFunc.Between(p.createTime, startTime, endTime))
                    .ToDataTableAsync();
            }
            else
            {
                return await DbClient.Queryable<test_sampleInfo>()
                    .Where(p => SqlFunc.Between(p.createTime, startTime, endTime))
                    .Where(p=>p.creater==userName)
                    .ToDataTableAsync();
            }
        }


        public async Task<int> InsertTestSampleInfo(per_sampleInfo perinfo, string sampleID, DateTime receiveTime, string GroupCodes, string GroupNames, string GroupNO, string groupFlowNO, bool delGroupState, string delGroupClientNO, string UserName, bool ReceiveState = true)
        {
            //int recive = AppSettingsConstVars.ReceiveState ? 0 : 1;
            //pairsInfo.Add("state", recive);
            //int visible = AppSettingsConstVars.SampleSort ? 0 : 1;//开启分拣功能 开启分拣
            //pairsInfo.Add("sortState", visible);
            test_sampleInfo pairsInfo = new test_sampleInfo()
            {
                state = AppSettingsConstVars.ReceiveState ? false : true,
                sortState = AppSettingsConstVars.SampleSort ? false : true,//开启分拣功能 开启分拣
                perid = perinfo.id.ToString(),
                sampleID = sampleID,
                disState = false,
                dstate = false,
                visible = true,
                //connState=perinfo.connstate,//数据来源 0本地1疾控接收2微信
                report = 0,//样本信息报告状态 (0为报告，1上传报告，2接口报告，3生成报告)
                reportState = false,
                urgent = perinfo.urgent,
                createTime = DateTime.Now,
                receiveTime = receiveTime,
                sampleTime = perinfo.sampleTime,
                ageDay = perinfo.ageDay,
                ageMoth = perinfo.ageMoth,
                pathologyStateNO = 1,
                testStateNO = 1,
                patientName = perinfo.patientName,
                agentNO = perinfo.agentNO,
                ageYear = perinfo.ageYear.ToString(),
                applyItemCodes = perinfo.applyItemCodes,
                applyItemNames = perinfo.applyItemNames,
                barcode = perinfo.barcode,
                bedNo = perinfo.bedNo,
                clinicalDiagnosis = perinfo.clinicalDiagnosis,
                creater = UserName,
                cutPart = perinfo.cutPart,
                department = perinfo.department,
                doctorPhone = perinfo.doctorPhone,
                groupCodes = GroupCodes,
                groupNames = GroupNames,
                hospitalBarcode = perinfo.hospitalBarcode,
                frameNo = perinfo.frameNo,
                hospitalNames = perinfo.hospitalNames,
                hospitalNO = perinfo.hospitalNO,
                medicalNo = perinfo.medicalNo,
                menstrualTime = perinfo.menstrualTime,
                number = perinfo.number.ToString(),
                passportNo = perinfo.passportNo,
                pathologyNo = perinfo.pathologyNo,
                patientAddress = perinfo.patientAddress,
                patientCardNo = perinfo.patientCardNo,
                patientPhone = perinfo.patientPhone,
                patientSexNames = perinfo.patientSexNames,
                patientSexNO = perinfo.patientSexNO,
                patientTypeNames = perinfo.patientTypeNames,
                patientTypeNO = perinfo.patientTypeNO,
                perRemark = perinfo.perRemark,
                sampleAddress = perinfo.sampleAddress,
                sampleLocation = perinfo.sampleLocation,
                sampleShapeNames = perinfo.sampleShapeNames,
                sampleShapeNO = perinfo.sampleShapeNO,
                sampleTypeNames = perinfo.sampleTypeNames,
                sampleTypeNO = perinfo.sampleTypeNO,
                sendDoctor = perinfo.sendDoctor,
                groupNO = GroupNO,
                groupFlowNO = groupFlowNO,
                delegateState = delGroupState,
                delstateClientNO = delGroupClientNO,
                tabTypeNO = 1
            };
            return await DbClient.Insertable<test_sampleInfo>(pairsInfo).ExecuteReturnIdentityAsync();

        }



        #region 实现重写增删改查操作==========================================================


        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>

        public  async Task<WebApiCallBack> InsertAsync(test_sampleInfo entity)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync() > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> UpdateAsync(test_sampleInfo entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<test_sampleInfo>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.perid = entity.perid;
            oldModel.sampleID = entity.sampleID;
            oldModel.testNo = entity.testNo;
            oldModel.frameNo = entity.frameNo;
            oldModel.groupNO = entity.groupNO;
            oldModel.barcode = entity.barcode;
            oldModel.hospitalBarcode = entity.hospitalBarcode;
            oldModel.reachTime = entity.reachTime;
            oldModel.hospitalNO = entity.hospitalNO;
            oldModel.hospitalNames = entity.hospitalNames;
            oldModel.agentNO = entity.agentNO;
            oldModel.agentNames = entity.agentNames;
            oldModel.medicalNo = entity.medicalNo;
            oldModel.sampleAddress = entity.sampleAddress;
            oldModel.sampleTime = entity.sampleTime;
            oldModel.receiveTime = entity.receiveTime;
            oldModel.patientTypeNO = entity.patientTypeNO;
            oldModel.patientTypeNames = entity.patientTypeNames;
            oldModel.patientName = entity.patientName;
            oldModel.patientSexNO = entity.patientSexNO;
            oldModel.patientSexNames = entity.patientSexNames;
            oldModel.ageYear = entity.ageYear;
            oldModel.ageMoth = entity.ageMoth;
            oldModel.ageDay = entity.ageDay;
            oldModel.ageNames = entity.ageNames;
            oldModel.patientbirthday = entity.patientbirthday;
            oldModel.departmentNO = entity.departmentNO;
            oldModel.department = entity.department;
            oldModel.bedNo = entity.bedNo;
            oldModel.patientPhone = entity.patientPhone;
            oldModel.patientCardNo = entity.patientCardNo;
            oldModel.passportNo = entity.passportNo;
            oldModel.patientAddress = entity.patientAddress;
            oldModel.sendDoctor = entity.sendDoctor;
            oldModel.doctorPhone = entity.doctorPhone;
            oldModel.pathologyNo = entity.pathologyNo;
            oldModel.cutPart = entity.cutPart;
            oldModel.menstrualTime = entity.menstrualTime;
            oldModel.sampleTypeNO = entity.sampleTypeNO;
            oldModel.sampleTypeNames = entity.sampleTypeNames;
            oldModel.sampleShapeNO = entity.sampleShapeNO;
            oldModel.sampleShapeNames = entity.sampleShapeNames;
            oldModel.clinicalDiagnosis = entity.clinicalDiagnosis;
            oldModel.sampleLocation = entity.sampleLocation;
            oldModel.perRemark = entity.perRemark;
            oldModel.testRemark = entity.testRemark;
            oldModel.applyItemCodes = entity.applyItemCodes;
            oldModel.applyItemNames = entity.applyItemNames;
            oldModel.groupCodes = entity.groupCodes;
            oldModel.groupNames = entity.groupNames;
            oldModel.groupFlowNO = entity.groupFlowNO;
            oldModel.testStateNO = entity.testStateNO;
            oldModel.pathologyStateNO = entity.pathologyStateNO;
            oldModel.delegateState = entity.delegateState;
            oldModel.delstateClientNO = entity.delstateClientNO;
            oldModel.reportState = entity.reportState;
            oldModel.state = entity.state;
            oldModel.urgent = entity.urgent;
            oldModel.number = entity.number;
            oldModel.tester = entity.tester;
            oldModel.testTime = entity.testTime;
            oldModel.reTester = entity.reTester;
            oldModel.reTestTime = entity.reTestTime;
            oldModel.checker = entity.checker;
            oldModel.checkTime = entity.checkTime;
            oldModel.creater = entity.creater;
            oldModel.createTime = entity.createTime;
            oldModel.testReceive = entity.testReceive;
            oldModel.testReceiveTime = entity.testReceiveTime;
            oldModel.editer = entity.editer;
            oldModel.editTime = entity.editTime;
            oldModel.disState = entity.disState;
            oldModel.realtester = entity.realtester;
            oldModel.realtestTime = entity.realtestTime;
            oldModel.realreTester = entity.realreTester;
            oldModel.realreTestTime = entity.realreTestTime;
            oldModel.realchecker = entity.realchecker;
            oldModel.realcheckTime = entity.realcheckTime;
            oldModel.visible = entity.visible;
            oldModel.dstate = entity.dstate;
            oldModel.xgState = entity.xgState;
            oldModel.tabTypeNO = entity.tabTypeNO;
            oldModel.eState = entity.eState;
            oldModel.sortState = entity.sortState;


            oldModel.sortState = entity.sortState;
            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> UpdateAsync(List<test_sampleInfo> entity)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            return jm;
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> DeleteByIdAsync(object id)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Deleteable<test_sampleInfo>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> DeleteByIdsAsync(int[] ids)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Deleteable<test_sampleInfo>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;
        }

        /// <summary>
        /// 隐藏指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> HideByIdAsync(object id)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Updateable<test_sampleInfo>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;
        }


        #endregion

        #region 获取缓存的所有数据==========================================================

        ///// <summary>
        ///// 获取缓存的所有数据
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<test_sampleInfo>> GetCaChe()
        //{
        //    var cache = ManualDataCache.Instance.Get<List<test_sampleInfo>>(GlobalConstVars.CacheSampleImg);
        //    if (cache != null)
        //    {
        //        return cache;
        //    }
        //    return await UpdateCaChe();
        //}

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //public async Task<List<test_sampleInfo>> UpdateCaChe()
        //{
        //    var list = await DbClient.Queryable<test_sampleInfo>().With(SqlWith.NoLock).ToListAsync();
        //    ManualDataCache.Instance.Set(GlobalConstVars.CacheSampleImg, list);
        //    return list;
        //}

        #endregion








        #region 重写根据条件查询数据


        ///// <summary>
        /////重写根据条件查询数据
        /////var wheres = PredicateBuilder.True<CoreCmsGoodsParams>();
        /////wheres = wheres.And(p => p.id == id);
        /////Expression<Func<CoreCmsGoodsParams, object>> orderEx;
        /////orderEx = p => p.id;
        /////OrderByType.Asc,
        ///// </summary>
        ///// <param name="predicate">判断集合</param>
        ///// <param name="orderByType">排序方式</param>
        ///// <param name="pageIndex">当前页面索引</param>
        ///// <param name="pageSize">分布大小</param>
        ///// <param name="orderByExpression"></param>
        ///// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        ///// <returns></returns>
        //public new async Task<List<test_sampleInfo>> QueryAsync(
        //    Expression<Func<test_sampleInfo, bool>> predicate,
        //    Expression<Func<test_sampleInfo, object>> orderByExpression, OrderByType orderByType, bool blUseNoLock = false)
        //{
        //    if (blUseNoLock)
        //    {
        //        return await DbClient.Queryable<test_sampleInfo>()
        //        .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
        //        .WhereIF(predicate != null, predicate)
        //        .ToListAsync();
        //    }
        //    else
        //    {
        //        return await DbClient.Queryable<test_sampleInfo>()
        //        .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
        //        .WhereIF(predicate != null, predicate)
        //        .ToListAsync();
        //    }
        //}



        /// <summary>
        ///     重写根据条件查询分页数据
        ///var wheres = PredicateBuilder.True<CoreCmsGoodsParams>();
        ///wheres = wheres.And(p => p.id == id);
        ///Expression<Func<CoreCmsGoodsParams, object>> orderEx;
        ///orderEx = p => p.id;
        ///OrderByType.Asc,
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public  async Task<IPageList<test_sampleInfo>> QueryPageAsync(Expression<Func<test_sampleInfo, bool>> predicate,
            Expression<Func<test_sampleInfo, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<test_sampleInfo> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<test_sampleInfo>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<test_sampleInfo>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<test_sampleInfo>(page, pageIndex, pageSize, totalCount);
            return list;
        }



        #endregion

    }
}
