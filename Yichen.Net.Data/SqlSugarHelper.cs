using SqlSugar;
using Yichen.Net.Configuration;

namespace Yichen.Net.Data
{
    public class SqlSugarHelper
    {

        public static SqlSugarClient sugarClient;
        public SqlSugarHelper()
        {
            sugarClient = new SqlSugarClient(new ConnectionConfig()
            {
                //数据库连接
                ConnectionString = AppSettingsConstVars.DbSqlConnection,
                //判断数据库类型
                DbType = AppSettingsConstVars.DbDbType == SqlSugar.DbType.MySql.ToString() ? SqlSugar.DbType.MySql : SqlSugar.DbType.SqlServer,
                //是否开启自动关闭数据库连接-//不设成true要手动close
                IsAutoCloseConnection = true,

            });
        }
        /// <summary>
        /// 创建SqlSugarClient
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient SqlSugarClientCreate()
        {
            sugarClient = new SqlSugarClient(new ConnectionConfig()
            {
                //数据库连接
                ConnectionString = AppSettingsConstVars.DbSqlConnection,
                //判断数据库类型
                DbType = AppSettingsConstVars.DbDbType == SqlSugar.DbType.MySql.ToString() ? SqlSugar.DbType.MySql : SqlSugar.DbType.SqlServer,
                //是否开启自动关闭数据库连接-//不设成true要手动close
                IsAutoCloseConnection = true,

            });
            return sugarClient;
        }


    }
}
