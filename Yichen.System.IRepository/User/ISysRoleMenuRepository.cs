using Yichen.Comm.IRepository;
using Yichen.Net.Model.Entities;

namespace Yichen.System.IRepository
{
    /// <summary>
    ///     角色菜单关联表 工厂接口
    /// </summary>
    public interface ISysRoleMenuRepository : IBaseRepository<SysRoleMenu>
    {
        Task<List<SysRoleMenu>> RoleModuleMaps();
    }
}
