/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.System.IRepository;
using Yichen.System.Model;


namespace Yichen.System.Repository
{
    /// <summary>
    ///     用户日志 接口实现
    /// </summary>
    public class UserLogRepository : BaseRepository<sys_user_log>, IUserLogRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}