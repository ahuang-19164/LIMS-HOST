using Yichen.Comm.Model;

using Yichen.Finance.Model.table;
using Yichen.Flow.Model;
using Yichen.Net.Caching.Manuals;
using Yichen.Net.Configuration;
using Yichen.Net.Data;
using Yichen.System.Model;

namespace Yichen.Net.DLLs
{
    public static class StartupHelper
    {
        public static void loadbaseinfo()
        {
            //ManualDataCache<comm_item_flow>.MemoryCache.RemoveCacheAll();
            if (CommInfo.clientgroup.disname == "")
            {
                var clientgroup = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_client_group>().Where(p => p.state == true && p.dstate == false).ToList();
                string clientgroupkey = "clientgroup-" + Guid.NewGuid().ToString("N");
                ManualDataCache<comm_client_group>.MemoryCache.Set(clientgroupkey, clientgroup);
                CachInfos<comm_client_group> cachInfo1 = new CachInfos<comm_client_group>();
                cachInfo1.key = "clientgroup";
                cachInfo1.tablename = "comm_client_group";
                cachInfo1.disname = clientgroupkey;
                cachInfo1.data = clientgroup;
                CommInfo.clientgroup = cachInfo1;
            }
            else
            {
                ManualDataCache<comm_client_group>.MemoryCache.LIMSGetKeyValue(CommInfo.clientgroup);
            }


            if (CommInfo.clientinfo.disname == "")
            {
                var clientinfo = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_client_info>().Where(p => p.state == true && p.dstate == false).ToList();
                string clientinfokey = "clientinfo-" + Guid.NewGuid().ToString("N");
                ManualDataCache<comm_item_flow>.MemoryCache.Set(clientinfokey, clientinfo);
                CachInfos<comm_client_info> cachInfo2 = new CachInfos<comm_client_info>();
                cachInfo2.key = "clientinfo";
                cachInfo2.tablename = "comm_client_info";
                cachInfo2.disname = clientinfokey;
                cachInfo2.data = clientinfo;
                CommInfo.clientinfo = cachInfo2;
            }
            else
            {
                ManualDataCache<comm_client_info>.MemoryCache.LIMSGetKeyValue(CommInfo.clientinfo);
            }

            if (CommInfo.grouptest.disname == "")
            {
                var group_test = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_group_test>().Where(p => p.dstate == false).ToList();
                string grouptestkey = "grouptest-" + Guid.NewGuid().ToString("N");
                ManualDataCache<comm_group_test>.MemoryCache.Set(grouptestkey, group_test);
                CachInfos<comm_group_test> cachInfo3 = new CachInfos<comm_group_test>();
                cachInfo3.key = "grouptest";
                cachInfo3.tablename = "comm_group_test";
                cachInfo3.disname = grouptestkey;
                cachInfo3.data = group_test;
                CommInfo.grouptest = cachInfo3;
            }
            else
            {
                ManualDataCache<comm_group_test>.MemoryCache.LIMSGetKeyValue(CommInfo.grouptest);
            }

            if (CommInfo.itemflow.disname == "")
            {
                var item_flow = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_item_flow>().Where(p => p.state == true && p.dstate == false).ToList();
                string itemflowkey = "itemflow -" + Guid.NewGuid().ToString("N");
                ManualDataCache<comm_item_flow>.MemoryCache.Set(itemflowkey, item_flow);
                CachInfos<comm_item_flow> cachInfo4 = new CachInfos<comm_item_flow>();
                cachInfo4.key = "itemflow";
                cachInfo4.tablename = "comm_item_flow";
                cachInfo4.disname = itemflowkey;
                cachInfo4.data = item_flow;
                CommInfo.itemflow = cachInfo4;
            }
            else
            {
                ManualDataCache<comm_item_flow>.MemoryCache.LIMSGetKeyValue(CommInfo.itemflow);
            }

            if (CommInfo.itemapply.disname == "")
            {
                var item_apply = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_item_apply>().Where(p => p.state == true && p.dstate == false).ToList();
                string itemapplykey = "itemapply -" + Guid.NewGuid().ToString("N");
                ManualDataCache<comm_item_apply>.MemoryCache.Set(itemapplykey, item_apply);
                CachInfos<comm_item_apply> cachInfo5 = new CachInfos<comm_item_apply>();
                cachInfo5.key = "itemapply";
                cachInfo5.tablename = "comm_item_apply";
                cachInfo5.disname = itemapplykey;
                cachInfo5.data = item_apply;
                CommInfo.itemapply = cachInfo5;
            }
            else
            {
                ManualDataCache<comm_item_apply>.MemoryCache.LIMSGetKeyValue(CommInfo.itemapply);
            }

            if (CommInfo.itemgroup.disname == "")
            {

                var item_group = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_item_group>().Where(p => p.state == true && p.dstate == false).ToList();
                string itemgroupkey = "itemgroup -" + Guid.NewGuid().ToString("N");
                ManualDataCache<comm_item_group>.MemoryCache.Set(itemgroupkey, item_group);
                CachInfos<comm_item_group> cachInfo6 = new CachInfos<comm_item_group>();
                cachInfo6.key = "itemgroup";
                cachInfo6.tablename = "comm_item_group";
                cachInfo6.disname = itemgroupkey;
                cachInfo6.data = item_group;
                CommInfo.itemgroup = cachInfo6;
            }
            else
            {
                ManualDataCache<comm_item_group>.MemoryCache.LIMSGetKeyValue(CommInfo.itemgroup);
            }

            if (CommInfo.itemtest.disname == "")
            {
                var item_test = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_item_test>().Where(p => p.state == true && p.dstate == false).ToList();
                string itemtest = "itemtest -" + Guid.NewGuid().ToString("N");
                ManualDataCache<comm_item_test>.MemoryCache.Set(itemtest, item_test);
                CachInfos<comm_item_test> cachInfo7 = new CachInfos<comm_item_test>();
                cachInfo7.key = "itemtest";
                cachInfo7.tablename = "comm_item_test";
                cachInfo7.disname = itemtest;
                cachInfo7.data = item_test;
                CommInfo.itemtest = cachInfo7;
            }
            else
            {
                ManualDataCache<comm_item_test>.MemoryCache.LIMSGetKeyValue(CommInfo.itemtest);
            }

            if (CommInfo.itemreference.disname == "")
            {
                var item_reference = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_item_reference>().Where(p => p.state == true && p.dstate == false).ToList();
                string item_referencekey = "itemreference -" + Guid.NewGuid().ToString("N");
                ManualDataCache<comm_item_reference>.MemoryCache.Set(item_referencekey, item_reference);
                CachInfos<comm_item_reference> cachInfo8 = new CachInfos<comm_item_reference>();
                cachInfo8.key = "itemreference";
                cachInfo8.tablename = "comm_item_reference";
                cachInfo8.disname = item_referencekey;
                cachInfo8.data = item_reference;
                CommInfo.itemreference = cachInfo8;
            }
            else
            {
                ManualDataCache<comm_item_reference>.MemoryCache.LIMSGetKeyValue(CommInfo.itemreference);
            }

            if (CommInfo.financeGroup.disname == "")
            {
                var item_reference = SqlSugarHelper.SqlSugarClientCreate().Queryable<finance_group_charge>().Where(p => p.state == true).ToList();
                string finance_group_charge = "financeGroup -" + Guid.NewGuid().ToString("N");
                ManualDataCache<finance_group_charge>.MemoryCache.Set(finance_group_charge, item_reference);
                CachInfos<finance_group_charge> cachInfo9 = new CachInfos<finance_group_charge>();
                cachInfo9.key = "financeGroup";
                cachInfo9.tablename = "finance_group_charge";
                cachInfo9.disname = finance_group_charge;
                cachInfo9.data = item_reference;
                CommInfo.financeGroup = cachInfo9;
            }
            else
            {
                ManualDataCache<finance_group_charge>.MemoryCache.LIMSGetKeyValue(CommInfo.financeGroup);
            }


            #region 原始方法

            //string sql1 = $"select * from WorkComm.ItemApply where state=1 and dstate=0;";
            //DataTable dataTable1 = HLDBSqlHelper.ExecuteDataset(sql1).Tables[0];
            //redisData redisData1 = new redisData();
            //redisData1.key = "WorkComm.ItemApply-" + Guid.NewGuid().ToString("N");
            //redisData1.name = "WorkComm.ItemApply";
            //redisData1.DT = dataTable1;
            //CommonData.ItemApply = redisData1;
            //bool sss = redisCache.stringSet(CommonData.ItemApply.key, ConvertDatatable.DataTableToString(dataTable1));

            //string sql2 = $"select * from WorkComm.ItemGroup where state=1 and  dstate=0;";
            //DataTable dataTable2 = HLDBSqlHelper.ExecuteDataset(sql2).Tables[0];
            //redisData redisData2 = new redisData();
            //redisData2.key = "WorkComm.ItemGroup-" + Guid.NewGuid().ToString("N");
            //redisData2.name = "WorkComm.ItemGroup";
            //redisData2.DT = dataTable2;
            //CommonData.ItemGroup = redisData2;
            //redisCache.stringSet(CommonData.ItemGroup.key, ConvertDatatable.DataTableToString(dataTable2));

            //string sql3 = $"select * from WorkComm.ItemTest where state=1 and  dstate=0;";
            //DataTable dataTable3 = HLDBSqlHelper.ExecuteDataset(sql3).Tables[0];
            //redisData redisData3 = new redisData();
            //redisData3.key = "WorkComm.ItemTest-" + Guid.NewGuid().ToString("N");
            //redisData3.name = "WorkComm.ItemTest";
            //redisData3.DT = dataTable3;
            //CommonData.ItemTest = redisData3;
            //redisCache.stringSet(CommonData.ItemTest.key, ConvertDatatable.DataTableToString(dataTable3));

            //string sql4 = $"select * from WorkComm.TestReference where state=1 and dstate=0;";
            //DataTable dataTable4 = HLDBSqlHelper.ExecuteDataset(sql4).Tables[0];
            //redisData redisData4 = new redisData();
            //redisData4.key = "WorkComm.TestReference-" + Guid.NewGuid().ToString("N");
            //redisData4.name = "WorkComm.TestReference";
            //redisData4.DT = dataTable4;
            //CommonData.TestReference = redisData4;
            //redisCache.stringSet(CommonData.TestReference.key, ConvertDatatable.DataTableToString(dataTable4));

            //string sql5 = $"select * from WorkComm.ItemFlow where state=1 and  dstate=0;";
            //DataTable dataTable5 = HLDBSqlHelper.ExecuteDataset(sql5).Tables[0];
            //redisData redisData5 = new redisData();
            //redisData5.key = "WorkComm.ItemFlow-" + Guid.NewGuid().ToString("N");
            //redisData5.name = "WorkComm.ItemFlow";
            //redisData5.DT = dataTable5;
            //CommonData.ItemFlow = redisData5;
            //redisCache.stringSet(CommonData.ItemFlow.key, ConvertDatatable.DataTableToString(dataTable5));

            //string sql6 = $"select no,chargeLevelNO,discount,personNO from WorkComm.ClientInfo where dstate=0;";
            //DataTable dataTable6 = HLDBSqlHelper.ExecuteDataset(sql6).Tables[0];
            //redisData redisData6 = new redisData();
            //redisData6.key = "WorkComm.ClientInfo-" + Guid.NewGuid().ToString("N");
            //redisData6.name = "WorkComm.ClientInfo";
            //redisData6.DT = dataTable6;
            //CommonData.ClientInfo = redisData6;
            //redisCache.stringSet(CommonData.ClientInfo.key, ConvertDatatable.DataTableToString(dataTable6));

            ////缓存中插入
            //string sql7 = $"select clientid,clientNO,groupNO,chargeLevelNO,discount,state from WorkComm.ClientGroup where dstate=0;";
            //DataTable dataTable7 = HLDBSqlHelper.ExecuteDataset(sql7).Tables[0];
            //redisData redisData7 = new redisData();
            //redisData7.key = "WorkComm.ClientGroup-" + Guid.NewGuid().ToString("N");
            //redisData7.name = "WorkComm.ClientGroup";
            //redisData7.DT = dataTable7;
            //CommonData.ClientGroup = redisData7;
            //redisCache.stringSet(CommonData.ClientInfo.key, ConvertDatatable.DataTableToString(dataTable7));


            //string sql8 = $"select no,trequal,tcequal,rcequal,reviewState FROM WorkComm.GroupTest";
            //DataTable dataTable8 = HLDBSqlHelper.ExecuteDataset(sql8).Tables[0];
            //redisData redisData8 = new redisData();
            //redisData8.key = "WorkComm.GroupTest-" + Guid.NewGuid().ToString("N");
            //redisData8.name = "WorkComm.GroupTest";
            //redisData8.DT = dataTable8;
            //CommonData.GroupTest = redisData8;
            //redisCache.stringSet(CommonData.GroupTest.key, ConvertDatatable.DataTableToString(dataTable8));

            #endregion

        }

