/***********************************************************************
 *            Project: Yichen
 *        ProjectName: 屹辰智禾管理系统                                
 *                Web: https://www.zui51.com                 
 *             Author: 屹辰                                       
 *              Email: 499715561@qq.com                              
 *         CreateTime: 2023-11-16 11:56:59
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yichen.Net.Configuration;
using Yichen.Net.IRepository;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;
using Yichen.Comm.Services;
using Yichen.Stores.Model;
using Yichen.Stores.IServices;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Stores.IRepository;
using Yichen.Comm.Model;

namespace Yichen.Stores.Services
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class StoresJobServices : BaseServices<sw_record>, IStoresJobServices
    {
        private readonly IStoresJobRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public StoresJobServices(IUnitOfWork unitOfWork, IStoresJobRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 刷新存储标本记录
        /// </summary>
        /// <returns></returns>
        public  async Task<WebApiCallBack> refreshRecord()
        {
            var jm = new WebApiCallBack();
            jm.code = 0;
            jm.status = true;
            jm.data= await _dal.refreshRecord();
            return jm;
        }


        /// <summary>
        /// 刷新标本架状态
        /// </summary>
        /// <returns></returns>
        public  async Task<WebApiCallBack> refreshShelf()
        {
            var jm = new WebApiCallBack();
            jm.code = 0;
            jm.status = true;
            jm.data = await _dal.refreshShelf();
            return jm;
        }
    }
}
