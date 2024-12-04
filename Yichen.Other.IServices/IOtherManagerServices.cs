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
using Yichen.Comm.IServices;
using Yichen.Other.Model.table;

namespace Yichen.Other.IServices
{
	/// <summary>
    ///  服务工厂接口
    /// </summary>
    public interface IOtherManagerServices : IBaseServices<object>
    {

        Task<WebApiCallBack> GetSyntheticalInfo(ClientRecord entity);

    }
}
