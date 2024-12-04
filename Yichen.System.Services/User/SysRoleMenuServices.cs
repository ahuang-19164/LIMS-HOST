using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Services;
using Yichen.Net.IRepository;
using Yichen.Net.Model.Entities;
using Yichen.System.IServices.User;

namespace Yichen.System.Services.User
{
    /// <summary>
    ///     角色菜单关联表 接口实现
    /// </summary>
    public class SysRoleMenuServices : BaseServices<SysRoleMenu>, ISysRoleMenuServices
    {
        private readonly ISysRoleMenuRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public SysRoleMenuServices(IUnitOfWork unitOfWork, ISysRoleMenuRepository dal)
        {
            _dal = dal;
            BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///     角色权限Map
        ///     RoleModulePermission, Module, Role 三表联合
        ///     第四个类型 RoleModulePermission 是返回值
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysRoleMenu>> RoleModuleMaps()
        {
            return await _dal.RoleModuleMaps();
        }
    }
}
