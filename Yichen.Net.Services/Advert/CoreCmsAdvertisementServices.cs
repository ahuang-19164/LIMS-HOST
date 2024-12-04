/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using Yichen.Net.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Net.IServices;
using Yichen.Net.Model.Entities;
using Yichen.Comm.Services;

namespace Yichen.Net.Services
{
    /// <summary>
    ///     广告表 接口实现
    /// </summary>
    public class CoreCmsAdvertisementServices : BaseServices<CoreCmsAdvertisement>, ICoreCmsAdvertisementServices
    {
        private readonly ICoreCmsAdvertisementRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public CoreCmsAdvertisementServices(IUnitOfWork unitOfWork, ICoreCmsAdvertisementRepository dal)
        {
            _dal = dal;
            BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}