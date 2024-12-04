using System.Threading.Tasks;
using Yichen.Jop.IServices;
using Yichen.Stores.IServices;

namespace Yichen.Net.Tasks
{
    /// <summary>
    ///更新标本架状态信息
    /// </summary>
    public class StoresShelfHandleJOP
    {
        private readonly IStoresJobServices _storesJobServices;

        public StoresShelfHandleJOP(IStoresJobServices storesJobServices)
        {
            _storesJobServices = storesJobServices;
        }

        public async Task Execute()
        {
            await _storesJobServices.refreshShelf();
        }
    }
}
