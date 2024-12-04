using SqlSugar;

namespace Yichen.Net.Sqlsugar
{
    /// <summary>
    /// 利用sqlsugar生成model
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            //用单例模式
            SqlSugarScope db = new SqlSugarScope(new ConnectionConfig()
            {
                ConnectionString = "Server=47.98.206.71;uid=sa;pwd=abc123,;Database=HLIMSDB;MultipleActiveResultSets=true;pooling=true;min pool size=5;max pool size=32767;connect timeout=20;Encrypt=True;TrustServerCertificate=True;",//连接符字串
                DbType = DbType.SqlServer,//数据库类型
                IsAutoCloseConnection = true //不设成true要手动close

            });



            //db.DbFirst.IsCreateDefaultValue().CreateClassFile(@"D:\testmodel\Yichen.ScheduleTask.Model", "Yichen.Net.LIMSModel");

            //db.DbFirst.IsCreateDefaultValue().Where("Finance.GroupChargeInfo").CreateClassFile(@"D:\limsmodel\Yichen.Test.Model", "Yichen.Per.Model");



            ///生成所有数据库表的model对象
            db.DbFirst.CreateClassFile(@"D:\testmodel\Yichen.ScheduleTask.Model", "Yichen.Net.LIMSModel");


            ////指定表名生成实体
            //db.DbFirst.Where("Article").CreateClassFile(@"D:\VS_Space\XOA.ScheduleTask\XOA.ScheduleTask.Model", "命名空间");
            ////根据条件搜索要生成的表
            //db.DbFirst.Where(it => it.ToLower().StartsWith("Blog")).CreateClassFile(@"D:\VS_Space\XOA.ScheduleTask\XOA.ScheduleTask.Model", "XOA.ScheduleTask.Model");
            ////生成的时候如果数据库有设置默认值同步生成。
            ////可以和其他的结合起来使用其实就是一个IsCreateDefaultValue方法而已。
            //db.DbFirst.IsCreateDefaultValue().CreateClassFile(@"D:\VS_Space\XOA.ScheduleTask\XOA.ScheduleTask.Model", "命名空间");


            //生成的时候取表的别名，或者列的别名。
            //db.DbMaintenance.GetTableInfoList 可以拿到所有的表名，方便统一规则设置
            //   db.MappingTables.Add("MyCollect", "Student");
            //   db.MappingColumns.Add("MyId", "Id", "MyFocus");
            //   db.DbFirst.IsCreateAttribute().Where("Article").CreateClassFile(@"D:\VS_Space\XOA.ScheduleTask\XOA.ScheduleTask.Model", "命名空间");
            //   //自定义生成规则

            //   db.DbFirst.
            //   SettingClassTemplate(old =>
            //        {
            //            return old;
            //        })
            //        .SettingNamespaceTemplate(old =>
            //        {
            //   //修改using命名空间
            //            return old;
            //        })
            //        .SettingPropertyDescriptionTemplate(old =>
            //        {
            //            return @" 
            //    /// <summary>
            //    /// Desc_My:{PropertyDescription}
            //    /// Default_My:{DefaultValue}
            //    /// Nullable_My:{IsNullable}
            //    /// </summary>";
            //        })
            //        .SettingPropertyTemplate(old =>
            //        {
            //            return old;
            //        })
            //        .SettingConstructorTemplate(old =>
            //        {
            ////修改构造函数
            //            return old;
            //        })
            //    .CreateClassFile(@"D:\VS_Space\XOA.ScheduleTask\XOA.ScheduleTask.Model", "命名空间");



        }
    }
}
