/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using Yichen.Net.Model.Entities;
using Yichen.Net.Model.FromBody;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.Options;
using Yichen.Comm.Model.ViewModels.Sms;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.IServices;

namespace Yichen.Net.IServices
{
    /// <summary>
    ///     店铺设置表 服务工厂接口
    /// </summary>
    public interface ICoreCmsSettingServices : IBaseServices<CoreCmsSetting>
    {
        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(FMCoreCmsSettingDoSaveModel entity);

        /// <summary>
        ///     获取数据库整合后配置信息
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, DictionaryKeyValues>> GetConfigDictionaries();


        /// <summary>
        ///     获取附件存储的配置信息
        /// </summary>
        /// <returns></returns>
        Task<FilesStorageOptions> GetFilesStorageOptions();

        /// <summary>
        ///     获取短信配置实体
        /// </summary>
        /// <returns></returns>
        Task<SMSOptions> GetSmsOptions();
    }
}