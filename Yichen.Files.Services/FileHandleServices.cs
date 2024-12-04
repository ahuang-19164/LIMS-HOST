using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Services;
using Yichen.Files.IServices;
using Yichen.Files.Model;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Caching.Manual;
using Yichen.Net.Configuration;
using Yichen.Net.Files;
using Yichen.Net.Json;
using Yichen.Net.Utility.Helper;
using Yichen.Net.Utility;

namespace Yichen.Files.Services
{
    public partial class FileHandleServices : BaseServices<object>, IFileHandleServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IHttpContextUser _httpContextUser;
        //private readonly PermissionRequirement _permissionRequirement;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IUserLogServices _UserLogServices;

        public FileHandleServices(IUnitOfWork unitOfWork
            , IHttpContextUser httpContextUser
            //,PermissionRequirement permissionRequirement
            //,IHttpContextAccessor httpContextAccessor
            //,IUserLogServices UserLogServices
            )
        {
            _httpContextUser = httpContextUser;
            //_permissionRequirement = permissionRequirement;
            //_httpContextAccessor = httpContextAccessor;
            //_UserLogServices=UserLogServices;
        }

        #region 文件保存

        /// <summary>
        /// 通用保存文件方法
        /// </summary>
        /// <param name="filename">需要保存的位置路径</param>
        /// <param name="filestring">需要转换的字符串</param>
        /// <returns></returns>
        public async Task<bool> fileSave(string filename, string filestring)
        {
            using (await _mutex.LockAsync())
            {
                bool saveFile = true;
                try
                {
                    string path = filename;
                    FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                    //利用新传来的路径实例化一个FileStream对像  
                    BinaryWriter bw = new BinaryWriter(fs);
                    //实例化一个用于写的BinaryWriter  
                    bw.Write(Convert.FromBase64String(filestring));
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
        }


        public async Task<bool> fileSave(string filename, string filestring, string filetype)
       {
            throw new NotImplementedException();
        }

        #endregion


        #region 系统文件
        /// <summary>
        /// 系统信息查询
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>

        public async Task<WebApiCallBack> SysUpFile(commInfoModel<FileModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();

            commReInfo<commRehandleModel> commReInfo = new commReInfo<commRehandleModel>();
            List<commRehandleModel> commReList = new List<commRehandleModel>();
            foreach (FileModel fileModel in infos.infos)
            {
                string newpath = "";
                switch (fileModel.fileType)
                {
                    case "1":
                        newpath = AppSettingsConstVars.baseFilePath + "\\flow";
                        break;
                    case "2":
                        newpath = AppSettingsConstVars.baseFilePath + "\\img";
                        break;
                    default:
                        break;
                }
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }
                if (fileModel.dirname != null && fileModel.dirname.ToString() != "")
                {
                    newpath = newpath + "\\" + fileModel.dirname;
                    if (!Directory.Exists(newpath))
                        Directory.CreateDirectory(newpath);
                }
                string fileFullPath = newpath + "\\" + fileModel.fileName;
                //string fileFullPath = fileName;
                if (File.Exists(fileFullPath))
                {
                    int a = 1;
                    string fileFullPath2 = fileFullPath + a;
                    while (File.Exists(fileFullPath2))
                    {
                        a++;
                        fileFullPath2 = fileFullPath + a;
                    }
                    File.Copy(fileFullPath, fileFullPath2);
                    File.Delete(fileFullPath);    //存在则删除
                }
                commRehandleModel commRehandle = new commRehandleModel();
                try
                {

                   FileHelpers.StringToFile(fileFullPath, fileModel.fileString.ToString());

                    commRehandle.code = 0;
                    commRehandle.msg = "上传成功";
                }
                catch
                {
                    commRehandle.code = 1;
                    commRehandle.msg = "上传失败";
                }
                commReList.Add(commRehandle);
            }
            commReInfo.infos = commReList;
            jm.data = commReInfo;
            return jm;
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>

        public async Task<WebApiCallBack> SysDownFile(commInfoModel<FileModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            commReInfo<FileModel> commReInfo = new commReInfo<FileModel>();

            List<FileModel> infosList = new List<FileModel>();

            foreach (FileModel fileModel in infos.infos)
            {
                FileModel fileinfo = new FileModel();
                string newpath = AppSettingsConstVars.baseFilePath;
                if (!Directory.Exists(newpath))
                    Directory.CreateDirectory(newpath);
                if (fileModel.dirname != null && fileModel.dirname.ToString() != "")
                {
                    newpath = newpath + "\\" + fileModel.dirname;
                    if (!Directory.Exists(newpath))
                        Directory.CreateDirectory(newpath);
                }
                string fileFullPath = newpath + "\\" + fileModel.fileName;
                if (File.Exists(fileFullPath))
                {
                    //FileStream fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read);
                    //int shardSize = 1 * 1024 * 1024;//一次1M
                    //int count = (int)(fs.Length / shardSize);
                    //byte[] datas = new byte[shardSize];
                    //await fs.ReadAsync(datas, 0, datas.Length);
                    //return File(fs, "application/octet-stream");
                    string fs = FileHelpers.FilePathToString(fileFullPath);
                    fileinfo.code = 0;
                    fileinfo.fileString = fs;
                    infosList.Add(fileinfo);
                }
                else
                {

                    fileinfo.code = 1;
                    fileinfo.msg = "未找到该文件";
                }
            }
            commReInfo.infos = infosList;
            jm.data = commReInfo;
            return jm;
        }


        /// <summary>
        /// 上传flow文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>

        public async Task<WebApiCallBack> FlowUpFile(commInfoModel<FileModel> infos)
       {
            WebApiCallBack jm = new WebApiCallBack();

            commReInfo<commRehandleModel> commReInfo = new commReInfo<commRehandleModel>();
            List<commRehandleModel> commReList = new List<commRehandleModel>();
            foreach (FileModel fileModel in infos.infos)
            {
                string newpath = AppSettingsConstVars.flowFilePath;
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }
                if (fileModel.dirname != null && fileModel.dirname.ToString() != "")
                {
                    newpath = newpath + "\\" + fileModel.dirname;
                    if (!Directory.Exists(newpath))
                        Directory.CreateDirectory(newpath);
                }
                string fileFullPath = newpath + "\\" + fileModel.fileName;
                //string fileFullPath = fileName;
                if (File.Exists(fileFullPath))
                {
                    int a = 1;
                    string fileFullPath2 = fileFullPath + a;
                    while (File.Exists(fileFullPath2))
                    {
                        a++;
                        fileFullPath2 = fileFullPath + a;
                    }
                    File.Copy(fileFullPath, fileFullPath2);
                    File.Delete(fileFullPath);    //存在则删除
                }
                commRehandleModel commRehandle = new commRehandleModel();
                try
                {

                    FileHelpers.StringToFile(fileFullPath, fileModel.fileString.ToString());

                    commRehandle.code = 0;
                    commRehandle.info = fileModel.fileName;
                    commRehandle.msg = "上传成功";
                }
                catch
                {
                    commRehandle.code = 1;
                    commRehandle.info = fileModel.fileName;
                    commRehandle.msg = "上传失败";
                }
                commReList.Add(commRehandle);
            }
            commReInfo.infos = commReList;
            jm.data = commReInfo;
            return jm;
        }
        /// <summary>
        /// 下载flow文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>

        public async Task<WebApiCallBack> FlowDownFile(commInfoModel<FileModel> infos)
       {
            WebApiCallBack jm = new WebApiCallBack();
            commReInfo<FileModel> commReInfo = new commReInfo<FileModel>();
            List<FileModel> infosList = new List<FileModel>();
            foreach (FileModel fileModel in infos.infos)
            {
                FileModel fileinfo = new FileModel();
                string newpath = AppSettingsConstVars.flowFilePath;
                if (!Directory.Exists(newpath))
                    Directory.CreateDirectory(newpath);
                if (fileModel.dirname != null && fileModel.dirname.ToString() != "")
                {
                    newpath = newpath + "\\" + fileModel.dirname;
                    if (!Directory.Exists(newpath))
                        Directory.CreateDirectory(newpath);
                }
                string fileFullPath = newpath + "\\" + fileModel.fileName;
                if (File.Exists(fileFullPath))
                {
                    //FileStream fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read);
                    //int shardSize = 1 * 1024 * 1024;//一次1M
                    //int count = (int)(fs.Length / shardSize);
                    //byte[] datas = new byte[shardSize];
                    //await fs.ReadAsync(datas, 0, datas.Length);
                    //return File(fs, "application/octet-stream");
                    string fs = FileHelpers.FilePathToString(fileFullPath);
                    fileinfo.code = 0;
                    fileinfo.fileName = fileModel.fileName;
                    fileinfo.fileString = fs;
                    infosList.Add(fileinfo);
                }
                else
                {

                    fileinfo.code = 1;
                    fileinfo.fileName = fileModel.fileName;
                    fileinfo.msg = "未找到该文件";
                }
            }
            commReInfo.infos = infosList;
            jm.data = commReInfo;
            return jm;
        }
        /// <summary>
        /// 上传检验结果文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>

        public async Task<WebApiCallBack> TestUpFile(commInfoModel<FileModel> infos)
       {
            WebApiCallBack jm = new WebApiCallBack();

            commReInfo<commRehandleModel> commReInfo = new commReInfo<commRehandleModel>();
            List<commRehandleModel> commReList = new List<commRehandleModel>();
            foreach (FileModel fileModel in infos.infos)
            {
                string newpath = AppSettingsConstVars.TestFilePath;
                //创建结果文件根目录
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }
                if (fileModel.dirname != null && fileModel.dirname.ToString() != "")
                {
                    newpath = newpath + "\\" + fileModel.dirname;
                    if (!Directory.Exists(newpath))
                        Directory.CreateDirectory(newpath);
                }
                string fileFullPath = newpath + "\\" + fileModel.fileName;
                //string fileFullPath = fileName;
                if (File.Exists(fileFullPath))
                {
                    int a = 1;
                    string fileFullPath2 = fileFullPath + a;
                    while (File.Exists(fileFullPath2))
                    {
                        a++;
                        fileFullPath2 = fileFullPath + a;
                    }
                    File.Copy(fileFullPath, fileFullPath2);
                    File.Delete(fileFullPath);    //存在则删除
                }
                commRehandleModel commRehandle = new commRehandleModel();
                try
                {

                   FileHelpers.StringToFile(fileFullPath, fileModel.fileString.ToString());

                    commRehandle.code = 0;
                    commRehandle.msg = "上传成功";
                }
                catch
                {
                    commRehandle.code = 1;
                    commRehandle.msg = "上传失败";
                }
                commReList.Add(commRehandle);
            }
            commReInfo.infos = commReList;
            jm.data = commReInfo;
            return jm;
        }
        /// <summary>
        /// 下载检验结果文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>

