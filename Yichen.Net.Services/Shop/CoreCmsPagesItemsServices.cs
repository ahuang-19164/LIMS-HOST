/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Threading.Tasks;
using Yichen.Net.Configuration;
using Yichen.Net.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Net.IServices;
using Yichen.Net.Model.Entities;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Services;


namespace Yichen.Net.Services
{
    /// <summary>
    /// 单页内容 接口实现
    /// </summary>
    public class CoreCmsPagesItemsServices : BaseServices<CoreCmsPagesItems>, ICoreCmsPagesItemsServices
    {
        private readonly ICoreCmsPagesItemsRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsPagesItemsServices(IUnitOfWork unitOfWork, ICoreCmsPagesItemsRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

    }
}