        /// <summary>
        /// 如果有更信息数据，刷新缓存数据
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>

        public static async Task refreshbaseinfo(string keyName)
        {
            await Task.Run(() =>
            {
                keyName = keyName.ToLower();
                if (keyName.Contains("itemapply"))
                    UpItemApply();
                if (keyName.Contains("itemgroup"))
                    UpItemGroup();
                if (keyName.Contains("itemtest"))
                    UpItemTest();
                if (keyName.Contains("itemreference"))
                    UpTestReference();
                if (keyName.Contains("itemflow"))
                    UpItemFlow();
                if (keyName.Contains("clientinfo"))
                    UpClientInfo();
                if (keyName.Contains("clientgroup"))
                    UpClientGroup();
                if (keyName.Contains("grouptest"))
                    UpGroupTest();
            });



            //switch (keyName)
            //{
            //    case keyName.Contains("itemapply"):
            //        UpItemApply();
            //        break;
            //    case "itemgroup":
            //        UpItemGroup();
            //        break;
            //    case "itemtest":
            //        UpItemTest();
            //        break;
            //    case "itemreference":
            //        UpTestReference();
            //        break;
            //    case "itemflow":
            //        UpItemFlow();
            //        break;
            //    case "clientinfo":
            //        UpClientInfo();
            //        break;
            //    case "clientgroup":
            //        UpClientGroup();
            //        break;
            //    case "grouptest":
            //        UpGroupTest();
            //        break;
            //    default:
            //        break;
            //}



        }
        #region 更新缓存中的基础数据