        public async Task<WebApiCallBack> TestDownFile(commInfoModel<FileModel> infos)
        {
            WebApiCallBack jm = new WebApiCallBack();

            await Task.Run(() =>
            {
                commReInfo<FileModel> commReInfo = new commReInfo<FileModel>();

                List<FileModel> infosList = new List<FileModel>();

                foreach (FileModel fileModel in infos.infos)
                {
                    FileModel fileinfo = new FileModel();
                    string newpath = AppSettingsConstVars.TestFilePath;
                    if (!Directory.Exists(newpath))
                        Directory.CreateDirectory(newpath);
                    if (fileModel.dirname != null && fileModel.dirname.ToString() != "")
                    {
                        newpath = newpath + "\\" + fileModel.dirname;
                        if (!Directory.Exists(newpath))
                            Directory.CreateDirectory(newpath);
                    }
                    string fileFullPath = newpath + "\\" + fileModel.fileName;
                    if (File.Exists(fileFullPath))
                    {
                        //FileStream fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read);
                        //int shardSize = 1 * 1024 * 1024;//一次1M
                        //int count = (int)(fs.Length / shardSize);
                        //byte[] datas = new byte[shardSize];
                        //await fs.ReadAsync(datas, 0, datas.Length);
                        //return File(fs, "application/octet-stream");

                        string fs = FileHelpers.FilePathToString(fileFullPath);
                        fileinfo.code = 0;
                        fileinfo.fileString = fs;
                        infosList.Add(fileinfo);
                    }
                    else
                    {

                        fileinfo.code = 1;
                        fileinfo.msg = "未找到该文件";
                    }
                }
                commReInfo.infos = infosList;
                jm.data = commReInfo;
            });

            return jm;
        }




