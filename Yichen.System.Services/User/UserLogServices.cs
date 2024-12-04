/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/
using System.Linq.Expressions;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;
using Yichen.Comm.Services;
using Yichen.System.Model;
using Yichen.System.IServices;
using Yichen.System.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;

namespace Yichen.System.Services
{
    /// <summary>
    /// 用户日志 接口实现
    /// </summary>
    public class UserLogServices : BaseServices<sys_user_log>, IUserLogServices
    {
        private readonly IUserLogRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public UserLogServices(IUnitOfWork unitOfWork, IUserLogRepository dal)
        {
            _dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


    }
}
