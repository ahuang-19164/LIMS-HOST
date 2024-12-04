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
using Yichen.Net.Caching.Manual;

namespace Yichen.Per.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class SampleBlendInfoRepository : BaseRepository<per_sampleblend>, ISampleBlendInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SampleBlendInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(per_sampleblend entity)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync() > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> UpdateAsync(per_sampleblend entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<per_sampleblend>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.blendid = entity.blendid;
            oldModel.id = entity.id;
            oldModel.patientid = entity.patientid;
            oldModel.perid = entity.perid;
            oldModel.sampleID = entity.sampleID;
            oldModel.testNo = entity.testNo;
            oldModel.frameNo = entity.frameNo;
            oldModel.groupNO = entity.groupNO;
            oldModel.barcode = entity.barcode;
            oldModel.hospitalbarcode = entity.hospitalbarcode;
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
            
            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> UpdateAsync(List<per_sampleblend> entity)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

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

            var bl = await DbClient.Deleteable<per_sampleblend>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

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

            var bl = await DbClient.Deleteable<per_sampleblend>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

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

            var bl = await DbClient.Updateable<per_sampleblend>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }


        #endregion

       #region 获取缓存的所有数据==========================================================

        ///// <summary>
        ///// 获取缓存的所有数据
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<per_sampleblend>> GetCaChe()
        //{
        //    var cache = ManualDataCache.Instance.Get<List<per_sampleblend>>(GlobalConstVars.CacheSampleBlendInfo);
        //    if (cache != null)
        //    {
        //        return cache;
        //    }
        //    return await UpdateCaChe();
        //}

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //public async Task<List<per_sampleblend>> UpdateCaChe()
        //{
        //    var list = await DbClient.Queryable<per_sampleblend>().With(SqlWith.NoLock).ToListAsync();
        //    ManualDataCache.Instance.Set(GlobalConstVars.CacheSampleBlendInfo, list);
        //    return list;
        //}

        #endregion


        #region 重写根据条件查询分页数据
        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public  async Task<IPageList<per_sampleblend>> QueryPageAsync(Expression<Func<per_sampleblend, bool>> predicate,
            Expression<Func<per_sampleblend, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<per_sampleblend> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<per_sampleblend>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new per_sampleblend
                {
                      blendid = p.blendid,
                id = p.id,
                patientid = p.patientid,
                perid = p.perid,
                sampleID = p.sampleID,
                testNo = p.testNo,
                frameNo = p.frameNo,
                groupNO = p.groupNO,
                barcode = p.barcode,
                hospitalbarcode = p.hospitalbarcode,
                reachTime = p.reachTime,
                hospitalNO = p.hospitalNO,
                hospitalNames = p.hospitalNames,
                agentNO = p.agentNO,
                agentNames = p.agentNames,
                medicalNo = p.medicalNo,
                sampleAddress = p.sampleAddress,
                sampleTime = p.sampleTime,
                receiveTime = p.receiveTime,
                patientTypeNO = p.patientTypeNO,
                patientTypeNames = p.patientTypeNames,
                patientName = p.patientName,
                patientSexNO = p.patientSexNO,
                patientSexNames = p.patientSexNames,
                ageYear = p.ageYear,
                ageMoth = p.ageMoth,
                ageDay = p.ageDay,
                ageNames = p.ageNames,
                patientbirthday = p.patientbirthday,
                departmentNO = p.departmentNO,
                department = p.department,
                bedNo = p.bedNo,
                patientPhone = p.patientPhone,
                patientCardNo = p.patientCardNo,
                passportNo = p.passportNo,
                patientAddress = p.patientAddress,
                sendDoctor = p.sendDoctor,
                doctorPhone = p.doctorPhone,
                pathologyNo = p.pathologyNo,
                cutPart = p.cutPart,
                menstrualTime = p.menstrualTime,
                sampleTypeNO = p.sampleTypeNO,
                sampleTypeNames = p.sampleTypeNames,
                sampleShapeNO = p.sampleShapeNO,
                sampleShapeNames = p.sampleShapeNames,
                clinicalDiagnosis = p.clinicalDiagnosis,
                sampleLocation = p.sampleLocation,
                perRemark = p.perRemark,
                testRemark = p.testRemark,
                applyItemCodes = p.applyItemCodes,
                applyItemNames = p.applyItemNames,
                groupCodes = p.groupCodes,
                groupNames = p.groupNames,
                groupFlowNO = p.groupFlowNO,
                testStateNO = p.testStateNO,
                pathologyStateNO = p.pathologyStateNO,
                delegateState = p.delegateState,
                delstateClientNO = p.delstateClientNO,
                reportState = p.reportState,
                state = p.state,
                urgent = p.urgent,
                number = p.number,
                tester = p.tester,
                testTime = p.testTime,
                reTester = p.reTester,
                reTestTime = p.reTestTime,
                checker = p.checker,
                checkTime = p.checkTime,
                creater = p.creater,
                createTime = p.createTime,
                testReceive = p.testReceive,
                testReceiveTime = p.testReceiveTime,
                editer = p.editer,
                editTime = p.editTime,
                disState = p.disState,
                realtester = p.realtester,
                realtestTime = p.realtestTime,
                realreTester = p.realreTester,
                realreTestTime = p.realreTestTime,
                realchecker = p.realchecker,
                realcheckTime = p.realcheckTime,
                visible = p.visible,
                dstate = p.dstate,
                xgState = p.xgState,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<per_sampleblend>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new per_sampleblend
                {
                      blendid = p.blendid,
                id = p.id,
                patientid = p.patientid,
                perid = p.perid,
                sampleID = p.sampleID,
                testNo = p.testNo,
                frameNo = p.frameNo,
                groupNO = p.groupNO,
                barcode = p.barcode,
                hospitalbarcode = p.hospitalbarcode,
                reachTime = p.reachTime,
                hospitalNO = p.hospitalNO,
                hospitalNames = p.hospitalNames,
                agentNO = p.agentNO,
                agentNames = p.agentNames,
                medicalNo = p.medicalNo,
                sampleAddress = p.sampleAddress,
                sampleTime = p.sampleTime,
                receiveTime = p.receiveTime,
                patientTypeNO = p.patientTypeNO,
                patientTypeNames = p.patientTypeNames,
                patientName = p.patientName,
                patientSexNO = p.patientSexNO,
                patientSexNames = p.patientSexNames,
                ageYear = p.ageYear,
                ageMoth = p.ageMoth,
                ageDay = p.ageDay,
                ageNames = p.ageNames,
                patientbirthday = p.patientbirthday,
                departmentNO = p.departmentNO,
                department = p.department,
                bedNo = p.bedNo,
                patientPhone = p.patientPhone,
                patientCardNo = p.patientCardNo,
                passportNo = p.passportNo,
                patientAddress = p.patientAddress,
                sendDoctor = p.sendDoctor,
                doctorPhone = p.doctorPhone,
                pathologyNo = p.pathologyNo,
                cutPart = p.cutPart,
                menstrualTime = p.menstrualTime,
                sampleTypeNO = p.sampleTypeNO,
                sampleTypeNames = p.sampleTypeNames,
                sampleShapeNO = p.sampleShapeNO,
                sampleShapeNames = p.sampleShapeNames,
                clinicalDiagnosis = p.clinicalDiagnosis,
                sampleLocation = p.sampleLocation,
                perRemark = p.perRemark,
                testRemark = p.testRemark,
                applyItemCodes = p.applyItemCodes,
                applyItemNames = p.applyItemNames,
                groupCodes = p.groupCodes,
                groupNames = p.groupNames,
                groupFlowNO = p.groupFlowNO,
                testStateNO = p.testStateNO,
                pathologyStateNO = p.pathologyStateNO,
                delegateState = p.delegateState,
                delstateClientNO = p.delstateClientNO,
                reportState = p.reportState,
                state = p.state,
                urgent = p.urgent,
                number = p.number,
                tester = p.tester,
                testTime = p.testTime,
                reTester = p.reTester,
                reTestTime = p.reTestTime,
                checker = p.checker,
                checkTime = p.checkTime,
                creater = p.creater,
                createTime = p.createTime,
                testReceive = p.testReceive,
                testReceiveTime = p.testReceiveTime,
                editer = p.editer,
                editTime = p.editTime,
                disState = p.disState,
                realtester = p.realtester,
                realtestTime = p.realtestTime,
                realreTester = p.realreTester,
                realreTestTime = p.realreTestTime,
                realchecker = p.realchecker,
                realcheckTime = p.realcheckTime,
                visible = p.visible,
                dstate = p.dstate,
                xgState = p.xgState,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<per_sampleblend>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