        public static void UpItemApply()
        {
            var item_apply = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_item_apply>().Where(p => p.state == true && p.dstate == false).ToList();
            string itemapplykey = "itemapply -" + Guid.NewGuid().ToString("N");
            ManualDataCache<comm_item_apply>.MemoryCache.Set(itemapplykey, item_apply);
            CachInfos<comm_item_apply> cachInfo5 = new CachInfos<comm_item_apply>();
            cachInfo5.key = "itemapply";
            cachInfo5.tablename = "comm_item_apply";
            cachInfo5.disname = itemapplykey;
            cachInfo5.data = item_apply;
            CommInfo.itemapply = cachInfo5;
        }
        public static void UpItemGroup()
        {
            var item_group = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_item_group>().Where(p => p.state == true && p.dstate == false).ToList();
            string itemgroupkey = "itemgroup -" + Guid.NewGuid().ToString("N");
            ManualDataCache<comm_item_group>.MemoryCache.Set(itemgroupkey, item_group);
            CachInfos<comm_item_group> cachInfo6 = new CachInfos<comm_item_group>();
            cachInfo6.key = "itemgroup";
            cachInfo6.tablename = "comm_item_group";
            cachInfo6.disname = itemgroupkey;
            cachInfo6.data = item_group;
            CommInfo.itemgroup = cachInfo6;
        }
        public static void UpItemTest()
        {
            var item_test = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_item_test>().Where(p => p.state == true && p.dstate == false).ToList();
            string itemtest = "itemtest -" + Guid.NewGuid().ToString("N");
            ManualDataCache<comm_item_test>.MemoryCache.Set(itemtest, item_test);
            CachInfos<comm_item_test> cachInfo7 = new CachInfos<comm_item_test>();
            cachInfo7.key = "itemtest";
            cachInfo7.tablename = "comm_item_test";
            cachInfo7.disname = itemtest;
            cachInfo7.data = item_test;
            CommInfo.itemtest = cachInfo7;
        }
        public static void UpTestReference()
        {
            var item_reference = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_item_reference>().Where(p => p.state == true && p.dstate == false).ToList();
            string item_referencekey = "itemreference -" + Guid.NewGuid().ToString("N");
            ManualDataCache<comm_item_reference>.MemoryCache.Set(item_referencekey, item_reference);
            CachInfos<comm_item_reference> cachInfo8 = new CachInfos<comm_item_reference>();
            cachInfo8.key = "itemreference";
            cachInfo8.tablename = "comm_item_reference";
            cachInfo8.disname = item_referencekey;
            cachInfo8.data = item_reference;
            CommInfo.itemreference = cachInfo8;
        }











