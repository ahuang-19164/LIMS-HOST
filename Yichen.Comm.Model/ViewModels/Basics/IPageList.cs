/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

namespace Yichen.Comm.Model.ViewModels.Basics
{
    public interface IPageList<T> : IList<T>
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// 分页大小
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// 总数
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// 总页数
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}