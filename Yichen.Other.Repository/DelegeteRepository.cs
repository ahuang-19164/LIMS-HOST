using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.Net.Data;
using Yichen.Other.IRepository;

namespace Yichen.Other.Repository
{
    public class DelegeteRepository : BaseRepository<object>, IDelegeteRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public DelegeteRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        ///// <summary>
        ///// 标本检验新增委托记录
        ///// </summary>
        ///// <returns></returns>
        //public async Task AddRecord()
        //{
        //    string itemCodes = "";
        //    string itemNames = "";
        //    for (int a = 0; a < GVTestInfo.RowCount; a++)
        //    {
        //        if (GVTestInfo.GetRowCellValue(a, "check") != DBNull.Value)
        //        {
        //            if (Convert.ToBoolean(GVTestInfo.GetRowCellValue(a, "check")))
        //            {
        //                string itemcode = GVTestInfo.GetRowCellValue(a, "no") != DBNull.Value ? GVTestInfo.GetRowCellValue(a, "no").ToString() : "";
        //                itemCodes += itemcode + ",";
        //                string itemName = GVTestInfo.GetRowCellValue(a, "names") != DBNull.Value ? GVTestInfo.GetRowCellValue(a, "names").ToString() : "";
        //                itemNames += itemName + ",";
        //            }
        //        }

        //    }


        //    if (itemCodes != "" && itemNames != "")
        //    {
        //        itemCodes = itemCodes.Substring(0, itemCodes.Length - 1);
        //        itemNames = itemNames.Substring(0, itemNames.Length - 1);
        //        uInfo uInfo = new uInfo();
        //        uInfo.TableName = "WorkTest.SampleInfo";
        //        uInfo.value = "delegateState=1";
        //        uInfo.DataValueID = sampleInfoID;
        //        uInfo.MessageShow = 1;
        //        int s = ApiHelpers.postInfo(uInfo);
        //        if (s > 0)
        //        {
        //            iInfo iInfo = new iInfo();
        //            iInfo.TableName = "WorkOther.DelegeteRecord";
        //            Dictionary<string, object> pairs = new Dictionary<string, object>();
        //            pairs.Add("dstate", 0);
        //            pairs.Add("state", 1);
        //            pairs.Add("testID", sampleInfoID);
        //            pairs.Add("creater", CommonData.UserInfo.names);
        //            pairs.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //            pairs.Add("barcode", TEbarcode.EditValue);
        //            pairs.Add("delegateStateNO", 1);
        //            pairs.Add("itemCodes", itemCodes);
        //            pairs.Add("itemNames", itemNames);
        //            pairs.Add("reason", TEreason.EditValue);
        //            iInfo.values = pairs;
        //            iInfo.MessageShow = 1;
        //            s = ApiHelpers.postInfo(iInfo);
        //            if (s > 0)
        //            {
        //                SaveState = "0";
        //                this.Close();
        //            }
        //        }


        //    }
        //    else
        //    {
        //        SaveState = "0";
        //        MessageBox.Show("未获取到需要委托的项目！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        /// <summary>
        /// 更新委托记录
        /// </summary>
        /// <returns></returns>
        public async Task<int> EditRecord()
        {

            //uInfo uInfo2 = new uInfo();
            //uInfo2.TableName = "WorkOther.DelegeteRecord";
            //Dictionary<string, object> pairsd = new Dictionary<string, object>();
            //pairsd.Add("checker", CommonData.UserInfo.names);
            //pairsd.Add("checkTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            ////uInfo2.value = "delegateStateNO='4'";
            //uInfo2.values = pairsd;
            //uInfo2.wheres = $"testid={testid}";
            string a = "";
            return await DbClient.Ado.ExecuteCommandAsync(a);
        }
    }
}