        public static void UpItemFlow()
        {
            var item_flow = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_item_flow>().Where(p => p.state == true && p.dstate == false).ToList();
            string itemflowkey = "itemflow -" + Guid.NewGuid().ToString("N");
            ManualDataCache<comm_item_flow>.MemoryCache.Set(itemflowkey, item_flow);
            CachInfos<comm_item_flow> cachInfo4 = new CachInfos<comm_item_flow>();
            cachInfo4.key = "itemflow";
            cachInfo4.tablename = "comm_item_flow";
            cachInfo4.disname = itemflowkey;
            cachInfo4.data = item_flow;
            CommInfo.itemflow = cachInfo4;
        }

        public static void UpClientInfo()
        {
            var clientinfo = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_client_info>().Where(p => p.state == true && p.dstate == false).ToList();
            string clientinfokey = "clientinfo-" + Guid.NewGuid().ToString("N");
            ManualDataCache<comm_client_info>.MemoryCache.Set(clientinfokey, clientinfo);
            CachInfos<comm_client_info> cachInfo2 = new CachInfos<comm_client_info>();
            cachInfo2.key = "clientinfo";
            cachInfo2.tablename = "comm_client_info";
            cachInfo2.disname = clientinfokey;
            cachInfo2.data = clientinfo;
            CommInfo.clientinfo = cachInfo2;
        }
        public static void UpClientGroup()
        {
            var clientgroup = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_client_group>().Where(p => p.state == true && p.dstate == false).ToList();
            string clientgroupkey = "clientgroup-" + Guid.NewGuid().ToString("N");
            ManualDataCache<comm_client_group>.MemoryCache.Set(clientgroupkey, clientgroup);
            CachInfos<comm_client_group> cachInfo1 = new CachInfos<comm_client_group>();
            cachInfo1.key = "clientgroup";
            cachInfo1.tablename = "comm_client_group";
            cachInfo1.disname = clientgroupkey;
            cachInfo1.data = clientgroup;
            CommInfo.clientgroup = cachInfo1;
        }

        public static void UpGroupTest()
        {
            var group_test = SqlSugarHelper.SqlSugarClientCreate().Queryable<comm_group_test>().Where(p => p.dstate == false).ToList();
            string grouptestkey = "grouptest-" + Guid.NewGuid().ToString("N");
            ManualDataCache<comm_group_test>.MemoryCache.Set(grouptestkey, group_test);
            CachInfos<comm_group_test> cachInfo3 = new CachInfos<comm_group_test>();
            cachInfo3.key = "grouptest";
            cachInfo3.tablename = "comm_group_test";
            cachInfo3.disname = grouptestkey;
            cachInfo3.data = group_test;
            CommInfo.grouptest = cachInfo3;
        }

        #endregion
    }
}
