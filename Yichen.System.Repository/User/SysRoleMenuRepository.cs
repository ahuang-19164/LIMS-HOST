using SqlSugar;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.Net.Model.Entities;
using Yichen.System.IRepository;

namespace Yichen.System.Repository
{
    /// <summary>
    ///     角色菜单关联表 接口实现
    /// </summary>
    public class SysRoleMenuRepository : BaseRepository<SysRoleMenu>, ISysRoleMenuRepository
    {
        public SysRoleMenuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        /// <summary>
        ///     角色权限Map
        ///     RoleModulePermission, Module, Role 三表联合
        ///     第四个类型 RoleModulePermission 是返回值
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysRoleMenu>> RoleModuleMaps()
        {
            return await QueryMuchAsync<SysRoleMenu, SysMenu, SysRole, SysRoleMenu>(
                (rmp, m, r) => new object[]
                {
                    JoinType.Left, rmp.menuId == m.id,
                    JoinType.Left, rmp.roleId == r.id
                },
                (rmp, m, r) => new SysRoleMenu
                {
                    role = r,
                    menu = m
                },
                (rmp, m, r) => m.deleted == false && r.deleted == false
            );
        }
    }
}
