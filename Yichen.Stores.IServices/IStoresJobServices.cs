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
using Yichen.Stores.Model;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;
using Yichen.Comm.IServices;
using NPOI.SS.Formula.Functions;
using Yichen.Comm.Model;

namespace Yichen.Stores.IServices
{
	/// <summary>
    ///  服务工厂接口
    /// </summary>
    public interface IStoresJobServices : IBaseServices<sw_record>
    {
        /// <summary>
        /// 刷新存储标本记录
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> refreshRecord();

        /// <summary>
        /// 刷新标本架状态
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> refreshShelf();

    }
}
