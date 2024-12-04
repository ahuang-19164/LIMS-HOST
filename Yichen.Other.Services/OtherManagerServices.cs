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
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;
using Yichen.Comm.Services;
using Yichen.Other.Model.table;
using Yichen.Other.IServices;
using Yichen.Other.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Net.Model.Entities.Expression;
using Yichen.Per.Model.table;
using Yichen.Per.IRepository;
using System.Data;
using Yichen.Net.Table;

namespace Yichen.Other.Services
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class OtherManagerServices : BaseServices<object>, IOtherManagerServices
    {
        private readonly IOtherManagerRepository _dal;
        private readonly IPerSampleInfoRepository _perSampleInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OtherManagerServices(IUnitOfWork unitOfWork
            , IOtherManagerRepository dal
            , IPerSampleInfoRepository perSampleInfoRepository)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _perSampleInfoRepository = perSampleInfoRepository;

        }

        public async Task<WebApiCallBack> GetSyntheticalInfo(ClientRecord entity)
        {

            WebApiCallBack jm = new WebApiCallBack() { code = 0, status = true };

            var wheres = PredicateBuilder.True<per_sampleInfo>();
            //if(!string.IsNullOrEmpty(barcode))
            //{
            //    wheres = wheres.And(p => p.barcode.Contains(barcode));
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(hospitalBarcode))
            //    {
            //        wheres = wheres.And(p => p.hospitalBarcode.Contains(hospitalBarcode));
            //    }
            //    else
            //    {
            //        if (!string.IsNullOrEmpty(patientName))
            //        {
            //            wheres = wheres.And(p => p.patientName.Contains(patientName));
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(hospitalNO))
            //            {
            //                wheres = wheres.And(p => p.hospitalNO==hospitalNO);
            //            }
            //            else
            //            {
            //                wheres = wheres.And(p => SqlFunc.Between(p.createTime,startTime,endTime));
            //            }

            //        }

            //    }
            //}
            DataTable infoDT =await _perSampleInfoRepository.QueryDTByClauseAsync(wheres, true);
            jm.data = DataTableHelper.DTToString(infoDT);
            return jm;
        }

    }
}
