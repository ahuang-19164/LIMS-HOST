using Autofac;
using System;
using System.IO;
using System.Reflection;
using Yichen.Net.Configuration;
using Yichen.Net.DLLs;

namespace Yichen.Net.Core.AutoFac
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;

            #region 带有接口层的服务注入

            //var servicesDllFile = Path.Combine(basePath, "Yichen.Net.Services.dll");
            //var repositoryDllFile = Path.Combine(basePath, "Yichen.Net.Repository.dll");

            if (AppSettingsConstVars.servicefiles == null && AppSettingsConstVars.servicefiles.Length == 0)
            {
                var msg = "未读取到服务配置信息，请假查配置文件！";
                throw new Exception(msg);
            }
            else
            {
                foreach (string servicefile in AppSettingsConstVars.servicefiles)
                {
                    var servicesDllFile = Path.Combine(basePath, servicefile) + ".dll";
                    if (!File.Exists(servicesDllFile))
                    {
                        //var msg = $"{servicefile} 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                        var msg = $"{servicefile} 丢失，请检查文件是否存在";
                        throw new Exception(msg);
                    }
                    else
                    {
                        // 获取 Service.dll 程序集服务，并注册
                        var assemblysServices = Assembly.LoadFrom(servicesDllFile);
                        //支持属性注入依赖重复
                        builder.RegisterAssemblyTypes(assemblysServices)
                            .AsImplementedInterfaces()
                            .InstancePerDependency()
                            .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
                        Console.WriteLine(servicefile + "服务加载成功！");
                    }

                }

                foreach (string repositoryfile in AppSettingsConstVars.repositoryfiles)
                {
                    var servicesDllFile = Path.Combine(basePath, repositoryfile) + ".dll";
                    if (!File.Exists(servicesDllFile))
                    {
                        //var msg = $"{servicefile} 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                        var msg = $"{repositoryfile} 丢失，请检查文件是否存在";
                        throw new Exception(msg);
                    }
                    else
                    {
                        // 获取 Service.dll 程序集服务，并注册
                        var assemblysServices = Assembly.LoadFrom(servicesDllFile);
                        //支持属性注入依赖重复
                        builder.RegisterAssemblyTypes(assemblysServices)
                            .AsImplementedInterfaces()//指定将扫描程序集中的类型注册为提供所有其实现的接口。
                            .InstancePerDependency()//配置组件，以便每个依赖组件或调用 Resolve（）获取一个新的唯一实例（默认）。
                            .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);//配置组件，以便其类型注册的任何属性容器将连接到相应服务的实例。
                        Console.WriteLine(repositoryfile + "服务加载成功！");
                    }

                }



                //Console.WriteLine("服务正在加载缓存。。");
                //StartupHelper.loadbaseinfo();

                //Console.WriteLine("缓存加载完成");
                Console.WriteLine("HLIMS服务启动成功！");



                //List<comm_item_flow> testFlowDT = CommInfo.itemflow.data

                //Console.WriteLine(testFlowDT[1].names);

                //comm_item_flow testFlowInfo = testFlowDT.First(p => p.no == Convert.ToInt32(testFlowNO));

            }


            #endregion








            //var basePath = AppContext.BaseDirectory;

            //#region 带有接口层的服务注入

            //var servicesDllFile = Path.Combine(basePath, "Yichen.Net.Services.dll");
            //var repositoryDllFile = Path.Combine(basePath, "Yichen.Net.Repository.dll");

            //if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            //{
            //    var msg = "Repository.dll和Services.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
            //    throw new Exception(msg);
            //}

            //// AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
            ////var cacheType = new List<Type>();
            ////if (AppSettingsConstVars.RedisConfigEnabled)
            ////{
            ////    builder.RegisterType<RedisCacheAop>();
            ////    cacheType.Add(typeof(RedisCacheAop));
            ////}
            ////else
            ////{
            ////    builder.RegisterType<MemoryCacheAop>();
            ////    cacheType.Add(typeof(MemoryCacheAop));
            ////}

            //// 获取 Service.dll 程序集服务，并注册
            //var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            ////支持属性注入依赖重复
            //builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces().InstancePerDependency()
            //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            //// 获取 Repository.dll 程序集服务，并注册
            //var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            ////支持属性注入依赖重复
            //builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces().InstancePerDependency()
            //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);


            //// 获取 Service.dll 程序集服务，并注册
            ////var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            ////builder.RegisterAssemblyTypes(assemblysServices)
            ////    .AsImplementedInterfaces()
            ////    .InstancePerDependency()
            ////    .PropertiesAutowired()
            ////    .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
            ////    .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。

            ////// 获取 Repository.dll 程序集服务，并注册
            ////var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            ////builder.RegisterAssemblyTypes(assemblysRepository)
            ////    .AsImplementedInterfaces()
            ////    .PropertiesAutowired()
            ////    .InstancePerDependency();


            //#endregion

        }
    }
}
