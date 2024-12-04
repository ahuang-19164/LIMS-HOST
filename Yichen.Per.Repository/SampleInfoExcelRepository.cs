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
    public class SampleInfoExcelRepository : BaseRepository<SampleInfoExcel>, ISampleInfoExcelRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SampleInfoExcelRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(SampleInfoExcel entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(SampleInfoExcel entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<SampleInfoExcel>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.barcode = entity.barcode;
            oldModel.hospitalNo = entity.hospitalNo;
            oldModel.hospitalName = entity.hospitalName;
            oldModel.agentNo = entity.agentNo;
            oldModel.agentName = entity.agentName;
            oldModel.hospitalBarcode = entity.hospitalBarcode;
            oldModel.medicalNo = entity.medicalNo;
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
            oldModel.department = entity.department;
            oldModel.bedNo = entity.bedNo;
            oldModel.patientPhone = entity.patientPhone;
            oldModel.patientCardNo = entity.patientCardNo;
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
            oldModel.applyItemCodes = entity.applyItemCodes;
            oldModel.applyItemNames = entity.applyItemNames;
            oldModel.perStateNO = entity.perStateNO;
            oldModel.urgent = entity.urgent;
            oldModel.number = entity.number;
            oldModel.state = entity.state;
            oldModel.creater = entity.creater;
            oldModel.createTime = entity.createTime;
            oldModel.editer = entity.editer;
            oldModel.editTime = entity.editTime;
            oldModel.checker = entity.checker;
            oldModel.checkTime = entity.checkTime;
            oldModel.dstate = entity.dstate;
            
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
        public  async Task<WebApiCallBack> UpdateAsync(List<SampleInfoExcel> entity)
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

            var bl = await DbClient.Deleteable<SampleInfoExcel>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<SampleInfoExcel>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<SampleInfoExcel>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        //public async Task<List<SampleInfoExcel>> GetCaChe()
        //{
        //    var cache = ManualDataCache.Instance.Get<List<SampleInfoExcel>>(GlobalConstVars.CacheSampleInfoExcel);
        //    if (cache != null)
        //    {
        //        return cache;
        //    }
        //    return await UpdateCaChe();
        //}

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //public async Task<List<SampleInfoExcel>> UpdateCaChe()
        //{
        //    var list = await DbClient.Queryable<SampleInfoExcel>().With(SqlWith.NoLock).ToListAsync();
        //    ManualDataCache.Instance.Set(GlobalConstVars.CacheSampleInfoExcel, list);
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
        public  async Task<IPageList<SampleInfoExcel>> QueryPageAsync(Expression<Func<SampleInfoExcel, bool>> predicate,
            Expression<Func<SampleInfoExcel, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<SampleInfoExcel> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<SampleInfoExcel>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new SampleInfoExcel
                {
                      id = p.id,
                barcode = p.barcode,
                hospitalNo = p.hospitalNo,
                hospitalName = p.hospitalName,
                agentNo = p.agentNo,
                agentName = p.agentName,
                hospitalBarcode = p.hospitalBarcode,
                medicalNo = p.medicalNo,
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
                department = p.department,
                bedNo = p.bedNo,
                patientPhone = p.patientPhone,
                patientCardNo = p.patientCardNo,
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
                applyItemCodes = p.applyItemCodes,
                applyItemNames = p.applyItemNames,
                perStateNO = p.perStateNO,
                urgent = p.urgent,
                number = p.number,
                state = p.state,
                creater = p.creater,
                createTime = p.createTime,
                editer = p.editer,
                editTime = p.editTime,
                checker = p.checker,
                checkTime = p.checkTime,
                dstate = p.dstate,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<SampleInfoExcel>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new SampleInfoExcel
                {
                      id = p.id,
                barcode = p.barcode,
                hospitalNo = p.hospitalNo,
                hospitalName = p.hospitalName,
                agentNo = p.agentNo,
                agentName = p.agentName,
                hospitalBarcode = p.hospitalBarcode,
                medicalNo = p.medicalNo,
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
                department = p.department,
                bedNo = p.bedNo,
                patientPhone = p.patientPhone,
                patientCardNo = p.patientCardNo,
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
                applyItemCodes = p.applyItemCodes,
                applyItemNames = p.applyItemNames,
                perStateNO = p.perStateNO,
                urgent = p.urgent,
                number = p.number,
                state = p.state,
                creater = p.creater,
                createTime = p.createTime,
                editer = p.editer,
                editTime = p.editTime,
                checker = p.checker,
                checkTime = p.checkTime,
                dstate = p.dstate,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<SampleInfoExcel>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
