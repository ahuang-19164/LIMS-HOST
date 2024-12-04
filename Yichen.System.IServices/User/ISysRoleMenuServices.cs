using Yichen.Comm.IServices;
using Yichen.Net.Model.Entities;

namespace Yichen.System.IServices.User
{
    /// <summary>
    ///     角色菜单关联表 服务工厂接口
    /// </summary>
    public interface ISysRoleMenuServices : IBaseServices<SysRoleMenu>
    {
        Task<List<SysRoleMenu>> RoleModuleMaps();
    }
}
