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
using Yichen.Net.Caching.Manual;
using Yichen.Net.Configuration;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;
using Yichen.Comm.Repository;
using Yichen.Other.Model.table;
using Yichen.Other.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;

namespace Yichen.Other.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class OtherManagerRepository : BaseRepository<object>, IOtherManagerRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public OtherManagerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


    }
}
