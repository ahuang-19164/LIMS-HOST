using System.Threading.Tasks;
using Yichen.Jop.IServices;
using Yichen.Stores.IServices;

namespace Yichen.Net.Tasks
{
    /// <summary>
    ///更新存储标本记录状态信息
    /// </summary>
    public class StroesRecordHandleJOP
    {
        private readonly IStoresJobServices _storesJobServices;

        public StroesRecordHandleJOP(IStoresJobServices storesJobServices)
        {
            _storesJobServices = storesJobServices;
        }

        public async Task Execute()
        {
            await _storesJobServices.refreshRecord();
        }
    }
}
