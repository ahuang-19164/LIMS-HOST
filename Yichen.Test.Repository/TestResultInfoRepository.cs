using System.ComponentModel.DataAnnotations;
using System.Data;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.Other.IRepository;
using Yichen.Test.IRepository;
using Yichen.Test.Model.table;

namespace Yichen.Test.Repository
{
    public class TestResultInfoRepository : BaseRepository<object>, ITestResultInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecordRepository _recordRepository;
        public TestResultInfoRepository(IUnitOfWork unitOfWork
            , IRecordRepository recordRepository
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _recordRepository = recordRepository;
        }
        /// <summary>
        /// 获取检验样本信息
        /// </summary>
        /// <param name="testid"></param>
        /// <returns></returns>
        public async Task<test_sampleInfo> GetTestSampleInfo(string barcodes)
        {
            var testinfos = await DbClient.Queryable<test_sampleInfo>().FirstAsync(p => p.barcode==barcodes&&p.dstate==false);
            return testinfos;
        }


        public async Task<test_sampleInfo> GetTestInfo(int testid)
        {
            //string oldTestInfo1 = $"select * from WorkTest.SampleInfo  where id='{testid}';";
            //DataTable sampleInfoDT =await _commRepository.GetTable(oldTestInfo1); ;//样本信息

            var testinfo = await DbClient.Queryable<test_sampleInfo>().FirstAsync(p => p.id == testid);
            return testinfo;
        }

        public async Task<DataTable> GetSampleResult(int testid, string itemCodes)
        {
            string oldTestInfo2 = $"select * from WorkTest.SampleResult  where testid='{testid}' and itemCodes in ({itemCodes.Substring(0, itemCodes.Length - 1)});";
            DataTable oldItemInfoDT = await DbClient.Ado.GetDataTableAsync(oldTestInfo2); ;
            return oldItemInfoDT;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="testid"></param>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        public async Task<DataTable> GetMicrobeInfo(int testid, string itemCodes)
        {
            string oldTestInfo2 = $"select * from WorkTest.ResultMicrobeInfo where testid='{testid}' and itemCodes in ({itemCodes.Substring(0, itemCodes.Length - 1)}) and state=1 and dstate=0;";
            DataTable oldItemInfoDT = await DbClient.Ado.GetDataTableAsync(oldTestInfo2); ;
            return oldItemInfoDT;
        }
        /// <summary>
        /// 获取微生物项目信息
        /// </summary>
        /// <param name="testid"></param>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        public async Task<DataTable> GetMicrobeItem(int testid, string itemCodes)
        {
            string oldTestInfo2 = $"select * from WorkTest.ResultMicrobeItem where testid='{testid}' and itemCodes in ({itemCodes.Substring(0, itemCodes.Length - 1)}) and state=1 and dstate=0;";
            DataTable oldItemInfoDT = await DbClient.Ado.GetDataTableAsync(oldTestInfo2); ;
            return oldItemInfoDT;
        }
        /// <summary>
        /// 获取基因项目结果
        /// </summary>
        /// <param name="testid"></param>
        /// <param name="itemCodes"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<DataTable> GetGene(int testid, string itemCodes, string tableName)
        {

            //var oldItem = DbClient.Queryable<dynamic>().AS(tableName).Where("testid=@testida and itemCodes in (@itemCodesa}) and state=1 and dstate=0", new { testida =testid, itemCodesa = itemCodes});
            string oldTestInfo2 = $"select * from {tableName} where testid='{testid}' and itemCodes in ({itemCodes.Substring(0, itemCodes.Length - 1)}) and state=1 and dstate=0;";
            DataTable oldItemInfoDT = await DbClient.Ado.GetDataTableAsync(oldTestInfo2); ;
            return oldItemInfoDT;
        }




        /// <summary>
        /// 插入检验中样本信息 返回插入id
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<long> AddTestInfo(test_sampleInfo infos)
        {
            long infoid = 0;
            await Task.Run(() =>
            {
                infoid = DbClient.Insertable(infos).ExecuteReturnBigIdentity();
            } );
            return infoid;
        }
    }
}
