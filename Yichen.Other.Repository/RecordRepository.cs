using Yichen.Comm.IRepository.UnitOfWork;

using Yichen.Comm.Repository;
using Yichen.Other.IRepository;
using Yichen.System.Model;

namespace Yichen.Other.Repository
{
    public class RecordRepository : BaseRepository<object>, IRecordRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public RecordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task SampleRecord(string barcode, string operatType, string record, string operater, bool clientShow = true, string reason = null)
        {
            comm_samplerecord samplerecord = new comm_samplerecord();
            samplerecord.barcode = barcode;
            samplerecord.operatType = operatType;
            samplerecord.record = record;
            samplerecord.operater = operater;
            samplerecord.clientShow = operater == "admin" ? false : clientShow;
            samplerecord.reason = reason;
            samplerecord.createTime = DateTime.Now;
            DbClient.Insertable(samplerecord).ExecuteCommandAsync();
        }
    }
}
