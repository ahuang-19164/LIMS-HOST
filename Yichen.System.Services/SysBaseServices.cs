using Nito.AsyncEx;
using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Auth.Policys;
using Yichen.Net.Configuration;
using Yichen.Net.Data;
using Yichen.Net.DLLs;
using Yichen.Net.Table;
using Yichen.System.IServices;
using Yichen.System.IServices.User;

namespace Yichen.System.Services
{
    public partial class SysBaseServices : BaseServices<object>, ISysBaseServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IHttpContextUser _httpContextUser;
        private readonly PermissionRequirement _permissionRequirement;
        private readonly IUserLogServices _UserLogServices;
        private readonly ICommRepository _commRepository;

        public SysBaseServices(IUnitOfWork unitOfWork
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
        /// 系统信息修改
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BaseUpdate(uInfo infos)
        {

            WebApiCallBack jm = new WebApiCallBack();

            try
            {
                //uInfo updateInfo = JsonHandle.JsonConvertObject<uInfo>(infos);
                string sql = SqlFormartHelper.updateFormart(infos);
                int info = await _commRepository.sqlcommand(sql);
                if (info > 0)
                {
                    //await StartupHelper.refreshbaseinfo(infos.TableName);
                    jm.msg = SysConstVars.EditSuccess;
                    jm.data = info;
                }
                else
                {
                    jm.code = 1;
                    jm.data = 0;
                    jm.msg = SysConstVars.EditFailure;
                }

            }
            catch (Exception ex)
            {
                jm.code = 2;
                jm.msg = ex.Message;
            }
            return jm;

        }
        /// <summary>
        /// 系统信息查询
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BaseSelect(sInfo infos)
        {



            WebApiCallBack jm = new WebApiCallBack();
            try
            {
                //sInfo selectInfo = JsonHandle.JsonConvertObject<sInfo>(infos);
                string sql = SqlFormartHelper.selectFormart(infos);
                DataTable dt = await _commRepository.GetTable(sql);
                //DataTable dt = await _commRepository.GetTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //jm.data = dt;
                    jm.data = DataTableHelper.DTToString(dt); ;
                    jm.msg = SysConstVars.GetDataSuccess;
                }
            }
            catch (Exception ex)
            {
                jm.code = 1;
                jm.msg = ex.Message;
            }
            return jm;
        }
        /// <summary>
        /// 系统信息插入
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BaseInsert(iInfo infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            try
            {
                //iInfo updateInfo = JsonHandle.JsonConvertObject<iInfo>(infos);
                string sql = SqlFormartHelper.insertFormart(infos);
                int info = await _commRepository.sqlcommand(sql);
                if (info > 0)
                {
                  //await StartupHelper.refreshbaseinfo(infos.TableName);
                   jm.data = info;
                    jm.msg = SysConstVars.InsertSuccess;
                }
                else
                {
                    jm.code = 1;
                    jm.msg = SysConstVars.InsertFailure;
                }
            }
            catch (Exception ex)
            {
                jm.code = 2;
                jm.msg = ex.Message;
            }
            return jm;
        }
        /// <summary>
        /// 系统信息删除
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BaseDelete(dInfo infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            //int a = 1;
            try
            {
                //dInfo updateInfo = JsonHandle.JsonConvertObject<dInfo>(infos);
                string sql = SqlFormartHelper.deleteFormart(infos);
                int info = await _commRepository.sqlcommand(sql);

                if (info > 0)
                {
                   //await StartupHelper.refreshbaseinfo(infos.TableName);
                   jm.code = 0;
                    jm.data= info;
                    jm.msg = SysConstVars.DeleteSuccess;
                }
                else
                {
                    jm.code = 0;
                    jm.msg = SysConstVars.GetDataFailure;
                }
            }
            catch (Exception ex)
            {
                jm.code = 2;
                jm.msg = ex.Message;
            }
            return jm;
        }
        /// <summary>
        /// 系统信息隐藏
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BaseHide(hideInfo infos)
        {
            WebApiCallBack jm = new WebApiCallBack();
            //int a = 1;
            try
            {
                //hideInfo updateInfo = JsonHandle.JsonConvertObject<hideInfo>(infos);
                string sql = SqlFormartHelper.hideFormart(infos);
                int info = await _commRepository.sqlcommand(sql);

                if (info > 0)
                {
                    //await StartupHelper.refreshbaseinfo(infos.TableName);
                    jm.code = 0;
                    jm.data = info;
                    jm.msg = SysConstVars.DeleteSuccess;
                }
                else
                {
                    jm.code = 0;
                    jm.msg = SysConstVars.GetDataFailure;
                }
            }
            catch (Exception ex)
            {
                jm.code = 2;
                jm.msg = ex.Message;
            }
            return jm;
        }
        /// <summary>
        /// 系统信息整表保存
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BaseSaveDT(SaveTableInfo saveTable)
        {
            WebApiCallBack jm = new WebApiCallBack();
            try
            {
                bool a = false;

                int b = 0;

                if (saveTable.DT != null)
                {
                    string Ssql = "";
                    if (saveTable.DT.Columns.Contains("blendid"))
                    {
                        foreach (DataRow dataRow in saveTable.DT.Rows)
                        {
                            if (dataRow["blendid"] != null)
                            {
                                if (dataRow["blendid"].ToString() != "")
                                {
                                    Ssql += $"update {saveTable.TableName} set ";
                                    string setValue = "";
                                    for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
                                    {
                                        if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
                                        {
                                            setValue += saveTable.DT.Columns[ss].ColumnName + "=N'" + dataRow[ss] + "',";
                                        }
                                        else
                                        {
                                            setValue += saveTable.DT.Columns[ss].ColumnName + "=null,";
                                        }


                                    }
                                    Ssql += setValue.Substring(0, setValue.Length - 1);
                                    Ssql += $" where blendid={dataRow["blendid"]};\r\n";
                                    //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
                                }
                                else
                                {
                                    Ssql += "insert into " + saveTable.TableName;
                                    string Cname = string.Empty;
                                    string CValue = string.Empty;
                                    for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
                                    {
                                        Cname += saveTable.DT.Columns[ss].ColumnName + ",";
                                        if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
                                        {
                                            CValue += "N'" + dataRow[ss] + "',";
                                        }
                                        else
                                        {
                                            CValue += "null,";
                                        }
                                    }
                                    Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
                                    CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

                                    Ssql += Cname + " values " + CValue + ";\r\n";
                                    //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
                                }
                            }
                            else
                            {
                                Ssql += "insert into " + saveTable.TableName;
                                string Cname = string.Empty;
                                string CValue = string.Empty;
                                for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
                                {
                                    Cname += saveTable.DT.Columns[ss].ColumnName + ",";
                                    CValue += "N'" + dataRow[ss] + "',";
                                }
                                Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
                                CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";
                                Ssql += Cname + " values " + CValue + ";\r\n";

                            }
                        }
                        b += await _commRepository.sqlcommand(Ssql);
                        //reBase(saveTable.TableName);
                    }
                    else
                    {

                        foreach (DataRow dataRow in saveTable.DT.Rows)
                        {
                            if (dataRow["id"] != null)
                            {
                                if (dataRow["id"].ToString() != "")
                                {
                                    Ssql += $"update {saveTable.TableName} set ";
                                    string setValue = "";
                                    for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
                                    {
                                        if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
                                        {
                                            setValue += saveTable.DT.Columns[ss].ColumnName + "=N'" + dataRow[ss] + "',";
                                        }
                                        else
                                        {
                                            setValue += saveTable.DT.Columns[ss].ColumnName + "=null,";
                                        }


                                    }
                                    Ssql += setValue.Substring(0, setValue.Length - 1);
                                    Ssql += $" where id={dataRow["id"]};\r\n";
                                    //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
                                }
                                else
                                {
                                    Ssql += "insert into " + saveTable.TableName;
                                    string Cname = string.Empty;
                                    string CValue = string.Empty;
                                    for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
                                    {
                                        Cname += saveTable.DT.Columns[ss].ColumnName + ",";
                                        if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
                                        {
                                            CValue += "N'" + dataRow[ss] + "',";
                                        }
                                        else
                                        {
                                            CValue += "null,";
                                        }
                                    }
                                    Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
                                    CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

                                    Ssql += Cname + " values " + CValue + ";\r\n";
                                    //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
                                }
                            }
                            else
                            {
                                Ssql += "insert into " + saveTable.TableName;
                                string Cname = string.Empty;
                                string CValue = string.Empty;
                                for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
                                {
                                    Cname += saveTable.DT.Columns[ss].ColumnName + ",";
                                    CValue += "N'" + dataRow[ss] + "',";
                                }
                                Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
                                CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";
                                Ssql += Cname + " values " + CValue + ";\r\n";

                            }
                        }
                        b += await _commRepository.sqlcommand(Ssql);
                    }


                }

                if (b != 0)
                {
                    jm.code = 0;
                    jm.data = b;
                    jm.msg = SysConstVars.SetDataSuccess;
                }
                else
                {
                    jm.code = 1;
                    jm.msg = SysConstVars.SetDataFailure;
                }

            }
            catch (Exception ex)
            {
                jm.code = 2;
                jm.msg = ex.Message;
            }
            return jm;
        }

    }
}
