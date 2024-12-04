using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yichen.Comm.IRepository;

namespace Yichen.Other.IRepository
{
    public interface IDelegeteRepository : IBaseRepository<object>
    {
        Task<int> EditRecord();
    }
}
