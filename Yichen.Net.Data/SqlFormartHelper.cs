namespace Yichen.Net.Data
{
    /// <summary>
    /// 格式化sql
    /// </summary>
    public class SqlFormartHelper
    {
        /// <summary>
        /// insert格式化
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string insertFormart(iInfo info)
        {
            string Ssql = "";
            if (info.TableName != null)
            {
                if (info.TableName.Length != 0)
                {
                    string Cname = string.Empty;
                    string CValue = string.Empty;
                    //update HLIMSDB.Common.UserInfo set remark='aaaa'
                    Ssql = "insert into " + info.TableName;
                    if (info.values != null)
                    {
                        foreach (KeyValuePair<string, object> item in info.values)
                        {
                            Cname += item.Key + ",";
                            CValue += "N'" + item.Value + "',";
                        }
                        Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
                        CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

                        Ssql += Cname + " values " + CValue + ";\r\n";
                    }
                    else
                    {
                        if (info.listValues != null)
                        {
                            foreach (KeyValuePair<string, List<object>> item in info.listValues)
                            {
                                Cname += item.Key + ",";
                                if (info.listValues.Count != item.Value.Count)
                                {
                                    foreach (string s in item.Value)
                                    {
                                        CValue += "'" + item.Value + "',";
                                    }
                                    CValue = "(" + CValue.Substring(0, CValue.Length - 1) + "),";
                                }

                            }
                            CValue = CValue.Substring(0, CValue.Length - 1);
                            Ssql += Cname + " values " + CValue + ";\r\n";
                        }
                    }
                }
            }
            return Ssql;
        }
        /// <summary>
        /// update格式化
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string updateFormart(uInfo info)
        {
            if (info.TableName != null)
            {
                string Ssql = "update " + info.TableName;
                if (info.TableName.Length != 0)
                {
                    //update HLIMSDB.Common.UserInfo set remark='aaaa'

                    if (info.value != null)
                    {
                        if (info.value != "")
                        {
                            Ssql += " set  " + info.value;
                            if (info.DataValueID != 0)
                            {
                                Ssql += " where  id=" + info.DataValueID;

                            }
                            else
                            {
                                if (info.wheres != null)
                                {
                                    if (info.wheres != "")
                                    {
                                        Ssql += " where  " + info.wheres;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (info.values != null)
                            {
                                string setValue = string.Empty;
                                foreach (KeyValuePair<string, object> pair in info.values)
                                {
                                    if(pair.Value==null)
                                    {
                                        setValue += pair.Key + "=null,";
                                    }
                                    else
                                    {
                                        setValue += pair.Key + "=N'" + pair.Value + "',";
                                    }
                                    
                                }
                                Ssql += " set  " + setValue.Substring(0, setValue.Length - 1);
                                if (info.DataValueID != 0)
                                {
                                    Ssql += " where  id=" + info.DataValueID;

                                }
                                else
                                {
                                    if (info.wheres != "")
                                    {
                                        Ssql += " where  " + info.values;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (info.values != null)
                        {
                            string setValue = string.Empty;
                            foreach (KeyValuePair<string, object> pair in info.values)
                            {
                                if (pair.Value == null)
                                {
                                    setValue += pair.Key + "=null,";
                                }
                                else
                                {
                                    setValue += pair.Key + "=N'" + pair.Value + "',";
                                }
                            }
                            Ssql += " set  " + setValue.Substring(0, setValue.Length - 1);
                            if (info.DataValueID != 0)
                            {
                                Ssql += " where  id=" + info.DataValueID;
                            }
                            else
                            {
                                if (info.wheres != "")
                                {
                                    Ssql += " where  " + info.wheres;
                                }
                            }
                        }
                    }

                }
                return Ssql + ";\r\n";
            }
            return "";
        }
        /// <summary>
        /// delete格式化
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string deleteFormart(dInfo info)
        {
            if (info.TableName == null||info.TableName.Trim().Length == 0)
                return "";
            string Ssql = "delete " + info.TableName;
            if (info.DataValueID <= 0)
                return "";
            Ssql += " where  id=" + info.DataValueID;
            return Ssql;
        }
        /// <summary>
        /// 查询格式化
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string selectFormart(sInfo info)
        {


            if (info.TableName == null|| info.TableName.Trim().Length == 0)
                return "";
            string Ssql = "select ";
            if (info.values != null&& info.values.Trim().Length != 0)
            {
                Ssql += info.values + "  from  " + info.TableName;
            }
            else
            {
                Ssql += " * from  " + info.TableName;
            }
            if (info.wheres != null&& info.wheres.Trim().Length != 0)
                Ssql += " where " + info.wheres;
            if (info.GroupColumns != null&& info.GroupColumns.Trim().Length!=0)
                Ssql += " group by " + info.GroupColumns;
            if (info.OrderColumns != null&& info.OrderColumns.Trim().Length != 0)
                Ssql += " order by " + info.OrderColumns;
            return Ssql;
        }
        /// <summary>
        /// 隐藏格式化
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string hideFormart(hideInfo info)
        {

            if (info.TableName == null|| info.TableName.Trim().Length == 0)
                return "";
            if (info.DataValueID <=0)
                return "";
            string Ssql = "update " + info.TableName + " Set dstate=1";
            Ssql += " where  id=" + info.DataValueID;
            return Ssql;
        }




        #region select

        ///// <summary>
        ///// 查询数据返回datatable
        ///// </summary>
        ///// <param name="SelectValue">查询json</param>
        ///// <returns></returns>
        //public static DataTable SelectsH(sInfo info)
        //{
        //    try
        //    {
        //        DataTable dt = null;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //            if (info.TableName != null)
        //            {
        //                if (info.TableName.Length != 0)
        //                {
        //                    string Ssql = "select ";
        //                    if (info.values != null)
        //                    {
        //                        if (info.values.Length != 0)
        //                        {
        //                            Ssql += info.values + "  from  " + info.TableName;
        //                        }
        //                        else
        //                        {
        //                            Ssql += " * from  " + info.TableName;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        Ssql += " * from  " + info.TableName;
        //                    }

        //                    if (info.wheres != null)
        //                    {
        //                        if (info.wheres.Length != 0)
        //                        {
        //                            Ssql += " where " + info.wheres;
        //                        }

        //                    }
        //                    if (info.GroupColumns != null)
        //                    {
        //                        if (info.GroupColumns.Length != 0)
        //                        {
        //                            Ssql += " group by " + info.GroupColumns;
        //                        }

        //                    }
        //                    if (info.OrderColumns != null)
        //                    {
        //                        if (info.OrderColumns.Length != 0)
        //                        {
        //                            Ssql += " order by " + info.OrderColumns;
        //                        }

        //                    }
        //                    dt = HLDBSqlHelper.ExecuteDataset(CommonData.sqlconnH, Ssql).Tables[0];
        //                    dt.TableName = info.TableName;
        //                }
        //            }
        //        return dt;

        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 查询数据返回datatable
        ///// </summary>
        ///// <param name="SelectValue">查询json</param>
        ///// <returns></returns>
        //public static DataTable SelectsW(sInfo info)
        //{
        //    try
        //    {
        //        DataTable dt = null;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //            if (info.TableName != null)
        //            {
        //                if (info.TableName.Length != 0)
        //                {
        //                    string Ssql = "select ";
        //                    if (info.values != null)
        //                    {
        //                        if (info.values.Length != 0)
        //                        {
        //                            Ssql += info.values + "  from  " + info.TableName;
        //                        }
        //                        else
        //                        {
        //                            Ssql += " * from  " + info.TableName;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        Ssql += " * from  " + info.TableName;
        //                    }

        //                    if (info.wheres != null)
        //                    {
        //                        if (info.wheres.Length != 0)
        //                        {
        //                            Ssql += " where " + info.wheres;
        //                        }

        //                    }
        //                    if (info.GroupColumns != null)
        //                    {
        //                        if (info.GroupColumns.Length != 0)
        //                        {
        //                            Ssql += " group by " + info.GroupColumns;
        //                        }

        //                    }
        //                    if (info.OrderColumns != null)
        //                    {
        //                        if (info.OrderColumns.Length != 0)
        //                        {
        //                            Ssql += " order by " + info.OrderColumns;
        //                        }

        //                    }
        //                    dt = SqlHelper.ExecuteDataset(CommonData.sqlconnW, CommandType.Text, Ssql).Tables[0];
        //                    dt.TableName = info.TableName;
        //                }
        //            }
        //        return dt;

        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}


        ///// <summary>
        ///// 查询数据返回dataset
        ///// </summary>
        ///// <param name="SelectValue">查询json</param>
        ///// <returns></returns>
        //public static DataSet SelectsDSH(sInfo info)
        //{
        //    try
        //    {
        //        DataSet dt = null;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //            if (info.TableName != null)
        //            {
        //                if (info.TableName.Length != 0)
        //                {
        //                    string sss = "";
        //                    string[] names = info.TableName.Split(',');
        //                    foreach (string name in names)
        //                    {
        //                        string Ssql = "select ";
        //                        if (info.values != null)
        //                        {
        //                            if (info.values.Length != 0)
        //                            {
        //                                Ssql += info.values + "  from  " + name;
        //                            }
        //                            else
        //                            {
        //                                Ssql += " * from  " + name;
        //                            }

        //                        }
        //                        else
        //                        {
        //                            Ssql += " * from  " + name;
        //                        }

        //                        if (info.wheres != null)
        //                        {
        //                            if (info.wheres.Length != 0)
        //                            {
        //                                Ssql += " where " + info.wheres;
        //                            }

        //                        }
        //                        if (info.GroupColumns != null)
        //                        {
        //                            if (info.GroupColumns.Length != 0)
        //                            {
        //                                Ssql += " group by " + info.GroupColumns;
        //                            }

        //                        }
        //                        if (info.OrderColumns != null)
        //                        {
        //                            if (info.OrderColumns.Length != 0)
        //                            {
        //                                Ssql += " order by " + info.OrderColumns;
        //                            }

        //                        }
        //                        sss += Ssql + ";";
        //                    }

        //                    dt = HLDBSqlHelper.ExecuteDataset(CommonData.sqlconnH, sss);
        //                }
        //            }
        //        return dt;

        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 查询数据返回dataset
        ///// </summary>
        ///// <param name="SelectValue">查询json</param>
        ///// <returns></returns>
        //public static DataSet SelectsDSW(sInfo info)
        //{
        //    try
        //    {
        //        DataSet dt = null;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //            if (info.TableName != null)
        //            {
        //                if (info.TableName.Length != 0)
        //                {
        //                    string sss = "";
        //                    string[] names = info.TableName.Split(',');
        //                    foreach (string name in names)
        //                    {
        //                        string Ssql = "select ";
        //                        if (info.values != null)
        //                        {
        //                            if (info.values.Length != 0)
        //                            {
        //                                Ssql += info.values + "  from  " + name;
        //                            }
        //                            else
        //                            {
        //                                Ssql += " * from  " + name;
        //                            }

        //                        }
        //                        else
        //                        {
        //                            Ssql += " * from  " + name;
        //                        }

        //                        if (info.wheres != null)
        //                        {
        //                            if (info.wheres.Length != 0)
        //                            {
        //                                Ssql += " where " + info.wheres;
        //                            }

        //                        }
        //                        if (info.GroupColumns != null)
        //                        {
        //                            if (info.GroupColumns.Length != 0)
        //                            {
        //                                Ssql += " group by " + info.GroupColumns;
        //                            }

        //                        }
        //                        if (info.OrderColumns != null)
        //                        {
        //                            if (info.OrderColumns.Length != 0)
        //                            {
        //                                Ssql += " order by " + info.OrderColumns;
        //                            }

        //                        }
        //                        sss += Ssql + ";";
        //                    }

        //                    dt = SqlHelper.ExecuteDataset(CommonData.sqlconnW, CommandType.Text, sss);
        //                }
        //            }
        //        return dt;

        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}


        #endregion





        #region update
        //public static int updatesH(uInfo info)
        //{
        //    try
        //    {
        //        int a = 0;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //            if (info.TableName != null)
        //            {
        //                if (info.TableName.Length != 0)
        //                {
        //                    //update HLIMSDB.Common.UserInfo set remark='aaaa'
        //                    string Ssql = "update " + info.TableName;
        //                    if (info.value != null)
        //                    {
        //                        if (info.value != "")
        //                        {
        //                            Ssql += " set  " + info.value;
        //                            if (info.DataValueID != 0)
        //                            {
        //                                Ssql += " where  id=" + info.DataValueID;
        //                                a = HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                                reBase(info.TableName);
        //                            }
        //                            else
        //                            {
        //                                if (info.wheres != null)
        //                                {
        //                                    if (info.wheres != "")
        //                                    {
        //                                        Ssql += " where  " + info.wheres;
        //                                        a = HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                                        reBase(info.TableName);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (info.values != null)
        //                            {
        //                                string setValue = string.Empty;
        //                                foreach (KeyValuePair<string, object> pair in info.values)
        //                                {
        //                                    setValue += pair.Key + "=N'" + pair.Value + "',";
        //                                }
        //                                Ssql += " set  " + setValue.Substring(0, setValue.Length - 1);
        //                                if (info.DataValueID != 0)
        //                                {
        //                                    Ssql += " where  id=" + info.DataValueID;
        //                                    a = HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                                    reBase(info.TableName);
        //                                }
        //                                else
        //                                {
        //                                    if (info.wheres != "")
        //                                    {
        //                                        Ssql += " where  " + info.values;
        //                                        a = HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                                        reBase(info.TableName);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (info.values != null)
        //                        {
        //                            string setValue = string.Empty;
        //                            foreach (KeyValuePair<string, object> pair in info.values)
        //                            {
        //                                setValue += pair.Key + "=N'" + pair.Value + "',";
        //                            }
        //                            Ssql += " set  " + setValue.Substring(0, setValue.Length - 1);
        //                            if (info.DataValueID != 0)
        //                            {
        //                                Ssql += " where  id=" + info.DataValueID;
        //                                a = HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                                reBase(info.TableName);
        //                            }
        //                            else
        //                            {
        //                                if (info.wheres != "")
        //                                {
        //                                    Ssql += " where  " + info.wheres;
        //                                    a = HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                                    reBase(info.TableName);
        //                                }
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        return a;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }

        //}


        //public static int updatesW(uInfo info)
        //{
        //    try
        //    {
        //        int a = 0;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //            if (info.TableName != null)
        //            {
        //                if (info.TableName.Length != 0)
        //                {
        //                    //update HLIMSDB.Common.UserInfo set remark='aaaa'
        //                    string Ssql = "update " + info.TableName;
        //                    if (info.value != null)
        //                    {
        //                        if (info.value != "")
        //                        {
        //                            Ssql += " set  " + info.value;
        //                            if (info.DataValueID != 0)
        //                            {
        //                                Ssql += " where  id=" + info.DataValueID;
        //                                a = SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                            }
        //                            else
        //                            {
        //                                if (info.wheres != null)
        //                                {
        //                                    if (info.wheres != "")
        //                                    {
        //                                        Ssql += " where  " + info.wheres;
        //                                        a = SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (info.values != null)
        //                            {
        //                                string setValue = string.Empty;
        //                                foreach (KeyValuePair<string, object> pair in info.values)
        //                                {
        //                                    setValue += pair.Key + "=N'" + pair.Value + "',";
        //                                }
        //                                Ssql += " set  " + setValue.Substring(0, setValue.Length - 1);
        //                                if (info.DataValueID != 0)
        //                                {
        //                                    Ssql += " where  id=" + info.DataValueID;
        //                                    a = SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                                }
        //                                else
        //                                {
        //                                    if (info.wheres != "")
        //                                    {
        //                                        Ssql += " where  " + info.values;
        //                                        a = SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (info.values != null)
        //                        {
        //                            string setValue = string.Empty;
        //                            foreach (KeyValuePair<string, object> pair in info.values)
        //                            {
        //                                setValue += pair.Key + "=N'" + pair.Value + "',";
        //                            }
        //                            Ssql += " set  " + setValue.Substring(0, setValue.Length - 1);
        //                            if (info.DataValueID != 0)
        //                            {
        //                                Ssql += " where  id=" + info.DataValueID;
        //                                a = SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                            }
        //                            else
        //                            {
        //                                if (info.wheres != "")
        //                                {
        //                                    Ssql += " where  " + info.wheres;
        //                                    a = SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                                }
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        return a;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }

        //}
        #endregion




        #region delete

        //public static int deletesH(dInfo info)
        //{
        //    try
        //    {
        //        int a = 0;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //            if (info.TableName != null)
        //            {
        //                if (info.TableName.Length != 0)
        //                {
        //                    //update HLIMSDB.Common.UserInfo set remark='aaaa'
        //                    string Ssql = "delete " + info.TableName;
        //                    if (info.DataValueID > 1)
        //                    {
        //                        Ssql += " where  id=" + info.DataValueID;
        //                        a = HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                        reBase(info.TableName);
        //                    }
        //                }
        //            }
        //        return a;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}
        //public static int deletesW(dInfo info)
        //{
        //    try
        //    {
        //        int a = 0;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //            if (info.TableName != null)
        //            {
        //                if (info.TableName.Length != 0)
        //                {
        //                    //update HLIMSDB.Common.UserInfo set remark='aaaa'
        //                    string Ssql = "delete " + info.TableName;
        //                    if (info.DataValueID > 1)
        //                    {
        //                        Ssql += " where  id=" + info.DataValueID;
        //                        a = SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                    }
        //                }
        //            }
        //        return a;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}

        #endregion





        #region hide

        //public static int hidesH(hideInfo info)
        //{

        //    try
        //    {
        //        int a = 0;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //            if (info.TableName != null)
        //            {
        //                if (info.TableName.Length != 0)
        //                {
        //                    //update HLIMSDB.Common.UserInfo set remark='aaaa'
        //                    string Ssql = "update " + info.TableName + " Set dstate=1";
        //                    if (info.DataValueID > 1)
        //                    {
        //                        Ssql += " where  id=" + info.DataValueID;
        //                        a = HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                        reBase(info.TableName);
        //                    }
        //                }
        //            }
        //        return a;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }

        //}
        //public static int hidesW(hideInfo info)
        //{

        //    try
        //    {
        //        int a = 0;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //            if (info.TableName != null)
        //            {
        //                if (info.TableName.Length != 0)
        //                {
        //                    //update HLIMSDB.Common.UserInfo set remark='aaaa'
        //                    string Ssql = "update " + info.TableName + " Set dstate=1";
        //                    if (info.DataValueID > 1)
        //                    {
        //                        Ssql += " where  id=" + info.DataValueID;
        //                        a = SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                    }
        //                }
        //            }
        //        return a;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }

        //}

        #endregion





        #region insert

        //public static int insertsH(iInfo info)
        //{
        //    try
        //    {
        //        int a = 0;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //        {

        //            if (info.TableName != null)
        //            {


        //                if (info.TableName.Length != 0)
        //                {
        //                    string Cname = string.Empty;
        //                    string CValue = string.Empty;
        //                    //update HLIMSDB.Common.UserInfo set remark='aaaa'
        //                    string Ssql = "insert into " + info.TableName;
        //                    if (info.values != null)
        //                    {
        //                        if (info.values.Count > 0)
        //                        {
        //                            foreach (KeyValuePair<string, object> item in info.values)
        //                            {
        //                                Cname += item.Key + ",";
        //                                CValue += "N'" + item.Value + "',";
        //                            }
        //                            Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                            CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

        //                            Ssql += Cname + " values " + CValue;
        //                            a = HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                            reBase(info.TableName);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (info.listValues != null)
        //                        {
        //                            if (info.listValues.Count > 0)
        //                            {
        //                                foreach (KeyValuePair<string, List<object>> item in info.listValues)
        //                                {
        //                                    Cname += item.Key + ",";
        //                                    if (info.listValues.Count != item.Value.Count)
        //                                    {
        //                                        foreach (string s in item.Value)
        //                                        {
        //                                            CValue += "'" + item.Value + "',";
        //                                        }
        //                                        CValue = "(" + CValue.Substring(0, CValue.Length - 1) + "),";
        //                                    }

        //                                }
        //                                CValue = CValue.Substring(0, CValue.Length - 1);
        //                                Ssql += Cname + " values " + CValue;
        //                                a = HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                                reBase(info.TableName);
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return a;
        //    }

        //    catch (Exception ex)

        //    {
        //        return 0;
        //    }
        //}

        //public static int insertsW(iInfo info)
        //{
        //    try
        //    {
        //        int a = 0;
        //        if (SysLoadInfo.UserAuthens(info.UserToken))
        //        {

        //            if (info.TableName != null)
        //            {


        //                if (info.TableName.Length != 0)
        //                {
        //                    string Cname = string.Empty;
        //                    string CValue = string.Empty;
        //                    //update HLIMSDB.Common.UserInfo set remark='aaaa'
        //                    string Ssql = "insert into " + info.TableName;
        //                    if (info.values != null)
        //                    {
        //                        if (info.values.Count > 0)
        //                        {
        //                            foreach (KeyValuePair<string, object> item in info.values)
        //                            {
        //                                Cname += item.Key + ",";
        //                                CValue += "N'" + item.Value + "',";
        //                            }
        //                            Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                            CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

        //                            Ssql += Cname + " values " + CValue;
        //                            a = SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (info.listValues != null)
        //                        {
        //                            if (info.listValues.Count > 0)
        //                            {
        //                                foreach (KeyValuePair<string, List<object>> item in info.listValues)
        //                                {
        //                                    Cname += item.Key + ",";
        //                                    if (info.listValues.Count != item.Value.Count)
        //                                    {
        //                                        foreach (string s in item.Value)
        //                                        {
        //                                            CValue += "'" + item.Value + "',";
        //                                        }
        //                                        CValue = "(" + CValue.Substring(0, CValue.Length - 1) + "),";
        //                                    }

        //                                }
        //                                CValue = CValue.Substring(0, CValue.Length - 1);
        //                                Ssql += Cname + " values " + CValue;
        //                                a = SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return a;
        //    }

        //    catch (Exception ex)

        //    {
        //        return 0;
        //    }
        //}
        #endregion





        #region saveTable
        //public static bool SaveTablesH(SaveTableInfo saveTable)
        //{
        //    try
        //    {
        //        bool a = false;
        //        int b = 0;
        //        if (SysLoadInfo.UserAuthens(saveTable.UserToken))
        //        {
        //            if (saveTable.DT != null)
        //            {
        //                string Ssql = "";
        //                if (saveTable.DT.Columns.Contains("blendid"))
        //                {
        //                    foreach (DataRow dataRow in saveTable.DT.Rows)
        //                    {
        //                        if (dataRow["blendid"] != null)
        //                        {
        //                            if (dataRow["blendid"].ToString() != "")
        //                            {
        //                                Ssql += $"update {saveTable.TableName} set ";
        //                                string setValue = "";
        //                                for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                                {
        //                                    if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
        //                                    {
        //                                        setValue += saveTable.DT.Columns[ss].ColumnName + "=N'" + dataRow[ss] + "',";
        //                                    }
        //                                    else
        //                                    {
        //                                        setValue += saveTable.DT.Columns[ss].ColumnName + "=null,";
        //                                    }


        //                                }
        //                                Ssql += setValue.Substring(0, setValue.Length - 1);
        //                                Ssql += $" where blendid={dataRow["blendid"]};\r\n";
        //                                //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
        //                            }
        //                            else
        //                            {
        //                                Ssql += "insert into " + saveTable.TableName;
        //                                string Cname = string.Empty;
        //                                string CValue = string.Empty;
        //                                for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                                {
        //                                    Cname += saveTable.DT.Columns[ss].ColumnName + ",";
        //                                    if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
        //                                    {
        //                                        CValue += "N'" + dataRow[ss] + "',";
        //                                    }
        //                                    else
        //                                    {
        //                                        CValue += "null,";
        //                                    }
        //                                }
        //                                Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                                CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

        //                                Ssql += Cname + " values " + CValue + ";\r\n";
        //                                //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Ssql += "insert into " + saveTable.TableName;
        //                            string Cname = string.Empty;
        //                            string CValue = string.Empty;
        //                            for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                            {
        //                                Cname += saveTable.DT.Columns[ss].ColumnName + ",";
        //                                CValue += "N'" + dataRow[ss] + "',";
        //                            }
        //                            Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                            CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";
        //                            Ssql += Cname + " values " + CValue + ";\r\n";

        //                        }
        //                    }
        //                    b += HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                    reBase(saveTable.TableName);
        //                }
        //                else
        //                {

        //                    foreach (DataRow dataRow in saveTable.DT.Rows)
        //                    {
        //                        if (dataRow["id"] != null)
        //                        {
        //                            if (dataRow["id"].ToString() != "")
        //                            {
        //                                Ssql += $"update {saveTable.TableName} set ";
        //                                string setValue = "";
        //                                for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                                {
        //                                    if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
        //                                    {
        //                                        setValue += saveTable.DT.Columns[ss].ColumnName + "=N'" + dataRow[ss] + "',";
        //                                    }
        //                                    else
        //                                    {
        //                                        setValue += saveTable.DT.Columns[ss].ColumnName + "=null,";
        //                                    }


        //                                }
        //                                Ssql += setValue.Substring(0, setValue.Length - 1);
        //                                Ssql += $" where id={dataRow["id"]};\r\n";
        //                                //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
        //                            }
        //                            else
        //                            {
        //                                Ssql += "insert into " + saveTable.TableName;
        //                                string Cname = string.Empty;
        //                                string CValue = string.Empty;
        //                                for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                                {
        //                                    Cname += saveTable.DT.Columns[ss].ColumnName + ",";
        //                                    if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
        //                                    {
        //                                        CValue += "N'" + dataRow[ss] + "',";
        //                                    }
        //                                    else
        //                                    {
        //                                        CValue += "null,";
        //                                    }
        //                                }
        //                                Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                                CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

        //                                Ssql += Cname + " values " + CValue + ";\r\n";
        //                                //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Ssql += "insert into " + saveTable.TableName;
        //                            string Cname = string.Empty;
        //                            string CValue = string.Empty;
        //                            for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                            {
        //                                Cname += saveTable.DT.Columns[ss].ColumnName + ",";
        //                                CValue += "N'" + dataRow[ss] + "',";
        //                            }
        //                            Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                            CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";
        //                            Ssql += Cname + " values " + CValue + ";\r\n";

        //                        }
        //                    }
        //                    b += HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                    reBase(saveTable.TableName);
        //                }


        //            }
        //        }
        //        if (b != 0)
        //            a = true;
        //        return a;
        //    }

        //    catch (Exception ex)

        //    {
        //        return false;
        //    }

        //}
        //public static bool SaveTablesH(string userName, string userToken, string TableName, DataTable data)
        //{
        //    try
        //    {
        //        bool a = false;
        //        int b = 0;
        //        if (SysLoadInfo.UserAuthens(userToken))
        //        {

        //            foreach (DataRow dataRow in data.Rows)
        //            {
        //                if (dataRow["id"] != null)
        //                {
        //                    if (dataRow["id"].ToString() != "")
        //                    {
        //                        string sqqq = $"update {TableName} set ";
        //                        string setValue = "";
        //                        for (int ss = 1; ss < data.Columns.Count; ss++)
        //                        {
        //                            setValue += data.Columns[ss].ColumnName + "=N'" + dataRow[ss] + "',";
        //                        }
        //                        sqqq += setValue.Substring(0, setValue.Length - 1);
        //                        sqqq += $" where id={dataRow["id"]}";
        //                        b += HLDBSqlHelper.ExecuteNonQuery(sqqq);
        //                        reBase(TableName);
        //                    }
        //                    else
        //                    {
        //                        string Ssql = "insert into " + TableName;
        //                        string Cname = string.Empty;
        //                        string CValue = string.Empty;
        //                        for (int ss = 1; ss < data.Columns.Count; ss++)
        //                        {
        //                            Cname += data.Columns[ss].ColumnName + ",";
        //                            CValue += "N'" + dataRow[ss] + "',";
        //                        }
        //                        Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                        CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

        //                        Ssql += Cname + " values " + CValue;
        //                        b += HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                        reBase(TableName);
        //                    }
        //                }
        //                else
        //                {
        //                    string Ssql = "insert into " + TableName;
        //                    string Cname = string.Empty;
        //                    string CValue = string.Empty;
        //                    for (int ss = 1; ss < data.Columns.Count; ss++)
        //                    {
        //                        Cname += data.Columns[ss].ColumnName + ",";
        //                        CValue += "N'" + dataRow[ss] + "',";
        //                    }
        //                    Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                    CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

        //                    Ssql += Cname + " values " + CValue;
        //                    b += HLDBSqlHelper.ExecuteNonQuery(Ssql);
        //                    reBase(TableName);
        //                }
        //            }


        //        }
        //        if (b != 0)
        //            a = true;
        //        return a;
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //}


        //public static bool SaveTablesW(SaveTableInfo saveTable)
        //{
        //    try
        //    {
        //        bool a = false;
        //        int b = 0;
        //        if (SysLoadInfo.UserAuthens(saveTable.UserToken))
        //        {
        //            if (saveTable.DT != null)
        //            {
        //                string Ssql = "";
        //                if (saveTable.DT.Columns.Contains("blendid"))
        //                {
        //                    foreach (DataRow dataRow in saveTable.DT.Rows)
        //                    {
        //                        if (dataRow["blendid"] != null)
        //                        {
        //                            if (dataRow["blendid"].ToString() != "")
        //                            {
        //                                Ssql += $"update {saveTable.TableName} set ";
        //                                string setValue = "";
        //                                for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                                {
        //                                    if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
        //                                    {
        //                                        setValue += saveTable.DT.Columns[ss].ColumnName + "=N'" + dataRow[ss] + "',";
        //                                    }
        //                                    else
        //                                    {
        //                                        setValue += saveTable.DT.Columns[ss].ColumnName + "=null,";
        //                                    }


        //                                }
        //                                Ssql += setValue.Substring(0, setValue.Length - 1);
        //                                Ssql += $" where blendid={dataRow["blendid"]};\r\n";
        //                                //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
        //                            }
        //                            else
        //                            {
        //                                Ssql += "insert into " + saveTable.TableName;
        //                                string Cname = string.Empty;
        //                                string CValue = string.Empty;
        //                                for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                                {
        //                                    Cname += saveTable.DT.Columns[ss].ColumnName + ",";
        //                                    if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
        //                                    {
        //                                        CValue += "N'" + dataRow[ss] + "',";
        //                                    }
        //                                    else
        //                                    {
        //                                        CValue += "null,";
        //                                    }
        //                                }
        //                                Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                                CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

        //                                Ssql += Cname + " values " + CValue + ";\r\n";
        //                                //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Ssql += "insert into " + saveTable.TableName;
        //                            string Cname = string.Empty;
        //                            string CValue = string.Empty;
        //                            for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                            {
        //                                Cname += saveTable.DT.Columns[ss].ColumnName + ",";
        //                                CValue += "N'" + dataRow[ss] + "',";
        //                            }
        //                            Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                            CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";
        //                            Ssql += Cname + " values " + CValue + ";\r\n";

        //                        }
        //                    }
        //                    b += SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                }
        //                else
        //                {

        //                    foreach (DataRow dataRow in saveTable.DT.Rows)
        //                    {
        //                        if (dataRow["id"] != null)
        //                        {
        //                            if (dataRow["id"].ToString() != "")
        //                            {
        //                                Ssql += $"update {saveTable.TableName} set ";
        //                                string setValue = "";
        //                                for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                                {
        //                                    if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
        //                                    {
        //                                        setValue += saveTable.DT.Columns[ss].ColumnName + "=N'" + dataRow[ss] + "',";
        //                                    }
        //                                    else
        //                                    {
        //                                        setValue += saveTable.DT.Columns[ss].ColumnName + "=null,";
        //                                    }


        //                                }
        //                                Ssql += setValue.Substring(0, setValue.Length - 1);
        //                                Ssql += $" where id={dataRow["id"]};\r\n";
        //                                //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
        //                            }
        //                            else
        //                            {
        //                                Ssql += "insert into " + saveTable.TableName;
        //                                string Cname = string.Empty;
        //                                string CValue = string.Empty;
        //                                for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                                {
        //                                    Cname += saveTable.DT.Columns[ss].ColumnName + ",";
        //                                    if (dataRow[ss] != DBNull.Value && dataRow[ss].ToString() != "")
        //                                    {
        //                                        CValue += "N'" + dataRow[ss] + "',";
        //                                    }
        //                                    else
        //                                    {
        //                                        CValue += "null,";
        //                                    }
        //                                }
        //                                Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                                CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

        //                                Ssql += Cname + " values " + CValue + ";\r\n";
        //                                //b += HLDBSqlHelper.ExecuteNonQuery( Ssql);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Ssql += "insert into " + saveTable.TableName;
        //                            string Cname = string.Empty;
        //                            string CValue = string.Empty;
        //                            for (int ss = 1; ss < saveTable.DT.Columns.Count; ss++)
        //                            {
        //                                Cname += saveTable.DT.Columns[ss].ColumnName + ",";
        //                                CValue += "N'" + dataRow[ss] + "',";
        //                            }
        //                            Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                            CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";
        //                            Ssql += Cname + " values " + CValue + ";\r\n";

        //                        }
        //                    }
        //                    b += SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                }


        //            }
        //        }
        //        if (b != 0)
        //            a = true;
        //        return a;
        //    }

        //    catch (Exception ex)

        //    {
        //        return false;
        //    }

        //}
        //public static bool SaveTablesW(string userName, string userToken, string TableName, DataTable data)
        //{
        //    try
        //    {
        //        bool a = false;
        //        int b = 0;
        //        if (SysLoadInfo.UserAuthens(userToken))
        //        {

        //            foreach (DataRow dataRow in data.Rows)
        //            {
        //                if (dataRow["id"] != null)
        //                {
        //                    if (dataRow["id"].ToString() != "")
        //                    {
        //                        string sqqq = $"update {TableName} set ";
        //                        string setValue = "";
        //                        for (int ss = 1; ss < data.Columns.Count; ss++)
        //                        {
        //                            setValue += data.Columns[ss].ColumnName + "=N'" + dataRow[ss] + "',";
        //                        }
        //                        sqqq += setValue.Substring(0, setValue.Length - 1);
        //                        sqqq += $" where id={dataRow["id"]}";
        //                        b += SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, sqqq);
        //                    }
        //                    else
        //                    {
        //                        string Ssql = "insert into " + TableName;
        //                        string Cname = string.Empty;
        //                        string CValue = string.Empty;
        //                        for (int ss = 1; ss < data.Columns.Count; ss++)
        //                        {
        //                            Cname += data.Columns[ss].ColumnName + ",";
        //                            CValue += "N'" + dataRow[ss] + "',";
        //                        }
        //                        Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                        CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

        //                        Ssql += Cname + " values " + CValue;
        //                        b += SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                    }
        //                }
        //                else
        //                {
        //                    string Ssql = "insert into " + TableName;
        //                    string Cname = string.Empty;
        //                    string CValue = string.Empty;
        //                    for (int ss = 1; ss < data.Columns.Count; ss++)
        //                    {
        //                        Cname += data.Columns[ss].ColumnName + ",";
        //                        CValue += "N'" + dataRow[ss] + "',";
        //                    }
        //                    Cname = "(" + Cname.Substring(0, Cname.Length - 1) + ")";
        //                    CValue = "(" + CValue.Substring(0, CValue.Length - 1) + ")";

        //                    Ssql += Cname + " values " + CValue;
        //                    b += SqlHelper.ExecuteNonQuery(CommonData.sqlconnW, CommandType.Text, Ssql);
        //                }
        //            }


        //        }
        //        if (b != 0)
        //            a = true;
        //        return a;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}


        #endregion
    }
}
