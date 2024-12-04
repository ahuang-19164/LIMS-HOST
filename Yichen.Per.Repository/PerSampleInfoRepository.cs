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

namespace Yichen.Per.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class PerSampleInfoRepository : BaseRepository<per_sampleInfo>, IPerSampleInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public PerSampleInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public  async Task<bool> BarcodeExist(string barcode)
        {
            if (barcode != null && barcode != "")
                return false;
            return await DbClient.Queryable<per_sampleInfo>().AnyAsync(p => p.barcode == barcode && p.dstate == false);
        }

        /// <summary>
        /// 根据条码获取样本信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public  async Task<per_sampleInfo> GetByBarcode(string barcode)
        {
            return await DbClient.Queryable<per_sampleInfo>().FirstAsync(p => p.barcode == barcode && p.dstate == false);
        }
        /// <summary>
        /// 录入信息审核状态修改
        /// </summary>
        /// <param name="perid">录入信息id</param>
        /// <param name="reciveTime">接收时间</param>
        /// <param name="blendConut">样本信息数量</param>
        /// <returns></returns>
        public async Task<int> PerCheckInfo(int perid, DateTime reciveTime,string userName , int blendConut = 1)
        {
            //updateperInfo = $"update WorkPer.SampleInfo set receiveTime='{receiveTime}',perStateNO=2,state=1,checker='{infos.UserName}',checkTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where barcode='{sampleInfo.barcode}' and id='{sampleInfo.perid}';";

            return await DbClient.Updateable<per_sampleInfo>(p=>
            new per_sampleInfo()
            {
                perStateNO = 2,
                state = true,
                checker = userName,
                checkTime = DateTime.Now,
                receiveTime = reciveTime,
                number = blendConut
            }).Where(p => p.id == perid).ExecuteCommandAsync();








            //var perInfo = new per_sampleInfo()
            //{
            //    perStateNO=2 ,
            //    state=true ,
            //    checker=userName ,
            //    checkTime=DateTime.Now,
            //    receiveTime=reciveTime,
            //    number=blendConut
            //};
            //return await DbClient.Updateable<per_sampleInfo>(perInfo).Where(p => p.id == perid).ExecuteCommandAsync();


            //return await DbClient.Updateable<per_sampleInfo>().SetColumns(p =>
            //p.perStateNO == 2
            //&& p.state == true
            //&& p.checker == userName
            //&& p.checkTime == DateTime.Now
            //&& p.receiveTime == reciveTime
            //&& p.number == blendConut)
            //.Where(p => p.id == perid).ExecuteCommandAsync();

        }




        /// <summary>
        /// 删除录入条码信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public async Task<bool> BarcodeDelete(per_sampleInfo sampleinfo)
        {
            SampleInfoDelete deleteInfo=new SampleInfoDelete()
            {
                perid = sampleinfo.id,
                patientid = sampleinfo.patientid,
                barcode = sampleinfo.barcode,
                hospitalNO = sampleinfo.hospitalNO,
                hospitalNames = sampleinfo.hospitalNames,
                agentNO = sampleinfo.agentNO,
                agentNames = sampleinfo.agentNames,
                hospitalBarcode = sampleinfo.hospitalBarcode,
                frameNo = sampleinfo.frameNo,
                medicalNo = sampleinfo.medicalNo,
                sampleTime = sampleinfo.sampleTime,
                receiveTime = sampleinfo.receiveTime,
                patientTypeNO = sampleinfo.patientTypeNO,
                patientTypeNames = sampleinfo.patientTypeNames,
                patientName = sampleinfo.patientName,
                patientSexNO = sampleinfo.patientSexNO,
                patientSexNames = sampleinfo.patientSexNames,
                ageYear = sampleinfo.ageYear,
                ageMoth = sampleinfo.ageMoth,
                ageDay = sampleinfo.ageDay,
                sampleAddress = sampleinfo.sampleAddress,
                department = sampleinfo.department,
                bedNo = sampleinfo.bedNo,
                patientPhone = sampleinfo.patientPhone,
                patientCardNo = sampleinfo.patientCardNo,
                passportNo = sampleinfo.passportNo,
                patientAddress = sampleinfo.patientAddress,
                sendDoctor = sampleinfo.sendDoctor,
                doctorPhone = sampleinfo.doctorPhone,
                pathologyNo = sampleinfo.pathologyNo,
                cutPart = sampleinfo.cutPart,
                menstrualTime = sampleinfo.menstrualTime,
                sampleTypeNO = sampleinfo.sampleTypeNO,
                sampleTypeNames = sampleinfo.sampleTypeNames,
                sampleShapeNO = sampleinfo.sampleShapeNO,
                sampleShapeNames = sampleinfo.sampleShapeNames,
                clinicalDiagnosis = sampleinfo.clinicalDiagnosis,
                sampleLocation = sampleinfo.sampleLocation,
                perRemark = sampleinfo.perRemark,
                applyItemCodes = sampleinfo.applyItemCodes,
                applyItemNames = sampleinfo.applyItemNames,
                perStateNO = sampleinfo.perStateNO,
                urgent = sampleinfo.urgent,
                number = sampleinfo.number,
                state = sampleinfo.state,
                creater = sampleinfo.creater,
                createTime = sampleinfo.createTime,
                editer = sampleinfo.editer,
                editTime = sampleinfo.editTime,
                checker = sampleinfo.checker,
                checkTime = sampleinfo.checkTime,
                dstate = sampleinfo.dstate,
                connstate = sampleinfo.connstate,
                sortState = sampleinfo.sortState

            };
            int a = await DbClient.Deleteable<per_sampleInfo>().Where(p=>p.id==sampleinfo.id).ExecuteCommandAsync();
            if(a>0)
            {
                int b = await DbClient.Insertable<SampleInfoDelete>(deleteInfo).ExecuteCommandAsync();
                return b == 0 ? false : true;
            }
            return false;
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
                return await DbClient.Queryable<per_sampleInfo>()
                    .Where(p => SqlFunc.Between(p.createTime, startTime, endTime))
                    .ToDataTableAsync();
            }
            else
            {
                return await DbClient.Queryable<per_sampleInfo>()
                    .Where(p => SqlFunc.Between(p.createTime, startTime, endTime))
                    .Where(p=>p.creater==userName)
                    .ToDataTableAsync();
            }
        }


        /// <summary>
        /// 插入录入标本信息,返回插入信息id
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public  async Task<int> EntryInfo(Dictionary<string, object> info)
        {
            return await DbClient.Insertable<per_sampleInfo>(info).ExecuteReturnIdentityAsync();
        }
        /// <summary>
        /// 修改插入录入标本信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public  async Task<int> EntryInfoEdit(int perid, Dictionary<string, object> info)
        {
            return DbClient.Updateable<per_sampleInfo>(info).Where(p=>p.id==perid).ExecuteCommand();
        }


        #region 实现重写增删改查操作==========================================================


        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>

        public  async Task<WebApiCallBack> InsertAsync(per_sampleInfo entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(per_sampleInfo entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<per_sampleInfo>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.patientid = entity.patientid;
            oldModel.barcode = entity.barcode;
            oldModel.hospitalNO = entity.hospitalNO;
            oldModel.hospitalNames = entity.hospitalNames;
            oldModel.agentNO = entity.agentNO;
            oldModel.agentNames = entity.agentNames;
            oldModel.hospitalBarcode = entity.hospitalBarcode;
            oldModel.frameNo = entity.frameNo;
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
            oldModel.sampleAddress = entity.sampleAddress;
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
            oldModel.connstate = entity.connstate;
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
        public  async Task<WebApiCallBack> UpdateAsync(List<per_sampleInfo> entity)
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

            var bl = await DbClient.Deleteable<per_sampleInfo>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<per_sampleInfo>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<per_sampleInfo>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        //public async Task<List<per_sampleInfo>> GetCaChe()
        //{
        //    var cache = ManualDataCache.Instance.Get<List<per_sampleInfo>>(GlobalConstVars.CacheSampleImg);
        //    if (cache != null)
        //    {
        //        return cache;
        //    }
        //    return await UpdateCaChe();
        //}

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //public async Task<List<per_sampleInfo>> UpdateCaChe()
        //{
        //    var list = await DbClient.Queryable<per_sampleInfo>().With(SqlWith.NoLock).ToListAsync();
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
        //public new async Task<List<per_sampleInfo>> QueryAsync(
        //    Expression<Func<per_sampleInfo, bool>> predicate,
        //    Expression<Func<per_sampleInfo, object>> orderByExpression, OrderByType orderByType, bool blUseNoLock = false)
        //{
        //    if (blUseNoLock)
        //    {
        //        return await DbClient.Queryable<per_sampleInfo>()
        //        .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
        //        .WhereIF(predicate != null, predicate)
        //        .ToListAsync();
        //    }
        //    else
        //    {
        //        return await DbClient.Queryable<per_sampleInfo>()
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
        public  async Task<IPageList<per_sampleInfo>> QueryPageAsync(Expression<Func<per_sampleInfo, bool>> predicate,
            Expression<Func<per_sampleInfo, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<per_sampleInfo> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<per_sampleInfo>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<per_sampleInfo>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<per_sampleInfo>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
