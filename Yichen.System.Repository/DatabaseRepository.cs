
using SqlSugar;
using System.Data;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Auth.Policys;
using Yichen.System.IRepository;

namespace Yichen.System.Repository
{
    public partial class DatabaseRepository : BaseRepository<object>, IDatabaseRepository
    {
        private readonly IHttpContextUser _httpContextUser;
        private readonly PermissionRequirement _permissionRequirement;
        public DatabaseRepository(IUnitOfWork unitOfWork
            , IHttpContextUser httpContextUser
            , PermissionRequirement permissionRequirement
            ) : base(unitOfWork)
        {
            _httpContextUser = httpContextUser;
            _permissionRequirement = permissionRequirement;
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DbTableInfo>> GetDbTables() 
        {

            var tables = DbClient.DbMaintenance.GetTableInfoList(false).OrderBy(p => p.Name).ToList();
            return tables;
        }

        /// <summary>
        /// 获取视图表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DbTableInfo>> GetDbViews()
        {
            var views = DbClient.DbMaintenance.GetViewInfoList(false).OrderBy(p => p.Name).ToList();
            return views;
        }
        /// <summary>
        /// 获取表下面所有的字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<List<DbColumnInfo>> GetDbColumns(string tableName)
        {
            var columns = DbClient.DbMaintenance.GetColumnInfosByTableName(tableName, false).ToList();
            return columns;
        }

    }
}
