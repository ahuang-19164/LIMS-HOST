using Nito.AsyncEx;
using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.Files.Model;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Auth.Policys;
using Yichen.Net.Configuration;
using Yichen.Net.Data;
using Yichen.Report.IServices;
using Yichen.Report.Model;
using Yichen.System.IServices;

namespace Yichen.Report.Services
{
    public partial class ReportHandleServices : BaseServices<object>, IReportHandleServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IHttpContextUser _httpContextUser;
        private readonly PermissionRequirement _permissionRequirement;
        private readonly IUserLogServices _UserLogServices;
        private readonly ICommRepository _commRepository;

        public ReportHandleServices(IUnitOfWork unitOfWork
            , IHttpContextUser httpContextUser
            , PermissionRequirement permissionRequirement
            //,IHttpContextAccessor httpContextAccessor
            , IUserLogServices UserLogServices
            , ICommRepository commRepository
            ) 
        {
            _httpContextUser = httpContextUser;
            _permissionRequirement = permissionRequirement;
            //_httpContextAccessor = httpContextAccessor;
            _UserLogServices = UserLogServices;
            _commRepository = commRepository;
        }

        /// <summary>
        /// 上传报告接口
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ReportUpload(UpLoadReportModel info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            if (info.perid != 0 && info.testid != 0 && info.barcode != null && info.hospitalNo != null)
            {
                string FileName = info.hospitalNo + "-" + info.barcode + "-" + info.testid;
                if (info.upState == 0)
                {
                    if (info.FileString != null)
                    {
                        int a = 1;
                        string NewfileName = FileName + "-" + a + ".pdf";
                        string SavePath = DateTime.Now.ToString("yyyy-MM-dd") + "\\" + NewfileName;
                        string FilePath = AppSettingsConstVars.readReportFilePath + "\\" + SavePath;
                        string dirPath = AppSettingsConstVars.readReportFilePath + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                        if (!Directory.Exists(dirPath))
                            Directory.CreateDirectory(dirPath);
                        while (File.Exists(FilePath))//检查是否有同名文件
                        {
                            a++;
                            NewfileName = FileName + "-" + a + ".pdf";
                            SavePath = DateTime.Now.ToString("yyyy-MM-dd") + "\\" + NewfileName;
                            FilePath = AppSettingsConstVars.readReportFilePath + "\\" + SavePath;
                        }



                        if (StringToFile(AppSettingsConstVars.readReportFilePath, info.FileString, SavePath))
                        {
                            string sql = "";

                            sql = $"update WorkTest.SampleInfo set report=1 where id={info.testid};\r\n";
                            sql += $"update Report.SampleReportLog set dstate=1 where testid={info.testid};\r\n";
                            iInfo iInfo = new iInfo();
                            iInfo.TableName = "Report.SampleReportLog";
                            Dictionary<string, object> pairs = new Dictionary<string, object>();
                            pairs.Add("dstate", 0);
                            pairs.Add("iscreate", 1);
                            pairs.Add("state", 1);
                            pairs.Add("createTime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            //pairs.Add("printTime", DEprintTime.EditValue);
                            //pairs.Add("id", TEid.EditValue);
                            pairs.Add("perid", info.perid);
                            pairs.Add("printNum", 0);
                            pairs.Add("printState", 1);
                            pairs.Add("testid", info.testid);
                            //pairs.Add("patientName","");
                            //pairs.Add("printer", TEprinter.EditValue);
                            pairs.Add("barcode", info.barcode);
                            pairs.Add("clientNO", info.hospitalNo);
                            pairs.Add("fileName", NewfileName);
                            pairs.Add("filePath", "\\" + SavePath);

                            iInfo.values = pairs;
                            //pairs.Add("groupNO", GEgroupNO.EditValue);
                            string sql2= SqlFormartHelper.insertFormart(iInfo);
                            int aaa = await _commRepository.sqlcommand(sql+ sql2);
                            if (aaa > 0)
                            {
                                jm.code = 0;
                                jm.msg = "上传成功";
                            }
                            else
                            {
                                jm.code = 1;
                                jm.msg = "上传失败";
                            }
                        }
                        else
                        {
                            jm.code = 1;
                            jm.msg = "提交文件类型错误";
                        }
                    }
                    else
                    {
                        jm.code = 1;
                        jm.msg = "上传文件信息有误";
                    }
                }
                else
                {
                    string sql = "";
                    sql = $"update WorkTest.SampleInfo set reportState=0 where id={info.testid};\r\n";
                    sql += $"update Report.SampleReportLog set dstate=1 where testid={info.testid};\r\n";
                    int aaa = await _commRepository.sqlcommand(sql);
                    if (aaa > 0)
                    {
                        jm.code = 0;
                        jm.msg = "清空成功";
                    }
                    else
                    {
                        jm.code = 1;
                        jm.msg = "清空失败";
                    }
                }
            }
            else
            {
                jm.code = 1;
                jm.msg = "未提交样本信息";
            }

            return jm;
        }





        /// <summary>
        /// 下载报告接口
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ReportDown(GetReportModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            commReInfo<FileModel> commReInfo = new commReInfo<FileModel>();
            List<FileModel> fileModes = new List<FileModel>();
            if (infos.infoID.Count > 0)
            {
                string ids = "";
                foreach (int infoID in infos.infoID)
                {
                    ids += infoID + ",";
                }
                if (ids != "")
                {
                    Random a = new Random();
                    int nos = a.Next(0, 1000);
                    //fileNO = nos;
                    //Task<bool> task = Task<bool>(() =>
                    //{
                    ids = ids.Substring(0, ids.Length - 1);
                    //ids = "148324,148335,148346,148357";
                    string Sql = string.Empty;
                    if (infos.reportType == 1)
                    {
                        Sql = $"select barcode,filePath,fileName from Report.SampleReportLog where testid in ({ids}) and  dstate=0 order by barcode";
                    }
                    else
                    {
                        Sql = $"select barcode,filePath,fileName from Report.SampleReportLog where testid in ({ids}) and state=1 and  dstate=0 order by barcode";
                    }
                    DataTable infoDT = await _commRepository.GetTable(Sql);

                    if (infoDT != null && infoDT.Rows.Count > 0)
                    {
                        #region web
                        //List<string> infoPaths = new List<string>();
                        //foreach (DataRow dataRow in infoDT.Rows)
                        //{

                        //    string FilePath = dataRow["filePath"] != DBNull.Value ? dataRow["filePath"].ToString() : "";
                        //    if (FilePath != "")
                        //    {
                        //        FilePath = CommonData.readReportFilePath + FilePath;
                        //        infoPaths.Add(FilePath);
                        //    }


                        //    //string FilePath = dataRow["filePath"] != DBNull.Value ? dataRow["filePath"].ToString() : "";
                        //    //if (FilePath != "")
                        //    //{
                        //    //    FilePath = CommonData.readReportFilePath + FilePath;
                        //    //    if (File.Exists(FilePath))
                        //    //    {
                        //    //        infoPaths.Add(FilePath);
                        //    //    }
                        //    //}

                        //}
                        //if (infoPaths.Count > 0)
                        //{
                        //    return mergePDF.MergeFromWeb(infoPaths, nos);
                        //}
                        //else
                        //{
                        //    return "";
                        //}
                        #endregion

                        #region ftp

                        //List<string> reportStreams = new List<string>();
                        //foreach (DataRow dataRow in infoDT.Rows)
                        //{
                        //    string FilePath = dataRow["filePath"] != DBNull.Value ? dataRow["filePath"].ToString() : "";
                        //    if (FilePath != "")
                        //    {
                        //        reportStreams.Add(FilePath);
                        //    }
                        //}

                        //if (reportStreams.Count > 0)
                        //{
                        //    return mergePDF.MergeFromPDF(reportStreams, nos);
                        //}
                        //else
                        //{
                        //    return false;
                        //}
                        #endregion

                        #region 待解决问题

                        //List<byte[]> reportStreams = new List<byte[]>();
                        //foreach (DataRow dataRow in infoDT.Rows)
                        //{
                        //    string FilePath = dataRow["filePath"] != DBNull.Value ? dataRow["filePath"].ToString() : "";
                        //    if (FilePath != "")
                        //    {
                        //        FilePath = CommonData.readReportFilePath + FilePath;
                        //        //infoPaths.Add(FilePath);
                        //        //var folder = new SmbFile("smb://administrator:Mshang330@192.168.8.21/ExportHost-New/");
                        //        //获取目录的SmbFile.
                        //        SmbFile file = new SmbFile($"smb://administrator:Mshang330@{FilePath.Replace('\\', '/')}");
                        //        //获取可读的流。
                        //        var readStream = file.GetInputStream();
                        //        ////创建读取缓存
                        //        //var memStream = new MemoryStream();

                        //        byte[] bytes = new byte[readStream.Length];

                        //        ((Stream)readStream).Read(bytes, 0, bytes.Length);
                        //        ((Stream)readStream).Seek(0, SeekOrigin.Begin);
                        //        ////获取 bytes.
                        //        //((Stream)readStream).Write(memStream);


                        //        //Dispose可读的流。
                        //        readStream.Dispose();

                        //        reportStreams.Add(bytes);
                        //    }
                        //}

                        //if (reportStreams.Count > 0)
                        //{
                        //    return mergePDF.Merge(reportStreams, nos);
                        //}
                        //else
                        //{
                        //    return false;
                        //}
                        #endregion

                        List<string> infoPaths = new List<string>();
                        foreach (DataRow dataRow in infoDT.Rows)
                        {

                            //string FilePath = dataRow["filePath"] != DBNull.Value ? dataRow["filePath"].ToString() : "";
                            //if (FilePath != "")
                            //{
                            //    FilePath = CommonData.readReportFilePath + FilePath;
                            //    infoPaths.Add(FilePath);
                            //}


                            string barcode = dataRow["barcode"] != DBNull.Value ? dataRow["barcode"].ToString() : "";


                            string FilePath = dataRow["filePath"] != DBNull.Value ? dataRow["filePath"].ToString() : "";


                            string fileName = dataRow["fileName"] != DBNull.Value ? dataRow["fileName"].ToString() : "";

                            if (FilePath != "")
                            {
                                FileModel fileModel = new FileModel();
                                FilePath = AppSettingsConstVars.readReportFilePath + FilePath;
                                if (File.Exists(FilePath))
                                {
                                    //FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

                                    fileModel.code = 0;

                                    fileModel.fileName = fileName;

                                    fileModel.fileString = FilePathToString(FilePath);
                                    fileModel.msg = "读取成功";


                                }
                                else
                                {
                                    fileModel.code = 1;

                                    fileModel.fileName = fileName;

                                    fileModel.msg = "报告文件丢失";
                                }
                                fileModes.Add(fileModel);
                            }

                        }

                        commReInfo.infos = fileModes;
                        jm.code = 0;
                        jm.data = commReInfo;


                    }
                    else
                    {
                        jm.code = 2;
                        jm.msg = "未找到报告信息";
                    }
                }
                else
                {
                    jm.code = 2;
                    jm.msg = "样本信息不能为空";
                }

            }
            else
            {
                jm.code = 2;
                jm.msg = "样本信息不能为空";
            }
            return jm;
        }

        /// <summary>  
        /// 将传进来的字符串保存为文件  
        /// </summary>  
        /// <param name="path">需要保存的位置路径</param>  
        /// <param name="binary">需要转换的字符串</param>  
        /// <param name="filename">文件名称（包含扩展名称）</param>  
        public static bool StringToFile(string path, string binary, string filename)
        {
            bool saveFile = true;
            try
            {
                path = path + "\\" + filename;
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                //利用新传来的路径实例化一个FileStream对像  
                BinaryWriter bw = new BinaryWriter(fs);
                //实例化一个用于写的BinaryWriter  
                bw.Write(Convert.FromBase64String(binary));
                bw.Close();
                fs.Close();
                return saveFile;
            }
            catch
            {
                saveFile = false;
                return saveFile;
            }
        }

        /// <summary>
        /// 文件转string
        /// </summary>
        /// <param name="Filepath"></param>
        /// <returns></returns>
        public static string FilePathToString(string Filepath)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(Filepath);
                string base64String = Convert.ToBase64String(bytes);
                return base64String;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //try
            //{
            //    利用新传来的路径实例化一个FileStream对像
            //    using (Stream fs = new FileStream(Filepath, FileMode.Open, FileAccess.Read))
            //    {
            //        byte[] bt = new byte[fs.Length];
            //        file.Read(a, 0, (int)file.Length);
            //        string myStr = Encoding.UTF8.GetString(bt);
            //        return myStr;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}









        }
    }
}
