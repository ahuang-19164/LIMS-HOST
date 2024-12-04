using Nito.AsyncEx;
using SqlSugar;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.Net.Data;
using Yichen.Net.Table;
using Yichen.Other.IRepository;
using Yichen.Per.IRepository;
using Yichen.Per.Model;
using Yichen.Per.Model.table;
using Yichen.Net.Configuration;
using Yichen.Net.Model.Entities.Expression;
using Yichen.Net.Model.Entities;
using Yichen.Net.Auth.HttpContextUser;
using Qc.YilianyunSdk.Utils;

namespace Yichen.Per.IServices
{
    /// <summary>
    /// 录入信息服务
    /// </summary>
    public class EntryHandleServices : BaseServices<object>, IEntryHandleServices
    {
        /// <summary>
        /// 异步锁
        /// </summary>
        private readonly AsyncLock _mutex = new AsyncLock();
        public readonly IUnitOfWork _UnitOfWork;
        public readonly IHttpContextUser _httpContextUser;
        public readonly IRecordRepository _recordRepository;
        public readonly IPerSampleInfoRepository _perSampleInfoRepository;
        public readonly ISampleInfoOtherRepository _sampleInfoOtherRepository;
        public readonly ISampleImgRepository _sampleImgRepository;

        public readonly ICommRepository _commRepository;
        public EntryHandleServices(IUnitOfWork unitOfWork
            , IHttpContextUser httpContextUser
            , IRecordRepository recordRepository
            , ICommRepository commRepository
            , IPerSampleInfoRepository perSampleInfoRepository
            , ISampleInfoOtherRepository sampleInfoOtherRepository
            , ISampleImgRepository sampleImgRepository
            )
        {
            _UnitOfWork = unitOfWork;
            _httpContextUser = httpContextUser;
            _recordRepository = recordRepository;
            _commRepository = commRepository;
            _perSampleInfoRepository = perSampleInfoRepository;
            _sampleInfoOtherRepository = sampleInfoOtherRepository;
            _sampleImgRepository = sampleImgRepository;
        }

        /// <summary>
        /// 查询条码号是否存在
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> PerBarcodeExist(commInfoModel<string> barcodes)
        {
            WebApiCallBack jm = new WebApiCallBack() { code=0,status=true};
            commReInfo<commReBarcodeInfo> commReInfo = new commReInfo<commReBarcodeInfo>();
            List<commReBarcodeInfo> barcodeInfos = new List<commReBarcodeInfo>();
            foreach (string barocde in barcodes.infos)
            {
                commReBarcodeInfo barcodeInfo = new commReBarcodeInfo();
                barcodeInfo.code = 0;
                bool state = await _perSampleInfoRepository.BarcodeExist(barocde);
                if (state)
                {
                    jm.status = false;
                    barcodeInfo.code = 1;
                    barcodeInfo.barcode = barocde;
                    barcodeInfo.msg = "条码存在";
                }
                else
                {
                    barcodeInfo.code = 0;
                    barcodeInfo.barcode = barocde;
                    barcodeInfo.msg = "条码不存在";
                }
                barcodeInfos.Add(barcodeInfo);
            }
            commReInfo.infos = barcodeInfos;
            jm.data = commReInfo;
            return jm;
        }

        /// <summary>
        /// 获取录入信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> GetEntryInfo(EntrySelectModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack() { code=0,status=true};
            //var sss = _httpContextUser.ID;
            //var zzz = _httpContextUser.UserNo;
            //var qqq = _httpContextUser.Name;
            //var ggg = _httpContextUser.Role;
            //var xxx = _httpContextUser.WebRole;
            //Console.WriteLine("id:" + sss + "  UserNo:" + zzz + "  Name:" + qqq + "  rele:"+ggg+"  webRole:"+xxx);

            if(!string.IsNullOrEmpty(infos.sampleCode))
            {
                var infosDT = await _perSampleInfoRepository.QueryDTByClauseAsync(p=>p.barcode==infos.sampleCode);
                jm.data = DataTableHelper.DTToString(infosDT);
            }
            else
            {
                if (infos.operatType == "1")
                {
                    var infosDT = await _perSampleInfoRepository.GetEntryInfo(Convert.ToDateTime(infos.startTime), Convert.ToDateTime(infos.endTime), infos.UserName);
                    jm.data = DataTableHelper.DTToString(infosDT);
                }

                if (infos.operatType == "2")
                {
                    var infosDT = await _perSampleInfoRepository.GetEntryInfo(Convert.ToDateTime(infos.startTime), Convert.ToDateTime(infos.endTime), string.Empty);
                    jm.data = DataTableHelper.DTToString(infosDT);
                }
            }
            return jm;
        }


