using System.Threading.Tasks;
using Yichen.Comm.IRepository;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Net.Model.Entities;
using Yichen.Net.Model.FromBody;
namespace Yichen.Net.IRepository
{
    /// <summary>
    ///     商品类型属性表 工厂接口
    /// </summary>
    public interface ICoreCmsGoodsTypeSpecRepository : IBaseRepository<CoreCmsGoodsTypeSpec>
    {
        /// <summary>
        ///     重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> InsertAsync(FmGoodsTypeSpecInsert entity);


        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(FmGoodsTypeSpecUpdate entity);


        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DeleteByIdAsync(object id);
    }
}