        #endregion











        #region 系统更新


        /// <summary>
        /// 客户端文件更新
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<FileStream> UpgradeClient(DownFileModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {

                string FileNames = infos.FileName;

                if (FileNames != null)
                {


                    string rd = ManualDataCache.Instance.Get(infos.UserName) != null ? ManualDataCache.Instance.Get(infos.UserName).ToString() : "";


                    if (rd != "")
                    {
                        if (rd == "\"" + infos.UserToken + "\"")
                        {
                            //string filePath = CommonData.readReportFilePath + FileNames;
                            string filePath = AppSettingsConstVars.ClientFilePath + "//" + FileNames;
                            if (File.Exists(filePath))
                            {
                                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                                //jm.code = 0;
                                //jm.data = fs;
                                return fs;
                            }
                            else
                            {
                                jm.code = 1;
                                jm.msg = $"未找到文件{infos.FileName}";
                            }
                        }
                        else
                        {
                            jm.code = 1;
                            jm.msg = $"非法访问";
                        }
                    }
                    else
                    {
                        jm.code = 1;
                        jm.msg = $"非法访问";
                    }


                }
                else
                {
                    jm.code = 1;
                    jm.msg = $"提交信息错误";
                }

                return null;

            }
        }
        /// <summary>
        /// 报告客户端文件更新
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<FileStream> UpgradeReportClient(DownFileModel infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            using (await _mutex.LockAsync())
            {

                string FileNames = infos.FileName;

                if (FileNames != null)
                {

                    string rd = ManualDataCache.Instance.Get(infos.UserName) != null ? ManualDataCache.Instance.Get(infos.UserName).ToString() : "";


                    if (rd != "")
                    {
                        if (rd == infos.UserToken)
                        {
                            //string filePath = CommonData.readReportFilePath + FileNames;
                            string filePath = AppSettingsConstVars.ReportToolPath + "//" + FileNames;
                            if (File.Exists(filePath))
                            {
                                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                                return fs;
                            }
                            else
                            {
                                jm.code = 1;
                                jm.msg = $"未找到文件{infos.FileName}";
                            }
                        }
                        else
                        {
                            jm.code = 1;
                            jm.msg = $"非法访问";
                        }
                    }
                    else
                    {
                        jm.code = 1;
                        jm.msg = $"非法访问";
                    }
                }
                else
                {
                    jm.code = 1;
                    jm.msg = $"提交信息错误";
                }

                return null;

            }
        }