        /// <summary>
        /// 前处理双录
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
       public async Task<WebApiCallBack> EntryDouble(EntryInfoModel infos)
       {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 前处理录入
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        /// http://localhost:9610/api/PerHandle/EntryInfo/EntryInfo
        ///{"UserName":"admin","UserToken":null,"connType":0,"operatType":1,"editType":1,"sampleInfos":[{"connType":0,"sampleID":null,"barcode":"AAAAAAAAAAAA","hospital":"C0001","hospitalName":"测试客户1","fileString":"","pairsInfo":[{"keyName":"GEagentNo","columnName":"agentNames","valueString":"测试代理商1","caption":"代理商:"},{"keyName":"GEagentNo","columnName":"agentNO","valueString":"1","caption":"代理商:"},{"keyName":"TEpatientName","columnName":"patientName","valueString":"test","caption":"姓名:"},{"keyName":"GEsampleTypeNO","columnName":"sampleTypeNames","valueString":"口咽拭子","caption":"标本类型:"},{"keyName":"GEsampleTypeNO","columnName":"sampleTypeNO","valueString":"46","caption":"标本类型:"},{"keyName":"DEsampleTime","columnName":"sampleTime","valueString":"2022-11-21 16:02:23","caption":"采样时间:"},{"keyName":"DEreceiveTime","columnName":"receiveTime","valueString":"2022-11-21 16:02:23","caption":"接收时间:"},{"keyName":"CEurgent","columnName":"urgent","valueString":"False","caption":"是否加急:"},{"keyName":"GEsampleShapeNO","columnName":"sampleShapeNames","valueString":"合格","caption":"标本性状:"},{"keyName":"GEsampleShapeNO","columnName":"sampleShapeNO","valueString":"48","caption":"标本性状:"}],"pairsOhterInfo":[],"applyCodes":"17","applyNames":"核酸检测-人混检"}]}
        public async Task<WebApiCallBack> EntryInfo(EntryInfoModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {
                commReInfo<commReSampleInfo> commReInfo = new commReInfo<commReSampleInfo>();
                List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                if (infos.operatType == 1)
                {
                    if (infos.sampleInfos != null)
                    {
                        foreach (SampleInfoModel sampleinfo in infos.sampleInfos)
                        {
                            commReSampleInfo reSampleInfo = new commReSampleInfo();
                            if (sampleinfo.barcode != null && sampleinfo.hospital != null && sampleinfo.hospitalName != null && sampleinfo.applyCodes != "" && sampleinfo.applyNames != "")
                            {
                                //判断barcode是否存在
                                var PerInfo = await _perSampleInfoRepository.BarcodeExist(sampleinfo.barcode);
                                if (!PerInfo)
                                {
                                    string Ssql = "insert into WorkPer.SampleInfo";//样本信息处理
                                    string Isql = "insert into WorkPer.SampleImg";//图片处理
                                    string SOsql = "insert into WorkPer.SampleInfoOther";//样本其他信息处理
                                    bool receiveTimestate = false;//判断
                                    DateTime receiveTime = DateTime.MinValue;//接收时间
                                    DateTime sampleTime = DateTime.MinValue;//采样时间
                                    ///遍历样本信息
                                    if (sampleinfo.pairsInfo.Count > 0)
                                    {
                                        string Cname = string.Empty;
                                        string CValue = string.Empty;
                                        foreach (PairsInfoModel item in sampleinfo.pairsInfo)
                                        {
                                            if (item.columnName != null && item.valueString != null && item.valueString.ToString() != "null" && item.valueString.ToString() != "")
                                            {
                                                //if (item.keyName == "sampleTime")
                                                //{
                                                //    sampleTime = Convert.ToDateTime(item.valueString);
                                                //}
                                                if (item.columnName == "receiveTime")
                                                {
                                                    receiveTimestate = true;
                                                    receiveTime = Convert.ToDateTime(item.valueString);
                                                }

                                                Cname += item.columnName + ",";
                                                CValue += "N'" + item.valueString + "',";
                                            }

                                        }
                                        if (!receiveTimestate)
                                        {
                                            Cname += "receiveTime,";
                                            receiveTime = DateTime.Now;
                                            CValue += $"N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',";
                                        }

                                        Cname = $"(creater,createTime,state,dstate,perStateNO,barcode,hospitalNo,hospitalNames,applyItemCodes,applyItemNames,{Cname.Substring(0, Cname.Length - 1)})";
                                        CValue = $"('{infos.UserName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','0','0','1','{sampleinfo.barcode}','{sampleinfo.hospital}','{sampleinfo.hospitalName}','{sampleinfo.applyCodes}','{sampleinfo.applyNames}'," + CValue.Substring(0, CValue.Length - 1) + ");";
                                        Ssql += Cname + " values " + CValue;
                                    }
                                    else
                                    {
                                        string Cname = "(barcode,hospitalNo,hospitalNames,applyItemCodes,applyItemNames)";
                                        string CValue = $"('{sampleinfo.barcode}','{sampleinfo.hospital}','{sampleinfo.hospitalName}','{sampleinfo.applyCodes}','{sampleinfo.applyNames}');";
                                        Ssql += Cname + " values " + CValue;
                                    }

                                    if (sampleinfo.pairsOhterInfo.Count > 0)
                                    {
                                        string Cname = string.Empty;
                                        string CValue = string.Empty;
                                        foreach (PairsInfoModel item in sampleinfo.pairsOhterInfo)
                                        {
                                            Cname += item.columnName + ",";
                                            CValue += "N'" + item.valueString + "',";
                                        }
                                        Cname = "(barcode" + Cname.Substring(0, Cname.Length - 1) + ")";
                                        CValue = $"('{sampleinfo.barcode}'," + CValue.Substring(0, CValue.Length - 1) + ");";

                                        SOsql += Cname + " values " + CValue;
                                    }
                                    else
                                    {

                                        string Cname = "(barcode)";
                                        string CValue = $"('{sampleinfo.barcode}');";
                                        SOsql += Cname + " values " + CValue;

                                    }

                                    if (sampleinfo.fileString != "")
                                    {
                                        string Iname = "(barcode,filestring,createTime,state,dstate)";
                                        string IValue = $"('{sampleinfo.barcode}','{sampleinfo.fileString}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','1','0');";
                                        Isql += Iname + " values " + IValue;
                                    }
                                    else
                                    {
                                        Isql = "";
                                    }
                                    await _commRepository.sqlcommand(Ssql + Isql + SOsql);
                                    await _recordRepository.SampleRecord(sampleinfo.barcode, RecordEnumVars.Entry, "前处理信息录入。", infos.UserName, true);
                                    reSampleInfo.code = 0;
                                    reSampleInfo.barcode = sampleinfo.barcode;
                                    reSampleInfo.msg += "新增成功:";
                                }
                                else
                                {
                                    reSampleInfo.code = 1;
                                    reSampleInfo.barcode = sampleinfo.barcode;
                                    reSampleInfo.msg += "条码号已存在;";
                                }

                            }
                            else
                            {
                                reSampleInfo.code = 1;
                                reSampleInfo.barcode = sampleinfo.barcode;
                                reSampleInfo.msg += "提交数据不完整;";
                            }
                            reSampleInfos.Add(reSampleInfo);
                        }
                        commReInfo.infos = reSampleInfos;
                        jm.data = commReInfo;
                    }
                    else
                    {
                        jm.code = 1;
                        jm.msg += "未发现提交的样本信息";
                    }
                }
                else
                {
                    if (infos.sampleInfos != null)
                    {
                        try
                        {


                            //List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                            foreach (SampleInfoModel sampleinfo in infos.sampleInfos)
                            {
                                string sampleRecord = "";
                                commReSampleInfo reSampleInfo = new commReSampleInfo();

                                //using (HLDBContext hLIMSDBContext = new HLDBContext())
                                //{

                                //判断barcode是否存在
                                var PerInfo = await _perSampleInfoRepository.GetByBarcode(sampleinfo.barcode);
                                //var PerOtherInfo = hLIMSDBContext.SampleInfoOthers.FirstOrDefault(b => b.barcode == sampleinfo.barcode);

                                if (PerInfo != null)
                                {
                                    string SOsql = "";//其他信息
                                    string Ssql = ""; //样本信息
                                    string Isql = "";//图片信息
                                    string Asql = "";//项目信息
                                                     //string AsqlWx = "";
                                    DateTime receiveTime = DateTime.MinValue;//接收时间
                                    DateTime sampleTime = DateTime.MinValue;//采样时间

                                    if (sampleinfo.pairsInfo.Count > 0)
                                    {
                                        string setValue = "";
                                        //遍历改动的字段
                                        foreach (PairsInfoModel item in sampleinfo.pairsInfo)
                                        {
                                            //string asssss = item.keyName.Substring(0, 1).ToUpper() + item.keyName.Substring(1);



                                            string oldVlue = PerInfo.GetType().GetProperty(item.columnName).GetValue(PerInfo, null) != null ? PerInfo.GetType().GetProperty(item.columnName).GetValue(PerInfo, null).ToString() : "";



                                            if (item.columnName == "sampleTime")
                                            {
                                                sampleTime = Convert.ToDateTime(item.valueString);
                                            }
                                            if (item.columnName == "receiveTime")
                                            {
                                                //receiveTimestate = true;
                                                receiveTime = Convert.ToDateTime(item.valueString);
                                            }
                                            if (oldVlue != item.valueString.ToString())
                                            {

                                                setValue += item.columnName + "=N'" + item.valueString + "',";
                                                sampleRecord += $"[{item.caption}]由[{oldVlue}]更改为[{item.valueString}]。";
                                            }
                                        }
                                        if (setValue.Length > 0)
                                        {
                                            Ssql = $"update WorkPer.SampleInfo set editer='{infos.UserName}',editTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'," + setValue.Substring(0, setValue.Length - 1);
                                            Ssql += $" where  barcode='{sampleinfo.barcode}';";
                                        }

                                    }

                                    if (sampleTime <= receiveTime)
                                    {

                                        //判断项目是否有改动

                                        if (sampleinfo.applyCodes != "" && sampleinfo.applyCodes != PerInfo.GetType().GetProperty("applyItemCodes").GetValue(PerInfo, null).ToString())
                                        {
                                            string setValue = $"applyItemCodes='{sampleinfo.applyCodes}',applyItemNames='{sampleinfo.applyNames}'";
                                            Asql = "update WorkPer.SampleInfo set " + setValue;
                                            Asql += $" where  barcode='{sampleinfo.barcode}';";


                                            sampleRecord += $"申请项目：由[{PerInfo.GetType().GetProperty("applyItemCodes").GetValue(PerInfo, null).ToString()}][{PerInfo.GetType().GetProperty("applyItemNames").GetValue(PerInfo, null).ToString()}]更改为[{sampleinfo.applyCodes}][{sampleinfo.applyNames}]。";


                                        }
                                        string IValuea = $"update WorkPer.SampleImg set dstate=1,state=0  where  barcode='{sampleinfo.barcode}';";

                                        //判断原始单是否需要修改
                                        if (sampleinfo.fileString != "")
                                        {
                                            string Iname = "(barcode,filestring,createTime,state,dstate)";
                                            string IValue = $"('{sampleinfo.barcode}','{sampleinfo.fileString}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','1','0');";
                                            Isql = "insert into WorkPer.SampleImg "+ Iname + " values " + IValue;

                                        }
                                        else
                                        {
                                            Isql = "";
                                        }

                                        string sql = Ssql + Asql + SOsql + IValuea + Isql;
                                        await _commRepository.sqlcommand(sql);
                                        reSampleInfo.code = 0;
                                        reSampleInfo.barcode = sampleinfo.barcode;
                                        reSampleInfo.msg = "修改成功";

                                        if (sampleRecord.Length > 0)
                                            await _recordRepository.SampleRecord(sampleinfo.barcode, RecordEnumVars.EditInfo, sampleRecord, infos.UserName, false);

                                    }
                                    else
                                    {
                                        reSampleInfo.code = 1;
                                        reSampleInfo.barcode = sampleinfo.barcode;
                                        reSampleInfo.msg += "采样时间大于接收时间";
                                    }
                                }
                                else
                                {
                                    reSampleInfo.code = 1;
                                    reSampleInfo.barcode = sampleinfo.barcode;
                                    reSampleInfo.msg += "条码信息不存在";
                                }
                                //reSampleInfos.Add(reSampleInfo);
                                reSampleInfos.Add(reSampleInfo);
                            }
                            commReInfo.infos = reSampleInfos;
                            jm.data = commReInfo;
                        }
                        catch (Exception ex)
                        {
                            jm.code = 1;
                            jm.msg += ex.Message; ;
                        }
                    }
                    else
                    {
                        jm.code = 1;
                        jm.msg += "未发现提交的样本信息";
                    }

                }
            }
            return jm;
        }


        /// <summary>
        /// 前处理录入
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> EntryInfoNew(EntryInfoModel infos)
        {

            WebApiCallBack jm = new WebApiCallBack() { code=0,status=true};
            using (await _mutex.LockAsync())
            {
                commReInfo<commReSampleInfo> commReInfo = new commReInfo<commReSampleInfo>();
                List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                if (infos.operatType == 1)
                {
                    if (infos.sampleInfos == null)
                    {
                        jm.code = 1;
                        jm.msg += "未发现提交的样本信息";
                    }
                    else
                    {

                        foreach (SampleInfoModel sampleinfo in infos.sampleInfos)
                        {
                            commReSampleInfo reSampleInfo = new commReSampleInfo() { code=0, barcode = sampleinfo.barcode,msg = "录入成功"};
                            if (sampleinfo.barcode != null && sampleinfo.hospital != null && sampleinfo.hospitalName != null && sampleinfo.applyCodes != "" && sampleinfo.applyNames != "")
                            {
                                //判断barcode是否存在
                                var PerInfo = await _perSampleInfoRepository.BarcodeExist(sampleinfo.barcode);
                                if (PerInfo)
                                {
                                    reSampleInfo.code = 1;
                                    reSampleInfo.barcode = sampleinfo.barcode;
                                    reSampleInfo.msg += "条码号已存在;";
                                }
                                else
                                {
                                    bool receiveTimestate = false;//判断
                                    DateTime receiveTime = DateTime.MinValue;//接收时间
                                    DateTime sampleTime = DateTime.MinValue;//采样时间

                                    Dictionary<string, object> smapleinfoPairs = new Dictionary<string, object>();
                                    smapleinfoPairs.Add("creater", infos.UserName);
                                    smapleinfoPairs.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    smapleinfoPairs.Add("state", 0);
                                    smapleinfoPairs.Add("dstate", 0);
                                    smapleinfoPairs.Add("perStateNO", 1);
                                    smapleinfoPairs.Add("barcode", sampleinfo.barcode);
                                    smapleinfoPairs.Add("hospitalNo", sampleinfo.hospital);
                                    smapleinfoPairs.Add("hospitalNames", sampleinfo.hospitalName);
                                    smapleinfoPairs.Add("applyItemCodes", sampleinfo.applyCodes);
                                    smapleinfoPairs.Add("applyItemNames", sampleinfo.applyNames);
                                    ///遍历样本信息
                                    if (sampleinfo.pairsInfo != null && sampleinfo.pairsInfo.Count > 0)
                                    {
                                        foreach (PairsInfoModel item in sampleinfo.pairsInfo)
                                        {
                                            if (item.columnName != null && item.valueString != null && item.valueString.ToString() != "null" && item.valueString.ToString() != "")
                                            {

                                                if (item.columnName == "receiveTime")
                                                {
                                                    receiveTimestate = true;
                                                    receiveTime = Convert.ToDateTime(item.valueString);
                                                }
                                                smapleinfoPairs.Add(item.columnName, item.valueString);
                                            }

                                        }
                                        if (!receiveTimestate)
                                        {
                                            smapleinfoPairs.Add("receiveTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        }
                                    }

                                    int perid=await  _perSampleInfoRepository.EntryInfo(smapleinfoPairs);

                                    if (perid>0)
                                    {
                                        if (sampleinfo.pairsOhterInfo != null && sampleinfo.pairsOhterInfo.Count > 0)
                                        {
                                            Dictionary<string, object> otherPairs = new Dictionary<string, object>();
                                            otherPairs.Add("barcode", sampleinfo.barcode);
                                            otherPairs.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                            otherPairs.Add("state", 1);
                                            otherPairs.Add("perid", perid);

                                            foreach (PairsInfoModel item in sampleinfo.pairsOhterInfo)
                                            {
                                                otherPairs.Add(item.columnName, item.valueString);
                                            }
                                            await _sampleInfoOtherRepository.EntryOtherInfo(otherPairs);
                                        }


                                        if (sampleinfo.fileString != "")
                                        {
                                            Dictionary<string, object> imgPairs = new Dictionary<string, object>();
                                            imgPairs.Add("perid", perid);
                                            imgPairs.Add("barcode", sampleinfo.barcode);
                                            imgPairs.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                            imgPairs.Add("filestring", sampleinfo.fileString);
                                            imgPairs.Add("dstate", 0);
                                            imgPairs.Add("state", 1);
                                            await _sampleImgRepository.EntryImgInfo(imgPairs);
                                        }
                                        await _recordRepository.SampleRecord(sampleinfo.barcode, RecordEnumVars.Entry, "前处理信息录入。", infos.UserName, true);

                                    }
                                    else
                                    {
                                        reSampleInfo.code = 1;
                                        reSampleInfo.barcode = sampleinfo.barcode;
                                        reSampleInfo.msg += "新增失败;";
                                    }
                                }
                            }
                            else
                            {
                                reSampleInfo.code = 1;
                                reSampleInfo.barcode = sampleinfo.barcode;
                                reSampleInfo.msg += "提交数据不完整;";
                            }
                            reSampleInfos.Add(reSampleInfo);
                        }
                        commReInfo.infos = reSampleInfos;
                        jm.data = commReInfo;

                    }
                }
                else
                {
                    if (infos.sampleInfos == null)
                    {
                        jm.code = 1;
                        jm.msg += "未发现提交的样本信息";
                    }
                    else
                    {
                        try
                        {
                            foreach (SampleInfoModel sampleinfo in infos.sampleInfos)
                            {
                                string sampleRecord = "";
                                commReSampleInfo reSampleInfo = new commReSampleInfo();
                                //判断barcode是否存在
                                var PerInfo = await _perSampleInfoRepository.GetByBarcode(sampleinfo.barcode);
                                if (PerInfo == null)
                                {
                                    reSampleInfo.code = 1;
                                    reSampleInfo.barcode = sampleinfo.barcode;
                                    reSampleInfo.msg += "条码信息不存在";
                                }
                                else
                                {

                                    DateTime receiveTime = DateTime.MinValue;//接收时间
                                    DateTime sampleTime = DateTime.MinValue;//采样时间
                                    Dictionary<string, object> smapleinfoPairs = new Dictionary<string, object>();
                                    smapleinfoPairs.Add("creater", infos.UserName);
                                    smapleinfoPairs.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //smapleinfoPairs.Add("state", 0);
                                    //smapleinfoPairs.Add("dstate", 0);
                                    //smapleinfoPairs.Add("perStateNO", 1);
                                    if(PerInfo.perStateNO==1&& sampleinfo.barcode != PerInfo.barcode)
                                    {
                                        var PerInfoState = await _perSampleInfoRepository.BarcodeExist(sampleinfo.barcode);
                                        if(!PerInfoState)
                                        {
                                            smapleinfoPairs.Add("barcode", sampleinfo.barcode);
                                            smapleinfoPairs.Add("hospitalNo", sampleinfo.hospital);
                                            smapleinfoPairs.Add("hospitalNames", sampleinfo.hospitalName);
                                        }
                                        else
                                        {
                                            reSampleInfo.code = 1;
                                            reSampleInfo.barcode = sampleinfo.barcode;
                                            reSampleInfo.msg += "条码号已存在;";
                                            continue;
                                        }

                                    }


                                    if (sampleinfo.pairsInfo!=null&& sampleinfo.pairsInfo.Count > 0)
                                    {
                                        string setValue = "";
                                        //遍历改动的字段
                                        foreach (PairsInfoModel item in sampleinfo.pairsInfo)
                                        {

                                            string oldVlue = PerInfo.GetType().GetProperty(item.columnName).GetValue(PerInfo, null) != null ? PerInfo.GetType().GetProperty(item.columnName).GetValue(PerInfo, null).ToString() : "";

                                            if (item.columnName == "sampleTime")
                                            {
                                                sampleTime = Convert.ToDateTime(item.valueString);
                                            }
                                            if (item.columnName == "receiveTime")
                                            {
                                                //receiveTimestate = true;
                                                receiveTime = Convert.ToDateTime(item.valueString);
                                            }
                                            if (oldVlue != item.valueString.ToString())
                                            {
                                                smapleinfoPairs.Add(item.columnName, item.valueString);
                                                sampleRecord += $"[{item.caption}]由[{oldVlue}]更改为[{item.valueString}]。";
                                            }
                                        }
                                    }

                                    if (sampleTime > receiveTime)
                                    {
                                        reSampleInfo.code = 1;
                                        reSampleInfo.barcode = sampleinfo.barcode;
                                        reSampleInfo.msg += "采样时间大于接收时间";
                                    }
                                    else
                                    {

                                        //判断项目是否有改动

                                        if (sampleinfo.applyCodes != "" && sampleinfo.applyCodes != PerInfo.GetType().GetProperty("applyItemCodes").GetValue(PerInfo, null).ToString())
                                        {
                                            smapleinfoPairs.Add("applyItemCodes", sampleinfo.applyCodes);
                                            smapleinfoPairs.Add("applyItemNames", sampleinfo.applyNames);
                                            sampleRecord += $";\r\n申请项目：由[{PerInfo.GetType().GetProperty("applyItemCodes").GetValue(PerInfo, null).ToString()}][{PerInfo.GetType().GetProperty("applyItemNames").GetValue(PerInfo, null).ToString()}]更改为[{sampleinfo.applyCodes}][{sampleinfo.applyNames}]。";
                                        }
                                        var a = await _perSampleInfoRepository.EntryInfoEdit(PerInfo.id, smapleinfoPairs);
                                        if(a>0)
                                        {
                                            if (sampleinfo.pairsOhterInfo != null&& sampleinfo.pairsOhterInfo.Count>0)
                                            {
                                                Dictionary<string, object> otherPairs = new Dictionary<string, object>();
                                                otherPairs.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                                foreach (PairsInfoModel item in sampleinfo.pairsOhterInfo)
                                                {
                                                    otherPairs.Add(item.columnName, item.valueString);
                                                }
                                                await _sampleInfoOtherRepository.EntryOteherInfoEdit(PerInfo.id, otherPairs);
                                            }

                                            //判断原始单是否需要修改
                                            if (sampleinfo.fileString != "")
                                            {
                                                Dictionary<string, object> imgPairs = new Dictionary<string, object>();

                                                imgPairs.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                                imgPairs.Add("filestring", sampleinfo.fileString);
                                                await _sampleImgRepository.EntryImgInfoEdit(PerInfo.id, imgPairs);
                                                sampleRecord += ";\r\b原始单修改";
                                            }
                                            reSampleInfo.code = 0;
                                            reSampleInfo.barcode = sampleinfo.barcode;
                                            reSampleInfo.msg = "修改成功";
                                        }
                                        else
                                        {
                                            reSampleInfo.code = 1;
                                            reSampleInfo.barcode = sampleinfo.barcode;
                                            reSampleInfo.msg = "修改失败";
                                        }

                                        //string sql = Ssql + Asql + SOsql + IValuea + Isql;
                                        //await _commRepository.sqlcommand(sql);


                                        if (sampleRecord.Length > 0)
                                            await _recordRepository.SampleRecord(sampleinfo.barcode, RecordEnumVars.EditInfo, sampleRecord, infos.UserName, false);
                                    }

                                }
                                reSampleInfos.Add(reSampleInfo);
                            }
                            commReInfo.infos = reSampleInfos;
                            jm.data = commReInfo;
                        }
                        catch (Exception ex)
                        {
                            jm.code = 1;
                            jm.msg += ex.Message; ;
                        }
                    }
                }
            }
            return jm;
        }
        /// <summary>
        /// 删除录入信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> EntryDelete(DeleteInfoModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            if (infos.sampleinfos == null)
            {
                jm.code = 1;
                jm.msg = $"未提交样本信息";
                return jm;
            }
            else
            {

                commReInfo<commReSampleInfo> commReInfo = new commReInfo<commReSampleInfo>();
                List<commReSampleInfo> reSampleInfos = new List<commReSampleInfo>();
                //List<string> barcodes = new List<string>();

                foreach (InfoListModel sampleinfo in infos.sampleinfos)
                {
                    if (sampleinfo.barcode != null)
                    {
                        commReSampleInfo reSampleInfo = new commReSampleInfo();
                        var sampleInfo = await _perSampleInfoRepository.GetByBarcode(sampleinfo.barcode);
                        if (sampleinfo == null)
                        {
                            reSampleInfo.code = 1;
                            reSampleInfo.barcode = sampleinfo.barcode;
                            reSampleInfo.msg = "未找到样本信息";
                        }
                        else
                        {
                            if(sampleInfo.perStateNO!=1)
                            {
                                reSampleInfo.code = 1;
                                reSampleInfo.barcode = sampleinfo.barcode;
                                reSampleInfo.msg = sampleInfo.perStateNO == 2 ? "样本已审核" : "样本正在检验";
                            }
                            else
                            {
                                var deletestate = await _perSampleInfoRepository.BarcodeDelete(sampleInfo);
                                if (deletestate)
                                {
                                    await _recordRepository.SampleRecord(sampleinfo.barcode, RecordEnumVars.DeleteInfo, "样本信息删除成功。", infos.UserName, false);
                                    reSampleInfo.code = 0;
                                    reSampleInfo.barcode = sampleinfo.barcode;
                                    reSampleInfo.msg = "删除成功";
                                }
                                else
                                {
                                    reSampleInfo.code = 1;
                                    reSampleInfo.barcode = sampleinfo.barcode;
                                    reSampleInfo.msg = "删除失败";
                                }
                            }
                        }
                        reSampleInfos.Add(reSampleInfo);
                    }
                }
                commReInfo.infos = reSampleInfos;
                jm.data = commReInfo;

            }
            return jm;
        }



        /// <summary>
        /// 疾控信息录入
        /// </summary>
        /// <param name="infos"></param>
        /// <returns>ok</returns>
        public async Task<WebApiCallBack> EntryInfoJK(JKEntryModel infos)
        {
            var jm = new WebApiCallBack();
            List<commReInfo<string>> commReInfos = new List<commReInfo<string>>();


            jm.code = 0;
            try
            {
                if (infos.sampleinfos == null)
                {
                    jm.code = 1;
                    jm.msg = "新增失败:提交信息不能为空;";
                }
                else
                {
                    foreach (JKSampleInfoModel sampleInfo in infos.sampleinfos)
                    {
                        commReInfo<string> commReInfo = new commReInfo<string>();
                        if (sampleInfo.barcode != null && sampleInfo.clientCode != null && sampleInfo.clientName != null && sampleInfo.applyCode != null && sampleInfo.applyName != null)
                        {
                            //判断barcode是否存在
                            var PerInfo = await _perSampleInfoRepository.BarcodeExist(sampleInfo.barcode);

                            if (PerInfo)
                            {
                                commReInfo.code = 1;
                                commReInfo.msg = "新增失败:" + sampleInfo.barcode + "条码号已存在;";
                            }
                            else
                            {

                                string framNo = sampleInfo.frameNo != null ? sampleInfo.frameNo : "";

                                Dictionary<string,object> pairs= new Dictionary<string,object>();
                                pairs.Add("frameNo", framNo);
                                pairs.Add("sampleShapeNO", 48);
                                pairs.Add("sampleShapeNames", "合格");
                                pairs.Add("connstate", 1);
                                pairs.Add("urgent", 0);
                                pairs.Add("creater", infos.UserName);
                                pairs.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                pairs.Add("state", 0);
                                pairs.Add("dstate", 0);
                                pairs.Add("perStateNO", 1);
                                pairs.Add("barcode", sampleInfo.barcode);
                                pairs.Add("hospitalBarcode", sampleInfo.barcode);
                                pairs.Add("hospitalNo", sampleInfo.clientCode);
                                pairs.Add("hospitalNames", sampleInfo.clientName);
                                pairs.Add("applyItemCodes", sampleInfo.applyCode);
                                pairs.Add("applyItemNames", sampleInfo.applyName);
                                pairs.Add("patientName", sampleInfo.patientName);
                                pairs.Add("patientSexNO", sampleInfo.patientSexNO);
                                pairs.Add("patientSexNames", sampleInfo.patientSexNames);
                                pairs.Add("ageYear", sampleInfo.ageYear);
                                pairs.Add("patientPhone", sampleInfo.patientPhone);
                                pairs.Add("patientCardNo", sampleInfo.patientCardNo);
                                pairs.Add("passportNo", sampleInfo.passportNo);
                                pairs.Add("patientAddress", sampleInfo.patientAddress);
                                pairs.Add("sampleLocation", sampleInfo.sampleLocation);
                                pairs.Add("sampleTypeNO", sampleInfo.sampleTypeNO);
                                pairs.Add("sampleTypeNames", sampleInfo.sampleTypeNames);
                                pairs.Add("number", sampleInfo.number);
                                pairs.Add("sampleTime", sampleInfo.sampleTime);
                                pairs.Add("receiveTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                                var a=await _perSampleInfoRepository.EntryInfo(pairs);

                                if (a > 0)
                                {
                                    commReInfo.code = 0;
                                    commReInfo.msg = "新增成功:" + sampleInfo.barcode;
                                    await _recordRepository.SampleRecord(sampleInfo.barcode, RecordEnumVars.Entry, "疾控信息接收。", infos.UserName, true);
                                }
                                else
                                {
                                    commReInfo.code = 1;
                                    commReInfo.msg = "新增失败:" + sampleInfo.barcode;
                                }

                            }

                        }
                        else
                        {
                            commReInfo.code = 1;
                            commReInfo.msg = sampleInfo.barcode + "条码信息不能为空;";
                        }
                        commReInfos.Add(commReInfo);
                    }
                    jm.data=commReInfos;

                }

            }
            catch (Exception ex)
            {
                jm.code = 1;
                jm.msg = ex.Message;
            }
            return jm;
        }




    }
}
