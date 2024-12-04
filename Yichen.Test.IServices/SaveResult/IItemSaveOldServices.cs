using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Test.Model.Result;
using Yichen.Test.Model.table;


namespace Yichen.Test.IServices
{
    public interface IItemSaveOldServices : IBaseServices<test_sampleInfo>
    {

        /// <summary>
        /// 常规项目结果保存
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> SaveTest(CommResultModel<TestInfoModel> info);
        /// <summary>
        /// 微生物结果保存 
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> SaveMicrobe(CommResultModel<MicrobeInfoModel> info);
        /// <summary>
        /// 新冠结果保存
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> SaveGeneXG(CommResultModel<GeneInfoModel> info);
        /// <summary>
        /// 公共样本项目结果保存
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> SaveGene(CommResultModel<GeneInfoModel> info);

    }
}
