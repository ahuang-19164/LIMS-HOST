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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yichen.Net.Configuration;
using Yichen.Net.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Net.IServices;
using Yichen.Net.Model.Entities;
using Yichen.Comm.Model.ViewModels.Basics;
using SqlSugar;
using Yichen.Comm.Services;


namespace Yichen.Net.Services
{
    /// <summary>
    /// 服务项目表 接口实现
    /// </summary>
    public class CoreCmsServicesServices : BaseServices<CoreCmsServices>, ICoreCmsServicesServices
    {
        private readonly ICoreCmsServicesRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsServicesServices(IUnitOfWork unitOfWork, ICoreCmsServicesRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsServices>> TagQueryPageAsync(Expression<Func<CoreCmsServices, bool>> predicate,
            Expression<Func<CoreCmsServices, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {

            return await _dal.TagQueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize);
        }


    }
}