        /// <summary>
        /// 获取发布更新信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> UpgradeUpInfos(ClientInfoModel infos)
        {
            using (await _mutex.LockAsync())
            {
                WebApiCallBack jm = new WebApiCallBack();
                if (infos.state == 1)
                {
                    string updateInfoPath = AppSettingsConstVars.ClientFilePath;
                    if (Directory.Exists(updateInfoPath))
                    {


                        //Directory.CreateDirectory(updateInfoPath);
                        string ConfigFilePath = AppSettingsConstVars.ClientFilePath + "\\upfileinfo.conf";
                        if (File.Exists(ConfigFilePath))
                        {
                            //string configInfo = File.ReadAllText(ConfigFilePath,Encoding.GetEncoding("GB2312")).Replace("\r\n", "");
                            //string configInfo = File.ReadAllText(ConfigFilePath,Encoding.Default).Replace("\r\n", "");


                            StreamReader cinfo = new StreamReader(ConfigFilePath, Encoding.GetEncoding("GB2312"));
                            string configInfo = cinfo.ReadToEnd();



                            clientModel client = JsonHandle.JsonConvertObject<clientModel>(configInfo);
                            if (client.Version != infos.Version)
                            {
                                if (client.fileInfo != null)
                                {
                                    List<FileInfoModel> FileInfos = new List<FileInfoModel>();
                                    foreach (FileInfoModel fileInfo in client.fileInfo)
                                    {
                                        if (fileInfo.FileName != null && fileInfo.FileName != "")
                                        {
                                            string filePath = updateInfoPath + "//" + fileInfo.FileName;
                                            if (File.Exists(filePath))
                                            {
                                                FileInfo file = new FileInfo(filePath);
                                                FileInfoModel GetfileInfo = new FileInfoModel();
                                                GetfileInfo.FileName = fileInfo.FileName;
                                                GetfileInfo.FileSize = (file.Length / 1024).ToString() + "KB";
                                                GetfileInfo.CreateTime = file.CreationTime.ToString("yyyy-MM-dd");
                                                FileInfos.Add(GetfileInfo);
                                            }
                                        }
                                    }
                                    if (FileInfos.Count > 0)
                                    {
                                        jm.code = 0;
                                        string code = VierificationCode.RandomText();
                                        string rd = CommonHelper.EnPassword(infos.UserName + code, DateTime.Now.ToString("yyyy-MM-dd"));
                                        ManualDataCache.Instance.Set(infos.UserName, rd, 5);
                                        client.userToken = rd;
                                        client.fileInfo = FileInfos;
                                        jm.data = client;
                                    }
                                    else
                                    {

                                        jm.code = 1;
                                        //client.fileInfo = FileInfos;
                                        jm.data = client;
                                    }

                                }
                                else
                                {
                                    jm.code = 1;
                                    jm.msg = "更新配置文件错误";
                                }
                            }
                            else
                            {

                                jm.code = 0;
                                jm.msg = "已经是最新版本";
                            }
                            cinfo.Close();
                        }
                        else
                        {
                            jm.code = 0;
                            jm.msg = "已经是最新版本";
                        }
                    }
                }
                else
                {
                    if (infos.state == 2)
                    {
                        string updateInfoPath = AppSettingsConstVars.ReportToolPath;
                        if (Directory.Exists(updateInfoPath))
                        {



                            //Directory.CreateDirectory(updateInfoPath);
                            string ConfigFilePath = AppSettingsConstVars.ReportToolPath + "//upfileinfo.conf";
                            if (!File.Exists(ConfigFilePath))
                            {

                                jm.code = 1;
                                jm.msg = "配置文件不存在";


                            }
                            else
                            {
                                string configInfo = File.ReadAllText(ConfigFilePath).Replace("\r\n", "");
                                clientModel client = JsonHandle.JsonConvertObject<clientModel>(configInfo);
                                if (client.Version == infos.Version)
                                {

                                    jm.code = 0;
                                    jm.msg = "已经是最新版本";

                                }
                                else
                                {
                                    if (client.fileInfo == null)
                                    {
                                        jm.code = 1;
                                        jm.msg = "更新配置文件错误";
                                    }
                                    else
                                    {
                                        List<FileInfoModel> FileInfos = new List<FileInfoModel>();
                                        foreach (FileInfoModel fileInfo in client.fileInfo)
                                        {
                                            if (fileInfo.FileName != null && fileInfo.FileName != "")
                                            {
                                                string filePath = updateInfoPath + "//" + fileInfo.FileName;
                                                if (File.Exists(filePath))
                                                {
                                                    FileInfo file = new FileInfo(filePath);
                                                    FileInfoModel GetfileInfo = new FileInfoModel();
                                                    GetfileInfo.FileName = fileInfo.FileName;
                                                    GetfileInfo.FileSize = (file.Length / 1024).ToString() + "KB";
                                                    GetfileInfo.CreateTime = file.CreationTime.ToString("yyyy-MM-dd");
                                                    FileInfos.Add(GetfileInfo);
                                                }
                                            }
                                        }
                                        if (FileInfos.Count > 0)
                                        {
                                            jm.code = 0;
                                            client.fileInfo = FileInfos;
                                            jm.data = client;
                                        }
                                        else
                                        {
                                            jm.code = 1;
                                            jm.msg = "为获取到更新文件";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        jm.code = 1;
                        jm.msg = "提交信息有误！";
                    }
                }
                return jm;
            }
        }


        #endregion


        #region  上传实例



        string[] pictureFormatArray = { ".png", ".jpg", ".jpeg", ".gif", ".PNG", ".JPG", ".JPEG", ".GIF" };
        [HttpPost]
        [Route("upload")]

        public async Task<ActionResult> Upload([Required] string name, IFormFile file)
        {
            string uniqueFileName = null;
            JsonResult result = new JsonResult(new { name = "kxy1" });
            if (file != null)
            {
                //限制100M
                if (file.Length > 104857600)
                {
                    return new BadRequestObjectResult("上传文件过大");
                }
                //文件格式
                var fileExtension = Path.GetExtension(file.FileName);
                if (!pictureFormatArray.Contains(fileExtension))
                {
                    return new BadRequestObjectResult("上传文件格式错误");
                }

                var size = "";
                if (file.Length < 1024)
                    size = file.Length.ToString() + "B";
                else if (file.Length >= 1024 && file.Length < 1048576)
                    size = ((float)file.Length / 1024).ToString("F2") + "KB";
                else if (file.Length >= 1048576 && file.Length < 104857600)
                    size = ((float)file.Length / 1024 / 1024).ToString("F2") + "MB";
                else size = file.Length.ToString() + "B";
                //存放文件位置
                string uploadsFolder = Path.Combine(AppSettingsConstVars.baseFilePath, "Images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }

                //TODO：文件md5哈希校验
                //await _fileManager.Create(name, uniqueFileName, fileExtension, "", size, filePath, "/Images/" + uniqueFileName, FileType.Image);
            }
            result = new JsonResult(new { name = "kxy1" });
            return result;

        }

        #endregion

    }
}
