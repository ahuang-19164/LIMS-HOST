﻿/***********************************************************************
 *            Project: Yichen.Net                                     *
 *                Web: https://Yichen.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *           Versions: 1.0                                             *
 *         CreateTime: 2020-02-03 22:45:34
 *           FileName: SqlSugarSetup
 *   ClassDescription: 
 ***********************************************************************/


using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using SqlSugar.IOC;
using System;
using Yichen.Net.Configuration;
using Yichen.Net.Loging;

namespace Yichen.Net.Core.Config
{
    /// <summary>
    /// SqlSugar 启动服务
    /// </summary>
    public static class SqlSugarSetup
    {

        public static void AddSqlSugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //注入 ORM
            SugarIocServices.AddSqlSugar(new IocConfig()
            {
                //数据库连接
                ConnectionString = AppSettingsConstVars.DbSqlConnection,
                //判断数据库类型
                DbType = AppSettingsConstVars.DbDbType == IocDbType.MySql.ToString() ? IocDbType.MySql : IocDbType.SqlServer,
                //是否开启自动关闭数据库连接-//不设成true要手动close
                IsAutoCloseConnection = true,
            });

            //设置参数
            services.ConfigurationSugar(db =>
            {
                db.CurrentConnectionConfig.InitKeyType = InitKeyType.Attribute;
                //db.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices()
                //{
                //    //判断是否开启redis设置二级缓存方式
                //    DataInfoCacheService = AppSettingsConstVars.RedisUseCache ? (ICacheService)new SqlSugarRedisCache() : new SqlSugarMemoryCache()
                //};
                //执行SQL 错误事件，可监控sql（暂时屏蔽，需要可开启）
                db.Aop.OnLogExecuting = (sql, p) =>
                {
                    NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Other, "SqlSugar执行SQL错误事件打印Sql", sql);
                };

                //执行SQL 错误事件
                db.Aop.OnError = (exp) =>
                {
                    NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Other, "SqlSugar", "执行SQL错误事件", exp);
                };


                //设置更多连接参数
                //db.CurrentConnectionConfig.XXXX=XXXX
                //db.CurrentConnectionConfig.MoreSetting=new MoreSetting(){}
                //读写分离等都在这儿设置

                #region 开启读写分离配置

                //db.CurrentConnectionConfig.SlaveConnectionConfigs = new List<SlaveConnectionConfig>() {
                //        new SlaveConnectionConfig()
                //        {
                //            HitRate=10,ConnectionString="server=.;uid=sa;pwd=123456;database=SqlSugarTest_001"
                //        },
                //        new SlaveConnectionConfig()
                //        {
                //            HitRate=10,ConnectionString="server=.;uid=sa;pwd=123456;database=SqlSugarTest_002"
                //        },
                //        new SlaveConnectionConfig()
                //        {
                //            HitRate=10,ConnectionString="server=.;uid=sa;pwd=123456;database=SqlSugarTest_003"
                //        }
                //    };


                #endregion



            });

        }
    }
}
//try
//{
//    SqlSugarClient sqlSugarClient = new SqlSugarClient(new ConnectionConfig
//    {
//        DbType = DbType.SqlServer,//要连接的数据库类型
//        ConnectionString = "server=.;uid=sa;pwd=123456;database=SqlSugarTest",//sqlsqver数据库链接字符串 (主库)
//        IsAutoCloseConnection = true,
//        InitKeyType = InitKeyType.Attribute,

//        //从库
//        SlaveConnectionConfigs = new List<SlaveConnectionConfig>() {
//                        new SlaveConnectionConfig()
//                        {
//                            HitRate=10,ConnectionString="server=.;uid=sa;pwd=123456;database=SqlSugarTest_001"
//                        },
//                        new SlaveConnectionConfig()
//                        {
//                            HitRate=10,ConnectionString="server=.;uid=sa;pwd=123456;database=SqlSugarTest_002"
//                        },
//                        new SlaveConnectionConfig()
//                        {
//                            HitRate=10,ConnectionString="server=.;uid=sa;pwd=123456;database=SqlSugarTest_003"
//                        }
//                    }
//    });


//    //新增一条记录,往主库插入记录
//    sqlSugarClient.Insertable<Commodity>(new Commodity()
//    {
//        ProductId = 1,
//        CategoryId = 1,
//        Title = "测试数据库读写分离",
//        Price = 1,
//        Url = "测试数据库读写分离",
//        ImageUrl = "测试数据库读写分离"
//    }).ExecuteCommand();
//}
//catch (Exception)
//{

//    throw;
//}
