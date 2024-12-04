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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yichen.Net.Configuration;
using Yichen.Net.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Net.IServices;
using Yichen.Net.Loging;
using Yichen.Net.Model.Entities;
using Yichen.Net.Model.Entities.Expression;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;
using StackExchange.Redis;
using Yichen.Comm.Services;


namespace Yichen.Net.Services
{
    /// <summary>
    /// 商品收藏表 接口实现
    /// </summary>
    public class CoreCmsGoodsCollectionServices : BaseServices<CoreCmsGoodsCollection>, ICoreCmsGoodsCollectionServices
    {
        private readonly ICoreCmsGoodsCollectionRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsGoodsCollectionServices(IUnitOfWork unitOfWork, ICoreCmsGoodsCollectionRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 检查是否收藏了此商品
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<bool> Check(int userId, int goodsId)
        {
            var bl =await _dal.ExistsAsync(p => p.userId == userId && p.goodsId == goodsId);
            return bl;
        }



        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsGoodsCollection>> QueryPageAsync(Expression<Func<CoreCmsGoodsCollection, bool>> predicate,
            Expression<Func<CoreCmsGoodsCollection, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize);
        }



        /// <summary>
        /// 如果收藏了，就取消收藏，如果没有收藏，就收藏
        /// </summary>
        public async Task<WebApiCallBack> ToDo(int userId, int goodsId)
        {
            var collectionInfo = await _dal.ExistsAsync(p => p.userId == userId && p.goodsId == goodsId);
            if (collectionInfo)
            {
                return await ToDel(userId, goodsId);
            }
            else
            {
                return await ToAdd(userId, goodsId);
            }
        }


        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        private async Task<WebApiCallBack> ToDel(int userId, int goodsId)
        {
            var jm = new WebApiCallBack() { status = true, msg = "取消收藏成功" };
            await _dal.DeleteAsync(p => p.userId == userId && p.goodsId == goodsId);
            return jm;
        }


        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ToAdd(int userId, int goodsId)
        {
            return await _dal.ToAdd(userId, goodsId);
        }
    }
}
