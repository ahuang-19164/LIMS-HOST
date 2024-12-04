/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


using SqlSugar;

namespace Yichen.Comm.IRepository.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 获取DBclient
        /// </summary>
        /// <returns></returns>
        SqlSugarScope GetDbClient();

        /// <summary>
        /// 开始
        /// </summary>
        void BeginTran();
        /// <summary>
        /// 提交
        /// </summary>
        void CommitTran();
        /// <summary>
        /// 回滚
        /// </summary>
        void RollbackTran();
    }
}